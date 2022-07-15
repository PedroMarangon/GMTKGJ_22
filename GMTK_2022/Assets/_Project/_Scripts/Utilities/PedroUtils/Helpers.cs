//Maded by Pedro M Marangon
using UnityEngine;

namespace PedroUtils
{
	public static class Helpers
	{
		private static Camera _cam;
		public static Camera Camera
		{
			get
			{
				if (_cam == null) _cam = Camera.main;
				return _cam;
			}
		}

		public static bool IsNotNull(object p) => p != null;

		public static Vector2 SetUIPositionBasedOnObject(Vector3 position, Vector2 offset)
		{
			var newPos = new Vector3(position.x + offset.x, position.y + offset.y, position.z);
			return RectTransformUtility.WorldToScreenPoint(Camera, newPos);
		}
	}
}