using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BingoLineUp.Classes;

namespace BingoLineUp
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        UnionClasses myGame;

        public Game1()
        {
            Window.Title = "Bingo Line Up Client";
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 475;
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            myGame = new UnionClasses(6, 7);
            myGame.CloseGame += new EventHandler(myGame_CloseGame);
            base.Initialize();
        }
        void myGame_CloseGame(object sender, EventArgs e)
        {
            this.Exit();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            myGame.LoadContent(Content);

        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            //if (Keyboard.GetState().IsKeyDown(Keys.Escape)) this.Exit();
            myGame.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Lavender);
            spriteBatch.Begin();
            myGame.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
