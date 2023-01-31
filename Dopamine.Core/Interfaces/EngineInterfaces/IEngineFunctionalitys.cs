using SFML.System;
using SFML.Window;

namespace Dopamine.Core.Interfaces.EngineInterfaces
{
    public interface IEngineFunctionalitys
    {
        public float SFML_GetAngel(Vector2f from, Vector2f to);
        public void SFML_MousePositionEvent(object? sender, MouseMoveEventArgs e);
        public Vector2f SFML_GetMousePositionCartesianCenterPointVariabel(Vector2f centerCartesian);
        public Vector2f SFML_GetMousePositionCartesian();
        public Vector2f SFML_GetMousePosition();
        public void SFML_GetMousePositionAbsolute(out int mX, out int mY);
        public int SFML_IX(int x, int y);
        public Vector2f SFML_GetOffset(Vector2f value, float length, float angle);
        public float SFML_AddValuePerSec(float increment);


        public int D1ArryIndexWrapper(int index, int size);
        public string FindPathFileNameInDopamineGameFiles(string fileName, string inFolderPath);
        public string FindPathFileNameInDopamineCore(string fileName, string inFolderPath);
        public string FindPathFileNameInDopamineBenchmarkDotNet(string fileName, string inFolderPath);
        public string FindPathFileNameInDopamineBatchRenderer(string fileName, string inFolderPath);
    }
}
