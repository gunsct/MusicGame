using UnityEngine;
using System.Collections;

[RequireComponent(typeof (AudioClip))]
public class SoundManager : MonoBehaviour {
	static SoundManager instance = null;

	//음악,특수효과,유아이용
	public AudioClip[] music, effect, button;
	public AudioSource audio;

	//싱글톤에 씬전환시 삭제안되게
	public static SoundManager getInstance
	{
		get{
			if(instance == null){
				instance = new SoundManager ();
			}
			return instance;
		}
	}
	// Use this for initialization
	void Start () {
		audio = this.GetComponent<AudioSource> ();

		//클립들 크기 지정하고 그만큼 Resources 폴더에서 불러와 초기화해줌
		//effect = new AudioClip[4];
		//button = new AudioClip[4];
		music = new AudioClip[6];

		for (int i = 0; i < 6; i++) {
			/*if (i < 4) {
				effect [i] = new AudioClip ();
				button [i] = new AudioClip ();
			}*/

			music [i] = Resources.Load ("Sound/music"+(i+1)) as AudioClip;
		}
		audio.PlayOneShot (music [0], 0.8f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int m_data{ set; get; }
}
