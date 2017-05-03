using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn : MonoBehaviour {

    public int maxCars;
    public GameObject startPoint;
    public string startDirection;
	public Slider maxCarSlider;
	public float spawnDuration;
	public Slider spawnDurationSlider;

	private int currentCars;
    private int time;

	// Use this for initialization
	void Start () {
        currentCars = 0;
        time = 0;
		//Adds a listener to the main slider and invokes a method when the value changes.
		maxCarSlider.onValueChanged.AddListener(delegate { maxCarsValueChangeCheck(); });
		spawnDurationSlider = GameObject.Find("SliderCarSpawnDuration").GetComponent<Slider>();
		spawnDuration = spawnDurationSlider.value;
		spawnDurationSlider.onValueChanged.AddListener(delegate { spawnDurationValueChangeCheck(); });
		generateCar();
    }

    private void FixedUpdate()
    {
        time++;
        if (time >= (spawnDuration / 0.02))
        {
            generateCar();
            time = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        currentCars--;
    }

    private void generateCar()
    {
        if (currentCars > maxCars)
        {
            return;
        }
        GameObject prefab = Resources.Load("jeep", typeof(GameObject)) as GameObject; ;
        Vector3 vec = new Vector3(22.4f, 1.3f, 1.3f);
        GameObject prefabInstance = Instantiate(prefab, getChildGameObject(gameObject, "SpawnPoint").transform.position, transform.rotation) as GameObject;
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

	// Invoked when the value of the slider changes.
	public void maxCarsValueChangeCheck()
	{
		maxCars = Convert.ToInt32(maxCarSlider.value);
	}

	public void spawnDurationValueChangeCheck()
	{
		spawnDuration = spawnDurationSlider.value;
	}
}
