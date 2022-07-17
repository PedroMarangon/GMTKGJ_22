// maded by Pedro M Marangon
using NaughtyAttributes;
using PedroUtils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static GMTK22.Unit;

namespace GMTK22
{
	[System.Serializable]
	public class Audio
	{
		[SerializeField] private List<AudioClip> clips;
		[SerializeField] private AudioSource source;
		[SerializeField] private float baseVolume = 1;
		[SerializeField] private float basePitch = 1;
		[Range(0,0.2f), SerializeField] private float pitchVariation = 0.1f;
		[Range(0,0.2f), SerializeField] private float volumeVariation = 0.1f;

		public void PlayAudio()
		{
			float volume = baseVolume + Random.Range(-volumeVariation, volumeVariation);
			float pitch = basePitch + Random.Range(-pitchVariation, pitchVariation);

			source.volume = volume;
			source.pitch = pitch;
			source.clip = GetRandom.Element(clips);
			source.Play();
		}

	}

	public class GameManager : MonoBehaviour
    {
        public enum MicroState
		{
            None,
            SelectingAction,
            SelectingTarget
		}

		[Header("Units")]
		[HorizontalLine]
		[SerializeField] private LayerMask whatIsAlien;
		[SerializeField] private LayerMask whatIsRobot;
		[ReadOnly, SerializeField] private Transform crntTarget;
		[SerializeField] private Alien crntAlien;
		[SerializeField] private List<Transform> aliens, robots;
		[HorizontalLine]
		[Header("D20")]
		[SerializeField] private Button d20Btn;
		[SerializeField] private Animator d20Anim;
		[SerializeField] private TMP_Text d20Text;
		[Header("Audios")]
		[HorizontalLine]
		[SerializeField] private Audio selectUnitSound;
		[SerializeField] private Audio noEffectSound;
		[SerializeField] private Audio atkSound;
		[SerializeField] private Audio healSound;
		[SerializeField] private Audio selectActionSound;
		private MicroState microState = MicroState.None;
		private attackEnemy attackManager;

		public Alien CrntAlien => crntAlien;
		public Transform CrntTarget => crntTarget;
		public int D20 { get; private set; } = 0;

		private void Awake()
		{
			attackManager = FindObjectOfType<attackEnemy>();
			DisableD20();
		}

		private void Update()
        {
			if (!Mouse.current.leftButton.wasPressedThisFrame) return;

			switch (microState)
			{
				case MicroState.None: SelectAlien(); break;
				case MicroState.SelectingTarget: SelectTarget(); break;
				default: break;
			}
		}

		public Transform SelectRandomTarget(TargetGroup targetGroup)
		{
			return targetGroup switch
			{
				TargetGroup.Aliens => GetRandom.Element(aliens),
				TargetGroup.Robots => GetRandom.Element(robots),
				_ => throw new System.Exception($"Target Group is non-existent: {(int)targetGroup}")
			};
		}

		public List<Transform> GetAllTargets(TargetGroup targetGroup)
		{
			return targetGroup switch
			{
				TargetGroup.Aliens => aliens,
				TargetGroup.Robots => robots,
				_ => new List<Transform>()
			};
		}

		public Collider2D GetObjectOnMouse(ActionType action)
		{
			return action switch
			{
				ActionType.Attack => GetObjectOnMouse(whatIsRobot),
				ActionType.Heal => GetObjectOnMouse(whatIsAlien),
				_ => throw new System.Exception($"Current alien's {crntAlien} action is {crntAlien.CrntAction} and it is NOT implemented")
			};
		}

		#region D20
		public void RollD20()
		{
			D20 = Random.Range(0, 20) + 1;
			d20Anim.SetTrigger("Spin");
			d20Text.text = $"{D20}";
		}
		public void EnableD20() => d20Btn.interactable = true;
		public void DisableD20() => d20Btn.interactable = false;
		public void ExecuteAction()
		{
			crntAlien.ExecuteAction();
			StartCoroutine(nameof(AlienAction));
		}
		#endregion

		#region Sounds
		public void PlayAttackSound() => atkSound.PlayAudio();
		public void PlayHealSound() => healSound.PlayAudio();
		public void PlayUnitSound() => selectUnitSound.PlayAudio();
		public void PlayNoEffectSound() => noEffectSound.PlayAudio();
		public void PlayActionSound() => selectActionSound.PlayAudio();
		#endregion

		public void GoToTargetState()
		{
			this.Log($"Selected action");
			microState = MicroState.SelectingTarget;
		}

		public void SetTarget(Transform target) => crntTarget = target;

		private IEnumerator RobotsTurn()
		{
			robots.RemoveAll(x => x == null);
			var list = new List<Transform>(robots);
			list.OrderBy(x => Random.value);
			foreach (var rbt in robots)
			{
				aliens.RemoveAll(x => x == null);
				rbt.Log($"Starting action loop...");
				Robot robot = rbt.GetComponent<Robot>();
				robot.SelectAction();
				while (attackManager.isRunning) yield return null;
				rbt.Log($"Finished action loop");
				robot.Disable();
				yield return new WaitForSeconds(1f);
			}
			
			foreach (var robot in robots)
			{
				aliens.RemoveAll(x => x == null);
				robot.GetComponent<Robot>().Enable();
			}
			yield break;
		}

		private IEnumerator AlienAction()
		{
			while (attackManager.isRunning) yield return null;
			crntTarget = null;
			crntAlien.Disable();
			crntAlien = null;
			
			if(HasAllTheAliensFinishedAttacking())
			{
				yield return StartCoroutine(nameof(RobotsTurn));
				foreach (var aln in aliens)
				{
					var alien = aln.GetComponent<Alien>();
					alien.ResetUnit();
					alien.Enable();
				}
			}

			bool HasAllTheAliensFinishedAttacking()
			{
				foreach (var aln in aliens)
				{
					var alien = aln.GetComponent<Alien>();
					if (!alien.HasFinished) return false;
				}
				return true;
			}
		}

		private void SelectTarget()
		{
			Collider2D input = GetObjectOnMouse(crntAlien.CrntAction);
			if (input == null) return;

			crntAlien.SelectTarget();
			PlayUnitSound();

			var targetGroup = crntAlien.CrntAction == ActionType.Attack ? TargetGroup.Robots : TargetGroup.Aliens;
			this.Log($"Selecting target {input.transform.name} from target group {targetGroup}");

			microState = MicroState.None;
		}

		private void SelectAlien()
		{
			if (crntAlien != null) return;
			Collider2D input = GetObjectOnMouse(whatIsAlien);
			
			if (input == null) return;
			if (!input.TryGetComponent(out Alien alien) || alien.HasFinished) return;
			
			this.Log($"Selecting alien {input.transform.name}");
			crntAlien = alien;
			PlayUnitSound();

			microState = MicroState.SelectingAction;
			crntAlien.SelectAction();
		}

		private Collider2D GetObjectOnMouse(LayerMask mask)
		{
			Vector3 pos = Mouse.current.position.ReadValue();
			Vector3 worldPos = Helpers.Camera.ScreenToWorldPoint(pos);

			return Physics2D.OverlapPoint(worldPos, mask);
		}
	}
}
