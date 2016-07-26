using UnityEngine;
using System.Collections;

public class HitBoxControl : MonoBehaviour {
	public GameObject[] hitBox = new GameObject[6];
	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 6; i++) {
			hitBox [i].SetActive (false);
		}
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
	}

	//버튼 관리
	public void OnLU(){On(0);}
	public void OffLU(){Off (0);}

	public void OnLM(){On (1);}
	public void OffLM(){Off (1);}

	public void OnLD(){On (2);}
	public void OffLD(){Off (2);}

	public void OnRU(){On (3);}
	public void OffRU(){Off (3);}

	public void OnRM(){On (4);}
	public void OffRM(){Off (4);}

	public void OnRD(){On (5);}
	public void OffRD(){Off (5);}

	void On(int _num){//버튼눌리면 충돌체 킴
		hitBox[_num].SetActive(true);
	}
		
	void Off(int _num){//버튼때면 충돌체 끔 근데 아무 충돌없이 꺼지면 미스판정
		if (hitBox [_num].GetComponent<ScoreCheck> ().bHit == false)
			hitBox [_num].GetComponent<ScoreCheck> ().gameManager.ScoreCheck (0.3f);

		hitBox[_num].SetActive(false);
	}
}
