using Dopamine.Core.Interfaces.EngineInterfaces;
using SFML.Graphics;
using SFML.System;

namespace Dopamine.GameFiles.Projects.MetaBallsShader.Entities
{

    public class MessageControls
    {
        private IEngineConfiguration _configuration;

        Clock clock = new Clock();
        private Text txtMessage = new Text
        {
            Font = new Font("C:/Windows/Fonts/arial.ttf"),
            CharacterSize = 30,
            FillColor = Color.White
        };
        private string message = "GLSL Meta Balls !";

        public MessageControls(IEngineConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ShowMessage(string mesage)
        {
            clock.Restart();
            this.message = mesage;
        }
        public void Draw(RenderWindow window)
        {
            DrawMessage(window);
        }
        private void DrawMessage(RenderWindow window)
        {
            DrawMessageBackground(window);

            if (message != string.Empty)
            {
                var textWidth = txtMessage.GetLocalBounds().Width;
                txtMessage.DisplayedString = message;

                txtMessage.Position =
                    new(_configuration.WindowWidth / 2 - textWidth / 2, 40);

                window.Draw(txtMessage);
            }

            if (clock.ElapsedTime.AsSeconds() > 3) message = string.Empty;
        }
        private void DrawMessageBackground(RenderWindow window)
        {
            var textWidth = txtMessage.GetLocalBounds().Width;

            // Dont Draw if Message string is empty
            if (message != string.Empty)
            {
                RectangleShape messageBar = new RectangleShape();
                int barHight = 60;

                messageBar.Position = new((_configuration.WindowWidth / 2 - textWidth / 2) - 20, 30);
                messageBar.Size = new(textWidth + 40, barHight);
                messageBar.FillColor = new Color(0, 0, 0, 150);
                messageBar.OutlineColor = Color.White;
                messageBar.OutlineThickness = 1;

                window.Draw(messageBar);
            }
        }
    }
}
