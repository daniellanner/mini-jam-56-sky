using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using io.daniellanner.indiversity;

[TimelineDescription("main", true)]
public class PlayerController : MonoBehaviourInTimeline
{ 
	private const float TWO_PI = 6.28318548f;
	private const float MIN_DILATION_DISTANCE = 1f;
	private const float MAX_DILATION_DISTANCE = 3.25f;

	#region Properties
	[Header("Rotation Settings")]
	[SerializeField]
	[Range(.01f, 1f)]
	[Tooltip("Time in seconds for rotation to reach max speed")]
	private float _rotationGain = .25f;

	[SerializeField]
	[Range(.01f, 1f)]
	[Tooltip("Time in seconds for rotation to stop")]
	private float _rotationDampening = .1f;

	[SerializeField]
	[Tooltip("Time in seconds for a full rotation")]
	private float _rotationSpeed = 1f;

	[Header("Dilation Settings")]
	[SerializeField]
	[Range(.01f, 1f)]
	[Tooltip("Time in seconds for dilation to reach max speed")]
	private float _dilationGain = .1f;

	[SerializeField]
	[Range(.01f, 1f)]
	[Tooltip("Time in seconds for dilation to reach max speed")]
	private float _dilationDampening = .05f;

	[SerializeField]
	[Tooltip("Distance in metres for dilation movement per second")]
	private float _dilationSpeed = 5f;
	#endregion

	#region State
	private float _rotationInput = 0f;
	private float _rotationOffset = 0f;

	private float _dilationInput = 0f;
	private float _dilationOffset = 2f;

	private float _count = 0f;
	private float _inputDirection = -1f;
	#endregion

	private List<PlayerElement> _elements;

	private void Awake()
	{
		_elements = GetComponentsInChildren<PlayerElement>().ToList();
	}

	// Update is called once per frame
	public override void UpdateTimeline(float dt)
	{
		CheckForInputInvert();

		// dirty dirty movement easing
		// instead of using physics just gain speed with input and dampen speed when no input
		if (HasPlayerRotationInput())
		{
			float rotGainInverse = 1f / _rotationGain;
			_rotationInput += GetPlayerRotationInput() * dt * rotGainInverse;
		}
		else
		{
			float rotDampInverse = 1f / _rotationDampening;
			_rotationInput *= 1f - dt * rotDampInverse;
		}

		if (HasPlayerDilusionInput())
		{
			float dilGainInverse = 1f / _dilationGain;
			_dilationInput += GetPlayerDilusionInput() * dt * dilGainInverse;
		}
		else
		{
			float dilDampInverse = 1f / _dilationDampening;
			_dilationInput *= 1f - dt * dilDampInverse;
		}

		_dilationInput = Mathf.Clamp(_dilationInput, -1f, 1f);
		_rotationInput = Mathf.Clamp(_rotationInput, -1f, 1f);

		_dilationOffset += _dilationInput * dt * _dilationSpeed;
		_rotationOffset += _rotationInput * dt * TWO_PI * _rotationSpeed;
		
		_dilationOffset = Mathf.Clamp(_dilationOffset, MIN_DILATION_DISTANCE, MAX_DILATION_DISTANCE);

		// apply new offset

		float count = 0f;
		for (int i = _elements.Count - 1; i >= 0f; i--)
		{
			if (!_elements[i].Dead)
			{
				count++;
			}
			else
			{
				_elements.RemoveAt(i);
			}
		}

		_count = Mathf.Lerp(_count, count, Time.deltaTime * 10f);

		if(_count < 0.01f)
		{
			return;
		}

		for (int i = 0; i < _elements.Count; i++)
		{
			Vector3 pos = _elements[i].transform.localPosition;

			pos.x = Mathf.Sin((float)i / _count * TWO_PI + _rotationOffset) * _dilationOffset;
			pos.y = Mathf.Cos((float)i / _count * TWO_PI + _rotationOffset) * _dilationOffset;

			_elements[i].transform.localPosition = pos;
		}
	}

	private float GetPlayerRotationInput()
	{
		return Input.GetAxis("Horizontal") * _inputDirection;
	}

	private bool HasPlayerRotationInput()
	{
		return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Mathf.Abs(Input.GetAxisRaw("Horizontal")) > .1f;
	}

	private float GetPlayerDilusionInput()
	{
		return Input.GetAxis("Vertical") * _inputDirection;
	}

	private bool HasPlayerDilusionInput()
	{
		return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Mathf.Abs(Input.GetAxisRaw("Vertical")) > .1f;
	}

	private void CheckForInputInvert()
	{
		if (Input.GetKeyDown(KeyCode.I) || Input.GetButtonDown("Invert"))
		{
			_inputDirection = -_inputDirection;
		}
	}
}
