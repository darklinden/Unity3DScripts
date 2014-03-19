using UnityEngine;
using System.Collections;

public class touchRotate : MonoBehaviour
{

	public Texture Button0;
	public Texture Button1;
	private Ray ray;
	private RaycastHit hit;
	float speed = 4;
	bool move = false;

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		int count = Input.touchCount;
		
		if (count > 0) {
			switch (count) {
			case 1:
				
				{
					ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
						if (Physics.Raycast (ray, out hit, 1000)) {
							if (hit.collider.gameObject == gameObject) {
								{
									// Get movement of the finger since last frame
									Vector2 touchDeltaPosition = Input.GetTouch (0).deltaPosition;
									
									if (move == false) {
										// Move object across XY plane
										transform.Rotate (0, -touchDeltaPosition.x / speed, 0);
									}
									if (move == true) {
										Camera.main.transform.Translate (-touchDeltaPosition.x / 25, -touchDeltaPosition.y / 25, 0);
									}
								}
							}
						}
					}
				}

				break;
			case 2:
				
				{
					Touch touch1 = Input.GetTouch (0);
					Touch touch2 = Input.GetTouch (1);
					
					if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved) {
						Vector2 distance1 = touch1.position - touch2.position;
						Vector2 distance2 = (touch1.position - touch1.deltaPosition) - (touch2.position - touch2.deltaPosition);
						float distance = distance1.magnitude - distance2.magnitude;
						if (Camera.main.transform.position.z + (distance / 25) > transform.position.z && Camera.main.transform.position.z + (distance / 25) < transform.position.z + 20) {
							Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + (distance / 25));
						}
					}
				}

				break;
			default:
				Debug.Log ("more than 2 fingers");
				break;
			}
		}
	}

	void OnGUI ()
	{
		
		if (GUI.Button (new Rect (20, 44, 50, 50), Button0)) {
			
			move = true;
			print ("move = true");
		}
		
		
		if (GUI.Button (new Rect (100, 44, 50, 50), Button1)) {
			
			move = false;
			print ("move = false");
		}
	}
}
