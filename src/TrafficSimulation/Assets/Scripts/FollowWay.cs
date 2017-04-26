using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWay : MonoBehaviour {

    public float speed = 5f;
    public int rotationSpeed;

    private GameObject pathGO;
    Transform targetPathNode;
    int pathNodeIndex = 0;

    private GameObject crossingNext;
    private GameObject actualStreet;
    private string direction;
    private string generalDirection;
    private bool isNewStreet;
    private int streetIndex;

	// Use this for initialization
	void Start () {
        //        actualStreet = GameObject.Find("Alpenstrasse");
        //       streetIndex = 2;
        //      //streetIndex=actualStreet.transform.childCount-1;
        //     direction = "PathPos";
        //    generalDirection = direction;
        //   isNewStreet = false;
        //isNewStreet = true;
    }


    public void initialize(GameObject street, string direction)
    {
        this.direction = direction;
        this.actualStreet = street;
        isNewStreet = true;
    }

    // Update is called once per frame
    void Update () {
        if (targetPathNode == null)
        {
            GetNextPathNode();
        }
        Vector3 dir = targetPathNode.position - this.transform.localPosition;
        float distThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distThisFrame)
        {
            targetPathNode = null;
        }
        else
        {
            transform.Translate(dir.normalized * distThisFrame,Space.World);
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime*rotationSpeed);
        }
	}

    void GetNextPathNode()
    {
        if(pathGO==null || pathNodeIndex >= pathGO.transform.childCount)
        {
            getNextStreetPart();
        }
        targetPathNode = pathGO.transform.GetChild(pathNodeIndex);
        pathNodeIndex++;
    }

    private void getNextStreetPart()
    {
        if (crossingNext != null)
        {
            pathGO = crossingNext;
            crossingNext = null;
            pathNodeIndex = 0;
            targetPathNode = pathGO.transform.GetChild(pathNodeIndex);
            isNewStreet = true;
        }
        else if (actualStreet != null)
        {
            if (isNewStreet)
            {
                generalDirection = direction;
                if (generalDirection.Equals("PathPos"))
                {
                    streetIndex = 0;
                }
                else if (generalDirection.Equals("PathNeg"))
                {
                    streetIndex = actualStreet.transform.childCount - 1;
                }
                isNewStreet = false;
            }

            if (generalDirection.Equals("PathPos"))
            {
                if (streetIndex > actualStreet.transform.childCount)
                {
                    Destroy(gameObject);
                }
                pathGO = getChildGameObject(actualStreet.transform.GetChild(streetIndex).gameObject, direction);
                streetIndex++;
                pathNodeIndex = 0;
            }
            else if (generalDirection.Equals("PathNeg"))
            {
                if (streetIndex < 0)
                {
                    Destroy(gameObject);
                }
                pathGO = getChildGameObject(actualStreet.transform.GetChild(streetIndex).gameObject, direction);
                streetIndex--;
                pathNodeIndex = 0;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    static private GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    public void decideWay(CrossingColliderX collider)
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(0, 3);
        collider.setDirection(randomNumber, this);
    }

    public void decideWay(CrossingColliderT collider)
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(0, 2);
        collider.setDirection(randomNumber, this);
    }

    public void setNewDirection(GameObject crossingWay, GameObject nextWay)
    {
        this.crossingNext = crossingWay;
        this.actualStreet = nextWay;
    }

    public void setStreetDirection(string dir)
    {
        this.direction = dir;  
    }

}
