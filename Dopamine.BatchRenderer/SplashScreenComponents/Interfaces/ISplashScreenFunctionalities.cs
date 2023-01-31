using System.Drawing.Drawing2D;

namespace Dopamine.BatchRenderer.SplashScreenComponents.Interfaces
{
    public interface ISplashScreenFunctionalities
    {
        public GraphicsPath RoundedRect(Rectangle bounds, int radius);
        public void DrawSolidRoundedRect(PaintEventArgs e, Point position, int width, int height, int cornerRadius, Color color);
        public void DrawStrokedRoundedRect(PaintEventArgs e, Point position, int strokeSize, int width, int height, int cornerRadius, Color color);
        public void DrawStrokedCircle(PaintEventArgs e, Point position, int strokeSize, int diameter, Color color);
        public void DrawString(PaintEventArgs e, Point position, string txt, Color color, Font font);
        public void DrawPath(PaintEventArgs e, GraphicsPath path, int strokeSize, Color color);
        public List<string> GetProjectFiles();
        public List<string> GetProjectConfigFiles();
        public List<string> GetAllCategories();
        public List<string> GetAllProjectsOnCategoriesName(string categorie);
    }
}
