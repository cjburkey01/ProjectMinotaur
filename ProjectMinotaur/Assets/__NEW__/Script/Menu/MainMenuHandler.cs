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

	void Start() {
		GameHandler.paused = false;
		GameHandler.inGame = false;
	}

	void Update() {
		if (!GameHandler.inGame && Input.GetButtonDown("Cancel")) {
			if (optionsMenu.IsShown()) {
				MenuSystem.GetInstance().ShowMenu(mainMenu);
			} else {
				MenuSystem.GetInstance().ShowMenu(optionsMenu);
			}
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
		if (!FindObjectOfType<MazeHandler>().Generated) {
			Debug.LogError("Maze isn't fully generated yet, but a chunk was rendered");
			return;
		}
		MenuSystem.GetInstance().HideMenu(loadingScreen);
		player.locked = false;
		Debug.Log("Real maze done.");
		progBar.progress = 0.0f;
		GameHandler.inGame = true;
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
		MenuSystem.GetInstance().ShowMenu(loadingScreen);
		player.locked = true;
		player.gameObject.SetActive(true);
		GenerateRealMaze();
		handler.chunksX = realChunksX;
		handler.chunksY = realChunksY;
		handler.Clear();
		print("Loading real maze...");
		handler.Generate();
		RandomPlayerPos(handler);
	}

	public void LoadClick() {
	}

	public void OptionsClick() {
		MenuSystem.GetInstance().ShowMenu(optionsMenu);
	}

	private void RandomPlayerPos(MazeHandler handler) {
		MazePos pos = new MazePos(Util.NextRand(0, handler.chunksX * handler.chunkSize - 1), Util.NextRand(0, handler.chunksX * handler.chunkSize - 1));
		Vector3 plyPos = handler.GetWorldPosOfNode(pos, 0.5f) + new Vector3(handler.pathWidth / 2.0f, 5.0f, handler.pathWidth / 2.0f);
		player.transform.position = plyPos;
		Debug.Log("Player at node: " + pos + ". World: " + plyPos);
	}

}