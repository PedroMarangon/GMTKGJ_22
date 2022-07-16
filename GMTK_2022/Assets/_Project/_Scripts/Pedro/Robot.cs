// maded by Pedro M Marangon
using NaughtyAttributes;
using PedroUtils;
using UnityEngine;

namespace GMTK22
{
	public class Robot : Unit
	{
		private TargetGroup group;
		private Action selectedAction;

		public override void SelectAction()
		{
			var action = Random.value > 0.7f ? ActionType.Attack : ActionType.Heal;
			selectedAction = GetAction(action);
			group = action == ActionType.Attack ? TargetGroup.Aliens : TargetGroup.Robots;
			SelectTarget();
		}

		public override void SelectTarget()
		{
			target = manager.SelectRandomTarget(group);

			selectedAction.SetTarget(target);
			transform.Log($"Set action {selectedAction.GetType().ToString()} on target {target.name}");
			selectedAction.Execute();
		}
		public override void ResetUnit() => selectedAction = null;
	}
}
