public class MazeCell {

	public bool init = false;
	public int walls = 0x1111;

	public MazeCell() {
	}

	public MazeCell(MazeCell copy) {
		this.init = copy.init;
		this.walls = copy.walls;
	}

	/*	Up		0
		Down	1
		Left	2
		Right	3
	*/
	public bool HasWall(int inCode) {
		int wallCode = WALLS[inCode];
		return (walls & wallCode) == wallCode;
	}

	public static readonly int UP = 0x1000;
	public static readonly int DOWN = 0x0100;
	public static readonly int LEFT = 0x0010;
	public static readonly int RIGHT = 0x0001;

	private static readonly int[] WALLS = new int[] { UP, DOWN, LEFT, RIGHT };

}