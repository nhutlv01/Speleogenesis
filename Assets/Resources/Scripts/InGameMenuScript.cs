using UnityEngine;
using System.Collections;

public class InGameMenuScript : MonoBehaviour {

	public bool bMenu = false;
	// Use this for initialization
	void Start () {
		NotificationCenter.DefaultCenter().AddObserver(this, "MenuEnter");
		NotificationCenter.DefaultCenter().AddObserver(this, "MenuExit");
		DisableChildren();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void MenuEnter()
	{
		if(!bMenu)
		{
			//Enable the menu
			EnableChildren();
			bMenu = true;

			//Pause everything
			NotificationCenter.DefaultCenter().PostNotification(this, "Pause");

		}
	}

	void MenuExit()
	{
		if(bMenu)
		{
			//Disable the menu
			DisableChildren();
			bMenu = false;

			//Unpause everything
			NotificationCenter.DefaultCenter().PostNotification(this, "Unpause");
		}
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
