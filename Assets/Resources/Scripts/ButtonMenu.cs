using UnityEngine;
using System.Collections;

public class ButtonMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if (GUI.Button (new Rect (10, 70, 100, 100), "New Game")) {
			Application.LoadLevel("MainGame");
				
		}

	}
}
