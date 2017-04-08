using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Threading;


namespace PsychoTowers
{
    class GamePadInput: InputMethod
    {
        public bool InUse { get; set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public event Request Act;

        public Thread InputListener { get; set; }

        private GamePadState prev;

        public int Tower { get; private set; }

        public GamePadInput()
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
            Tower = 0;

            
        }

        public void DetectInput()
        {
            Thread.Sleep(500);

            while (InUse)
            {
                GamePadState gp = GamePad.GetState(0);
                if ((gp.DPad.Up == ButtonState.Pressed || gp.ThumbSticks.Left.Y > .5f) && Y > 0)
                {
                    Y--;
                    Thread.Sleep(100);
                }
                if ((gp.DPad.Down == ButtonState.Pressed || gp.ThumbSticks.Left.Y < -.5f) && Y < 18)
                {
                    Y++;
                    Thread.Sleep(100);
                }
                if ((gp.DPad.Left == ButtonState.Pressed || gp.ThumbSticks.Left.X < -.5f) && X > 0)
                {
                    X--;
                    Thread.Sleep(100);
                }
                if ((gp.DPad.Right == ButtonState.Pressed || gp.ThumbSticks.Left.X > .5f) && X < 20)
                {
                    X++;
                    Thread.Sleep(100);
                }
                if (gp.Buttons.LeftShoulder == ButtonState.Pressed && prev.Buttons.LeftShoulder == ButtonState.Released)
                {
                    Tower = (Tower + 1) % 8;
                }
                else if (gp.Buttons.RightShoulder == ButtonState.Pressed && prev.Buttons.RightShoulder == ButtonState.Released)
                {
                    Tower = (Tower + 7) % 8;
                }

                if (gp.Buttons.A == ButtonState.Pressed && prev.Buttons.A == ButtonState.Released)
                {
                    if (X / 2 != X / 2.0 && Y / 2 == Y / 2.0)
                    {
                        Act(Command.PlaceTower, Tower);
                    }
                    //Is it a wall?
                    else
                    {
                        Act(Command.PlaceWall, 0);
                    }
                }
                if (gp.Buttons.B == ButtonState.Pressed && prev.Buttons.B == ButtonState.Released)
                {
                    Act(Command.RemoveAt, 0);
                }
                prev = gp;
            }
        }

    }
}
