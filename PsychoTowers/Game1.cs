using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PsychoTowers
{


    public enum GameState
    {
        MainMenu,       //Main menu, allows you to start games
        PauseMenu,      //Pause Menu pauses the game mid play
        GamePlaying,    //Game is currently playing
        GameConclusion  //Game has just ended, declare a victor
    }



    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map active;
        public GameState MyState { get; set; }
        public static Random Rand { get; set; }
        public static Player PlayerOne { get; set; }
        public static Player PlayerTwo { get; set; }



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Rand = new Random();
            active = new Map();
            graphics.PreferredBackBufferWidth = 720;
            graphics.PreferredBackBufferHeight = 480;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            StartNewGame();
            PlayerOne = new Player(new MouseInput(), active);
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteManager.SquareTestTexture = Content.Load<Texture2D>("Square");
            SpriteManager.BackgroundTexture = Content.Load<Texture2D>("Background");
            SpriteManager.BacklightTexture = Content.Load<Texture2D>("Backlight");
            SpriteManager.BorderTexture = Content.Load<Texture2D>("Border");
            SpriteManager.PanelTexture = Content.Load<Texture2D>("Panel");


            SpriteManager.CreepDownTexture = Content.Load<Texture2D>("CreepRight");
            SpriteManager.CreepRightTexture = Content.Load<Texture2D>("CreepRight");
            SpriteManager.CreepLeftTexture = Content.Load<Texture2D>("CreepLeft");
            SpriteManager.CreepUpTexture = Content.Load<Texture2D>("CreepLeft");

            SpriteManager.AuraTexture = Content.Load<Texture2D>("Aura");
            SpriteManager.RangeTexture = Content.Load<Texture2D>("AimCircle");

            SpriteManager.EmptyTowerSlotTexture = Content.Load<Texture2D>("TowerSlot");
            SpriteManager.ShootTowerTexture = Content.Load<Texture2D>("ShootTower");
            SpriteManager.BuffTowerTexture = Content.Load<Texture2D>("BuffTower");
            SpriteManager.NerfTowerTexture = Content.Load<Texture2D>("NerfTower");
            SpriteManager.ExpTowerTexture = Content.Load<Texture2D>("BuffTower");


            SpriteManager.WallTexture = Content.Load<Texture2D>("Mountain");
            SpriteManager.TeamCoreTexture = Content.Load<Texture2D>("Flag");


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            if (PlayerOne != null && PlayerOne.PlayerInput != null && PlayerOne.PlayerInput.InUse)
                 PlayerOne.PlayerInput.InputListener.Abort();
            if (PlayerTwo != null && PlayerTwo.PlayerInput != null && PlayerTwo.PlayerInput.InUse)
                PlayerTwo.PlayerInput.InputListener.Abort();

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            switch (MyState)
            {
                case GameState.MainMenu:
                    break;
                case GameState.PauseMenu:
                    break;
                case GameState.GamePlaying:
                    if (!active.TeamOneCore.Alive)
                    {
                        EndGame(Team.Team1);
                    }
                    if (!active.TeamTwoCore.Alive)
                    {
                        EndGame(Team.Team2);
                    }
                    for(int i = 0; i < 10 && active.TeamOneCore.Alive && active.TeamTwoCore.Alive && Keyboard.GetState().IsKeyDown(Keys.LeftShift); i++)
                        active.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
                    active.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
                    break;
                case GameState.GameConclusion:
                    break;
                default:
                    break;
            }
            


            base.Update(gameTime);
        }



        public void StartNewGame()
        {
            active = new Map();
            MyState = GameState.GamePlaying;
        }

        public void EndGame(Team winner)
        {
            if (PlayerOne != null && PlayerOne.PlayerInput != null)
                PlayerOne.PlayerInput.InUse = false;
            if (PlayerTwo != null && PlayerTwo.PlayerInput != null)
                PlayerTwo.PlayerInput.InUse = false;

            MyState = GameState.GameConclusion;
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(160, 140, 120));

            spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);
            SpriteManager.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
            SpriteManager.DrawMap(spriteBatch, active);
            if (PlayerOne != null)
            {
                SpriteManager.DrawSidePannel(spriteBatch, PlayerOne);
                if (PlayerOne.PlayerInput != null && PlayerOne.PlayerInput.InUse)
                    SpriteManager.DrawCursor(spriteBatch, PlayerOne.PlayerInput);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
