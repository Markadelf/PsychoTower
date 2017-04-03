using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychoTowers
{
    public enum Team
    {
        None,
        Team1,
        Team2
    }
    public enum Direction { None, Right, Left, Up, Down }

    /// <summary>
    /// Represents a mob trying to get through the maze
    /// </summary>
    public class Creep
    {

        #region Orientation
        //Positional Info
        private float x;
        private int lastX;
        public float X
        {
            get { return x; }
            set
            {
                if (value >= 0 && value <= MapData.TileData.GetLength(0) - 1)
                {
                    x = value;
                }
            }
        }
        private float y;
        private int lastY;
        public float Y
        {
            get { return y; }
            set
            {
                if(value >= 0 && value <= MapData.TileData.GetLength(1) - 1)
                {
                    y = value;
                }
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


        public Creep(int health, float speed, int attack, int armor, Map map, Team team, int xParam, int yParam)
        {
            Alive = true;
            MaxHealth = health;
            Health = health;
            Speed = speed;
            Attack = attack;
            Armor = armor;
            MapData = map;
            MyTeam = team;
            x = xParam;
            y = yParam;
            lastX = (int)x;
            lastY = (int)y;
            Reface();
        }




        public void Step(float deltaTime, bool heartbeat)
        {

            Move(Facing, deltaTime);
            if (DetermineReface())
            {
                Reface();
            }

        }


        private bool DetermineReface()
        {
            switch (Facing)
            {
                case Direction.Right:
                    return (int)X > lastX;
                case Direction.Left:
                    return (int)X < lastX - 1;
                case Direction.Up:
                    return (int)Y < lastY - 1;
                case Direction.Down:
                    return (int)Y > lastY;
                default:
                    break;
            }


            return false;
        }




        #region Actions

        private void Reface()
        {
            switch (Facing)
            {
                case Direction.Right:
                    lastX++;
                    break;
                case Direction.Left:
                    lastX--;
                    break;
                case Direction.Up:
                    lastY--;
                    break;
                case Direction.Down:
                    lastY++;
                    break;
                default:
                    break;
            }
            X = lastX;
            y = lastY;
            if (MyTeam == Team.Team1)
                Facing = MapData.TeamOnePath[lastX, lastY];
            if (MyTeam == Team.Team2)
                Facing = MapData.TeamTwoPath[lastX, lastY];
            currentTile = MapData.TileData[lastX, lastY];

        }


        //Move in a given direction
        public void Move(Direction direct, float deltaTime)
        {
            //Update orientation
            float displacement = Speed * deltaTime;
            switch (direct)
            {
                case Direction.Right:
                    if (lastX + 2 <= MapData.TileData.GetLength(0))
                        if (MapData.TileData[lastX + 1, lastY].HasFlag(TileProperties.Blocked))
                        {
                            X = (int)(X + displacement);
                            Reface();
                        }
                        else
                            x += displacement;
                    break;
                case Direction.Left:
                    if (lastX >= 1)
                        if (MapData.TileData[lastX - 1, lastY].HasFlag(TileProperties.Blocked))
                        {
                            X = (int)(X - displacement + 1);
                            Reface();
                        }
                        else
                            x -= displacement;
                    break;
                case Direction.Up:
                    if (lastY >= 1)
                        if (MapData.TileData[lastX, lastY - 1].HasFlag(TileProperties.Blocked))
                        {
                            Y = (int)(Y - displacement + 1);
                            Reface();
                        }
                        else
                            Y -= displacement;
                    break;
                case Direction.Down:
                    if (lastY + 2 <= MapData.TileData.GetLength(1))
                        if (MapData.TileData[lastX, lastY + 1].HasFlag(TileProperties.Blocked))
                        {
                            Y = (int)(Y + displacement);
                            Reface();
                        }
                        else
                            Y += displacement;
                    break;
                default:
                    break;
            }
            
        }

        
        
        
        
        //Assault enemy TeamCore
        public void Strike(TeamCore other)
        {
            other.Health--;
        }


        //Assaults the other Creep dealing damage
        public void Strike(Creep other)
        {
            other.Health -= (this.Attack - (other.Armor / 2));
        }

        #endregion



    }
}
