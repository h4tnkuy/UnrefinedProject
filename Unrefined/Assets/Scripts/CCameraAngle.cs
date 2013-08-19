using UnityEngine;
using System.Collections;

public class CCameraAngle : MonoBehaviour {
	//Y軸方向の回転 
	private float angleY;
	//Y軸方向の回転 
	private float angleX;
	
	
	// Use this for initialization
	void Start () {
		angleX = 0f;
		angleY = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		//transform.rotation = Quaternion.Euler(angleX, angleY, 0f);
	}
	public float getFunctionRotate(){
		return angleY;
	}
	
	//GUIから取得 
	public void setAngle(Vector2 viewVec){
		angleX += viewVec.y;
		angleY += viewVec.x;
	}
}
