using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Core.UI.Dragging
{
    public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
        where T : class
    {
        Vector3 _startPosition;
        Transform _originalParent;

        Canvas _parentCanvas;

        private void Awake()
        {
            _parentCanvas = GetComponentInParent<Canvas>();
            parentContainer = GetComponentInParent<IDragContainer<T>>();
        }

        public IDragContainer<T> parentContainer { get; private set; }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = transform.position;
            _originalParent = transform.parent;
            transform.parent = _parentCanvas.transform;
            // Else won't get the drop event.
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.position = _startPosition;
            transform.parent = _originalParent;
            GetComponent<CanvasGroup>().blocksRaycasts = true;

            // Not over UI we should drop.
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                DropItem();
            }

            var container = GetContainer(eventData);
            if (container != null)
            {
                DropItemIntoContainer(container);
            }
        }

        private void DropItem()
        {
            var item = parentContainer.ReplaceItem(null);
            var player = GameObject.FindWithTag("Player");
            var dropHandler = player.GetComponent<IDiscardHandler<T>>();
            dropHandler.DropItem(item);
        }

        private IDragContainer<T> GetContainer(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            foreach (var raycastResult in results)
            {
                var container = raycastResult.gameObject.GetComponent<IDragContainer<T>>();

                if (container != null) 
                {
                    return container;
                }
            }
            return null;
        }

        private void DropItemIntoContainer(IDragContainer<T> receivingContainer)
        {
            var draggingItem = parentContainer.ReplaceItem(null);
            if (!receivingContainer.CanAcceptItem(draggingItem))
            {
                parentContainer.ReplaceItem(draggingItem);
                return;
            }
            
            var swappedItem = receivingContainer.ReplaceItem(draggingItem);
            if (swappedItem != null)
            {
                parentContainer.ReplaceItem(swappedItem);
            }
        }
    }
}