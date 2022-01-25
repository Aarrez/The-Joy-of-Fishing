using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
using UnityEngine;


public sealed class TooltipScript : MonoBehaviour
{
    public static TooltipScript instance = null;

    public GameTooltip gameToolTip;
    public InputSystemUIInputModule inputmodule;
    InputAction leftClick;
    InputAction middleClick;
    InputAction rightClick;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than one TooltipScript");
            Destroy(instance.gameObject);
            instance = this;
        }
    }
    // Start is called before the first frame update
    public static void ShowTooltip(string body, string header = "")
    {
        instance.gameToolTip.SetText(body, header);
        instance.gameToolTip.gameObject.SetActive(true);
    }
    public static void HideTooltip()
    {
        instance.gameToolTip.gameObject.SetActive(false);
    }

    public static Vector2 MousePosition
    {
        get
        {
            if(instance == null)
            {
                return new Vector2();
            }
            else
            {
                return instance.inputmodule.point.action.ReadValue<Vector2>();
            }
        }
    }


}
