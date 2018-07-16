using Harmony;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using System;

namespace StackToNearbyChests
{
	[HarmonyPatch(typeof(StardewValley.Menus.InventoryPage))]
	[HarmonyPatch(new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) })]
	class InventoryPage_Patcher_Constructor
	{
		public static void Postfix(InventoryPage __instance, int x, int y, int width, int height)
		{
			ButtonHolder.Constructor(__instance, x, y, width, height);
		}
	}
	
	[HarmonyPatch(typeof(StardewValley.Menus.InventoryPage))]
	[HarmonyPatch("receiveLeftClick")]
	[HarmonyPatch(new Type[] { typeof(int), typeof(int), typeof(bool) })]
	class InventoryPage_Patcher_receiveLeftClick
	{
		public static void Postfix(int x, int y)
		{
			ButtonHolder.ReceiveLeftClick(x, y);
		}
	}

	[HarmonyPatch(typeof(StardewValley.Menus.InventoryPage))]
	[HarmonyPatch("draw")]
	[HarmonyPatch(new Type[] { typeof(Microsoft.Xna.Framework.Graphics.SpriteBatch) })]
	class InventoryPage_Patcher_draw
	{
		public static void Postfix(SpriteBatch b)
		{
			ButtonHolder.PostDraw(b);
		}
	}

	[HarmonyPatch(typeof(StardewValley.Menus.InventoryPage))]
	[HarmonyPatch("performHoverAction")]
	[HarmonyPatch(new Type[] { typeof(int), typeof(int) })]
	class InventoryPage_Patcher_performHoverAction
	{
		public static void Postfix(int x, int y)
		{
			ButtonHolder.PerformHoverAction(x, y);
		}
	}

	[HarmonyPatch(typeof(StardewValley.Menus.IClickableMenu))]
	[HarmonyPatch("populateClickableComponentList")]
	[HarmonyPatch(new Type[] { })]
	class IClickableMenu_Patcher_populateClickableComponentList
	{
		public static void Postfix(IClickableMenu __instance)
		{
			if (__instance is InventoryPage inventoryPage)
				ButtonHolder.PopulateClickableComponentsList(inventoryPage);
		}
	}
	
	[HarmonyPatch(typeof(StardewValley.Menus.ClickableTextureComponent))]
	[HarmonyPatch("draw")]
	[HarmonyPatch(new Type[] { typeof(Microsoft.Xna.Framework.Graphics.SpriteBatch) })]
	class ClickableTextureComponent_Patcher_Draw
	{
		public static void Postfix(ClickableTextureComponent __instance, SpriteBatch b)
		{
			ButtonHolder.TrashCanDrawn(__instance, b);
		}
	}
}
