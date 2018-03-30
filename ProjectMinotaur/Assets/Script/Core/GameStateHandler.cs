using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate IEnumerator LoadingStep(GameStateHandler state, LoadingStepComplete onDone, Action<string> text, Action<float> progress);
public delegate void LoadingStepComplete();
public delegate void StepsComplete();

public class GameStateHandler : MonoBehaviour {

	public static GameStateHandler Instance { private set; get; }
	public static WorldHandler WorldHandler { private set; get; }
	public static string SaveFileTemp { private set; get; }

	public GameState State { private set; get; }

	public IMenu loadingMenu;
	public IMenu mainMenu;
	public IMenu optionsMenu;

	public MazeHandler menuMaze;
	public MazeHandler worldMaze;

	public MenuCamera menuCamera;
	public Player player;

	public ProgressBar progBar;
	public Text info;

	private bool working;
	
	public bool firstInit = true;

	void Start() {
		StartCoroutine(LateStart(0.5f));
	}

	private IEnumerator LateStart(float time) {
		yield return new WaitForSeconds(time);

		Instance = this;
		WorldHandler = new WorldHandler();
		GameHandler.paused = false;
		MenuSystem.GetInstance().HideMenus();
		menuMaze.gameObject.SetActive(false);
		worldMaze.gameObject.SetActive(false);

		PMEventSystem.GetEventSystem().AddListener<LoadingStepPopulate>(LoadSteps);

		SetState(GameState.MENU);
	}

	private void LoadSteps<T>(T e) where T : LoadingStepPopulate {
		if (firstInit) {
			e.AddStep(RegisterItems);
		}
		e.AddStep(AddMaze);
		if (e.Handler.State.Equals(GameState.MENU)) {
			e.AddStep(AddMenuCamera);
			e.AddStep(WaitForChunks);
			e.AddStep(HideMenus);
			e.AddStep(ShowMainMenu);
		} else {
			firstInit = false;
			e.AddStep(SpawnPlayer);
			e.AddStep(WaitForChunks);
			e.AddStep(LoadWorldParameters);
			e.AddStep(HideMenus);
		}
	}

	private bool d;
	private IEnumerator WaitForChunks(GameStateHandler state, LoadingStepComplete onDone, Action<string> t, Action<float> p) {
		d = false;
		t.Invoke("Loading chunks...");
		p.Invoke(0.5f);
		PMEventSystem.GetEventSystem().AddListener<EventMazeRenderChunkFinish>(ChunksDone);
		while (!d) {
			yield return null;
		}
		PMEventSystem.GetEventSystem().RemoveListener<EventMazeRenderChunkFinish>(ChunksDone);
		onDone.Invoke();
	}

	private void ChunksDone<T>(T e) where T : EventMazeRenderChunkFinish {
		d = true;
	}

	private IEnumerator SpawnPlayer(GameStateHandler state, LoadingStepComplete onDone, Action<string> t, Action<float> p) {
		t.Invoke("Spawning player...");
		p.Invoke(0.5f);
		menuCamera.gameObject.SetActive(false);
		player.gameObject.SetActive(true);
		player.Init();
		player.MovementMotor.locked = false;
		yield return null;
		onDone.Invoke();
		RandomPlayerPos(worldMaze);
	}

	private IEnumerator LoadWorldParameters(GameStateHandler state, LoadingStepComplete onDone, Action<string> t, Action<float> p) {
		t.Invoke("Loading world...");
		//WorldHandler.Load(SaveFileTemp);
		WorldHandler.NewWorld(worldMaze);
		//WorldHandler.Data.Set("CheatMode", true);
		yield return null;
		onDone.Invoke();
	}

	private IEnumerator ShowMainMenu(GameStateHandler state, LoadingStepComplete onDone, Action<string> t, Action<float> p) {
		t.Invoke("Loading menu...");
		p.Invoke(0.5f);
		MenuSystem.GetInstance().ShowMenu(mainMenu);
		yield return null;
		onDone.Invoke();
	}

	private IEnumerator RegisterItems(GameStateHandler state, LoadingStepComplete onDone, Action<string> t, Action<float> p) {
		t.Invoke("Loading items...");
		DefaultWeapons.Initialize();
		yield return null;
		onDone.Invoke();
	}

	private IEnumerator HideMenus(GameStateHandler state, LoadingStepComplete onDone, Action<string> t, Action<float> p) {
		t.Invoke("Clearing menus...");
		p.Invoke(0.5f);
		MenuSystem.GetInstance().HideMenus();
		yield return null;
		onDone.Invoke();
	}

	private IEnumerator AddMenuCamera(GameStateHandler state, LoadingStepComplete onDone, Action<string> t, Action<float> p) {
		t.Invoke("Spawning camera...");
		p.Invoke(0.5f);
		menuCamera.gameObject.SetActive(true);
		menuCamera.Init();
		yield return null;
		onDone.Invoke();
	}

	private IEnumerator AddMaze(GameStateHandler state, LoadingStepComplete onDone, Action<string> text, Action<float> progress) {
		text.Invoke("Generating maze...");
		progress.Invoke(0.0f);

		if (worldMaze.Generated) {
			worldMaze.gameObject.SetActive(true);
			worldMaze.Clear();
			worldMaze.gameObject.SetActive(false);
		}
		if (menuMaze.Generated) {
			menuMaze.gameObject.SetActive(true);
			menuMaze.Clear();
			menuMaze.gameObject.SetActive(false);
		}

		if (state.State.Equals(GameState.MENU)) {
			menuMaze.gameObject.SetActive(true);
			menuMaze.Generate((p) => progress.Invoke(p), onDone);
		} else {
			worldMaze.gameObject.SetActive(true);
			worldMaze.Generate((p) => progress.Invoke(p), onDone);
		}
		yield return null;
	}

	void Update() {
		if (State.Equals(GameState.MENU) && Input.GetButtonDown("Cancel")) {
			if (optionsMenu.IsShown()) {
				MenuSystem.GetInstance().ShowMenu(mainMenu);
			} else {
				MenuSystem.GetInstance().ShowMenu(optionsMenu);
			}
		}
	}

	public void SetState(GameState state) {
		StartCoroutine(DoState(state));
	}

	private IEnumerator DoState(GameState state) {
		if (progBar) {
			progBar.Reset();
		}
		if (info != null) {
			info.text = "Please wait...";
		}
		yield return null;

		State = state;
		PMEventSystem.GetEventSystem().TriggerEvent(new StateChangeEvent(this));
		if (player.MovementMotor != null) {
			player.MovementMotor.locked = true;
		}
		player.gameObject.SetActive(false);
		menuCamera.gameObject.SetActive(false);
		GameHandler.paused = false;

		if (state.Equals(GameState.MENU)) {
			LoadMenu();
		} else if (state.Equals(GameState.INGAME)) {
			LoadGame();
		}
	}

	private void LoadMenu() {
		List<LoadingStep> steps = PopulateLoadingSteps();
		StartCoroutine(ExecuteSteps(steps, () => {
			Debug.Log("Loaded menu");
		}));
	}

	private void LoadGame() {
		List<LoadingStep> steps = PopulateLoadingSteps();
		StartCoroutine(ExecuteSteps(steps, () => {
			Debug.Log("Loaded game");
		}));
	}

	private IEnumerator ExecuteSteps(List<LoadingStep> steps, StepsComplete done) {
		MenuSystem.GetInstance().ShowMenu(loadingMenu);
		yield return null;

		float progress = 0.0f;
		string label = "Please wait...";
		for (int i = 0; i < steps.Count; i++) {
			LoadingStep step = steps[i];
			working = true;
			StartCoroutine(step.Invoke(this, StepComplete, l => label = l, p => progress = p));
			while (working) {
				if (progBar != null) {
					progBar.Progress = (i + progress) / steps.Count;
				}
				if (info != null) {
					info.text = label;
				}
				yield return null;
			}
		}
		Debug.Log("Loading steps complete.");
		done.Invoke();
	}

	private List<LoadingStep> PopulateLoadingSteps() {
		List<LoadingStep> steps = new List<LoadingStep>();
		PMEventSystem.GetEventSystem().TriggerEvent(new LoadingStepPopulate(this, steps));
		return steps;
	}

	private void StepComplete() {
		working = false;
	}

	private void RandomPlayerPos(MazeHandler handler) {
		MazePos pos = new MazePos(Util.NextRand(0, handler.chunksX * handler.chunkSize - 1), Util.NextRand(0, handler.chunksX * handler.chunkSize - 1));
		Vector3 plyPos = handler.GetWorldPosOfNode(pos, 0.5f) + new Vector3(handler.pathWidth / 2.0f, 5.0f, handler.pathWidth / 2.0f);
		player.MovementMotor.ResetTransform(plyPos);
		Debug.Log("Player at node: " + pos + ". World: " + plyPos);
	}

	public void NewGameClick() {
		SetState(GameState.INGAME);
	}

	public void OptionsMenuClick() {
		MenuSystem.GetInstance().ShowMenu(optionsMenu);
	}

}

public enum GameState {

	MENU,
	INGAME

}

public class LoadingStepPopulate : IPMEvent {

	public GameStateHandler Handler { private set; get; }

	private List<LoadingStep> steps;

	public LoadingStepPopulate(GameStateHandler handler, List<LoadingStep> todo) {
		Handler = handler;
		steps = todo;
	}

	public void AddStep(LoadingStep step) {
		if (!steps.Contains(step)) {
			steps.Add(step);
		}
	}

	public string GetName() {
		return GetType().Name;
	}

	public bool IsCancellable() {
		return false;
	}

	public void Cancel() { }

	public bool IsCancelled() {
		return false;
	}

}

public class StateChangeEvent : IPMEvent {

	public GameStateHandler Handler { private set; get; }

	public StateChangeEvent(GameStateHandler handler) {
		Handler = handler;
	}

	public string GetName() {
		return GetType().Name;
	}

	public bool IsCancellable() {
		return false;
	}

	public void Cancel() { }

	public bool IsCancelled() {
		return false;
	}

}