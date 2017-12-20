using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject loadingScreen;
	public PlayerMove player;
	public Text loadingText;

	public int realChunksX = 16;
	public int realChunksY = 16;

	void Start() {
		PMEventSystem.GetEventSystem().AddListener<EventMazeGenerationUpdate>(MazeUpdate);
		PMEventSystem.GetEventSystem().AddListener<EventMazeGenerationFinish>(MazeDone);
	}

	private void MazeUpdate<T>(T e) where T : EventMazeGenerationUpdate {
		if (loadingText != null) {
			loadingText.text = "Generating: " + (e.GetProgress() * 100.0f).ToString("00.0000") + "%";
		}
	}

	private void MazeDone<T>(T e) where T : EventMazeGenerationFinish {
		if (loadingText != null) {
			loadingText.text = "Loading...";
		}
	}

	public void NewClick() {
		MazeHandler handler = FindObjectOfType<MazeHandler>();
		if (handler == null) {
			return;
		}
		MenuCamera cam = FindObjectOfType<MenuCamera>();
		if (cam != null) {
			cam.gameObject.SetActive(false);
		}
		player.locked = true;
		mainMenu.SetActive(false);
		loadingScreen.SetActive(true);
		player.gameObject.SetActive(true);
		handler.chunksX = realChunksX;
		handler.chunksY = realChunksY;
		handler.Clear();
		print("Loading real maze...");
		handler.Generate();
	}

	public void LoadClick() {

	}

	public void OptionsClick() {

	}

}