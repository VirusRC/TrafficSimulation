using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingColliderX : MonoBehaviour {

    private GameObject directions;
    private GameObject wayLeft;
    private GameObject wayStraight;
    private GameObject wayRight;

    private GameObject crossing;
    private CrossingX scriptCrossing;
    private int counterTrafficLightSync = 0;

    private RemoteObject.Enum.TrafficLightsStatus actLightState;

    private bool isControlled;
    private string uuid;

    private int countUpdateTrffic;

    // Use this for initialization
    void Start () {
        GameObject temp = gameObject.transform.parent.gameObject;
        crossing = temp.transform.parent.gameObject;
        scriptCrossing = (CrossingX)crossing.GetComponent(typeof(CrossingX));
        scriptCrossing.setCollider(gameObject.name, this);
	}
	
	// Update is called once per frame
	void Update () {
		//TODO perhaps handle Traffic light here
	}

    private void FixedUpdate()
    {
        if (isControlled)
        {
            counterTrafficLightSync++;
            if (counterTrafficLightSync > 5)
            {
                actLightState = Simulation.getInstance().getTrafficLightState(uuid, gameObject.name);
                //Debug.logger.Log(LogType.Log, gameObject.name + " has State: " + temp.ToString());
                counterTrafficLightSync = 0;
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
        if (dir == 0)
        {
            follow.setNewDirection(getChildGameObject(directions,"Left"), wayLeft, isControlled, uuid, gameObject.name);
        }
        else if (dir == 1)
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

    static private GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}
