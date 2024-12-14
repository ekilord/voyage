using UnityEngine;
using UnityEngine.EventSystems;

public class CardEffects : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Vector2 dragOffset;

    private RectTransform rectTransform;
    private Canvas canvas;

    public float hoverScaleMultiplier = 1.1f;
    private string targetAreaName = "CastingSpace";

    private BattleController BattleController;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.position;
        initialScale = rectTransform.localScale;

        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Card requires a Canvas in its parent hierarchy.");
        }

        BattleController = GameObject.FindWithTag("BattleController").GetComponent<BattleController>();
        if (BattleController == null)
        {
            Debug.LogError("BattleController not found in the scene. Make sure it has the correct tag.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = rectTransform.anchoredPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            null,
            out Vector2 localPoint);

        dragOffset = rectTransform.anchoredPosition - localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas != null)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                null,
                out Vector2 localPoint))
            {
                rectTransform.anchoredPosition = localPoint + dragOffset;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            null,
            out Vector2 localPoint);

        Vector3 worldPoint = canvas.transform.TransformPoint(localPoint);

        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.name == targetAreaName)
        {
            int index = gameObject.GetComponent<CardEntity>().GetIndex();
            if (BattleController.PlayCard(index))
            {
                Destroy(gameObject);
                return;
            }
        }

        rectTransform.anchoredPosition = initialPosition;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.localScale = initialScale * hoverScaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.localScale = initialScale;
    }
}
