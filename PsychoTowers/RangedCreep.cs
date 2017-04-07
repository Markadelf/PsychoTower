using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychoTowers
{
    public class RangedCreep: Creep
    {
        public Gun Weapon { get; set; }

        public RangedCreep(int health, float speed, int attack, int armor, Map map, Team team, int xParam, int yParam, float range): base(health, speed, attack, armor, map, team, xParam, yParam)
        {
            Team target = team;
            if (target == Team.Team1)
                target = Team.Team2;
            else
                target = Team.Team1;

            Weapon = new Gun(target, range, attack);
        }


        public override void Step(float deltaTime)
        {
            currentTile = MapData.TileData[(int)(X + .5f), (int)(Y + .5f)];



            if (MyTeam == Team.Team1 && CheckCollision(MapData.TeamTwoCore))
                Strike(MapData.TeamTwoCore);
            if (MyTeam == Team.Team2 && CheckCollision(MapData.TeamOneCore))
                Strike(MapData.TeamOneCore);


            

            if (Weapon.CanShoot(MapData, X, Y))
            {
                if (AttackTimer > 0)
                    AttackTimer -= deltaTime;
                else
                {
                    Weapon.Shoot(MapData, X, Y);
                    AttackTimer += .5f;
                }
            }
            else
            {
                for (float displacement = Speed * deltaTime; displacement > 0; displacement = Move(Facing, displacement))
                    if (DetermineReface())
                    {
                        Reface();
                    }
            }



        }








    }
}
