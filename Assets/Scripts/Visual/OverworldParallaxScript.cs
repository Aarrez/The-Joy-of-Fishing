using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldParallaxScript : MonoBehaviour
{
    private float length, startPos;
    public GameObject insertCamera;
    public float parallaxAmount;
    public bool SetYParallax;
    public bool SetXParallax;
    public int objNumber;

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
        if (SetXParallax == true)
        {
            xDistance = (insertCamera.transform.position.x * parallaxAmount);
        }

        if (SetYParallax == true)
        {
            yDistance = (insertCamera.transform.position.y * parallaxAmount);
        }

        transform.position = new Vector3(startPos + xDistance, startPos + yoffset_select_all_objs_w_this_script + yDistance, transform.position.z) ;

        if(UIManager.instance.moveCam == 3)
        {
            SetXParallax = true;
            SetYParallax = false;
            //if (objNumber == 0)
            //{
            //    ParallaxAmountTransitionNeg();
            //    if (parallaxAmount <= 0.7f)
            //    {
            //        parallaxAmount = 0.7f;
            //    }
            //}
            //if (objNumber == 1)
            //{
            //    ParallaxAmountTransitionNeg();
            //    if (parallaxAmount <= 0.71f)
            //    {
            //        parallaxAmount = 0.71f;
            //    }
            //}
            //if (objNumber == 2)
            //{
            //    ParallaxAmountTransitionNeg();
            //    if (parallaxAmount <= 0.72f)
            //    {
            //        parallaxAmount = 0.72f;
            //    }
            //}
            //if (objNumber == 3)
            //{
            //    ParallaxAmountTransitionNeg();
            //    if (parallaxAmount <= 0.73f)
            //    {
            //        parallaxAmount = 0.73f;
            //    }
            //}
            //if (objNumber == 4)
            //{
            //    ParallaxAmountTransitionNeg();
            //    if (parallaxAmount <= 0.74f)
            //    {
            //        parallaxAmount = 0.74f;
            //    }
            //}
            //if (objNumber == 5)
            //{
            //    ParallaxAmountTransitionNeg();
            //    if (parallaxAmount <= 0.75f)
            //    {
            //        parallaxAmount = 0.75f;
            //    }
            //}
            //if (objNumber == 6)
            //{
            //    ParallaxAmountTransitionNeg();
            //    if (parallaxAmount <= 0.76f)
            //    {
            //        parallaxAmount = 0.76f;
            //    }
            //}
           //yoffset_select_all_objs_w_this_script = -15.5f;
        }
        else {
            SetXParallax = true; 
            SetYParallax = true;
            //if(objNumber == 0)
            //{
            //    ParallaxAmountTransitionPos();
            //    if (parallaxAmount >= 0.9)
            //    {
            //        parallaxAmount = 0.9f;
            //    }
            //}
            //if (objNumber == 1)
            //{
            //    ParallaxAmountTransitionPos();
            //    if (parallaxAmount >= 0.91)
            //    {
            //        parallaxAmount = 0.91f;
            //    }
            //}
            //if (objNumber == 2)
            //{
            //    ParallaxAmountTransitionPos();
            //    if (parallaxAmount >= 0.92)
            //    {
            //        parallaxAmount = 0.92f;
            //    }
            //}
            //if (objNumber == 3)
            //{
            //    ParallaxAmountTransitionPos();
            //    if (parallaxAmount >= 0.93)
            //    {
            //        parallaxAmount = 0.93f;
            //    }
            //}
            //if (objNumber == 4)
            //{
            //    ParallaxAmountTransitionPos();
            //    if (parallaxAmount >= 0.94)
            //    {
            //        parallaxAmount = 0.94f;
            //    }
            //}
            //if (objNumber == 5)
            //{
            //    ParallaxAmountTransitionPos();
            //    if (parallaxAmount >= 0.95)
            //    {
            //        parallaxAmount = 0.95f;
            //    }
            //}
            //if (objNumber == 6)
            //{
            //    ParallaxAmountTransitionPos();
            //    if (parallaxAmount >= 0.96)
            //    {
            //        parallaxAmount = 0.96f;
            //    }

            //}
            //yoffset_select_all_objs_w_this_script = -19.2f;
        }

        void ParallaxAmountTransitionNeg()
        {
            parallaxAmount -= Time.deltaTime;
            
        }

        void ParallaxAmountTransitionPos()
        {
            //yield return new WaitForSeconds(0.5f);
            parallaxAmount += Time.deltaTime;

        }
    }
}
