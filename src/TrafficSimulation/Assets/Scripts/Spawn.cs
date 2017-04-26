using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public int maxCars;
    public int spawDuration;
    public GameObject startPoint;
    public string startDirection;

    private int currentCars;
    private int time;

	// Use this for initialization
	void Start () {
        currentCars = 0;
        time = 0;
        generateCar();
    }

    private void FixedUpdate()
    {
        time++;
        if (time >= (spawDuration / 0.02))
        {
            generateCar();
            time = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }

    private void generateCar()
    {
        if (currentCars > maxCars)
        {
            return;
        }
        GameObject prefab = Resources.Load("jeep", typeof(GameObject)) as GameObject; ;
        Vector3 vec = new Vector3(22.4f, 0, 0);
        GameObject prefabInstance = GameObject.Instantiate(prefab, getChildGameObject(gameObject, "SpawnPoint").transform.position, transform.rotation) as GameObject;
        if (prefabInstance != null)
        {
            var myScriptReference = prefabInstance.GetComponent<FollowWay>();
            if (myScriptReference != null)
            {
                myScriptReference.initialize(startPoint, startDirection);
            }
        }
        currentCars++;
    }

    static private GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}
