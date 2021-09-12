Unity Event Bus system. 
==========

[![Build](https://github.com/alexmalyutindev/unity-event-bus/actions/workflows/upm-ci.yml/badge.svg)](https://github.com/alexmalyutindev/unity-event-bus/actions/workflows/upm-ci.yml)
[![Release](https://img.shields.io/github/v/release/alexmalyutindev/unity-event-bus)](https://github.com/alexmalyutindev/unity-event-bus/releases)

Examples
--------
- Single subscription:
```c#
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
```
- Bus subscription using `IEventBusSubscriber` interface:
```c#
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
```
- Fire events:
```c#
eventBus.Fire(new EventA());
```
Installation
------------
Find the manifest.json file in the Packages folder of your project and add a line to `dependencies` field:

* `"com.alexmalyutindev.event-bus": "https://github.com/alexmalyutindev/unity-event-bus.git#latest"`

Or, you can add this package using PackageManager `Add package from git URL` option:

* `https://github.com/alexmalyutindev/unity-event-bus.git#latest`
