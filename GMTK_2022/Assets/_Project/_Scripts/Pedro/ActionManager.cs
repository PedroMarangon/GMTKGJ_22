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
	public class ActionManager : MonoBehaviour
    {
        public enum MacroState
		{
            DMTurn,
            PlayersTurn
		}
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
		private MacroState macroState = MacroState.DMTurn;
        private MicroState microState = MicroState.None;
		private attackEnemy attackManager;

		public int D20 { get; private set; } = 0;

		private void Awake()
		{
			attackManager = FindObjectOfType<attackEnemy>();
			DisableD20();
		}

		private void Update()
        {
            if (macroState == MacroState.PlayersTurn) return;
			if (!Mouse.current.leftButton.wasPressedThisFrame) return;

			switch (microState)
			{
				case MicroState.None: SelectAlien(); break;
				//case MicroState.SelectingAction: SetAction();break;
				case MicroState.SelectingTarget: SelectTarget(); break;
				default: break;
			}
		}

		public Transform SelectRandomTarget(TargetGroup targetGroup)
		{
			return targetGroup switch
			{
				Unit.TargetGroup.Aliens => GetRandom.ElementInList(aliens),
				Unit.TargetGroup.Robots => GetRandom.ElementInList(robots),
				_ => transform
			};
		}

		public List<Transform> GetAllTargets(TargetGroup targetGroup)
		{
			return targetGroup switch
			{
				Unit.TargetGroup.Aliens => aliens,
				Unit.TargetGroup.Robots => robots,
				_ => new List<Transform>()
			};
		}

		public Collider2D GetMouseObjectBasedOnAction(ActionType action)
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

			var alienStats = crntAlien.GetComponent<characterStats>();
			//TODO: Check value
		}

		public void EnableD20() => d20Btn.interactable = true;
		public void DisableD20() => d20Btn.interactable = false;

		public void GoToTargetState()
		{
			this.Log($"Selected action on {macroState}");
			microState = MicroState.SelectingTarget;
		}


		private IEnumerator RobotsTurn()
		{
			var list = new List<Transform>(robots);
			list.OrderBy(x => Random.value);
			foreach (var rbt in robots)
			{
				Robot robot = rbt.GetComponent<Robot>();
				robot.SelectAction();
				while (attackManager.isRunning) yield return null;
			}
			yield break;
		}



		private void SelectTarget()
		{
			Collider2D input = GetMouseObjectBasedOnAction(crntAlien.CrntAction);
			if (input == null) return;

			crntAlien.SelectTarget();

			var targetGroup = crntAlien.CrntAction == ActionType.Attack ? TargetGroup.Robots : TargetGroup.Aliens;
			this.Log($"Selecting target {input.transform.name} from target group {targetGroup}");

			microState = MicroState.None;
		}

		private void SelectAlien()
		{
			Collider2D input = GetObjectOnMouse(whatIsAlien);
			if (input == null) return;
			this.Log($"Selecting alien {input.transform.name}");
			crntAlien = input.GetComponent<Alien>();

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
