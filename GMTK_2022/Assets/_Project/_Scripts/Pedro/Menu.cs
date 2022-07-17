// maded by Pedro M Marangon
using UnityEngine;

namespace GMTK22
{
	public class Menu : MonoBehaviour
    {
		private SceneTransitions sceneTransitions;

		private void Awake()
		{
			sceneTransitions = FindObjectOfType<SceneTransitions>();
		}

		public void PlayGame() => sceneTransitions.LoadScene(sceneTransitions.GameScene);
	}
}
