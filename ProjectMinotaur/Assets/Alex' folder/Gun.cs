using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	//variables 
	public float target;
	public Rigidbody rb;
	public GameObject player;
	public GameObject playerArm;
	public GameObject enemy;
	public GameObject bulletLineBegin;
	public GameObject bulletLineEnd;

	private Vector3 Offset;
	private RaycastHit hit;

	// Use this for initialization
	void Start() {
		rb = GetComponent<Rigidbody>();
		Offset = transform.position;
	}

	// Update is called once per frame
	void Update() {
		gunHandler();
	}

	void gunHandler() {
		if (Input.GetKey(KeyCode.X)) {
			Physics.Raycast(bulletLineBegin.transform.position, transform.TransformDirection(Vector3.forward), out hit);
			OnDrawGizmos();
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawRay(transform.position, Vector3.forward);
	}
}
