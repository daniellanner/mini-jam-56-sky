using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SimpleRadialCollision : MonoBehaviour
{
	[SerializeField]
	private float _collisionDistance = 2f;

	private float _collisionDistanceSqrd = 4f;

	private List<PlayerElement> _playerElements = new List<PlayerElement>();
	private List<Transform> _hazards = new List<Transform>();

	private void Awake()
	{
		_playerElements = FindObjectsOfType<PlayerElement>().ToList();
	}

	private void Start()
	{
		_collisionDistanceSqrd = _collisionDistance * _collisionDistance;
	}

	public void AddHazard(Transform tr)
	{
		_hazards.Add(tr);
	}

	public bool RemoveHazard(Transform tr)
	{
		return _hazards.Remove(tr);
	}

	private void Update()
	{
		foreach(var hazard in _hazards)
		{
			// go through backwards since we delete object from list
			for (int i = _playerElements.Count-1; i >= 0; i--)
			{

				// project to 0 z-axis
				Vector3 hazardPosition = hazard.position;
				hazardPosition.z = 0f;

				if (Vector3.SqrMagnitude(hazardPosition - _playerElements[i].transform.position) <= _collisionDistanceSqrd)
				{
					_playerElements[i].Die();
					_playerElements.RemoveAt(i);
				}
			}
		}
	}
}
