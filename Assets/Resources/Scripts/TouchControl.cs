using UnityEngine;
using System.Collections;

public class TouchControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Input input = Input.GetMouseButtonDown(0);
		Vector2 origin;
		Vector2 direction;
		if(input)
			RaycastHit2d hitInfo = Physics2D.NavMesh.Raycast(origin, direction);
	}
}
