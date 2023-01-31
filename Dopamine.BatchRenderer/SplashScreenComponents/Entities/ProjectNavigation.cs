using Dopamine.BatchRenderer.SplashScreenComponents.Interfaces;

namespace Dopamine.BatchRenderer.SplashScreenComponents.Entities
{
    public class ProjectNavigation
    {
        private readonly ISplashScreenFunctionalities _splashScreenFunctionalities;
        public string GameFile { get; set; } = string.Empty;
        public FlowLayoutPanel ProjectFlowLayoutPanel { get; set; } = new();
        public ComboBox CategoryComboBox { get; set; } = new();

        public ProjectNavigation(ISplashScreenFunctionalities splashScreenFunctionalities)
        {
            _splashScreenFunctionalities = splashScreenFunctionalities;

            // FlowPanal settings
            CreateProjectFlowLayoutPanel();        
            CreateCategoryComboBox();
            FillCategoryComboBox();
            PrefillProjectFlowLayoutPanel();
        }

        private void CreateProjectFlowLayoutPanel()
        {
            ProjectFlowLayoutPanel.Location = new Point(245, 175);
            ProjectFlowLayoutPanel.Size = new Size(510, 160);
            ProjectFlowLayoutPanel.BackColor = Color.White;
            ProjectFlowLayoutPanel.BorderStyle = BorderStyle.FixedSingle;
            ProjectFlowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
            ProjectFlowLayoutPanel.AutoScroll = true;
        }
        private void FillProjectFlowLayoutPanel(string catogorieName)
        {
            var projectsOnCatogoryName = 
                _splashScreenFunctionalities.GetAllProjectsOnCategoriesName(catogorieName);

            // Fill the FlowLayoutPanel.controler with the buttons
            projectsOnCatogoryName.ForEach(gamefile =>
            {
                // Button Settings
                Button button = new();
                button.Text = gamefile;
                button.BackColor = Color.LightGray;
                button.Scale(new SizeF(2f, 1.36f));

                //click action fills GameFile whit the game you want to pass it to the splachscreen
                button.Click += (object? sender, EventArgs e) => GameFile = gamefile;

                // Add the button
                ProjectFlowLayoutPanel.Controls.Add(button);
            });
        }

        private void CreateCategoryComboBox()
        {
            CategoryComboBox.Location = new Point(245, 145);
            CategoryComboBox.TabIndex = 0;
            CategoryComboBox.BackColor = Color.LightGray;
        }
        private void FillCategoryComboBox()
        {
            var defaultSelectionName = "Alle Files";

            _splashScreenFunctionalities
                .GetAllCategories()
                .ForEach(catogorie => CategoryComboBox.Items.Add(catogorie));

            CategoryComboBox.Sorted = true;
            CategoryComboBox.BackColor = Color.White;
            CategoryComboBox.Size = new Size(200,30);
            CategoryComboBox.Items.Add(defaultSelectionName);
            CategoryComboBox.SelectedItem = CategoryComboBox.Items[0];

            CategoryComboBox.SelectedIndexChanged += (object? sender, EventArgs e) =>
            {
                ProjectFlowLayoutPanel.Controls.Clear();
                FillProjectFlowLayoutPanel(CategoryComboBox.SelectedItem.ToString() ?? "");
                if(CategoryComboBox.SelectedItem.ToString() == defaultSelectionName) PrefillProjectFlowLayoutPanel();
            };          
        }

        private void PrefillProjectFlowLayoutPanel()
        {
            _splashScreenFunctionalities.GetProjectFiles().ForEach(gamefile =>
            {
                // Button Settings
                Button button = new();
                button.Text = gamefile;
                button.BackColor = Color.LightGray;
                button.Scale(new SizeF(2f, 1.36f));

                //click action fills GameFile whit the game you want to pass it to the splachscreen
                button.Click += (object? sender, EventArgs e) => GameFile = gamefile;

                // Add the button
                ProjectFlowLayoutPanel.Controls.Add(button);
            });
        }
    }
}
