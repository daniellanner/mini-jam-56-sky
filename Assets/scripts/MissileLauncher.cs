﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using io.daniellanner.indiversity;
using System.Linq;

[TimelineDescription("game")]
public class MissileLauncher : MonoBehaviourInTimeline
{
	private const float BREADTH = 3.5f;
	private const float BREADTH_OFF = 2.5f;
	private readonly Vector3 RENDER_OFFSET = new Vector3(0, 0, .1f);

#pragma warning disable CS0649
	[SerializeField]
	private GameObject _missileGO;
	[SerializeField]
	private GameObject _missileShadow;
#pragma warning restore CS0649

	#region Cache
	private ParticlePool _dirtParticles;
	private LinePool _lines;
	private SimpleRadialCollision _collision;
	private CameraController _camera;
	private AudioSource _audio;
	#endregion

	#region State
	private float _shootInterval = 4f;
	private float _missileDelay = 1.5f;
	private float _currentCtr = 0f;
	#endregion

	private void Awake()
	{
		_dirtParticles = FindObjectsOfType<ParticlePool>().First(it => it.gameObject.tag == "Dirt");
		_lines = FindObjectOfType<LinePool>();
		_collision = FindObjectOfType<SimpleRadialCollision>();
		_camera = FindObjectOfType<CameraController>();
		_audio = GetComponent<AudioSource>();
	}
	
	public override void EnterTimeline()
	{
		_shootInterval = 4f;
		_missileDelay = 1.5f;
		_currentCtr = 0f;

		var children = GetComponentsInChildren<Transform>();
		for (int i = children.Length - 1; i >= 0; i--)
		{
			if(children[i].gameObject == gameObject)
			{
				continue;
			}

			Destroy(children[i].gameObject);
		}
	}

	public override void UpdateTimeline(float dt)
	{
		_currentCtr += dt;

		if(_currentCtr > _shootInterval)
		{
			Shoot();
			_currentCtr = 0f;

			_shootInterval -= .2f;
			_shootInterval = Mathf.Clamp(_shootInterval, .75f, 4f);

			_missileDelay -= 0.05f;
			_missileDelay = Mathf.Clamp(_missileDelay, 0.5f, 1.5f);
		}
	}

	void Shoot()
	{
		Vector3 from = new Vector3(Random.Range(-BREADTH, BREADTH), 15f, 0f);
		Vector3 to = new Vector3(Random.Range(-BREADTH, BREADTH), 0f, Random.Range(-BREADTH_OFF, BREADTH_OFF));

		to.z += Mathf.Sign(to.z) * 0.5f; // make space for our water pools

		GameObject miss = Instantiate(_missileGO, from, Quaternion.identity, transform);
		miss.transform.LookAt(from + Vector3.forward, from - to);

		_collision.AddHazard(miss.transform);
		var line = _lines?.RequestLine(from + RENDER_OFFSET, to + RENDER_OFFSET);

		Instantiate(_missileShadow, to, Quaternion.identity, transform);

		_audio.pitch = Random.Range(0.95f, 1.05f);
		_audio.PlayDelayed(_missileDelay + .15f);

		var trans = new CoroutineTransformPosition(miss.transform, from, to)
			.SetInterpolation(new ExponentialInterpolation())
			.SetDelay(_missileDelay)
			.SetDuration(.5f)
			.SetCallback(() =>
			{
				// cleanup
				_collision.RemoveHazard(miss.transform);
				if (line)
				{
					_lines?.ResetLine(line.Value);
				}
				
				// impact
				var res = _dirtParticles?.RequestAndPlayParticles(to);
				_camera?.MissileImpact();
			});

		StartCoroutine(trans.GetIEnumerator());
	}
}
