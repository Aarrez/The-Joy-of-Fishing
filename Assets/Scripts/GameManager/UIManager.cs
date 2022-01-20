using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public sealed class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
public static UIManager instance = null;


private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than one UIManager");
            Destroy(instance.gameObject);
            instance = this;
        }
    }
}
