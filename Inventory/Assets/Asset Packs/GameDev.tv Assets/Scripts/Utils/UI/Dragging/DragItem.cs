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
            source = GetComponentInParent<IDragSource<T>>();
        }

        public IDragSource<T> source { get; private set; }

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
        private interface IDragSrcDst : IDragSource<T>, IDragDestination<T> { }

        private void DropItemIntoContainer(IDragDestination<T> destination)
        {
            if (object.ReferenceEquals(destination, source)) return;

            var destinationSrcDst = destination as IDragSrcDst;
            var sourceSrcDst = source as IDragSrcDst;

            // Swap won't be possible
            if (destinationSrcDst == null || sourceSrcDst == null || destinationSrcDst.GetItem() == null)
            {
                AttemptSimpleTransfer(destination);
                return;
            }

            AttemptSwap(destinationSrcDst, sourceSrcDst);
        }

        private void AttemptSwap(IDragSrcDst destination, IDragSrcDst source)
        {
            // Provisionally remove item from both sides. 
            var removedSourceNumber = source.GetNumber();
            var removedSourceItem = source.GetItem();
            var removedDestinationNumber = destination.GetNumber();
            var removedDestinationItem = destination.GetItem();

            source.RemoveItems(removedSourceNumber);
            destination.RemoveItems(removedDestinationNumber);

            var sourceTakeBackNumber = CalculateTakeBack(source, destination);
            var destinationTakeBackNumber = CalculateTakeBack(destination, source);

            // Do take backs (if needed)
            if (sourceTakeBackNumber > 0)
            {
                source.AddItems(removedSourceItem, sourceTakeBackNumber);
                removedSourceNumber -= sourceTakeBackNumber;
            }
            if (destinationTakeBackNumber > 0)
            {
                destination.AddItems(removedDestinationItem, destinationTakeBackNumber);
                removedDestinationNumber -= destinationTakeBackNumber;
            }

            // Abort if we can't do a successful swap
            if (source.MaxAcceptable(removedDestinationItem) < removedDestinationNumber ||
                destination.MaxAcceptable(removedSourceItem) < removedSourceNumber)
            {
                destination.AddItems(removedDestinationItem, removedDestinationNumber);
                source.AddItems(removedSourceItem, removedSourceNumber);
                return;
            }

            // Do swaps
            source.AddItems(removedDestinationItem, removedDestinationNumber);
            destination.AddItems(removedSourceItem, removedSourceNumber);
        }

        private bool AttemptSimpleTransfer(IDragDestination<T> destination)
        {
            var draggingItem = source.GetItem();
            var draggingNumber = source.GetNumber();

            var acceptable = destination.MaxAcceptable(draggingItem);
            var toTransfer = Mathf.Min(acceptable, draggingNumber);

            if (toTransfer > 0)
            {
                source.RemoveItems(toTransfer);
                destination.AddItems(draggingItem, toTransfer);
                return false;
            }

            return true;
        }

        private int CalculateTakeBack(IDragSrcDst source, IDragSrcDst destination)
        {
            var removedNumber = source.GetNumber();
            var removedItem = source.GetItem();

            var takeBackNumber = 0;
            var destinationMaxAcceptable = destination.MaxAcceptable(removedItem);

            if (destinationMaxAcceptable < removedNumber)
            {
                takeBackNumber = removedNumber - destinationMaxAcceptable;

                var sourceTakeBackAcceptable = source.MaxAcceptable(removedItem);

                // Abort and reset
                if (sourceTakeBackAcceptable < takeBackNumber)
                {
                    return 0;
                }
            }
            return takeBackNumber;
        }
    }
}