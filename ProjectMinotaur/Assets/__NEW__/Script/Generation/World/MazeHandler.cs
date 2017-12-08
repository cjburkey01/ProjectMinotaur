using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeHandler : MonoBehaviour {

	public int chunkSize = 16;
	public int chunksX = 4;
	public int chunksY = 4;
	public float pathWidth = 15.0f;
	public float pathHeight = 25.0f;
	public float distanceVariability = 1.0f;
	public string chunkPrefabName = "MazeRenderedChunk";
	public GameObject[] nearnessObjects;

	private GameObject chunkPrefab;
	private Maze maze;

	void Start() {
		if (chunkPrefabName == null) {
			Debug.LogError("Couldn't load chunk prefab, name is null.");
			return;
		}
		chunkPrefabName = chunkPrefabName.Trim();
		if (chunkPrefabName.Length == 0) {
			Debug.LogError("Couldn't load chunk prefab, name is empty.");
			return;
		}
		chunkPrefab = Resources.Load(chunkPrefabName, typeof(GameObject)) as GameObject;
		if (chunkPrefab == null) {
			Debug.LogError("Couldn't load chunk prefab, file was missing or corrupted.");
			return;
		}
		Debug.Log("Loaded chunk prefab.");
		maze = new Maze(new DepthFirstMaze(), chunkSize, chunksX, chunksY);
		GenerateMaze();
	}

	public MazePos GetWorldPosAsNode(Vector3 pos) {
		int x = Mathf.FloorToInt(pos.x);
		int y = Mathf.FloorToInt(pos.z);
		return new MazePos(x, y);
	}

	public IEnumerator RenderMazeAroundObject(GameObject obj) {
		// TODO: To get it to stop telling me it's never used.
		obj.ToString();
		for (int x = 0; x < chunksX; x++) {
			for (int y = 0; y < chunksY; y++) {
				RenderChunk(new MazePos(x, y));
				yield return null; // Render another frame of the game to give the game
								   // time to refresh and prevent "not responding".
			}
		}
	}

	private void RenderChunk(MazePos pos) {
		GameObject instance = Instantiate(chunkPrefab, Vector3.zero, Quaternion.identity);
		instance.transform.name = "Chunk: " + pos;
		instance.transform.parent = transform;
		MazeRenderedChunk chunkObj = instance.GetComponent<MazeRenderedChunk>();
		if (chunkObj == null) {
			Debug.LogError("Failed to get maze chunk on chunk: " + pos);
			return;
		}
		StartCoroutine(chunkObj.Render(this, maze.GetChunk(pos.GetX(), pos.GetY())));
	}

	public Maze GetMaze() {
		return maze;
	}

	public void GenerateMaze() {
		PMEventSystem.GetEventSystem().AddListener<EventMazeGenerationBegin>(MazeBegin);
		PMEventSystem.GetEventSystem().AddListener<EventMazeGenerationFinish>(MazeFinish);
		PMEventSystem.GetEventSystem().AddListener<EventMazeGenerationUpdate>(MazeUpdate);
		PMEventSystem.GetEventSystem().AddListener<EventMazeRenderChunkBegin>(OnMazeRenderChunkBegin);
		PMEventSystem.GetEventSystem().AddListener<EventMazeRenderChunkFinish>(OnMazeRenderChunkFinish);
		maze.Generate(this, new MazePos(0, 0));
	}

	private void MazeBegin<T>(T e) where T : EventMazeGenerationBegin {
		e.IsCancellable();
		Debug.Log("Began maze generation.");
	}

	private void MazeFinish<T>(T e) where T : EventMazeGenerationFinish {
		Debug.Log("Finished maze generation.");
		//foreach (GameObject obj in nearnessObjects) {
		//	StartCoroutine(RenderMazeAroundObject(obj));
		//}
		Debug.Log("Let's test pathfinding!");
		/*Stack<MazePos> path = AStartMaze.PathFind(e.GetMaze(), new MazePos(0, 0), new MazePos(5, 5));
		Debug.Log("Failed: " + path == null);
		if (path != null) {
			Debug.Log("Nodes traversed:" + path.Count);
		}*/

		TemporaryDrawCode();
		StartCoroutine(AStarMaze.PathFind(e.GetMaze(), new MazePos(0, 0), new MazePos(maze.GetSizeX() - 1, maze.GetSizeY() - 1)));
	}

	private void MazeUpdate<T>(T e) where T : EventMazeGenerationUpdate {
		Debug.Log("Generating. Cycles: " + e.GetProgress());
	}

	private void OnMazeRenderChunkBegin<T>(T e) where T : EventMazeRenderChunkBegin {
		Debug.Log("Rendering chunk: " + e.GetChunk().GetPosition());
	}

	private void OnMazeRenderChunkFinish<T>(T e) where T : EventMazeRenderChunkFinish {
		Debug.Log("Rendered chunk: " + e.GetChunk().GetPosition());
	}

	private void TemporaryDrawCode() {
		float time = 300.0f;
		float yPos = 0.1f;
		for (int x = 0; x < maze.GetSizeX(); x++) {
			for (int y = 0; y < maze.GetSizeY(); y++) {
				MazeNode node = maze.GetNode(x, y);
				if (node == null) {
					Debug.LogError("How did this happen?");
					return;
				}
				bool[] walls = node.GetWalls();
				if (walls[0]) { // TOP
					Debug.DrawLine(new Vector3(x * pathWidth, yPos, y * pathWidth), new Vector3((x + 1) * pathWidth, yPos, y * pathWidth), Color.red, time, false);
				}
				if (walls[1]) { // BOTTOM
					Debug.DrawLine(new Vector3(x * pathWidth, yPos, (y + 1) * pathWidth), new Vector3((x + 1) * pathWidth, yPos, (y + 1) * pathWidth), Color.red, time, false);
				}
				if (walls[2]) { // RIGHT
					Debug.DrawLine(new Vector3((x + 1) * pathWidth, yPos, y * pathWidth), new Vector3((x + 1) * pathWidth, yPos, (y + 1) * pathWidth), Color.red, time, false);
				}
				if (walls[3]) { // LEFT
					Debug.DrawLine(new Vector3(x * pathWidth, yPos, y * pathWidth), new Vector3(x * pathWidth, yPos, (y + 1) * pathWidth), Color.red, time, false);
				}
			}
		}
	}

}

public class MazeRenderEvent : IPMEvent {

	private readonly Maze maze;

	public MazeRenderEvent(Maze maze) {
		this.maze = maze;
	}

	public string GetName() {
		return GetType().Name;
	}

	public bool IsCancellable() {
		return false;
	}

	public bool IsCancelled() {
		return false;
	}

	public void Cancel() {
	}

	public Maze GetMaze() {
		return maze;
	}

}

public class EventMazeRenderChunkBegin : MazeRenderEvent {

	private readonly MazeChunk chunk;

	public EventMazeRenderChunkBegin(Maze maze, MazeChunk chunk) : base(maze) {
		this.chunk = chunk;
	}

	public MazeChunk GetChunk() {
		return chunk;
	}

}

public class EventMazeRenderChunkFinish : MazeRenderEvent {

	private readonly MazeChunk chunk;

	public EventMazeRenderChunkFinish(Maze maze, MazeChunk chunk) : base(maze) {
		this.chunk = chunk;
	}

	public MazeChunk GetChunk() {
		return chunk;
	}

}