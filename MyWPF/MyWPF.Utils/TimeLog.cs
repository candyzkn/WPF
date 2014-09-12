using System;
using System.Collections.Generic;
using System.IO;

namespace MyWPF.Utils
{
    public class TimeLog
    {
        private readonly IDictionary<string, LogInfo> _dict = new Dictionary<string, LogInfo>();
        private readonly string _fileName;
        public TimeLog(string fileName)
        {
            this._fileName = fileName;
        }

        public void BeginLog(string log)
        {
            this.BeginLog(log, null);
        }

        public void BeginLog(string log, string author)
        {
            if (_dict.ContainsKey(log))
                _dict.Remove(log);

            _dict.Add(log, new LogInfo { Log = log, Author = author });
        }

        public void EndLog(string log)
        {
            var info = _dict[log];
            if (info == null || log == null) return;
            _dict.Remove(log);
            var str = info.GetLog();
            using (var sw = new StreamWriter(_fileName, true))
            {
                sw.WriteLine(str);
                sw.Close();
            }
        }


        public void WriteLog(string log)
        {
            var str = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "：" + log;
            using (var sw = new StreamWriter(_fileName, true))
            {
                sw.WriteLine(str);
                sw.Close();
            }
        }

        class LogInfo
        {
            public LogInfo()
            {
                this.BeginTime = DateTime.Now;
            }

            DateTime BeginTime { get; set; }
            public string Log { get; set; }
            public string Author { get; set; }

            public string GetLog()
            {
                var time = DateTime.Now;
                var ms = (time - this.BeginTime).TotalMilliseconds.ToString();
                var result = this.Log;
                if (!string.IsNullOrEmpty(this.Author)) result += "(" + this.Author + ")";
                result += " 开始执行时间:" + this.BeginTime.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "  结束时间:" + time.ToString("hh:mm:ss.ffff") + " 共计执行:" + ms + "毫秒";
                return result;
            }
        }
    }
}
