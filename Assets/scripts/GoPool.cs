using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using io.daniellanner.indiversity;

public class GoPool : PoolBase
{
	private Transform[] _tr;
	private int _trIdx = 0;

	private void OnEnable()
	{
		_tr = GetComponentsInChildren<Transform>();
	}

	public Result<Transform> RequestObject(Vector3 position)
	{
		var result = GetObject<Transform>(_tr, ref _trIdx);

		if (result)
		{
			result.Value.position = position;
		}

		return result;
	}
}
