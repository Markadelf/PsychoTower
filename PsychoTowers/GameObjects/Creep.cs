using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychoTowers
{
    public enum Team
    {
        Team1,
        Team2
    }
    public enum Direction { Right, Left, Up, Down }

    /// <summary>
    /// Represents a mob trying to get through the maze
    /// </summary>
    public class Creep
    {

        #region Orientation
        //Positional Info
        private float x;
        public float X
        {
            get { return x; }
            set
            {
                //if(!MapData.TileData[(int)value, (int)Y].HasFlag(TileProperties.Blocked) && !MapData.TileData[(int)(value + .8f), (int)(Y + .8f)].HasFlag(TileProperties.Blocked) && 
                //    !MapData.TileData[(int)value, (int)(Y + .8f)].HasFlag(TileProperties.Blocked) && !MapData.TileData[(int)(value + .8f), (int)(Y)].HasFlag(TileProperties.Blocked))
                x = value;
                currentTile = MapData.TileData[(int)X,(int)Y];
            }
        }
        private float y;
        public float Y
        {
            get { return y; }
            set
            {
                //if (!MapData.TileData[(int)X, (int)value].HasFlag(TileProperties.Blocked) && !MapData.TileData[(int)(X + .8f), (int)(value + .8f)].HasFlag(TileProperties.Blocked) &&
                //     !MapData.TileData[(int)X, (int)(value + .8f)].HasFlag(TileProperties.Blocked) && !MapData.TileData[(int)(X + .8f), (int)(value)].HasFlag(TileProperties.Blocked))
                y = value;
                currentTile = MapData.TileData[(int)X, (int)Y];
            }
        }

        //Map it is on and info about it
        private Map MapData;
        private TileProperties currentTile;

        //Which team am I on
        public Team MyTeam { get; set; }

        //Am I Alive or dead?
        public bool Alive { get; set; }

        //Which direction am I facing
        public Direction Facing { get; set; }


        #endregion

        #region Stats
        //Health
        private int health;
        public int MaxHealth { get; private set; }
        public int Health
        {
            get { return health; }
            set
            {
                if(value < MaxHealth)
                    health = value;
                if (health < 0)
                    Alive = false;
            }
        }
        //Speed
        private float speed;
        public float Speed
        {
            get
            {
                //Apply the propper de/buffs.
                if (currentTile.HasFlag(TileProperties.SpeedBuff) && currentTile.HasFlag(TileProperties.SpeedNerf))
                    return speed;
                if (currentTile.HasFlag(TileProperties.SpeedBuff))
                    return speed * 3 / 2;
                if (currentTile.HasFlag(TileProperties.SpeedNerf) && speed > 1)
                    return speed * 2 / 3;
                return speed;
            }
            private set { speed = value; }
        }
        //Attack
        private int attack;
        public int Attack
        {
            get
            {
                //Apply the propper de/buffs.
                if (currentTile.HasFlag(TileProperties.AttackBuff) && currentTile.HasFlag(TileProperties.AttackNerf))
                    return attack;
                if (currentTile.HasFlag(TileProperties.AttackBuff))
                    return attack * 3 / 2;
                if (currentTile.HasFlag(TileProperties.AttackNerf) && attack > 1)
                    return attack * 2 / 3;
                return attack;
            }
            private set { attack = value; }
        }
        //Armor
        private int armor;
        public int Armor
        {
            get
            {
                //Apply the propper de/buffs.
                if (currentTile.HasFlag(TileProperties.ArmorBuff) && currentTile.HasFlag(TileProperties.ArmorNerf))
                    return armor;
                if (currentTile.HasFlag(TileProperties.ArmorBuff))
                    return armor * 3 / 2;
                if (currentTile.HasFlag(TileProperties.ArmorNerf))
                    return armor * 2 / 3;
                return armor;
            }
            private set { armor = value; }
        }

        #endregion


        public Creep(int health, float speed, int attack, int armor, Map map, Team team, int x, int y)
        {
            Alive = true;
            MaxHealth = health;
            Health = health;
            Speed = speed;
            Attack = attack;
            Armor = armor;
            MapData = map;
            MyTeam = team;
            X = x;
            Y = y;
        }







        #region Actions

        //Move in a given direction
        public void Move(Direction direct)
        {
            //Update orientation
            Facing = direct;
            switch (direct)
            {
                case Direction.Right:
                    if (MapData.TileData[(int)(X + speed + 1), (int)Y].HasFlag(TileProperties.Blocked))
                        X = (int)(X + speed);
                    else
                        X += Speed;
                    break;
                case Direction.Left:
                    if (MapData.TileData[(int)(X - speed), (int)Y].HasFlag(TileProperties.Blocked))
                        X = (int)(X - speed + 1);
                    else
                        X -= Speed;
                    break;
                case Direction.Up:
                    break;
                case Direction.Down:
                    break;
                default:
                    break;
            }
        }





        //Assaults the other Creep dealing damage
        public void Strike(Creep other)
        {
            other.Health -= (this.Attack - (other.Armor / 2));
        }

        #endregion



    }
}
