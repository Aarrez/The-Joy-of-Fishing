using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

[ExecuteInEditMode()]
public class GameTooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI bodyField;
    public LayoutElement layoutElement;

    public RectTransform rectTransform;
    
    public int characterWrapLimit;

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string body, string header = "")
    {
        if(string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        bodyField.text = body;
    }

    private void Update() 
    {
        /*if (Application.isEditor)
        {
            int headerLength = headerField.text.Length;
            int bodyLength = bodyField.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || bodyLength > characterWrapLimit ? true : false);
        }*/

        Vector2 position = TooltipScript.MousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX + 0.2f, pivotY +0.7f);
        transform.position = position;
    }
}
