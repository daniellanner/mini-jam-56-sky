using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace io.daniellanner.indiversity
{
	public class Result<T>
	{
		private bool _safe;
		private T _value;

		public Result(T p_value)
		{
			_value = p_value;
			_safe = false;
		}

		public Result<T> AsSafe()
		{
			_safe = true;
			return this;
		}

		public T Value => _value;

		public static implicit operator bool(Result<T> foo)
		{
			return foo._safe;
		}

		public static Result<T> FALSE => new Result<T>(default);
	}
}