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

	private Transform _cam;

	private void Awake()
	{
		_playerElements = FindObjectsOfType<PlayerElement>().ToList();
		_cam = Camera.main.transform;
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
				UnityEngine.Profiling.Profiler.BeginSample("Collision Projection");
				
				// project position to origin plane
				Vector3 viewDirection = hazard.position - _cam.position;

				Ray ray = new Ray(_cam.position, viewDirection);
				Plane plane = new Plane(new Vector3(0f, 0f, -1f), 0f);
				plane.Raycast(ray, out float enter);

				Vector3 hazardPosition = _cam.position + viewDirection.normalized * enter;

				if (Vector3.SqrMagnitude(hazardPosition - _playerElements[i].transform.position) <= _collisionDistanceSqrd)
				{
					_playerElements[i].Die();
				}
				UnityEngine.Profiling.Profiler.EndSample();
			}
		}
	}
}
