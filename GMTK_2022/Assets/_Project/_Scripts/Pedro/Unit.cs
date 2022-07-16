// maded by Pedro M Marangon
using NaughtyAttributes;
using PedroUtils;
using UnityEngine;

namespace GMTK22
{
	public abstract class Unit : MonoBehaviour
	{
		public enum TargetGroup { Aliens, Robots }
		public enum ActionType { Attack, Heal, Taunt }

		protected Transform target;
		protected ActionManager manager;
		protected characterStats stats;

		private void Awake()
		{
			manager = FindObjectOfType<ActionManager>();
			stats = GetComponent<characterStats>();
		}

		public abstract void SelectAction();

		public abstract void SelectTarget();

		protected Action GetAction(ActionType actionType) => actionType switch
		{
			ActionType.Attack => new AttackAction(transform),
			ActionType.Heal => new HealAction(transform),
			ActionType.Taunt => new TauntAction(transform),
			_ => new NullAction(transform)
		};
	}
}
