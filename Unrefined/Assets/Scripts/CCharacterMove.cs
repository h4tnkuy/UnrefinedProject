using UnityEngine;
using System.Collections;
/*
 * 
 * 左アナログスティックで移動 
 * 
 *  */
public class CCharacterMove : MonoBehaviour {

	
	//取得する情報 
	private float getMoveX;
	private float getMoveZ;
	private float getR;
	
	//回転量 
	private float rotateQuantity;
	
	//ほかのオブジェクトに適用したスクリプトの読み込み 
	private GameObject plane;
	private GameObject cam;
	
	private Vector3 GB;
	private Vector3 LR;
	private CCameraAngle ca;
	
	// Use this for initialization
	void Start () {
		
		getMoveX = 0.0f;
		getMoveZ = 0.0f;
		getR = 0.0f;
		
		rotateQuantity = 0f;
		
		plane = GameObject.Find("Plane");
		cam = GameObject.Find("Main Camera");
		
		ca = cam.GetComponent<CCameraAngle>();
	}
	//GUI
	void OnGUI(){
		/**********************************************************************/
		//test
		GUILayout.Label("" + Input.mousePosition.x);
		GUILayout.Label("" + Input.mousePosition.y);
		GUILayout.Label("" + rotateQuantity);
		/**********************************************************************/
		
	}
	
	// Update is called once per frame
	void Update () {
		//カメラ移動からY軸の角度を持ってくる 
		rotateQuantity = ca.getFunctionRotate();
		GB = Vector3.forward * getMoveX;
		LR = Vector3.right * getMoveZ;
		transform.Translate(GB);
		transform.Translate(LR);
		transform.rotation = Quaternion.Euler(0f, rotateQuantity, 0f);
	}
	
	//GUIから取得 
	/*public void setGBLR(float gb, float lr, float rot){
		getMoveX = gb;
		getMoveZ = lr;
		getR = rot;
	}*/
}
