using UnityEngine;
using System.Collections;

public class CSinglePlayer : MonoBehaviour {
	
	private Rect rect;
	
	// Use this for initialization
	void Start () {
		rect = new Rect(100, 180, 360, 50);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		
		if (GUI.Button(rect, "Single Player")) {
			Application.LoadLevel("Menu");
		}
	}
}
