using UnityEngine;
using System.Collections;

public class CCameraController : MonoBehaviour {
	
	private GameObject camera;
	private Joystick joystick;
	
	// Use this for initialization
	void Start () {
		camera = GameObject.FindGameObjectWithTag("Camera");
		joystick = this.gameObject.GetComponent<Joystick>();
	}
	
	// Update is called once per frame
	void Update () {
	
		camera.SendMessage("setAngle", joystick.position);
	}
}
