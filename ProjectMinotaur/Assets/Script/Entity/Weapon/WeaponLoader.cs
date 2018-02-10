using System;
using UnityEngine;
using SimpleJSON;

public static class WeaponLoader {

	/// <summary>
	///		This will load a weapon's information from their own .json definition file.
	///		The file, does not, however contain any information about custom executions,
	///		those must be done manually in C# once the weapon has been loaded.
	/// </summary>
	/// <param name="path">The path to the weapon, the weapon's .json file *must* be in a /Resources/ folder *somewhere*</param>
	/// <returns></returns>
	public static WeaponDefinition LoadWeapon(string path) {
		try {
			TextAsset file = Resources.Load<TextAsset>(path);
			if (file == null) {
				Debug.LogError("Failed to load weapon .json file: " + path);
				return null;
			}
			JSONNode json = JSON.Parse(file.text);

			// These are hard coded in...for now, hehehe >:D
			string name = json["name"].Value;
			string description = json["description"].Value;
			bool isPrimary = json["is_primary"].AsBool;
			float resetTime = json["reset_time"].AsFloat;
			int damage = json["damage"].AsInt;
			float maxDistance = json["max_distance"].AsFloat;
			float spray = json["spray"].AsFloat;
			int ammoPerClip = json["ammo_per_clip"].AsInt;
			int shotsPerPrimary = json["shots_per_primary"].AsInt;
			Vector3 displayPositionOffset = LoadVector3(json, "display_position_offset");
			Vector3 displayRotationOffset = LoadVector3(json, "display_rotation_offset");
			Vector3 barrelPositionOffset = LoadVector3(json, "barrel_position_offset");
			string modelPath = json["model_path"].Value;
			string iconPath = json["icon_path"].Value;
			bool trail = json["draw_trail"].AsBool;
			bool auto = json["full_auto"].AsBool;
			float recoilTime = json["recoil_time"].AsFloat;
			float recoilX = json["recoil_x"].AsFloat;
			float recoilY = json["recoil_y"].AsFloat;
			float recoilSpeed = json["recoil_speed"].AsFloat;

			return new WeaponDefinition(path, name, description, isPrimary, resetTime, damage, maxDistance, spray, ammoPerClip, shotsPerPrimary, displayPositionOffset, displayRotationOffset, barrelPositionOffset, modelPath, iconPath, trail, auto, recoilTime, recoilX, recoilY, recoilTime);
		} catch (Exception e) {
			Debug.LogError("An error occurred while reading the weapon JSON: " + e.Message);
		}
		return null;
	}

	private static Vector3 LoadVector3(JSONNode json, string name) {
		try {
			return new Vector3(json[name]["x"].AsFloat, json[name]["y"].AsFloat, json[name]["z"].AsFloat);
		} catch (Exception e) {
			throw e;
		}
	}

}