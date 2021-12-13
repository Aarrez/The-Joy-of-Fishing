using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldParallaxScript : MonoBehaviour
{
    private float length, startPos;
    public GameObject insertCamera;
    public float parallaxAmount;
    public bool SetYParallax;

    float xDistance;
    float yDistance;
    [SerializeField] float yoffset_select_all_objs_w_this_script;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }


    void Update()
    {
        xDistance = (insertCamera.transform.position.x * parallaxAmount);
        if (SetYParallax == true)
        {
            yDistance = (insertCamera.transform.position.y * parallaxAmount);
        }

        transform.position = new Vector3(startPos + xDistance, startPos + yoffset_select_all_objs_w_this_script + yDistance, transform.position.z);
    }
}
