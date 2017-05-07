using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingT : MonoBehaviour
{

	public GameObject posX;
	public GameObject posY;
	public GameObject negY;
	
	public bool isControlledByTrafficLight = true;
	private string uuid = "";
	private CrossingColliderT crossingColliderT;

	// Use this for initialization
	void Start()
	{

	}

	private void Awake()
	{
		if(isControlledByTrafficLight)
		{
			uuid = System.Guid.NewGuid().ToString();
			Simulation.getInstance().createNewTrafficLight(uuid, "PosY", "NegY", "NegX");
		}
	}

	public RemoteObject.Enum.TrafficLightsStatus getTrafficLightStatus()
	{
		return crossingColliderT.actLightState;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void setCollider(string id, CrossingColliderT coll)
	{
		crossingColliderT = coll;
		switch(id)
		{
			case "NegX":
				coll.setDirectionsIntern(getChildGameObject(gameObject, "PathPosX"), negY, posY, isControlledByTrafficLight, uuid);
				break;
			case "NegY":
				coll.setDirectionsIntern(getChildGameObject(gameObject, "PathPosY"), posY, posX, isControlledByTrafficLight, uuid);
				break;
			case "PosY":
				coll.setDirectionsIntern(getChildGameObject(gameObject, "PathNegY"), posX, negY, isControlledByTrafficLight, uuid);
				break;
		}

	}

	static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
	{
		//Author: Isaac Dart, June-13.
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
		foreach(Transform t in ts) if(t.gameObject.name == withName) return t.gameObject;
		return null;
	}
}
