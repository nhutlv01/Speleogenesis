using UnityEngine;
using System.Collections;

public class TextShadowScript : MonoBehaviour {

	Renderer r;
	// Use this for initialization
	void Start () {
		r = GetComponent<Renderer>();
		r.sortingLayerName = "GUI";
		r.sortingOrder = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
