using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	//게임중 스코어같은 실시간으로 얻어야하는 정보들
	private int perfect, greate, good, bad ,miss;
	private int combo, totalHit, score;
	// Use this for initialization
	void Start () {
		perfect = greate = good = bad = miss = combo = totalHit = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//충돌체로부터 호출되서 단계별 점수 누적 및 콤보, 미스 체크
	public void ScoreCheck(float _time){
		if (_time <= 0.05f) {
			perfect++;
			totalHit++;
			combo++;
		}
		if (0.05f < _time && _time <= 0.1f) {
			greate++;
			totalHit++;
			combo++;
		}
		if (0.1f < _time && _time <= 0.15f) {
			good++;
			totalHit++;
			combo++;
		}
		if (0.15f < _time && _time <= 0.2f) {
			bad++;
			totalHit++;
			combo++;
		}
		if (0.25f < _time) {
			miss++;
			combo = 0;
		}
		Debug.Log (perfect +" "+ greate +" "+ good +" "+ bad +" "+ miss +" "+ combo +" "+ totalHit);
	}
}
