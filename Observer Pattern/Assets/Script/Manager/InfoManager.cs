using UnityEngine;
using System.Collections;

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
