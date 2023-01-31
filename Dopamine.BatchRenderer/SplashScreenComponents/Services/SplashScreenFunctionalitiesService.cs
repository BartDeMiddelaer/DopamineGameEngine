using Dopamine.BatchRenderer.SplashScreenComponents.Interfaces;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace Dopamine.BatchRenderer.SplashScreenComponents.Services
{
    public class SplashScreenFunctionalitiesService : ISplashScreenFunctionalities
    {
        public GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new (diameter, diameter);
            Rectangle arc = new (bounds.Location, size);
            GraphicsPath path = new();

            if (radius == 0) {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
        public void DrawSolidRoundedRect(PaintEventArgs e, Point position, int width, int height, int cornerRadius, Color color)
        {
            var path = RoundedRect(new Rectangle(position.X, position.Y, width, height), cornerRadius);
            e.Graphics.FillPath(new SolidBrush(color), path);
        }
        public void DrawStrokedRoundedRect(PaintEventArgs e, Point position, int strokeSize, int width, int height, int cornerRadius, Color color)
        {
            var strokePath = RoundedRect(new Rectangle(position.X, position.Y, width, height), cornerRadius);
            e.Graphics.DrawPath(new Pen(new SolidBrush(color), strokeSize), strokePath);
        } 
        public void DrawStrokedCircle(PaintEventArgs e, Point position, int strokeSize, int diameter, Color color)
        {
            e.Graphics.DrawEllipse(new Pen(color, strokeSize), 
                new Rectangle(position.X, position.Y, diameter, diameter));
        }
        public void DrawString(PaintEventArgs e, Point position, string txt, Color color, Font font)
        {
            e.Graphics.DrawString(txt, font, new SolidBrush(color), position.X, position.Y);
        }
        public void DrawPath(PaintEventArgs e, GraphicsPath path, int strokeSize, Color color)
        {
            e.Graphics.DrawPath(new Pen(color, strokeSize), path);
        }
        public List<string> GetProjectFiles()
        {
            // Get Configuration files form domain
            Assembly domain = Assembly.Load("Dopamine.GameFiles");
            var files = domain.GetTypes().Where(f => f.Name.Contains("Configuration")).ToList();

            // Put the names of the files in the fileNames List - the Configuration pefix
            List<string> fileNames = new();
            files.ForEach(f => fileNames.Add(f.Name.Replace("Configuration", "")));

            // return the names of the files
            fileNames.Sort();
            return fileNames;
        }
        public List<string> GetProjectConfigFiles()
        {
            // Get Configuration files form domain
            Assembly domain = Assembly.Load("Dopamine.GameFiles");
            var files = domain.GetTypes().Where(f => f.Name.Contains("Configuration")).ToList();

            // gets alle thz configfiles names
            List<string> fileNames = new();
            files.ForEach(f => fileNames.Add(f.Name));

            // return the names of the files
            fileNames.Sort();
            return fileNames;
        }
        public List<string> GetAllCategories()
        {
            // loads domain in
            Assembly domain = Assembly.Load("Dopamine.GameFiles");

            // get alle the values out of the SplashScreenCategorie <- the prop haves to be static sorry :)
            List<string> configFiles = GetProjectConfigFiles();
            List<string> categories = new();
            configFiles.ForEach(cf =>
            {
                var files = domain
                    .GetTypes()
                    .Where(f => f.Name.Contains(cf))
                    .ToList().FirstOrDefault();

                PropertyInfo prop =
                    files?.GetProperty("SplashScreenCategorie") 
                    ?? throw new ArgumentException("prop in GetAllCategories is null");

                var values = prop.GetValue(files);

                foreach (var item in values as string[] ?? Array.Empty<string>())
                    if (!categories.Contains(item)) categories.Add(item);                             
            });
            categories.Sort();
            return categories;
        }
        public List<string> GetAllProjectsOnCategoriesName(string categorie)
        {
            // loads domain in
            Assembly domain = Assembly.Load("Dopamine.GameFiles");

            // finds all projects On CategoriesName
            List<string> configFiles = GetProjectConfigFiles();
            List<string> projectsOnCategoriesName = new();
            configFiles.ForEach(cf =>
            {
                var files = domain
                    .GetTypes()
                    .Where(f => f.Name.Contains(cf))
                    .ToList().FirstOrDefault();

                PropertyInfo prop = 
                    files?.GetProperty("SplashScreenCategorie") 
                    ?? throw new ArgumentException("prop in GetAllProjectsOnCategoriesName is null");

                var values = prop.GetValue(files);

                foreach (var item in values as string[] ?? Array.Empty<string>())
                if (item == categorie) projectsOnCategoriesName.Add(cf.Replace("Configuration", ""));
            });

            projectsOnCategoriesName.Sort();
            return projectsOnCategoriesName;
        }
    }
}
