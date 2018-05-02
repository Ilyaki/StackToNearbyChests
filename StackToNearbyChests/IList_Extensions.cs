using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackToNearbyChests
{
	public static class IList_Extensions
	{
		public static void RemoveAll<T> (this IList<T> list, Func<T, bool> predicate)
		{
			List<T> toRemove = new List<T>();
			foreach (T item in list)
			{
				if (predicate(item))
					toRemove.Add(item);
			}
			foreach(T item in toRemove)
			{
				list.Remove(item);
			}
		}
	}
}
