// Maded by Pedro M Marangon
using PedroUtils;
using MVM16;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
	public class DebugScreen : MonoBehaviour
	{
		[SerializeField] private int gameNameSize = 50;
		[SerializeField] private InputAction debugKey;
		private FPS fps = new FPS();
		private string _text;
		private bool _showGUI;
		private Rect _debugRect;
		private GUIStyle _debugStyle;

		private string PC_SPECS => $"OS: {SystemInfo.operatingSystem}\nCPU: {SystemInfo.processorType}\nGPU: {SystemInfo.graphicsDeviceName}";
		private string VERSION => $"{Application.productName.Bold().ToSize(gameNameSize)}\nv. {Application.version.Bold()}";
		private string FORMATTED_TEXT => _text.ToSize(40).Color("white");

		private void Awake()
		{
			_debugRect = new Rect(new Vector2(Screen.width / 2, 0), new Vector2(Screen.width / 2, Screen.height));
			_debugStyle = SetupStyle();

			UpdateText();
		}

		private void OnEnable()
		{
			debugKey.performed += _ => ToggleDebugScreen();
			debugKey.Enable();
		}

		private void OnDisable()
		{
			debugKey.performed -= _ => ToggleDebugScreen();
			debugKey.Disable();
		}

		private void OnGUI()
		{
			if (!_showGUI) return;

			GUI.Label(_debugRect, FORMATTED_TEXT, _debugStyle);
		}

		private void ToggleDebugScreen() => _showGUI = !_showGUI;

		private async void UpdateText()
		{
			_text = $"{VERSION}\n\n{PC_SPECS}\n\n{fps.GetFPS()}";
			
			await Task.Delay((int)(1000 * fps.REPEAT_RATE));
			
			UpdateText();
		}

		private GUIStyle SetupStyle()
		{
			var style = new GUIStyle();
			style.alignment = TextAnchor.UpperRight;
			style.padding = new RectOffset(0, 10, 10, 0);
			return style;
		}

		public class FPS
		{
			public float REPEAT_RATE => 0.1f;
			private const int MIN_FRAME_TIME = 10;
			private const string TEXT_FORMAT = "FPS: {0}\nAvg FPS: {1}\nMin FPS: {2}\nMax FPS: {03}";

			//Current FPS
			private float fps;
			//Average FPS
			private float avgFPS;
			private float fpsTotal;
			private int framesPassed;
			//Min/Max FPS
			private float minFPS = Mathf.Infinity;
			private float maxFPS = 0f;


			public string GetFPS()
			{
				fps = (1f / Time.unscaledDeltaTime).With2Decimals();

				fpsTotal += fps;
				framesPassed++;
				avgFPS = (fpsTotal / framesPassed).With2Decimals();

				if (fps > maxFPS && framesPassed > MIN_FRAME_TIME) maxFPS = fps.With2Decimals();
				if (fps < minFPS && framesPassed > MIN_FRAME_TIME) minFPS = fps.With2Decimals();

				return string.Format(TEXT_FORMAT, fps, avgFPS, minFPS, maxFPS);
			}
		}

	}

}