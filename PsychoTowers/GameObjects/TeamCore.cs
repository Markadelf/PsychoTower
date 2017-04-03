using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychoTowers
{
    public class TeamCore
    {
        //Position
        public int X { get; set; }
        public int Y { get; set; }

        //Team of this core
        public Team MyTeam { get; private set; }

        //Link to map
        public Map MapData { get; set; }

        //Am I alive?
        public bool Alive { get; set; }

        //Stats
        public int Level { get; set; }
        private int health;

        public int Health
        {
            get { return health; }
            set
            {
                if (health > value)
                {
                    health = value;
                    if (health > 0)
                    {
                        Pulse();
                    }
                    else
                        Alive = false;
                }
            }
        }


        //Constructor
        public TeamCore(int x, int y, Team team, Map mapdata)
        {
            Alive = true;
            X = x;
            Y = y;
            MyTeam = team;
            MapData = mapdata;
            Level = 0;
            Health = 10;
        }


        //Large Pulse Attack that the core uses whenever it takes damage
        public void Pulse()
        {

        }


        //Factory
        public Creep NewCreep()
        {
            if (Level < 10000)
                Level++;
            return new Creep(10 * Level, 0.5f + .01f * Level, 5 * Level, 5 * Level, MapData, MyTeam, X, Y);
        }

    }
}
