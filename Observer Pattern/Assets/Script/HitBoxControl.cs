using UnityEngine;
using System.Collections;

public class HitBoxControl : MonoBehaviour {
	public GameObject[] hitBox = new GameObject[6];

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 6; i++) {
			hitBox [i].SetActive (false);
		}
	}

	//버튼 관리
	public void OnLU(){hitBox[0].SetActive(true);}
	public void OffLU(){hitBox[0].SetActive(false);}

	public void OnLM(){hitBox[1].SetActive(true);}
	public void OffLM(){hitBox[1].SetActive(false);}

	public void OnLD(){hitBox[2].SetActive(true);}
	public void OffLD(){hitBox[2].SetActive(false);}

	public void OnRU(){hitBox[3].SetActive(true);}
	public void OffRU(){hitBox[3].SetActive(false);}

	public void OnRM(){hitBox[4].SetActive(true);}
	public void OffRM(){hitBox[4].SetActive(false);}

	public void OnRD(){hitBox[5].SetActive(true);}
	public void OffRD(){hitBox[5].SetActive(false);}
}
