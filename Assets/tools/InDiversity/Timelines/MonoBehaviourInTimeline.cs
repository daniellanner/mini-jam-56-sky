using UnityEngine;

namespace io.daniellanner.indiversity
{
	public class MonoBehaviourInTimeline : MonoBehaviour
	{
		public TimelineManager Timelines { get; set; }

		public virtual void EnterTimeline() { }
		public virtual void ExitTimeline() { }
		public virtual void UpdateTimeline(float dt) { }
	}
}
