using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BingoLineUp.Classes
{
    class Button
    {
        public enum FontStyle {FirstStyle,SecondStyle}
        public string Text {get;set;}
        public event EventHandler OnClick, OnFocus, OnOutside;
        public Rectangle RectButtonCase { get; private set; }
        Rectangle defaultSettingsRect;
        Vector2 textPosition;
        Texture2D backgroundButtonTex2D;
        public SpriteFont TextFont { get; set; }
        public int ID { get; private set; }
        SpriteFont defaultSettingsFont, newStyleFont;

        public Button(Rectangle ButtonCase,string text, int id)
        {
            RectButtonCase = ButtonCase;
            textPosition = new Vector2(RectButtonCase.X+10,RectButtonCase.Y+10);
            defaultSettingsRect = RectButtonCase;
            Text = text;
            ID = id;
        }
        public void LoadContent(ContentManager content)
        {
            backgroundButtonTex2D = content.Load<Texture2D>(@"Images\Background");
            TextFont = content.Load<SpriteFont>(@"Fonts\FontButton");
            defaultSettingsFont = TextFont;
            newStyleFont = content.Load<SpriteFont>(@"Fonts\FontButton2");
        }

        MouseState lastMouseState;
        bool isFocused = false;

        public void Update(MouseState mouseState)
        {
            if (RectButtonCase.Contains(new Point(mouseState.X, mouseState.Y))) 
            {
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                { OnClick.Invoke(this, new EventArgs()); }
                else if (!isFocused) { OnFocus.Invoke(this, new EventArgs()); isFocused = true; }
            }
            else { OnOutside.Invoke(this, new EventArgs()); isFocused = false; }
            lastMouseState = mouseState;
        }

        public void ChangeSize(int width, int height)
        {
            RectButtonCase = new Rectangle(RectButtonCase.X, RectButtonCase.Y,width,height);
        }
        public void ChangeFont(FontStyle style)
        {
            switch (style)
            {
                case FontStyle.FirstStyle: TextFont = defaultSettingsFont; break;
                case FontStyle.SecondStyle: TextFont = newStyleFont; break;
            }
        }
        public void AddDeltaSize(int deltaWidth, int deltaHeight)
        {
            RectButtonCase = new Rectangle(RectButtonCase.X, RectButtonCase.Y, RectButtonCase.Width+deltaWidth, RectButtonCase.Height+deltaHeight);
        }
        public void DefaultSettings()
        {
            RectButtonCase = defaultSettingsRect;
            TextFont = defaultSettingsFont;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundButtonTex2D,RectButtonCase,Color.Red);
            spriteBatch.DrawString(TextFont, Text, textPosition, Color.Lime);
        }
    }
}
