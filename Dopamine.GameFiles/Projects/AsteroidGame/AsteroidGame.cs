using Dopamine.Core.ExtensionMethods;
using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.Services.ProjectServices;
using Dopamine.GameFiles.Projects.AsteroidGame.Entities;
using Dopamine.GameFiles.Projects.AsteroidGame.Entities.Ammo;
using Dopamine.GameFiles.Projects.AsteroidGame.Entities.Astroids;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Numerics;
using System.Diagnostics;

namespace Dopamine.GameFiles.Projects.AsteroidGame;

public class AsteroidGame : BaseGameFile, IGameFile
{
    private readonly IRenderer _renderer;
    private readonly IEngineConfiguration _configuration;
    private readonly IEngineFunctionalitys _functionalitys;
    private readonly IKeyMapping _keyMapping;

    private Shader rayCastingShader;
    private UiControls ui;
    private Ship ship;
    private bool joystickConnected = false;
    private AmmoLister ammoLister;
    private AstroidLister astroidLister;

    private bool shaderOn = true;
    private float maxShipSpeed = 300;
    private float inputSpeedUp = 1;

    public AsteroidGame(IRenderer renderer, IEngineConfiguration configuration, 
        IEngineFunctionalitys functionalitys, IKeyMapping keyMapping)
    {
        _functionalitys = functionalitys;
        _renderer = renderer;
        _configuration = configuration;
        _keyMapping = keyMapping;   
    }
    public void EventDeclaration(RenderWindow window) 
    {
        JoystickConnectionEvents(window);
        MousEvents(window);
        KeyBordEvents(window);
    }
    public void LoadInProjectAssets()
    {
        ui = new(_configuration);
        ammoLister = new(_functionalitys, _configuration);
        astroidLister = new(_functionalitys, _configuration);
        ship = new(_configuration, _functionalitys, _keyMapping, astroidLister);
        SetColoring();

        // get shader file from path still nee to shorten the path name
        Stream shaderFile =
            new FileStream(
                _functionalitys.FindPathFileNameInDopamineGameFiles("RayCastingShader.frag", "Projects/AsteroidGame"),
                FileMode.Open);

        // set shader mode to fragment shader
        rayCastingShader = new Shader(null, null, shaderFile);
        rayCastingShader.SetUniform("resulution", new Vector2f(_configuration.WindowWidth, _configuration.WindowHeight));
        rayCastingShader.SetUniform("castRaysFrom", new Vector2f(-100, -100));
        rayCastingShader.SetUniformArray("astroidOridginals", astroidLister.GetAstroidOridginalsForShader());

        Thread.Sleep(3000);
    }
    public void GameLoop(RenderWindow window)
    {
        InputStatus();

        if (IsShipliving())
        {
            DrawEnemys(window);
            DrawAmmo(window);
            DrawShip(window);
        }
        else
        {
            if (ship.LifePoints > 0) {
                ui.ShowMessage($"Press speasbar to respawn ship at your mouse possition {ship.NewLifePoints} life's left");
                DrawEnemys(window);
                DrawAmmo(window);
            }
            else {
                shaderOn = false;
                ui.ShowMessage("Game Over press enter to restart",20);
            } 
        }
     
        ui.Draw(window);
        if (shaderOn) {

            var rayCastingPosition =
                new Vector2f(_functionalitys.SFML_GetMousePosition().X / 10, _configuration.WindowHeight +200);

            rayCastingShader.SetUniform("castRaysFrom", rayCastingPosition);
            rayCastingShader.SetUniformArray("astroidOridginals", astroidLister.GetAstroidOridginalsForShader());
            _renderer.Draw(window,rayCastingShader);
        }
    }
    private void JoystickConnectionEvents(RenderWindow window)
    {
        window.JoystickConnected += (object? sender, JoystickConnectEventArgs e) =>
        {
            joystickConnected = true;
            ui.ShowMessage("DualSense Controller Connected");
        };
        window.JoystickDisconnected += (object? sender, JoystickConnectEventArgs e) =>
        {
            joystickConnected = false;
            ui.ShowMessage("DualSense Controller Disconnected");
        };
        window.JoystickMoved += (object? sender, JoystickMoveEventArgs e) => {
            // R1
            if (e.Axis == Joystick.Axis.V)
            {
                inputSpeedUp = e.Position < 0 ? 1 : e.Position * 5;
            }
            // R2
            if (e.Axis == Joystick.Axis.U)
            {
                inputSpeedUp = e.Position < 0 ? 1 : e.Position * 5;
                inputSpeedUp = -inputSpeedUp;
            }
            // Left stick
            if (e.Axis == Joystick.Axis.Y)
            {
                ship.Rotation = e.Position * 2;
            }
        };
        window.JoystickButtonPressed += (object? sender, JoystickButtonEventArgs e) =>
        {
            if (e.Button == _keyMapping.RightDpadDown && IsShipliving())
            {
                ammoLister.ShootBullit(350, ship.Possition, ship.ShipTipLocation);
            }

            if (e.Button == _keyMapping.RightDpadRight && IsShipliving())
            {
                ammoLister.ShootMissel(1000, ship.Possition, ship.ShipTipLocation);
            }
        };
    }

    private void MousEvents(RenderWindow window)
    {
        window.MouseButtonPressed += (object? sender, MouseButtonEventArgs e) => {

            if (e.Button == Mouse.Button.Left && IsShipliving())
            {
                ammoLister.ShootBullit(350, ship.Possition, ship.ShipTipLocation);
            }

            if (e.Button == Mouse.Button.Right && IsShipliving())
            {
                ammoLister.ShootMissel(1000, ship.Possition, ship.ShipTipLocation);
            }
        };
    }
    private void KeyBordEvents(RenderWindow window)
    {
        window.KeyPressed += (object? sender, KeyEventArgs e) => {

            if (e.Code == Keyboard.Key.Space && !IsShipliving() && ship.LifePoints > 0)
            {
                ship.LifePoints = ship.NewLifePoints;
                ship.ReSpawnShipToMouse();
            }

            if (e.Code == Keyboard.Key.Enter && ship.LifePoints <= 0)
            {
                ResetGame();
            }

            if (e.Code == Keyboard.Key.F1)
            {
                shaderOn = !shaderOn;
            }
        };
    }
    private bool IsShipliving()
    {
        return ship.LifePoints == ship.NewLifePoints;
    }
    private void DrawEnemys(RenderWindow window)
    {
        astroidLister.DrawAstroids(window);
        astroidLister.AmmoColisionCheak(ammoLister);
        astroidLister.AstoidIsHitAction(ui);
        astroidLister.AddOridginals(ship.Possition);
    }
    private void DrawAmmo(RenderWindow window)
    {
        ammoLister.DrawProjectiles(window);
    }
    private void DrawShip(RenderWindow window)
    {
        if (joystickConnected) 
            ship.ControlerMoveShip(inputSpeedUp, maxShipSpeed);
        else
            ship.MoveShipToMouse(maxShipSpeed);

        ship.ShipColistionCheak(astroidLister);
        ship.Draw(window);
    }
    private void ResetGame()
    {
        ui = new(_configuration);
        ammoLister = new(_functionalitys, _configuration);
        astroidLister = new(_functionalitys, _configuration);
        ship = new(_configuration, _functionalitys, _keyMapping, astroidLister);
        SetColoring();
        shaderOn = true;
    }
    private void InputStatus()
    {
        ui.StatusInfo =
            $"Life's: {ship.LifePoints} \t" +
            $"{ammoLister.GetInfo()}";
    }
    private void SetColoring()
    {
        byte fillColorAlfha = 80;

        ship.ShipColor = new Color(255, 255, 255, fillColorAlfha);
        ship.ShipOutlineColor = Color.White;

        astroidLister.AstroidFragmentColor = new Color(0, 255, 0, fillColorAlfha);
        astroidLister.AstroidFragmentOutlineColor = Color.White;
        astroidLister.AstroidOridginalsColor = new Color(0, 0, 255, fillColorAlfha);
        astroidLister.AstroidOridginalsOutlineColor = Color.White;

        ammoLister.BullitsColor = new Color(255, 0, 0, fillColorAlfha);
        ammoLister.BullitsOutlineColor = Color.White;
        ammoLister.MisselsColor = new Color(100, 50, 100, fillColorAlfha);
        ammoLister.MisselsOutlineColor = Color.White;
        ammoLister.MisselsFragmentColor = new Color(255, 100, 255, fillColorAlfha);
        ammoLister.MisselsFragmentOutlineColor = Color.Red;
    }   
}

