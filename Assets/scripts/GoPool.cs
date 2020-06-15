using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using io.daniellanner.indiversity;
using System.Linq;

public class GoPool : PoolBase
{
	private Transform[] _tr;
	private int _trIdx = 0;

	private void OnEnable()
	{
		_tr = GetComponentsInChildren<Transform>()
		.Where(it => it != transform)
		.ToArray();
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

	public void Cleanup()
	{
		foreach(var it in _tr)
		{
			it.localPosition = Vector3.zero;
		}
}
}
