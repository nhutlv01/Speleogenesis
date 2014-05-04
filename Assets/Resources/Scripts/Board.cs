using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;


public class Board : MonoBehaviour {

	//Variables
	public int GridWidth = 6;
	public int GridHeight = 6;
	private string tileType = "";
	private float GridXOffset = -2.525f;
	private float GridYOffset = 2.0f;
	
	//Prefabs
	public GameObject tilePrefab;
	public GameObject tileSpawn;
	public GameObject dimPrefab;
	public GameObject lifeBarPrefab;

	//Lines
	public VectorLine arrayLine;
	public VectorLine arrayLineTip;
	public Material arrayLineMat;
	public Material arrayTipMat;

	//Board
	public Tile[,] tileBoard = new Tile[6,6]; 
	public List<Tile> tilesTouched = new List<Tile>();
	public Player player;
	public GameObject dim;
	public LifeBar lifeBar;

	//State booleans
	private bool bShifting = false;
	private bool bPaused = false;
	private bool trace = false;
	private bool bFirstTile = false;


	/// <summary>
	/// void Start ()
	/// 
	/// Initializes the gameboard.
	/// 
	/// </summary>
	void Start () {
		//Add observers
		NotificationCenter.DefaultCenter().AddObserver(this, "Pause");
		NotificationCenter.DefaultCenter().AddObserver(this, "Unpause");

		//Move gameboard into position
		gameObject.transform.position = new Vector3 (0, 0, 1f);

		//Create tileboard
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
		//Create line object
		arrayLine = new VectorLine("MyLine", new Vector3[100], Color.green, arrayLineMat, 5.0f, LineType.Continuous, Joins.Fill);
		arrayLineTip = new VectorLine("MyLine", new Vector3[100], Color.red, arrayTipMat, 5.0f, LineType.Continuous, Joins.Fill);
		arrayLine.ZeroPoints();
		arrayLineTip.ZeroPoints();

		//Initialize dimmer
		dim = Instantiate(dimPrefab,  new Vector3(-GridXOffset, -1.509109f, -2f), Quaternion.identity) as GameObject;
		dim.transform.name = "Dimmer";
		dim.transform.parent = gameObject.transform;
		dim.renderer.enabled = false;
		dim.renderer.sortingLayerName = "Default";
		dim.renderer.sortingOrder = 0;

		//Initialize lifebar
		lifeBar.renderer.enabled = false;
		lifeBar.renderer.sortingLayerName = "GUI";
		lifeBar.renderer.sortingOrder = 1;
		
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
		if(!bPaused)
		{
			handleInput ();
			trackPosition ();
			DrawLines ();
			tilesUpdateNeighbors ();
			if(!bShifting)
				shiftColumnsDown ();
		}
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
		//If mouse release, and we were tracing, and there are 3 matches.
		} else if (Input.GetMouseButtonUp (0) && trace && bMatched3) {
				//Ending click, end dragging, clear array, delete tiles.
				trace = false;
				unDimPieces(tileType);
				disableLifeBar();
				breakdownTileScore(tileType,tilesTouched.Count);
				deleteAllTiles (tilesTouched);
				tilesTouched.Clear ();
				tileType = "";
		} else if (Input.GetMouseButtonUp (0) && trace && !bMatched3) {
				//Ending click, end dragging, clear array, delete tiles.
				trace = false;
				disableLifeBar();
				unDimPieces(tileType);
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

			//Draw lifebar
			drawLifeBar(pos);

			//Cast a ray at mouse position
			rayHit = Physics2D.Raycast (pos, Vector2.zero);

			//if there is a collision
			if(rayHit.collider != null)
			if (rayHit.collider.tag == "Tile" ) {
				//set tileFound to rayHit Tile
				tileFound = rayHit.transform.GetComponent<Tile> ();
				//if this is the first tile
				if(bFirstTile)
				{
					//Debug.Log ("First Tile");
					//Set the type of drag to this tile's type
					tileType = tileFound.type;
					//Dim the pieces
					dimPieces(tileType);
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
							//Debug.Log ("Added Tile: " + tileFound);
							tilesTouched.Add (tileFound);
						}
						//else it is found in the array, and it wasn't the last tile we dragged over
						else if (bTileFound && tileIsNotLastTile(tileFound)) {
							//remove all tiles up to the previous position of this tile in the array
							//Debug.Log("Removing tiles: " + tileFoundIndex + " - " + tilesTouched.Count + tileFoundIndex);
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
		arrayLine.ZeroPoints();
		arrayLineTip.ZeroPoints();
		if(tilesTouched.Count >= 2)
		{
			for (int i = 0; i < tilesTouched.Count; i++)
			{
				if(tilesTouched[i] != null)
				{
					arrayLine.points3[i] = tilesTouched[i].transform.position;
					arrayLine.points3[i].z = -4;
				}
			}
			arrayLineTip.points3[0] = tilesTouched[tilesTouched.Count-1].transform.position;
			arrayLineTip.points3[0].z = -4;
			arrayLineTip.points3[1] = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			arrayLineTip.points3[1].z = -4;
			arrayLine.drawEnd = tilesTouched.Count-1;
			arrayLineTip.drawEnd = 1;

		}
		arrayLine.Draw();
		arrayLineTip.Draw();

		/*
		LineRenderer l = fireLine.GetComponent<LineRenderer> ();
		if (tilesTouched.Count >= 1) {
			if(!l.enabled)
				l.enabled = true;
			l.SetVertexCount(tilesTouched.Count);
			for (int i = 0; i < tilesTouched.Count; i++)
				fireLine.GetComponent<LineRenderer> ().SetPosition (i, new Vector3(tilesTouched [i].transform.position.x, tilesTouched [i].transform.position.y, -2));
		} else
			fireLine.GetComponent<LineRenderer> ().enabled = false;
		*/
			/*
			 * 
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
			if(tileArray[i] == null)
			{
				return -1;
			}
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
				t.dead();
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
							//Debug.Log ("Shifting Col: " + x + " Row: " + i + " to " + y);
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
		for (int y = 0; y < GridHeight; y++)
			for (int x = 0; x < GridWidth; x++)
			{
				if (tileBoard [x, y] == null) {
					GameObject g = Instantiate (tilePrefab, new Vector3 (x-2.525f, y + 3.7f, -1), Quaternion.identity)as GameObject;
					g.GetComponent<SliderJoint2D>().anchor = new Vector2(x-2.525f, y-2f);
					string tileName = string.Format ("Tile ({0},{1})", x, y);
					g.transform.name = tileName;
					//Debug.Log("Refilling with tile" + tileName);
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
					NotificationCenter.DefaultCenter().PostNotification(this, "ArmorUp");
				}
		} else if (tileType == "EnemyWeapon")
		{
			//Count the number of enemies

			//Count the number of weapons

			//Calculate damage

			//Subtract damage from enemy health

			//Calculate number of dead enemies

			//Add xp equivalent to number of enemies killed.
			float xpToAdd = (float)arrayLength* (player.xpMultiplier * 3.0f + 1.15f);
			if((player.currentXP += xpToAdd) > player.maxXP)
			{
				player.currentXP -= player.maxXP;
				NotificationCenter.DefaultCenter().PostNotification(this, "LevelUp");
			}

		}
		else if (tileType == "Time")
		{
			float timeToAdd = (float)arrayLength * player.timeMultiplier * .3f;
			NotificationCenter.DefaultCenter().PostNotification(this, "AddTime", timeToAdd);
		}
	}

	public int numEnemies()
	{
		int counter = 0;
		for(int x = 0; x < GridWidth; x++)
		{
			for(int y = 0; y < GridHeight; y++)
			{
				if(tileBoard[x,y].subtype == "Enemy")
					counter++;
			}
		}
		return counter;
	}

	void dimPieces(string type)
	{
		Vector3 pos;

		//Enable the renderer
		dim.renderer.enabled = true;

		//For all tiles of the same tiletype, move them past the dimmer
		foreach(Tile t in tileBoard)
		{
			if(t.type == type)
			{
				pos = t.transform.position;
				t.transform.position = new Vector3(pos.x, pos.y, -4);
			}
		}
	}

	void unDimPieces(string type)
	{
		Vector3 pos;
		
		//Enable the renderer
		dim.renderer.enabled = false;
		
		//For all tiles of the same tiletype, move them back to original position
		foreach(Tile t in tileBoard)
		{
			if(t.type == type)
			{
				pos = t.transform.position;
				t.transform.position = new Vector3(pos.x, pos.y, -1);
			}
		}
	}

	void drawLifeBar(Vector3 mousePos)
	{
		lifeBar.transform.position = new Vector3( mousePos.x, mousePos.y, -4);
		lifeBar.percent = player.currentHealth / player.maxHealth * 100;
		lifeBar.renderer.enabled = true;

	}

	void disableLifeBar()
	{
		lifeBar.renderer.enabled = false;
	}

	/////////////////////////////////
	//Pause
	/////////////////////////////////
	void Pause()
	{
		bPaused = true;
	}
	
	/////////////////////////////////
	//Unpause
	/////////////////////////////////
	void Unpause()
	{
		bPaused = false;
	}
}


