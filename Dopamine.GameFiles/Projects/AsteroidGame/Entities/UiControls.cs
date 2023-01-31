using Dopamine.Core.Interfaces.EngineInterfaces;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dopamine.GameFiles.Projects.AsteroidGame.Entities
{
    public class UiControls
    {
        private readonly IEngineConfiguration _configuration;
        private readonly Clock clock = new();

        private string message = string.Empty;
        public string StatusInfo { get; set; } = "";
        public int Score { get; set; } = 0;
        public int MessageShowTimeInSec { get; set; } = 1;

        private const int textSizeStatusInfo = 15;
        private const int textMessage = 15;
        private const int textScore = 15;


        private Text txtStatusInfo = new Text
        {
            Font = new Font("C:/Windows/Fonts/arial.ttf"),
            CharacterSize = textSizeStatusInfo,
            FillColor = Color.White
        };
        private Text txtMessage = new Text
        {
            Font = new Font("C:/Windows/Fonts/arial.ttf"),
            CharacterSize = textMessage,
            FillColor = Color.White
        };
        private Text txtScore = new Text
        {
            Font = new Font("C:/Windows/Fonts/arial.ttf"),
            CharacterSize = textScore,
            FillColor = Color.White
        };

        public UiControls(IEngineConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Draw(RenderWindow window)
        {
            DrawStatus(window);
            DrawScoreBackground(window);
            DrawScore(window);
            
            DrawMessageBackground(window);
            DrawMessage(window);
        }

        private void DrawStatus(RenderWindow window)
        {
            // Background
            RectangleShape infoBar = new RectangleShape();

            var textHeight = txtStatusInfo.GetLocalBounds().Height;
            var textTopSpeasing = txtStatusInfo.GetLocalBounds().Top;
            var textWidth = txtStatusInfo.GetLocalBounds().Width;

            int offset = 5;
            int barHight = (int)textHeight + (int)textTopSpeasing * 2;

            infoBar.Size = new(_configuration.WindowWidth - (offset * 2), barHight);
            infoBar.Position = new(offset, (_configuration.WindowHeight - barHight) - offset);
            infoBar.FillColor = new Color(0, 0, 0, 150);
            infoBar.OutlineColor = Color.White;
            infoBar.OutlineThickness = 1;

            window.Draw(infoBar);

            // Text
            txtStatusInfo.DisplayedString = StatusInfo;

            //txtStatusInfo.Position =
            //    new(_configuration.WindowWidth / 2 - textWidth / 2,
            //         _configuration.WindowHeight - (offset + barHight));

            txtStatusInfo.Position =
                new(20,_configuration.WindowHeight - (offset + barHight));

            window.Draw(txtStatusInfo);
        }

        private void DrawMessageBackground(RenderWindow window)
        {
            var textWidth = txtMessage.GetLocalBounds().Width;
            var textHeight = txtMessage.GetLocalBounds().Height;
            var textTopSpeasing = txtMessage.GetLocalBounds().Top;

            // Dont Draw if Message string is empty
            if (message != string.Empty)
            {
                RectangleShape messageBar = new RectangleShape();
                int barHight = (int)textHeight + (int)textTopSpeasing*2;

                messageBar.Position = new(( _configuration.WindowWidth / 2 - textWidth / 2) - 20,
                                            _configuration.WindowHeight / 2 - (textHeight / 2));

                messageBar.Size = new(textWidth + 40, barHight);
                messageBar.FillColor = new Color(0, 0, 0, 150);
                messageBar.OutlineColor = Color.White;
                messageBar.OutlineThickness = 1;

                window.Draw(messageBar);
            }
        }
        private void DrawMessage(RenderWindow window)
        {
            if (message != string.Empty)
            {
                var textWidth = txtMessage.GetLocalBounds().Width;
                var textHeight = txtMessage.GetLocalBounds().Height;

                txtMessage.DisplayedString = message;

                txtMessage.Position =
                    new(_configuration.WindowWidth / 2 - textWidth / 2,
                    _configuration.WindowHeight / 2 - (textHeight/2) );

                window.Draw(txtMessage);
            }

            if (clock.ElapsedTime.AsSeconds() > MessageShowTimeInSec) message = string.Empty;
        }
        public void ShowMessage(string mesage)
        {
            clock.Restart();
            message = mesage;
        }
        public void ShowMessage(string mesage, uint fontSize)
        {
            txtMessage.CharacterSize = fontSize;
            clock.Restart();
            message = mesage;
        }

        private void DrawScoreBackground(RenderWindow window)
        {
            var textWidth = txtScore.GetLocalBounds().Width;
            var textHeight = txtScore.GetLocalBounds().Height;
            var textTopSpeasing = txtScore.GetLocalBounds().Top;
       
            RectangleShape messageBar = new RectangleShape();
            int barHight = (int)textHeight + (int)textTopSpeasing * 2;

            messageBar.Position = new((_configuration.WindowWidth / 2 - textWidth / 2) - 20, 40);
            messageBar.Size = new(textWidth + 40, barHight);
            messageBar.FillColor = new Color(0, 0, 0, 150);
            messageBar.OutlineColor = Color.White;
            messageBar.OutlineThickness = 1;

            window.Draw(messageBar);
            
        }
        private void DrawScore(RenderWindow window)
        {
            var textWidth = txtScore.GetLocalBounds().Width;
            var textHeight = txtScore.GetLocalBounds().Height;

            txtScore.DisplayedString = "Score: " + Score.ToString();

            txtScore.Position =
                new(_configuration.WindowWidth / 2 - textWidth / 2,
                40);

            window.Draw(txtScore);        
        }
    }
}
