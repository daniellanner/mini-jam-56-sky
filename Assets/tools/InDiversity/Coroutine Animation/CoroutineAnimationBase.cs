using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace io.daniellanner.indiversity
{
	public abstract class CoroutineAnimationBase
	{
		public float Duration { get; set; }
		public float Delay { get; set; }
		public float T { get; set; }
		public System.Action Callback { get; set; }
		public IInterpolationMethod Interpolation { get; set; }
		public int Repeat { get; set; }

		public CoroutineAnimationBase()
		{
			Duration = 1f;
			Interpolation = new EaseInterpolation();
			Callback = null;
			Delay = 0f;
		}
		
		protected abstract void ApplyDelta(float t);

		public virtual IEnumerator GetIEnumerator()
		{
			yield return new WaitForSeconds(Delay);
			int currentRepeat = 0;

			do
			{
				if (Duration > 0f)
				{
					T = 0;
					while (T <= 1f)
					{
						ApplyDelta(Interpolation.Interpolate(T));
						T += Time.deltaTime / Duration;
						yield return null;
					}
				}

				T = 1f;
				ApplyDelta(Interpolation.Interpolate(T));

				currentRepeat++;
			} while (currentRepeat < Repeat);

			Callback?.Invoke();
		}
	}

	static class CoroutineAnimationBaseChaining
	{
		static public T ResetAll<T>(this T o) where T : CoroutineAnimationBase
		{
			o.T = 0f;
			o.Delay = 0f;
			o.Callback = null;
			o.Repeat = 0;
			return o;
		}

		static public T SetDuration<T>(this T o, float p_duration) where T : CoroutineAnimationBase
		{
			o.Duration = p_duration;
			return o;
		}

		static public T SetDelay<T>(this T o, float p_delay) where T : CoroutineAnimationBase
		{
			o.Delay = p_delay;
			return o;
		}

		static public T SetCallback<T>(this T o, System.Action p_callback) where T : CoroutineAnimationBase
		{
			o.Callback = p_callback;
			return o;
		}

		static public T SetInterpolation<T>(this T o, IInterpolationMethod p_interpolation) where T : CoroutineAnimationBase
		{
			o.Interpolation = p_interpolation;
			return o;
		}

		static public T SetRepeat<T>(this T o, int p_repeat) where T : CoroutineAnimationBase
		{
			o.Repeat = p_repeat;
			return o;
		}
	}
}