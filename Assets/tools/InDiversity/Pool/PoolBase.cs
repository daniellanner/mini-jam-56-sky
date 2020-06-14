using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace io.daniellanner.indiversity
{
	public class PoolBase : MonoBehaviour
	{
		protected Result<T> GetObject<T>(T[] p_array, ref int p_counter)
		{
			if (p_array == null || p_array.Length == 0)
			{
				return Result<T>.FALSE;
			}

			if (p_counter >= p_array.Length)
			{
				p_counter = 0;
			}

			if(p_array[p_counter] == null)
			{
				return Result<T>.FALSE;
			}

			var result = new Result<T>(p_array[p_counter]);

			p_counter++;
			if (p_counter >= p_array.Length)
			{
				p_counter = 0;
			}

			return result.AsSafe();
		}
	}
}