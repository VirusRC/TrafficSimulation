using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossing : MonoBehaviour {

    public GameObject posX ;
    public string pathPosX=  "PathPos";
    public GameObject negX ;
    public string pathNegX = "PathPos";
    public GameObject posY ;
    public string pathPosY=  "PathNeg";
    public GameObject negY ;
    public string pathNegY = "PathPos";

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setCollider(string id, CrossingCollider coll)
    {
        switch (id)
        {
            case "PosX":
                coll.setDirectionsIntern(getChildGameObject(gameObject, "PathPosX"), negY, pathNegY, posX, pathPosX, posY, pathPosY);
                break;
            case "NegX":
                coll.setDirectionsIntern(getChildGameObject(gameObject, "PathNegX"), posY, pathPosY, negX, pathNegX, negY, pathNegY);
                break;
            case "PosY":
                coll.setDirectionsIntern(getChildGameObject(gameObject, "PathPosY"), posX, pathPosX, posY, pathPosY, negX, pathNegX);
                break;
            case "NegY":
                coll.setDirectionsIntern(getChildGameObject(gameObject, "PathNegY"), negX, pathNegX, negY, pathNegY, posX, pathPosX);
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
