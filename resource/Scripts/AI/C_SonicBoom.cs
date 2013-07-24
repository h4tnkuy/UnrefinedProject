using UnityEngine;
using System.Collections;

public class C_SonicBoom : MonoBehaviour {
	public GameObject shellPrefab;
	float power = 40.0f;
	int a;
	float timer;
	GameObject shell;
	// Use this for initialization
	void Start () {
		timer=0;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(C_AIScript.attackFlag==true){
			timer+=Time.deltaTime;
			if(timer>=animation["attackOwn"].length){
				shell = (GameObject)Instantiate(shellPrefab, transform.position, Quaternion.identity);
				shell.transform.position += new Vector3(0.0f, 1.3f, 0.0f);
				shell.transform.rotation = this.transform.rotation;
				shell.rigidbody.velocity = transform.forward * (power);
				timer=0;
			}
		}else{
			timer=0;
		}
		
	}
}
