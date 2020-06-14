using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace io.daniellanner.indiversity
{
#region Recttransform base
	public abstract class CoroutineAnimationRectTransform : CoroutineAnimationBase
	{
		protected RectTransform _transform;

		public Vector2 From { get; set; }
		public Vector2 To { get; set; }

		public CoroutineAnimationRectTransform(RectTransform transform, Vector2 from, Vector3 to)
		{
			_transform = transform;

			From = from;
			To = to;
		}
	}

	static class CoroutineAnimationRectTransformChaining
	{
		static public T SetFrom<T>(this T o, Vector2 p_from) where T : CoroutineAnimationRectTransform
		{
			o.From = p_from;
			return o;
		}

		static public T SetTo<T>(this T o, Vector2 p_to) where T : CoroutineAnimationRectTransform
		{
			o.To = p_to;
			return o;
		}
	}
	#endregion

	#region Translation
	public class CoroutineAnchoredPosition : CoroutineAnimationRectTransform
	{
		public CoroutineAnchoredPosition(RectTransform transform, Vector2 from, Vector3 to) : base(transform, from, to)
		{
		}

		protected override void ApplyDelta(float t)
		{
			_transform.anchoredPosition = Vector2.Lerp(From, To, t);
		}
	}
	#endregion

	#region Size Delta
	public class CoroutineSizeDelta : CoroutineAnimationRectTransform
	{
		public CoroutineSizeDelta(RectTransform transform, Vector2 from, Vector2 to) : base(transform, from, to)
		{
		}

		protected override void ApplyDelta(float t)
		{
			_transform.sizeDelta = Vector2.Lerp(From, To, t);
		}
	}
	#endregion
}