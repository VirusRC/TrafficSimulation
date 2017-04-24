using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingCollider : MonoBehaviour {

    GameObject directions;

    private GameObject wayLeft;
    private GameObject wayStraight;
    private GameObject wayRight;

    private string pathLeft;
    private string pathStraight;
    private string pathRight;

    private GameObject crossing;
    private Crossing scriptCrossing;

    // Use this for initialization
    void Start () {
        GameObject temp = gameObject.transform.parent.gameObject;
        crossing = temp.transform.parent.gameObject;
        scriptCrossing = (Crossing)crossing.GetComponent(typeof(Crossing));
        scriptCrossing.setCollider(gameObject.name, this);
	}
	
	// Update is called once per frame
	void Update () {
		//TODO perhaps handle Traffic light here
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
            follow.setNewDirection(getChildGameObject(directions,"Left"), wayLeft, pathLeft);
        }
        else if (dir == 1)
        {
            follow.setNewDirection(getChildGameObject(directions, "Straight"), wayStraight, pathStraight);
        }
        else if(dir == 2)
        {
            follow.setNewDirection(getChildGameObject(directions, "Right"), wayRight, pathRight);
        } 
    }

    public void setDirectionsIntern(GameObject directions, GameObject wayLeft, string pathLeft , GameObject wayStraight, string pathStraight, GameObject wayRight, string pathRight)
    {
        this.directions = directions;
        this.wayLeft = wayLeft;
        this.wayStraight = wayStraight;
        this.wayRight = wayRight;
        this.pathLeft = pathLeft;
        this.pathStraight = pathStraight;
        this.pathRight = pathRight;
    }

    static private GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}
