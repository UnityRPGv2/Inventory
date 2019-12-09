using UnityEngine;
using UnityEngine.EventSystems;

namespace GameDevTV.Core.UI.Tooltips
{
    public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] GameObject tooltipPrefab = null;

        GameObject tooltip = null;

        private void OnDestroy()
        {
            ClearTooltip();
        }

        private void OnDisable()
        {
            ClearTooltip();
        }

        public abstract void UpdateTooltip(GameObject tooltip);

        public void OnPointerEnter(PointerEventData eventData)
        {
            var parentCanvas = GetComponentInParent<Canvas>();

            if (!tooltip)
            {
                tooltip = Instantiate(tooltipPrefab, parentCanvas.transform);
            }

            UpdateTooltip(tooltip);

            PositionTooltip();
        }

        private void PositionTooltip()
        {
            // Required to ensure corners are updated by positioning elements.
            Canvas.ForceUpdateCanvases();

            var tooltipCorners = new Vector3[4];
            tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);
            var slotCorners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(slotCorners);

            bool below = transform.position.y > Screen.height / 2;
            bool right = transform.position.x < Screen.width / 2;

            int slotCorner = GetCornerIndex(below, right);
            int tooltipCorner = GetCornerIndex(!below, !right);

            tooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + tooltip.transform.position;
        }

        private int GetCornerIndex(bool below, bool right)
        {
            if (below && !right) return 0;
            else if (!below && !right) return 1;
            else if (!below && right) return 2;
            else return 3;

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ClearTooltip();
        }

        private void ClearTooltip()
        {
            if (tooltip)
            {
                Destroy(tooltip.gameObject);
            }
        }
    }
}