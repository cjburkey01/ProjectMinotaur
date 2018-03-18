using System;
using System.IO;
using UnityEngine;

public class WorldHandler {

	public static readonly string FILE_PATH = "{0}/save/world{1}.save";
	
	private string saveFile;
	public DataHandler Data { private set; get; }

	public void Save() {
		Debug.Log("Saving world to disk...");
		if (Data == null) {
			Data = new DataHandler();
		}
		if (!File.Exists(saveFile) && !Directory.GetParent(saveFile).Exists) {
			Directory.GetParent(saveFile).Create();
			Debug.Log("Created save directory: " + Directory.GetParent(saveFile).FullName);
		}
		Data.Set("CheatMode", true);
		PMEventSystem.GetEventSystem().TriggerEvent(new WorldSaveEvent(this, Data));
		Data.WriteToFile(saveFile);
		Debug.Log("Saved.");
	}

	public void Load(string saveFile) {
		Debug.Log("Loading world from disk...");
		this.saveFile = saveFile;
		Data = DataHandler.ReadFromFile(saveFile);
		Debug.Log("Loaded. Null? " + (Data == null));
	}

	public void NewWorld() {
		for (int i = 0; i < int.MaxValue; i ++) {
			string path = string.Format(FILE_PATH, Application.persistentDataPath, i);
			if (File.Exists(path)) {
				continue;
			}
			saveFile = path;
			Debug.Log("Creating new world at: " + path);
			Save();
			Load(path);
			return;
		}
	}

	public string GetSaveFile() {
		return saveFile;
	}

	private bool ByteArrayToFile(string fileName, byte[] byteArray) {
		try {
			using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write)) {
				fs.Write(byteArray, 0, byteArray.Length);
				return true;
			}
		} catch (Exception ex) {
			Debug.LogError(string.Format("Failed to write world bytes to data file: {0}", ex.Message));
			return false;
		}
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