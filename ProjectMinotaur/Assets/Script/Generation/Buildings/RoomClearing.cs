using UnityEngine;

public class RoomClearing : MonoBehaviour, IBuilding {

	public int width = 3;
	public int height = 3;
	public float generateChance = 0.005f;

	public bool ForCell(MazeGenerate generator, int mazeX, int mazeY, int seed) {
		int max = 999999999;
		float rand = (float) BetterRandom.Between(0, max, seed) / max;
		return rand < generateChance;
	}

	public void OnGenerate(Vector2 position, MazeGenerate generator, int mazeX, int mazeY, int seed) {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				generator.SetWalls(mazeX, mazeY, 0x0000);
			}
		}
	}

}