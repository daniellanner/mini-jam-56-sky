using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace io.daniellanner.indiversity
{
	public class LoadingSceneTransitionAnimationBarnDoors : MonoBehaviour, ILoadingSceneTransitionAnimation
	{

		[SerializeField]
		private float _openYPosition;
		[SerializeField]
		private float _closedYPosition;
		[SerializeField]
		private float _duration;

		private void Awake()
		{
			FindObjectOfType<LoadingScreenAnimationDeployer>()?.AddAnimation(this);
		}

		public void Close(Action p_callback)
		{
			CoroutineAnchoredPosition translation = new CoroutineAnchoredPosition(
					transform: GetComponent<RectTransform>(),
				from: new Vector2(0, _openYPosition),
				to: new Vector2(0, _closedYPosition));

			StartCoroutine(translation.GetIEnumerator());
		}

		public void Open(Action p_callback)
		{
			CoroutineAnchoredPosition translation = new CoroutineAnchoredPosition(
					transform: GetComponent<RectTransform>(),
				from: new Vector2(0, _closedYPosition),
				to: new Vector2(0, _openYPosition));

			StartCoroutine(translation.GetIEnumerator());
		}
	}
}