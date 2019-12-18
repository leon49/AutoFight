using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using DG.Tweening;

public class TimerManager : BehaviourSingleton<TimerManager>
{

    public delegate void OnTimer(Timer timer);
    public delegate void OnComlete();

    private void Start()
    {
    }

    public Timer addTimer(long delayMilliSeconds, OnTimer callback, MonoBehaviour liveWith = null)
    {
        return addTimer(delayMilliSeconds, 0, callback, liveWith != null ? liveWith.gameObject : null);
    }
    /// <summary>
    /// 添加延迟时间执行
    /// </summary>
    /// <param name="delaySeconds"></param>
    /// <param name="callback"></param>
    /// <param name="liveWith"></param>
    /// <returns></returns>
    public Timer addTimer(long delayMilliSeconds, OnTimer callback, GameObject liveWith )
    {
        return addTimer(delayMilliSeconds, 0, callback, liveWith);
    }
    public Timer addTimer(long delayMilliSeconds, int repeatTimes, OnTimer callback, GameObject liveWith = null)
    {
        GameObject t = new GameObject();
        t.name = "timer_old";
#if UNITY_EDITOR
        t.name = callback.Method.ReflectedType.Name + "-" + callback.Method.Name;
#endif
        Timer timer = t.AddComponent<Timer>();
        timer.startTimes = SafeTime.GetMilliseconds();
        timer.delayMilliSeconds = delayMilliSeconds;
        timer.delaySum = delayMilliSeconds;
        timer.repeatTimes = repeatTimes;
        timer.callback = callback;
#if UNITY_EDITOR
        timer.name = t.name;
#endif

        if (liveWith != null)
        {
            t.transform.SetParent(liveWith.transform);
        }
        else
        {
            t.transform.SetParent(gameObject.transform);
        }
        return timer;
    }

    public void removeTimer(Timer timer)
    {
        if (timer != null && timer.isStoped == false)
        {
            timer.isStoped = true;
            Destroy(timer.gameObject);
        }
    }
}
