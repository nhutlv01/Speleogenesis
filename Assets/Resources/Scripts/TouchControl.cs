using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchControl : MonoBehaviour {
	public List<Tile> tilesTouched = new List <Tile>();
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
}
