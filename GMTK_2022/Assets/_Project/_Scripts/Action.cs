// maded by Pedro M Marangon
using UnityEngine;

namespace GMTK22
{
	public abstract class Action
	{
		protected Transform target;
		protected Transform transform;
		protected float damage;

		public Action(Transform transform) => this.transform = transform;

		public abstract void Execute();

		public void SetTargetAndDamage(Transform newTarget, float newDamage)
		{
			target = newTarget;
			damage = newDamage;
		}
	}

	public class AttackAction : Action
	{
		public AttackAction(Transform transform) : base(transform) { }

		public override void Execute()
		{
			//TODO: Go to attack position
			//TODO: Damage
		}
	}

	public class HealAction : Action
	{
		public HealAction(Transform transform) : base(transform) { }

		public override void Execute()
		{
			//TODO: Heal
		}
	}

	public class TauntAction : Action
	{
		public TauntAction(Transform transform) : base(transform) { }

		public override void Execute()
		{
			//TODO: Taunt
		}
	}

	public class NullAction : Action
	{
		public NullAction(Transform transform) : base(transform) { }

		public override void Execute() => throw new System.NotImplementedException("Selected null action");
	}
}
