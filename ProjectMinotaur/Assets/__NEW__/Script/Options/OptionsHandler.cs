using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class OptionsHandler : MonoBehaviour {

	public static OptionsHandler Instance { private set; get; }

	private readonly Dictionary<string, string> options = new Dictionary<string, string>();
	private FileInfo file;

	void Start() {
		Instance = this;
		options.Clear();
		file = new FileInfo(Application.persistentDataPath + "/options.txt");
		Load();
	}

	public bool Has(string key) {
		return options.ContainsKey(key);
	}

	public void Set(string key, object val) {
		string str = (val == null) ? "null" : val.ToString();
		if (options.ContainsKey(key)) {
			options[key] = str;
		} else {
			options.Add(key, str);
		}
		Save();
	}

	public string Get(string key) {
		Load();
		string outs = null;
		if (!options.TryGetValue(key, out outs)) {
			return null;
		}
		return outs;
	}

	public int GetInt(string key) {
		string found = Get(key);
		if (found == null) {
			return int.MinValue;
		}
		int parsed;
		if (!int.TryParse(found, out parsed)) {
			return int.MinValue;
		}
		return parsed;
	}

	public float GetFloat(string key) {
		string found = Get(key);
		if (found == null) {
			return float.MinValue;
		}
		float parsed;
		if (!float.TryParse(found, out parsed)) {
			return float.MinValue;
		}
		return parsed;
	}

	public long GetLong(string key) {
		string found = Get(key);
		if (found == null) {
			return long.MinValue;
		}
		long parsed;
		if (!long.TryParse(found, out parsed)) {
			return long.MinValue;
		}
		return parsed;
	}

	public double GetDouble(string key) {
		string found = Get(key);
		if (found == null) {
			return double.MinValue;
		}
		double parsed;
		if (!double.TryParse(found, out parsed)) {
			return double.MinValue;
		}
		return parsed;
	}

	public bool GetBool(string key) {
		string found = Get(key);
		if (found == null) {
			return false;
		}
		bool parsed;
		if (!bool.TryParse(found, out parsed)) {
			return false;
		}
		return parsed;
	}

	// -- DATA -- //

	private void Load() {
		if (!file.Exists) {
			return;
		}
		string ins = File.ReadAllText(file.FullName);
		string[] lines = ins.Split('\n');
		foreach (string line in lines) {
			string[] split = line.Split('=');
			if (split.Length == 2 && !options.ContainsKey(split[0])) {
				options.Add(split[0], split[1]);
			}
		}
	}

	private void Save() {
		if (file.Exists) {
			file.Delete();
		}
		string outs = "";
		foreach (KeyValuePair<string, string> entry in options) {
			outs += entry.Key + "=" + entry.Value + "\n";
		}
		File.WriteAllText(file.FullName, outs);
	}

}