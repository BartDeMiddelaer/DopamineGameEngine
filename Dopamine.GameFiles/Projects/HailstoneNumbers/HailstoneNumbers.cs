using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.RendererInterfaces;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Dopamine.GameFiles.Projects.HailstoneNumbers.Entities;
using Dopamine.Core.Services.ProjectServices;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using Color = SFML.Graphics.Color;

namespace Dopamine.GameFiles.Projects.HailstoneNumbers
{
    public class HailstoneNumbers : BaseGameFile, IGameFile
    {
        private readonly IRenderer _renderer;
        private readonly IEngineConfiguration _configuration;
        private readonly IEngineFunctionalitys _functionalitys;

        private readonly ThreeXPlusOneGenerator threeXPlusOneGenerator;
        private List<List<int>> threeXPlusOneTrieNumbers = new();
        private int modiloOffset = 150;
        private float amplidude = 1.0f;
        private float maxSegmentLeght = 25.0f;
        private float baseLeghtMultyplayer = 3.5f;
        private bool drawFeafs = true;

        public HailstoneNumbers(IRenderer renderer, IEngineConfiguration configuration, IEngineFunctionalitys functionalitys)
        {
            _functionalitys = functionalitys;
            _renderer = renderer;
            _configuration = configuration;
       
            threeXPlusOneGenerator = new(1000);
        }
        public void EventDeclaration(RenderWindow window)
        {
            window.KeyPressed += KeyPressed;
        }
        public void LoadInProjectAssets()
        {

        }
        public void GameLoop(RenderWindow window)
        {
            threeXPlusOneTrieNumbers.ForEach(b => {
                Color branchColor = new Color(255, 200, 100, 50);
                DrawBranch(window, b, modiloOffset, 90, amplidude, maxSegmentLeght, branchColor);
            });
        }
        private void DrawBranch(RenderWindow window, List<int> branch, int modilo, int directionAngel, float amplidude, float maxSegmentLeght, Color color)
        {
            Vertex[] vertices = new Vertex[branch.Count];

            for (int i = 0; i < branch.Count; i++)
            {
                if (i == 0)
                {
                    vertices[i] = new Vertex(new Vector2f(_configuration.WindowWidth / 2, _configuration.WindowHeight));
                    vertices[i].Color = color;
                }
                else
                {
                    Vector2f priviusPossition = vertices[i - 1].Position;

                    // you cant divida by 0 so 1O is the minimum
                    modilo = modilo < 10 ? 10 : modilo;

                    // Segment Leght
                    float segmentLeght = maxSegmentLeght - (i / 3) < 5
                        ? 5
                        : maxSegmentLeght - (i / 3);

                    // set the first 3 sigments longer
                    float segmentLeghtinput = i < 3 
                        ? segmentLeght * baseLeghtMultyplayer
                        : segmentLeght;

                    Vector2f newPossition = _functionalitys.SFML_GetOffset(priviusPossition, segmentLeghtinput, ((branch[i] % modilo) / amplidude) - directionAngel);

                    vertices[i] = new Vertex(newPossition);
                    vertices[i].Color = color;
                }              
            }

            window.Draw(vertices, PrimitiveType.LineStrip);
            if(drawFeafs) DrawLeafs(window, vertices);
        }
        private void DrawLeafs(RenderWindow window, Vertex[] vertices)
        {
            vertices
                .TakeLast((int)(vertices.Length / 1.3))
                .ToList()
                .ForEach(v => {

                    byte leafAlfha = 34;
                    Color leafColor = new Color(0, 200, 0, 255);

                    Vertex[] leaf = new Vertex[4];

                    leaf[0].Position = v.Position - new Vector2f(-maxSegmentLeght/4, 0);
                    leaf[1].Position = v.Position - new Vector2f(0, -maxSegmentLeght / 4);
                    leaf[2].Position = v.Position - new Vector2f(maxSegmentLeght / 4, 0);
                    leaf[3].Position = v.Position - new Vector2f(0, maxSegmentLeght / 4);

                    leaf[0].Color = leafColor;
                    leaf[0].Color.A = leafAlfha;

                    leaf[1].Color = leafColor;
                    leaf[1].Color.A = leafAlfha;

                    leaf[2].Color = leafColor;
                    leaf[2].Color.A = leafAlfha;

                    leaf[3].Color = leafColor;
                    leaf[3].Color.A = leafAlfha;
                  
                    window.Draw(leaf,PrimitiveType.Quads);
                });         
        }

        public void KeyPressed(object? sender, KeyEventArgs e) {

            if (e.Code == Keyboard.Key.F1)
            {
                threeXPlusOneTrieNumbers = threeXPlusOneGenerator.GenerateMultySet(200);
            }
            if (e.Code == Keyboard.Key.F2)
            {
                drawFeafs = !drawFeafs;
            }
            if (e.Code == Keyboard.Key.Up)
            {
                modiloOffset++;
            }
            if (e.Code == Keyboard.Key.Down)
            {
                modiloOffset--;
            }
            if (e.Code == Keyboard.Key.Left)
            {
                amplidude -= 0.01f;
            }
            if (e.Code == Keyboard.Key.Right)
            {
                amplidude += 0.01f;
            }
            if (e.Code == Keyboard.Key.Q)
            {
                maxSegmentLeght -= 0.1f;
            }
            if (e.Code == Keyboard.Key.D)
            {
                maxSegmentLeght += 0.1f;
            }
            if (e.Code == Keyboard.Key.S)
            {
                baseLeghtMultyplayer -= 0.1f;
            }
            if (e.Code == Keyboard.Key.Z)
            {
                baseLeghtMultyplayer += 0.1f;
            }
        }
    }
}
