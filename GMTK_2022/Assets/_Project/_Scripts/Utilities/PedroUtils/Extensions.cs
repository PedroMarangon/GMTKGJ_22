//Maded by Pedro M Marangon
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PedroUtils
{
	public static class Extensions
	{
		//---------GENERICS-----------//
		public static T GetOrAddComponent<T>(this GameObject gObj) where T : Component
		{
			var comp = gObj.GetComponent<T>();
			if (comp == null) comp = gObj.AddComponent<T>();
			return comp;
		}
		public static T[] RemoveAll<T>(this T[] array, System.Predicate<T> match)
		{
			List<T> list = array.ToList();
			list.RemoveAll(match);
			return list.ToArray();
		}

		//---------COMPONENT-----------//
		public static void Suicide(this Component gObj, float time = 0) => Object.Destroy(gObj.gameObject, time);
		public static List<Transform> GetParents(this Component[] comps)
		{
			List<Transform> list = new List<Transform>();

			foreach (var comp in comps)
				list.Add(comp.transform.parent);

			return list;
		}
		public static List<Transform> GetParents(this List<Component> comps)
		{
			List<Transform> list = new List<Transform>();

			foreach (var comp in comps)
				list.Add(comp.transform.parent);

			return list;
		}

		//---------FLOAT-----------//
		public static float With2Decimals(this float v) => Mathf.Round(v * 100f) / 100f;
		public static float With3Decimals(this float v) => Mathf.Round(v * 1000f) / 1000f;
		public static float ClampAngle(this float angle, float min, float max)
		{
			if (angle < -360f) angle += 360f;
			if (angle > 360f) angle -= 360f;
			return Mathf.Clamp(angle, min, max);
		}
		public static bool IsInsideInterval(this float val, float sVal, float eVal, bool sClosed, bool eClosed)
		{
			int count = 0;

			if (sClosed)
				count += (val >= sVal) ? 1 : 0;
			else
				count += (val > sVal) ? 1 : 0;

			if (eClosed)
				count += (val <= eVal) ? 1 : 0;
			else
				count += (val < eVal) ? 1 : 0;

			return count == 2;
		}

		//---------VECTOR3-----------//
		public static Vector3 With2Decimals(this Vector3 v) => new Vector3(v.x.With2Decimals(), v.y.With2Decimals(), v.z.With2Decimals());
		public static Vector3 With3Decimals(this Vector3 v) => new Vector3(v.x.With3Decimals(), v.y.With3Decimals(), v.z.With3Decimals());
		public static Vector3 SetXZFromVector2(this Vector3 og, Vector2 vector, float multiplier = 1) => new Vector3(vector.x * multiplier, og.y, vector.y * multiplier);

		//---------ANIMATOR-----------//
		public static bool IsPlayingAnimation(this Animator anim, string animation, int layerIndex = 0) => anim.GetCurrentAnimatorStateInfo(0).IsName(animation);

		//---------GAMEOBJECT-----------//
		public static bool IsInsideLayerMask(this GameObject go, LayerMask layerMask) => (layerMask.value & (1 << go.layer)) > 0;
		public static bool IsInLayer(this GameObject go, string name) => (LayerMask.NameToLayer(name) & (1 << go.layer)) > 0;
		public static bool IsInSameLayerAsMe(this GameObject go, GameObject obj) => (1 << go.layer & (1 << obj.layer)) > 0;
		public static void Activate(this GameObject go) => go.SetActive(true);
		public static void Deactivate(this GameObject go) => go.SetActive(false);

		//---------CANVAS GROUP-----------//
		public static void SetValues(this CanvasGroup group, float alpha, bool combinedValue)
		{
			group.alpha = alpha;
			group.interactable = group.blocksRaycasts = combinedValue;
		}

		//---------STRING-----------//
		public static string Color(this string myStr, string color) => $"<color={color}>{myStr}</color>";
		public static string ToSize(this string myStr, int size) => $"<size={size}>{myStr}</size>";
		public static string Bold(this string myStr) => $"<b>{myStr}</b>";

		//---------DOTWEEN-----------//
		public static Tween DOPosition(this LineRenderer rend, int index, Vector3 endPos, float duration)
		{
			if (index < 0) index = 0;
			if (index >= rend.positionCount) index = rend.positionCount - 1;
			return DOTween.To(() => rend.GetPosition(index), x => rend.SetPosition(index, x), endPos, duration);
		}
	}
}