using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BingoLineUp.Classes
{
    class ButtonsCollection: IEnumerable
    {
        private List<Button> Buttons { get; set; }
        public ButtonsCollection() { Buttons = new List<Button>(); }
        public Button this[int index]
        {
            get { return Buttons[index]; }
        }
        public void Add(Rectangle rectCaseButton, string title)
        {
            int id = Buttons.Count + 1;
            Buttons.Add(new Button(rectCaseButton, title, id));
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                yield return Buttons[i];
            }
        }
    }
}
