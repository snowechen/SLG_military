using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;

public class Debuger {

    public static bool EnableLog; //是否开启日志
    public static bool EnableLoop;//循环重复输出开关
    public static bool EnableTime = true; //时间开关
    public static bool EnableSave = false; //日志保存开关
    public static bool EnableStack = false; //堆栈开关
    public static string LogFileDir = ""; //日志文件目录
    public static string LogFileName = ""; //日志文件名
    public static string Prefix = "> "; //前缀符号
    public static StreamWriter LogFileWriter = null;//文件流信息
    public static bool UseUnityEngine = true; //是否为Unity引擎

    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        //判断是否为Unity引擎
        if (UseUnityEngine)
        {
            //获取日志保存目录
            LogFileDir = UnityEngine.Application.persistentDataPath + "/DebugerLog/";
        }
        else
        {
            // 获取程序的基目录
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            LogFileDir = path + "/DebugerLog/";
        }
    }

    /// <summary>
    /// 内部日志输出
    /// </summary>
    /// <param name="msg">输出内容</param>
    /// <param name="context">关系文</param>
    private static void Internal_Log(string msg,object context = null)
    {
        if (UseUnityEngine)
        {
            //使用unity debug输出普通日志信息
            UnityEngine.Debug.Log(msg, (UnityEngine.Object)context);
        }
        else
        {
            Console.WriteLine(msg);
        }
    }

    /// <summary>
    /// 输出警告日志内容
    /// </summary>
    /// <param name="msg">日志内容</param>
    /// <param name="context">关联文</param>
    private static void Internal_LogWarning(string msg,object context=null)
    {
        if (UseUnityEngine)
        {
            //输出Unity警告日志信息，转换context为Unity Object类型
            UnityEngine.Debug.LogWarning(msg, (UnityEngine.Object)context);
        }
        else
        {
            Console.WriteLine(msg);
        }
    }

    /// <summary>
    /// 输出Unity错误日志信息
    /// </summary>
    /// <param name="msg">输出内容</param>
    /// <param name="context">关联文</param>
    private static void Internal_LogError(string msg,object context = null)
    {
        if (UseUnityEngine)
        {
            UnityEngine.Debug.LogError(msg, (UnityEngine.Object)context);
        }
        else
        {
            Console.WriteLine(msg);
        }
    }

   //=========================
   /// <summary>
   /// 循环日志
   /// </summary>
   /// <param name="tag">标签</param>
   /// <param name="methodName">方法名</param>
   /// <param name="message">日志信息</param>
   [Conditional("ENABLE_LOG_LOOP")]
   public static void LogLoop(string tag,string methodName,string message = "")
    {
        if (!Debuger.EnableLoop) { return; }

        message = GetLogText(tag, methodName, message);
        Internal_Log(Prefix + message);
        LogToFile("[I]" + message);
    }
    /// <summary>
    /// 循环输出-格式化obj数组日志内容
    /// </summary>
    /// <param name="tag">标签</param>
    /// <param name="methodName">方法名</param>
    /// <param name="format">格式</param>
    /// <param name="args">被格式内容数组</param>
    [Conditional("ENABLE_LOG_LOOP")]
    public static void LogLoop(string tag, string methodName, string format, params object[] args)
    {
        if (!Debuger.EnableLoop){ return;}

        string message = GetLogText(tag, methodName, string.Format(format, args));
        Internal_Log(Prefix + message);
        LogToFile("[I]" + message);
    }

    public static void Log(object message)
    {
        if (!Debuger.EnableLog) return;

        string msg = GetLogTime() + message;
        Internal_Log(Prefix + msg);
    }
    /// <summary>
    /// 普通日志
    /// </summary>
    /// <param name="tag">标签</param>
    /// <param name="methodName">方法名</param>
    /// <param name="message">日志内容</param>
    [Conditional("ENABLE_LOG_LOOP"), Conditional("ENABLE_LOG")]
    public static void Log(string tag, string methodName, string message = "")
    {
        if (!Debuger.EnableLog)
        {
            return;
        }

        message = GetLogText(tag, methodName, message);
        Internal_Log(Prefix + message);
        LogToFile("[I]" + message);
    }
    /// <summary>
    /// 普通日志-obj数组格式化
    /// </summary>
    /// <param name="tag">标签</param>
    /// <param name="methodName">方法名</param>
    /// <param name="format">指定格式</param>
    /// <param name="args">obj数组</param>
    [Conditional("ENABLE_LOG_LOOP"), Conditional("ENABLE_LOG")]
    public static void Log(string tag, string methodName, string format, params object[] args)
    {
        if (!Debuger.EnableLog)
        {
            return;
        }

        string message = GetLogText(tag, methodName, string.Format(format, args));
        Internal_Log(Prefix + message);
        LogToFile("[I]" + message);
    }
    /// <summary>
    /// 报错日志
    /// </summary>
    /// <param name="tag">标签</param>
    /// <param name="methodName">方法名</param>
    /// <param name="message">日志内容</param>
    public static void LogError(string tag, string methodName, string message)
    {
        message = GetLogText(tag, methodName, message);
        Internal_LogError(Prefix + message);
        LogToFile("[E]" + message, true);
    }
    /// <summary>
    /// 报错日志-obj数组格式化
    /// </summary>
    /// <param name="tag">标签</param>
    /// <param name="methodName">方法名</param>
    /// <param name="format">指定格式</param>
    /// <param name="args">obj内容数组</param>
    public static void LogError(string tag, string methodName, string format, params object[] args)
    {
        string message = GetLogText(tag, methodName, string.Format(format, args));
        Internal_LogError(Prefix + message);
        LogToFile("[E]" + message, true);
    }

    /// <summary>
    /// 警告日志
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="methodName"></param>
    /// <param name="message"></param>
    public static void LogWarning(string tag, string methodName, string message)
    {
        //UnityEngine.Debug.Log("进入打印");
        message = GetLogText(tag, methodName, message);
        Internal_LogWarning(Prefix + message);
        LogToFile("[W]" + message);
    }
    /// <summary>
    /// 警告日志-obj格式化数组
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="methodName"></param>
    /// <param name="format"></param>
    /// <param name="args"></param>
    public static void LogWarning(string tag, string methodName, string format, params object[] args)
    {
        string message = GetLogText(tag, methodName, string.Format(format, args));
        Internal_LogWarning(Prefix + message);
        LogToFile("[W]" + message);
    }

    //===========================
    /// <summary>
    /// 加入执行时间和重新组合日志信息
    /// </summary>
    /// <param name="tag">标签</param>
    /// <param name="methodName">方法名</param>
    /// <param name="message">信息</param>
    /// <returns></returns>
    private static string GetLogText(string tag,string methodName,string message)
    {
        string str = "";
        if (Debuger.EnableTime)
        {
            DateTime now = DateTime.Now;
            str = now.ToString("HH:mm:ss.fff") + " ";
        }
        str += tag + "::" + methodName + "()" + message;
        return str;
    }
    private static string GetLogTime()
    {
        string time = "";
        if (Debuger.EnableTime)
        {
            DateTime now = DateTime.Now;
            time = now.ToString("HH:mm:ss.fff") + " ";
        }
        return time;
    }
    //==============================
    /// <summary>
    /// 检查日志存放路径
    /// </summary>
    /// <returns></returns>
    internal static string CheckLogDir()
    {
        //如果没有路径
        if (string.IsNullOrEmpty(LogFileDir))
        {
            //无法在线程中执行！
            try
            {
                if (UseUnityEngine)
                {
                    //通过Unity路径方法获取
                    LogFileDir = UnityEngine.Application.persistentDataPath + "/DebugerLog/";
                }
                else
                {
                    //利用C#方法获取当前项目目录
                    string path = System.AppDomain.CurrentDomain.BaseDirectory;
                    LogFileDir = path + "/DebugerLog/";
                }
            }
            catch (Exception e)
            {
                //错误报告
                Internal_LogError("Debuger::CheckLogFileDir()" + e.Message + e.StackTrace);
                return "";
            }
        }
        try
        {
            //检查文件夹是否存在，不存在就创建一个
            if (!Directory.Exists(LogFileDir))
            {
                Directory.CreateDirectory(LogFileDir);
            }
        }catch(Exception e)
        {
            //错误报告
            Internal_LogError("Debuger::CheckLogFileDir()" + e.Message + e.StackTrace);
            return "";
        }
        return LogFileDir;
    }

    /// <summary>
    /// 生成日志文件名
    /// </summary>
    /// <returns>返回以当前日期时间组成的文件名</returns>
    internal static string GenLogFileName()
    {
        DateTime now = DateTime.Now;
        //获取精确到秒的时间
        string filename = now.GetDateTimeFormats('s')[0].ToString();
        //转换字符
        filename = filename.Replace("-", "_");
        filename = filename.Replace(":", "_");
        filename = filename.Replace(" ", "");
        filename += ".log";
        return filename;
    }

    /// <summary>
    /// 保存日志内容
    /// </summary>
    /// <param name="message">内容</param>
    /// <param name="EnableStack">日志保存开关</param>
    private static void LogToFile(string message,bool EnableStack = false)
    {
        if (!EnableStack) { return; }
        //UnityEngine.Debug.Log("创建文件");
        if (LogFileWriter == null)
        {
            LogFileName = GenLogFileName();
            LogFileDir = CheckLogDir();
            if (string.IsNullOrEmpty(LogFileDir)) { return; }
            
            string fullpath = LogFileDir + LogFileName;
            try
            {
                LogFileWriter = File.AppendText(fullpath);
                LogFileWriter.AutoFlush = true;
            }catch(Exception e)
            {
                LogFileWriter = null;
                Internal_LogError("Debuger::LogToFile() " + e.Message + e.StackTrace);
                return;
            }
        }

        //文件流不为空时
        if(LogFileWriter != null)
        {
            try
            {
                //写入日志
                LogFileWriter.WriteLine(message);
                if( (EnableStack||Debuger.EnableStack) && UseUnityEngine)
                {
                    LogFileWriter.WriteLine(UnityEngine.StackTraceUtility.ExtractStackTrace());
                }
            }
            catch (Exception) { return; }
        }
    }
}
