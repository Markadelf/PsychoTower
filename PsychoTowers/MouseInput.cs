using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Threading;


namespace PsychoTowers
{
    class MouseInput: InputMethod
    {
        public bool InUse { get; set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public event Request Act;

        public Thread InputListener { get; set; }

        private MouseState prev;
        
        public MouseInput()
        {
            X = 0;
            Y = 0;
            Activate();
        }

        public void Activate()
        {
            InUse = true;
            InputListener = new Thread(DetectInput);
            InputListener.Start();

            
        }

        public void DetectInput()
        {
            while (InUse)
            {
                MouseState mouse = Mouse.GetState();
                X = (mouse.X - SpriteManager.DrawMapX) / 24;
                Y = (mouse.Y - SpriteManager.DrawMapY) / 24;

                if (mouse.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released)
                {
                    Act(Command.PlaceWall, 0);
                }
                if (mouse.RightButton == ButtonState.Pressed && prev.RightButton == ButtonState.Released)
                {
                    Act(Command.RemoveAt, 0);
                }
                prev = mouse;
            }
        }

    }
}
