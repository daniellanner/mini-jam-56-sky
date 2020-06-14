using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElement : MonoBehaviour
{
	private bool _dead = false;
	public bool Dead => _dead;

	public void Die()
	{
		transform.localScale = Vector3.one * .5f;
		_dead = true;

		
	}
}
