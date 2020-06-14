using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace io.daniellanner.indiversity
{
	#region Shader Base
	public abstract class CoroutineAnimationShaderProperty : CoroutineAnimationBase
	{
		protected Material _material;

		public int PropertyId { get; set; }

		public CoroutineAnimationShaderProperty(Material p_material, int p_propertyId)
		{
			_material = p_material;
			PropertyId = p_propertyId;
		}

		public CoroutineAnimationShaderProperty(Material p_material, string p_property)
		{
			_material = p_material;
			PropertyId = Shader.PropertyToID(p_property);
		}
	}

	static class CoroutineAnimationShaderPropertyChaining
	{
		static public T SetShaderProperty<T>(this T o, int p_property) where T : CoroutineAnimationShaderProperty
		{
			o.PropertyId = p_property;
			return o;
		}

		static public T SetShaderProperty<T>(this T o, string p_property) where T : CoroutineAnimationShaderProperty
		{
			o.PropertyId = Shader.PropertyToID(p_property);
			return o;
		}
	}
	#endregion

	#region Shader Float
	public class CoroutineShaderFloat : CoroutineAnimationShaderProperty
	{
		public float From { get; set; }
		public float To { get; set; }

		public CoroutineShaderFloat(Material p_material, int p_propertyId, float from, float to) : base(p_material, p_propertyId)
		{
			From = from;
			To = to;
		}

		public CoroutineShaderFloat(Material p_material, string p_property, float from, float to) : base(p_material, p_property)
		{
			From = from;
			To = to;
		}
		protected override void ApplyDelta(float t)
		{
			_material.SetFloat(PropertyId, Mathf.Lerp(From, To, t));
		}
	}

	static class CoroutineShaderFloatChaining
	{
		static public T SetFrom<T>(this T o, float from) where T : CoroutineShaderFloat
		{
			o.From = from;
			return o;
		}

		static public T SetTo<T>(this T o, float to) where T : CoroutineShaderFloat
		{
			o.To = to;
			return o;
		}
	}
	#endregion
}