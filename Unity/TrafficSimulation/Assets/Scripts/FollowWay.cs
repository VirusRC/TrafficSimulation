using System;
using System.Collections;
using System.Collections.Generic;
using RemoteObject;
using UnityEngine;
using UnityEngine.UI;

public class FollowWay : MonoBehaviour
{

	public float maxSpeed;
	public Slider maxSpeedSlider;

	private float speed = 20f;
	private float rotationSpeed = 2f;
	private float mass = 1000f;
	private float ps = 5000f;
	private float lengthCar = 3.5f;
	private float lengthTanker = 15f;

	private GameObject pathGO;
	Transform targetPathNode;
	int pathNodeIndex = 0;

	private GameObject crossingNext = null;
	private GameObject crossingCurrent = null;
	private GameObject firstCollider = null;
	private CrossingColliderT nextCrossingColliderT;
	private CrossingColliderX nextCrossingColliderX;
	private bool nextCrossingIsControlled;

	private GameObject actualStreet;

	private string direction;
	private string generalDirection;
	private bool isNewStreet;
	private int streetIndex;
	private Vector3 fwd;
	private float raycastSize;

	int countTraffic = 0;
	private bool mayIdrive = true;

	// Use this for initialization
	void Start()
	{
		speed = Simulation.getInstance().getCarSpeed();
		maxSpeedSlider = GameObject.Find("SliderMaxCarSpeed").GetComponent<Slider>();
		maxSpeed = maxSpeedSlider.value;
		maxSpeedSlider.onValueChanged.AddListener(delegate { maxSpeedValueChangeCheck(); });
		fwd = transform.TransformDirection(Vector3.forward);
	}


	public void initialize(GameObject street, string _direction)
	{
		direction = _direction;
		actualStreet = street;
		isNewStreet = true;
	}

	private void accelerate()
	{
		float a = ps / mass;
		speed = speed + a * Time.deltaTime;
	}

	private void brakeWithDistance(float distance)
	{
		if(distance > 1.5f)
		{
			speed = speed - 2 * distance * Time.deltaTime;
		}
		else
		{
			speed = 0.01f;
		}
	}

	private void brake()
	{
		float a = ps / mass;
		speed = speed - a * Time.deltaTime;
	}

	// Update is called once per frame
	void Update()
	{
		if(speed < maxSpeed && mayIdrive == true)
		{
			accelerate();
		}
		if(speed > maxSpeed)
		{
			brake();
		}
		if(speed < 0f)
		{
			speed = 0.01f;
		}
		rotationSpeed = speed / 2f;
		checkRaycast();
		resizeCollider();

		Vector3 dir = new Vector3();
		if(targetPathNode == null)
		{
			GetNextPathNode();
		}
		try
		{
			dir = targetPathNode.position;
			if(gameObject.name.Equals("jeep(Clone)"))
			{
				dir.y = 1.3f;
			}
			else if(gameObject.name.Equals("Tanker(Clone)"))
			{
				dir.y = -0.25f;
			}

			dir = dir - transform.localPosition;
			float distThisFrame = speed * Time.deltaTime;

			if(dir.magnitude <= distThisFrame)
			{
				targetPathNode = null;
			}
			else
			{
				transform.Translate(dir.normalized * distThisFrame, Space.World);
				Quaternion targetRotation = Quaternion.LookRotation(dir);
				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
			}
		}
		catch(Exception)
		{

		}
	}

	private void checkRaycast()
	{
		// Save current object layer
		int oldLayer = gameObject.layer;
		//Change object layer to a layer it will be alone
		gameObject.layer = 12;
		int layerToIgnore = 1 << 12;
		layerToIgnore = ~layerToIgnore;

		RaycastHit[] hits;
		hits = Physics.RaycastAll(transform.position, transform.forward, raycastSize, layerToIgnore);
		bool somethingInFront = false;
		for(int i = 0; i < hits.Length; i++)
		{
			RaycastHit hit = hits[i];
			GameObject collidedObject = hit.collider.gameObject;
			if(collidedObject.name.Equals("jeep(Clone)") || collidedObject.name.Equals("Rock(Clone)"))
			{
				float distance = getDistance(gameObject, collidedObject);
				if(gameObject.name.Equals("jeep(Clone)"))
				{
					distance = distance - (lengthCar / 2 + 1f);
				}
				else
				{
					distance = distance - (lengthTanker / 2 + 1f);
				}
				brakeWithDistance(distance);
				mayIdrive = false;
				somethingInFront = true;
			}
			if(somethingInFront == false)
			{
				mayIdrive = true;
			}
		}
		raycastSize = speed;
		if(raycastSize < 1)
		{
			raycastSize = 1;
		}
		// set the game object back to its original layer
		gameObject.layer = oldLayer;
	}

	private float getDistance(GameObject itself, GameObject hitObject)
	{
		Vector3 itselfPosition = itself.transform.position;
		Vector3 hitObjectPosition = hitObject.transform.position;

		float distanceX = Math.Abs(hitObjectPosition.x - itselfPosition.x);
		float distanceZ = Math.Abs(hitObjectPosition.z - itselfPosition.z);

		float distance = 0;
		if(distanceX > distanceZ)
		{
			distance = distanceX;
		}
		else
		{
			distance = distanceZ;
		}
		return distance;
	}

	void OnTriggerEnter(Collider collider)
	{
		Console.WriteLine("Detected collision between " + gameObject.name + " and " + collider.name);
		GameObject itself = gameObject;
		GameObject colliededObject = collider.gameObject;

		float distance = getDistance(itself, colliededObject);
		if((colliededObject.name.Equals("PosX") ||
			colliededObject.name.Equals("NegX") ||
			colliededObject.name.Equals("PosY") ||
			colliededObject.name.Equals("NegY")) && (firstCollider == null))
		{
			//Here the car collided with a crossing collider
			crossingCurrent = collider.transform.parent.gameObject.transform.parent.gameObject; //The current crossing?!
			firstCollider = colliededObject;
		}
	}

	void OnTriggerStay(Collider collider)
	{
		GameObject collidedObject = collider.gameObject;

		if(collidedObject.name.Equals("PosX") ||
		collidedObject.name.Equals("NegX") ||
		collidedObject.name.Equals("PosY") ||
		collidedObject.name.Equals("NegY"))
		{
			CrossingColliderT crossT = null;
			CrossingColliderX crossX = null;

			float distance = getDistance(gameObject, collidedObject);
			if(collidedObject == firstCollider)
			{
				crossT = collidedObject.GetComponentInParent<CrossingColliderT>();
				if(crossT == null)
				{
					crossX = collidedObject.GetComponentInParent<CrossingColliderX>();
					if(crossX == null)
					{
						return;
					}
					RemoteObject.Enum.TrafficLightsStatus trafficLightStatus = crossX.actLightState;
					carDecisionOnCrossing(trafficLightStatus, distance);
				}
				else
				{
					RemoteObject.Enum.TrafficLightsStatus trafficLightStatus = crossT.actLightState;
					carDecisionOnCrossing(trafficLightStatus, distance);
				}
			}
		}
	}

	private void resizeCollider()
	{
		CapsuleCollider collider = GetComponent<CapsuleCollider>();
		float radius = speed * 0.01f;
		float height = speed * 0.04f;
		float centerZ = height / 2f - radius;

		Vector3 center = new Vector3(0f, 0f, centerZ);
		collider.center = center;
		collider.height = height;
	}

	void GetNextPathNode()
	{
		if(pathGO == null || pathNodeIndex >= pathGO.transform.childCount)
		{
			getNextStreetPart();
		}
		try
		{
			targetPathNode = pathGO.transform.GetChild(pathNodeIndex);
		}
		catch(Exception ex)
		{
			Console.WriteLine("Exception occured: " + ex.Message);
		}

		pathNodeIndex++;
	}

	private void getNextStreetPart()
	{
		if(crossingNext != null)
		{
			pathGO = crossingNext;
			crossingNext = null;
			pathNodeIndex = 0;
			targetPathNode = pathGO.transform.GetChild(pathNodeIndex);
			isNewStreet = true;
		}
		else if(actualStreet != null)
		{
			if(isNewStreet)
			{
				generalDirection = direction;
				if(generalDirection.Equals("PathPos"))
				{
					streetIndex = 0;
				}
				else if(generalDirection.Equals("PathNeg"))
				{
					streetIndex = actualStreet.transform.childCount - 1;
				}
				isNewStreet = false;
				firstCollider = null;
			}

			if(generalDirection.Equals("PathPos"))
			{
				if(streetIndex > actualStreet.transform.childCount)
				{
					Console.WriteLine("destroyed");
					Destroy(gameObject);
				}
				pathGO = getChildGameObject(actualStreet.transform.GetChild(streetIndex).gameObject, direction);
				streetIndex++;
				pathNodeIndex = 0;
			}
			else if(generalDirection.Equals("PathNeg"))
			{
				if(streetIndex < 0)
				{
					Console.WriteLine("destroyed");
					Destroy(gameObject);
				}
				pathGO = getChildGameObject(actualStreet.transform.GetChild(streetIndex).gameObject, direction);
				streetIndex--;
				pathNodeIndex = 0;
			}
			else
			{
				System.Console.WriteLine("destroyed");
				Destroy(gameObject);
			}
		}
		else
		{
			System.Console.WriteLine("destroyed");
			Destroy(gameObject);
		}
	}

	static private GameObject getChildGameObject(GameObject fromGameObject, string withName)
	{
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
		foreach(Transform t in ts) if(t.gameObject.name == withName) return t.gameObject;
		return null;
	}

	public void decideWay(CrossingColliderX collider)
	{
		if(!isNewStreet)
		{
			System.Random random = new System.Random();
			int randomNumber = random.Next(0, 3);
			nextCrossingColliderT = null;
			nextCrossingColliderX = collider;
			collider.setDirection(randomNumber, this);
		}

	}

	public void decideWay(CrossingColliderT collider)
	{
		if(!isNewStreet)
		{
			System.Random random = new System.Random();
			int randomNumber = random.Next(0, 2);
			nextCrossingColliderT = collider;
			nextCrossingColliderX = null;
			collider.setDirection(randomNumber, this);
		}
	}

	public void setNewDirection(GameObject crossingWay, GameObject nextWay, bool isControlled)
	{
		crossingNext = crossingWay;
		actualStreet = nextWay;
	}

	public void setNewDirection(GameObject crossingWay, GameObject nextWay, bool isControlled, string uuidTrafficLight, string nameTrafficLight)
	{
		crossingNext = crossingWay;
		actualStreet = nextWay;
		nextCrossingIsControlled = isControlled;
	}

	public void setStreetDirection(string dir)
	{
		direction = dir;
	}

	public void maxSpeedValueChangeCheck()
	{
		maxSpeed = maxSpeedSlider.value;
	}

	private void carDecisionOnCrossing(RemoteObject.Enum.TrafficLightsStatus trafficLightStatus, float distance)
	{
		switch(trafficLightStatus)
		{
			case RemoteObject.Enum.TrafficLightsStatus.Green:
				accelerate();
				break;
			case RemoteObject.Enum.TrafficLightsStatus.BlinkGreen:
				accelerate();
				break;
			case RemoteObject.Enum.TrafficLightsStatus.Yellow:
				brakeWithDistance(distance);
				break;
			case RemoteObject.Enum.TrafficLightsStatus.BlinkYellow:
				break;
			case RemoteObject.Enum.TrafficLightsStatus.RedYellow:
				accelerate();
				break;
			case RemoteObject.Enum.TrafficLightsStatus.Red:
				brakeWithDistance(distance);
				break;
		}
	}
}
