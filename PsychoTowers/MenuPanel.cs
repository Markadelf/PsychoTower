using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace PsychoTowers
{
    public delegate void Click(); 

    public class MenuPanel
    {
        public Texture2D MyTexture { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Clickable { get; set; }
        public event Click ClickEvent;
        public string MyText { get; set; }


        public MenuPanel(Texture2D content, int x, int y, int width, int height, string text, bool clickable)
        {
            MyTexture = content;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Clickable = clickable;
            MyText = text;
            MyTexture = SpriteManager.ButtonTexture;
        }

        public void TryClick()
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.X > X && mouse.X - Width < X && mouse.Y > Y && mouse.Y - Height < Y)
                if(ClickEvent != null && Clickable)
                    ClickEvent();
        }


    }
}
