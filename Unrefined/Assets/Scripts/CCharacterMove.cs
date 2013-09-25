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
	
	public GameObject camera;
	
	private Vector3 angle;
	
	float speed;
	
	// Use this for initialization
	void Start () {
		speed = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		speed = Time.deltaTime * MoveSpeed;
		
		angle = camera.transform.TransformDirection(moveVec);
		
		this.transform.Translate(angle * speed);
	}
	
	//GUIから取得 
	public void Move(Vector2 vec){
		moveVec.x = vec.x;
		moveVec.z = vec.y;
	}
}
