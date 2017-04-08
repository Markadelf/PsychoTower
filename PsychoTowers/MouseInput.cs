﻿using System;
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

        public int tower { get; private set; }

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
            tower = 0;

            
        }

        public void DetectInput()
        {
            while (InUse)
            {
                MouseState mouse = Mouse.GetState();
                X = (mouse.X - SpriteManager.DrawMapX) / 24;
                Y = (mouse.Y - SpriteManager.DrawMapY) / 24;
                if (mouse.ScrollWheelValue > prev.ScrollWheelValue)
                {
                    tower = (tower + 1) % 8;
                }
                else if (mouse.ScrollWheelValue < prev.ScrollWheelValue)
                {
                    tower = (tower + 7) % 8;
                }

                if (mouse.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released)
                {
                    if (X / 2 != X / 2.0 && Y / 2 == Y / 2.0)
                    {
                        Act(Command.PlaceTower, tower);
                    }
                    //Is it a wall?
                    else
                    {
                        Act(Command.PlaceWall, 0);
                    }
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
