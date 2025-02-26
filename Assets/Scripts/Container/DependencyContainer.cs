using System.Collections.Generic;
using System;

public class DependencyContainer
{
    private Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public void Register<T>(T implementation)
    {
        _services[typeof(T)] = implementation;
    }

    public T Resolve<T>()
    {
        if (_services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }

        throw new InvalidOperationException($"Service of type {typeof(T)} is not registered.");
    }
}
