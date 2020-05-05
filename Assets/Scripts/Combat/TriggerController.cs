using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour {

    public GameObject prefabBoss;
    public Vector3 spawnPosition;

    private bool activated = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>() != null && !activated)
        {
            activated = true;
            CombatManager.instance.spawnBoss(other.gameObject, prefabBoss, spawnPosition);
        }
    }
}
