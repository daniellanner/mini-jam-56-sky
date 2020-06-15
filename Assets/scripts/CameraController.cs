using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using io.daniellanner.indiversity;

[TimelineDescription("epilogue")]
public class CameraController : MonoBehaviourInTimeline
{
	[SerializeField]
	[Tooltip("Radius in metres for the camera shake")]
	private float _missileImpactCameraShakeStrength = .5f;

	private readonly Vector3 DEFAULT_POSITION = new Vector3(0f, 3f, -10f);

	public void MissileImpact()
	{
		Shake(_missileImpactCameraShakeStrength);
	}

	public override void EnterTimeline()
	{
		StopAllCoroutines();
		GetComponent<Animation>().Play();
	}

	public override void ExitTimeline()
	{
		GetComponent<Animation>().Stop();
		transform.position = DEFAULT_POSITION;
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
