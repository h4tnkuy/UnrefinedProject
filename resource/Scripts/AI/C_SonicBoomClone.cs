using UnityEngine;
using System.Collections;

public class C_SonicBoomClone : MonoBehaviour {
	int a;
	int timer;
	// Use this for initialization
	void Start () {
		timer=0;
	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		if(timer>160){
			Destroy(this.gameObject);
		}
	}
	
	void OnTriggerEnter(Collider  collision){
		if(collision.gameObject.name == "Player"){
			a=Random.Range(1,100);
			if(C_AIScript.probability[C_AIScript.Level]>=a){
				Debug.Log("HIT!");
			}else{
				Debug.Log("MISS!");
			}
		}
	}
}
