using UnityEngine;
using System.Collections;
/*
 * 
 * 左アナログスティックで移動 
 * 
 *  */
public class CCharacterMove : MonoBehaviour {
	
	public float MoveSpeed = 1f;
	
	private Vector3 moveVec;
	
	private GameObject camera;
	
	// Use this for initialization
	void Start () {
		camera = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
		float speed = Time.deltaTime * MoveSpeed;
		
		Vector3 angle = camera.transform.TransformDirection(moveVec);
		
		this.transform.Translate(angle * speed);
	}
	
	//GUIから取得 
	public void Move(Vector2 vec){
		moveVec.x = vec.x;
		moveVec.z = vec.y;
	}
}
