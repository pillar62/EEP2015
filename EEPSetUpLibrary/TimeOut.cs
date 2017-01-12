using System;
using System.Collections.Generic;
using System.Text;

namespace EEPSetUpLibrary
{
    /// <summary>
    /// 定时类,用以检测连接超时的情况
    /// </summary>
    public class TimeOut
    { 
        /// <summary>
        /// 定时到达时的回调方法
        /// </summary>
        /// <param name="param"></param>
        public delegate void TimeOutCallBack(object param);
        private System.Timers.Timer time = new System.Timers.Timer();
        private TimeOutCallBack TimeCallBack;
        private object Param = null;

        /// <summary>
        /// 创建一个定时实例
        /// </summary>
        /// <param name="interval">定时的间隔</param>
        /// <param name="callback">定时到达时的回调方法</param>
        /// <param name="paramemter">回调方法的参数</param>
        public TimeOut(double interval,TimeOutCallBack callback, object paramemter)
        {
            time.Interval = interval;
            TimeCallBack = callback;
            Param = paramemter;
            time.Elapsed += new System.Timers.ElapsedEventHandler(time_Elapsed);
        }

        void time_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimeCallBack(Param); ;
        }

        /// <summary>
        /// 启动定时
        /// </summary>
        public void Start()
        {
            time.Start();
        }

        /// <summary>
        /// 停止定时
        /// </summary>
        public void Stop()
        {
            time.Stop();
        }

        /// <summary>
        /// 重新启动定时
        /// </summary>
        public void Restart()
        {
            time.Stop();
            time.Start();
        }

        /// <summary>
        /// 释放定时所用的资源
        /// </summary>
        public void Close()
        {
            time.Stop();
            time.Close();
        }

    }
}
