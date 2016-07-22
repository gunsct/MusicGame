using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {
	private GameObject ControlBox;
	public Material[] materials = new Material[6];
	// Use this for initialization
	void Start () {
		this.GetComponent<Renderer>().material = materials[Random.Range (0, 6)];
		ControlBox = GameObject.Find ("ControlBox");
		this.transform.LookAt (ControlBox.transform.position);
		this.GetComponent<Rigidbody> ().velocity = this.transform.forward * ControlBox.GetComponent<ControlBox> ().nodeSpd;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider _col){
		if (_col.transform.tag.Equals ("PlayerHitBox")) {
			Destroy (this.gameObject);
		}

		if (_col.transform.tag.Equals ("PenaltyHitBox")) {
			Destroy (this.gameObject);
		}
	}
}
