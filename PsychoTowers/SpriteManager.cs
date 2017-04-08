using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PsychoTowers
{
    public class SpriteManager
    {

        #region Textures
        public static Texture2D SquareTestTexture { get; set; }
        public static Texture2D BorderTexture { get; set; }
        public static Texture2D WallTexture { get; set; }

        public static Texture2D CreepUpTexture { get; set; }
        public static Texture2D CreepDownTexture { get; set; }
        public static Texture2D CreepRightTexture { get; set; }
        public static Texture2D CreepLeftTexture { get; set; }

        public static Texture2D AuraTexture { get; set; }
        public static Texture2D RangeTexture { get; set; }

        public static Texture2D BackgroundTexture { get; set; }
        public static Texture2D BacklightTexture { get; set; }

        public static Texture2D EmptyTowerSlotTexture { get; set; }
        public static Texture2D ShootTowerTexture { get; set; }
        public static Texture2D BuffTowerTexture { get; set; }
        public static Texture2D NerfTowerTexture { get; set; }
        public static Texture2D ExpTowerTexture { get; set; }


        public static Texture2D TeamCoreTexture { get; set; }


        #endregion

        #region Timer
        public static float FlagTimer { get; set; }

        public static float CreepTimer { get; set; }

        #endregion

        #region singleton



        //Singleton Structure
        private static SpriteManager screen = new SpriteManager();
        public static SpriteManager Screen
        {
            get
            {
                if (screen == null)
                    screen = new SpriteManager();
                return screen;
            }
        }
        private SpriteManager()
        {
            FlagTimer = 0;
            CreepTimer = 0;
        }
        #endregion

        //Info for drawing game screen
        public const int DrawMapX  = 12;
        public const int DrawMapWidth  = 21 * 24;
        public const int DrawMapY  = 12;
        public const int DrawMapHeight  = 19 * 24;
        public const int DrawMapScale = 24;

        public const int DrawPanelX = 516;
        public const int DrawPanelWidth = 21 * DrawMapScale;
        public const int DrawPanelY = 12;
        public const int DrawPanelHeight = 19 * 24;

        public static void Update(float deltaTime)
        {
            FlagTimer += deltaTime;
            CreepTimer += deltaTime;
            if (FlagTimer > .66f)
                FlagTimer -= .66f;
            if (CreepTimer > .5f)
                CreepTimer -= .5f;
        }




        public static void DrawMap(SpriteBatch sb, Map mapdata)
        {
            //Draw background to board
            sb.Draw(BackgroundTexture, destinationRectangle: new Rectangle(DrawMapX, DrawMapY, DrawMapWidth, DrawMapHeight), layerDepth: 0, color: Color.SandyBrown);
            sb.Draw(BorderTexture, destinationRectangle: new Rectangle(DrawMapX, DrawMapY, DrawMapWidth, DrawMapHeight), layerDepth: 1, color: Color.SandyBrown);
            sb.Draw(BacklightTexture, destinationRectangle: new Rectangle(0, 0, 720, 480), layerDepth: .99f, color: new Color(160, 140, 120));


            //Draw Path
            //for (int i = 0; i < mapdata.TileData.GetLength(0); i++)
            //{
            //    for (int j = 0; j < mapdata.TileData.GetLength(1); j++)
            //    {
            //        switch (mapdata.TeamTwoPath[i, j])
            //        {
            //            case Direction.Right:
            //                sb.Draw(SquareTestTexture, new Rectangle(DrawMapX + i * DrawMapScale, DrawMapY + j * DrawMapScale, DrawMapScale, DrawMapScale), Color.Red);
            //                break;
            //            case Direction.Left:
            //                sb.Draw(SquareTestTexture, new Rectangle(DrawMapX + i * DrawMapScale, DrawMapY + j * DrawMapScale, DrawMapScale, DrawMapScale), Color.Blue);
            //                break;
            //            case Direction.Up:
            //                sb.Draw(SquareTestTexture, new Rectangle(DrawMapX + i * DrawMapScale, DrawMapY + j * DrawMapScale, DrawMapScale, DrawMapScale), Color.Green);
            //                break;
            //            case Direction.Down:
            //                sb.Draw(SquareTestTexture, new Rectangle(DrawMapX + i * DrawMapScale, DrawMapY + j * DrawMapScale, DrawMapScale, DrawMapScale), Color.HotPink);
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //}

            //Draw Walls
            for (int i = 0; i < mapdata.TileData.GetLength(0); i++)
            {
                for (int j = 0; j < mapdata.TileData.GetLength(1); j++)
                {
                    if (mapdata.TileData[i, j].HasFlag(TileProperties.Blocked))
                    {
                        if (i < 2 || i > mapdata.TileData.GetLength(0) - 3 || j == 0 || j == mapdata.TileData.GetLength(1) - 1)
                            sb.Draw(WallTexture, destinationRectangle: new Rectangle(DrawMapX + i * DrawMapScale, DrawMapY + j * DrawMapScale, DrawMapScale, DrawMapScale), 
                                color: Color.Black, layerDepth: j / 100f);
                        else if (!(i / 2 != i / 2.0 && j / 2 == j / 2.0))
                            sb.Draw(WallTexture, destinationRectangle: new Rectangle(DrawMapX + i * DrawMapScale, DrawMapY + j * DrawMapScale - 8, DrawMapScale, DrawMapScale + 8), 
                                color: Color.White, layerDepth: j / 100f);
                    }
                }
            }





            

            //Draw Team Cores
            if(FlagTimer > .44f)
            {
                sb.Draw(TeamCoreTexture, sourceRectangle: new Rectangle(48, 0, 24, 24), destinationRectangle: new Rectangle(DrawMapX + (int)(mapdata.TeamOneCore.X * DrawMapScale), DrawMapY + (int)(mapdata.TeamOneCore.Y * DrawMapScale) + 4,
                DrawMapScale, DrawMapScale), color: Color.Red, layerDepth: (mapdata.TeamOneCore.Y / 100f));
                sb.Draw(TeamCoreTexture, sourceRectangle: new Rectangle(48, 0, 24, 24), destinationRectangle: new Rectangle(DrawMapX + (int)(mapdata.TeamTwoCore.X * DrawMapScale), DrawMapY + (int)(mapdata.TeamTwoCore.Y * DrawMapScale) + 4,
                    DrawMapScale, DrawMapScale), color: Color.Blue, layerDepth: (mapdata.TeamTwoCore.Y / 100f));
            }
            else if(FlagTimer > .22f)
            {
                sb.Draw(TeamCoreTexture, sourceRectangle: new Rectangle(24, 0, 24, 24), destinationRectangle: new Rectangle(DrawMapX + (int)(mapdata.TeamOneCore.X * DrawMapScale), DrawMapY + (int)(mapdata.TeamOneCore.Y * DrawMapScale) + 4,
                   DrawMapScale, DrawMapScale), color: Color.Red, layerDepth: (mapdata.TeamOneCore.Y / 100f));
                sb.Draw(TeamCoreTexture, sourceRectangle: new Rectangle(24, 0, 24, 24), destinationRectangle: new Rectangle(DrawMapX + (int)(mapdata.TeamTwoCore.X * DrawMapScale), DrawMapY + (int)(mapdata.TeamTwoCore.Y * DrawMapScale) + 4,
                    DrawMapScale, DrawMapScale), color: Color.Blue, layerDepth: (mapdata.TeamTwoCore.Y / 100f));
            }
            else
            {
                sb.Draw(TeamCoreTexture, sourceRectangle: new Rectangle(0, 0, 24, 24), destinationRectangle: new Rectangle(DrawMapX + (int)(mapdata.TeamOneCore.X * DrawMapScale), DrawMapY + (int)(mapdata.TeamOneCore.Y * DrawMapScale) + 4,
                    DrawMapScale, DrawMapScale), color: Color.Red, layerDepth: (mapdata.TeamOneCore.Y / 100f));
                sb.Draw(TeamCoreTexture, sourceRectangle: new Rectangle(0, 0, 24, 24), destinationRectangle: new Rectangle(DrawMapX + (int)(mapdata.TeamTwoCore.X * DrawMapScale), DrawMapY + (int)(mapdata.TeamTwoCore.Y * DrawMapScale) + 4,
                    DrawMapScale, DrawMapScale), color: Color.Blue, layerDepth: (mapdata.TeamTwoCore.Y / 100f));
            }



            //Draw Team Creeps
            for (int i = 0; i < mapdata.TeamOne.Count; i++)
            {
                sb.Draw(SquareTestTexture,
                    destinationRectangle: new Rectangle(DrawMapX + (int)(mapdata.TeamOne[i].X * DrawMapScale) + (DrawMapScale * mapdata.TeamOne[i].Health / mapdata.TeamOne[i].MaxHealth), DrawMapY + (int)(mapdata.TeamOne[i].Y * DrawMapScale) 
                   + DrawMapScale + 2, DrawMapScale - (DrawMapScale * mapdata.TeamOne[i].Health / mapdata.TeamOne[i].MaxHealth), 2), color: Color.Red, layerDepth: 1);
                sb.Draw(SquareTestTexture, destinationRectangle: new Rectangle(DrawMapX + (int)(mapdata.TeamOne[i].X * DrawMapScale), DrawMapY + (int)(mapdata.TeamOne[i].Y * DrawMapScale)
                    + DrawMapScale + 2, (DrawMapScale * mapdata.TeamOne[i].Health / mapdata.TeamOne[i].MaxHealth), 2), color: Color.Green, layerDepth: 1);
                DrawCreep(sb, mapdata.TeamOne[i]);
            }
            for (int i = 0; i < mapdata.TeamTwo.Count; i++)
            {
                sb.Draw(SquareTestTexture, 
                    destinationRectangle: new Rectangle(DrawMapX + (int)(mapdata.TeamTwo[i].X * DrawMapScale) + (DrawMapScale * mapdata.TeamTwo[i].Health / mapdata.TeamTwo[i].MaxHealth), 
                    DrawMapY + (int)(mapdata.TeamTwo[i].Y * DrawMapScale) + DrawMapScale + 2, DrawMapScale - (DrawMapScale * mapdata.TeamTwo[i].Health / mapdata.TeamTwo[i].MaxHealth), 2), 
                    color: Color.Red, layerDepth: 1);
                sb.Draw(SquareTestTexture, destinationRectangle: new Rectangle(DrawMapX + (int)(mapdata.TeamTwo[i].X * DrawMapScale), DrawMapY + (int)(mapdata.TeamTwo[i].Y * DrawMapScale)
                    + DrawMapScale + 2, (DrawMapScale * mapdata.TeamTwo[i].Health / mapdata.TeamTwo[i].MaxHealth), 2), color: Color.Green, layerDepth: 1);
                DrawCreep(sb, mapdata.TeamTwo[i]);

            }

            //Draw Towers
            for (int i = 0; i < mapdata.TowerData.GetLength(0); i++)
            {
                for (int j = 0; j < mapdata.TowerData.GetLength(1); j++)
                {
                    if (mapdata.TowerData[i, j] == null)
                        sb.Draw(EmptyTowerSlotTexture, destinationRectangle:
                            new Rectangle(DrawMapX + (2 * i + 3) * DrawMapScale, DrawMapY + (j * 2 + 2) * DrawMapScale, DrawMapScale, DrawMapScale), color: Color.Brown, layerDepth: (j * 2 + 2) / 100f);
                    else
                        DrawTower(sb, mapdata.TowerData[i, j].Aura, DrawMapX + (2 * i + 3) * DrawMapScale, DrawMapY + (j * 2 + 2) * DrawMapScale - 8, DrawMapScale, DrawMapScale + 8);
                }
            }

            //Draw Team Creeps
            for (int i = 0; i < mapdata.Projectiles.Count; i++)
            {
                sb.Draw(SquareTestTexture, destinationRectangle: new Rectangle(DrawMapX + (int)(mapdata.Projectiles[i].X * DrawMapScale + 10), DrawMapY + (int)(mapdata.Projectiles[i].Y * DrawMapScale + 10), 4, 4), color: Color.Red, layerDepth: 1);
            }


        }//End Draw


        public static void DrawSidePannel(SpriteBatch sb, Player player)
        {

        }

        public static void DrawSidePannel(SpriteBatch sb, Player playerOne, Player playerTwo)
        {

        }

        public static void DrawTower(SpriteBatch sb, TileProperties type, int x, int y, int width, int height)
        {
            switch (type)
            {
                case TileProperties.AttackBuff:
                    sb.Draw(BuffTowerTexture, destinationRectangle:
                        new Rectangle(x, y, width, height), color: Color.Crimson, layerDepth: (y) / 100f);
                    break;
                case TileProperties.AttackNerf:
                    sb.Draw(NerfTowerTexture, destinationRectangle:
                        new Rectangle(x, y, width, height), color: Color.LightCoral, layerDepth: (y) / 100f);
                    break;
                case TileProperties.ArmorBuff:
                    sb.Draw(BuffTowerTexture, destinationRectangle:
                        new Rectangle(x, y, width, height), color: Color.RoyalBlue, layerDepth: (y) / 100f);
                    break;
                case TileProperties.ArmorNerf:
                    sb.Draw(NerfTowerTexture, destinationRectangle:
                        new Rectangle(x, y, width, height), color: Color.DeepSkyBlue, layerDepth: (y) / 100f);
                    break;
                case TileProperties.SpeedBuff:
                    sb.Draw(BuffTowerTexture, destinationRectangle:
                        new Rectangle(x, y, width, height), color: Color.SpringGreen, layerDepth: (y) / 100f);
                    break;
                case TileProperties.SpeedNerf:
                    sb.Draw(NerfTowerTexture, destinationRectangle:
                        new Rectangle(x, y, width, height), color: Color.YellowGreen, layerDepth: (y) / 100f);
                    break;
                case TileProperties.DoubleXP:
                    sb.Draw(ExpTowerTexture, destinationRectangle:
                        new Rectangle(x, y, width, height), color: Color.Orange, layerDepth: (y) / 100f);
                    break;
                default:
                    sb.Draw(ShootTowerTexture, destinationRectangle:
                        new Rectangle(x, y, width, height), color: Color.Orchid, layerDepth: (y) / 100f);
                    break;
            }
        }

        //Method that draws one creep
        public static void DrawCreep(SpriteBatch sb, Creep target)
        {
            Color col = Color.White;
            Texture2D sheet = CreepDownTexture;
            switch (target.MyTeam)
            {
                case Team.None:
                    col = Color.White;
                    break;
                case Team.Team1:
                    col = Color.Red;
                    break;
                case Team.Team2:
                    col = Color.SkyBlue;
                    break;
                default:
                    col = Color.White;
                    break;
            }
            switch (target.Facing)
            {
                case Direction.None:
                    break;
                case Direction.Right:
                    sheet = CreepRightTexture;
                    break;
                case Direction.Left:
                    sheet = CreepLeftTexture;
                    break;
                case Direction.Up:
                    sheet = CreepUpTexture;
                    break;
                case Direction.Down:
                    sheet = CreepDownTexture;
                    break;
                default:
                    break;
            }

            if(target.Target == null)
            {
                if(CreepTimer > .375)
                    sb.Draw(sheet, sourceRectangle: new Rectangle(72, 0, 24, 24),destinationRectangle: new Rectangle(DrawMapX + (int)(target.X * DrawMapScale), DrawMapY + (int)(target.Y * DrawMapScale),
                                        DrawMapScale, DrawMapScale), color: col, layerDepth: target.Y / 100f);
                else if (CreepTimer > .25)
                    sb.Draw(sheet, sourceRectangle: new Rectangle(48, 0, 24, 24), destinationRectangle: new Rectangle(DrawMapX + (int)(target.X * DrawMapScale), DrawMapY + (int)(target.Y * DrawMapScale),
                                        DrawMapScale, DrawMapScale), color: col, layerDepth: target.Y / 100f);
                else if (CreepTimer > .125)
                    sb.Draw(sheet, sourceRectangle: new Rectangle(24, 0, 24, 24), destinationRectangle: new Rectangle(DrawMapX + (int)(target.X * DrawMapScale), DrawMapY + (int)(target.Y * DrawMapScale),
                                        DrawMapScale, DrawMapScale), color: col, layerDepth: target.Y / 100f);
                else
                    sb.Draw(sheet, sourceRectangle: new Rectangle(0, 0, 24, 24), destinationRectangle: new Rectangle(DrawMapX + (int)(target.X * DrawMapScale), DrawMapY + (int)(target.Y * DrawMapScale),
                                        DrawMapScale, DrawMapScale), color: col, layerDepth: target.Y / 100f);
            }
            else
            {
                if (target.AttackTimer < .125)
                    sb.Draw(sheet, sourceRectangle: new Rectangle(72, 0, 24, 24), destinationRectangle: new Rectangle(DrawMapX + (int)(target.X * DrawMapScale), DrawMapY + (int)(target.Y * DrawMapScale),
                                        DrawMapScale, DrawMapScale), color: col, layerDepth: target.Y / 100f);
                else if (target.AttackTimer < .25)
                    sb.Draw(sheet, sourceRectangle: new Rectangle(48, 0, 24, 24), destinationRectangle: new Rectangle(DrawMapX + (int)(target.X * DrawMapScale), DrawMapY + (int)(target.Y * DrawMapScale),
                                        DrawMapScale, DrawMapScale), color: col, layerDepth: target.Y / 100f);
                else if (target.AttackTimer < .375)
                    sb.Draw(sheet, sourceRectangle: new Rectangle(24, 0, 24, 24), destinationRectangle: new Rectangle(DrawMapX + (int)(target.X * DrawMapScale), DrawMapY + (int)(target.Y * DrawMapScale),
                                        DrawMapScale, DrawMapScale), color: col, layerDepth: target.Y / 100f);
                else
                    sb.Draw(sheet, sourceRectangle: new Rectangle(0, 0, 24, 24), destinationRectangle: new Rectangle(DrawMapX + (int)(target.X * DrawMapScale), DrawMapY + (int)(target.Y * DrawMapScale),
                                        DrawMapScale, DrawMapScale), color: col, layerDepth: target.Y / 100f);
            }
        }



    }//End Class
}
