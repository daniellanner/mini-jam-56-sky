using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace io.daniellanner.indiversity
{
	public class CoroutineGraphicColor : CoroutineAnimationBase
	{
		protected UnityEngine.UI.Graphic _graphic;
		public Color From { get; set; }
		public Color To { get; set; }

		public CoroutineGraphicColor(UnityEngine.UI.Graphic graphic, Color from, Color to)
		{
			_graphic = graphic;
			From = from;
			To = to;
		}

		protected override void ApplyDelta(float t)
		{
			_graphic.color = Color.Lerp(From, To, t);
		}
	}

	static class CoroutineGraphicColorChaining
	{
		static public T SetFrom<T>(this T o, Color p_from) where T : CoroutineGraphicColor
		{
			o.From = p_from;
			return o;
		}

		static public T SetTo<T>(this T o, Color p_to) where T : CoroutineGraphicColor
		{
			o.To = p_to;
			return o;
		}
	}
}