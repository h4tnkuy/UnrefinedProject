using UnityEngine;
using System.Collections;

public class CPlayersHitpoint : MonoBehaviour {
	
	public float playerHp;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(playerHp <= 0){
			Destroy(this.gameObject);
		}
	}
	
	public void hpCounter(float damage){
		playerHp -= damage;
	}
}
