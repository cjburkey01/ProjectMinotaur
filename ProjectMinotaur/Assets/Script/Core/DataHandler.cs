using System;
using System.IO;
using System.Collections.Generic;

public class DataHandler {

	public static readonly string SEPARATOR = "=";
	public static readonly string DATA_FORMAT_VERSION = "0";

	public static readonly DataType TYPE_BYTE = new DataTypeByte();
	public static readonly DataType TYPE_INT = new DataTypeInt();
	public static readonly DataType TYPE_LONG = new DataTypeLong();
	public static readonly DataType TYPE_FLOAT = new DataTypeFloat();
	public static readonly DataType TYPE_DOUBLE = new DataTypeDouble();
	public static readonly DataType TYPE_STRING = new DataTypeString();
	public static readonly DataType TYPE_BOOL = new DataTypeBool();
	public static readonly DataType TYPE_VEC3 = new DataTypeVector3();
	public static readonly List<DataType> TYPES = new List<DataType>() { TYPE_BYTE, TYPE_INT, TYPE_LONG, TYPE_FLOAT, TYPE_DOUBLE, TYPE_STRING, TYPE_BOOL, TYPE_VEC3 };

	private readonly Dictionary<string, TypeValuePair> data;

	public DataHandler() : this(new Dictionary<string, TypeValuePair>()) {
	}

	protected DataHandler(Dictionary<string, TypeValuePair> dict) {
		data = dict;
	}

	public void Set(string key, object value, DataType type) {
		if (key == null) {
			UnityEngine.Debug.LogError("Null key for data " + value + " of type " + type.Id);
			return;
		}
		if (type == null || value == null) {
			UnityEngine.Debug.LogError("Cannot set " + key + " as null");
			return;
		}
		SetPair(key, new TypeValuePair(type, value));
	}
	
	// Return of 'False' means an invalid type.
	public bool Set(string key, object value) {
		if (value == null) {
			return false;
		}
		foreach (DataType type in TYPES) {
			if (type.GetDataType().Equals(value.GetType())) {
				Set(key, value, type);
				return true;
			}
		}
		UnityEngine.Debug.LogError("Failed to determine type for " + key + " as " + value);
		return false;
	}

	protected void SetPair(string key, TypeValuePair pair) {
		if (key == null || pair == null || pair.value == null || pair.type == null) {
			UnityEngine.Debug.LogError("Invalid data");
			return;
		}
		if (data.ContainsKey(key)) {
			data[key] = pair;
		} else {
			data.Add(key, pair);
		}
	}

	public void SetAll(DataHandler data) {
		foreach (KeyValuePair<string, TypeValuePair> dataPiece in data.data) {
			SetPair(dataPiece.Key, dataPiece.Value);
		}
	}

	public bool Has(string key) {
		return data.ContainsKey(key);
	}

	public object Get(string key) {
		if (!data.ContainsKey(key)) {
			return null;
		}
		TypeValuePair ret;
		if (!data.TryGetValue(key, out ret) || ret == null || ret.value == null || ret.type == null) {
			return null;
		}
		return ret.type.GetValue(ret.value.ToString());
	}

	public T Get<T>(string key, T def) {
		object at = Get(key);
		if (!(at is T)) {
			return def;
		}
		if (at == null || at.Equals(def)) {
			return def;
		}
		return (T) at;
	}

	public DataHandler Copy() {
		return new DataHandler(data);
	}

	public override bool Equals(object obj) {
		var data = obj as DataHandler;
		return data != null && EqualityComparer<Dictionary<string, TypeValuePair>>.Default.Equals(this.data, data.data);
	}

	public override int GetHashCode() {
		return 1768953197 + EqualityComparer<Dictionary<string, TypeValuePair>>.Default.GetHashCode(data);
	}

	public override string ToString() {
		string back = "[";
		foreach (KeyValuePair<string, TypeValuePair> value in data) {
			back += value.Key + "=" + ((value.Value == null || value.Value.value == null) ? "null" : value.Value.value.ToString()) + ",";
		}
		if(data.Count > 0) {
			back = back.Substring(0, back.Length - 1);
		}
		return back + ']';
	}

	public void WriteToFile(string file) {
		List<string> lines = new List<string> { "v_\t" + DATA_FORMAT_VERSION, "s_\t" + SEPARATOR };
		foreach (KeyValuePair<string, TypeValuePair> pairs in data) {
			lines.Add(pairs.Key + SEPARATOR + pairs.Value.ToString());
		}
		File.WriteAllLines(file, lines.ToArray());
	}

	public static DataHandler ReadFromFile(string file) {
		if (!File.Exists(file)) {
			return null;
		}
		string[] lines = File.ReadAllLines(file);
		DataHandler ret = new DataHandler();
		string[] separator = new string[] { SEPARATOR };
		string version = DATA_FORMAT_VERSION;
		for (int i = 0; i < lines.Length; i ++) {
			string line = lines[i];
			if (i == 0) {
				if (line.StartsWith("v_\t")) {
					version = line.Substring("v_\t".Length);
					continue;
				}
			} else if (i == 1) {
				if (line.StartsWith("s_\t")) {
					separator[0] = line.Substring("s_\t".Length);
					continue;
				}
			}
			if (version != DATA_FORMAT_VERSION) {
				UnityEngine.Debug.LogError("Unable to load world, file version does not match.");
				return null;
			}
			string[] spl = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			if (spl.Length != 3) {
				UnityEngine.Debug.LogError("Failed to parse line: " + line);
				continue;
			}
			DataType type = null;
			foreach (DataType inType in TYPES) {
				if (inType.Id.Equals(spl[1])) {
					type = inType;
					break;
				}
			}
			if (type == null) {
				UnityEngine.Debug.LogError("Failed to determine type: " + spl[1] + " in line: " + line);
				continue;
			}
			ret.Set(spl[0], type.GetValue(spl[2]), type);
		}
		return ret;
	}

}

public class TypeValuePair {
	
	public DataType type;
	public object value;

	public TypeValuePair() {
	}

	public TypeValuePair(DataType type, object value) {
		this.type = type;
		this.value = value;
	}
	
	public override bool Equals(object obj) {
		var pair = obj as TypeValuePair;
		return pair != null && EqualityComparer<DataType>.Default.Equals(type, pair.type) && EqualityComparer<object>.Default.Equals(value, pair.value);
	}

	public override int GetHashCode() {
		var hashCode = 1265339359;
		hashCode = hashCode * -1521134295 + EqualityComparer<DataType>.Default.GetHashCode(type);
		hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(value);
		return hashCode;
	}

	public override string ToString() {
		return type.Id + DataHandler.SEPARATOR + value.ToString();
	}

}

public abstract class DataType {
	
	public string Id { private set; get; }

	protected DataType(string id) {
		Id = id;
	}

	public abstract Type GetDataType();
	public abstract object GetValue(string data);

}

public class DataTypeByte : DataType {

	public DataTypeByte() : base("byte") {
	}

	public override Type GetDataType() {
		return typeof(byte);
	}

	public override object GetValue(string data) {
		byte ret;
		if (!byte.TryParse(data.Trim(), out ret)) {
			return byte.MinValue;
		}
		return ret;
	}

}

public class DataTypeInt : DataType {

	public DataTypeInt() : base("int") {
	}

	public override Type GetDataType() {
		return typeof(int);
	}

	public override object GetValue(string data) {
		int ret;
		if (!int.TryParse(data.Trim(), out ret)) {
			return int.MinValue;
		}
		return ret;
	}

}

public class DataTypeLong : DataType {

	public DataTypeLong() : base("long") {
	}

	public override Type GetDataType() {
		return typeof(long);
	}

	public override object GetValue(string data) {
		long ret;
		if (!long.TryParse(data.Trim(), out ret)) {
			return long.MinValue;
		}
		return ret;
	}

}

public class DataTypeFloat : DataType {

	public DataTypeFloat() : base("float") {
	}

	public override Type GetDataType() {
		return typeof(float);
	}

	public override object GetValue(string data) {
		float ret;
		if (!float.TryParse(data.Trim(), out ret)) {
			return float.MinValue;
		}
		return ret;
	}

}

public class DataTypeDouble : DataType {

	public DataTypeDouble() : base("double") {
	}

	public override Type GetDataType() {
		return typeof(double);
	}

	public override object GetValue(string data) {
		double ret;
		if (!double.TryParse(data.Trim(), out ret)) {
			return double.MinValue;
		}
		return ret;
	}

}

public class DataTypeString : DataType {

	public DataTypeString() : base("string") {
	}

	public override Type GetDataType() {
		return typeof(string);
	}

	public override object GetValue(string data) {
		return data.Trim();
	}

}

public class DataTypeBool : DataType {

	public DataTypeBool() : base("bool") {
	}

	public override Type GetDataType() {
		return typeof(bool);
	}

	public override object GetValue(string data) {
		bool ret;
		if (!bool.TryParse(data.Trim(), out ret)) {
			return false;
		}
		return ret;
	}

}

public class DataTypeVector3 : DataType {

	public DataTypeVector3() : base("vec3") {
	}

	public override Type GetDataType() {
		return typeof(UnityEngine.Vector3);
	}

	public override object GetValue(string data) {
		string[] spl = data.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
		if (spl.Length != 3) {
			return UnityEngine.Vector3.zero;
		}
		float x, y, z;
		if (!float.TryParse(spl[0].Trim(), out x) || !float.TryParse(spl[1].Trim(), out y) || !float.TryParse(spl[2].Trim(), out z)) {
			return UnityEngine.Vector3.zero;
		}
		return new UnityEngine.Vector3(x, y, z);
	}

}