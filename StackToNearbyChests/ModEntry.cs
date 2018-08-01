using Harmony;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using System.Reflection;

namespace StackToNearbyChests
{
	class ModEntry : Mod
	{
		internal static ModConfig Config { get; private set; }
		
		public override void Entry(IModHelper helper)
		{
			Config = helper.ReadConfig<ModConfig>();

			ButtonHolder.ButtonIcon = helper.Content.Load<Texture2D>(@"icon.png");

			HarmonyInstance harmony = HarmonyInstance.Create("me.ilyaki.StackToNearbyChests");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}

	}
}
