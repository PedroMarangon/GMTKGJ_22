// maded by Pedro M Marangon
using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace GMTK22
{
	[RequireComponent(typeof(CanvasGroup))]
	public class FadeController : MonoBehaviour
	{
		[SerializeField] private bool fadeAtStart = true;
		[EnableIf("fadeAtStart"), SerializeField] private float startingValue = 1f;
		[HideIf("fadeAtStart"), SerializeField] private float timeToFade = 0f;
		[SerializeField] private float durationToFade = 1f;
		[SerializeField] private bool destroyAfterFade = false;
		private CanvasGroup _group;

		private void Awake() => _group = GetComponent<CanvasGroup>();

		private void Start()
		{
			if (!fadeAtStart) return;
			_group.DOFade(startingValue, 0);
			WaitToFade();
		}

		public void SetDuration(float newDur) => durationToFade = newDur;

		private void WaitToFade()
		{
			StartCoroutine(nameof(Wait));
		}

		private IEnumerator Wait()
		{
			yield return new WaitForSeconds(timeToFade);
			Fade(0);
		}

		public Tween Fade(float value) => _group.DOFade(value, durationToFade).OnComplete(() => { if (destroyAfterFade) Destroy(gameObject); });
	}
}
