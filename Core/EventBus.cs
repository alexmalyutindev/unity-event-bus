using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace EventBusSystem
{
	public class EventBus : IEventBus
	{
		private readonly List<IEventBusSubscriber> _subscribers = new List<IEventBusSubscriber>();
		private readonly ConcurrentDictionary<Type, Delegate> _handlers = new ConcurrentDictionary<Type, Delegate>();

		public void Fire<T>(T message)
		{
			if (_handlers.TryGetValue(typeof(T), out var handler))
			{
				try
				{
					((Action<T>) handler)?.Invoke(message);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
			}

			foreach (var subscriber in _subscribers)
			{
				try
				{
					subscriber.HandleEvent(message);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
			}
		}

		public IDisposable Subscribe<T>(Action<T> handler)
		{
			_handlers.AddOrUpdate(
				typeof(T),
				handler,
				(_, d) => (Action<T>) d + handler
			);

			return new Unsubscriber(() => Unsubscribe(handler));
		}

		public void Unsubscribe<T>(Action<T> handler) =>
			_handlers.AddOrUpdate(
				typeof(T),
				default(Delegate),
				(_, d) => (Action<T>) d - handler
			);

		public IDisposable Subscribe(Type eventType, Delegate handler)
		{
			_handlers.AddOrUpdate(
				eventType,
				handler,
				(_, d) => Delegate.Combine(d, handler)
			);

			return new Unsubscriber(() => Unsubscribe(eventType, handler));
		}

		public void Unsubscribe(Type eventType, Delegate handler) =>
			_handlers.AddOrUpdate(
				eventType,
				default(Delegate),
				(_, d) => Delegate.Remove(d, handler)
			);

		public void Subscribe(IEventBusSubscriber subscriber) => _subscribers.Add(subscriber);

		public void Unsubscribe(IEventBusSubscriber subscriber) => _subscribers.Remove(subscriber);

		private class Unsubscriber : IDisposable
		{
			private readonly Action _action;
			public Unsubscriber(Action action) => _action = action;
			public void Dispose() => _action?.Invoke();
		}
	}
}
