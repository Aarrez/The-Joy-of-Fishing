using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bffEventTrigger : MonoBehaviour
{

    public Animator bffanim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Bait")
        {
            bffanim.Play("Swimmer");
            Debug.Log("HEY TRIGGER");
        }
    }

    public void endgame()
    {
        GameManager.instance.UIScreenfadeout();
    }
}
