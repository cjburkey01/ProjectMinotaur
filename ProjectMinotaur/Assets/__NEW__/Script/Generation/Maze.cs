using UnityEngine;

public class Maze {

	public readonly int chunkSize;
	public readonly int mazeChunkWidth;
	public readonly int mazeChunkHeight;

	protected MazeChunk[,] maze;

	public Maze(int chunkSize, int mazeChunkWidth, int mazeChunkHeight) {
		this.chunkSize = chunkSize;
		this.mazeChunkWidth = mazeChunkWidth;
		this.mazeChunkHeight = mazeChunkHeight;
		maze = new MazeChunk[mazeChunkWidth, mazeChunkHeight];
		AddGenerationListeners();
	}

	private void AddGenerationListeners() {
		PMEventSystem.GetEventSystem().AddListener<EventChunkPrePopulationBegin>(BeginPrepopulate<EventChunkPrePopulationBegin>);
		PMEventSystem.GetEventSystem().AddListener<EventChunkPrePopulationFinish>(FinishPrepopulate<EventChunkPrePopulationFinish>);
		PMEventSystem.GetEventSystem().AddListener<EventMazeGenerationBegin>(BeginMaze<EventMazeGenerationBegin>);
		PMEventSystem.GetEventSystem().AddListener<EventMazeGenerationFinish>(FinishMaze<EventMazeGenerationFinish>);
		PMEventSystem.GetEventSystem().AddListener<EventChunkGenerationBegin>(BeginChunk<EventChunkGenerationBegin>);
		PMEventSystem.GetEventSystem().AddListener<EventChunkGenerationFinish>(FinishChunk<EventChunkGenerationFinish>);
	}

	// Prepopulate the maze with chunks.
	private void InitializeChunks() {
		for (int x = 0; x < mazeChunkWidth; x++) {
			for (int y = 0; y < mazeChunkWidth; y++) {
				AddChunk(x, y);
				MazeChunk chunk = GetChunk(x, y);
				if (chunk != null) {
					chunk.InitializeNodes();
				}
			}
		}
	}

	public MazeChunk GetChunk(int x, int y) {
		if (!InMaze(x, y)) {
			return null;
		}
		return maze[x, y];
	}

	public void AddChunk(int x, int y) {
		if (!InMaze(x, y)) {
			return;
		}
		if (GetChunk(x, y) != null) {
			return;
		}
		maze[x, y] = new MazeChunk(chunkSize);
	}

	public bool InMaze(int x, int y) {
		return x >= 0 && x < mazeChunkWidth && y >= 0 && y < mazeChunkHeight;
	}

	public MazeNode GetNode(int x, int y) {
		int chunkX = Mathf.FloorToInt((float) chunkSize / (float) x);
		int chunkY = Mathf.FloorToInt((float) chunkSize / (float) y);
		MazeChunk chunk = GetChunk(chunkX, chunkY);
		if (chunk == null) {
			return null;
		}
		int inChunkX = chunkSize % x;
		int inChunkY = chunkSize % y;
		return chunk.GetNode(inChunkX, inChunkY);
	}

	// -- The Rest -- //

	public void Generate() {
		PMEventSystem.GetEventSystem().TriggerEvent<EventChunkPrePopulationBegin>(new EventChunkPrePopulationBegin(this));
		InitializeChunks();
		PMEventSystem.GetEventSystem().TriggerEvent<EventChunkPrePopulationFinish>(new EventChunkPrePopulationFinish(this));

		PMEventSystem.GetEventSystem().TriggerEvent<EventMazeGenerationBegin>(new EventMazeGenerationBegin(this));
		for (int x = 0; x < mazeChunkWidth; x++) {
			for (int y = 0; y < mazeChunkWidth; y++) {
				MazeChunk chunk = GetChunk(x, y);
				PMEventSystem.GetEventSystem().TriggerEvent<EventChunkGenerationBegin>(new EventChunkGenerationBegin(this, chunk, x, y));
				PMEventSystem.GetEventSystem().TriggerEvent<EventChunkGenerationFinish>(new EventChunkGenerationFinish(this, chunk, x, y));
			}
		}
		PMEventSystem.GetEventSystem().TriggerEvent<EventMazeGenerationFinish>(new EventMazeGenerationFinish(this));
	}

	// -- EVENTS -- //

	// Fired when the maze begins to create all the chunks.
	private void BeginPrepopulate<IPMEvent>(EventChunkPrePopulationBegin e) where IPMEvent : EventChunkPrePopulationBegin {
		Debug.Log("Begin chunk prepopulation chunk event");
	}

	// Fired when the maze finishes creating all the chunks.
	private void FinishPrepopulate<IPMEvent>(EventChunkPrePopulationFinish e) where IPMEvent : EventChunkPrePopulationFinish {
		Debug.Log("Finish chunk prepopulation chunk event");
	}

	// Called when the maze generation begins.
	private void BeginMaze<IPMEvent>(EventMazeGenerationBegin e) where IPMEvent : EventMazeGenerationBegin {
		Debug.Log("Begin generating maze event");
	}

	// Called when the maze generation finishes.
	private void FinishMaze<IPMEvent>(EventMazeGenerationFinish e) where IPMEvent : EventMazeGenerationFinish {
		Debug.Log("Finish generating maze event");
	}

	// Called when a specific chunk begins to generate.
	private void BeginChunk<IPMEvent>(EventChunkGenerationBegin e) where IPMEvent : EventChunkGenerationBegin {
		Debug.Log("Begin generating chunk event");
	}

	// This event is fired when the current chunk has finished being generated.
	private void FinishChunk<IPMEvent>(EventChunkGenerationFinish e) where IPMEvent : EventChunkGenerationFinish {
		Debug.Log("Finish generating chunk event");
	}

}