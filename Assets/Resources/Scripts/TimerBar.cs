using UnityEngine;
using System.Collections;

public class TimerBar : MonoBehaviour {
	public float percent;
	public float current;
	public float max;
	public float barScale;
	public GameObject barObject;
	public GameObject text;
	public TextMesh textmesh;
	// Use this for initialization
	void Start () {
		textmesh = GetComponent<TextMesh> ();
		textmesh.font = Resources.Load ("Fonts/Lemiesz") as Font;
		textmesh.renderer.material = textmesh.font.material;
		textmesh.characterSize = 0.02f;
		textmesh.alignment = TextAlignment.Center;
		textmesh.anchor = TextAnchor.MiddleCenter;
		textmesh.fontSize = 100;
	}
	
	// Update is called once per frame
	void Update () {
		barObject.transform.localScale = new Vector3 (percent * barScale *.01f +.001f, barObject.transform.localScale.y, barObject.transform.localScale.z);
		textmesh.text = ("(" + Mathf.Round(current) + "," + max + ")");
		
	}
}
