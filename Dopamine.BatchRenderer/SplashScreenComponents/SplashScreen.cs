using System.Drawing.Drawing2D;
using Dopamine.BatchRenderer.SplashScreenComponents.Interfaces;
using Dopamine.BatchRenderer.SplashScreenComponents.Entities;
using Dopamine.Core.Interfaces.EngineInterfaces;
using Timer = System.Windows.Forms.Timer;

namespace Dopamine.BatchRenderer.SplashScreenComponents
{
    // https://github.com/BartDeMiddelaer/CsFormsGameEngine

    public class SplashScreen : Form, IDisposable
    {
        public string GameToStart { get; set; } = string.Empty;  // Gamefile name to give to Autofac to start the game
        readonly private PictureBox LoopContainer = new(); // the draw Control
        readonly private Timer tikker = new(); // is to simulate the tickker to reander N amount/sec
        readonly private ISplashScreenFunctionalities _splashScreenFunctionalities; // Dependesie to draw stuff
        readonly private IEngineFunctionalitys _functionalitys; // Dependesie to draw stuff
        private ProjectNavigation projectNavigation; // FlowPanal for the gamefile Projects to pick out off
        private CubeShadre? cubeShader;

        // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        #pragma warning disable CS8618
        public SplashScreen(ISplashScreenFunctionalities splashScreenFunctionalities, IEngineFunctionalitys functionalitys)
        {          
            _splashScreenFunctionalities = splashScreenFunctionalities;
            _functionalitys = functionalitys;

            SetCubeShader();
            SetSplashScreenSettings();
            SetLoopContainer();
            SetProjectNavigation();
            //SetIco();
        }
        #pragma warning restore CS8618

        private void Render(object? sender, PaintEventArgs e)
        {
            // RenderMode
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Draws the splachscreen
            DrawBackground(e);
            DrawFooter(e);
            DrawText(e);
            cubeShader?.Render();

            // get the clicht Gamefile You want in to GameToStart "the click event from buttonFlowPanel"
            GameToStart = projectNavigation.GameFile;

            // Window Close Event if File is asinde
            if (GameToStart != string.Empty)
            {
                // Dont forget to Clear your Controls it was not fun to find this out took me a hole day
                Controls.Clear();
                Close();
            }
        }
        private void DrawText(PaintEventArgs e)
        {
            _splashScreenFunctionalities.DrawString(e, new Point(0, 50), "Project", Color.White, new Font("Arial", 50));
            _splashScreenFunctionalities.DrawString(e, new Point(12, 120), "Bart.D.M", Color.White, new Font("Arial", 10));
            _splashScreenFunctionalities.DrawString(e, new Point(235, 50), "Dopamine", Color.Black, new Font("Arial", 50));
            _splashScreenFunctionalities.DrawString(e, new Point(550, 92), "Game Engine", Color.Red, new Font("Arial", 15));
        }
        private void DrawFooter(PaintEventArgs e)
        {
            // Drawing of wobely lines in window
            for (int y = 0; y < 25; y += 5)
                _splashScreenFunctionalities.DrawPath(e, GenerateLineGrafhics(y, y), y, Color.Gray);
        }
        private GraphicsPath GenerateLineGrafhics(int lineThicknis, int onIndex)
        {
            var lineGrafhics = new GraphicsPath();

            int yPosision = 345;
            int xPosision = 200;

            for (int x = 0; x < this.Width + xPosision; x++)
                lineGrafhics.AddLine(
                    xPosision + x, yPosision  + onIndex + (onIndex * lineThicknis / 8),
                    xPosision + x, yPosision  + onIndex + (onIndex * lineThicknis / 8)
                );

            return lineGrafhics;
        }
        private void DrawBackground(PaintEventArgs e)
        {
            _splashScreenFunctionalities.DrawSolidRoundedRect(e, new Point(232, 0), Width - 232, Height, 5, Color.White);
            _splashScreenFunctionalities.DrawStrokedRoundedRect(e, new Point(2, 20), 5, Width, Height - 40, 5, Color.White);
        }    
        private void SetLoopContainer()
        {
            LoopContainer.Dock = DockStyle.Fill; // Fill the screen window With the LoopContainer "Replasing it"
            LoopContainer.Paint += Render; // Declare the paint Fuction
            Controls.Add(LoopContainer); // Atatch the LoopContainer to the window

            tikker.Tick += (sender, e) => LoopContainer.Refresh(); // Refresh the LoopContainer
            tikker.Interval = 15; // intervals of the Refresh
            tikker.Start(); // starts timer
        }
        private void SetSplashScreenSettings()
        {
            ClientSize = new(860, 450); // Set size of splachscreen    
            Opacity = 0.9; // Set opacity of the screen          
            FormBorderStyle = FormBorderStyle.None; // Set border of screen          
            StartPosition = FormStartPosition.CenterScreen; // Set position of screen
            TransparencyKey = Color.Gray; // Set Color on the screen to filter out so you can cee true it
            BackColor = Color.Gray; // BG color
        }
        private void SetProjectNavigation()
        {
            projectNavigation = new(_splashScreenFunctionalities, new Point(245, 175), new Size(600, 160));
            LoopContainer.Controls.Add(projectNavigation.ProjectFlowLayoutPanel);
            LoopContainer.Controls.Add(projectNavigation.CategoryComboBox);
        }
        private void SetCubeShader()
        {
            // Create a new CubeShadre Form
            cubeShader = new CubeShadre(new Point(730, 20), new Size(120, 120), _functionalitys, Color.White);

            // Determines whether the object is a top-level window.
            // A top-level window is a window that is not contained within another window.
            cubeShader.TopLevel = false;

            // Adds cubeShader Form to the SplashScreen Form
            Controls.Add(cubeShader);

            // Show The Form
            cubeShader.Show();
        }
        private void SetIco()
        {
            var icoPath = _functionalitys.FindPathFileNameInDopamineBatchRenderer("dopamine.ico", "Image");
            Icon = new Icon(icoPath);
            Text = "Project Dopamine";
        }
    }
}
