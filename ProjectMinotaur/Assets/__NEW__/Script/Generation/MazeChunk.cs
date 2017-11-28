using UnityEngine;

public class MazeChunk {

	private readonly int chunkSize;
	protected MazeNode[,] chunk;

	public MazeChunk(int chunkSize) {
		this.chunkSize = chunkSize;
		chunk = new MazeNode[chunkSize, chunkSize];
	}

	public MazeNode GetNode(int x, int y) {
		if (!InChunk(x, y)) {
			return null;
		}
		return chunk[x, y];
	}

	public void AddNode(int x, int y) {
		if (!InChunk(x, y)) {
			return;
		}
		if (GetNode(x, y) != null) {
			return;
		}
		chunk[x, y] = new MazeNode();
	}

	// Prepopulates the chunk with empty nodes.
	public void InitializeNodes() {
		for (int x = 0; x < chunkSize; x++) {
			for (int y = 0; y < chunkSize; y++) {
				AddNode(x, y);
			}
		}
	}

	public bool InChunk(int x, int y) {
		return x >= 0 && x < chunkSize && y >= 0 && y < chunkSize;
	}

}