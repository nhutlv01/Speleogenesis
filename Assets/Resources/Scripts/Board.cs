using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Board : MonoBehaviour {
	public List<Tile> tiles = new List <Tile>();
	public int GridWidth;
	public int GridHeight;
	public GameObject tilePrefab;
	//public GameObject paperTilePrefab;

	// Use this for initialization
	void Start () {
		for (int y = 0; y < GridHeight; y++) {
			for (int x = 0; x < GridWidth; x++)
			{
				GameObject g = Instantiate(tilePrefab, new Vector3(x,y,0), Quaternion.identity)as GameObject;
				g.transform.name = string.Format("Tile ({0},{1})",x,y);
				//GameObject s = Instantiate(paperTilePrefab, new Vector3(x,y,0), new Quaternion(0,0,Random.Range(0,3)*90,0)) as GameObject;
				g.transform.parent =gameObject.transform;
				//s.transform.parent = gameObject.transform;
				tiles.Add(g.GetComponent<Tile>());
			}
				
		}

		gameObject.transform.position = new Vector3 (-2.525f, -2.0f, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
