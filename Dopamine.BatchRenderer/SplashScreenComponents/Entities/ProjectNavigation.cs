using Dopamine.BatchRenderer.SplashScreenComponents.Interfaces;

namespace Dopamine.BatchRenderer.SplashScreenComponents.Entities
{
    public class ProjectNavigation
    {
        private readonly ISplashScreenFunctionalities _splashScreenFunctionalities;
        public string GameFile { get; set; } = string.Empty;
        public FlowLayoutPanel ProjectFlowLayoutPanel { get; set; } = new();
        public ComboBox CategoryComboBox { get; set; } = new();
        private Point location;
        private Size size;

        public ProjectNavigation(ISplashScreenFunctionalities splashScreenFunctionalities, Point location, Size size)
        {
            _splashScreenFunctionalities = splashScreenFunctionalities;
            this.location = location;
            this.size = size;

            // FlowPanal settings
            CreateProjectFlowLayoutPanel();        
            CreateCategoryComboBox();
            FillCategoryComboBox();
            FillProjectFlowLayoutPanel(null);
        }
        private void CreateProjectFlowLayoutPanel()
        {
            ProjectFlowLayoutPanel.Location = location;
            ProjectFlowLayoutPanel.Size = size;
            ProjectFlowLayoutPanel.BackColor = Color.White;
            ProjectFlowLayoutPanel.BorderStyle = BorderStyle.FixedSingle;
            ProjectFlowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            ProjectFlowLayoutPanel.AutoScroll = true;
        }
        private void FillProjectFlowLayoutPanel(string? catogorieName)
        {
            List<string> files = new();

            if (catogorieName != null)
                files = _splashScreenFunctionalities.GetAllProjectsOnCategoriesName(catogorieName);   
            else 
                files = _splashScreenFunctionalities.GetProjectFiles();
            
            // Fill the FlowLayoutPanel.controler with the buttons
            files.ForEach(gamefile =>
            {
                // Button Settings
                Button button = new();
                button.Text = gamefile;
                button.BackColor = Color.LightGray;
                button.Scale(new SizeF(1.80f, 1.6f));

                //click action fills GameFile whit the game you want to pass it to the splachscreen
                button.Click += (object? sender, EventArgs e) => GameFile = gamefile;

                // Add the button
                ProjectFlowLayoutPanel.Controls.Add(button);
            });
        }
        private void CreateCategoryComboBox()
        {
            CategoryComboBox.Location = new Point(location.X,location.Y - 30);
            CategoryComboBox.TabIndex = 0;
            CategoryComboBox.Sorted = true;
            CategoryComboBox.BackColor = Color.White;
        }
        private void FillCategoryComboBox()
        {
            var defaultSelectionName = "Alle Files";

            _splashScreenFunctionalities
                .GetAllCategories()
                .ForEach(catogorie => CategoryComboBox.Items.Add(catogorie));

            CategoryComboBox.Items.Add(defaultSelectionName);
            CategoryComboBox.SelectedItem = CategoryComboBox.Items[0];

            CategoryComboBox.SelectedIndexChanged += (object? sender, EventArgs e) =>
            {
                ProjectFlowLayoutPanel.Controls.Clear();
                FillProjectFlowLayoutPanel(CategoryComboBox.SelectedItem.ToString() ?? "");
                if(CategoryComboBox.SelectedItem.ToString() == defaultSelectionName) FillProjectFlowLayoutPanel(null);
            };          
        }      
    }
}
