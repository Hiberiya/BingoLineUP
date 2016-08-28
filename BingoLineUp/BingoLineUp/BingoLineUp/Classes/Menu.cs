using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BingoLineUp.Classes
{
    class Menu
    {
        public event EventHandler PlayerReadyToPlaySingleGame, PlayerReadyToPlayNetGame, PlayerWantsToSetUp, PlayerDoesntWantToPlay;

        Texture2D backgroundMenuTex2D;
        SpriteFont fontTitle;
        ButtonsCollection menuButtons;
        Rectangle rectBackground;
        SoundEffect soundSelect;

        public Menu() 
        {
            rectBackground = new Rectangle(0, 0, 500, 475);
            menuButtons = new ButtonsCollection();
            menuButtons.Add(new Rectangle(120, 160, 250, 50), "Одиночная игра");
            menuButtons.Add(new Rectangle(120, 220, 250, 50), "Сетевая игра");
            menuButtons.Add(new Rectangle(120, 280, 250, 50), "Настройки");
            menuButtons.Add(new Rectangle(120, 340, 250, 50), "Выход");

            foreach (Button b in menuButtons)
            {
                b.OnClick += new EventHandler(b_OnClick);
                b.OnFocus += new EventHandler(b_OnFocus);
                b.OnOutside += new EventHandler(b_OnOutside);
            }
        }

        void b_OnOutside(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b != null) 
            {
                b.DefaultSettings();
            }
        }

        void b_OnFocus(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b != null)
            {
                b.AddDeltaSize(5,5);
                b.ChangeFont(Button.FontStyle.SecondStyle);
                soundSelect.Play();
            }
        }

        void b_OnClick(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b != null)
            {
                switch(b.ID)
                {
                    case 1: PlayerReadyToPlaySingleGame.Invoke(this, new EventArgs()); break;
                    case 2: PlayerReadyToPlayNetGame.Invoke(this, new EventArgs()); break;
                    case 3: PlayerWantsToSetUp.Invoke(this, new EventArgs()); break;
                    case 4: PlayerDoesntWantToPlay.Invoke(this, new EventArgs()); break;
                }
            }
        }
        public void LoadContent(ContentManager content)
        {
            backgroundMenuTex2D = content.Load<Texture2D>(@"Images\Background");
            fontTitle = content.Load<SpriteFont>(@"Fonts\FontTitle");
            soundSelect = content.Load<SoundEffect>(@"Sounds\MoveChip");
            foreach (Button b in menuButtons)
            {
                b.LoadContent(content);
            }
        }
        public void Update(GameTime gameTime) 
        {
            foreach (Button b in menuButtons)
            {
                b.Update(Mouse.GetState());
            }
        }
        public void Draw(SpriteBatch spriteBath) 
        {
            spriteBath.Draw(backgroundMenuTex2D,rectBackground,Color.White);
            foreach (Button b in menuButtons)
            {
                b.Draw(spriteBath);
            }
        }
    }
}
