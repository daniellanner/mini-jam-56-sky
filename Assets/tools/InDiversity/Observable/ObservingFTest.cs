using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservingFTest : ObservingFloat<Vector3>
{
	private void Update()
	{
		v.x += Time.deltaTime;
		Apply();
	}
}
