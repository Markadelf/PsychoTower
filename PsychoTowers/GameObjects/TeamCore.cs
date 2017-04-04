﻿using System;
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
        public static int Level { get; set; }
        private int health;

        public static float RespawnTimer { get; set; }

        public int Health
        {
            get { return health; }
            set
            {
                if (health > value && Alive)
                {
                    health = value;
                    if (Alive)
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
            health = 10;
            RespawnTimer = 0;
        }



        //Large Pulse Attack that the core uses whenever it takes damage
        public void Pulse()
        {
            for(int i = 0; i < MapData.TeamOne.Count;i++)
            {
                MapData.TeamOne[i].Alive = false;
            }
            for (int i = 0; i < MapData.TeamTwo.Count; i++)
            {
                MapData.TeamTwo[i].Alive = false;
            }
        }


        //Factory
        public Creep NewCreep()
        {
            return new Creep(20 + 1 * Level, 1.5f + .01f * Level * Level, 10 * Level * Level, 5 * Level, MapData, MyTeam, X, Y);
        }

    }
}
