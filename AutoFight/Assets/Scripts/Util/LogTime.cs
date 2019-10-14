using UnityEngine;

public class LogTime:Singleton<LogTime>
{
    private static string strPre = "Time---";
    private static long startTime = 0;
    private static int ms = 10000;
    public static void init()
    {
        startTime = System.DateTime.Now.ToFileTimeUtc();
    }
    
    private static long preTime = 0;
    public static void LogCurTime()
    {
        Debug.Log(System.DateTime.Now.ToString());    
    }
 
    public static void LogTimeToStart(string aStepName="")
    {
        long timeGap = System.DateTime.Now.ToFileTimeUtc() - startTime;
        Debug.Log(strPre+"∧"+timeGap+"\t"+aStepName);
    }
    
    public static void LogStepTime(string aStepName = "")
    {
        long timeGap = System.DateTime.Now.ToFileTimeUtc() - preTime;
        long timeGapFromStart = System.DateTime.Now.ToFileTimeUtc() - startTime;
        //Debug.Log(strPre+"∧"+(int)(timeGapFromStart/ms)+"\t↑"+(int)(timeGap/ms)+"\t"+aStepName);
        
        preTime = System.DateTime.Now.ToFileTimeUtc();
    }
}