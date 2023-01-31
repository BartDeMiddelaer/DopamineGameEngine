using Autofac;
using Dopamine.BatchRenderer.SplashScreenComponents.Entities;
using Dopamine.BatchRenderer.SplashScreenComponents.Interfaces;
using Dopamine.Core.Interfaces.EngineInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dopamine.BatchRenderer.SplashScreenComponents
{
    public class LoadScreen : Form
    {
        readonly private ISplashScreenFunctionalities _splashScreenFunctionalities;
        readonly private IEngineFunctionalitys _functionalitys;
        readonly private IEngineConfiguration _configuration;

        public LoadScreen(ISplashScreenFunctionalities splashScreenFunctionalities, IEngineFunctionalitys functionalitys
            , IEngineConfiguration configuration)
        {
            _splashScreenFunctionalities = splashScreenFunctionalities;
            SetLoadScreenSettings();
        }
        private void Draw(object? sender, PaintEventArgs e)
        {
            DrawBackground(e);
            DrawText(e);
        }
        private void DrawText(PaintEventArgs e)
        {
            _splashScreenFunctionalities.DrawString(e, new Point(0, 25), "Loading Assets", Color.White, new Font("Arial", 50));
        }
        private void SetLoadScreenSettings()
        {
            ClientSize = new(500, 150); // Set size of splachscreen    
            Opacity = 0.9; // Set opacity of the screen          
            FormBorderStyle = FormBorderStyle.None; // Set border of screen          
            StartPosition = FormStartPosition.CenterScreen; // Set position of screen
            TransparencyKey = Color.Gray; // Set Color on the screen to filter out so you can cee true it
            BackColor = Color.Gray; // BG color
            Paint += Draw;
        }
        private void DrawBackground(PaintEventArgs e)
        {
            _splashScreenFunctionalities.DrawSolidRoundedRect(e, new(0, 20), this.Width, 85, 5, Color.Black);
        }
        public bool IsLoadingAssits(GameLoopLogic? loopLogic)
        {
            var state =  loopLogic?.LoadAssets().Status;
            return state == TaskStatus.RanToCompletion ? false : true;
        }

    }
}
