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
        public int LastX { get; set; }
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
        public int LastY { get; set; }
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

        public Creep Target;

        //Timer that handles attacking
        private float attackTimer;

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
                if (health <= 0)
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
                    return speed + 1;
                if (currentTile.HasFlag(TileProperties.SpeedNerf) && speed > 1)
                    return speed - 1;
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
                    return attack + 10;
                if (currentTile.HasFlag(TileProperties.AttackNerf) && attack > 1)
                    return attack - 10;
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
                    return armor + 10;
                if (currentTile.HasFlag(TileProperties.ArmorNerf))
                    return armor - 10;
                return armor;
            }
            private set { armor = value; }
        }

        #endregion


        public Creep(int health, float speed, int attack, int armor, Map map, Team team, int xParam, int yParam)
        {
            Alive = true;
            MaxHealth = health;
            this.health = health;
            Speed = speed;
            Attack = attack;
            Armor = armor;
            MapData = map;
            MyTeam = team;
            x = xParam;
            y = yParam;
            LastX = (int)x;
            LastY = (int)y;
            Reface();
            Target = null;
        }




        public void Step(float deltaTime)
        {
            currentTile = MapData.TileData[(int) (X + .5f), (int) (Y + .5f)];

            

            if (MyTeam == Team.Team1 && CheckCollision(MapData.TeamTwoCore))
                Strike(MapData.TeamTwoCore);
            if (MyTeam == Team.Team2 && CheckCollision(MapData.TeamOneCore))
                Strike(MapData.TeamOneCore);


            for(float displacement = Speed * deltaTime; displacement > 0; displacement = Move(Facing, displacement))
                if (DetermineReface())
                {
                    Reface();
                }

            
            if (Target != null)
            {
                if (attackTimer > 0)
                    attackTimer -= deltaTime;
                else
                {
                    Strike(Target);
                    if (!Target.Alive)
                    {

                        attack += Target.attack/10;
                        speed += Target.speed / 10;
                        armor += Target.armor / 10;
                        if (currentTile.HasFlag(TileProperties.DoubleXP))
                        {
                            attack += Target.attack / 10;
                            speed += Target.speed / 10;
                            armor += Target.armor / 10;
                        }
                        Target = null;
                        if (Speed > 2075.1f)
                            Speed = 2075.1f;
                    }
                    attackTimer += .5f;
                }
                    
            }
            

        }


        #region Logic


        private bool DetermineReface()
        {
            switch (Facing)
            {
                case Direction.Right:
                    return (int)X > LastX;
                case Direction.Left:
                    return (int)X < LastX - 1;
                case Direction.Up:
                    return (int)Y < LastY - 1;
                case Direction.Down:
                    return (int)Y > LastY;
                case Direction.None:
                    return false;
            }
            return false;
        }

        public bool CheckCollision(Creep other, float displacement)
        {
            if (other == this)
                return false;
            switch (Facing)
            {
                case Direction.None:
                    return false;
                case Direction.Right:
                    if (X + displacement + 1 < other.X || X > other.X)
                        return false;
                    if (Y >= other.Y + 1)
                        return false;
                    if (Y + 1 <= other.Y)
                        return false;
                    return true;
                case Direction.Left:
                    if (X - displacement > other.X + 1|| X < other.X)
                        return false;
                    if (Y >= other.Y + 1)
                        return false;
                    if (Y + 1 <= other.Y)
                        return false;
                    return true;
                case Direction.Up:
                    if (Y - displacement > other.Y + 1|| Y < other.Y)
                        return false;
                    if (X >= other.X + 1)
                        return false;
                    if (X + 1 <= other.X)
                        return false;
                    return true;
                case Direction.Down:
                    if (Y + displacement + 1 < other.Y || Y > other.Y)
                        return false;
                    if (X >= other.X + 1)
                        return false;
                    if (X + 1 <= other.X)
                        return false;
                    return true;
                default:
                    return false;
            }
        }

        public bool CheckCollision(TeamCore other)
        {
            switch (Facing)
            {
                case Direction.None:
                    if (X >= other.X + 1)
                        return false;
                    if (X + 1 <= other.X)
                        return false;
                    if (Y >= other.Y + 1)
                        return false;
                    if (Y + 1 <= other.Y)
                        return false;
                    return true;
                case Direction.Right:
                    if (X + .1 + 1 <= other.X || X > other.X)
                        return false;
                    if (Y >= other.Y + 1)
                        return false;
                    if (Y + 1 <= other.Y)
                        return false;
                    return true;
                case Direction.Left:
                    if (X - .1 >= other.X + 1 || X < other.X)
                        return false;
                    if (Y >= other.Y + 1)
                        return false;
                    if (Y + 1 <= other.Y)
                        return false;
                    return true;
                case Direction.Up:
                    if (Y - .1 >= other.Y + 1 || Y < other.Y)
                        return false;
                    if (X >= other.X + 1)
                        return false;
                    if (X + 1 <= other.X)
                        return false;
                    return true;
                case Direction.Down:
                    if (Y + .1 + 1 <= other.Y || Y > other.Y)
                        return false;
                    if (X >= other.X + 1)
                        return false;
                    if (X + 1 <= other.X)
                        return false;
                    return true;
                default:
                    return false;
            }
        }




        #endregion

        #region Actions

        private void SnapToGrid()
        {
            X = (int)(X * 10 + .5f) / 10.0f;
            Y = (int)(Y * 10 + .5f) / 10.0f;
        }




        /// <summary>
        /// Turns the creep if neccesary
        /// </summary>
        private void Reface()
        {
            Direction newFace = Direction.None;
            if (this.DetermineReface())
                switch (Facing)
                {
                    case Direction.Right:
                        LastX++;
                        break;
                    case Direction.Left:
                        LastX--;
                        break;
                    case Direction.Up:
                        LastY--;
                        break;
                    case Direction.Down:
                        LastY++;
                        break;
                    default:
                        break;
                }
            SnapToGrid();
            if (MyTeam == Team.Team1)
                newFace = MapData.TeamOnePath[LastX, LastY];
            if (MyTeam == Team.Team2)
                newFace = MapData.TeamTwoPath[LastX, LastY];
            if(Facing != newFace)
            {
                Facing = newFace;
                X = LastX;
                Y = LastY;
            }
            currentTile = MapData.TileData[LastX, LastY];
            if (DetermineReface())
                Reface();
        }

        //Move in a given direction
        public float Move(Direction direct, float displacement)
        {
            

            for (int i = 0; i < MapData.TeamOne.Count; i++)
            {
                if(CheckCollision(MapData.TeamOne[i], displacement))
                {
                    if (MyTeam != Team.Team1)
                    {
                        Target = MapData.TeamOne[i];
                        Target.Target = this;
                    }
                    switch (Facing)
                    {
                        case Direction.None:
                            break;
                        case Direction.Right:
                            X = MapData.TeamOne[i].X - 1;
                            break;
                        case Direction.Left:
                            X = MapData.TeamOne[i].X + 1;
                            break;
                        case Direction.Up:
                            Y = MapData.TeamOne[i].Y + 1;
                            break;
                        case Direction.Down:
                            Y = MapData.TeamOne[i].Y - 1;
                            break;
                        default:
                            break;
                    }
                    return 0;
                }
            }
            for (int i = 0; i < MapData.TeamTwo.Count; i++)
            {
                if (CheckCollision(MapData.TeamTwo[i], displacement))
                {
                    if (MyTeam != Team.Team2)
                    {
                        Target = MapData.TeamTwo[i];
                        Target.Target = this;
                    }
                    switch (Facing)
                    {
                        case Direction.None:
                            break;
                        case Direction.Right:
                            X = MapData.TeamTwo[i].X - 1;
                            break;
                        case Direction.Left:
                            X = MapData.TeamTwo[i].X + 1;
                            break;
                        case Direction.Up:
                            Y = MapData.TeamTwo[i].Y + 1;
                            break;
                        case Direction.Down:
                            Y = MapData.TeamTwo[i].Y - 1;
                            break;
                        default:
                            break;
                    }
                    return 0;
                }
            }
            switch (direct)
            {
                case Direction.Right:
                    if (displacement < 1)
                    {
                        X += displacement;
                        return 0;
                    }
                    else
                    {
                        X++;
                        return displacement - 1;
                    }
                case Direction.Left:
                    if (displacement < 1)
                    {
                        X -= displacement;
                        return 0;
                    }
                    else
                    {
                        X--;
                        return displacement - 1;
                    }
                case Direction.Up:
                    if (displacement < 1)
                    {
                        Y -= displacement;
                        return 0;
                    }
                    else
                    {
                        Y--;
                        return displacement - 1;
                    }
                case Direction.Down:
                    if (displacement < 1)
                    {
                        Y += displacement;
                        return 0;
                    }
                    else
                    {
                        Y++;
                        return displacement - 1;
                    }
                default:
                    Reface();
                    break;
            }
            return 0;
            
        }

      


        //Assault enemy TeamCore
        public void Strike(TeamCore other)
        {
            other.Health--;
        }


        //Assaults the other Creep dealing damage
        public void Strike(Creep other)
        {
            int damage = (Attack - (other.Armor));
            if(damage > 10)
            {
                if(Game1.Rand.Next(10) == 0)
                    other.Health -= 2 * damage;
                else
                    other.Health -= damage;
            }
            else
                other.Health -= 10;

        }

        #endregion



    }
}
