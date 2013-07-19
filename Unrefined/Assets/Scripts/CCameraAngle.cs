using UnityEngine;
using System.Collections;

public class CCameraAngle : MonoBehaviour {
	
	/************************************************/
	//Y軸方向の回転 
	private float angleY;
	//Y軸方向の回転 
	private float angleX;
	
	private float r;
	private bool tr;
	/************************************************/
	
	
	// Use this for initialization
	void Start () {
	/************************************************/
		angleX = 0f;
		angleY = 0f;
		r = 0.1f;
		tr = true;
	/************************************************/
	}
	
	// Update is called once per frame
	void Update () {
	/************************************************/
		//angleY += 0.5f;
		/*
		if(angleX > 30f && tr){
			r = -0.1f;
			tr = false;
		}
		if(angleX < -30f && !tr){
			r = 0.1f;
			tr = true;
		}*/
		//angleX += r;
		if(angleY > 360f){
			angleY = 0;
		}
	/************************************************/
		transform.rotation = Quaternion.Euler(angleX, angleY, 0f);
	}
	public float getFunctionRotate(){
		return angleY;
	}
	//GUIから取得 
	/*public void setAngle(float x, float y){
		angleX = y;
		angleY = x;
	}*/
}
