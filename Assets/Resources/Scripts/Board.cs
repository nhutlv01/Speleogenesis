using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Board : MonoBehaviour {
	public List<Tile> tiles = new List <Tile>();
	public int GridWidth;
	public int GridHeight;
	public GameObject tilePrefab;
	public List<Vector2> positions = new List<Vector2>();
	public List<Tile> tilesTouched = new List<Tile>();
	bool trace = false;
	bool firstTile = false;
	string tileType = "";

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

	void trackPosition ()
	{
		int tileFoundIndex = 0;
		Vector3 pos;
		RaycastHit2D rayHit;
		Tile tileFound;
		if (Input.GetMouseButtonDown (0) && !trace) {
			trace = true;
			firstTile = true;
		} else if (Input.GetMouseButtonUp (0) && trace) {
			trace = false;
			tilesTouched.Clear();
			tileType = "";
			tileFoundIndex = 0;
		}

		if (trace) {
			bool bTileFound = false;
			//Calculate position of mouse
			pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			//Cast a ray at mouse position
			rayHit = Physics2D.Raycast (pos, Vector2.zero);
			//if there is a collision
			if (rayHit.collider != null) {
				//set tileFound to rayHit Tile
				tileFound = rayHit.transform.GetComponent<Tile> ();
				if(firstTile)
				{
					Debug.Log ("First Tile");
					tileType = tileFound.type;
					firstTile = false;
				}
				else{
					tileFoundIndex = findTiles(tilesTouched, tileFound, ref bTileFound);
					if(tileType == tileFound.type)
					{
						if (!bTileFound) {
						//add to array
							Debug.Log ("Added Tile {0}", tileFound);
							tilesTouched.Add (tileFound);
						}
						//else it is found in the array
						else if (bTileFound && tileIsNotLastTile(tileFound)) {
						//find where in the array there is a tile
							Debug.Log(string.Format("Removing tiles {0} - {1}", tileFoundIndex, tilesTouched.Count - tileFoundIndex));
							tilesTouched.RemoveRange (tileFoundIndex, tilesTouched.Count - tileFoundIndex);
						}
						else
						{
							//Do nothing
						}
					}
				}
			}
		}
		//}

	}

	void DrawLines()
	{
		Vector3 pos1;
		Vector3 pos2;
		for(int i = 0; i < tilesTouched.Count-1; i++)
		{
			pos1 = tilesTouched[i].transform.position;
			pos2 = tilesTouched[i+1].transform.position;
			Debug.DrawLine(pos1,pos2);
		}
	}

	int findTiles(List<Tile> tilesTouched, Tile toFind, ref bool bTileFound)
	{
		int tileFoundIndex = 0;
		foreach (Tile t in tilesTouched) {
			//If the tile found is in the array
			if (toFind.name == t.name) {
				bTileFound = true;
				break;
			} else
				tileFoundIndex++;
		}
		return tileFoundIndex;
	}

	bool tileIsNotLastTile(Tile tileFound)
	{
		return tilesTouched[tilesTouched.Count-1].name != tileFound.name;
	}

	void Update()
	{
		trackPosition ();
		DrawLines();
	}
}
