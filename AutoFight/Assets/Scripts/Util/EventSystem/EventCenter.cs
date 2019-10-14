using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter : Singleton<EventCenter>
{
    public delegate void EventDelegate<T>(T e) where T : BaseEvent;
    protected delegate void EventDelegate(BaseEvent e);
    
    protected Dictionary<Type,EventDelegate> delegates = new Dictionary<Type, EventDelegate>();
    private Dictionary<Delegate,EventDelegate> delegateLookup = new Dictionary<Delegate, EventDelegate>();

    public void AddListener<T>(EventDelegate<T> del) where T : BaseEvent
    {
        if (delegateLookup.ContainsKey(del))
            return;
        EventDelegate CurDelegate = (e) => del((T) e);
        delegateLookup[del] = CurDelegate;
        EventDelegate tempDel;
        if (delegates.TryGetValue(typeof(T),out tempDel))
        {
            delegates[typeof(T)] = tempDel += CurDelegate;
        }
        else
        {
            delegates[typeof(T)] = CurDelegate;
        }
    }

    public void AddListener(Type type, EventDelegate<BaseEvent> del)
    {
        if(delegateLookup.ContainsKey(del))
            return;
        EventDelegate CurDelegate = (e) => del(e);
        delegateLookup[del] = CurDelegate;

        EventDelegate tempDel;
        if (delegates.TryGetValue(type,out tempDel))
        {
            delegates[type] = tempDel += CurDelegate;
        }
        else
        {
            delegates[type] = CurDelegate;
        }
    }

    public void RemoveListener(Type type, EventDelegate<BaseEvent> del)
    {
        EventDelegate CurDelegate;
        if (delegateLookup.TryGetValue(del,out CurDelegate))
        {
            EventDelegate TempDel;
            if (delegates.TryGetValue(type,out TempDel))
            {
                TempDel -= CurDelegate;
                if (TempDel == null)
                {
                    delegates.Remove(type);
                }
                else
                {
                    delegates[type] = TempDel;
                }
            }
            delegateLookup.Remove(del);
        }
    }
    public void RemoveListener<T>(EventDelegate<T> del) where T : BaseEvent
    {
        EventDelegate CurDelegate;
        if (delegateLookup.TryGetValue(del,out CurDelegate))
        {
            EventDelegate TempDel;
            if (delegates.TryGetValue(typeof(T),out TempDel))
            {
                TempDel -= CurDelegate;
                if (TempDel == null)
                {
                    delegates.Remove(typeof(T));
                }
                else
                {
                    delegates[typeof(T)] = TempDel;
                }
            }
            delegateLookup.Remove(del);
        }
    }

    public void Raise(BaseEvent e)
    {
        EventDelegate del;
        if (delegates.TryGetValue(e.GetType(),out del))
        {
            del.Invoke(e);
        }
    }
}
