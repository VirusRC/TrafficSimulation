using CymaticLabs.Unity3D.Amqp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRegister : MonoBehaviour {
    private GameObject _spawnfield;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ampqUpdate(string msg)
    {

        Debug.Log("Rabbitqm msg received");
        Spawn sp = GameObject.FindWithTag("spawn1").GetComponent<Spawn>();

        sp.generateCar(1);

    }
}
