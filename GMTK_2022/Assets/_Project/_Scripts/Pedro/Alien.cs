// maded by Pedro M Marangon
using NaughtyAttributes;
using PedroUtils;
using UnityEngine;

namespace GMTK22
{
	public class Alien : Unit
	{
		[SerializeField] private GameObject actionList;
		[ReadOnly, SerializeField] private bool hasFinished = false;
		private ActionType action = ActionType.None;

		public bool HasFinished => hasFinished;
		public ActionType CrntAction => action;

		private void Start() => actionList.Deactivate();

		public override void SelectAction() => actionList.Activate();

		public void ExecuteAction()
		{
			var action = GetAction(CrntAction);
			action.SetTarget(target);
			action.Execute();
		}

		public void SetAction(int type)
		{
			action = (ActionType)type;
			actionList.Deactivate();
			manager.GoToTargetState();
		}

		public override void SelectTarget()
		{
			target = manager.GetMouseObjectBasedOnAction(action).transform;
			manager.EnableD20();
			hasFinished = true;
		}

		public override void ResetUnit()
		{
			action = ActionType.None;
			hasFinished = false;
			target = null;
		}
	}
}
