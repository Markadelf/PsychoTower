using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychoTowers
{



    public class Player
    {

        #region ActionCosts
        private const int WALLPLACECOST = 50;
        private const int WALLREMOVECOST = 75;
        private const int TOWERPLACECOST = 100;
        private const int TOWERREMOVECOST = 200;


        #endregion






        public Map MapData { get; set; }
        public int Gold { get; set; }
        public InputMethod PlayerInput { get; set; }
        public Player(InputMethod input, Map mapdata)
        {
            Gold = 500;
            PlayerInput = input;
            PlayerInput.Act += Act;
            MapData = mapdata;
        }

        public void Step(int income)
        {
            Gold += income;
        }

        public void Act(Command command, int args)
        {
            switch (command)
            {
                case Command.None:
                    break;
                case Command.PlaceWall:
                    if (Gold >= WALLPLACECOST)
                    {
                        if(MapData.PlaceBlock(PlayerInput.X, PlayerInput.Y))
                            Gold -= WALLPLACECOST;
                    }
                    break;
                case Command.PlaceTower:
                    if(args < 8)
                    {
                        TowerType tp = (TowerType) args;
                        if(Gold >= TOWERPLACECOST)
                        {
                            if (MapData.PlaceTower(PlayerInput.X, PlayerInput.Y, new Tower(tp)))
                                Gold -= TOWERPLACECOST;
                        }
                    }
                    break;
                case Command.RemoveAt:
                    //Is it a tower?
                    if (PlayerInput.X / 2 != PlayerInput.X / 2.0 && PlayerInput.Y / 2 == PlayerInput.Y / 2.0)
                    {
                        if (Gold >= TOWERREMOVECOST && MapData.RemoveTower(PlayerInput.X, PlayerInput.Y))
                            Gold -= TOWERREMOVECOST;
                    }
                    //Is it a wall?
                    else if (!(PlayerInput.X < 2 || PlayerInput.X > MapData.TileData.GetLength(0) - 3 || PlayerInput.Y == 0 || PlayerInput.Y == MapData.TileData.GetLength(1) - 1))
                    {
                        if (Gold >= WALLREMOVECOST && MapData.RemoveBlock(PlayerInput.X, PlayerInput.Y))
                            Gold -= WALLREMOVECOST;
                    }
                        break;
                default:
                    break;
            }
        }





    }
}
