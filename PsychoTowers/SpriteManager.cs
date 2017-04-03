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
        public static Texture2D CreepTexture { get; set; }
        public static Texture2D BackgroundTexture { get; set; }
        public static Texture2D EmptyTowerSlotTexture { get; set; }
        public static Texture2D TeamCoreTexture { get; set; }

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
            DrawMapScale = 20;
            DrawMapX = 32;
            DrawMapWidth = 21 * DrawMapScale;
            DrawMapY = 32;
            DrawMapHeight = 19 * DrawMapScale;
        }
        #endregion

        //Info for drawing game screen
        public static int DrawMapX { get; set; }
        public static int DrawMapWidth { get; set; }
        public static int DrawMapY { get; set; }
        public static int DrawMapHeight { get; set; }
        public static int DrawMapScale { get; set; }






        public static void DrawMap(SpriteBatch sb, Map mapdata)
        {
            //Draw background to board
            sb.Draw(BackgroundTexture, new Rectangle(DrawMapX, DrawMapY, DrawMapWidth, DrawMapHeight), Color.White);


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





            

            //Draw Towers
            for (int i = 0; i < mapdata.TowerData.GetLength(0); i++)
            {
                for (int j = 0; j < mapdata.TowerData.GetLength(1); j++)
                {
                    if (mapdata.TowerData[i, j] == null)
                        sb.Draw(EmptyTowerSlotTexture, new Rectangle(DrawMapX + (2 * i + 3) * DrawMapScale, DrawMapY + (j * 2 + 2) * DrawMapScale, DrawMapScale, DrawMapScale), Color.Brown);
                }
            }

            //Draw Team Cores
            if(false)
            sb.Draw(TeamCoreTexture, new Rectangle(DrawMapX + (int)(mapdata.TeamOneCore.X * DrawMapScale), DrawMapY + (int)(mapdata.TeamOneCore.Y * DrawMapScale), DrawMapScale, DrawMapScale), Color.Red);
            if(false)
             sb.Draw(TeamCoreTexture, new Rectangle(DrawMapX + (int)(mapdata.TeamTwoCore.X * DrawMapScale), DrawMapY + (int)(mapdata.TeamTwoCore.Y * DrawMapScale), DrawMapScale, DrawMapScale), Color.Blue);



            //Draw Team Creeps
            for (int i = 0; i < mapdata.TeamOne.Count; i++)
            {
                sb.Draw(CreepTexture, new Rectangle(DrawMapX + (int)(mapdata.TeamOne[i].X * DrawMapScale), DrawMapY + (int)(mapdata.TeamOne[i].Y * DrawMapScale), DrawMapScale, DrawMapScale), Color.Red);
            }
            for (int i = 0; i < mapdata.TeamTwo.Count; i++)
            {
                sb.Draw(CreepTexture, new Rectangle(DrawMapX + (int)(mapdata.TeamTwo[i].X * DrawMapScale), DrawMapY + (int)(mapdata.TeamTwo[i].Y * DrawMapScale), DrawMapScale, DrawMapScale), Color.Blue);
            }

            //Draw Walls
            for (int i = 0; i < mapdata.TileData.GetLength(0); i++)
            {
                for (int j = 0; j < mapdata.TileData.GetLength(1); j++)
                {
                    if (mapdata.TileData[i, j].HasFlag(TileProperties.Blocked))
                    {
                        if (i < 2 || i > mapdata.TileData.GetLength(0) - 3 || j == 0 || j == mapdata.TileData.GetLength(1) - 1)
                            sb.Draw(BorderTexture, new Rectangle(DrawMapX + i * DrawMapScale, DrawMapY + j * DrawMapScale, DrawMapScale, DrawMapScale), Color.Black);
                        else
                            sb.Draw(WallTexture, new Rectangle(DrawMapX + i * DrawMapScale, DrawMapY + j * DrawMapScale, DrawMapScale, DrawMapScale), Color.Gray);
                    }
                }
            }


        }//End Draw

    }//End Class
}
