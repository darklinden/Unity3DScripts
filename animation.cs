using UnityEngine;
using System.Collections;

public class animation : MonoBehaviour
{
	float x;
	float y;
	bool drawLabel = false;

	// Use this for initialization
	void Start ()
	{
		animation.wrapMode = WrapMode.Loop;
		animation.Play ();
		animation.Stop ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.touchCount == 1) {
			Touch touch = Input.GetTouch (0);
			if (touch.phase == TouchPhase.Began) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray.origin, ray.direction * 10, out hit)) {
					if (hit.collider.gameObject.name == "Box_kongzhiqi") {
						x = touch.position.x;
						y = touch.position.y;
						drawLabel = true;
						
						if (animation.isPlaying) {
							Debug.Log ("stop");
							animation.Play ();
							animation.Stop ();
						} else {
							Debug.Log ("play");
							animation.Play ();
						}
					}
				}
			}
		}
	}



	void OnGUI ()
	{
		if (drawLabel) {
			if (animation.isPlaying) {
				GUI.Label (new Rect (x, 924 - y, 120, 120), "The machine begin work!");
			} else {
				GUI.Label (new Rect (x, 924 - y, 120, 120), "The machine ending work!");
			}
		}
	}
}
