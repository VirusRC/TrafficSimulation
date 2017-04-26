using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingColliderT : MonoBehaviour{

    GameObject directions;

    private GameObject wayLeft;
    private GameObject wayRight;

    private GameObject crossing;
    private CrossingT scriptCrossing;

    // Use this for initialization
    void Start()
    {
        GameObject temp = gameObject.transform.parent.gameObject;
        crossing = temp.transform.parent.gameObject;
        scriptCrossing = (CrossingT)crossing.GetComponent(typeof(CrossingT));
        scriptCrossing.setCollider(gameObject.name, this);
    }

    // Update is called once per frame
    void Update()
    {
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
            follow.setNewDirection(getChildGameObject(directions, "Left"), wayLeft);
        }
        else if (dir == 1)
        {
            follow.setNewDirection(getChildGameObject(directions, "Right"), wayRight);
        }
    }

    public void setDirectionsIntern(GameObject directions, GameObject wayLeft, GameObject wayRight)
    {
        this.directions = directions;
        this.wayLeft = wayLeft;
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
