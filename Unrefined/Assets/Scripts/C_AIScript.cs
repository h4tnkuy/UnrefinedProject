using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class C_AIScript : MonoBehaviour{
    #region variables
    #region cached variables
    CharacterController _controller;
    Transform _transform;
    Transform player;
	Transform ObstacleObject;
	Transform _eyes;
    #endregion
    #region movement variables
    float speed = 3;
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
	int modeTimer1;
	public float angle;
	//*************************************************************************************************
	//デバッグ用に弄っても良い変数等;
	public int AtkType;										//敵がプレイヤーを発見した後の攻撃パターン;
	public static int Level = 0;							//現在のゲームレベル;
	string[] LevelName = {"Easy","Normal","Hard","Lunatic"};//ゲームレベルの表示用;
	int[] CreateCloneLevel = {1,2,4,8};						//ゲームレベルによる敵キャラ(クローン)の出現数;
	public static int[] probability = {30,50,75,90};		//ゲームレベルによる敵の攻撃の命中率;
	float TraceSpeed = 0.025f;								//敵が振り向く速度;
	//*************************************************************************************************
	int trriger;
	float timer;
	public static bool attackFlag;
	Transform safePlace;
	
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
        foreach (GameObject go in gos){
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
		attackFlag = false;
		//AtkType=3;
    }

    void Update(){
        if (AIFunction() && isCorouting){
            StopAllCoroutines();
            del = true;
        }
        if (del)delFunc();
        else if (!isCorouting){
            isCorouting = true;
            StartCoroutine(delEnum());
        }
		if(PlayerFunction()==true){//プレイヤーが敵を見ていればtrue(現状、壁貫通でも見れる);
			Debug.Log("Look");
		}else{
			Debug.Log("No Look");
		}
    }

    #region movement functions
	/**
	 * 指定したターゲットに向かって行く;
	 **/
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
	
	/**
	 * 指定したターゲットから離れる;
	 * (ターゲットの方向を向きながら);
	 **/
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
	
	/**
	 * 指定したターゲットから離れる;
	 * (向きはそのまま);
	 **/
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
	
	/**
	 * プレイヤーを見つけていない時;
	 * 徘徊AI;
	 **/
    void Walk(){
		transform.localScale = new Vector3(3, 5, 5);
        if ((_transform.position - waypoint[index].position).sqrMagnitude > range){
            Move(waypoint[index]);
            animation.CrossFade("walk");
			attackFlag = false;
        }else{
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
	
	/**
	 * 攻撃パターン分岐処理
	 **/
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
	
	/**
	 * 攻撃パターン1;
	 * 
	 * プレイヤーを見つけ次第、物陰に隠れに行き、;
	 * しゃがんでじっとしている←→攻撃;
	 * を繰り返す	;
	 * (しゃがみのモーションが無いので、無理やり縦の拡縮を弄っている。);
	 **/
	void Attack1(){
        if ((_transform.position - safePlace.position).sqrMagnitude > range){
            Move(ObstacleObject);
            animation.CrossFade("run");
			attackFlag = false;
        }else{
			//gameObject.transform.LookAt(player);
			this.transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(player.position - this.transform.position),
				TraceSpeed);
			//Debug.Log(modeTimer1);
			modeTimer1++;
			if(modeTimer1<=300){
				transform.localScale = new Vector3(5, 3, 5);
            	animation.CrossFade("idlebattle");
				attackFlag = false;
			}else if(modeTimer1<=600){
				transform.localScale = new Vector3(5, 5, 5);
            	animation.CrossFade("attackOwn");
				attackFlag = true;
			}else{
				modeTimer1=0;
			}
        }
    }
	
	/**
	 * 攻撃パターン2;
	 * 
	 * プレイヤーを見つけ次第、物陰に隠れに行き、;
	 * プレイヤーが此方(敵)を向いていなければ攻撃を繰り返し、;
	 * プレイヤーが此方(敵)を向いていれば;
	 * 物陰に隠れて様子を窺う;
	 * 少し外に出て攻撃する;
	 * の二つのパターンをランダムに繰り返す;
	 **/
	void Attack2(){
		if(PlayerFunction()==true){
			//Debug.Log(trriger);
			if(timer>=120){
				trriger=Random.Range(1,3);
				timer=0;
			}
	        if ((_transform.position - safePlace.position).sqrMagnitude > range&&trriger==1){
	            Move(safePlace);
	            animation.CrossFade("run");
				attackFlag = false;
	        }else{
				if(trriger==1){
					timer++;
					this.transform.rotation = Quaternion.Slerp(transform.rotation,
						Quaternion.LookRotation(player.position - this.transform.position),
						TraceSpeed);
					animation.CrossFade("idle");
					attackFlag = false;
				}else{
					if ((_transform.position - safePlace.position).sqrMagnitude < range*20){
						if(trriger==1){
							Move(safePlace);
	            			animation.CrossFade("run");
							attackFlag = false;
						}else{
							timer++;
							Leave2 (safePlace);
							this.transform.rotation = Quaternion.Slerp(transform.rotation,
								Quaternion.LookRotation(player.position - this.transform.position),
								TraceSpeed);
							animation.CrossFade("attackOwn");
							attackFlag = true;
						}
					}else{
						Move(safePlace);
	            		animation.CrossFade("run");
						attackFlag = false;
						trriger=1;
					}
				}
				
	        }
		}else{
			if ((_transform.position - safePlace.position).sqrMagnitude > range*20){
				Move(safePlace);
	            animation.CrossFade("run");
				attackFlag = false;
			}else{
				Leave2 (safePlace);
				this.transform.rotation = Quaternion.Slerp(transform.rotation,
					Quaternion.LookRotation(player.position - this.transform.position),
					TraceSpeed);
				animation.CrossFade("attackOwn");
				attackFlag = true;
			}
		}
    }
	
	/**
	 * 攻撃パターン3;
	 * 
	 * プレイヤーを見つけ次第、一定の上限まで距離を取ろうとし、(逃げながら)攻撃を繰り返す;
	 **/
	void Attack3(){
        if ((_transform.position - player.position).sqrMagnitude > range){
            Leave(player);
			if((_transform.position - player.position).sqrMagnitude < (100)){
            	animation.CrossFade("attackOwn");
				attackFlag = true;
			}else{
				animation.CrossFade("idlebattle");
				attackFlag = false;
			}
        }
        else{
            animation.CrossFade("idlebattle");
			attackFlag = false;
        }
    }
	
	/**
	 * 隠れる場所を探す
	 * 
	 * "Safe"タグがついているObjectを探す
	 **/
    void FindClosestSafe() {
	    GameObject[] gos;
	    gos = GameObject.FindGameObjectsWithTag("Safe");
	    GameObject closest=null;
	    float distance = Mathf.Infinity;
	    Vector3 position = transform.position;
	    foreach (GameObject go in gos) {
		    Vector3 diff = go.transform.position - position;
		    float curDistance = diff.sqrMagnitude;
		    if (curDistance < distance) {
			    closest = go;
			    distance = curDistance;
		    }
	    }
	    safePlace = closest.transform;
    }
	void TakeCover(){
	    /*if((_transform.position - safePlace.position).sqrMagnitude >range ){
		    Move(safePlace);
		    animation.CrossFade("run");
			attackFlag = false;
	    }else{*/
			Attack();
	    //}
    }

    #endregion
   
    #region animation functions
    IEnumerator Victory(){
        if (!animation.IsPlaying("victory")) animation.CrossFade("victory");
		attackFlag = false;
        yield return new WaitForSeconds(animation["victory"].length);
        NextIndex();
        del = true;
    }
    
    IEnumerator Wait(){
        animation.CrossFade("idle");
		attackFlag = false;
        yield return new WaitForSeconds(2.0f);
        NextIndex();
        del = true;
    }
	
	IEnumerator Walks(){
        animation.CrossFade("walk");
		attackFlag = false;
        yield return new WaitForSeconds(2.0f);
        NextIndex();
        del = true;
    }
    #endregion
    
    #region AI function
    bool AIFunction(){
		Vector3 direction = player.position - _transform.position;
        if (direction.sqrMagnitude < attackRange){
			FindClosestSafe();
            if (seenAround){
				if(safePlace!=null){
					Vector3 a = safePlace.position - _transform.position;
					Vector3 b = player.position - _transform.position;
					if(a.sqrMagnitude < b.sqrMagnitude){
						delFunc = this.TakeCover;
					}else{
						delFunc = this.Attack;
					}
				}else{
					delFunc = this.Attack;
				}
                return true;
            } else{
                if (Vector3.Dot(direction.normalized, _transform.forward) > 0 &&
                    !Physics.Linecast(_eyes.position, player.position, layerMask)){
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