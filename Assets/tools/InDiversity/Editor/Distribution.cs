using UnityEditor;
using UnityEngine;

namespace io.daniellanner.indiversity
{
	public class Distribution : EditorWindow
	{
		// Add menu named "My Window" to the Window menu
		[MenuItem("Window/Distribution")]
		static void Init()
		{
			// Get existing open window or if none, make a new one:
			Distribution window = (Distribution)EditorWindow.GetWindow(typeof(Distribution));
			window.Show();
		}

		public static GameObject SelectedObject { get; set; }

		private static Vector3 _minValues;
		private static Vector3 _maxValues;
		private static Vector3Int _numberOfObjectsPerDimension;
		private static int _numberOfObjects;

		private static bool _isCanvasElement = false;
		private static bool _useColumnsAndRows = false;

		private static bool _randomizeSize = false;
		private static Vector2 _minSize;
		private static Vector2 _maxSize;

		void OnGUI()
		{
			SelectedObject = (GameObject)EditorGUILayout.ObjectField("Label:", SelectedObject, typeof(GameObject), true);

			_isCanvasElement = SelectedObject?.GetComponent<RectTransform>() != null;

			if (_isCanvasElement)
			{
				GUIStyle s = new GUIStyle(EditorStyles.boldLabel);
				s.normal.textColor = Color.blue;
				s.fontStyle = FontStyle.Bold;

				EditorGUILayout.LabelField("UI Element", s);
			}

			GUI.enabled = SelectedObject != null;

			_minValues = EditorGUILayout.Vector3Field("Min Values", _minValues);
			_maxValues = EditorGUILayout.Vector3Field("Max Values", _maxValues);

			if (_useColumnsAndRows)
			{
				_numberOfObjectsPerDimension = EditorGUILayout.Vector3IntField("Number of Objects", _numberOfObjectsPerDimension);

				_numberOfObjectsPerDimension.x = Mathf.Max(_numberOfObjectsPerDimension.x, 1);
				_numberOfObjectsPerDimension.y = Mathf.Max(_numberOfObjectsPerDimension.y, 1);
				_numberOfObjectsPerDimension.z = Mathf.Max(_numberOfObjectsPerDimension.z, 1);
			}
			else
			{
				_numberOfObjects = EditorGUILayout.IntField("Number of Objects", _numberOfObjects);
			}

			EditorGUI.indentLevel++;
			_useColumnsAndRows = EditorGUILayout.Toggle("Split Axis", _useColumnsAndRows);
			EditorGUI.indentLevel--;

			if (_isCanvasElement)
			{
				_randomizeSize = EditorGUILayout.Toggle("Randomize Size", _randomizeSize);

				if (_randomizeSize)
				{
					_minSize = EditorGUILayout.Vector2Field("Min Size", _minSize);
					_maxSize = EditorGUILayout.Vector2Field("Max Size", _maxSize);
				}
			}

			if (GUILayout.Button("Spawn Object"))
			{
				SpawnObjects();
			}

			GUI.enabled = true;
		}

		private void SpawnObjects()
		{
			Transform parent = SelectedObject.transform.parent;

			if (!_useColumnsAndRows)
			{
				for (int i = 0; i < _numberOfObjects; i++)
				{
					var inst = Instantiate(SelectedObject, parent);

					Vector3 p = Vector3.zero;
					p.x = Random.Range(_minValues.x, _maxValues.x);
					p.y = Random.Range(_minValues.y, _maxValues.y);
					inst.transform.localPosition = p;

					if(_randomizeSize)
					{
						Vector2 s = Vector2.one * Random.Range(_minSize.x, _maxSize.x);
						
						inst.GetComponent<RectTransform>().sizeDelta = s;
					}

					Undo.RegisterCreatedObjectUndo(inst, "Create object");
				}

				return;
			}

			Vector3 pos = Vector3.zero;

			_numberOfObjectsPerDimension.x = Mathf.Max(_numberOfObjectsPerDimension.x, 1);
			_numberOfObjectsPerDimension.y = Mathf.Max(_numberOfObjectsPerDimension.y, 1);
			_numberOfObjectsPerDimension.z = Mathf.Max(_numberOfObjectsPerDimension.z, 1);

			for(int row = 0; row < _numberOfObjectsPerDimension.x; row++)
			{
				pos.x = Mathf.Lerp(_minValues.x, _maxValues.x, (float)row / (float)_numberOfObjectsPerDimension.x);

				for (int column = 0; column < _numberOfObjectsPerDimension.y; column++)
				{
					pos.y = Mathf.Lerp(_minValues.y, _maxValues.y, (float)column / (float)_numberOfObjectsPerDimension.y);

					for (int aisle = 0; aisle < _numberOfObjectsPerDimension.z; aisle++)
					{
						pos.z = Mathf.Lerp(_minValues.z, _maxValues.z, (float)aisle / (float)_numberOfObjectsPerDimension.z);

						var inst = Instantiate(SelectedObject, parent);
						inst.transform.localPosition = pos;

						Undo.RegisterCreatedObjectUndo(inst, "Create object");
					}
				}
			}
		}
	}
}
