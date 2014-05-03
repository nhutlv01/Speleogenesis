using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float timerTimeLeft = 3.0f;
	public float timerMax = 3.0f;
	public float barScale;
	public GameObject barObject;
	public TextMesh textMesh;
	public bool bPaused = false;
	public float timerDecreaseLevel;
	bool bPreAttack = false;
	// Use this for initialization
	void Start () {
		NotificationCenter.DefaultCenter().AddObserver(this, "Pause");
		NotificationCenter.DefaultCenter().AddObserver(this, "Unpause");
		NotificationCenter.DefaultCenter().AddObserver(this, "AddTime");

		textMesh = this.GetComponent<TextMesh> ();
		textMesh.text = timerMax.ToString();
		 
		/*textMesh.font = Resources.Load ("Fonts/Lemiesz") as Font;
		textMesh.renderer.material = textMesh.font.material;
		textMesh.characterSize = 0.02f;
		textMesh.alignment = TextAlignment.Center;
		textMesh.anchor = TextAnchor.MiddleCenter;
		textMesh.fontSize = 100;*/
	}
	
	// Update is called once per frame
	void Update () {
		if(!bPaused)
		{
			if (timerTimeLeft <= 0.0f)
			{
				ResetTimer();
			}
			else if(timerTimeLeft <= 1.30f)
			{
				if(!bPreAttack)
				{
					bPreAttack = true;
					Debug.Log("PreEnemyAttack");
					NotificationCenter.DefaultCenter().PostNotification (this, "PreEnemyAttack");
				}
			}
			Color colorLerp;
			timerTimeLeft = Mathf.Round((timerTimeLeft - Time.deltaTime) * 1000f) / 1000f;
			if(timerTimeLeft < timerMax)
			{
				barObject.transform.localScale = new Vector3 (timerTimeLeft / timerMax * barScale + .001f, barObject.transform.localScale.y, barObject.transform.localScale.z);
				colorLerp = Color.Lerp(Color.red, Color.green, timerTimeLeft % timerMax / timerMax);
			}
			else
			{
				barObject.transform.localScale = new Vector3 (timerTimeLeft % timerMax / timerMax * barScale + .001f, barObject.transform.localScale.y, barObject.transform.localScale.z);
				colorLerp = Color.Lerp(Color.green, Color.cyan, timerTimeLeft % timerMax / timerMax);
			}
			barObject.GetComponent<SpriteRenderer>().color = colorLerp;
			textMesh.text = timerTimeLeft.ToString ("F2");
			textMesh.color = colorLerp;
		}
	}
	

	void ResetTimer()
	{
		NotificationCenter.DefaultCenter().PostNotification (this, "TimerTrigger");
		bPreAttack = false;
		timerTimeLeft = timerMax;
	}

	void AddTime(Notification notification)
	{
		timerTimeLeft += (float)notification.data;
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
