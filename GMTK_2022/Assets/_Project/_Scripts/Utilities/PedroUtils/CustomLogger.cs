//Maded by Pedro M Marangon
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PedroUtils
{
	public static class CustomLogger
	{
		private static void DoLog(Action<string, Object> logFunction, string prefix, Object myObj, params object[] msg)
		{
#if UNITY_EDITOR
			logFunction($"{prefix}[{myObj.name.Color("lightblue")}]: { string.Join(";", msg) }\n ", myObj);
#endif
		}

		public static void Log(this Object myObj, params object[] msg) => DoLog(Debug.Log, "", myObj, msg);

		public static void LogError(this Object myObj, params object[] msg) => DoLog(Debug.LogError, "<!>".Color("#ff1748"), myObj, msg);

		public static void LogWarning(this Object myObj, params object[] msg) => DoLog(Debug.LogWarning, "⚠".Color("yellow"), myObj, msg);

		public static void LogSucess(this Object myObj, params object[] msg) => DoLog(Debug.Log, "<✔>".Color("#40ff37"), myObj, msg);
	}
}