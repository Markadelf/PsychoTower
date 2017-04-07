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

        public float ShotTimer { get; set; }

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
            ShotTimer = 0;
        }

        public Tower(TowerType tower)
        {
            switch (tower)
            {
                case TowerType.Shooter:
                    Range = 0;
                    Aura = TileProperties.None;
                    Weapon = new Gun(Team.None, 5, 10);
                    ShotTimer = 0;
                    break;
                case TowerType.AttackUp:
                    Range = 1;
                    Aura = TileProperties.AttackBuff;
                    Weapon = null;
                    ShotTimer = 0;
                    break;
                case TowerType.AttackDown:
                    Range = 1;
                    Aura = TileProperties.AttackNerf;
                    Weapon = null;
                    ShotTimer = 0;
                    break;
                case TowerType.DefenseUp:
                    Range = 1;
                    Aura = TileProperties.ArmorBuff;
                    Weapon = null;
                    ShotTimer = 0;
                    break;
                case TowerType.DefenseDown:
                    Range = 1;
                    Aura = TileProperties.ArmorNerf;
                    Weapon = null;
                    ShotTimer = 0;
                    break;
                case TowerType.SpeedUp:
                    Range = 1;
                    Aura = TileProperties.SpeedBuff;
                    Weapon = null;
                    ShotTimer = 0;
                    break;
                case TowerType.SpeedDown:
                    Range = 1;
                    Aura = TileProperties.SpeedNerf;
                    Weapon = null;
                    ShotTimer = 0;
                    break;
                case TowerType.ExtraXP:
                    Range = 3;
                    Aura = TileProperties.DoubleXP;
                    Weapon = null;
                    ShotTimer = 0;
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

        public void Shoot(Map mapdata, int x, int y, float deltaTime)
        {
            if (Weapon != null)
            {
                ShotTimer -= deltaTime;
                if(ShotTimer <= 0)
                {
                    Weapon.Shoot(mapdata, x, y);
                    ShotTimer += 1f;
                }

            }
        }







    }
}
