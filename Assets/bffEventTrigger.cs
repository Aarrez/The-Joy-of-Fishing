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

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "BaitLayer")
        {
            bffanim.Play("Swimmer");
        }
    }

    public void endgame()
    {
        GameManager.instance.UIScreenfadeout();
    }
}
