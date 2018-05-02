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

		internal static Texture2D ButtonIcon { get; private set; }

		Texture2D fadeToBlackTexture;

		internal static ModConfig Config { get; private set; }

		ControllerButtonReceiver controllerButtonReceiver;

		public override void Entry(IModHelper helper)
		{
			Config = helper.ReadConfig<ModConfig>();

			ButtonIcon = helper.Content.Load<Texture2D>(@"icon.png");
			fadeToBlackTexture = Game1.fadeToBlackRect;

			//Controller button(e.g. RightStick) workaround
			controllerButtonReceiver = new ControllerButtonReceiver();
			StardewModdingAPI.Events.ControlEvents.ControllerButtonPressed += controllerButtonReceiver.ControlEvents_ControllerButtonPressed;

			//since the button is not the ACTIVE menu, the receiveLeftClick will never be called by SDV.
			StardewModdingAPI.Events.ControlEvents.MouseChanged += (o, e) =>
			{
				button?.performHoverAction(Game1.getOldMouseX(), Game1.getOldMouseY());

				if (button != null && e.NewState.LeftButton == ButtonState.Pressed && e.PriorState.LeftButton == ButtonState.Released)
				{
					if (button.ButtonBounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()))
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
						Game1.onScreenMenus.RemoveAll(x => x is StackButtonMenu);

						button = null;
						Game1.fadeToBlackRect = fadeToBlackTexture;
					}
					if (newTab == 0 && currentTab != 0)
					{
						//switched into menu
						#region Add button
						List<IClickableMenu> menuList = helper.Reflection.GetField<List<IClickableMenu>>(gameMenu, "pages").GetValue();
						foreach (IClickableMenu page in menuList)
						{
							if (page is InventoryPage invPage)
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


			StardewModdingAPI.Events.MenuEvents.MenuClosed += (o, e) => { Game1.onScreenMenus.RemoveAll(x => x is StackButtonMenu); button = null; Game1.fadeToBlackRect = fadeToBlackTexture; };

		}

	}
}
