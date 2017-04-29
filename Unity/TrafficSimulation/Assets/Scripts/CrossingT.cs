using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingT : MonoBehaviour {

    public GameObject posX;
    public GameObject posY;
    public GameObject negY;

    public bool isControlledByTrafficLight=true;
    private string uuid;

    // Use this for initialization
    void Start()
    {

    }

    private void Awake()
    {
        if (!isControlledByTrafficLight)
        {
            Destroy(getChildGameObject(gameObject, "TrafficLights"));
            uuid = "";
        }
        else
        {
            uuid = System.Guid.NewGuid().ToString();
            Simulation.getInstance().createNewTrafficLight(uuid, "PosY", "NegY", "NegX");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setCollider(string id, CrossingColliderT coll)
    {
        switch (id)
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
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}
