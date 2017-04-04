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
            active = new Map();
            Rand = new Random();
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
            SpriteManager.CreepTexture = Content.Load<Texture2D>("Square");
            SpriteManager.EmptyTowerSlotTexture = Content.Load<Texture2D>("Square");
            SpriteManager.WallTexture = Content.Load<Texture2D>("Mountain");
            SpriteManager.TeamCoreTexture = Content.Load<Texture2D>("Square");


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

            //active.PlaceBlock(Rand.Next(2, active.TileData.GetLength(0) - 2), Rand.Next(1, active.TileData.GetLength(1) - 1));
            active.PlaceBlock(Rand.Next(2, active.TileData.GetLength(0) - 2), Rand.Next(1, active.TileData.GetLength(1) - 1));
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

            spriteBatch.Begin();
            SpriteManager.DrawMap(spriteBatch, active);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
