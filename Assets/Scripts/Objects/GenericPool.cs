using System;
using System.Collections.Generic;

public class GenericPool<T>
{
    private Queue<T> _objects;
    private Func<object[], T> _objectGenerator;

    public GenericPool(Func<object[], T> objectGenerator)
    {
        _objects = new Queue<T>();
        _objectGenerator = objectGenerator;
    }

    public int Length
    {
        get { return _objects.Count; }
    }

    public T GetObject(params object[] parameters)
    {
        T item = default;

        if (Length > 0)
        {
            item = _objects.Dequeue();
        }
        else
        {
            item = _objectGenerator(parameters);
        }

        return item;
    }

    public void ReleaseObject(T obj)
    {
        _objects.Enqueue(obj);
    }

    public void ClearPool()
    {
        while (Length > 0)
        {
            _objects.Dequeue();
        }
    }
}