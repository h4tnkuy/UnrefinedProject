using UnityEngine;
using System.Collections;

public class C_SwordScript : MonoBehaviour {
	
	Transform player;
	Transform _transform;
	float sqrRange = 3;
	int a;
	
	void Start(){
		player = GameObject.Find ("Player").GetComponent<Transform>();
        if (player == null)
            Debug.LogError("No player on scene");
		_transform = GetComponent<Transform>();
	}
	
    void CollisionSword()
    {
        Vector3 direction = player.position - _transform.position;
		if (direction.sqrMagnitude < sqrRange){
			if(Vector3.Dot (direction.normalized,_transform.forward) > 0){
				//Add here your logic for decreasing player health
				a=Random.Range(1,100);
				if(C_AIScript.probability[C_AIScript.Level]>=a){
					Debug.Log("HIT!");
				}else{
					Debug.Log("MISS!");
				}
				//print ("Hit");
			}
		}
    }
	void OnGUI(){
		GUI.Label(new Rect(0, 20, 200, 100), "a="+a);
	}
}
