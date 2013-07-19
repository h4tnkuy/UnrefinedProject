using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class C_AIScript : MonoBehaviour
{
    #region variables
    #region cached variables
    CharacterController _controller;
    Transform _transform;
    Transform player;
	Transform ObstacleObject;
	Transform _eyes;
    #endregion
    #region movement variables
    float speed = 2;
    float gravity = 100;
    Vector3 moveDirection;
    float maxRotSpeed = 200.0f;
    float minTime = 0.1f;
    float velocity;
    float range;
    float attackRange;
    bool isCorouting;
    #endregion
    #region waypoint variables
    int index;
    public string strTag;
    Dictionary<int, Transform> waypoint = new Dictionary<int, Transform>();
    #endregion
    #region delegate variable
    delegate void DelFunc();
    delegate IEnumerator DelEnum();
    DelFunc delFunc;
    DelEnum delEnum;
    bool del;
    #endregion
    #endregion
    bool seenAround;
    int layerMask = 1 << 8;
	
	int a;
	public int AtkType;
	int modeTimer1;
	public float angle;
	float radius = 15.0f;
	//******************************************************
	public static int Level = 0;
	//******************************************************
	string[] LevelName = {"Easy","Normal","Hard","Lunatic"};
	int[] CreateCloneLevel = {1,2,4,8};
	public static int[] probability = {30,50,75,90};
	float y1,y2;
	float TraceSpeed = 0.025f;
	int trriger;
	bool aaa;
	float timer;
	
    void Start(){
        _controller = GetComponent<CharacterController>();
        _transform = GetComponent<Transform>();
		_eyes = transform.Find ("Eyes");
        player = GameObject.Find("Player").GetComponent<Transform>();
		ObstacleObject = GameObject.Find("ObstacleObject").GetComponent<Transform>();
        if (player == null)
            Debug.LogError("No player on scene");
        if (string.IsNullOrEmpty(strTag)) 
            Debug.LogError("No waypoint tag given");
        
        index = 0;
        range = 2.5f; attackRange = 200f;
        
        GameObject[] gos = GameObject.FindGameObjectsWithTag(strTag);
        foreach (GameObject go in gos)
        {
            C_WaypointScript script = go.GetComponent<C_WaypointScript>();
            waypoint.Add(script.index, go.transform);
        }
        animation["victory"].wrapMode = WrapMode.Once;
        
        delFunc = this.Walk;
        delEnum = null;
        del = true;
        
        isCorouting = false;
        seenAround = false;
        layerMask = ~layerMask;
		aaa=false;
		//AtkType=3;
    }

    void Update(){
        if (AIFunction() && isCorouting)
        {
            StopAllCoroutines();
            del = true;
        }
        if (del)
            delFunc();
        else if (!isCorouting)
        {
            isCorouting = true;
            StartCoroutine(delEnum());
        }
		if(PlayerFunction()==true){
			Debug.Log("Look");
		}else{
			Debug.Log("No Look");
		}
    }

    #region movement functions
    void Move(Transform target){
        //Movements
        moveDirection = _transform.forward;
        moveDirection *= speed;
        moveDirection.y -= gravity * Time.deltaTime;
        _controller.Move(moveDirection * Time.deltaTime);
        //Rotation
        var newRotation = Quaternion.LookRotation(target.position - _transform.position).eulerAngles;
        var angles = _transform.rotation.eulerAngles;
        _transform.rotation = Quaternion.Euler(angles.x,
            Mathf.SmoothDampAngle(angles.y, newRotation.y, ref velocity, minTime, maxRotSpeed), angles.z);
    }
	
	void Hide(Transform target){
        //Movements
        moveDirection = _transform.forward;
        moveDirection *= speed;
        moveDirection.y -= gravity * Time.deltaTime;
        _controller.Move(moveDirection * Time.deltaTime);
        //Rotation
        var newRotation = Quaternion.LookRotation(target.position - _transform.position).eulerAngles;
        var angles = _transform.rotation.eulerAngles;
        _transform.rotation = Quaternion.Euler(angles.x,
            Mathf.SmoothDampAngle(angles.y, newRotation.y, ref velocity, minTime, maxRotSpeed), angles.z);
    }
	
	void Leave(Transform target){
        //Movements
		moveDirection = _transform.forward;
		if((_transform.position - player.position).sqrMagnitude < (100)){
	        moveDirection *= speed;
		}else{
			moveDirection *= 0.0f;
		}
		moveDirection.x*=-1;
        moveDirection.y -= gravity * Time.deltaTime;
		moveDirection.z*=-1;
        _controller.Move(moveDirection * Time.deltaTime);
        //Rotation
        var newRotation = Quaternion.LookRotation(target.position - _transform.position).eulerAngles;
        var angles = _transform.rotation.eulerAngles;
        _transform.rotation = Quaternion.Euler(angles.x,
            Mathf.SmoothDampAngle(angles.y, newRotation.y, ref velocity, minTime, maxRotSpeed), angles.z);
    }
	
	void Leave2(Transform target){
        //Movements
		moveDirection = _transform.forward;
		if((_transform.position - player.position).sqrMagnitude < (100)){
	        moveDirection *= speed;
		}else{
			moveDirection *= 0.0f;
		}
		moveDirection.x*=-1;
        moveDirection.y -= gravity * Time.deltaTime;
		moveDirection.z*=-1;
        _controller.Move(moveDirection * Time.deltaTime);

    }

    void NextIndex(){
        if (++index == waypoint.Count) index = 0;
    }

    void Walk(){
		transform.localScale = new Vector3(5, 5, 5);
        if ((_transform.position - waypoint[index].position).sqrMagnitude > range)
        {
            Move(waypoint[index]);
            animation.CrossFade("walk");
        }
        else
        {
            switch (index)
            {
                case 0:
                    del = false;
                    isCorouting = false;
                    delEnum = this.Victory;
                    break;
                case 1:
                    del = false;
                    isCorouting = false;
                    delEnum = this.Wait;
                    break;
                default:
                    NextIndex(); break;
            }
        }
    }
	
	void Attack(){
        switch(AtkType){
		case 0:
			Debug.Log("You did not specify the type of attack.");
			Walk();
			break;
		case 1:
			Attack1();
			break;
		case 2:
			Attack2();
			break;
		case 3:
			Attack3();
			break;
		default :
			Debug.Log("You did not specify the type of attack.");
			Walk ();
			break;
		}
    }
	
	void Attack1(){
        if ((_transform.position - ObstacleObject.position).sqrMagnitude > range)
        {
            Hide(ObstacleObject);
            animation.CrossFade("run");
        }
        else
        {
			//gameObject.transform.LookAt(player);
			this.transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(player.position - this.transform.position),
				TraceSpeed);
			//Debug.Log(modeTimer1);
			modeTimer1++;
			if(modeTimer1<=300){
				transform.localScale = new Vector3(5, 3, 5);
            	animation.CrossFade("idlebattle");
			}else if(modeTimer1<=600){
				transform.localScale = new Vector3(5, 5, 5);
            	animation.CrossFade("attackOwn");
			}else{
				modeTimer1=0;
			}
        }
    }
	
	void Attack2(){
		if(PlayerFunction()==true){
			timer++;
			//Debug.Log(trriger);
			if(timer>=120){
				trriger=Random.Range(1,3);
				timer=0;
			}
	        if ((_transform.position - ObstacleObject.position).sqrMagnitude > range&&trriger==1){
	            Move(ObstacleObject);
	            animation.CrossFade("run");
	        }else{
				if(trriger==1){
					this.transform.rotation = Quaternion.Slerp(transform.rotation,
						Quaternion.LookRotation(player.position - this.transform.position),
						TraceSpeed);
					animation.CrossFade("idle");
				}else{
					if ((_transform.position - ObstacleObject.position).sqrMagnitude < range*20){
						Leave2 (ObstacleObject);
						this.transform.rotation = Quaternion.Slerp(transform.rotation,
							Quaternion.LookRotation(player.position - this.transform.position),
							TraceSpeed);
						animation.CrossFade("attackOwn");
					}else{
						animation.CrossFade("idle");
					}
				}
				
	        }
		}else{
			if ((_transform.position - ObstacleObject.position).sqrMagnitude > range*20){
				Move(ObstacleObject);
	            animation.CrossFade("run");
			}else{
				Leave2 (ObstacleObject);
				this.transform.rotation = Quaternion.Slerp(transform.rotation,
					Quaternion.LookRotation(player.position - this.transform.position),
					TraceSpeed);
				animation.CrossFade("attackOwn");
			}
		}
    }
	
	void Attack3(){
        if ((_transform.position - player.position).sqrMagnitude > range)
        {
            Leave(player);
			if((_transform.position - player.position).sqrMagnitude < (100)){
            	animation.CrossFade("attackOwn");
			}else{
				animation.CrossFade("idlebattle");
			}
        }
        else
        {
            animation.CrossFade("idlebattle");
        }
    }

    #endregion
   
    #region animation functions
    IEnumerator Victory(){
        if (!animation.IsPlaying("victory")) animation.CrossFade("victory");
        yield return new WaitForSeconds(animation["victory"].length);
        NextIndex();
        del = true;
    }
    
    IEnumerator Wait(){
        animation.CrossFade("idle");
        yield return new WaitForSeconds(2.0f);
        NextIndex();
        del = true;
    }
	
	IEnumerator Walks(){
        animation.CrossFade("walk");
        yield return new WaitForSeconds(2.0f);
        NextIndex();
        del = true;
    }
    #endregion
    
    #region AI function
    bool AIFunction(){
		Vector3 direction = player.position - _transform.position;
        Vector3 direction2 = ObstacleObject.position - _transform.position;
        if (direction.sqrMagnitude < attackRange){
            if (seenAround){
				//if(AtkType==1){
						delFunc = this.Attack;
				//	}else if(AtkType==2){
				//		delFunc = this.Attack2;
				//	}
                return true;
            } else{
                if (Vector3.Dot(direction.normalized, _transform.forward) > 0 &&
                    !Physics.Linecast(_eyes.position, player.position, layerMask)){
	            /*        if(a==1){
							Debug.Log("1:A"); 
							AtkType=1;
						}else if(a==2){
							Debug.Log("2:B");
							AtkType=2;
						}else{
							Debug.Log("E:C");
						}
						*/
						delFunc = this.Attack;
	                    seenAround = true;
	                    return true;
                }
                return false;
            }
		
        }else{
            delFunc = this.Walk;
            seenAround = false;
            return false;
        }
    }
    #endregion
	
	bool PlayerFunction(){
		Vector3 v = _transform.position - player.position;
	    if(
			Vector3.Angle(player.transform.forward,player.transform.position - _transform.position) <= 135){
		    return false;
	    }else{
		    return true;
	    }
    }
	
	void OnGUI(){
		GUI.Label(new Rect(0, 0, 200, 100), "Level="+LevelName[Level]);
	}
	
}