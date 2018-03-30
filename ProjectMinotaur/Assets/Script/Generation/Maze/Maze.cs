using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Maze {

	public readonly int chunkSize;
	public readonly int mazeChunkWidth;
	public readonly int mazeChunkHeight;
	public readonly float variability;

	protected readonly IAlgorithm mazeAlgorithm;
	protected List<MazeChunk> chunks;

	public Maze(IAlgorithm mazeAlgorithm, int chunkSize, int mazeChunkWidth, int mazeChunkHeight, float variability) {
		this.mazeAlgorithm = mazeAlgorithm;
		this.chunkSize = chunkSize;
		this.mazeChunkWidth = mazeChunkWidth;
		this.mazeChunkHeight = mazeChunkHeight;
		this.variability = variability;
		chunks = new List<MazeChunk>();
	}

	public Maze(int chunkSize, int chunksX, int chunksY, float variability, List<MazeChunk> chunks) : this(null, chunkSize, chunksX, chunksY, variability) {
		this.chunks = chunks;
	}

	public void SaveToFile(string filepath) {
		string output = "// What are you doing in this file?\n";
		output += "// Well, since you're already here, here's a link explaning how this thing works: http://bit.ly/2GDhYG5\n";
		output += "// I don't know why you'd need that; anyway, have fun decompiling this mess:\n\n";
		output += chunkSize + "|" + mazeChunkWidth + "|" + mazeChunkHeight + "|" + variability + "|";
		foreach (MazeChunk chunk in chunks) {
			output += chunk.GetString() + ">";
		}
		output = output.Substring(0, output.Length - 1);
		File.WriteAllText(filepath, output);
	}

	public static Maze LoadFromFile(string filepath) {
		// Comment				//
		// Between headers		|
		// Between chunks		>
		// Between chunkPos		x
		// Between nodes		^
		// Between offsets		,
		// Between nodePos		y
		// Example; a 1x1 chunk maze with 4x4 nodes in the chunk and 0 meters of variability:
		//		4|1|1|0.0|0x0x0y0y0.0,0.0y0^0y1y0.0,0.0y0^0y2y0.0,0.0y0^0y3y0.0,0.0y0^1y0y0.0,0.0y0^1y1y0.0,0.0y0^1y2y0.0,0.0y0^1y3y0.0,0.0y0^2y0y0.0,0.0y0^2y1y0.0,0.0y0^2y2y0.0,0.0y0^2y3y0.0,0.0y0^3y0y0.0,0.0y0^3y1y0.0,0.0y0^3y2y0^3y3y0.0,0.0y0
		// Example; a 1x2 chunk maze with 2x2 nodes in each chunk and 0 meters of variability:
		//		2|1|2|0.0|0x0x0y0y0.0,0.0y0^0y1y0.0,0.0y0^1y0y0.0,0.0y0^1y1y0.0,0.0y0>0x1x0y0y0.0,0.0y0^0y1y0.0,0.0y0^1y0y0.0,0.0y0^1y1y0.0,0.0y0
		//		^ ^ ^  ^  ^ ^ ^ ^    ^    ^^										 ^
		//		| | |  |  | | | |    |    ||										 |
		//		| | |  |  | | | |    |    ||										 The separator between chunks
		//		| | |  |  | | | |    |    |The separator between nodes in the chunk
		//		| | |  |  | | | |    |    The wall data
		//		| | |  |  | | | |    The world offset position (random and based on variability)
		//		| | |  |  | | | The node y position (in the chunk)
		//		| | |  |  | | The node x position (in the chunk)
		//		| | |  |  | The chunk y position
		//		| | |  |  The chunk x position
		//		| | |  The variability in maze node world offsets (+/-)
		//		| | The number of chunks in the y direction
		//		| The number of chunks in the x direction
		//		The number of node in the x and y direction that each chunk has
		if (filepath == null || !File.Exists(filepath)) {
			return null;
		}
		string[] inlines = File.ReadAllLines(filepath);
		string input = "";
		foreach (string line in inlines) {
			if (!line.StartsWith("//") && line.Trim().Length > 0) {
				input += line.Trim().Replace('\n', (char) 0).Replace(' ', (char) 0).Replace('\t', (char) 0);
			}
		}
		if (input == null || input.Length <= 0) {
			return null;
		}
		string[] headers = input.Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);
		if (headers.Length != 5) {
			Debug.LogError("Failed to load maze from file: " + filepath + ". The header split count was not 5");
			return null;
		}
		int chunkSize = -1;
		if (!int.TryParse(headers[0], out chunkSize)) {
			Debug.LogError("Failed to load maze from file: " + filepath + ". Could not parse chunksize: " + headers[0]);
			return null;
		}
		int chunksX = -1;
		if (!int.TryParse(headers[1], out chunksX)) {
			Debug.LogError("Failed to load maze from file: " + filepath + ". Could not parse chunksX: " + headers[1]);
			return null;
		}
		int chunksY = -1;
		if (!int.TryParse(headers[2], out chunksY)) {
			Debug.LogError("Failed to load maze from file: " + filepath + ". Could not parse chunksY: " + headers[2]);
			return null;
		}
		float variability = -1.0f;
		if (!float.TryParse(headers[3], out variability)) {
			Debug.LogError("Failed to load maze from file: " + filepath + ". Could not parse chunksY: " + headers[3]);
			return null;
		}
		string[] chunks = headers[4].Split(new char[] { '>' }, System.StringSplitOptions.RemoveEmptyEntries);
		if (chunks.Length != chunksX * chunksY) {
			Debug.LogError("Failed to load maze from file: " + filepath + ". Expected " + chunksX + "x" + chunksY + " chunks, but found " + chunks.Length + " instead.");
			return null;
		}
		List<MazeChunk> mazeChunks = new List<MazeChunk>();
		foreach (string chunk in chunks) {
			string[] pos = chunk.Split(new char[] { 'x' }, System.StringSplitOptions.RemoveEmptyEntries);
			if (pos.Length != 3) {
				Debug.LogError("Failed to load chunk in file: " + filepath + ". Did not find 3 position splits");
				return null;
			}
			int posX = -1;
			if (!int.TryParse(pos[0], out posX)) {
				Debug.LogError("Failed to load chunk in file: " + filepath + ". Could not parse " + pos[0] + " as x position");
				return null;
			}
			int posY = -1;
			if (!int.TryParse(pos[1], out posY)) {
				Debug.LogError("Failed to load chunk in file: " + filepath + ". Could not parse " + pos[1] + " as y position");
				return null;
			}
			string[] nodes = pos[2].Split(new char[] { '^' }, System.StringSplitOptions.RemoveEmptyEntries);
			if (nodes.Length != chunkSize * chunkSize) {
				Debug.LogError("Failed to load chunk in file: " + filepath + ". Expected " + chunkSize * chunkSize + " nodes, found " + nodes.Length);
				return null;
			}
			List<MazeNode> chunkNodes = new List<MazeNode>();
			foreach (string node in nodes) {
				string[] poss = node.Split(new char[] { 'y' }, System.StringSplitOptions.RemoveEmptyEntries);
				if (poss.Length != 4) {
					Debug.LogError("Failed to load node in file: " + filepath + ". Did not find 4 position splits");
					return null;
				}
				int x = -1;
				if (!int.TryParse(poss[0], out x)) {
					Debug.LogError("Failed to load node in file: " + filepath + ". Could not parse " + poss[0] + " as x position");
					return null;
				}
				int y = -1;
				if (!int.TryParse(poss[1], out y)) {
					Debug.LogError("Failed to load node in file: " + filepath + ". Could not parse " + poss[1] + " as y position");
					return null;
				}
				string[] offset = poss[2].Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
				if (offset.Length != 2) {
					Debug.LogError("Failed to load node in file: " + filepath + ". Could not parse " + poss[2] + " as offset data");
					return null;
				}
				float offX = -1.0f;
				if (!float.TryParse(offset[0], out offX)) {
					Debug.LogError("Failed to load node in file: " + filepath + ". Could not parse " + offset[0] + " as x offset");
					return null;
				}
				float offY = -1.0f;
				if (!float.TryParse(offset[1], out offY)) {
					Debug.LogError("Failed to load node in file: " + filepath + ". Could not parse " + offset[1] + " as y offset");
					return null;
				}
				int walls = -1;
				if (!int.TryParse(poss[3], out walls)) {
					Debug.LogError("Failed to load node in file: " + filepath + ". Could not parse " + poss[3] + " as wall data");
					return null;
				}
				chunkNodes.Add(new MazeNode(posX, posY, chunkSize, x, y, walls, new Vector2(offX, offY)));
			}
			mazeChunks.Add(new MazeChunk(posX, posY, chunkSize, chunkNodes));
		}
		return new Maze(chunkSize, chunksX, chunksY, variability, mazeChunks);
	}

	// Prepopulate the maze with chunks.
	private void InitializeChunks() {
		for (int x = 0; x < mazeChunkWidth; x++) {
			for (int y = 0; y < mazeChunkWidth; y++) {
				AddChunk(x, y);
				MazeChunk chunk = GetChunk(x, y);
				if (chunk != null) {
					chunk.InitializeNodes(variability);
				}
			}
		}
	}

	public void Destroy() {
		chunks.Clear();

	}

	public MazeChunk GetChunk(int x, int y) {
		if (!InMaze(x, y)) {
			return null;
		}
		foreach (MazeChunk chunk in chunks) {
			if (chunk.GetPosition().GetX() == x && chunk.GetPosition().GetY() == y) {
				return chunk;
			}
		}
		return null;
	}

	public void AddChunk(int x, int y) {
		if (!InMaze(x, y)) {
			return;
		}
		if (GetChunk(x, y) != null) {
			return;
		}
		MazeChunk chunk = new MazeChunk(x, y, chunkSize);
		chunks.Add(chunk);
		chunk.InitializeNodes(variability);
	}

	public bool InMaze(int x, int y) {
		return x >= 0 && x < mazeChunkWidth && y >= 0 && y < mazeChunkHeight;
	}

	public MazeNode GetNode(int x, int y) {
		int chunkX = Mathf.FloorToInt((float) x / (float) chunkSize);
		int chunkY = Mathf.FloorToInt((float) y / (float) chunkSize);
		MazeChunk chunk = GetChunk(chunkX, chunkY);
		if (chunk == null) {
			return null;
		}
		int inChunkX = 0;
		int inChunkY = 0;
		if (x != 0) {
			inChunkX = x % chunkSize;
		}
		if (y != 0) {
			inChunkY = y % chunkSize;
		}
		MazeNode node = chunk.GetNode(inChunkX, inChunkY);
		return node;
	}

	public MazePos[] GetConnectedNeighbors(MazePos pos) {
		MazeNode nodeAt = GetNode(pos.GetX(), pos.GetY());
		if (nodeAt == null) {
			return null;
		}
		List<MazePos> neighs = new List<MazePos>();
		if (pos.GetX() > 0 && !nodeAt.HasWall(MazeNode.LEFT)) {
			neighs.Add(new MazePos(pos.GetX() - 1, pos.GetY()));
		}
		if (pos.GetX() < GetSizeX() - 1 && !nodeAt.HasWall(MazeNode.RIGHT)) {
			neighs.Add(new MazePos(pos.GetX() + 1, pos.GetY()));
		}
		if (pos.GetY() > 0 && !nodeAt.HasWall(MazeNode.TOP)) {
			neighs.Add(new MazePos(pos.GetX(), pos.GetY() - 1));
		}
		if (pos.GetY() < GetSizeY() - 1 && !nodeAt.HasWall(MazeNode.BOTTOM)) {
			neighs.Add(new MazePos(pos.GetX(), pos.GetY() + 1));
		}
		return neighs.ToArray();
	}

	public IAlgorithm GetMazeGenerationAlgorithm() {
		return mazeAlgorithm;
	}

	public int GetSizeX() {
		return mazeChunkWidth * chunkSize;
	}

	public int GetSizeY() {
		return mazeChunkHeight * chunkSize;
	}

	// -- Actual Generation -- //

	public void Generate(MazeHandler handler, MazePos startingPoint) {
		PMEventSystem.GetEventSystem().TriggerEvent(new EventChunkPrePopulationBegin(this));
		InitializeChunks();
		PMEventSystem.GetEventSystem().TriggerEvent(new EventChunkPrePopulationFinish(this));
		for (int x = 0; x < mazeChunkWidth; x++) {
			for (int y = 0; y < mazeChunkWidth; y++) {
				MazeChunk chunk = GetChunk(x, y);
				PMEventSystem.GetEventSystem().TriggerEvent(new EventChunkGenerationBegin(this, chunk, x, y));
				if (chunk == null) {
					AddChunk(x, y);
				}
				if (chunk == null) {
					Debug.LogError("Chunk was null after creation: " + x + ", " + y + ". This is a major error.");
					return;
				}
				if (!chunk.IsInitialized()) {
					chunk.InitializeNodes(variability);
				}
				PMEventSystem.GetEventSystem().TriggerEvent(new EventChunkGenerationFinish(this, chunk, x, y));
			}
		}
		Debug.Log("Generating maze using: " + mazeAlgorithm.GetName());
		handler.StartCoroutine(mazeAlgorithm.Generate(handler, true, this, startingPoint));
	}

}