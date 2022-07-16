// maded by Pedro M Marangon
using UnityEngine;

namespace GMTK22
{
	public abstract class Action
	{
		protected Transform target;
		protected Transform transform;
		protected ActionManager manager;

		public Action(Transform transform)
		{
			this.transform = transform;
			manager = Object.FindObjectOfType<ActionManager>();
		}

		public abstract void Execute();

		public void SetTarget(Transform newTarget) => target = newTarget;
	}

	public class AttackAction : Action
	{
		public AttackAction(Transform transform) : base(transform) { }

		public override void Execute()
		{
			var atkManager = Object.FindObjectOfType<attackEnemy>();
			atkManager.AtacarInimigo(transform, target, manager.D20);
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
