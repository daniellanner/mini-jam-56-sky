using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace io.daniellanner.indiversity
{
	public interface IInterpolationMethod
	{
		float Interpolate(float x);
	}

	public class EaseInterpolation : IInterpolationMethod
	{
		private float _easeFactor = 2f;

		public EaseInterpolation(float p_easeFactor = 2f)
		{
			_easeFactor = p_easeFactor;
		}

		public float Interpolate(float x)
		{
			return Mathf.Pow(x, _easeFactor) / (Mathf.Pow(x, _easeFactor) + Mathf.Pow(1f - x, _easeFactor));
		}
	}

	public class ExponentialInterpolation : IInterpolationMethod
	{
		private float _easeFactor = 2f;

		public ExponentialInterpolation(float p_easeFactor = 2f)
		{
			_easeFactor = p_easeFactor;
		}

		public float Interpolate(float x)
		{
			return Mathf.Pow(x, _easeFactor);
		}
	}

	public class OneMinusExponentialInterpolation : IInterpolationMethod
	{
		private float _easeFactor = 2f;

		public OneMinusExponentialInterpolation(float p_easeFactor = 2f)
		{
			_easeFactor = p_easeFactor;
		}

		public float Interpolate(float x)
		{
			return 1f - Mathf.Pow(x, _easeFactor);
		}
	}

	public class InverseExponentialInterpolation : IInterpolationMethod
	{
		private float _easeFactor = 2f;

		public InverseExponentialInterpolation(float p_easeFactor = 2f)
		{
			p_easeFactor = _easeFactor;
		}

		public float Interpolate(float x)
		{
			return Mathf.Pow(1f - x, _easeFactor);
		}
	}

	public class OneMinusInverseExponentialInterpolation : IInterpolationMethod
	{
		private float _easeFactor = 2f;

		public OneMinusInverseExponentialInterpolation(float p_easeFactor = 2f)
		{
			p_easeFactor = _easeFactor;
		}

		public float Interpolate(float x)
		{
			return 1f - Mathf.Pow(1f - x, _easeFactor);
		}
	}

	public class LinearInterpolation : IInterpolationMethod
	{
		public float Interpolate(float x)
		{
			return x;
		}
	}

	public class LinearBell : IInterpolationMethod
	{
		public float Interpolate(float x)
		{
			return Mathf.Sin(x * Mathf.PI * 2f);
		}
	}

	public class SoftBell : IInterpolationMethod
	{
		public float Interpolate(float x)
		{
			return (Mathf.Sin(x * Mathf.PI * 2f - (Mathf.PI * .5f)) + 1f) * .5f;
		}
	}

	public class ExponentialSoftBell : IInterpolationMethod
	{
		private float _easeFactor = 2f;

		public ExponentialSoftBell(float p_easeFactor = 2f)
		{
			_easeFactor = p_easeFactor;
		}

		public float Interpolate(float x)
		{
			float bell = (Mathf.Sin(x * Mathf.PI * 2f - (Mathf.PI * .5f)) + 1f) * .5f;
			return Mathf.Pow(bell, _easeFactor) / (Mathf.Pow(bell, _easeFactor) + Mathf.Pow(1f - bell, _easeFactor));
		}
	}

	public class InverseExponentialSoftBell : IInterpolationMethod
	{
		private float _easeFactor = 2f;

		public InverseExponentialSoftBell(float p_easeFactor = 2f)
		{
			_easeFactor = p_easeFactor;
		}

		public float Interpolate(float x)
		{
			float bell = (Mathf.Sin(x * Mathf.PI * 2f - (Mathf.PI * .5f)) + 1f) * .5f;
			return 1f - Mathf.Pow(1f - bell, _easeFactor);
		}
	}

	public class AnimationCurveEvaluaiton : IInterpolationMethod
	{
		AnimationCurve _curve;
		public AnimationCurveEvaluaiton(AnimationCurve p_curve)
		{
			_curve = p_curve;
		}

		public float Interpolate(float x)
		{
			return _curve.Evaluate(x);
		}
	}
}