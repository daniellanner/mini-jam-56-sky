using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using io.daniellanner.indiversity;

public class LinePool : PoolBase
{
	private LineRenderer[] _lines;
	private int _linesIdx = 0;

	private void OnEnable()
	{
		_lines = GetComponentsInChildren<LineRenderer>();
	}

	public Result<LineRenderer> RequestLine(Vector3 from, Vector3 to)
	{
		var result = GetObject<LineRenderer>(_lines, ref _linesIdx);

		if (result)
		{
			result.Value.SetPositions(new Vector3[]{from, to});
		}

		return result;
	}

	public void ResetLine(LineRenderer line)
	{
		line.SetPositions(new Vector3[] { transform.position, transform.position + Vector3.up });
	}
}
