using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace StackToNearbyChests
{
	abstract class Patch
	{
		/// <summary>
		/// Don't use typeof() or it won't work on Mac/Linux
		/// </summary>
		/// <returns></returns>
		public abstract Type GetTargetType();

		/// <summary>
		/// Null if constructor is desired
		/// </summary>
		public abstract string GetTargetMethodName();

		public abstract Type[] GetTargetMethodArguments();

		public void ApplyPatch(Harmony harmonyInstance)
		{
			MethodBase targetMethod = String.IsNullOrEmpty(GetTargetMethodName()) ?
				(MethodBase)GetTargetType().GetConstructor(GetTargetMethodArguments()) :
				GetTargetType().GetMethod(GetTargetMethodName(), GetTargetMethodArguments());

			MethodInfo prefix = GetType().GetMethod("Prefix");
			MethodInfo postfix = GetType().GetMethod("Postfix");

			harmonyInstance.Patch(targetMethod, prefix != null ? new HarmonyMethod(prefix) : null, postfix != null ? new HarmonyMethod(postfix) : null);
		}

		public static void PatchAll(Harmony harmonyInstance)
		{
			foreach (Type type in (from type in Assembly.GetExecutingAssembly().GetTypes()
								   where type.IsClass && type.BaseType == typeof(Patch)
								   select type))
				((Patch)Activator.CreateInstance(type)).ApplyPatch(harmonyInstance);
		}
	}
}
