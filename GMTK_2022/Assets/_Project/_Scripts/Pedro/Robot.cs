﻿// maded by Pedro M Marangon
using NaughtyAttributes;
using PedroUtils;

namespace GMTK22
{
	public class Robot : Unit
	{
		private TargetGroup group;
		private Action selectedAction;

		public override void SelectAction()
		{
			var action = GetRandom.Boolean() ? ActionType.Attack : ActionType.Heal;
			selectedAction = GetAction(action);
			group = action == ActionType.Attack ? TargetGroup.Aliens : TargetGroup.Robots;
			SelectTarget();
		}

		public override void SelectTarget()
		{
			target = manager.SelectRandomTarget(group);

			selectedAction.SetTarget(target);
			selectedAction.Execute();
		}
		public override void ResetUnit() => selectedAction = null;
	}
}
