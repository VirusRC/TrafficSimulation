﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingT : MonoBehaviour {

    public GameObject posX;
    public string pathPosX;
    public GameObject posY;
    public string pathPosY;
    public GameObject negY;
    public string pathNegY;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setCollider(string id, CrossingColliderT coll)
    {
        switch (id)
        {
            case "PosX":
                coll.setDirectionsIntern(getChildGameObject(gameObject, "PathPosX"), negY, pathNegY, posY, pathPosY);
                break;
            case "PosY":
                coll.setDirectionsIntern(getChildGameObject(gameObject, "PathPosY"), posY, pathPosY, posX, pathPosX);
                break;
            case "NegY":
                coll.setDirectionsIntern(getChildGameObject(gameObject, "PathNegY"), posX, pathPosX, negY, pathNegY);
                break;
        }

    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}
