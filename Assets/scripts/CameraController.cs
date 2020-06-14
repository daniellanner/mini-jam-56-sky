using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using io.daniellanner.indiversity;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Radius in metres for the camera shake")]
	private float _missileImpactCameraShakeStrength = .5f;

	public void MissileImpact()
	{
		Shake(_missileImpactCameraShakeStrength);
	}

	private void Shake(float strength)
	{
		var wiggle = new CoroutineTransformWiggle(transform, transform.position, Vector3.one * strength)
		.SetDuration(.5f)
		.SetInterpolation(new InverseExponentialInterpolation());

		StopAllCoroutines();
		StartCoroutine(wiggle.GetIEnumerator());
	}
}
