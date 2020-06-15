using io.daniellanner.indiversity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TimelineDescription("menu", true)]
public class Intro : MonoBehaviourInTimeline
{
	TimelineManager _timelines;

#pragma warning disable CS0649
	[SerializeField]
	private Canvas _controlsCanvas;
#pragma warning restore CS0649

	private void Awake()
	{
		_timelines = FindObjectOfType<TimelineManager>();
	}

	public override void EnterTimeline()
	{
		_controlsCanvas.enabled = true;
	}
	public override void ExitTimeline()
	{
		_controlsCanvas.enabled = false;
	}


	public override void UpdateTimeline(float dt)
	{
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))
		{
			_timelines.ActivateTimeline("game");
			_timelines.DeactiveTimeline("menu");
		}
	}
}
