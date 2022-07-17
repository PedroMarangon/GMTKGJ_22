// maded by Pedro M Marangon
using NaughtyAttributes;
using PedroUtils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static GMTK22.Unit;

namespace GMTK22
{
	public class GameManager : MonoBehaviour
    {
        public enum MicroState
		{
            None,
            SelectingAction,
            SelectingTarget
		}

		[SerializeField] private LayerMask whatIsAlien, whatIsRobot;
		[ReadOnly, SerializeField] private Transform crntTarget;
		[SerializeField] private Alien crntAlien;
		[SerializeField] private List<Transform> aliens, robots;
		[SerializeField] private Button d20Btn;
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
				_ => transform
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
			return crntAlien.CrntAction switch
			{
				ActionType.Attack => GetObjectOnMouse(whatIsRobot),
				ActionType.Heal => GetObjectOnMouse(whatIsAlien),
				_ => throw new System.Exception($"Current alien's {crntAlien} action is {crntAlien.CrntAction} and it is NOT implemented")
			};
		}

		public void RollD20()
		{
			D20 = Random.Range(0, 20) + 1;
			this.Log($"D20: {D20}");

			crntAlien.ExecuteAction();
			StartCoroutine(nameof(AlienAction));
		}
		public void EnableD20() => d20Btn.interactable = true;
		public void DisableD20() => d20Btn.interactable = false;

		public void GoToTargetState()
		{
			this.Log($"Selected action");
			microState = MicroState.SelectingTarget;
		}

		public void SetTarget(Transform target) => crntTarget = target;

		private IEnumerator RobotsTurn()
		{
			var list = new List<Transform>(robots);
			list.OrderBy(x => Random.value);
			foreach (var rbt in robots)
			{
				rbt.Log($"Starting action loop...");
				Robot robot = rbt.GetComponent<Robot>();
				robot.SelectAction();
				while (attackManager.isRunning) yield return null;
				rbt.Log($"Finished action loop");
				yield return new WaitForSeconds(1f);
			}
			yield break;
		}

		private IEnumerator AlienAction()
		{
			while (attackManager.isRunning) yield return null;
			crntAlien = null;
			
			if(HasAllTheAliensFinishedAttacking())
			{
				yield return StartCoroutine(nameof(RobotsTurn));
				foreach (var aln in aliens)
				{
					var alien = aln.GetComponent<Alien>();
					alien.ResetUnit();
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
