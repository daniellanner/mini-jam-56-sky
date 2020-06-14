using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;

public class ObservingBaseEditor<T> : Editor
{
	private List<string> _compname = new List<string>();
	private List<Type> _comptype = new List<Type>();

	private int _compSelection = 0;

	private List<string> _propname = new List<string>();
	private int _propSelection = 0;

	private List<string> _methodname = new List<string>();
	private int _methodSelection = 0;

	protected GameObject _target;

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		QueryComponents();
		_compSelection = EditorGUILayout.Popup("Component", _compSelection, _compname.ToArray());

		QueryProperties();
		_propSelection = Math.Min(_propSelection, _propname.Count - 1);
		_propSelection = Math.Max(_propSelection, 0);
		_propSelection = EditorGUILayout.Popup("Property", _propSelection, _propname.ToArray());

		QueryMethods();
		_methodSelection = Math.Min(_methodSelection, _methodname.Count - 1);
		_methodSelection = Math.Max(_methodSelection, 0);
		_methodSelection = EditorGUILayout.Popup("Method", _methodSelection, _methodname.ToArray());

		//Debug.Log(serializedObject.FindProperty("_propertyName"));

		//EditorGUILayout.PropertyField(lookAtPoint);
		serializedObject.ApplyModifiedProperties();
	}

	private void QueryComponents()
	{
		_compname.Clear();
		_comptype.Clear();
		var comps = _target.GetComponents<Component>();

		comps.ToList().ForEach(it => { _comptype.Add(it.GetType()); _compname.Add(it.GetType().Name); });
	}

	private void QueryProperties()
	{
		var type = _comptype[_compSelection];

		_propname.Clear();

		type.GetProperties().ToList()
			.Where(it => it.PropertyType == typeof(T)).ToList()
			.ForEach(it => _propname.Add(ObjectNames.NicifyVariableName(it.Name)));

		type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToList()
			.Where(it => it.GetCustomAttribute<SerializeField>() != null)
			.Where(it => it.FieldType == typeof(T)).ToList()
			.ForEach(it => _propname.Add(ObjectNames.NicifyVariableName(it.Name)));
	}

	private void QueryMethods()
	{
		var type = _comptype[_compSelection];

		_methodname.Clear();

		type.GetMethods().ToList()
		.Where(it => it.GetParameters().Count() == 1).ToList()
			.Where(it => it.GetParameters()[0].ParameterType == typeof(T)).ToList()
			.ForEach(it => _methodname.Add(ObjectNames.NicifyVariableName(it.Name)));

		//type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToList()
		//	.Where(it => it.GetCustomAttribute<SerializeField>() != null)
		//	.Where(it => it.FieldType == typeof(T)).ToList()
		//	.ForEach(it => _methodname.Add(ObjectNames.NicifyVariableName(it.Name)));
	}
}


[CustomEditor(typeof(ObservingFTest))]
public class ObservingFloatEditor : ObservingBaseEditor<Vector3>
{
	private void OnEnable()
	{
		_target = ((ObservingFTest)target).gameObject;
	}
}