using UnityEngine;
using System.Collections;

public class CMoveController : MonoBehaviour {
	
	private GameObject player;
	private Joystick joystick;
	
	// Use this for initialization
	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		joystick = this.gameObject.GetComponent<Joystick>();
	}
	
	// Update is called once per frame
	void Update() {
		player.SendMessage("Move", joystick.position);
	}
}
