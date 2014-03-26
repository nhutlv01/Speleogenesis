using UnityEngine;
using System.Collections;

public class TileWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		if(hitInfo.name != "Prod")
			hitInfo.rigidbody.velocity = Vector2.zero;
	}
}
