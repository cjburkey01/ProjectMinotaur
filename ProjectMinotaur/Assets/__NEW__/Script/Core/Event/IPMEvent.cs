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