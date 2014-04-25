using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {

	public bool bPaused = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		if(!bPaused)
		{
			NotificationCenter.DefaultCenter().PostNotification(this, "Pause");
			bPaused = true;
		}
		else
		{
			NotificationCenter.DefaultCenter().PostNotification(this, "Unpause");
			bPaused = false;
		}
	}
}
