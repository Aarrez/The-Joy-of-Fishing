using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class PreventQuit : MonoBehaviour
{
    static bool WantsToQuit()
    {
        Debug.Log("Editor prevented from quitting.");
        return false;
    }
 
    static PreventQuit()
    {
        Debug.Log("Editor wants to quit!");
        EditorApplication.wantsToQuit += WantsToQuit;
    }

}
