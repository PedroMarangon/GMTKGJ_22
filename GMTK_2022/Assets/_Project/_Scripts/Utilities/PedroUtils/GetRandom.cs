//Maded by Pedro M Marangon
using System.Collections.Generic;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace PedroUtils
{
	public class GetRandom
	{
		public static bool Boolean() => UnityRandom.value > 0.5f;

		public static TObject ElementInArray<TObject>(TObject[] obj)
		{
			if (obj.Length <= 0) return default(TObject);
			int position = UnityRandom.Range(0, obj.Length);
			return obj[position];
		}

		public static TObject ElementInList<TObject>(List<TObject> obj)
		{
			if (obj.Count <= 0) return default(TObject);
			if (obj.Count == 1) return obj[0];
			int position = UnityRandom.Range(0, obj.Count);
			return obj[position];
		}

		public static float ValueInRange(Vector2 range) => UnityRandom.Range(range.x, range.y);
	}
}