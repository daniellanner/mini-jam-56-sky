using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace io.daniellanner.indiversity
{
	public class SwipeCardMovement : MonoBehaviour
	{
		#region Exposed Properties
		[Header("Swipe Settings")]
		[Tooltip("The maximum amount of distance in pixels the elemtns will travel while dragging.")]
		[SerializeField]
		private float _maxTravelDistance = 160f;
		[Tooltip("Minimum normalized width of screen to move to be recognised as a swipe event.")]
		[Range(0, 1)]
		[SerializeField]
		private float _screenWidthForSwipe = 0.1f;
		[Tooltip("Normalized screen width used to move the elements the maximum distance.")]
		[Range(0,1)]
		[SerializeField]
		private float _screenWidthForMaxDistance = 1f;
		[Tooltip("How long it takes to translate to the new active screen after letting it go.")]
		[Range(0, 1)]
		[SerializeField]
		private float _swipeTransitionDuration = .2f;
		[Tooltip("How long it takes for the screen to reset to the center after letting it go.")]
		[Range(0, 1)]
		[SerializeField]
		private float _swipeResetDuration = .1f;

		[Header("Card Settings")]
		[Tooltip("Card width of swipe elements in pixel.")]
		[SerializeField]
		private float _cardWidth = 1080f;
		[Tooltip("Card padding between swipe elements in pixel.")]
		[SerializeField]
		private float _cardPadding = 0f;
		[Tooltip("Number of cards in the setup. used to clamp the movement.")]
		[SerializeField]
		private int _numberOfCards = 3;

		[Header("Input Settings")]
		[Tooltip("Use a UIRaycastElement component to define the area that is swipe-able")]
		public UIRaycastElement area;
		#endregion

		#region State
		private bool    _inputSleep = false;
		private bool    _dragging = false;
		private int     _currentCardIdx = 0;
		private float   _downPositionX = 0f;
		private Vector2 _currentBasePosition = Vector2.zero;
		#endregion

		#region Cache
		private RectTransform _transform;
		private CoroutineAnchoredPosition _ptranslation;
		#endregion
		
		public ICardIndexVisual visualIndex;
		
		private void Awake()
		{
			_transform = GetComponent<RectTransform>();
		}

		private void Start()
		{
			area.onPointerDown.AddListener(BeginTouch);
			area.onDrag.AddListener(ContinueTouch);
			area.onPointerUp.AddListener(EndTouch);

			_ptranslation = new CoroutineAnchoredPosition(_transform, Vector2.zero, Vector2.zero).SetDuration(.15f);

			_inputSleep = false;
		}

		#region Handle Input
		public void BeginTouch(UnityEngine.EventSystems.PointerEventData mouseposition)
		{
			if(_inputSleep)
			{
				return;
			}

			_downPositionX = mouseposition.position.x;
			_dragging = true;
		}

		public void ContinueTouch(UnityEngine.EventSystems.PointerEventData mouseposition)
		{

			if (!_dragging)
			{
				return;
			}

			float currentPosition = mouseposition.position.x;
			float pixelDelta = currentPosition - _downPositionX;
			float screenDelta = Mathf.Abs(pixelDelta) / Screen.width;
			float sign = (currentPosition < _downPositionX) ? -1f : 1f;
			PressedDelta(sign, screenDelta);
		}

		public void EndTouch(UnityEngine.EventSystems.PointerEventData mouseposition)
		{
			if (!_dragging)
			{
				return;
			}

			float upPosition = mouseposition.position.x;
			float pixelDelta = upPosition - _downPositionX;
			float screenDelta = pixelDelta / Screen.width;

			if (Mathf.Abs(screenDelta) > _screenWidthForSwipe)
			{
				if(upPosition < _downPositionX)
				{
					SwipeLeft();
				}
				else
				{
					SwipeRight();
				}
			}
			else
			{
				Release();
			}

			_dragging = false;
		}
		#endregion

		#region Handle Movement
		public void GoToCard(int p_index)
		{
			p_index = Mathf.Min(p_index, _numberOfCards);
			p_index = Mathf.Max(p_index, 0);

			float toPosition = p_index * -(_cardWidth + _cardPadding);
			_currentCardIdx = p_index;

			SwipeTransition(new Vector2(toPosition, _transform.anchoredPosition.y));
		}

		private void SwipeLeft()
		{
			Vector2 toPosition = _currentBasePosition;

			if (_currentCardIdx >= 0 && _currentCardIdx < (_numberOfCards - 1))
			{
				toPosition.x = _currentBasePosition.x - (_cardWidth + _cardPadding);
				_currentCardIdx++;
			}

			_ptranslation.SetDuration(_swipeTransitionDuration);
			SwipeTransition(toPosition);
		}

		private void SwipeRight()
		{
			Vector2 toPosition = _currentBasePosition;

			if (_currentCardIdx > 0 && _currentCardIdx <= (_numberOfCards - 1))
			{
				toPosition.x = _currentBasePosition.x + (_cardWidth + _cardPadding);
				_currentCardIdx--;
			}

			_ptranslation.SetDuration(_swipeTransitionDuration);
			SwipeTransition(toPosition);
		}

		private void Release()
		{
			_ptranslation.SetDuration(_swipeResetDuration);
			SwipeTransition(_currentBasePosition);
		}

		private void PressedDelta(float sign, float screenWidthDelta)
		{
			float t = Mathf.Clamp01(screenWidthDelta / _screenWidthForMaxDistance);
			Vector2 position = _transform.anchoredPosition;
			position.x = _currentBasePosition.x + Mathf.Lerp(0, sign * _maxTravelDistance, 1 - Mathf.Pow(1 - t, 2f));
			_transform.anchoredPosition = position;
		}

		private void SwipeTransition(Vector2 to)
		{
			StopAllCoroutines();

			_inputSleep = true;
			_ptranslation
				.ResetAll()
				.SetFrom(_transform.anchoredPosition)
				.SetTo(to)
				.SetCallback(() =>
					{
						visualIndex?.SetActiveCard(_currentCardIdx);
						_currentBasePosition = to;
						_inputSleep = false;
					});

			StartCoroutine(_ptranslation.GetIEnumerator());
		}
		#endregion
	}
}