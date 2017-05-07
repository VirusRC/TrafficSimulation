using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingColliderX : MonoBehaviour
{

	private GameObject directions;
	private GameObject wayLeft;
	private GameObject wayStraight;
	private GameObject wayRight;

	private GameObject crossing;
	private CrossingX scriptCrossing;
	private int counterTrafficLightSync = 0;
	private int counterTrafficLightBlinking = 0;
	private Light trafficLight;

	public RemoteObject.Enum.TrafficLightsStatus actLightState;

	private bool isControlled;
	private string uuid;

	private int countUpdateTrffic;

	// Use this for initialization
	void Start()
	{
		GameObject temp = gameObject.transform.parent.gameObject;
		crossing = temp.transform.parent.gameObject;
		scriptCrossing = (CrossingX)crossing.GetComponent(typeof(CrossingX));
		scriptCrossing.setCollider(gameObject.name, this);

		trafficLight = gameObject.GetComponentInChildren<Light>();

		if(!isControlled)
		{
			Destroy(getChildGameObject(gameObject, "TrafficLight"));
			Destroy(getChildGameObject(gameObject, "Spotlight"));
		}
	}

	// Update is called once per frame
	void Update()
	{
		//TODO perhaps handle Traffic light here
	}

	private void FixedUpdate()
	{
		if(isControlled)
		{
			counterTrafficLightSync++;
			if(counterTrafficLightSync > 5)
			{
				var temp = Simulation.getInstance().getTrafficLightState(uuid, gameObject.name);
				if(!actLightState.Equals(temp))
				{
					actLightState = temp;
					changeLightColor();
				}

				counterTrafficLightBlinking++;
				if((actLightState.Equals(RemoteObject.Enum.TrafficLightsStatus.BlinkGreen) || actLightState.Equals(RemoteObject.Enum.TrafficLightsStatus.BlinkYellow)) && counterTrafficLightBlinking > 25)
				{
					counterTrafficLightBlinking = 0;
					if(trafficLight.enabled)
					{
						trafficLight.enabled = false;
					}
					else
					{
						trafficLight.enabled = true;
					}
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		FollowWay next = (FollowWay)other.GetComponent(typeof(FollowWay));
		next.decideWay(this);
	}

	public void setDirection(int dir, FollowWay follow)
	{
		if(dir == 0)
		{
			follow.setNewDirection(getChildGameObject(directions, "Left"), wayLeft, isControlled, uuid, gameObject.name);
		}
		else if(dir == 1)
		{
			follow.setNewDirection(getChildGameObject(directions, "Straight"), wayStraight, isControlled, uuid, gameObject.name);
		}
		else if(dir == 2)
		{
			follow.setNewDirection(getChildGameObject(directions, "Right"), wayRight, isControlled, uuid, gameObject.name);
		}
	}

	public void setDirectionsIntern(GameObject directions, GameObject wayLeft, GameObject wayStraight, GameObject wayRight, bool isControlled, string uuid)
	{
		this.directions = directions;
		this.wayLeft = wayLeft;
		this.wayStraight = wayStraight;
		this.wayRight = wayRight;
		this.isControlled = isControlled;
		this.uuid = uuid;
	}

	public RemoteObject.Enum.TrafficLightsStatus getActualTrafficLight()
	{
		return actLightState;
	}

	private void changeLightColor()
	{
		if(!trafficLight.enabled)
		{
			trafficLight.enabled = true;
		}
		switch(actLightState)
		{
			case RemoteObject.Enum.TrafficLightsStatus.Green:
				trafficLight.color = Color.green;
				break;
			case RemoteObject.Enum.TrafficLightsStatus.BlinkGreen:
				counterTrafficLightBlinking = 0;
				trafficLight.color = Color.green;
				break;
			case RemoteObject.Enum.TrafficLightsStatus.Yellow:
				trafficLight.color = Color.yellow;
				break;
			case RemoteObject.Enum.TrafficLightsStatus.BlinkYellow:
				counterTrafficLightBlinking = 0;
				trafficLight.color = Color.yellow;
				break;
			case RemoteObject.Enum.TrafficLightsStatus.RedYellow:
				trafficLight.color = Color.yellow;
				break;
			case RemoteObject.Enum.TrafficLightsStatus.Red:
				trafficLight.color = Color.red;
				break;

		}
	}

	static private GameObject getChildGameObject(GameObject fromGameObject, string withName)
	{
		//Author: Isaac Dart, June-13.
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
		foreach(Transform t in ts) if(t.gameObject.name == withName) return t.gameObject;
		return null;
	}
}
