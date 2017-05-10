using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn : MonoBehaviour
{
	public int maxCars;
	public GameObject startPoint;
	public string startDirection;
	public Slider maxCarSlider;
	public float spawnDuration;
	public Slider spawnDurationSlider;

	private int currentCars;
	private int time;

	// Use this for initialization
	void Start()
	{
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
		if(time >= (spawnDuration / 0.02))
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
		//if(currentCars == 1)
		if(currentCars > maxCars)
		{
			return;
		}
		GameObject prefabJeep = Resources.Load("jeep", typeof(GameObject)) as GameObject; ;
		GameObject prefabTruck = Resources.Load("Tanker", typeof(GameObject)) as GameObject; ;

		System.Random random = new System.Random();
		int randomNumber = random.Next(0, 100);


		if(randomNumber < 70)
		{
			Vector3 vec = new Vector3(22.4f, 1.3f, 1.3f);
			GameObject prefabInstance = Instantiate(prefabJeep, getChildGameObject(gameObject, "SpawnPoint").transform.position, transform.rotation) as GameObject;
			if(prefabInstance != null)
			{
				var myScriptReference = prefabInstance.GetComponent<FollowWay>();
				if(myScriptReference != null)
				{
					myScriptReference.initialize(startPoint, startDirection);
				}
			}
		}
		else
		{
			Vector3 vec = new Vector3(22.4f, 1.3f, 1.3f);
			//GameObject prefabInstance = Instantiate(prefabTruck, getChildGameObject(gameObject, "SpawnPoint").transform.position, prefabTruck.transform.rotation) as GameObject;
			GameObject prefabInstance = Instantiate(prefabJeep, getChildGameObject(gameObject, "SpawnPoint").transform.position, transform.rotation) as GameObject;
			//prefabInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
			if(prefabInstance != null)
			{
				var myScriptReference = prefabInstance.GetComponent<FollowWay>();
				if(myScriptReference != null)
				{
					myScriptReference.initialize(startPoint, startDirection);
				}
			}
		}
		
		currentCars++;
	}

	static private GameObject getChildGameObject(GameObject fromGameObject, string withName)
	{
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
		foreach(Transform t in ts) if(t.gameObject.name == withName) return t.gameObject;
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
