// maded by Pedro M Marangon
using UnityEngine;

namespace GMTK22
{
	public abstract class Action
	{
		protected Transform target;
		protected Transform transform;
		protected GameManager manager;

		public Action(Transform transform)
		{
			this.transform = transform;
			manager = Object.FindObjectOfType<GameManager>();
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
			atkManager.CurarAmigo(transform, target, manager.D20);
		}
	}
}
