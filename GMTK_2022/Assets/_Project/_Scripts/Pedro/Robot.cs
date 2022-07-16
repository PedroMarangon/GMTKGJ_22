// maded by Pedro M Marangon
using NaughtyAttributes;
using PedroUtils;
using UnityEngine;

namespace GMTK22
{
	public class Robot : Unit
	{
		[MinMaxSlider(1,8), SerializeField] private Vector2 damageRange = new Vector2(1,2);
		private TargetGroup group;
		private Action selectedAction;
		private int damage;

		public override void SelectAction()
		{
			var action = GetRandom.Boolean() ? ActionType.Attack : ActionType.Heal;
			selectedAction = GetAction(action);
			group = action == ActionType.Attack ? TargetGroup.Aliens : TargetGroup.Robots;
			SelectTarget();
		}

		[Button]
		private void Test()
		{
			this.Log(Mathf.RoundToInt(GetRandom.ValueInRange(damageRange)));
		}

		public override void SelectTarget()
		{
			target = manager.SelectRandomTarget(group);

			damage = Mathf.RoundToInt(GetRandom.ValueInRange(damageRange));


			selectedAction.Execute();
		}
	}
}
