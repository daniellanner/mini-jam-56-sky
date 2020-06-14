using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ObservingFloat<T> : MonoBehaviour
{
	protected T v = default;

	// Serialized
	[SerializeField]
	private Type _type;
	[SerializeField]
	private string _propertyName;

	// Cached
	private PropertyInfo _prop;
	private UnityEngine.Object _obj;
	private bool _initSafely = false;

	private void Awake()
	{
		if(_type == null)
		{
			return;
		}

		try
		{
			_obj = GetComponent(_type);
			_prop = _type.GetProperty("localPosition");
		}
		catch(Exception e)
		{
			Debug.LogError($"{e.Message}\n{e.StackTrace}");
			_initSafely = false;
			return;
		}

		_initSafely = true;
	}

	protected void Apply()
	{
		if (!_initSafely)
		{
			return;
		}

		_prop?.SetValue(_obj, v);
	}
}
