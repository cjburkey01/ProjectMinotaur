using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMazeGenerator : MonoBehaviour {

	private Dictionary<ChunkPos, MazeChunk> chunks;

	public int chunksX = 4;
	public int chunksY = 4;

	public float cellSize = 10.0f;
	public float wallSize = 2.0f;
	public float textureSize = 1.0f;

	public GameObject chunkPrefab;

	void Start() {
		chunks = new Dictionary<ChunkPos, MazeChunk>();
	}

	public void GenerateMaze() {
		for (int x = 0; x < chunksX; x++) {
			for (int y = 0; y < chunksX; y++) {
				AddChunk(new ChunkPos(x, y));
			}
		}
	}

	private void AddChunk(ChunkPos pos) {
		GameObject obj = Instantiate<GameObject>(chunkPrefab);
		chunks.Add(pos, obj.GetComponent<MazeChunk>());
	}

	public void RenderMaze() {

	}

	public int GetSizeX() {
		return chunksX * MazeChunk.chunkSize;
	}

	public int GetSizeY() {
		return chunksY * MazeChunk.chunkSize;
	}

}