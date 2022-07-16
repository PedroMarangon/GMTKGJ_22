// maded by Pedro M Marangon
using UnityEngine;

namespace GMTK22
{
	public abstract class Action
	{
		protected Transform target;
		protected Transform transform;
		protected int damage;

		public Action(Transform transform) => this.transform = transform;

		public abstract void Execute();

		public void SetTargetAndDamage(Transform newTarget, int newDamage)
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
			var atkManager = Object.FindObjectOfType<attackEnemy>();
			atkManager.AtacarInimigo(transform, target, damage);
		}
	}

	public class HealAction : Action
	{
		public HealAction(Transform transform) : base(transform) { }

		public override void Execute()
		{
			var atkManager = Object.FindObjectOfType<attackEnemy>();
			//TODO: Heal
		}
	}

	public class NullAction : Action
	{
		public NullAction(Transform transform) : base(transform) { }

		public override void Execute() => throw new System.NotImplementedException("Selected null action");
	}
}
