using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace io.daniellanner.indiversity
{
	public class LoadingScreenAnimationDeployer : MonoBehaviour
	{

		public delegate void DoneEvent();
		public static DoneEvent Done;

		private List<ILoadingSceneTransitionAnimation> _overlayAnimations = new List<ILoadingSceneTransitionAnimation>();
		private int _activeOverlayAnimationCount = 0;

		private List<ILoadingSceneTransitionAnimation> _heroAnimations = new List<ILoadingSceneTransitionAnimation>();
		private int _activeHeroAnimationCount = 0;

		public void AddAnimation(ILoadingSceneTransitionAnimation p_animation, bool p_isOverlay = true)
		{
			if (p_isOverlay)
			{
				_overlayAnimations.Add(p_animation);
			}
			else
			{
				_heroAnimations.Add(p_animation);
			}
		}

		public LoadinScreenAnimationWait OverlayScenes()
		{
			FireShowOverlayAnimations();
			return new LoadinScreenAnimationWait();
		}

		private void FireShowOverlayAnimations()
		{
			foreach (var it in _overlayAnimations)
			{
				_activeOverlayAnimationCount++;
				it.Close(() =>
				{
					_activeOverlayAnimationCount--;
					if (_activeOverlayAnimationCount == 0)
						Done?.Invoke();
				});
			}

			foreach (var it in _heroAnimations)
			{
				_activeHeroAnimationCount++;
				it.Close(() =>
				{
					_activeHeroAnimationCount--;
				});
			}
		}

		public LoadinScreenAnimationWait ClearScenes()
		{
			InvokeClosing();
			return new LoadinScreenAnimationWait();
		}

		private void InvokeClosing()
		{
			if (_activeHeroAnimationCount <= 0)
			{
				FireHideOverlayAnimations();
			}
			else
			{
				Invoke("InvokeClosing", .5f);
			}
		}

		private void FireHideOverlayAnimations()
		{


			foreach (var it in _heroAnimations)
			{
				_activeHeroAnimationCount++;
				it.Open(() =>
				{
					_activeHeroAnimationCount--;
					if (_activeOverlayAnimationCount == 0 && _activeHeroAnimationCount == 0)
						Done?.Invoke();
				});
			}

			foreach (var it in _overlayAnimations)
			{
				_activeOverlayAnimationCount++;
				it.Open(() =>
				{
					_activeOverlayAnimationCount--;
					if (_activeOverlayAnimationCount == 0 && _activeHeroAnimationCount == 0)
						Done?.Invoke();
				});
			}
		}
	}
}