using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace io.daniellanner.indiversity
{
	#region 3D Base
	public abstract class CoroutineAnimationTransform : CoroutineAnimationBase
	{
		protected Transform _transform;

		public Vector3 From { get; set; }
		public Vector3 To { get; set; }

		public CoroutineAnimationTransform(Transform transform, Vector3 from, Vector3 to) :
				base()
		{
			_transform = transform;

			From = from;
			To = to;
		}

	}

	static class CoroutineAnimationTransformChaining
	{
		static public T SetFrom<T>(this T o, Vector3 p_from) where T : CoroutineAnimationTransform
		{
			o.From = p_from;
			return o;
		}

		static public T SetTo<T>(this T o, Vector3 p_to) where T : CoroutineAnimationTransform
		{
			o.To = p_to;
			return o;
		}
	}
	#endregion

	#region Scale
	public class CoroutineTransformScale : CoroutineAnimationTransform
	{
		public CoroutineTransformScale(Transform transform, Vector3 from, Vector3 to) :
				base(transform, from, to)
		{
		}

		protected override void ApplyDelta(float t)
		{
			_transform.localScale = Vector3.Lerp(From, To, t);
		}
	}
	#endregion

	#region Position
	public class CoroutineTransformPosition : CoroutineAnimationTransform
	{
		public CoroutineTransformPosition(Transform transform, Vector3 from, Vector3 to) :
				base(transform, from, to)
		{
		}

		protected override void ApplyDelta(float t)
		{
			_transform.position = Vector3.Lerp(From, To, t);
		}
	}
	#endregion

	#region Wiggle
	public class CoroutineTransformWiggle : CoroutineAnimationTransform
	{
		public CoroutineTransformWiggle(Transform transform, Vector3 from, Vector3 to) :
				base(transform, from, to)
		{
		}

		protected override void ApplyDelta(float t)
		{
			Vector3 random = new Vector3
			{
				x = Random.Range(-To.x, To.x),
				y = Random.Range(-To.y, To.y),
				z = Random.Range(-To.z, To.z)
			};

			random *= t;

			_transform.position = From + random;
		}
	}
	#endregion


}