using System.Collections.Generic;
using UnityEngine;

public class MazeChunk {

	private readonly int chunkSize;
	private readonly MazePos pos;
	private bool initialized = false;
	protected List<MazeNode> nodes;

	public MazeChunk(int x, int y, int chunkSize) {
		pos = new MazePos(x, y);
		this.chunkSize = chunkSize;
		nodes = new List<MazeNode>();
	}

	public MazeNode GetNode(int x, int y) {
		if (!InChunk(x, y)) {
			return null;
		}
		foreach (MazeNode node in nodes) {
			if (node.GetPosition().GetX() == x && node.GetPosition().GetY() == y) {
				return node;
			}
		}
		return null;
	}

	public void AddNode(int x, int y) {
		if (!InChunk(x, y)) {
			return;
		}
		if (GetNode(x, y) != null) {
			return;
		}
		nodes.Add(new MazeNode(x, y, pos.GetX() * chunkSize + x, pos.GetY() * chunkSize + y));
	}

	// Prepopulates the chunk with empty nodes.
	public void InitializeNodes() {
		for (int x = 0; x < chunkSize; x++) {
			for (int y = 0; y < chunkSize; y++) {
				AddNode(x, y);
			}
		}
		initialized = true;
	}

	public bool InChunk(int x, int y) {
		return x >= 0 && x < chunkSize && y >= 0 && y < chunkSize;
	}

	public MazePos GetGlobalPosition(int x, int y) {
		return new MazePos(chunkSize * pos.GetX() + x, chunkSize * pos.GetY() + y);
	}

	public bool IsInitialized() {
		return initialized;
	}

	public MazePos GetPosition() {
		return pos;
	}

}