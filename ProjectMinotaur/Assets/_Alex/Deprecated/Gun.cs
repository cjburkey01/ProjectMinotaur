/*using UnityEngine;

public class GunOld : MonoBehaviour {

	//variables 
	public float target;
	public GameObject player;
	public GameObject playerArm;
	public GameObject enemy;
	public GameObject barrelEnd;

	private Vector3 Offset;
	private RaycastHit hit;

	// Use this for initialization
	void Start() {
		Offset = transform.position;
	}

	// Update is called once per frame
	void Update() {
		GunHandler();
	}

	//gun stuff
	void GunHandler() {
		if (Input.GetKeyDown(KeyCode.X)) {
			Physics.Raycast(barrelEnd.transform.position, barrelEnd.transform.TransformDirection(Vector3.forward), out hit);
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawRay(barrelEnd.transform.position, barrelEnd.transform.TransformDirection(Vector3.forward));
	}
}*/