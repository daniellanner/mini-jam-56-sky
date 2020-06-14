using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace io.daniellanner.indiversity
{
	public class TimelineManager : MonoBehaviour
	{
		private List<MonoBehaviourInTimeline> _activeTimelines = new List<MonoBehaviourInTimeline>();
		private List<string> _activeTimelineIDs = new List<string>();
		private Dictionary<string, List<MonoBehaviourInTimeline>> _allTimelines = new Dictionary<string, List<MonoBehaviourInTimeline>>();

		private void AddToActiveTimeline(string id)
		{
			if (_activeTimelineIDs.Contains(id))
			{
				return;
			}

			_activeTimelineIDs.Add(id);
		}

		private void BuildActiveTimelines()
		{
			_activeTimelines.Clear();

			foreach (var id in _activeTimelineIDs)
			{
				_activeTimelines.AddRange(_allTimelines[id]);
			}
		}

		public void DeactiveTimeline(string id)
		{
			_activeTimelineIDs.Remove(id);

			if (_allTimelines.ContainsKey(id))
			{
				_allTimelines[id].ForEach(it => it.ExitTimeline());
			}
		}

		public void ActivateTimeline(string id)
		{
			AddToActiveTimeline(id);

			if (_allTimelines.ContainsKey(id))
			{
				_allTimelines[id].ForEach(it => it.EnterTimeline());
			}
		}

		private void OnEnable()
		{
			var obj = FindObjectsOfType<MonoBehaviour>().OfType<MonoBehaviourInTimeline>();

			string id;
			bool active;

			foreach (var it in obj)
			{
				var desc = (TimelineDescription)System.Attribute.GetCustomAttribute(it.GetType(), typeof(TimelineDescription));

				if (desc == null)
				{
					Debug.LogWarning($"{it.GetType()} does not define a TimelineDescription and will be executed constantly. To change this please add a TimelineDescription attribute.");

					id = "default";
					active = true;
				}
				else
				{
					id = desc.ID;
					active = desc.PlayOnAwake;
				}

				if (active)
				{
					AddToActiveTimeline(id);
				}

				if (!_allTimelines.ContainsKey(id))
				{
					_allTimelines.Add(id, new List<MonoBehaviourInTimeline>());
				}

				_allTimelines[id].Add(it);
			}

			BuildActiveTimelines();
		}

		private void Start()
		{
			foreach (var it in _activeTimelines)
			{
				it.EnterTimeline();
			}
		}

		private void Update()
		{
			foreach (var it in _activeTimelines)
			{
				it.UpdateTimeline(Time.deltaTime);
			}

			// Cleanup remove or adds
			BuildActiveTimelines();
		}
	}
}