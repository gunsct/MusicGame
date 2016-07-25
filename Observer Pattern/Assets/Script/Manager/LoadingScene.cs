using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine ("Load");
	}

	IEnumerator Load(){
		AsyncOperation async = SceneManager.LoadSceneAsync(GameSceneManager.getInstance.sceneName);

		while (!async.isDone) {
			//프로그래스바 및 텍스트
			float progress = async.progress * 100.0f;

		}

		yield return true;
	}
}
