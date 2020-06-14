using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using io.daniellanner.indiversity;

public class PlayerElement : MonoBehaviour
{
	private bool _dead = false;
	public bool Dead => _dead;

	private ParticlePool _waterfallParticles;
	private GoPool _poolElements;

	private void Awake()
	{
		_waterfallParticles = FindObjectsOfType<ParticlePool>().First(it => it.gameObject.tag == "Pool");
		_poolElements = FindObjectsOfType<GoPool>().First(it => it.gameObject.tag == "Pool");
	}

	public void Die()
	{
		if(_dead)
		{
			return;
		}

		_dead = true;

		transform.localScale = Vector3.zero;
		_waterfallParticles.RequestAndPlayParticles(transform.position);

		Vector3 pos = transform.position;
		pos.y = 0.01f; // small offset for render

		var element = _poolElements.RequestObject(pos);
		
		if(element)
		{
			element.Value.transform.localScale = Vector3.zero;

			var scale = new CoroutineTransformScale(element.Value, Vector3.zero, Vector3.one)
			.SetDelay(.5f);
			
			StartCoroutine(scale.GetIEnumerator());
		}
	}
}
