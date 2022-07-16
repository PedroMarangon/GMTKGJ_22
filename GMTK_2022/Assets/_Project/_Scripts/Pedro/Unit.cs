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
		protected attackEnemy atkManager;
		[MinMaxSlider(1, 8), SerializeField] protected Vector2 damageRange = new Vector2(1, 2);
		protected int damage;

		private void Awake()
		{
			manager = FindObjectOfType<ActionManager>();
			atkManager = FindObjectOfType<attackEnemy>();
		}

		public abstract void SelectAction();

		public abstract void SelectTarget();

		protected void SetDamage() => damage = Mathf.RoundToInt(GetRandom.ValueInRange(damageRange));

		protected Action GetAction(ActionType actionType) => actionType switch
		{
			ActionType.Attack => new AttackAction(transform),
			ActionType.Heal => new HealAction(transform),
			ActionType.Taunt => new TauntAction(transform),
			_ => new NullAction(transform)
		};
	}
}
