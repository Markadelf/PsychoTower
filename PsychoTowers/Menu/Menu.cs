using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsychoTowers
{
    public enum MenuType
    {
        MainMenu,
        PauseMenu,
        GameOverMenu
    }

    public class Menu
    {
        public List<MenuPanel> Panels { get; set; }

        public Menu(MenuType type, Game1 game)
        {
            Panels = new List<MenuPanel>();
            switch (type)
            {
                case MenuType.MainMenu:
                    Panels.Add(new MenuPanel(SpriteManager.ButtonTexture, 720/2 - 64, 480/3, 128, 64, "Start Game", true));
                    Panels[0].ClickEvent += game.StartNewGame;
                    break;
                case MenuType.PauseMenu:
                    Panels.Add(new MenuPanel(SpriteManager.ButtonTexture, 720 / 2 - 64, 480 / 3, 128, 64, "Start Game", true));
                    Panels[0].ClickEvent += game.StartNewGame;
                    break;
                case MenuType.GameOverMenu:

                    break;
                default:
                    break;
            }
        }

        public void TryClick()
        {
            for(int i = 0; i < Panels.Count; i++)
            {
                Panels[i].TryClick();
            }
        }


    }
}
