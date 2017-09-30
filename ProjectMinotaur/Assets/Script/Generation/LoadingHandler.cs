using UnityEngine;
using UnityEngine.UI;

public class LoadingHandler : MonoBehaviour {

	public GameObject loadingCamera;
	public GameObject player;
	public GameObject loadingScreen;
	public bool loading { private set; get; }

	public Text displayText;

	private MazeMesher mesher;
	private bool previousLoading;

	void Start() {
		mesher = GetComponent<MazeMesher>();
		if (mesher == null) {
			Debug.LogError("Mesher not found on loading object.");
			gameObject.SetActive(false);
		}
	}

	public void Set(bool loading) {
		this.loading = loading;
		loadingCamera.SetActive(loading);
		loadingScreen.SetActive(loading);
		player.SetActive(!loading);
		if (!loading) {
			int x = Random.Range(0, mesher.generator.width - 1);
			int y = Random.Range(0, mesher.generator.height - 1);
			Vector2 pos = mesher.WorldPosOfCell(x, y);
			player.transform.position = new Vector3(pos.x, 5.0f, pos.y);
		}
	}

}