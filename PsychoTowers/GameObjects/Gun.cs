using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychoTowers
{
    public class Gun
    {
        public Team TargetTeam { get; set; }
        public Team LastTeamShot { get; set; }
        public float Range { get; set; }
        public int Attack { get; set; }
        public float ShotVelocity { get; set; }

        public Gun(Team target, float range, int attack)
        {
            TargetTeam = target;
            Range = range;
            Attack = attack;
            LastTeamShot = target;
        }




        public void Shoot(Map mapdata, float x, float y)
        {
            List<Creep> targets = null;
            switch (TargetTeam)
            {
                case Team.None:
                    switch (LastTeamShot)
                    {
                        case Team.None:
                            if (Game1.Rand.Next(2) == 0)
                            {
                                targets = mapdata.TeamTwo;
                                LastTeamShot = Team.Team2;
                            }
                            else
                            {
                                targets = mapdata.TeamOne;
                                LastTeamShot = Team.Team1;
                            }
                            break;
                        case Team.Team1:
                            targets = mapdata.TeamTwo;
                            LastTeamShot = Team.Team2;
                            break;
                        case Team.Team2:
                            targets = mapdata.TeamOne;
                            LastTeamShot = Team.Team1;
                            break;
                        default:
                            break;
                    }
                    break;
                case Team.Team1:
                    targets = mapdata.TeamOne;
                    break;
                case Team.Team2:
                    targets = mapdata.TeamTwo;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                double distSquared = Math.Pow(targets[i].X - x, 2) + Math.Pow(targets[i].Y - y, 2);
                if (Range * Range >= distSquared)
                {
                    mapdata.Projectiles.Add(new Projectile(x, y, targets[i], Attack));
                    return;
                }
            }

            if (TargetTeam == Team.None)
            {
                switch (LastTeamShot)
                {
                    case Team.Team1:
                        targets = mapdata.TeamTwo;
                        LastTeamShot = Team.Team2;
                        break;
                    case Team.Team2:
                        targets = mapdata.TeamOne;
                        LastTeamShot = Team.Team1;
                        break;
                    default:
                        break;
                }
                for (int i = 0; i < targets.Count; i++)
                {
                    double distSquared = Math.Pow(targets[i].X - x, 2) + Math.Pow(targets[i].Y - y, 2);
                    if (Range * Range >= distSquared)
                    {
                        mapdata.Projectiles.Add(new Projectile(x, y, targets[i], Attack));
                        return;
                    }
                }
            }



        }

        public bool CanShoot(Map mapdata, float x, float y)
        {
            List<Creep> targets = null;
            switch (TargetTeam)
            {
                case Team.None:
                    switch (LastTeamShot)
                    {
                        case Team.None:
                            if (Game1.Rand.Next(2) == 0)
                            {
                                targets = mapdata.TeamTwo;
                                LastTeamShot = Team.Team2;
                            }
                            else
                            {
                                targets = mapdata.TeamOne;
                                LastTeamShot = Team.Team1;
                            }
                            break;
                        case Team.Team1:
                            targets = mapdata.TeamTwo;
                            LastTeamShot = Team.Team2;
                            break;
                        case Team.Team2:
                            targets = mapdata.TeamOne;
                            LastTeamShot = Team.Team1;
                            break;
                        default:
                            break;
                    }
                    break;
                case Team.Team1:
                    targets = mapdata.TeamOne;
                    break;
                case Team.Team2:
                    targets = mapdata.TeamTwo;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                double distSquared = Math.Pow(targets[i].X - x, 2) + Math.Pow(targets[i].Y - y, 2);
                if (Range * Range >= distSquared)
                {
                    return true;
                }
            }

            if (TargetTeam == Team.None)
            {
                switch (LastTeamShot)
                {
                    case Team.Team1:
                        targets = mapdata.TeamTwo;
                        LastTeamShot = Team.Team2;
                        break;
                    case Team.Team2:
                        targets = mapdata.TeamOne;
                        LastTeamShot = Team.Team1;
                        break;
                    default:
                        break;
                }
                for (int i = 0; i < targets.Count; i++)
                {
                    double distSquared = Math.Pow(targets[i].X - x, 2) + Math.Pow(targets[i].Y - y, 2);
                    if (Range * Range >= distSquared)
                    {
                        return true;
                    }
                }
            }
            return false;
        }



    }//End of class


}
