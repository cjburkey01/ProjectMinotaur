using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void PMEventListener(Type eventType, PMEvent eventObj);

public class PMEventSystem : MonoBehaviour {

	private static PMEventSystem instance;

	private Dictionary<Type, List<PMEventListener>> listeners;

	void Start() {
		instance = this;
		listeners = new Dictionary<Type, List<PMEventListener>>();
	}

	public bool RegisterListener(Type e, PMEventListener listener) {
		if (!e.IsSubclassOf(typeof(PMEvent))) {
			return false;
		}
		List<PMEventListener> inSlot;
		if (!listeners.TryGetValue(e, out inSlot) || inSlot == null) {
			inSlot = new List<PMEventListener>();
		}
		inSlot.Add(listener);
		listeners.Add(e, inSlot);
		return true;
	}

	public void TriggerEvent(PMEvent e) {

	}

	public static PMEventSystem GetEventSystem() {
		return instance;
	}

}