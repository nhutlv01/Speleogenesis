using UnityEngine;
using System.Collections;

public class TileSpawn : MonoBehaviour {

	public bool[] spawnTaken = new bool[6];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		int access = (int)(hitInfo.transform.position.x - 2.525f);
		spawnTaken [access] = true;
		Debug.Log ("SpawnTaken " + access);
	}
}
