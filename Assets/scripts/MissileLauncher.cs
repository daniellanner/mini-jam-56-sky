using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using io.daniellanner.indiversity;
using System.Linq;

[TimelineDescription("game")]
public class MissileLauncher : MonoBehaviourInTimeline
{
	private const float BREADTH = 5f;
	private const float BREADTH_OFF = 2.5f;
	private readonly Vector3 RENDER_OFFSET = new Vector3(0, 0, .1f);

	public GameObject missileGO;
	public GameObject missileShadow;

	private ParticlePool _dirtParticles;
	private LinePool _lines;
	private SimpleRadialCollision _collision;
	private CameraController _camera;

	private float _shootInterval = 2f;
	private float _currentCtr = 0f;

	private void Awake()
	{
		_dirtParticles = FindObjectsOfType<ParticlePool>().First(it => it.gameObject.tag == "Dirt");
		_lines = FindObjectOfType<LinePool>();
		_collision = FindObjectOfType<SimpleRadialCollision>();
		_camera = FindObjectOfType<CameraController>();
	}
	
	public override void EnterTimeline()
	{
		_shootInterval = 2f;
		_currentCtr = 0f;
	}

	public override void UpdateTimeline(float dt)
	{
		_currentCtr += dt;

		if(_currentCtr > _shootInterval)
		{
			Shoot();
			_currentCtr = 0f;

			// TODO: (Daniel) adapt shoot interval to speed up
		}
	}

	void Shoot()
	{
		Vector3 from = new Vector3(Random.Range(-BREADTH, BREADTH), 15f, 0f);
		Vector3 to = new Vector3(Random.Range(-BREADTH, BREADTH), 0f, Random.Range(-BREADTH_OFF, BREADTH_OFF));

		to.z += Mathf.Sign(to.z) * 0.5f; // make space for our water pools

		GameObject miss = Instantiate(missileGO, from, Quaternion.identity);
		miss.transform.LookAt(from + Vector3.forward, from - to);

		_collision.AddHazard(miss.transform);
		var line = _lines?.RequestLine(from + RENDER_OFFSET, to + RENDER_OFFSET);

		Instantiate(missileShadow, to, Quaternion.identity);

		var trans = new CoroutineTransformPosition(miss.transform, from, to)
			.SetInterpolation(new ExponentialInterpolation())
			.SetDelay(1f)
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
