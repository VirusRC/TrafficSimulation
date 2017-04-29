using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetCollider : MonoBehaviour {

    private string direction;

	// Use this for initialization
	void Start () {
        direction = gameObject.transform.parent.gameObject.name;
    }

    private void OnTriggerEnter(Collider other)
    {
        FollowWay next = (FollowWay)other.GetComponent(typeof(FollowWay));
        next.setStreetDirection(direction);
    }
}
