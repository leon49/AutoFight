using System;

public class SafeTime
{
    static long startupTick = DateTime.Now.Ticks;
    const float TicksPerSecond = 10000000f;
    static long unixBaseMillis = new DateTime(1970, 1, 1, 0, 0, 0).ToFileTimeUtc() / 10000;
    public static float realtimeSinceStartup { get
        {
            return (DateTime.Now.Ticks - startupTick) / TicksPerSecond;
        }
    }

    public static long GetSystemTime()
    {
        long seconds = GetUnixTimeStamp(DateTime.Now);
        return seconds;
    }

    public static long GetUnixTimeStamp(DateTime d)
    {
        return GetUnixTimeStampMillis(d) / 1000;
    }

    public static long GetUnixTimeStampMillis(DateTime d)
    {
        return (d.ToFileTime() / 10000) - unixBaseMillis;
    }

    public static long GetMilliseconds()
    {
        return GetUnixTimeStampMillis(DateTime.Now);
    }
}