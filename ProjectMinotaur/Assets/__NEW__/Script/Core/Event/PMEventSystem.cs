using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void PMEventListener<T>(T eventObj) where T : IPMEvent;

public class PMEventSystem {

	private static PMEventSystem instance;

	private Dictionary<Type, object> eventHandlers;

	// Initialize variables.
	public PMEventSystem() {
		eventHandlers = new Dictionary<Type, object>();
	}

	// Registers an event handler for the event of type T (generic type).
	public EventHandler<T> RegisterEventHandler<T>() where T : IPMEvent {
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

	// Finds the event handler for the passed type.
	public EventHandler<T> GetHandler<T>() where T : IPMEvent {
		if (eventHandlers == null) {
			return null;
		}
		object obj;
		if (!eventHandlers.TryGetValue(typeof(T), out obj)) {
			return RegisterEventHandler<T>();
		}
		return obj as EventHandler<T>;
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

	// Get the current event system via a static reference, makes this object easier to access.
	public static PMEventSystem GetEventSystem() {
		if (instance == null) {
			instance = new PMEventSystem();
		}
		return instance;
	}

}

public class EventHandler<T> where T : IPMEvent {

	private List<PMEventListener<T>> listeners;

	// Initialize the list.
	public EventHandler() {
		listeners = new List<PMEventListener<T>>();
	}

	// Registers the supplied method as a listener for this event.
	public void RegisterListener(PMEventListener<T> listener) {
		if (listeners == null) {
			return;
		}
		if (!listeners.Contains(listener)) {
			listeners.Add(listener);
		}
	}

	// Removes the supplied event from the event listeners.
	public void RemoveListener(PMEventListener<T> listener) {
		if (listeners == null) {
			return;
		}
		if (listeners.Contains(listener)) {
			listeners.Remove(listener);
		}
	}

	// Calls an event for all listeners, or until the event is cancelled if it is cancellable.
	public void TriggerEvent(T eventObj) {
		if (ReferenceEquals(eventObj, null)) {
			return;
		}
		foreach (PMEventListener<T> listener in listeners) {
			if (eventObj.IsCancellable() && eventObj.IsCancelled()) {
				return;
			}
			listener.Invoke(eventObj);
		}
	}

}

public interface IPMEvent {

	// Returns the name of the event.
	string GetName();

	// Returns whether or not the event may be cancelled. This should probably be false, usually.
	bool IsCancellable();

	// Returns whether or not the event has been cancelled. Only does anything if IsCancellable() is true.
	bool IsCancelled();

	// Cancels the event, which prevents propogation. Only does anything if IsCancellable() is true.
	void Cancel();

}