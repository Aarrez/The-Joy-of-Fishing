using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] fishPrefabs;

    private GameObject gameBound;

    private void Awake()
    {
        GetGameBounds();
    }
    private void Start()
    {
        for (int i = 0; i < fishPrefabs.Length; i++)
        {
            Instantiate(fishPrefabs[i]);
        }

        PlaceFishInGameBounds();
    }

    private void GetGameBounds()
    {
        try
        {
            gameBound = GameObject.FindGameObjectWithTag("GameBound");
        }
        catch (System.Exception)
        {

            Debug.Log("There is no object in the scene with the tag GameBounds." +
                "\n Please make one.");
        }
    }

    private void PlaceFishInGameBounds()
    {
    }



}
