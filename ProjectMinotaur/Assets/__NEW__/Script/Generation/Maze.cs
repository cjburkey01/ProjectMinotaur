using UnityEngine;

public class Maze : MonoBehaviour {

	public int chunkSize = 16;
	public int mazeChunkWidth = 2;
	public int mazeChunkHeight = 2;

	private MazeChunk[,] maze;
	private MazeChunk chunkPrefab;

	// Initialize the maze array and locate the prefab in the resources folder, called "MazeChunk"
	void Start() {
		maze = new MazeChunk[mazeChunkWidth, mazeChunkHeight];
		chunkPrefab = ((GameObject) Resources.Load("MazeChunk")).GetComponent<MazeChunk>();
	}

	// Adds a new chunk to the maze, provided that the position is inside the maze-bounds-
	// and the chunk does not already exist.
	private void addChunk(int x, int y) {
		if (!chunkInMaze(x, y)) {
			return;
		}
		if (maze[x, y] != null) {
			return;
		}
	}

	// Returns whether or not the chunk can exist in the maze.
	public bool chunkInMaze(int x, int y) {
		return x >= 0 && x < mazeChunkWidth && y >= 0 && y < mazeChunkHeight;
	}

	// Returns whether or not the node can exist in the maze.
	public bool nodeInMaze(int x, int y) {
		return x >= 0 && x < mazeChunkWidth * chunkSize && y >= 0 && y < mazeChunkHeight * chunkSize;
	}

}