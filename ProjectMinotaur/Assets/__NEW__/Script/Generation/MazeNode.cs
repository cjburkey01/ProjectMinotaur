public class MazeNode {

	public readonly static int TOP = 0x1000;        // The top wall
	public readonly static int BOTTOM = 0x0100;     // The bottom wall
	public readonly static int RIGHT = 0x0010;      // The right wall
	public readonly static int LEFT = 0x0001;       // The left wall

	// The walls enabled or disabled on this node.
	private int walls;

	// Defaults the walls to none
	public MazeNode() {
		walls = 0x0000;
	}

	// Adds the specified wall to the node, providing it is not already enabled.
	public void AddWall(int wallCode) {
		if (hasWall(wallCode)) {
			return;
		}
		walls += wallCode;
	}

	// Removes the specified wall from the node, providing it is enabled.
	public void RemoveWall(int wallCode) {
		if (!hasWall(wallCode)) {
			return;
		}
		walls -= wallCode;
	}

	// Sets the specified wall to enabled to disabled, providing that it is not already in that state.
	public void SetWall(int wallCode, bool enabled) {
		if (enabled) {
			AddWall(wallCode);
		} else {
			RemoveWall(wallCode);
		}
	}

	// Sets all the walls to the specified walls.
	public void SetWalls(int walls) {
		this.walls = walls;
	}

	// Returns whether or not the provided wall is enabled or disabled.
	public bool hasWall(int wallCode) {
		return (walls & wallCode) == wallCode;
	}

	// Returns the state of all walls for this node in the following order:
	// TOP, BOTTOM, RIGHT, LEFT
	public bool[] GetWalls() {
		return new bool[] { hasWall(TOP), hasWall(BOTTOM), hasWall(RIGHT), hasWall(LEFT) };
	}

}