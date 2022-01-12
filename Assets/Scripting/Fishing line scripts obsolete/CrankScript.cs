using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankScript : MonoBehaviour
{

    public float rotateSpeed = 10f;
    private Transform selected;
    private WholeRopeScript rope;
    private int numLinks;
    public int maxLinks = 15;
    // Start is called before the first frame update
    void Awake()
    {
        selected = transform.Find("Selected");
        rope = transform.parent.GetComponent<WholeRopeScript>();
        numLinks = rope.numLinks;
    }

    public void Rotate(int direction)
    {
        if(direction > 0 && rope != null && numLinks <= maxLinks)
        {
            transform.Rotate(0, 0, direction * rotateSpeed);
            rope.AddLink();
            numLinks++;
        }
        else if(direction < 0 && rope != null && numLinks > 1)
        {
                transform.Rotate(0, 0, direction * rotateSpeed);
                rope.RemoveLink();
            numLinks--;
        } 
    }

    public void Select()
    {
        selected.gameObject.SetActive(true);
    }
    public void Deselect()
    {
        selected.gameObject.SetActive(false);
    }

}
