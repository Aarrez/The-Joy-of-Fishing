using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Ensure class initializer is called whenever scripts recompile
[InitializeOnLoad]
public class EditorQuitExample : MonoBehaviour
{
    static void Quit()
    {
        Debug.Log("Quitting the Editor");
    }

    static EditorQuitExample()
    {
        EditorApplication.quitting += Quit;
    }
}
