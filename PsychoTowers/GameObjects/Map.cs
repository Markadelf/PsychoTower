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
        HealingAura = 128
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
        public TeamCore TeamCoreOne { get; set; }
        public TeamCore TeamCoreTwo { get; set; }




    }
}
