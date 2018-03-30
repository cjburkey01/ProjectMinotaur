using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldHandler {
	
	public static readonly string DIR_PATH = "{0}/save/world{1}";
	public static readonly string WORLD_DATA_PATH = DIR_PATH + "/worldData.save";
	public static readonly string MAZE_DATA_PATH = DIR_PATH + "/mazeData.save";

	public DataFile WorldData { private set; get; }
	public DataFile MazeData { private set; get; }
	public DataHandler Data { private set; get; }

	public void DummySave(MazeHandler mazeHandler) {
		Save((p) => { }, () => { }, mazeHandler);
	}

	public void Save(System.Action<float> progress, LoadingStepComplete done, MazeHandler mazeHandler) {
		Debug.Log("Saving world to disk...");
		if (Data == null) {
			Data = new DataHandler();
		}
		if (CreateDirIfNot(true, WorldData.File)) {
			Directory.GetParent(WorldData.Dir).Create();
			Debug.Log("Created save directory: " + WorldData.Dir);
		}
		Data.Set("CheatMode", true);
		PMEventSystem.GetEventSystem().TriggerEvent(new WorldSaveEvent(this, Data));
		Data.WriteToFile(WorldData.File);
		Debug.Log("Saved world data.");
		Debug.Log("Saving maze data to disk...");
		mazeHandler.StartCoroutine(mazeHandler.GetMaze().SaveToFile(progress, () => { Debug.Log("Saved maze data."); done.Invoke(); }, MazeData.File));
	}

	public bool Load(System.Action<float> progress, LoadingStepComplete done, MazeHandler mazeHandler, int worldId, bool trueLoad) {
		if (trueLoad && !WorldExists(worldId)) {
			Debug.LogError("World does not exist: " + worldId);
			return false;
		}
		WorldData = new DataFile(GetWorldDir(worldId), GetFormatted(WORLD_DATA_PATH, worldId));
		MazeData = new DataFile(GetWorldDir(worldId), GetFormatted(MAZE_DATA_PATH, worldId));
		Maze maze = null;
		if (trueLoad) {
			Debug.Log("Loading world data from disk...");
			Data = DataHandler.ReadFromFile(WorldData.File);
			Debug.Log("Loaded world data. Failed = " + (Data == null));
			Debug.Log("Loading maze data from disk...");
			maze = Maze.LoadFromFile(MazeData.File);
			Debug.Log("Loaded maze data. Failed = " + (maze == null));
			if (maze != null) {
				mazeHandler.Load(progress, done, maze);
			}
		}
		return (trueLoad || (Data != null && maze != null));
	}

	public bool WorldExists(int id) {
		return File.Exists(GetFormatted(WORLD_DATA_PATH, id));
	}

	public void NewWorld(System.Action<float> progress, LoadingStepComplete done, MazeHandler mazeHandler) {
		for (int i = 0; i < int.MaxValue; i ++) {
			string path = GetFormatted(WORLD_DATA_PATH, i);
			if (File.Exists(path)) {
				continue;
			}
			Debug.Log("New World ID: " + i);
			Load((p) => { }, () => { }, null, i, false);
			Save(progress, done, mazeHandler);
			return;
		}
	}

	public string GetWorldDir(int i) {
		return GetFormatted(DIR_PATH, i);
	}

	public string GetFormatted(string instr, int i) {
		return string.Format(instr, Application.persistentDataPath, i);
	}

	public DataFile GetInternalWorldFile(string name) {
		return new DataFile(WorldData.Dir, WorldData.Dir + "/" + name);
	}

	private bool CreateDirIfNot(bool file, string path) {
		if (!file) {
			Directory.CreateDirectory(path);
		}
		DirectoryInfo parent = Directory.GetParent(path);
		if (!parent.Exists) {
			CreateDirIfNot(false, parent.FullName);
			return true;
		}
		return false;
	}

}

public struct DataFile {

	public bool IsSet { private set; get; }
	public string Dir { private set; get; }
	public string File { private set; get; }

	public DataFile(string dir, string file) {
		IsSet = true;
		Dir = dir;
		File = file;
	}

	public override bool Equals(object obj) {
		if (!(obj is DataFile)) {
			return false;
		}
		var file = (DataFile) obj;
		return IsSet == file.IsSet && File == file.File && Dir == file.Dir;
	}

	public override int GetHashCode() {
		var hashCode = 175763103;
		hashCode = hashCode * -1521134295 + base.GetHashCode();
		hashCode = hashCode * -1521134295 + IsSet.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(File);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Dir);
		return hashCode;
	}

	public override string ToString() {
		return File;
	}

}

public abstract class WorldLoadingEvent : IPMEvent {

	public WorldHandler WorldHandler { private set; get; }
	public DataHandler DataHandler { private set; get; }

	protected WorldLoadingEvent(WorldHandler worldHandler, DataHandler dataHandler) {
		WorldHandler = worldHandler;
		DataHandler = dataHandler;
	}

	public string GetName() {
		return GetType().Name;
	}

	public bool IsCancellable() {
		return false;
	}

	public bool IsCancelled() {
		return false;
	}

	public void Cancel() {
	}

}

public class WorldSaveEvent : WorldLoadingEvent {

	public WorldSaveEvent(WorldHandler worldHandler, DataHandler dataHandler) : base(worldHandler, dataHandler) {
	}

}

public class WorldLoadEvent : WorldLoadingEvent {

	public WorldLoadEvent(WorldHandler worldHandler, DataHandler dataHandler) : base(worldHandler, dataHandler) {
	}

}