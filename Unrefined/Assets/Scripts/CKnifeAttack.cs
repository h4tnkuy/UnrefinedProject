using UnityEngine;
using System.Collections;

public class CKnifeAttack : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Destroy(this.gameObject,0.1f);
	}
	
	void OnTriggerEnter(Collider coll){
		if(coll.gameObject != null){
			coll.gameObject.GetComponent<CPlayersHitpoint>().playerHp 
				= 0;
		}
	}
}
