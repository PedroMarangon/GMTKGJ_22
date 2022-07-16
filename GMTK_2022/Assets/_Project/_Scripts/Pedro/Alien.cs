// maded by Pedro M Marangon

using PedroUtils;
using UnityEngine;
namespace GMTK22
{
	public class Alien : Unit
	{
		[SerializeField] private GameObject actionList;

		private ActionType action = ActionType.None;

		public ActionType CrntAction => action;

		private void Start()
		{
			actionList.Deactivate();
		}

		public override void SelectAction() => actionList.Activate();

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
		}
	}
}
