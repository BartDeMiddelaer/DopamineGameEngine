using Dopamine.Core.Interfaces.EngineInterfaces;
using SFML.System;
using SFML.Window;

namespace Dopamine.Core.Services.EngineServices
{
    public class EngineFunctionalitysService : IEngineFunctionalitys
    {
        private int mousposX = 1;
        private int mousposY = 1;

        private readonly IEngineConfiguration _configuration;
        private readonly IWindowStatus _windowStatus;

        public EngineFunctionalitysService(IEngineConfiguration propertys, IWindowStatus windowStatus)
        {
            _configuration = propertys;
            _windowStatus = windowStatus;
        }

        // SFML Based Fuctions

        public float SFML_GetAngel(Vector2f from, Vector2f to)
        {
            float xDiff = from.X - to.X;
            float yDiff = from.Y - to.Y;
            return (float)(Math.Atan2(-yDiff, -xDiff) * (180 / Math.PI));
        }
        public void SFML_MousePositionEvent(object? sender, MouseMoveEventArgs e)
        {
            mousposX = e.X;
            mousposY = e.Y;
        }
        public Vector2f SFML_GetMousePositionCartesianCenterPointVariabel(Vector2f centerCartesian)
        {
            var wSize = _windowStatus.RenderWindow?.Size ?? throw new ArgumentException("RenderWindow is not creatde");

            double xProcent = mousposX * 100 / wSize.X;
            double xPixel = Math.Round(_configuration.WindowWidth * xProcent / 100, 0);

            double yProcent = mousposY * 100 / wSize.Y;
            double yPixel = Math.Round(_configuration.WindowHeight * yProcent / 100, 0);

            return new Vector2f
            {
                X = (int)xPixel - centerCartesian.X,
                Y = (int)yPixel - centerCartesian.Y
            };
        }
        public Vector2f SFML_GetMousePositionCartesian()
        {
            // https://en.wikipedia.org/wiki/Cartesian_coordinate_system
            var wSize = _windowStatus.RenderWindow?.Size ?? throw new ArgumentException("RenderWindow is not creatde");

            double xProcent = mousposX * 100 / wSize.X;
            double xPixel = Math.Round(_configuration.WindowWidth * xProcent / 100, 0);

            double yProcent = mousposY * 100 / wSize.Y;
            double yPixel = Math.Round(_configuration.WindowHeight * yProcent / 100, 0);

            return new Vector2f
            {
                X = (int)xPixel - (_configuration.WindowWidth / 2),
                Y = (int)yPixel - (_configuration.WindowHeight / 2),
            };
        }
        public Vector2f SFML_GetMousePosition()
        {
            var wSize = _windowStatus.RenderWindow?.Size ?? throw new ArgumentException("RenderWindow is not creatde");

            double xProcent = mousposX * 100 / wSize.X;
            double xPixel = Math.Round(_configuration.WindowWidth * xProcent / 100, 0);

            double yProcent = mousposY * 100 / wSize.Y;
            double yPixel = Math.Round(_configuration.WindowHeight * yProcent / 100, 0);

            return new Vector2f
            {
                X = (int)xPixel,
                Y = (int)yPixel,
            };
        }
        public void SFML_GetMousePositionAbsolute(out int mX, out int mY)
        {
            mY = mousposY;
            mX = mousposX;
        }
        public int SFML_IX(int x, int y) => x + y * _configuration.WindowWidth;
        public Vector2f SFML_GetOffset(Vector2f value, float length, float angle)
        {
            value.X = (float)(value.X + Math.Cos(Math.PI / 180.0 * angle) * length);
            value.Y = (float)(value.Y + Math.Sin(Math.PI / 180.0 * angle) * length);

            return value;
        }
        public float SFML_AddValuePerSec(float increment)
        {
            float speed = increment / (float)_windowStatus.GetFps();
            return float.IsInfinity(speed) ? 0 : speed;
        }

        // Good for alle frameworks
        public int D1ArryIndexWrapper(int index, int size) => index % size;     

        public string FindPathFileNameInDopamineGameFiles(string fileName, string inFolderPath) 
            => FindPathFileNameInDomain(fileName,inFolderPath, "Dopamine.GameFiles");

        public string FindPathFileNameInDopamineCore(string fileName, string inFolderPath)
            => FindPathFileNameInDomain(fileName, inFolderPath, "Dopamine.Core");

        public string FindPathFileNameInDopamineBenchmarkDotNet(string fileName, string inFolderPath) 
            => FindPathFileNameInDomain(fileName, inFolderPath, "Dopamine.BenchmarkDotNet");
        
        public string FindPathFileNameInDopamineBatchRenderer(string fileName, string inFolderPath) 
            => FindPathFileNameInDomain(fileName, inFolderPath, "Dopamine.BatchRenderer");
        
        private string FindPathFileNameInDomain(string fileName, string inFolderPath, string domain)
        {
            bool wordIsFound = false;

            string path = "";
            var fileRootToSplit = Directory.GetCurrentDirectory();
            var splitFileRoot = fileRootToSplit.Replace("\\", "/").Split('/');

            splitFileRoot.ToList().ForEach(s =>
            {
                if (s == "Dopamine.Project") wordIsFound = true;
                if (!wordIsFound) path += s + "/";
            });

            path += $"Dopamine.Project/{domain}/{inFolderPath}/{fileName}";

            return path;
        }

    }
}
