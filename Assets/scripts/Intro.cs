using io.daniellanner.indiversity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TimelineDescription("menu", true)]
public class Intro : MonoBehaviourInTimeline
{
	TimelineManager _timelines;

	private void Awake()
	{
		_timelines = FindObjectOfType<TimelineManager>();
	}

	public override void EnterTimeline()
	{
		base.EnterTimeline();
	}

	public override void UpdateTimeline(float dt)
	{
		if(Input.GetKeyDown(KeyCode.Return))
		{
			_timelines.ActivateTimeline("game");
			_timelines.DeactiveTimeline("menu");
		}
	}
}
