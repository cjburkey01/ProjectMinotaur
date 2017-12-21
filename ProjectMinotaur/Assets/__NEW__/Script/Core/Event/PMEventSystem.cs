using System;
using System.Collections.Generic;

public delegate void PMEventListener<T>(T eventObj) where T : IPMEvent;

public class PMEventSystem {

	private static PMEventSystem instance;

	private Dictionary<Type, object> eventHandlers;

	// Initialize handler list.
	protected PMEventSystem() {
		eventHandlers = new Dictionary<Type, object>();
	}

	// Registers an event handler for the event of type T (generic type).
	protected EventHandler<T> RegisterEventHandler<T>() where T : IPMEvent {
		if (eventHandlers == null) {
			return null;
		}
		if (eventHandlers.ContainsKey(typeof(T))) {
			return null;
		}
		EventHandler<T> handler = new EventHandler<T>();
		eventHandlers.Add(typeof(T), handler);
		return handler;
	}

	// Finds the event handler for the passed type.
	protected EventHandler<T> GetHandler<T>() where T : IPMEvent {
		if (eventHandlers == null) {
			return null;
		}
		object obj;
		if (!eventHandlers.TryGetValue(typeof(T), out obj)) {
			return RegisterEventHandler<T>();
		}
		return obj as EventHandler<T>;
	}

	// Triggers the specified event, passing the data in the event object.
	public void TriggerEvent<T>(T e) where T : IPMEvent {
		if (eventHandlers == null) {
			return;
		}
		if (ReferenceEquals(e, null)) {
			return;
		}
		EventHandler<T> handler = GetHandler<T>();
		if (handler == null) {
			return;
		}
		handler.TriggerEvent(e);
	}

	// Registers a listener with the respective event handler.
	public void AddListener<T>(PMEventListener<T> listener) where T : IPMEvent {
		if (eventHandlers == null) {
			return;
		}
		if (ReferenceEquals(listener, null)) {
			return;
		}
		EventHandler<T> handler = GetHandler<T>();
		if (handler == null) {
			return;
		}
		handler.RegisterListener(listener);
	}

	public void RemoveListener<T>(PMEventListener<T> listener) where T : IPMEvent {
		if (eventHandlers == null) {
			return;
		}
		if (ReferenceEquals(listener, null)) {
			return;
		}
		EventHandler<T> handler = GetHandler<T>();
		if (handler == null) {
			return;
		}
		handler.RemoveListener(listener);
	}

	// Get the current event system via a static reference, makes this object easier to access.
	public static PMEventSystem GetEventSystem() {
		if (instance == null) {
			instance = new PMEventSystem();
		}
		return instance;
	}

}