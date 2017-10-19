using UnityEngine;

public class MazeChunk : MonoBehaviour {
	
	// Walls: { top, left }
	public static readonly int chunkSize = 16;
	private static readonly int[] WALLS = new int[] { 0x10, 0x01 };

	private int[,] data;

	void Start() {
		data = new int[chunkSize, chunkSize];
	}

	public void GenerateChunk() {

	}

	public void SetWalls(int x, int y, bool top, bool left) {
		if (InChunk(x, y)) {
			data[x, y] = ((top) ? WALLS[0] : 0) + ((left) ? WALLS[1] : 0);
		}
	}

	public bool HasWall(int x, int y, int wall) {
		if (wall < WALLS.Length && InChunk(x, y)) {
			int wallCode = WALLS[wall];
			return (data[x, y] & wallCode) == wallCode;
		}
		return false;
	}

	public bool[] GetWalls(int x, int y) {
		if (InChunk(x, y)) {
			return new bool[] { HasWall(x, y, 0), HasWall(x, y, 1) };
		}
		return new bool[] { false };
	}

	public bool InChunk(int x, int y) {
		return x < chunkSize && y < chunkSize && x >= 0 && y >= 0;
	}

}