using System;

namespace EventBusSystem
{
	public interface IEventBus
	{
		void Fire<T>(T eventData);

		IDisposable Subscribe<T>(Action<T> eventHandler);
		void Unsubscribe<T>(Action<T> eventHandler);

		IDisposable Subscribe(Type eventType, Delegate handler);
		void Unsubscribe(Type eventType, Delegate handler);

		void Subscribe(IEventBusSubscriber subscriber);
		void Unsubscribe(IEventBusSubscriber subscriber);
	}
}
