using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace io.daniellanner.indiversity
{
	public static class DistributionContextMenu
	{
		[MenuItem("GameObject/InDiversity/Distribution")]
		static void LogSelectedTransformName()
		{
			// Get existing open window or if none, make a new one:
			Distribution window = (Distribution)EditorWindow.GetWindow(typeof(Distribution));
			Distribution.SelectedObject = Selection.activeGameObject;
			window.Show();
		}
	}
}