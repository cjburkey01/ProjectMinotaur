public abstract class ItemEvent : IPMEvent {

	public Maze Maze { private set; get; }

	protected ItemEvent(Maze maze) {
		Maze = maze;
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

}

// -- EVENTS -- //

public class ItemBeginEvent : ItemEvent {

	public ItemBeginEvent(Maze maze) : base(maze) {
	}

}

public class ItemSpawnEvent : ItemEvent {

	public WorldItem Item { private set; get; }
	public float Progress { private set; get; }

	public ItemSpawnEvent(Maze maze, WorldItem world, float progress) : base(maze) {
		Item = world;
		Progress = progress;
	}

}

public class ItemFinishEvent : ItemEvent {

	public ItemFinishEvent(Maze maze) : base(maze) {
	}

}