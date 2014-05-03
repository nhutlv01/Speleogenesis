using UnityEngine;
using System.Collections;

public class TileSprite : MonoBehaviour {

	public Tile parent;
	// Use this for initialization
	void Start () {
		CreateTile();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void CreateTile()
	{
		string tilePath;
		parent.type = parent.TileTypes [Random.Range (0, parent.TileTypes.Length)];
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		//if this is a 
		if (parent.type == "EnemyWeapon") {
			parent.subtype = parent.SubTypes [Random.Range(0, parent.SubTypes.Length)];
			tilePath = "TileTypes/" + parent.subtype;
		}
		else
			tilePath = "TileTypes/" + parent.type;
		sr.sprite = Resources.Load<Sprite>(tilePath);
	}


}
