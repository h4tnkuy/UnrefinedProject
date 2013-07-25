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
	
	//ほかのオブジェクトに適用したスクリプトの読み込み 
	private GameObject cam;
	
	private Vector3 GB;
	private Vector3 LR;
	private CCameraAngle ca;
	
	// Use this for initialization
	void Start () {
		
		getMoveX = 0.0f;
		getMoveZ = 0.0f;
		getR = 0.0f;
		
		cam = GameObject.Find("Main Camera");
		ca = cam.GetComponent<CCameraAngle>();
	}
	//GUI
	void OnGUI(){
		
	}
	
	// Update is called once per frame
	void Update () {
		//仮想アナログパッドから値を持ってくる 
		
		GB = Vector3.forward * getMoveX;
		LR = Vector3.right * getMoveZ;
		transform.Translate(GB);
		transform.Translate(LR);
		transform.rotation = Quaternion.Euler(0f, ca.getFunctionRotate(), 0f);
	}
	
	//GUIから取得 
	public void setGBLR(float gb, float lr){
		getMoveX = gb;
		getMoveZ = lr;
	}
}
