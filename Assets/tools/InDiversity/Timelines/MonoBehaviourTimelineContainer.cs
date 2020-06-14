using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace io.daniellanner.indiversity
{
	public class TimelineDescription : System.Attribute
	{
		public TimelineDescription(string id, bool playOnAwake = false)
		{
			ID = id;
			PlayOnAwake = playOnAwake;
		}

		public string ID { get; set; }
		public bool PlayOnAwake { get; set; }
	}
}