using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using io.daniellanner.indiversity;
using System.Linq;

[TimelineDescription("epilogue")]
public class Epilogue : MonoBehaviourInTimeline
{
	private MissileLauncher _missileParent;
	private List<Transform> _trees = new List<Transform>();

#pragma warning disable CS0649
	[SerializeField]
	private Canvas _quote;
#pragma warning restore CS0649

	private void Awake()
	{
		_missileParent = FindObjectOfType<MissileLauncher>();
	}

	public override void EnterTimeline()
	{
		_trees = _missileParent.GetComponentsInChildren<TrailRenderer>().Select(it => it.transform).ToList(); // our missile prefab has a trailrenderer
		Invoke("ShowCanvas", 2f);
	}

	public override void ExitTimeline()
	{
		_trees.Clear();

		CancelInvoke();
		_quote.enabled = false;
	}

	public override void UpdateTimeline(float dt)
	{
		if (_trees != null)
		{
			for (int i = 0; i < _trees.Count; i++)
			{
				if (_trees[i] != null)
				{
					_trees[i].rotation = Quaternion.Slerp(_trees[i].rotation, Quaternion.identity, dt * .25f);
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))
		{
			Timelines.DeactiveTimeline("epilogue");
			Timelines.ActivateTimeline("game");
		}
	}

	private void ShowCanvas()
	{
		_quote.enabled = true;
	}
}
