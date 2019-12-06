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
            parentContainer = GetComponentInParent<IDragSource<T>>();
        }

        public IDragSource<T> parentContainer { get; private set; }

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

            var container = GetContainer(eventData);
            if (container != null)
            {
                DropItemIntoContainer(container);
            }
        }

        private IDragDestination<T> GetContainer(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            foreach (var raycastResult in results)
            {
                var container = raycastResult.gameObject.GetComponent<IDragDestination<T>>();

                if (container != null) 
                {
                    return container;
                }
            }
            return null;
        }

        private void DropItemIntoContainer(IDragDestination<T> destination)
        {
            var source = parentContainer;

            var draggingItem = source.GetItem();
            var draggingNumber = source.GetNumber();

            var acceptable = destination.MaxAcceptable(draggingItem);
            var toTransfer = Mathf.Min(acceptable, draggingNumber);

            if (toTransfer > 0)
            {
                source.RemoveItems(toTransfer);
                destination.AddItems(draggingItem, toTransfer);
                return;
            }

            var destinationSource = destination as IDragSource<T>;
            var sourceDestination = source as IDragDestination<T>;

            if (destinationSource == null || sourceDestination == null) return;
            print("Attempt swap");
            var removedSourceNumber = source.GetNumber();
            var removedSourceItem = source.GetItem();
            source.RemoveItems(removedSourceNumber);

            var removedDestinationNumber = destinationSource.GetNumber();
            var removedDestinationItem  = destinationSource.GetItem();
            destinationSource.RemoveItems(removedDestinationNumber);

            var sourceMaxAcceptable = sourceDestination.MaxAcceptable(removedDestinationItem);

            var destinationMaxAcceptable = destination.MaxAcceptable(removedSourceItem);

            print("destinationMaxAcceptable " + destinationMaxAcceptable + " removedSourceNumber " + removedSourceNumber + " sourceMaxAcceptable " + sourceMaxAcceptable + " removedDestinationNumber " + removedDestinationNumber);

            var sourceTakeBackNumber = 0;

            if (destinationMaxAcceptable < removedSourceNumber)
            {
                sourceTakeBackNumber = removedSourceNumber - destinationMaxAcceptable;

                var sourceTakeBackAcceptable = sourceDestination.MaxAcceptable(removedSourceItem);

                if (sourceTakeBackAcceptable < sourceTakeBackNumber)
                {
                    sourceDestination.AddItems(removedSourceItem, removedSourceNumber);
                    destination.AddItems(removedDestinationItem, removedDestinationNumber);
                    return;
                }

            }

            var destinationTakeBackNumber = 0;

            if (sourceMaxAcceptable < removedDestinationNumber)
            {
                destinationTakeBackNumber = removedDestinationNumber - sourceMaxAcceptable;

                var destinationTakeBackAcceptable = destination.MaxAcceptable(removedDestinationItem);

                if (destinationTakeBackAcceptable < destinationTakeBackNumber)
                {
                    sourceDestination.AddItems(removedSourceItem, removedSourceNumber);
                    destination.AddItems(removedDestinationItem, removedDestinationNumber);
                    return;
                }

            }

            if (sourceTakeBackNumber > 0)
            {
                sourceDestination.AddItems(removedSourceItem, sourceTakeBackNumber);
            }
            if (destinationTakeBackNumber > 0)
            {
                destination.AddItems(removedDestinationItem, destinationTakeBackNumber);
            }

            if (sourceDestination.MaxAcceptable(removedDestinationItem) < removedDestinationNumber - destinationTakeBackNumber ||
                destination.MaxAcceptable(removedSourceItem) < removedSourceNumber - sourceTakeBackNumber)
            {
                destination.AddItems(removedDestinationItem, removedDestinationNumber - destinationTakeBackNumber);
                sourceDestination.AddItems(removedSourceItem, removedSourceNumber - sourceTakeBackNumber);
                return;
            }


            sourceDestination.AddItems(removedDestinationItem, removedDestinationNumber - destinationTakeBackNumber);
            destination.AddItems(removedSourceItem, removedSourceNumber - sourceTakeBackNumber);


        }
    }
}