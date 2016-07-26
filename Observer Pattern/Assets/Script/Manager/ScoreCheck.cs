using UnityEngine;
using System.Collections;

//버튼 눌렀을때 노드 판정 시간 체크해줌
public class ScoreCheck : MonoBehaviour {
	public float time;
	public GameManager gameManager;
	public bool bHit = false;//버튼 누른상태에서 노드 판정 여부 판단
	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
		time = 0.0f;
		bHit = false;
	}
	
	// Update is called once per frame
	void Update () {//버튼누르면 히트박스 액티브 트루되며 프레임 돔
		time += Time.deltaTime;

		if (time > 0.25f) {//노드 충돌없이 0.25이상이면 미스판정 낸 후 충돌체 꺼버림
			gameManager.ScoreCheck (time);
			time = 0.0f;
			this.gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter(Collider _col){
		if (_col.tag.Equals ("Node")) {//노드에 박으면 충돌까지 걸린 시간 체크해서 전달 후 충돌체 끔 
			gameManager.ScoreCheck (time);
			time = 0.0f;
			bHit = true;
			StartCoroutine ("Off");
		}
	}
	IEnumerator Off(){//겹치는 노드까지 처리
		yield return new WaitForSeconds (0.1f);
		bHit = false;
		this.gameObject.SetActive (false);
	}
}
