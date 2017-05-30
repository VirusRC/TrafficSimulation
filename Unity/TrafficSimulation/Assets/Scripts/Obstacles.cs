using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
	private GameObject prefabLog;
	private KeyCode shiftLeft = KeyCode.LeftShift;

	// Use this for initialization
	void Start()
	{
		prefabLog = Resources.Load("Rock", typeof(GameObject)) as GameObject;
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetMouseButtonDown(0) && Input.GetKey(shiftLeft)) //Left mouse button clicked
		{
			Vector3 mousePosition = Input.mousePosition;
			var ray = Camera.main.ScreenPointToRay(mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, 1000f))
			{
				Vector3 position = hit.point;
				Vector3 yOffset = new Vector3(0, 1.5f, 0);
				position += yOffset;
				GameObject prefabInstance = Instantiate(prefabLog, position, new Quaternion()) as GameObject;
			}
		}
		//http://answers.unity3d.com/questions/366157/mouse-click-to-world-space.html
		if(Input.GetMouseButtonDown(1) && Input.GetKey(shiftLeft)) //Right mouse button clicked
		{
			Vector3 mousePosition = Input.mousePosition;
			var ray = Camera.main.ScreenPointToRay(mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, 1000f))
			{
				Vector3 position = hit.point;
				GameObject collidedObject = hit.collider.gameObject;
				if(collidedObject.name.Equals("Rock(Clone)"))
				{
					Destroy(collidedObject);
				}
			}
		}
	}
}
