using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using StardewValley.Menus;
using Microsoft.Xna.Framework.Graphics;

namespace StackToNearbyChests
{
    class ModEntry : Mod
    {
        StackButtonMenu button;

        int? currentTab = null;

        internal static Texture2D buttonIcon { get; private set; }

        Texture2D fadeToBlackTexture;
        
        //test
        
        public override void Entry(IModHelper helper)
        {
            buttonIcon = helper.Content.Load<Texture2D>(@"icon.png");
            fadeToBlackTexture = Game1.fadeToBlackRect;

            //since the button is not the ACTIVE menu, the receiveLeftClick will never be called by SDV.
            StardewModdingAPI.Events.ControlEvents.MouseChanged += (o, e) => {
                if (button != null && e.NewState.LeftButton == ButtonState.Pressed && e.PriorState.LeftButton == ButtonState.Released)
                {
                    if (button.isWithinBounds(e.NewState.X, e.NewState.Y))
                    {
                        button.receiveLeftClick(e.NewState.X, e.NewState.Y);
                    }
                }
            };

            //Check if the menu tab has changed. if changed away from inv, remove button and vice versa
            StardewModdingAPI.Events.GameEvents.UpdateTick += (o, e) =>
            {
                if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is GameMenu)
                {
                    GameMenu gameMenu = Game1.activeClickableMenu as GameMenu;
                    int newTab = gameMenu.currentTab;
                    
                    if (newTab != 0 && currentTab == 0)
                    {
                        //switched tab from inventory, so remove button
                        Game1.onScreenMenus.Remove(button);
                        button = null;
                        Game1.fadeToBlackRect = fadeToBlackTexture;
                    }
                    if(newTab == 0 && currentTab != 0)
                    {
                        //switched into menu
                        #region Add button
                        List<IClickableMenu> menuList = helper.Reflection.GetField<List<IClickableMenu>>(gameMenu, "pages").GetValue();
                        foreach (IClickableMenu page in menuList)
                        {
                            InventoryPage invPage = page as InventoryPage;
                            if (invPage != null)
                            {
                                button = new StackButtonMenu(invPage.xPositionOnScreen, invPage.yPositionOnScreen, invPage.width, invPage.height);
                                Game1.onScreenMenus.Add(button);
                            }
                        };
                        #endregion
                    }
                    currentTab = newTab;
                }
                else
                {
                    currentTab = null;
                }
            };

            StardewModdingAPI.Events.MenuEvents.MenuChanged += (o, e) => {
                GameMenu menu = e.NewMenu as GameMenu;
                if (menu != null)
                {
                    List<IClickableMenu> menuList = helper.Reflection.GetField<List<IClickableMenu>>(menu, "pages").GetValue();
                    foreach (IClickableMenu page in menuList)
                    {
                        InventoryPage invPage = page as InventoryPage;
                        if (invPage != null)
                        {
                            //remove old one first
                            Game1.onScreenMenus.Remove(button);

                            button = new StackButtonMenu(invPage.xPositionOnScreen, invPage.yPositionOnScreen, invPage.width, invPage.height);
                            Game1.onScreenMenus.Add(button);
                        }
                    }
                }

            };
            
            StardewModdingAPI.Events.MenuEvents.MenuClosed += (o, e) => { if (button != null) { Game1.onScreenMenus.Remove(button); button = null; Game1.fadeToBlackRect = fadeToBlackTexture; } };
            
        }
              
    }
}
