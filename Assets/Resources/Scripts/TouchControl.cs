using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchControl : MonoBehaviour {
	public List<Vector2> positions = new List<Vector2>();
	public List<Tile> tilesTouches = new List<Tile>();
	bool trace = false;


	void Start()
	{

	}
	void OnMouseDown()
	{
		Debug.Log ("Trace off");
		trace = true;
	}

	void OnMouseUp()
	{
		Debug.Log ("Trace On");
		trace = false;
	}

	void Update()
	{
		Vector2 pos = Input.mousePosition;
		if (trace) {
			Debug.Log("Add Positions");
			positions.Add(pos);
		}

	}

}
	/*public List<Tile> tilesTouched = new List <Tile>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit2D hitInfo;
		if(Input.GetMouseButtonDown(0))
		{
			hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if(hitInfo)
			{
				Debug.Log(string.Format("Start: {0}",hitInfo.transform.gameObject.name));
				tilesTouched.Add (hitInfo.transform.GetComponent<Tile>());
			}
		}
		if (Input.GetMouseButtonUp (0)) 
		{
			hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if(hitInfo)
				Debug.Log(string.Format("End: {0}",hitInfo.transform.gameObject.name));
		}
	}
}*/
