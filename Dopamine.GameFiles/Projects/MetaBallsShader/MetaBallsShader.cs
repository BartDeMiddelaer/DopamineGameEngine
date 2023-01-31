using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.Services.ProjectServices;
using Dopamine.GameFiles.Projects.MetaBallsShader.Entities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Color = SFML.Graphics.Color;

namespace Dopamine.GameFiles.Projects.MetaBallsShader
{
    // Shader lessons for Youtube
    // https://www.youtube.com/watch?v=HIvNePu7UEE&list=PL4neAtv21WOmIrTrkNO3xCyrxg4LKkrF7&ab_channel=LewisLepton

    // Info to program from
    // https://github.com/SFML/SFML.Net.git

    public class MetaBallsShader : BaseGameFile, IGameFile
    {
        private readonly IRenderer _renderer;
        private readonly IEngineFunctionalitys _functionalitys;
        private readonly IEngineConfiguration _configuration;

        private readonly ShaderDataControls shaderDataControlls;
        private readonly SoundControls soundControls;
        private readonly MessageControls messageControls;
        private readonly Clock clock = new();

        private Shader mbShader;
        private List<MetaBall> metaBalles;

        // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        #pragma warning disable CS8618
        public MetaBallsShader(IRenderer renderer, IEngineConfiguration configuration, IEngineFunctionalitys functionalitys)
        {
            shaderDataControlls = new ShaderDataControls(configuration);
            soundControls = new SoundControls(functionalitys);
            messageControls = new MessageControls(configuration);

            _functionalitys = functionalitys;
            _renderer = renderer;
            _configuration = configuration;

            SetShaderInfo();
            CreateMetaballs();
        }
        #pragma warning restore CS8618 

        public void EventDeclaration(RenderWindow window)
        {
            window.KeyPressed += KeyPressed;
            window.KeyReleased += KeyReleased;
        }
        public void LoadInProjectAssets()
        {

        }
        public void GameLoop(RenderWindow window)
        {
            soundControls.FideInBackgroundSound();

            metaBalles.ForEach(mb => mb.Update());
            SetShaderUniforms();

            _renderer.Draw(window, mbShader);

            if(shaderDataControlls.ShowOutline)DrawOutlineMetaballs(window);
            shaderDataControlls.Draw(window);
            messageControls.Draw(window);
        }


        public void KeyPressed(object? sender, KeyEventArgs e) 
        {
            // Shaw Menu
            if (e.Code == Keyboard.Key.F1)
                shaderDataControlls.ShowMenu = true;

            if (e.Code == Keyboard.Key.Left)
            {
                shaderDataControlls.ModIntesety -= 0.1f;
                shaderDataControlls.ModIntesety = (float)Math.Round(shaderDataControlls.ModIntesety, 2);
            }
            if (e.Code == Keyboard.Key.Right)
            {
                shaderDataControlls.ModIntesety += 0.1f;
                shaderDataControlls.ModIntesety = (float)Math.Round(shaderDataControlls.ModIntesety, 2);

            }
            if (e.Code == Keyboard.Key.S)
            {
                shaderDataControlls.HsvMultiplayer -= 0.1f;
                shaderDataControlls.HsvMultiplayer = (float)Math.Round(shaderDataControlls.HsvMultiplayer, 2);

            }
            if (e.Code == Keyboard.Key.Z)
            {
                shaderDataControlls.HsvMultiplayer += 0.1f;
                shaderDataControlls.HsvMultiplayer = (float)Math.Round(shaderDataControlls.HsvMultiplayer, 2);
            }
            if (e.Code == Keyboard.Key.Q)
            {
                shaderDataControlls.ColorMultiplayer -= 0.05f;
                shaderDataControlls.ColorMultiplayer = (float)Math.Round(shaderDataControlls.ColorMultiplayer, 2);

            }
            if (e.Code == Keyboard.Key.D)
            {
                shaderDataControlls.ColorMultiplayer += 0.05f;
                shaderDataControlls.ColorMultiplayer = (float)Math.Round(shaderDataControlls.ColorMultiplayer, 2);
            }
            if (e.Code == Keyboard.Key.E)
            {
                shaderDataControlls.MaxVelosety++;
            }
            if (e.Code == Keyboard.Key.A)
            {
                if (shaderDataControlls.MaxVelosety != -1)
                    shaderDataControlls.MaxVelosety--;
            }
            if (e.Code == Keyboard.Key.F3)
            {
                var newRadius = shaderDataControlls.MaxRadius - 5;
                if (newRadius >= 20)
                    shaderDataControlls.MaxRadius = newRadius;
            }
            if (e.Code == Keyboard.Key.F4)
            {
                shaderDataControlls.MaxRadius += 5;
            }

            ShowKeyMessages(e);
        }
        public void KeyReleased(object? sender, KeyEventArgs e) 
        {
            // Hide Menu
            if (e.Code == Keyboard.Key.F1)
                shaderDataControlls.ShowMenu = false;

            // Flip ShowOutline
            if (e.Code == Keyboard.Key.F2)
                shaderDataControlls.ShowOutline = !shaderDataControlls.ShowOutline;

            // Remove ball
            if (e.Code == Keyboard.Key.Down) if (metaBalles.Count != 0)
            {
                shaderDataControlls.BallCount--;
                var getLastBall = metaBalles.Last();
                metaBalles.Remove(getLastBall);
            }
            
            // Add ball
            if (e.Code == Keyboard.Key.Up) if (metaBalles.Count < 99)
            {
                shaderDataControlls.BallCount++;
                metaBalles.Add(new MetaBall(
                    shaderDataControlls.MaxRadius, 
                    shaderDataControlls.MaxVelosety, 
                    _configuration));
            }

            var resetClockKeys = new Keyboard.Key[] {
                Keyboard.Key.Up,
                Keyboard.Key.Down
            };
            var statUpdateSoundKeys = new Keyboard.Key[] {
                Keyboard.Key.F3, Keyboard.Key.F4,
                Keyboard.Key.E,  Keyboard.Key.A,
                Keyboard.Key.Q,  Keyboard.Key.D,
                Keyboard.Key.S,  Keyboard.Key.Z,
                Keyboard.Key.F2, Keyboard.Key.Left,
                Keyboard.Key.Right
            };
            var pingSoundKeys = new Keyboard.Key[] {
                Keyboard.Key.Up
            };
            var popSoundKeys = new Keyboard.Key[] {
                Keyboard.Key.Down
            };

            PlaySoundOnKeys(e, popSoundKeys, soundControls.PlayPopSound);
            PlaySoundOnKeys(e, pingSoundKeys, soundControls.PlayPingSound);
            PlaySoundOnKeys(e, statUpdateSoundKeys, soundControls.PlayStatUpdateSound);
            ResetClockOnKeys(e, resetClockKeys);
        }
        private void CreateMetaballs()
        {
            // Ceate Alle the metaballs
            metaBalles = new List<MetaBall>();
            Enumerable.Repeat(metaBalles, shaderDataControlls.BallCount)
               .ToList()
               .ForEach(mb => mb.Add(new MetaBall(shaderDataControlls.MaxRadius, shaderDataControlls.MaxVelosety, _configuration)));
        }
        private void SetShaderUniforms()
        {
            // Eddit var in .frag file
            mbShader.SetUniform("time", clock.ElapsedTime.AsSeconds());
            mbShader.SetUniform("ballCount", shaderDataControlls.BallCount);
            mbShader.SetUniform("modIntesety", shaderDataControlls.ModIntesety);
            mbShader.SetUniform("hsvMultiplayer", shaderDataControlls.HsvMultiplayer);
            mbShader.SetUniform("colorMultiplayer", shaderDataControlls.ColorMultiplayer);

            // get metaball valus to pass to shader
            mbShader.SetUniformArray("xMetaBallPositions", GetMetaballXPositions());
            mbShader.SetUniformArray("yMetaBallPositions", GetMetaballYPositions());
            mbShader.SetUniformArray("metaBallRadius", GetMetaballRadius());
        }
        private void SetShaderInfo()
        {
            // get shader file from path still nee to shorten the path name
            Stream shaderFile =
                new FileStream(
                    _functionalitys.FindPathFileNameInDopamineGameFiles("Shader.frag", "Projects/MetaBallsShader"),
                    FileMode.Open);

            // set shader mode to fragment shader
            mbShader = new Shader(null, null, shaderFile);

            // Eddit var in .frag file
            mbShader.SetUniform("resulution", new Vector2f(_configuration.WindowWidth, _configuration.WindowHeight));
            mbShader.SetUniform("bollCount", shaderDataControlls.BallCount);
        }
        private void DrawOutlineMetaballs(RenderWindow window)
        {
            CircleShape shape = new();
            metaBalles.ForEach(mb => {

                shape.Position = new(mb.Position.X, mb.Position.Y);
                shape.Radius = mb.Radius;
                shape.Origin = new(mb.Radius, mb.Radius);
                shape.OutlineColor = Color.Black;
                shape.FillColor = new Color(255, 255, 255, 200);
                shape.OutlineThickness = 5;
                window.Draw(shape);
            });
        }
        private float[] GetMetaballXPositions()
        {
            var toReturn = new float[shaderDataControlls.BallCount];

            for (int i = 0; i < metaBalles.Count; i++)
                toReturn[i] = metaBalles[i].Position.X;

            return toReturn;
        }
        private float[] GetMetaballYPositions()
        {
            var toReturn = new float[shaderDataControlls.BallCount];

            for (int i = 0; i < metaBalles.Count; i++)
                toReturn[i] = metaBalles[i].Position.Y;

            return toReturn;
        }
        private float[] GetMetaballRadius()
        {
            var toReturn = new float[shaderDataControlls.BallCount];

            for (int i = 0; i < metaBalles.Count; i++)
                toReturn[i] = metaBalles[i].Radius;

            return toReturn;
        }


        public void ResetClockOnKeys(KeyEventArgs e, Keyboard.Key[] keys)
        {
            keys.ToList().ForEach(key => {
                if (e.Code == key) clock.Restart();
            });
        }
        public void ShowKeyMessages(KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Left || e.Code == Keyboard.Key.Right)
                messageControls.ShowMessage($"ModIntesety set to {shaderDataControlls.ModIntesety}");

            if (e.Code == Keyboard.Key.S || e.Code == Keyboard.Key.Z)
                messageControls.ShowMessage($"HsvMultiplayer set to {shaderDataControlls.HsvMultiplayer}");

            if (e.Code == Keyboard.Key.Q || e.Code == Keyboard.Key.D)
                messageControls.ShowMessage($"ColorMultiplayer set to {shaderDataControlls.ColorMultiplayer}");

            if (e.Code == Keyboard.Key.E || e.Code == Keyboard.Key.A)
                messageControls.ShowMessage($"MaxVelosety set to {shaderDataControlls.MaxVelosety} for new balls");

            if (e.Code == Keyboard.Key.F3 || e.Code == Keyboard.Key.F4)
                messageControls.ShowMessage($"MaxRadius set to {shaderDataControlls.MaxRadius} for new balls");

            if (e.Code == Keyboard.Key.F2)
            {
                var showOutlineStatus = shaderDataControlls.ShowOutline ? "ON" : "OFF";
                messageControls.ShowMessage($"ShowOutline {showOutlineStatus}");
            }
        }

        #pragma warning disable CA1822 // Mark members as static
        public void PlaySoundOnKeys(KeyEventArgs e, Keyboard.Key[] keys, Action soundFunction)
#pragma warning restore CA1822 // Mark members as static
        {
            keys.ToList().ForEach(key => {          
                if (e.Code == key) soundFunction.Invoke();                        
            });
        }

    }
}