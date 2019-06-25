using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AntRace.GameObjects;
using AntRace.GameObjects.NPCS;
using AntRace.Handlers;
using AntRace.Helpers;
using System;
using System.Collections.Generic;

namespace AntRace
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameCore : Game
    {
        public static Random random = new Random();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RaceHandler RaceHandler;
        BalanceHandler BalanceHandler;
        InputHandler InputHandler;
        Texture2D BackgroundTexture;

        public GameCore()
        {
            Window.Title = "AA2K - The Amazing AntRace 2k18";
            IsMouseVisible = true;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            RaceHandler = new RaceHandler(Content);
            BalanceHandler = new BalanceHandler(Content);
            InputHandler = new InputHandler();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            BackgroundTexture = Content.Load<Texture2D>("Sprites/background");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _Timer.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            RaceHandler.Update(gameTime);
            InputHandler.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.GreenYellow);

            spriteBatch.Begin();
            spriteBatch.Draw(BackgroundTexture, Vector2.Zero, Color.White);
            spriteBatch.End();

            BalanceHandler.Draw(spriteBatch);
            RaceHandler.Draw(spriteBatch);

            base.Draw(gameTime);
        }
                
        protected override void UnloadContent()
        {
            //GC will take care of this. but for good measure
            BackgroundTexture = null;
            RaceHandler.UnloadContent();
            BalanceHandler.UnloadContent();
            base.UnloadContent();
        }
    }
}
