using UnityEngine;
using System.Collections;

public class TileSprite : MonoBehaviour {

	public Tile parentTile;
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

		parentTile = transform.parent.gameObject.GetComponent<Tile>();
		
		if(parentTile != null)
		{
			parentTile.type = parentTile.TileTypes [Random.Range (0, parentTile.TileTypes.Length)];
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			//if this is a 
			if (parentTile.type == "EnemyWeapon") {
				parentTile.subtype = parentTile.SubTypes [Random.Range(0, parentTile.SubTypes.Length)];
				tilePath = "TileTypes/" + parentTile.subtype;
			}
			else
				tilePath = "TileTypes/" + parentTile.type;
			sr.sprite = Resources.Load<Sprite>(tilePath);
		}
		else {
			Debug.Log("parentTile is null");
		}

	}


}
