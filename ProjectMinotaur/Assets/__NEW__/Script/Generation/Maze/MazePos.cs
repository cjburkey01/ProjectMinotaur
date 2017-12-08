public struct MazePos {

	private readonly int x;
	private readonly int y;

	public MazePos(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public int GetX() {
		return x;
	}

	public int GetY() {
		return y;
	}

	public MazePos GetLeft(int i) {
		return new MazePos(x - i, y);
	}

	public MazePos GetRight(int i) {
		return new MazePos(x + i, y);
	}

	public MazePos GetUp(int i) {
		return new MazePos(x, y - i);
	}

	public MazePos GetDown(int i) {
		return new MazePos(x, y + i);
	}

	public override string ToString() {
		return "{" + x + ", " + y + "}";
	}

	public override int GetHashCode() {
		int result = 17;
		result = 37 * result + (x ^ (x >> 32));
		result = 37 * result + (y ^ (y >> 32));
		return result;
	}

}