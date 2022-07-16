// maded by Pedro M Marangon
using NaughtyAttributes;
using PedroUtils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
		[ReadOnly, SerializeField] private Transform crntTarget, crntAlien;
		[SerializeField] private List<Transform> aliens, robots;
		[SerializeField] private Transform actionPanel;
		[SerializeField] private float distance = 0.75f;

		private MacroState macroState = MacroState.DMTurn;
        private MicroState microState = MicroState.None;

		private void Update()
        {
            if (macroState == MacroState.PlayersTurn) return;
			if (!Mouse.current.leftButton.wasPressedThisFrame) return;

			switch (microState)
			{
				case MicroState.None: SelectAlien();break;
				case MicroState.SelectingAction: SetAction();break;
				case MicroState.SelectingTarget: SelectTarget();break;
			}
		}

		public Transform SelectRandomTarget(Unit.TargetGroup targetGroup)
		{
			return targetGroup switch
			{
				Unit.TargetGroup.Aliens => GetRandom.ElementInList(aliens),
				Unit.TargetGroup.Robots => GetRandom.ElementInList(robots),
				_ => transform
			};
		}

		public List<Transform> GetAllTargets(Unit.TargetGroup targetGroup)
		{
			return targetGroup switch
			{
				Unit.TargetGroup.Aliens => aliens,
				Unit.TargetGroup.Robots => robots,
				_ => new List<Transform>()
			};
		}

		public void RollD20()
		{
			int d20 = Random.Range(0, 20) + 1;
			this.Log($"D20: {d20}");

			var alienStats = crntAlien.GetComponent<characterStats>();
		}

		private void SelectTarget()
		{
			Collider2D input = GetObjectOnMouse(whatIsRobot);
			if (input == null) return;
			crntTarget = input.transform;
			this.Log($"Selecting robot {input.transform.name}");
			microState = MicroState.None;
		}

		private void SelectAlien()
		{
			Collider2D input = GetObjectOnMouse(whatIsAlien);
			if (input == null) return;
			this.Log($"Selecting alien {input.transform.name}");
			crntAlien = input.transform;
			microState = MicroState.SelectingAction;

			actionPanel.transform.position = Helpers.SetUIPositionBasedOnObject(crntAlien.transform.position, Vector2.left * distance);
			actionPanel.gameObject.Activate();
		}

		private void SetAction()
		{
			this.Log($"Selecting action on {macroState}");
			microState = MicroState.SelectingTarget;
		}

		private Collider2D GetObjectOnMouse(LayerMask mask)
		{
			Vector3 pos = Mouse.current.position.ReadValue();
			Vector3 worldPos = Helpers.Camera.ScreenToWorldPoint(pos);

			return Physics2D.OverlapPoint(worldPos, mask);
		}
	}
}
