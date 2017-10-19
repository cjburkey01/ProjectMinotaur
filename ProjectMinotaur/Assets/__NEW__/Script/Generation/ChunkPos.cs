using System;

public struct ChunkPos : IEquatable<ChunkPos> {

	public int x { get; private set; }

	public int y { get; private set; }

	public ChunkPos(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public override bool Equals(object other) {
		if (other is ChunkPos) {
			return Equals((ChunkPos)other);
		}
		return false;
	}

	public bool Equals(ChunkPos other) {
		return other.x == x && other.y == y;
	}

	public override int GetHashCode() {
		int hash = 17;
		hash = hash * 31 + x;
		hash = hash * 31 + y;
		return hash;
	}

}