using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.SetUpTooltip(description, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.CloseTooltip();
    }
}
