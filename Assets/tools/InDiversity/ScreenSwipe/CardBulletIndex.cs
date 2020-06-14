using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace io.daniellanner.indiversity
{
	public class CardBulletIndex : MonoBehaviour, ICardIndexVisual
	{
		public SwipeCardMovement _swipingBehaviour;
		private Image[] _bullets;

		public void SetActiveCard(int p_idx)
		{
			Color c;

			for (int i = 0; i < _bullets.Length; i++)
			{
				c = _bullets[i].color;
				c.a = (i == p_idx) ? 1f : .2f;

				_bullets[i].color = c;
			}
		}

		private void Awake()
		{
			_bullets = GetComponentsInChildren<Image>();
			_swipingBehaviour.visualIndex = this;
		}
	}
}
