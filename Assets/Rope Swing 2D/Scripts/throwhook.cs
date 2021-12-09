using UnityEngine;
using System.Collections;

public class throwhook : MonoBehaviour {

	//hook prefab
	public GameObject hook;

	//holds whether rope is active or not
	bool ropeActive;

	//current hook on the scene
	GameObject curHook;
		
	// Update is called once per frame
	void Update () {
	
		//on left click
		if (Input.GetMouseButtonDown (0)) {

			//when rope is not activated
			if (ropeActive == false) {

				//destiny is where the mouse is
				Vector2 destiny = Camera.main.ScreenToWorldPoint (Input.mousePosition);

				//creates a hook
				curHook = (GameObject)Instantiate (hook, transform.position, Quaternion.identity);

				//sets its destiny
				curHook.GetComponent<RopeScript> ().destiny = destiny;

				//sets rope to enabled
				ropeActive = true;


			} else {

				//delete rope
				Destroy (curHook);

				//sets rope to disabled
				ropeActive = false;

			}
		}
	}
}
