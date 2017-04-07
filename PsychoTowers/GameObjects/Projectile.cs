using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace PsychoTowers
{
    public class Projectile
    {
        //Target
        public Creep Target { get; set; }

        //Have I hit yet?
        public bool Alive { get; set; }

        //Damage the bullet inflicts
        public int Damage { get; set; }

        //Location
        public float OriginalX { get; set; }
        public float OriginalY { get; set; }
        public float X { get; set; }
        public float Y { get; set; }


        public float ShotVelocity { get; set; }


        /// <summary>
        /// Creates a projectile
        /// </summary>
        /// <param name="x">Start position x</param>
        /// <param name="y">Start position y</param>
        /// <param name="target">Target</param>
        public Projectile(float x, float y, Creep target, int attack)
        {
            Alive = true;
            X = x;
            OriginalX = x;
            OriginalY = y;
            Y = y;
            Target = target;
            ShotVelocity = 3;
            Damage = attack;
        }

        //Chase bullet every step
        public void Step(float deltaTime)
        {
            Seek(deltaTime);
            if (CheckCollision(Target))
            {
                StrikeTarget();
                Alive = false;
            }
        }

        public void StrikeTarget()
        {
            int damage = (Damage - (Target.Armor));
            if (damage > 10)
            {
                if (Game1.Rand.Next(10) == 0)
                    Target.Health -= 2 * damage;
                else
                    Target.Health -= damage;
            }
            else
                Target.Health -= 10;
        }






        public bool CheckCollision(Creep other)
        {
            if (X + 5f / 12> other.X + 1)
                return false;
            if (other.X > X + 7f / 12)
                return false;
            if (Y + 5f / 12 > other.Y + 1)
                return false;
            if (other.Y > Y + 7f / 12)
                return false;
            return true;
        }

        public void Seek(float deltaTime)
        {
            Vector2 direction = new Vector2(Target.X - X, Target.Y - Y);
            direction /= direction.Length();
            direction *= ShotVelocity;
            X += direction.X * deltaTime;
            Y += direction.Y * deltaTime;

        }





    }
}
