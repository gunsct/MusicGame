using UnityEngine;
using System.Collections;

[RequireComponent(typeof (AudioClip))]
public class ControlBox : MonoBehaviour {
	public int numOfSamples = 1024; //Min: 64, Max: 8192

	//사운드 스펙트럼 정보 변수들
	public float[] freqData;//전체 스펙트럼
	public float[] band;//특정(일정) 스펙트럼

	//노래 평균 스펙트럼
	public float avgFreqData;
	public float FreqData3;

	//노트 생성하는 오브젝트 
	public GameObject[] spawnerModel = new GameObject[6];
	private GameObject[] spawner;
	private int core = 3;
	private float radius = 10.0f;
	private float angle = 60.0f;
	private float angleSpd;
	public float fps;//프레임단위로 스펙트럼 뽑아내는거 관리

	private bool bChange = false;//프레임 전환 제어

	//메모리풀과 노드
	public MemoryPool pool = new MemoryPool();
	public int poolCnt;
	public GameObject[] Node;
	public GameObject nodeObj;
	public float nodeSpd;

	void Start()
	{
		//사운드 스펙트럼을 일정 량으로 구분 나눔
		angleSpd = 1.0f / fps;
		freqData = new float[numOfSamples];

		int fdl = freqData.Length;

		int k = 0;
		for (int j = 0; j < fdl; j++)
		{
			fdl = fdl/ core;
			if (fdl <= 0) break;
			k++;
		}

		band = new float[k + 1];
		spawner = new GameObject[k + 1];

		//개수에 맞춰 노드 스포너를 만들어주고
		CreateSpawner ();

		//메모리풀을 만들어 준 뒤 
		pool.CreatePool (nodeObj, poolCnt, new Vector3(0,-20.0f,0));
		Node = new GameObject[poolCnt];

		for (int i = 0; i < Node.Length; i++) {
			Node [i] = null;
		}

		//초기 평균, 전체 스펙트럼 과 회전등 작업 시작 
		StartCoroutine ("StartAvgFreqData");
		StartCoroutine ("ChangeAngle");
		InvokeRepeating("check", 0.0f, angleSpd); // update at 15 fps
	}

	void Update(){
		this. transform.Rotate(new Vector3(0,60.0f / angleSpd,0) * Time.deltaTime, Space.World);
	}

	//스펙트럼 데이터를 뽑아 담아줌
	private void check()
	{
		//현재 틀고있는 사운드 스펙트럼 추출
		AudioListener.GetSpectrumData(freqData, 0, FFTWindow.Rectangular);

		int k = 0;
		int crossover = core;

		//배열에 쭉 담아줌
		for (int i = 0; i < freqData.Length; i++)
		{
			float frequency = freqData[i];
			float bandwidth = band[k];

			// find the max as the peak value in that frequency band.
			band[k] = (frequency > bandwidth) ? frequency : bandwidth;

			//대역폭 할당 후 이펙트 넣어주고 노드 생성 및 초기화
			if (i > (crossover - (Mathf.Pow(core,2)-1)))
			{
				k++;
				crossover *= core;   // frequency crossover point for each band.
				Vector3 tmp = new Vector3(spawner[k].transform.position.x, band[k] * 64, spawner[k].transform.position.z);
				spawner[k].transform.position = tmp;

				for(int j = 0; j<Node.Length; j++){
					if (Node [j] == null) {
						Node[j] = SpawnCycle (k, bandwidth, tmp);
						if (Node [j] != null) {
							Node [j].GetComponent<Node> ().StartMove (this.gameObject);
							break;
						}
					}
				}
				
				//대역폭에 따라 노드 생성 관리
				FreqData3 = band [3];
				if (k == 2 && bChange == true) {
					SoundStep (bandwidth);
				}
				band[k] = 0;
			}
		}
	}

	//노드 스포너 생성 60도 단위씩 회전
	private void CreateSpawner(){
		for (int i = 0; i < band.Length; i++)
		{
			band[i] = 0;
			spawner [i] = Instantiate(spawnerModel [i],this.transform.position,Quaternion.identity) as GameObject;
			spawner [i].transform.parent = this.transform;
			spawner [i].name = "Band" + i;
			spawner[i].GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);

			float engle = (Mathf.PI / 3.0f) * i;
			float x = Mathf.Cos (engle) * radius;
			float z = Mathf.Sin (engle) * radius;

			spawner[i].transform.position = new Vector3(x, 0, z);
		}
	}

	//대역폭에 따라 스폰 수가 다르게 해줌
	private GameObject SpawnCycle(int _num, float _bandwidth, Vector3 _pos){
		switch (_num) {
		case 0:
			if (_bandwidth > avgFreqData / 30.0f)
				return pool.NewItem (_pos);
			break;
		case 1:
			if(_bandwidth > avgFreqData / 3.0f)
				return pool.NewItem (_pos);
			break;
		case 2:
			if(_bandwidth > avgFreqData)
				return pool.NewItem (_pos);
			break;
		case 3:
			if(_bandwidth > avgFreqData / 8.0f)
				return pool.NewItem (_pos);
			break;
		case 4:
			if(_bandwidth > avgFreqData / 10.0f)
				return pool.NewItem (_pos);
			break;
		case 5:
			if(_bandwidth > avgFreqData / 40.0f)
				return pool.NewItem (_pos);
			break;
		}

		return null;
	}

	//대역폭에 따라 시작평균대역폭과 비교해 생성 및 노드 속도 관리
	void SoundStep(float _band){
		bChange = false;

		if (_band <= avgFreqData) {
			fps = 0.6f;
			nodeSpd = 3.0f;
			ChangeStep (fps);
		}
		if (avgFreqData < _band && _band <= avgFreqData * 1.5f) {
			fps = 0.8f;
			nodeSpd = 3.2f;
			ChangeStep (fps);
		}
		if (avgFreqData * 1.5f < _band && _band <= avgFreqData * 2.0f) {
			fps = 1.0f;
			nodeSpd = 3.4f;
			ChangeStep (fps);
		}
		if (avgFreqData * 2.0f < _band && _band <= avgFreqData * 2.5f) {
			fps = 1.2f;
			nodeSpd = 3.6f;
			ChangeStep (fps);
		}
		if (avgFreqData * 2.5f < _band && _band <= avgFreqData * 3.0f) {
			fps = 1.4f;
			nodeSpd =3.8f;
			ChangeStep (fps);
		}
		if (avgFreqData * 3.0f < _band && _band <= avgFreqData * 3.5f) {
			fps = 1.6f;
			nodeSpd = 4.0f;
			ChangeStep (fps);
		}
		if (avgFreqData * 3.5f < _band) {
			fps = 2.0f;
			nodeSpd = 4.4f;
			ChangeStep (fps);
		}

		StartCoroutine ("ChangeUnlock");
	}

	//대역폭 맞춰 속도 관리
	void ChangeStep(float _fps){
		angleSpd = 1.0f / _fps;

		CancelInvoke("check");
		InvokeRepeating("check", 0.0f, angleSpd);
	}

	//대역폭 평균, 회전, 변경 제어
	IEnumerator StartAvgFreqData(){
		yield return new WaitForSeconds (3.0f);
		avgFreqData = (band [1] + band[2] + band[3]) /4;
		bChange = true;
	}
	IEnumerator ChangeAngle(){
		yield return new WaitForSeconds (2.0f);
		angle = 60 * Random.Range (-5, 6);
		StartCoroutine ("ChangeAngle");
	}
	IEnumerator ChangeUnlock(){
		yield return new WaitForSeconds (0.05f);
		bChange = true;
	}
}
