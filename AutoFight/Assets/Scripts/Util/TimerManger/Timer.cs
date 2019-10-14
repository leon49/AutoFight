using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
	public int curRepeatTimes = 0;
	public int repeatTimes = 1;
	public long startTimes = 0;
	public long delayMilliSeconds;
	public long delaySum = 0;
	internal TimerManager.OnTimer callback;

	internal object privateData;
	public string name = "";
	//internal IEnumerator looper;
	public bool isStoped = false;
	//internal MonoBehaviour liveWith = null;

	public void setPrivateData<T>(T privateData)
	{
		this.privateData = privateData;
	}
	public T getPrivateData<T>()
	{
		return (T)privateData;
	}
	public override bool Equals(object obj)
	{
		return this == obj;
	}

	public void Update()
	{
		long curTime = SafeTime.GetMilliseconds();
		long delay = startTimes + delaySum - curTime;
		if(delay > 0)
		{
			return;
		}
		
		if (isStoped)
		{
			Destroy(gameObject);
			return;
		}

		if (callback != null && callback.Target != null && callback.Target.ToString().Equals("null") == false)
		{
			try
			{
				callback(this);
			}
			catch(System.Exception e)
			{
				Debug.LogError(e);
			}
		}

		if (repeatTimes != -1)
		{
			if(	curRepeatTimes++ >= repeatTimes)
			{
				Destroy(gameObject);
			}
		}
		else
		{
			delaySum += delayMilliSeconds;
		}
	}
}
