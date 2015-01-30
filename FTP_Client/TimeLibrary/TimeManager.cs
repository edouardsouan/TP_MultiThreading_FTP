using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLibrary
{
    public static class TimeManager
    {
        public static double CalculateTimeLeft(DateTime beginDate, double nbProcessed, double nbTotal)
        {
            return (DateTime.Now.Subtract(beginDate).TotalSeconds / nbProcessed) * (nbTotal - nbProcessed);
        }
    }
}
