using System.Collections.Generic;

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