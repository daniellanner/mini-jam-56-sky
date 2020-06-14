using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using io.daniellanner.indiversity;

public class ParticlePool : PoolBase
{
	private ParticleSystem[] _particles;
	private int _particleIdx = 0;

	private void OnEnable()
	{
		_particles = GetComponentsInChildren<ParticleSystem>();
	}

	public Result<ParticleSystem> RequestAndPlayParticles(Vector3 position)
	{
		var result = GetObject<ParticleSystem>(_particles, ref _particleIdx);

		if (result)
		{
			result.Value.transform.position = position;
			result.Value.Play();
		}

		return result;
	}
}
