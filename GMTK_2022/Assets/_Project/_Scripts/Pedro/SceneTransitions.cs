// maded by Pedro M Marangon
using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMTK22
{
	public class SceneTransitions : MonoBehaviour
	{
		[Required, SerializeField] private FadeController fadeController;
		[Scene, SerializeField] private int menuScene;
		[Scene, SerializeField] private int gameplayScene;
		private int _crntScene;

		public int GameScene => gameplayScene;

		private void Start()
		{
			Time.timeScale = 1;

			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene scene = SceneManager.GetSceneAt(i);
				if (!scene.name.Contains("Base")) _crntScene = scene.buildIndex;
			}

			if (SceneManager.sceneCount == 1) LoadScene(menuScene);
		}

		public void LoadScene(int scene)
		{
			Sequence s = DOTween.Sequence().SetUpdate(true)
				.Append(fadeController?.Fade(1))
				.AppendCallback(() => LoadSceneAdditive(scene))
				.AppendCallback(() => Time.timeScale = 1)
				.Append(fadeController?.Fade(0));
		}

		private void LoadSceneAdditive(int scene)
		{
			if (_crntScene != -1) SceneManager.UnloadSceneAsync(_crntScene);
			SceneManager.LoadScene(scene, _crntScene != -1 ? LoadSceneMode.Additive : LoadSceneMode.Single);
			_crntScene = scene;
		}

		public void ReloadScene(bool shouldFade)
		{
			Sequence s = DOTween.Sequence().SetUpdate(true);
			if (shouldFade) s.Append(fadeController?.Fade(1));
			s.AppendCallback(() => LoadSceneAdditive(_crntScene));
		}

	}
}
