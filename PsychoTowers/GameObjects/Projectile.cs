using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //Time it will take to hit the target
        public float TimeToHit { get; set; }

        /// <summary>
        /// Creates a projectile
        /// </summary>
        /// <param name="x">Start position x</param>
        /// <param name="y">Start position y</param>
        /// <param name="target">Target</param>
        public Projectile(float x, float y, Creep target)
        {
            Alive = true;
            X = x;
            OriginalX = x;
            OriginalY = y;
            Y = y;
            Target = target;
            TimeToHit = 1;
        }

        //Chase bullet every step
        public void Step(float deltaTime)
        {
            if (!Target.Alive)
                Alive = false;
            TimeToHit -= deltaTime;
            X = TimeToHit * OriginalX + (1 - TimeToHit) * Target.X;
            Y = TimeToHit * OriginalY + (1 - TimeToHit) * Target.Y;
            if (TimeToHit == 0)
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
            if (X > other.X + 1)
                return false;
            if (other.X > X + .33f)
                return false;
            if (Y > other.Y + 1)
                return false;
            if (other.Y > Y + .33f)
                return false;
            return true;
        }






    }
}
