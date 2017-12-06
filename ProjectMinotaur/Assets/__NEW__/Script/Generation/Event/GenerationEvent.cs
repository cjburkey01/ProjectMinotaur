public abstract class MazeGenerationEvent : IPMEvent {

	private readonly Maze maze;

	protected MazeGenerationEvent(Maze maze) {
		this.maze = maze;
	}

	public string GetName() {
		return GetType().Name;
	}

	public virtual bool IsCancellable() {
		return false;
	}

	public virtual bool IsCancelled() {
		return false;
	}

	public virtual void Cancel() {
	}

	public Maze GetMaze() {
		return maze;
	}

}

public abstract class MazeChunkGenerationEvent : MazeGenerationEvent {

	private readonly MazeChunk chunk;
	private readonly int x;
	private readonly int y;

	protected MazeChunkGenerationEvent(Maze maze, MazeChunk chunk, int x, int y) : base(maze) {
		this.chunk = chunk;
		this.x = x;
		this.y = y;
	}

	public MazeChunk GetChunk() {
		return chunk;
	}

	public int GetX() {
		return x;
	}

	public int GetY() {
		return y;
	}

}

// -- EVENTS -- //

public class EventChunkPrePopulationBegin : MazeGenerationEvent {

	public EventChunkPrePopulationBegin(Maze maze) : base(maze) {
	}

}

public class EventChunkPrePopulationFinish : MazeGenerationEvent {

	public EventChunkPrePopulationFinish(Maze maze) : base(maze) {
	}

}

public class EventMazeGenerationBegin : MazeGenerationEvent {

	public EventMazeGenerationBegin(Maze maze) : base(maze) {
	}

}

public class EventMazeGenerationUpdate : MazeGenerationEvent {

	private readonly int progress;

	public EventMazeGenerationUpdate(Maze maze, int progress) : base(maze) {
		this.progress = progress;
	}

	public int GetProgress() {
		return progress;
	}

}

public class EventMazeGenerationFinish : MazeGenerationEvent {

	public EventMazeGenerationFinish(Maze maze) : base(maze) {
	}

}

public class EventChunkGenerationBegin : MazeChunkGenerationEvent {

	public EventChunkGenerationBegin(Maze maze, MazeChunk chunk, int x, int y) : base(maze, chunk, x, y) {
	}

}

public class EventChunkGenerationFinish : MazeChunkGenerationEvent {

	public EventChunkGenerationFinish(Maze maze, MazeChunk chunk, int x, int y) : base(maze, chunk, x, y) {
	}

}