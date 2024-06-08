using System.Collections.Generic;
using UnityEngine;

namespace VarCo
{
    public static class Extensions
    {
		#region List
		private static System.Random rng = new();

		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static T GetRandomItem<T>(this IList<T> list)
		{
			int randomIndex = Random.Range(0,list.Count);
			return list[randomIndex];
		}
		#endregion

		#region Transform
		public static void DestroyImmediateAllChildren(this Transform target)
		{
			for (int i = target.childCount - 1; i >= 0; i--)
			{
				Object.DestroyImmediate(target.GetChild(i).gameObject);
			}
		}

		public static void ShuffleChildren(this Transform target)
		{
			List<Transform> children = new List<Transform>();
			foreach (Transform child in target)
			{
				children.Add(child);
			}
			children.Shuffle();
			foreach (Transform child in children)
			{
				child.SetSiblingIndex(children.IndexOf(child));
			}
		}
		#endregion
	}
}
