using UnityEngine;
using System.Collections;

public class CTouchShot : MonoBehaviour {
	
	//変数の宣言 
	//発射レートの管理用 
	//テストではintだけど本番では武器変更用のスクリプトから読み込む 
	private int gunNumber;
	//銃の発射レートの管理 
	private int fireRate;
	//銃の発射レート用のタイマー 
	private int fireRateTimer;
	
	//タッチした座標から透明な直線を出す 
	//スクリーンからワールド座標に変換した値を入れる 
	private Ray ray;
	//まっすぐに伸びる透明な直線 
	private RaycastHit hit;
	//hitが当たったオブジェクトを入れる 
	private GameObject hitObject;
	
	//弾痕の処理
	//Resourcesからプレハブを読み込み格納するオブジェクト 
	public GameObject bulletMark1;
	public GameObject bulletMark2;
	
	private int gunAmmoNum;
	
	private bool reLoadFlag;
	
	private int changeNum;
	
	private Vector3 trance;
	
	//ノックアップ 
	private int nockUp;
	
	public GameObject gunObject;
	
	
	// Use this for initialization
	void Start () {
		
		gunNumber = 1;
		fireRate = 0;
		fireRateTimer = 0;
		
		gunAmmoNum = 0;
		reLoadFlag = true;
		
		changeNum = 4;
		
		trance = GameObject.Find ("gunPos").transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetMouseButton(0)){
			/*if(gunAmmoNum < 0){
			//発射レートの管理 
				if(fireRateTimer == 0 | fireRateTimer % fireRate == 0){*/
					//タッチした座標からビームを飛ばす 
					ray = camera.ScreenPointToRay(Input.mousePosition);
					if(Physics.Raycast(ray, out hit, 100f)){
						//ビームが当たったところの情報を取得 
						hitObject = hit.collider.gameObject;
						if(hitObject.gameObject.tag == "Player"){
							vsEnemyObject();
						}
						if(hitObject.gameObject.tag == "Obstacle"){
							Instantiate(bulletMark1, hit.point, Quaternion.identity);
						}
						if(hitObject.gameObject.tag == "Floor"){
							Instantiate(bulletMark2, hit.point, Quaternion.identity);
						}
					}
					if(reLoadFlag){
						reLoadFlag = false;
					}
					gunAmmoNum --;
				}
				fireRateTimer ++;
			/*}
			
		}*/
		if(Input.GetMouseButtonUp(0)){
			fireRateTimer = 0;
		}
		
	}
	
	//対人戦の時の処理 
	//相手に当たった時にHPを減らす処理をする（テストではGetComponentを使いスケールをいじる） 
	//引数、戻り値はなし 
	void vsEnemyObject(){
		hitObject.transform.localScale = hitObject.transform.localScale *= 1.01f;
	}

	//GUIから武器の情報をもらってくる 
	public void ChangeWeapon(int gunNum){
		//武器の切り替え 
		//武器モデルの変更 
		switch(gunNum){
		case 0:
			if(changeNum != gunNum){
				reLoadFlag = true;
				if(GameObject.Find("gun") != null){
					Destroy(GameObject.Find("gun"));
				}
				Instantiate (gunObject
					, trance, Quaternion.identity);
				GameObject.Find("fast(Clone)").name = "gun";
				changeNum = gunNum;
			}
			break;
		}
		
	}
	
	//武器から情報を取得する 
	//威力 
	public void ammoPower(float power){
		
	}
	//命中率 
	public void hitProbability(float hitP){
		
	}
	//反動 
	public void recoli(float reco){
		
	}
	//発射レート 
	public void shotRate(int sr){
		fireRate = sr;
	}
	//重さ 
	public void weight(float weight){
		
	}
	//リロード 
	public void reload(bool re){
		if(re){
			reLoadFlag = true;
		}
	}
	//装弾数 
	public void ammoCapacity(int ac){
		if(reLoadFlag){
			gunAmmoNum = ac;
		}
	}
	
	//テスト 
	void OnGUI(){
		//GUI.Label(new Rect(0f,150f,100f,100f), "Ammo" + gunAmmoNum);
	}
}
