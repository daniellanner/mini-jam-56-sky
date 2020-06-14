using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace io.daniellanner.indiversity
{
	public class BackbuttonStack
	{

		private static BackbuttonStack Instance => NestedInstance._instance;

		private class NestedInstance
		{
			static NestedInstance() { }
			internal static readonly BackbuttonStack _instance = new BackbuttonStack();
		}

		private BackbuttonStack()
		{
			Init();
		}

		private System.Action[] _actionStack;
		private int _head;

		private void Init()
		{
			// we define this at design time and don't check every push if there's still room
			_actionStack = new System.Action[8];
			_head = -1;
		}

		private void PushAction(System.Action action)
		{
			_actionStack[++_head] = action;
		}

		private void PopAction()
		{
			if (_head < 0)
				return;

			_actionStack[_head--]?.Invoke();
		}

		public static void AddAction(System.Action action)
		{
			Instance.PushAction(action);
		}

		public static void ButtonPressed()
		{
			Instance.PopAction();
		}

		public static void Flush()
		{
			Instance.Init();
		}
	}
}