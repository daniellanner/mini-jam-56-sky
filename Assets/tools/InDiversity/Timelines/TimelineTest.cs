using io.daniellanner.indiversity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TimelineDescription("test", true)]
public class TimelineTest : MonoBehaviourInTimeline//, IMonoBehaviourInTimeline
{
	public override void UpdateTimeline(float dt)
	{
		Debug.Log($"Update {dt}");
	}

	public override void EnterTimeline()
	{
		Debug.Log("Enter");
	}

	public override void ExitTimeline()
	{
		Debug.Log("Exit");
	}
}
