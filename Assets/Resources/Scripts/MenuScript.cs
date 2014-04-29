using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		DisableChildren();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void menuEnable()
	{
			//Enable the menu
			EnableChildren();
	}

	public void menuDisable()
	{
			//Disable the menu
			DisableChildren();
	}

	void DisableChildren(){
		foreach(Transform child in this.transform)
		{
			if(child.gameObject.name != "Menu")
				child.gameObject.SetActive(false);
		}
	}

	void EnableChildren(){
		foreach(Transform child in this.transform)
		{
			if(child.gameObject.name != "Menu")
				child.gameObject.SetActive(true);
		}
	}
}
