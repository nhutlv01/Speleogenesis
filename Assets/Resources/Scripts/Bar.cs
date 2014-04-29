using UnityEngine;
using System.Collections;

public class Bar : MonoBehaviour {
	public float percent;
	public float current;
	public float max;
	public float barScale;
	public GameObject barObject;
	public GameObject text;
	public TextMesh textmesh;
	// Use this for initialization
	void Start () {
		text = new GameObject ("Text");
		textmesh = text.AddComponent ("TextMesh") as TextMesh;
		text.transform.parent = transform;
		text.transform.position = text.transform.parent.position + Vector3.back;
		text.GetComponent<Renderer>().sortingLayerName = "GUI";
		text.GetComponent<Renderer>().sortingOrder = 1;
		Debug.Log ("Text Transform Position: " + text.transform.position.x + " " + text.transform.position.y + " " + text.transform.position.z);

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
