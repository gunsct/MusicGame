using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {
	private GameObject ControlBox;
	public Material[] materials = new Material[6];
	private MemoryPool pool;
	GameManager gameManager;
	// Use this for initialization

	void Start () {
		this.GetComponent<Renderer>().material = materials[Random.Range (0, 6)];
		ControlBox = GameObject.Find ("ControlBox");

		pool = ControlBox.GetComponent<ControlBox> ().pool;
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	//센터로 이동
	public void StartMove(GameObject _obj){
		this.transform.LookAt (_obj.transform.position);
		this.GetComponent<Rigidbody> ().velocity = this.transform.forward * _obj.GetComponent<ControlBox> ().nodeSpd;
	}

	//충돌 후 메모리풀로 제거 관리
	public void RemoveNode(){
		this.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
		for (int i = 0; i < ControlBox.GetComponent<ControlBox> ().poolCnt; i++) {
			if (ControlBox.GetComponent<ControlBox> ().Node [i] == this.gameObject)
				ControlBox.GetComponent<ControlBox> ().Node [i] = null;
		}
		pool.RemoveItem (this.gameObject);
	}
		
	void OnTriggerEnter(Collider _col){
		if (_col.transform.tag.Equals ("PlayerHitBox")) {
			//점수부분은 따로 처리
			RemoveNode ();
		}

		if (_col.transform.tag.Equals ("PenaltyHitBox")) {
			//패널티도 따로있는곳에서 씀
			gameManager.ScoreCheck (0.3f);
			RemoveNode ();
		}
	}
}
