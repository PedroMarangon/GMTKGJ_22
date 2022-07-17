// maded by Pedro M Marangon
using NaughtyAttributes;
using PedroUtils;
using UnityEngine;

namespace GMTK22
{
	public abstract class Unit : MonoBehaviour
	{
		public enum TargetGroup { Aliens, Robots }
		public enum ActionType { None, Attack, Heal }

		protected Transform target;
		protected GameManager manager;
		protected characterStats stats;
		private AttackAction atkAction;
		private HealAction healAction;
		[SerializeField] private RuntimeAnimatorController redAnimator, blueAnimator;

		private void Awake()
		{
			manager = FindObjectOfType<GameManager>();
			stats = GetComponent<characterStats>();

			atkAction = new AttackAction(transform);
			healAction = new HealAction(transform);

			GetComponentInChildren<Animator>().runtimeAnimatorController = GetRandom.Boolean() ? redAnimator : blueAnimator;

		}

		public abstract void SelectAction();
		public abstract void SelectTarget();
		public abstract void ResetUnit();

		public void Disable() => GetComponentInChildren<SpriteRenderer>().color = Color.grey;
		public void Enable() => GetComponentInChildren<SpriteRenderer>().color = Color.white;

		protected Action GetAction(ActionType actionType) => actionType switch
		{
			ActionType.Attack => atkAction,
			ActionType.Heal => healAction,
			_ => throw new System.Exception($"Provided action type is non-existent")
		};
	}
}
