using UnityEngine;

public class RoomClearing : IBuilding {

	public int width = 3;
	public int height = 3;

	public override bool ForCell(MazeGenerate generator, int mazeX, int mazeY) {
		if (mazeX >= generator.width || mazeY >= generator.height || mazeX == 0 || mazeY == 0) {
			return false;
		}
		for (int x = -1; x < width; x ++) {
			for (int y = -1; y < height; y ++) {
				MazeCell cell = generator.GetCell(mazeX + x, mazeY + y);
				if (cell == null || cell.walls == 0x0000) {
					return false;
				}
			}
		}
		return true;
	}

	public override bool OnGenerate(Vector2 position, MazeGenerate generator, int mazeX, int mazeY, int seed) {
		for (int x = 0; x < width - 1; x ++) {
			for (int y = 0; y < height - 1; y ++) {
				generator.SetWalls(mazeX + x, mazeY + y, 0x0000);
			}
		}
		return true;
	}

}