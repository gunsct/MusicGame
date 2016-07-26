using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour {
	static GameSceneManager instance = null;
	static GameObject container = null;

	public string sceneName = "";

	//싱글톤에 씬전환시 삭제안되게, 씬,인포매니져,사운드매니져 3개 붙임
	public static GameSceneManager getInstance
	{
		get{
			if(instance == null){
				container = new GameObject ();
				container.name = "SceneManager";
				instance = container.AddComponent (typeof(GameSceneManager)) as GameSceneManager;
				container.AddComponent (typeof(InfoManager));
				container.AddComponent (typeof(SoundManager));
			}
			return instance;
		}
	}

	public void Create(){
		DontDestroyOnLoad (transform.gameObject); 
	}

	public void SceneChange(string _scenename){
		sceneName = _scenename;
		SceneManager.LoadScene ("Loading");
	}

	public int m_data{ set; get; }
}
