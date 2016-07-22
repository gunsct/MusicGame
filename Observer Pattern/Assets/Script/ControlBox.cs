using UnityEngine;
using System.Collections;

[RequireComponent(typeof (AudioClip))]
public class ControlBox : MonoBehaviour {
	public int numOfSamples = 1024; //Min: 64, Max: 8192

	public float[] freqData;
	public float avgFreqData;
	public float FreqData3;
	public float[] band;

	public GameObject[] spawnerModel = new GameObject[6];
	private GameObject[] spawner;
	private int core = 3;
	private float radius = 10.0f;
	private float angle = 60.0f;
	private float angleSpd;
	public float fps;

	public GameObject node;
	public float nodeSpd;

	private bool bChange = false;

	void Start()
	{
		angleSpd = 1.0f / fps;
		freqData = new float[numOfSamples];

		int n = freqData.Length;

		int k = 0;
		for (int j = 0; j < freqData.Length; j++)
		{
			n = n / core;
			if (n <= 0) break;
			k++;
		}

		band = new float[k + 1];
		spawner = new GameObject[k + 1];

		CreateSpawner ();

		StartCoroutine ("StartAvgFreqData");
		StartCoroutine ("ChangeAngle");
		InvokeRepeating("check", 0.0f, angleSpd); // update at 15 fps

	}
	void Update(){
		this. transform.Rotate(new Vector3(0,60.0f / angleSpd,0) * Time.deltaTime, Space.World);
	}

	private void check()
	{
		AudioListener.GetSpectrumData(freqData, 0, FFTWindow.Rectangular);

		int k = 0;
		int crossover = core;

		for (int i = 0; i < freqData.Length; i++)
		{
			float frequency = freqData[i];
			float bandwidth = band[k];

			// find the max as the peak value in that frequency band.
			band[k] = (frequency > bandwidth) ? frequency : bandwidth;

			if (i > (crossover - (Mathf.Pow(core,2)-1)))
			{
				k++;
				crossover *= core;   // frequency crossover point for each band.
				Vector3 tmp = new Vector3(spawner[k].transform.position.x, band[k] * 64, spawner[k].transform.position.z);
				spawner[k].transform.position = tmp;

				SpawnCycle (k, bandwidth, tmp);
				FreqData3 = band [3];
				if (k == 2 && bChange == true) {
					SoundStep (bandwidth);
				}
				band[k] = 0;
			}
		}
	}

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

	private void SpawnCycle(int _num, float _bandwidth, Vector3 _pos){
		switch (_num) {
		case 0:
			if(_bandwidth > avgFreqData / 30.0f)
				Instantiate (node, _pos, Quaternion.identity);
			break;
		case 1:
			if(_bandwidth > avgFreqData / 3.0f)
				Instantiate (node, _pos, Quaternion.identity);
			break;
		case 2:
			if(_bandwidth > avgFreqData)
				Instantiate (node, _pos, Quaternion.identity);
			break;
		case 3:
			if(_bandwidth > avgFreqData / 8.0f)
				Instantiate (node, _pos, Quaternion.identity);
			break;
		case 4:
			if(_bandwidth > avgFreqData / 10.0f)
				Instantiate (node, _pos, Quaternion.identity);
			break;
		case 5:
			if(_bandwidth > avgFreqData / 40.0f)
				Instantiate (node, _pos, Quaternion.identity);
			break;
		}
	}

	void SoundStep(float _band){
		bChange = false;

		if (_band <= avgFreqData) {
			fps = 0.6f;
			nodeSpd = 3.2f;
			ChangeStep (fps);
		}
		if (avgFreqData < _band && _band <= avgFreqData * 1.5f) {
			fps = 0.8f;
			nodeSpd = 3.6f;
			ChangeStep (fps);
		}
		if (avgFreqData * 1.5f < _band && _band <= avgFreqData * 2.0f) {
			fps = 1.0f;
			nodeSpd = 4.0f;
			ChangeStep (fps);
		}
		if (avgFreqData * 2.0f < _band && _band <= avgFreqData * 2.5f) {
			fps = 1.2f;
			nodeSpd = 4.4f;
			ChangeStep (fps);
		}
		if (avgFreqData * 2.5f < _band && _band <= avgFreqData * 3.0f) {
			fps = 1.4f;
			nodeSpd = 4.8f;
			ChangeStep (fps);
		}
		if (avgFreqData * 3.0f < _band && _band <= avgFreqData * 3.5f) {
			fps = 1.6f;
			nodeSpd = 5.2f;
			ChangeStep (fps);
		}
		if (avgFreqData * 3.5f < _band) {
			fps = 2.0f;
			nodeSpd = 6.0f;
			ChangeStep (fps);
		}

		StartCoroutine ("ChangeUnlock");
	}

	void ChangeStep(float _fps){
		angleSpd = 1.0f / _fps;

		CancelInvoke("check");
		InvokeRepeating("check", 0.0f, angleSpd);
	}

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
