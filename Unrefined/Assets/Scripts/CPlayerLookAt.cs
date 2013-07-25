using UnityEngine;
using System.Collections;

public class CPlayerLookAt : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.LookAt(GameObject.Find("Player").transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
