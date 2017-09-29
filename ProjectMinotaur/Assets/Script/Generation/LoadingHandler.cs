using UnityEngine;
using UnityEngine.UI;

public class LoadingHandler : MonoBehaviour {

	public GameObject loadingCamera;
	public GameObject player;
	public GameObject loadingScreen;

	public Text displayText;

	private MazeMesher mesher;
	private bool previousLoading;

	void Start() {
		print(gameObject);
		mesher = GetComponent<MazeMesher>();
		if (mesher == null) {
			Debug.LogError("Mesher not found on loading object.");
			gameObject.SetActive(false);
		}
	}

	void Update() {
		if (previousLoading != mesher.isBuilding) {
			Set(mesher.isBuilding);
			previousLoading = mesher.isBuilding;
		}
	}

	public void Set(bool loading) {
		loadingCamera.SetActive(loading);
		loadingScreen.SetActive(loading);
		player.SetActive(!loading);
		if (!loading) {
			player.transform.position = new Vector3(5.0f, 5.0f, 5.0f);
		}
	}

}