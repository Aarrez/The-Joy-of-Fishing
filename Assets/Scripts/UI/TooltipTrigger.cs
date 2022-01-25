using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string body;
    public string header;
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipScript.ShowTooltip(body, header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipScript.HideTooltip();
    }
}
