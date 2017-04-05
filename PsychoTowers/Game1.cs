using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PsychoTowers
{


    public enum GameState
    {

    }



    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map active;
        public static Random Rand { get; set; }

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
            SpriteManager.BackgroundTexture = Content.Load<Texture2D>("Square");
            SpriteManager.BorderTexture = Content.Load<Texture2D>("Square");

            SpriteManager.CreepDownTexture = Content.Load<Texture2D>("CreepRight");
            SpriteManager.CreepRightTexture = Content.Load<Texture2D>("CreepRight");
            SpriteManager.CreepLeftTexture = Content.Load<Texture2D>("CreepLeft");
            SpriteManager.CreepUpTexture = Content.Load<Texture2D>("CreepLeft");

            SpriteManager.EmptyTowerSlotTexture = Content.Load<Texture2D>("Square");
            SpriteManager.ShootTowerTexture = Content.Load<Texture2D>("ShootTower");
            SpriteManager.BuffTowerTexture = Content.Load<Texture2D>("BuffTower");
            SpriteManager.NerfTowerTexture = Content.Load<Texture2D>("NerfTower");
            SpriteManager.ExpTowerTexture = Content.Load<Texture2D>("Square");


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
            // TODO: Unload any non ContentManager content here
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

            active.PlaceBlock(Rand.Next(2, active.TileData.GetLength(0) - 2), Rand.Next(1, active.TileData.GetLength(1) - 1));
            //active.PlaceBlock(Rand.Next(3, active.TileData.GetLength(0) - 3), Rand.Next(1, active.TileData.GetLength(1) - 1));
            active.Step((float)gameTime.ElapsedGameTime.TotalSeconds);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);
            SpriteManager.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
            SpriteManager.DrawMap(spriteBatch, active);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
