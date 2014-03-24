using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	//Variables
	private int GridWidth = 6;
	private int GridHeight = 6;
	public GameObject tilePrefab;
	public GameObject fireLine;
	public GameObject tileSpawn;
	public Tile[,] tileBoard = new Tile[6,6]; 
	public List<Tile> tilesTouched = new List<Tile>();
	public bool objectsRemoved;
	public Player playerPrefab;
	public Player player;
	public bool bShifting = false;
	bool trace = false;
	bool bFirstTile = false;
	string tileType = "";
	private float GridXOffset = -2.525f;
	private float GridYOffset = 2.0f;


	/// <summary>
	/// void Start ()
	/// 
	/// Initializes the gameboard.
	/// 
	/// </summary>
	void Start () {

		player = Instantiate (playerPrefab) as Player;

		for (int x = 0; x < GridWidth; x++) {
			for (int y = 0; y < GridHeight; y++)
			{
				GameObject g = Instantiate(tilePrefab, new Vector3(x,y,0), Quaternion.identity)as GameObject;
				g.transform.name = string.Format("Tile ({0},{1})",x,y);
				g.transform.parent =gameObject.transform;
				tileBoard[x,y] = g.GetComponent<Tile>();
				g.GetComponent<SliderJoint2D>().anchor = new Vector2(x+GridXOffset, y);
			}
				
		}

		gameObject.transform.position = new Vector3 (GridXOffset, GridYOffset, 0);
	}

	/// <summary>
	/// void Update()
	/// 
	/// track fingers and draw lines
	/// 
	/// </summary>
	void Update()
	{
		handleInput ();
		trackPosition ();
		DrawLines ();
		tilesUpdateNeighbors ();
		if(!bShifting)
			shiftColumnsDown ();
	}


	/// <summary>
	/// void handleInput()
	/// 
	/// This function handles input from mouse/touch clicks and drags.
	/// 
	/// </summary>
	void handleInput()
	{
		bool bMatched3 = (tilesTouched.Count >= 3);
		//Starting click, start dragging
		if (Input.GetMouseButtonDown (0) && !trace) {
				trace = true;
				bFirstTile = true;
				//Ending click, end dragging, clear array, delete tiles
		} else if (Input.GetMouseButtonUp (0) && trace && bMatched3) {
				trace = false;
				breakdownTileScore(tileType,tilesTouched.Count);
				deleteAllTiles (tilesTouched);
				tilesTouched.Clear ();
				tileType = "";
		} else if (Input.GetMouseButtonUp (0) && trace && !bMatched3) {
				trace = false;
				tilesTouched.Clear ();
				tileType = "";
		}

	}
	
	/// <summary>
	/// void trackPosition(void)
	/// 
	/// This function keeps tracks of which tiles are touched
	/// 
	/// </summary>
	void trackPosition ()
	{
		int tileFoundIndex = 0;
		Vector3 pos;
		RaycastHit2D rayHit;
		Tile tileFound;

		//If we have started to trace
		if (trace) {
			bool bTileFound = false;
			//Calculate position of mouse
			pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			//Cast a ray at mouse position
			rayHit = Physics2D.Raycast (pos, Vector2.zero);
			//if there is a collision
			if (rayHit.collider != null ) {
				//set tileFound to rayHit Tile
				tileFound = rayHit.transform.GetComponent<Tile> ();
				//if this is the first tile
				if(bFirstTile)
				{
					Debug.Log ("First Tile");
					//Set the type of drag to this tile's type
					tileType = tileFound.type;
					//Add tile to the array
					tilesTouched.Add (tileFound);
					//indicate we are no longer dealing with the first tile
					bFirstTile = false;
				}
				//if this is not the first tile
				else{
					//search the touchedTiles for this tile
					tileFoundIndex = findTiles(tilesTouched, tileFound);
					//set boolean
					if(tileFoundIndex >= 0)
					{
						bTileFound = true;
					}
					//if the tiletype matches
					if(tileType == tileFound.type)
					{
						//if the tile is not found and is a neighbor of the last tile
						if (!bTileFound && tileIsNeighbor(tileFound)) {
						//add to array
							Debug.Log ("Added Tile: " + tileFound);
							tilesTouched.Add (tileFound);
						}
						//else it is found in the array, and it wasn't the last tile we dragged over
						else if (bTileFound && tileIsNotLastTile(tileFound)) {
							//remove all tiles up to the previous position of this tile in the array
							Debug.Log("Removing tiles: " + tileFoundIndex + " - " + tilesTouched.Count + tileFoundIndex);
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
	}

	/// <summary>
	/// void DrawLines()
	/// Simple debug function for drawing lines over the mouse/touch inputs.
	/// 
	/// </summary>
	void DrawLines()
	{
		LineRenderer l = fireLine.GetComponent<LineRenderer> ();
		if (tilesTouched.Count >= 1) {
			if(!l.enabled)
				l.enabled = true;
			l.SetVertexCount(tilesTouched.Count);
			for (int i = 0; i < tilesTouched.Count; i++)
					fireLine.GetComponent<LineRenderer> ().SetPosition (i, tilesTouched [i].transform.position);
		} else
			fireLine.GetComponent<LineRenderer> ().enabled = false;
			/*
		Vector3 pos1;
		Vector3 pos2;
		for(int i = 0; i < tilesTouched.Count-1; i++)
		{
			pos1 = tilesTouched[i].transform.position;
			pos2 = tilesTouched[i+1].transform.position;
			//Debug.DrawLine(pos1,pos2);
			GameObject g = Instantiate(linePrefab, new Vector3(pos1.x,pos1.y,0), Quaternion.identity)as GameObject;
			g.GetComponent<LineRenderer>().SetPosition(i, pos1);
		}
		*/
	}

	/// <summary>
	/// int findTiles(List<Tile> tileArray, Tile toFind, ref bool bTileFound)
	/// tileArray - any array of tiles
	/// Tile toFind - the tile you are looking for in the array.
	/// returns int - index of the array if found, otherwise -1
	/// Simple debug function for drawing lines over the mouse/touch inputs.
	/// 
	/// </summary>
	int findTiles(List<Tile> tileArray, Tile toFind)
	{
		int arrayIndex = -1;
		for (int i = 0; i < tileArray.Count; i++) {
			if (toFind.name == tileArray [i].name) {
					arrayIndex = i;
					break;
			}
		}
		return arrayIndex;
	}

	/// <summary>
	/// bool tileIsNeighbor(Tile tileFound)
	/// tileFound - tile to check
	/// 
	/// Function which checkts the last tile in the tilesTouched 
	/// array to see if it is a neighbor
	/// 
	/// </summary>
	bool tileIsNeighbor(Tile tileFound)
	{
		bool retval = false;
		if (tilesTouched.Count >= 1) {
				Tile t = tilesTouched [tilesTouched.Count - 1];
				if (findTiles (t.Neighbors, tileFound) >= 0)
						retval = true;
		}
		return retval;
	}

	bool tileIsNotLastTile(Tile tileFound)
	{
		return tilesTouched[tilesTouched.Count-1].name != tileFound.name;
	}

	void deleteAllTiles(List<Tile> tileArray)
	{
			foreach (Tile t in tileArray) {
				Destroy (t.gameObject);
			}
	}

	/*
	void tilesCheckPositions()
	{
		if (!tilesAreMoving ()) {
						for (int y = 0; y < GridHeight; y++) {
								for (int x = 0; x < GridWidth; x++) {
										RaycastHit2D rayHit = Physics2D.Raycast (new Vector2 (x - 2.525f, y - 2.0f), Vector2.zero);		
											if(tileBoard[x,y] != null)
											{
												tileBoard [x, y] = rayHit.transform.GetComponent<Tile> ();
												tileBoard [x,y].transform.name = string.Format("Tile ({0},{1})",x,y);
											}	

								}
						}
				}
	}*/

	bool tilesAreMoving()
	{
		Rigidbody2D[] rigidbodies;
		rigidbodies = GetComponentsInChildren<Rigidbody2D> ();
		foreach (Rigidbody2D body in rigidbodies)
		{
			if(body.velocity != Vector2.zero)
				return true;
		}
		return false;
	}

	void tilesUpdateNeighbors()
	{
		Tile[] tiles = GetComponentsInChildren<Tile>();
		foreach (Tile tile in tiles)
			tile.checkNeighbors ();
	}


	void shiftColumnsDown()
	{
		bShifting = true;
		for (int x = 0; x < GridWidth; x++){
			for (int y = 0; y < GridHeight; y++){
				if (tileBoard [x, y] == null) {
					for(int i = y; i < GridHeight; i++){
						if(tileBoard[x,i] != null)
						{
							Debug.Log ("Shifting Col: " + x + " Row: " + i + " to " + y);
							tileBoard[x,y] = tileBoard[x,i];
							tileBoard[x,y].transform.name = string.Format ("Tile ({0},{1})", x, y);
							tileBoard[x,i] = null;
							break;
						}
					}
				}	
			}
		}
		refillGameBoard ();
	}

	void refillGameBoard()
	{
		Collider2D spawn = tileSpawn.collider2D;
		for (int y = 0; y < GridHeight; y++)
			for (int x = 0; x < GridWidth; x++)
			{
				if (tileBoard [x, y] == null) {
					GameObject g = Instantiate (tilePrefab, new Vector3 (x-2.525f, y + 3.7f, 0), Quaternion.identity)as GameObject;
					g.GetComponent<SliderJoint2D>().anchor = new Vector2(x-2.525f, y-2f);
					string tileName = string.Format ("Tile ({0},{1})", x, y);
					g.transform.name = tileName;
					Debug.Log("Refilling with tile" + tileName);
					g.transform.parent = gameObject.transform;
					tileBoard [x, y] = g.GetComponent<Tile> ();
				}
           }
		bShifting = false;
	}

	void breakdownTileScore(string tileType, int arrayLength)
	{
		if (tileType == "Potion") {
			player.currentHealth += arrayLength * player.strength * 1;
			if (player.currentHealth > player.maxHealth)
					player.currentHealth = player.maxHealth;
		} else if (tileType == "Armor") {
			player.armor += arrayLength * 15;
			float armorToAdd = (float)arrayLength * (player.salvageMultiplier * 7.0f + 1.15f);
			if ((player.currentSalvage += armorToAdd) > player.maxSalvage) {
					player.currentSalvage -= player.maxSalvage;
					//TODO: player got a salvage level
				}
		} else if (tileType == "Enemy")
		{
			float xpToAdd = (float)arrayLength* (player.xpMultiplier * 7.0f + 1.15f);
			if((player.currentXP += xpToAdd) > player.maxXP)
			{
				player.currentXP -= player.maxXP;
				//TODO: player got a new level
			}
		}
	}
}


