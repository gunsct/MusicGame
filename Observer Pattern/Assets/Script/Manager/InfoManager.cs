using UnityEngine;
using System.Collections;

//기기내에 기록남기는곳이니 이전에 했던 최고기록 볼 수 있을듯
//딕셔너리나 기타 자료구조로 노래별로 저장할 수 도 있고 데이터베이스로 넘길 수도
public class InfoManager : MonoBehaviour {
	static InfoManager instance = null;

	//싱글톤에 씬전환시 삭제안되게
	public static InfoManager getInstance
	{
		get{
			if(instance == null){
				instance = new InfoManager ();
			}
			return instance;
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int m_data{ set; get; }
}
