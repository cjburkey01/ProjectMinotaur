using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour {

	public IMenu mainMenu;
	public IMenu loadingScreen;
	public IMenu optionsMenu;
	public PlayerMove player;
	public Text loadingText;
	public ProgressBar progBar;

	public int realChunksX = 16;
	public int realChunksY = 16;

	void Update() {
		if (optionsMenu.IsShown() && !player.gameObject.activeInHierarchy && Input.GetButtonDown("Cancel")) {
			MenuSystem.GetInstance().ShowMenu(mainMenu);
		}
	}

	private void GenerateRealMaze() {
		PMEventSystem.GetEventSystem().AddListener<EventMazeGenerationUpdate>(MazeUpdate);
		PMEventSystem.GetEventSystem().AddListener<EventMazeGenerationFinish>(MazeDone);
		PMEventSystem.GetEventSystem().AddListener<EventMazeRenderChunkFinish>(OnMazeGenerated);
	}

	private void MazeUpdate<T>(T e) where T : EventMazeGenerationUpdate {
		if (loadingText != null) {
			loadingText.text = "Generating...";
			progBar.progress = e.GetProgress() / 2.0f;
		}
	}

	private void MazeDone<T>(T e) where T : EventMazeGenerationFinish {
		if (loadingText != null) {
			loadingText.text = "Creating chunks...";
			progBar.progress = 1.0f;
		}
	}

	private void OnMazeGenerated<T>(T e) where T : EventMazeRenderChunkFinish {
		MenuSystem.GetInstance().HideMenu(loadingScreen);
		player.locked = false;
		Debug.Log("Real maze done.");
		progBar.progress = 0.0f;
		PMEventSystem.GetEventSystem().RemoveListener<T>(OnMazeGenerated);
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
		MenuSystem.GetInstance().ShowMenu(loadingScreen);
		player.gameObject.SetActive(true);
		GenerateRealMaze();
		handler.chunksX = realChunksX;
		handler.chunksY = realChunksY;
		handler.Clear();
		print("Loading real maze...");
		handler.Generate();
	}

	public void LoadClick() {
	}

	public void OptionsClick() {
		MenuSystem.GetInstance().ShowMenu(optionsMenu);
	}

}