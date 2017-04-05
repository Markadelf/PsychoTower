using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychoTowers
{
    public enum TowerType
    {
        Shooter,
        AttackUp,
        AttackDown,
        DefenseUp,
        DefenseDown,
        SpeedUp,
        SpeedDown,
        ExtraXP
    }


    public class Tower
    {
        //Square radius around the Tower
        public int Range { get; set; }

        //Aura around the tower
        public TileProperties Aura { get; set; }

        public Gun Weapon { get; set; }

        /// <summary>
        /// Makes a tower that expells an aura at a range and uses a gun.
        /// </summary>
        /// <param name="range">Range the aura extends in tiles in a square shape.</param>
        /// <param name="aura">The aura that is extended by the tower.</param>
        /// <param name="gun">The ranged weapon the tower uses. If it is null, the tower does not shoot.</param>
        public Tower(int range, TileProperties aura, Gun gun)
        {
            Range = range;
            Aura = aura;
            Weapon = gun;
        }

        public Tower(TowerType tower)
        {
            switch (tower)
            {
                case TowerType.Shooter:
                    Range = 0;
                    Aura = TileProperties.None;
                    Weapon = null;
                    break;
                case TowerType.AttackUp:
                    Range = 1;
                    Aura = TileProperties.AttackBuff;
                    Weapon = null;
                    break;
                case TowerType.AttackDown:
                    Range = 5;
                    Aura = TileProperties.AttackNerf;
                    Weapon = null;
                    break;
                case TowerType.DefenseUp:
                    Range = 1;
                    Aura = TileProperties.ArmorBuff;
                    Weapon = null;
                    break;
                case TowerType.DefenseDown:
                    Range = 5;
                    Aura = TileProperties.ArmorNerf;
                    Weapon = null;
                    break;
                case TowerType.SpeedUp:
                    Range = 3;
                    Aura = TileProperties.SpeedBuff;
                    Weapon = null;
                    break;
                case TowerType.SpeedDown:
                    Range = 7;
                    Aura = TileProperties.SpeedNerf;
                    Weapon = null;
                    break;
                case TowerType.ExtraXP:
                    Range = 4;
                    Aura = TileProperties.DoubleXP;
                    Weapon = null;
                    break;
                default:
                    break;
            }
        }



        /// <summary>
        /// Expell aura onto the map from the tower
        /// </summary>
        /// <param name="mapdata">The map to apply it to</param>
        /// <param name="x">Center X</param>
        /// <param name="y">Center Y</param>
        public void ApplyAura(Map mapdata, int x, int y)
        {
            for(int i = x - Range; i <= x + Range && i < mapdata.TileData.GetLength(0); i++)
            {
                if (i < 0)
                    i = 0;
                for (int j = y - Range; j <= y + Range && j < mapdata.TileData.GetLength(1); j++)
                {
                    if (j < 0)
                        j = 0;
                    mapdata.TileData[i, j] = mapdata.TileData[i, j] | Aura;
                }
            }
        }

        public void Shoot(Map mapdata, int x, int y)
        {
            if (Weapon != null)
                Weapon.Shoot(mapdata, x, y);
        }







    }
}
