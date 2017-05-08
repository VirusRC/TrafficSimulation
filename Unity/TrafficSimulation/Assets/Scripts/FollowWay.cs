using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowWay : MonoBehaviour
{

	public float maxSpeed;
	public Slider maxSpeedSlider;

	private float speed = 20f;
	private float rotationSpeed = 2f;
	private float mass = 2000f;
	private float ps = 200f;
	private float carLength = 3.5f;

	private GameObject pathGO;
	Transform targetPathNode;
	int pathNodeIndex = 0;

	private GameObject crossingNext;
	private CrossingColliderT nextCrossingColliderT;
	private CrossingColliderX nextCrossingColliderX;
	private bool nextCrossingIsControlled;

	private GameObject actualStreet;

	private string direction;
	private string generalDirection;
	private bool isNewStreet;
	private int streetIndex;

	int countTraffic = 0;

	//private Rigidbody rigedBody;

	// Use this for initialization
	void Start()
	{
		speed = Simulation.getInstance().getCarSpeed();
		maxSpeedSlider = GameObject.Find("SliderMaxCarSpeed").GetComponent<Slider>();
		maxSpeed = maxSpeedSlider.value;
		maxSpeedSlider.onValueChanged.AddListener(delegate { maxSpeedValueChangeCheck(); });

		//rigedBody = GetComponent<Rigidbody>();
	}


	public void initialize(GameObject street, string direction)
	{
		this.direction = direction;
		this.actualStreet = street;
		isNewStreet = true;
	}

	private void accelerate()
	{
		float a = ps / mass;
		speed = speed + a;
	}

	private void brake(float distance)
	{
		if(distance > 0.4f)
		{
			speed = speed - 2 * distance * Time.deltaTime;
		}
		else
		{
			speed = 0f;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(targetPathNode == null)
		{
			GetNextPathNode();
		}
		Vector3 dir = targetPathNode.position;
		if(gameObject.name.Equals("jeep(Clone)"))
		{
			dir.y = 1.3f;
		}
		else if(gameObject.name.Equals("Tanker(Clone)"))
		{
			dir.y = -0.25f;
		}

		dir = dir - this.transform.localPosition;

		if(speed < maxSpeed) //&& no collision detected
		{
			//speed += 0.1f;
			accelerate();
		}
		if(speed > maxSpeed)
		{
			//speed -= 0.2f;
			brake(20f);
		}
		rotationSpeed = speed / 2f;
		resizeCollider();

		float distThisFrame = speed * Time.deltaTime;

		if(dir.magnitude <= distThisFrame)
		{
			targetPathNode = null;
		}
		else
		{
			//rigedBody.MovePosition(dir.normalized * distThisFrame);
			transform.Translate(dir.normalized * distThisFrame, Space.World);
			Quaternion targetRotation = Quaternion.LookRotation(dir);
			//rotationSpeed = dir.magnitude * speed / 10f;
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
		}
	}

	private float getDistance(GameObject itself, GameObject hitObject)
	{
		Vector3 itselfPosition = itself.transform.position;
		Vector3 hitObjectPosition = hitObject.transform.position;

		float distance = 0;
		if((itselfPosition.x + hitObjectPosition.x) > (itselfPosition.z + hitObjectPosition.z))
		{
			if(hitObjectPosition.x > itselfPosition.x)
			{
				distance = hitObjectPosition.x - itselfPosition.x;
			}
			if(hitObjectPosition.x < itselfPosition.x)
			{
				distance = itselfPosition.x - hitObjectPosition.x;
			}
		}
		else
		{
			if(hitObjectPosition.z > itselfPosition.z)
			{
				distance = hitObjectPosition.z - itselfPosition.z;
			}
			if(hitObjectPosition.z < itselfPosition.z)
			{
				distance = itselfPosition.z - hitObjectPosition.z;
			}
		}

		return distance;
	}

	//void OnTriggerEnter(Collider collider)
	//{
	//	Console.WriteLine("Detected collision between " + gameObject.name + " and " + collider.name);
	//	GameObject itself = gameObject;
	//	GameObject colliededObject = collider.gameObject;
	//	CrossingT crossT = null;
	//	CrossingX crossX = null;

	//	float distance = getDistance(itself, colliededObject);
	//	if(colliededObject.name.Equals("PosX") ||
	//		colliededObject.name.Equals("NegX") ||
	//		colliededObject.name.Equals("PosY") ||
	//		colliededObject.name.Equals("NegY"))
	//	{
	//		try
	//		{
	//			crossT = colliededObject.GetComponentInParent<CrossingT>();
	//			crossX = colliededObject.GetComponentInParent<CrossingX>();
	//		}
	//		catch(Exception ex)
	//		{
	//			Console.WriteLine("Oops, something went wrong: " + ex.Message);
	//		}

	//		if(crossT != null)
	//		{
	//			RemoteObject.Enum.TrafficLightsStatus trafficLightStatus = crossT.getTrafficLightStatus();
	//			switch(trafficLightStatus)
	//			{
	//				case RemoteObject.Enum.TrafficLightsStatus.Green:
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.BlinkGreen:
	//					brake(distance);
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.Yellow:
	//					brake(distance);
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.BlinkYellow:
	//					brake(distance);
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.RedYellow:
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.Red:
	//					brake(distance);
	//					break;
	//			}
	//		}
	//		else if(crossX != null)
	//		{
	//			RemoteObject.Enum.TrafficLightsStatus trafficLightStatus = crossX.getTrafficLightStatus();
	//			switch(trafficLightStatus)
	//			{
	//				case RemoteObject.Enum.TrafficLightsStatus.Green:
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.BlinkGreen:
	//					brake(distance);
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.Yellow:
	//					brake(distance);
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.BlinkYellow:
	//					brake(distance);
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.RedYellow:
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.Red:
	//					brake(distance);
	//					break;
	//			}
	//		}
	//	}
	//	else if(colliededObject.name.Equals("jeep(Clone)"))
	//	{
	//		if(distance > carLength / 2 + 0.3f)
	//		{
	//			brake(distance);
	//		}
	//	}
	//}

	//void OnTriggerStay(Collider collider)
	//{
	//	Console.WriteLine("Detected collision between " + gameObject.name + " and " + collider.name);
	//	GameObject itself = gameObject;
	//	GameObject colliededObject = collider.gameObject;
	//	CrossingT crossT = null;
	//	CrossingX crossX = null;

	//	float distance = getDistance(itself, colliededObject);
	//	if(colliededObject.name.Equals("PosX") ||
	//		colliededObject.name.Equals("NegX") ||
	//		colliededObject.name.Equals("PosY") ||
	//		colliededObject.name.Equals("NegY"))
	//	{
	//		try
	//		{
	//			crossT = colliededObject.GetComponentInParent<CrossingT>();
	//			crossX = colliededObject.GetComponentInParent<CrossingX>();
	//		}
	//		catch(Exception ex)
	//		{
	//			Console.WriteLine("Oops, something went wrong: " + ex.Message);
	//		}

	//		if(crossT != null)
	//		{
	//			RemoteObject.Enum.TrafficLightsStatus trafficLightStatus = crossT.getTrafficLightStatus();
	//			switch(trafficLightStatus)
	//			{
	//				case RemoteObject.Enum.TrafficLightsStatus.Green:
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.BlinkGreen:
	//					brake(distance);
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.Yellow:
	//					brake(distance);
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.BlinkYellow:
	//					brake(distance);
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.RedYellow:
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.Red:
	//					brake(distance);
	//					break;
	//			}
	//		}
	//		else if(crossX != null)
	//		{
	//			RemoteObject.Enum.TrafficLightsStatus trafficLightStatus = crossX.getTrafficLightStatus();
	//			switch(trafficLightStatus)
	//			{
	//				case RemoteObject.Enum.TrafficLightsStatus.Green:
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.BlinkGreen:
	//					brake(distance);
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.Yellow:
	//					brake(distance);
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.BlinkYellow:
	//					brake(distance);
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.RedYellow:
	//					break;
	//				case RemoteObject.Enum.TrafficLightsStatus.Red:
	//					brake(distance);
	//					break;
	//			}
	//		}
	//	}
	//	else if(colliededObject.name.Equals("jeep(Clone)"))
	//	{
	//		if(distance > carLength / 2 + 0.3f)
	//		{
	//			brake(distance);
	//		}
	//	}
	//}

	private void resizeCollider()
	{
		CapsuleCollider collider = GetComponent<CapsuleCollider>();
		float radius = speed * 0.01f;
		float height = speed * 0.04f;
		float centerZ = height / 2f - radius;

		Vector3 center = new Vector3(0f, 0f, centerZ);
		collider.center = center;
		collider.height = height;
		//collider.radius = radius;
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
}
