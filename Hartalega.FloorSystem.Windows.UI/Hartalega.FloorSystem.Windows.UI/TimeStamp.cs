using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Windows.UI
{
    class TimeStamp
    {
        public void WriteTime(string name)
        {
            using (StreamWriter sw = new StreamWriter("PerformanceTestTimer.txt", true))
            {
                sw.Write(name + ": ");
                sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"));
                sw.Close();
            }
        }
    }
}
