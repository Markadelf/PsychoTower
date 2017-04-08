using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychoTowers
{
    /// <summary>
    /// This enum holds all of the info on auras and effects that effect a given tile.
    /// </summary>
    [Flags] public enum TileProperties
    {
        None = 0,
        Blocked = 1,
        AttackBuff = 2,
        AttackNerf = 4,
        ArmorBuff = 8,
        ArmorNerf = 16,
        SpeedBuff = 32,
        SpeedNerf = 64,
        DoubleXP = 128
    }


    /// <summary>
    /// This class holds all of the essential game info and runs the game.
    /// </summary>
    public class Map
    {
        //Tile Info
        public TileProperties[,] TileData{ get; set; }
        //Tower info
        public Tower[,] TowerData { get; set; }

        //Team Rosters
        public List<Creep> TeamOne { get; set; }
        public List<Creep> TeamTwo { get; set; }
        //Team Cores
        public TeamCore TeamOneCore { get; set; }
        public TeamCore TeamTwoCore { get; set; }
        //TeamPath
        public Direction[,] TeamOnePath { get; set; }
        public Direction[,] TeamTwoPath { get; set; }

        //Projectiles
        public List<Projectile> Projectiles { get; set; }




        //Constructor
        public Map()
        {
            //Set up map tiles
            TileData = new TileProperties[21, 19];
            for(int i = 0; i < TileData.GetLength(0); i++)
            {
                for(int j = 0; j < TileData.GetLength(1); j++)
                {
                    if((i / 2 != i / 2.0 && j / 2 == j / 2.0) || 
                        ((i <= 1 || i >= TileData.GetLength(0) - 2) && (j != TileData.GetLength(1) / 2 || i == 0 || i == TileData.GetLength(0) - 1)) || (j == 0) || (j == TileData.GetLength(1) - 1))
                        TileData[i, j] = TileProperties.Blocked;
                    else
                        TileData[i, j] = TileProperties.None;

                }
            }




            //Set Up towers
            TowerData = new Tower[8,8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    TowerData[i, j] = null;//new Tower((TowerType) Game1.Rand.Next(8));
                }
            }

            //Set up Teams
            TeamOne = new List<Creep>();
            TeamTwo = new List<Creep>();
            TeamOneCore = new TeamCore(1, 9, Team.Team1, this);
            TeamTwoCore = new TeamCore(19, 9, Team.Team2, this);
            RedefinePaths();
            Projectiles = new List<Projectile>();
            
            //ReApplyAuras();


        }//End Constructor


        public void Step(float deltaTime)
        {
            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectiles[i].Step(deltaTime);
                if (!Projectiles[i].Alive)
                {
                    Projectiles.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (TowerData[i, j] != null)
                        TowerData[i, j].Shoot(this, (2 * i + 3), (j * 2 + 2), deltaTime);
                }
            }

            for (int i = 0; i < TeamOne.Count; i++)
            {
                TeamOne[i].Step(deltaTime);
                if (!TeamOne[i].Alive)
                {
                    TeamOne.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < TeamTwo.Count; i++)
            {
                TeamTwo[i].Step(deltaTime);
                if (!TeamTwo[i].Alive)
                {
                    TeamTwo.RemoveAt(i);
                    i--;
                }
            }
            TeamCore.RespawnTimer -= deltaTime;
            if (TeamCore.RespawnTimer <= 0)
            {
                if (TeamCore.Level < 144)
                    TeamCore.Level++;
                TeamCore.RespawnTimer += 5;
                bool spawnSafe = true;
                for (int i = 0; i < TeamOne.Count; i++)
                {
                    if (TeamOne[i].CheckCollision(TeamOneCore))
                    {
                        spawnSafe = false;
                    }
                }
                if (spawnSafe)
                    TeamOne.Add(TeamOneCore.NewCreep());
                
                spawnSafe = true;
                for (int i = 0; i <TeamTwo.Count; i++)
                {
                    if (TeamTwo[i].CheckCollision(TeamTwoCore))
                    {
                        spawnSafe = false;
                    }
                }
                if(spawnSafe)
                    TeamTwo.Add(TeamTwoCore.NewCreep());
            }

            ReApplyAuras();



        }


     




        #region Commands

        public bool PlaceBlock(int x, int y)
        {
            if (x < 0 || y < 0 || x >= TileData.GetLength(0) || y >= TileData.GetLength(1))
                return false;
            if (TileData[x, y].HasFlag(TileProperties.Blocked) || (x == TeamOneCore.X && y == TeamOneCore.Y) || (x == TeamTwoCore.X && y == TeamTwoCore.Y))
                return false;
            for (int i = 0; i < TeamOne.Count; i++)
            {
                if (TeamOne[i].CheckCollision(x,y))
                    return false;
            }
            for (int i = 0; i < TeamTwo.Count; i++)
            {
                if (TeamTwo[i].CheckCollision(x, y))
                    return false;
            }

            if (TryBlock(x, y))
            {
                return true;
            }
            else
                return false;
        }

        public bool RemoveBlock(int x, int y)
        {
            if (!TileData[x, y].HasFlag(TileProperties.Blocked))
                return false;
            else
            {
                TileData[x, y] -= TileProperties.Blocked;
                RedefinePaths();
                return true;
            }

        }

        public bool PlaceTower(int x, int y, Tower t)
        {
            if (x < 0 || y < 0 || x >= TileData.GetLength(0) || y >= TileData.GetLength(1))
                return false;
            if (x / 2 != x / 2.0 && y / 2 == y / 2.0 && x >= 2 && x <= 17 && y >= 2 && y <= 16)
            {
                if (TowerData[(x / 2) - 1, (y / 2) - 1] != null)
                    return false;
                else
                {
                    TowerData[(x / 2) - 1, (y / 2) - 1] = t;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveTower(int x, int y)
        {
            if (x < 0 || y < 0 || x >= TileData.GetLength(0) || y >= TileData.GetLength(1))
                return false;
            if (x / 2 != x / 2.0 && y / 2 == y / 2.0 && x >= 3 && x <= 17 && y >= 2 && y <= 16)
            {
                if (TowerData[(x / 2) - 1, (y / 2 ) - 1] == null)
                    return false;
                else
                {
                    TowerData[(x / 2) - 1, (y / 2) - 1] = null;
                    return true;
                }
            }
                return false;
            

        }

        public void ReApplyAuras()
        {
            //Remove auras
            for(int i = 0; i < TileData.GetLength(0); i++)
            {
                for(int j = 0; j < TileData.GetLength(1); j++)
                {
                    TileData[i, j] = TileData[i, j] & TileProperties.Blocked;
                }
            }

            //Reapply auras
            for (int i = 0; i < TowerData.GetLength(0); i++)
            {
                for (int j = 0; j < TowerData.GetLength(1); j++)
                {
                    if (TowerData[i, j] != null)
                        TowerData[i, j].ApplyAura(this, (2 * i + 3), (j * 2 + 2));
                }
            }
        }

        #endregion


        #region Pathfinding

        //Fixes all paths, false if no path exists
        public bool RedefinePaths()
        {
            if(CalculatePath(Team.Team1) && CalculatePath(Team.Team2))
            {
                EnsureHomoginity();
                return true;
            }
            else
                return false;
        }

        //Checks if blocking a given square would be a legal move
        private bool TryBlock(int x, int y)
        {
            TileData[x, y] += (int)TileProperties.Blocked;
            TeamCore target = TeamTwoCore;
            Direction[,] path = new Direction[TileData.GetLength(0), TileData.GetLength(1)];
            int[,] count = new int[TileData.GetLength(0), TileData.GetLength(1)];
            int max = TileData.GetLength(0) * TileData.GetLength(1);
            for (int i = 0; i < path.GetLength(0); i++)
            {
                for (int j = 0; j < path.GetLength(1); j++)
                {
                    path[i, j] = Direction.None;
                    count[i, j] = max;
                }
            }

            count[target.X, target.Y] = 0;

            RecursePath(path, count, target.X, target.Y);
            if (count[TeamOneCore.X, TeamOneCore.Y] != max)
            {
                RedefinePaths();
                return true;
            }
            TileData[x, y] -= (int)TileProperties.Blocked;
            return false;
        }

        //Calculates the path for a given team False if no path exists
        public bool CalculatePath(Team team)
        {
            TeamCore target;
            if (team == Team.Team1)
                target = TeamTwoCore;
            else if (team == Team.Team2)
                target = TeamOneCore;
            else
                return false;
            Direction[,] path = new Direction[TileData.GetLength(0), TileData.GetLength(1)];
            int[,] count = new int[TileData.GetLength(0), TileData.GetLength(1)];
            int max = TileData.GetLength(0) * TileData.GetLength(1);
            for (int i = 0; i < path.GetLength(0); i++)
            {
                for (int j = 0; j < path.GetLength(1); j++)
                {
                    path[i, j] = Direction.None;
                    count[i,j] = max;
                }
            }

            count[target.X, target.Y] = 0;

            RecursePath(path, count, target.X, target.Y);
            if(team == Team.Team1)
            {
                if(count[TeamOneCore.X, TeamOneCore.Y] != max)
                {
                    TeamOnePath = path;
                    return true;
                }
            }
            else
            {
                if (count[TeamTwoCore.X, TeamTwoCore.Y] != max)
                {
                    TeamTwoPath = path;
                    return true;
                }
            }
            return false;
        }

        //A recursive function that finds the shortest path from to any given square
        private void RecursePath(Direction[,] path, int[,] count, int x, int y)
        {
            //Check Up
            if (count[x, y] + 1 < count[x , y - 1] && !TileData[x, y - 1].HasFlag(TileProperties.Blocked))
            {
                path[x, y - 1] = Direction.Down;
                count[x, y - 1] = count[x, y] + 1;
                RecursePath(path, count, x, y - 1);
            }
            //Check Down
            if (count[x, y] + 1 < count[x, y + 1] && !TileData[x, y + 1].HasFlag(TileProperties.Blocked))
            {
                path[x, y + 1] = Direction.Up;
                count[x, y + 1] = count[x, y] + 1;
                RecursePath(path, count, x, y + 1);
            }
            //Check Left
            if(count[x,y] + 1 < count[x - 1, y] && !TileData[x - 1, y].HasFlag(TileProperties.Blocked))
            {
                path[x - 1, y] = Direction.Right;
                count[x - 1, y] = count[x, y] + 1;
                RecursePath(path, count, x - 1, y);
            }
            //Check Right
            if (count[x, y] + 1 < count[x + 1, y] && !TileData[x + 1, y].HasFlag(TileProperties.Blocked))
            {
                path[x + 1, y] = Direction.Left;
                count[x + 1, y] = count[x, y] + 1;
                RecursePath(path, count, x + 1, y);
            }
            
        }

        private void EnsureHomoginity()
        {
            int x = TeamOneCore.X;
            int y = TeamOneCore.Y;
            while(x != TeamTwoCore.X || y != TeamTwoCore.Y)
            {
                switch (TeamOnePath[x,y])
                {
                    case Direction.Right:
                        x++;
                        TeamTwoPath[x, y] = Direction.Left;
                        break;
                    case Direction.Left:
                        x--;
                        TeamTwoPath[x, y] = Direction.Right;
                        break;
                    case Direction.Up:
                        y--;
                        TeamTwoPath[x, y] = Direction.Down;
                        break;
                    case Direction.Down:
                        y++;
                        TeamTwoPath[x, y] = Direction.Up;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion







    }//End of class



}
