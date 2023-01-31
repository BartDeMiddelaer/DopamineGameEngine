using Dopamine.Core.Interfaces.EngineInterfaces;
using SFML.Graphics;

namespace Dopamine.GameFiles.Projects.MetaBallsShader.Entities
{
    public class ShaderDataControls
    {
        private IEngineConfiguration _configuration;
        public int BallCount { get; set; } = 5; // ball count
        public float ModIntesety { get; set; } = 1.2f; // ball sizeing
        public float HsvMultiplayer { get; set; } = 1.0f; // coloring // 2.0 is nice to
        public float ColorMultiplayer { get; set; } = 0.7f; // brithniss // 6.0 is nice to
        public int MaxVelosety { get; set; } = 3; // cant be lower as -1 max speed of the balls
        public int MaxRadius { get; set; } = 100; // cant be lower as 20 max radius of the balls
        public bool ShowMenu { get; set; } = false;
        public bool ShowOutline { get; set; } = false;

        private Text txtInfoStatus = new Text
        {
            Font = new Font("C:/Windows/Fonts/arial.ttf"),
            CharacterSize = 15,
            FillColor = Color.White
        };
        private Text txtControleInfo = new Text
        {
            Font = new Font("C:/Windows/Fonts/arial.ttf"),
            CharacterSize = 25,
            FillColor = Color.White
        };

        public ShaderDataControls(IEngineConfiguration configuration)
            => _configuration = configuration;         
       
        public void Draw(RenderWindow window)
        {
            DrawMenuBackGround(window);
            DrawStatus(window);
            if (ShowMenu) DrawControleInfo(window);
        }
        private void DrawMenuBackGround(RenderWindow window)
        {
            RectangleShape infoBar = new RectangleShape();

            int offset = 5;
            int infoBarHight = ShowMenu 
                ? _configuration.WindowHeight - (2 * offset)
                : 30;

            infoBar.Size = new(_configuration.WindowWidth - (offset * 2) , infoBarHight);
            infoBar.Position = new(offset, (_configuration.WindowHeight - infoBarHight) - offset);
            infoBar.FillColor = new Color(0, 0, 0, 150);
            infoBar.OutlineColor = Color.White;
            infoBar.OutlineThickness = 1;
            window.Draw(infoBar);    
        }
        private void DrawStatus(RenderWindow window)
        {
            var textWidth = txtInfoStatus.GetLocalBounds().Width;

            var showOutlineStatus = ShowOutline ? "ON" : "OFF";

            txtInfoStatus.DisplayedString =
                $"Press F1 for Info\t " +
                $"ShowOutline: {showOutlineStatus}\t " +
                $"BallCount: {BallCount}\t " +
                $"ModIntesety: {ModIntesety}\t " +
                $"HsvMultiplayer: {HsvMultiplayer}\t " +
                $"ColorMultiplayer: {ColorMultiplayer}\t" +
                $"MaxVelosety: {MaxVelosety}\t " +
                $"MaxRadius: {MaxRadius}\t ";

            txtInfoStatus.Position = 
                new( _configuration.WindowWidth/2 - textWidth/2,
                     _configuration.WindowHeight - 30);

            window.Draw(txtInfoStatus);
        }
        private void DrawControleInfo(RenderWindow window)
        {
            var textWidth = txtControleInfo.GetLocalBounds().Width;
            var textHight = txtControleInfo.GetLocalBounds().Height;

            txtControleInfo.DisplayedString =
                "F1 = Show Controls\n" +
                "F2 = Flip Outline On\\Off\n" +
                "F3 = MaxRadius --\n" +
                "F4 = MaxRadius ++\n" +
                "Arrow Down = Remove Meta Ball\n" +
                "Arrow Up = Add Meta Ball\n" +
                "Arrow Left = ModIntesety --\n" +
                "Arrow Right = ModIntesety ++\n" +
                "S = HsvMultiplayer --\n" +
                "Z = HsvMultiplayer ++\n" +
                "Q = ColorMultiplayer --\n" +
                "D = ColorMultiplayer ++\n" +
                "A = MaxVelosety On New Balls --\n" +
                "E = MaxVelosety On New Balls ++\n";

            txtControleInfo.Position =
                new( _configuration.WindowWidth / 2 - textWidth / 2,
                     _configuration.WindowHeight / 2  - textHight / 2);

            window.Draw(txtControleInfo);
        }
       
    }
}
