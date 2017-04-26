﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingColliderX : MonoBehaviour {

    GameObject directions;

    private GameObject wayLeft;
    private GameObject wayStraight;
    private GameObject wayRight;

    private GameObject crossing;
    private CrossingX scriptCrossing;

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

    private void OnTriggerEnter(Collider other)
    {
        FollowWay next = (FollowWay)other.GetComponent(typeof(FollowWay));
        next.decideWay(this);
    }

    public void setDirection(int dir, FollowWay follow)
    {
        if (dir == 0)
        {
            follow.setNewDirection(getChildGameObject(directions,"Left"), wayLeft);
        }
        else if (dir == 1)
        {
            follow.setNewDirection(getChildGameObject(directions, "Straight"), wayStraight);
        }
        else if(dir == 2)
        {
            follow.setNewDirection(getChildGameObject(directions, "Right"), wayRight);
        } 
    }

    public void setDirectionsIntern(GameObject directions, GameObject wayLeft, GameObject wayStraight, GameObject wayRight)
    {
        this.directions = directions;
        this.wayLeft = wayLeft;
        this.wayStraight = wayStraight;
        this.wayRight = wayRight;
    }

    static private GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}
