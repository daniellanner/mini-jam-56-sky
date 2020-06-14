using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

/// A concrete subclass of the Unity UI `Graphic` class that just skips drawing.
/// Useful for providing a raycast target without actually drawing anything.
public class UIRaycastElement : Graphic,
	IPointerClickHandler,
	IPointerUpHandler,
	IPointerDownHandler,
	IPointerExitHandler,
	IPointerEnterHandler,
	IBeginDragHandler,
	IDragHandler,
	IEndDragHandler
{
	public class EventCallback
	{
		private List<System.Action<PointerEventData>> _cb = new List<System.Action<PointerEventData>>();
		public void AddListener(System.Action<PointerEventData> action) => _cb.Add(action);

		public void Invoke(PointerEventData eventData)
		{
			_cb?.ForEach(it => it.Invoke(eventData));
		}

		public void Clear()
		{
			_cb?.Clear();
		}
	}

	public EventCallback onBeginDrag = new EventCallback();
	public EventCallback onEndDrag = new EventCallback();
	public EventCallback onDrag = new EventCallback();
	public EventCallback onPointerClick = new EventCallback();
	public EventCallback onPointerDown = new EventCallback();
	public EventCallback onPointerUp = new EventCallback();
	public EventCallback onPointerEnter = new EventCallback();
	public EventCallback onPointerExit = new EventCallback();

	public void OnBeginDrag(PointerEventData eventData)
	{
		Debug.Log("Begin Drag");
		onBeginDrag.Invoke(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		Debug.Log("Drag");
		onDrag.Invoke(eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		onEndDrag.Invoke(eventData);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		onPointerClick.Invoke(eventData);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("Pointer Down");
		onPointerDown.Invoke(eventData);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		onPointerEnter.Invoke(eventData);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		onPointerExit.Invoke(eventData);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		onPointerUp.Invoke(eventData);
	}

	#region Graphics overrides
	public override void SetMaterialDirty() { return; }
	public override void SetVerticesDirty() { return; }

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
		return;
	}
	#endregion
}