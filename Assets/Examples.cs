using System;
using Core.Systems.Events;
using UnityEngine;

namespace DefaultNamespace
{
	public class EventA { }

	public class Example1
	{
		public class Bar
		{
			public Bar(IEventBus eventBus)
			{
				eventBus.Fire(new EventA());
			}
		}

		public class Foo : IDisposable
		{
			private IDisposable _sub;

			public Foo(IEventBus eventBus)
			{
				_sub = eventBus.Subscribe<EventA>(e => Debug.Log(e));
			}

			public void Dispose()
			{
				_sub.Dispose();
			}
		}
	}

	public class Example2
	{
		public class Foo : IEventBusSubscriber, IDisposable
		{
			private readonly IEventBus _eventBus;

			public Foo(IEventBus eventBus)
			{
				_eventBus = eventBus;
				_eventBus.Subscribe(this);
			}

			public void HandleEvent<T>(T e)
			{
				switch (e)
				{
					case EventA a:
						//...
					break;
				}
			}

			public void Dispose()
			{
				_eventBus.Unsubscribe(this);
			}
		}
	}
}
