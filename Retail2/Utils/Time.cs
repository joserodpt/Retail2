using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail2.Utils
{
    class Time
    { 
        public static String get()
        {
            DateTime date = DateTime.Now;
            return date + "";
        }

        public static String calculateTimeElapsed(String time)
        {
            string[] tim = time.Split(' ');
            string[] date = tim[0].Split('/');
            string[] tme = tim[1].Split(':');

            DateTime date1 = new DateTime(Int32.Parse(date[2]), Int32.Parse(date[1]), Int32.Parse(date[0]), Int32.Parse(tme[0]), Int32.Parse(tme[1]), Int32.Parse(tme[2]));
            DateTime date2 = DateTime.Now;
            TimeSpan ts = date2 - date1;

            TimeSpan output = TimeSpan.FromSeconds(ts.TotalSeconds);
            string str = output.ToString(@"hh\:mm\:ss");

            return str;
        }

        static int Years;
        static int Months;
        static int Days;

        public static String calculateTimeAbout(String time)
        {
            string[] tim = time.Split(' ');
            string[] date = tim[0].Split('/');
            string[] tme = tim[1].Split(':');

            DateTime hs = new DateTime(Int32.Parse(date[2]), Int32.Parse(date[1]), Int32.Parse(date[0]), Int32.Parse(tme[0]), Int32.Parse(tme[1]), Int32.Parse(tme[2]));

            if ((DateTime.Now.Year - hs.Year) > 0 ||
        (((DateTime.Now.Year - hs.Year) == 0) && ((hs.Month < DateTime.Now.Month) ||
          ((hs.Month == DateTime.Now.Month) && (hs.Day <= DateTime.Now.Day)))))
            {
                int DaysInBdayMonth = DateTime.DaysInMonth(hs.Year, hs.Month);
                int DaysRemain = DateTime.Now.Day + (DaysInBdayMonth - hs.Day);

                if (DateTime.Now.Month > hs.Month)
                {
                   Years = DateTime.Now.Year - hs.Year;
                    Months = DateTime.Now.Month - (hs.Month + 1) + Math.Abs(DaysRemain / DaysInBdayMonth);
                    Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                }
                else if (DateTime.Now.Month == hs.Month)
                {
                    if (DateTime.Now.Day >= hs.Day)
                    {
                        Years = DateTime.Now.Year - hs.Year;
                        Months = 0;
                        Days = DateTime.Now.Day - hs.Day;
                    }
                    else
                    {
                        Years = (DateTime.Now.Year - 1) - hs.Year;
                        Months = 11;
                        Days = DateTime.DaysInMonth(hs.Year, hs.Month) - (hs.Day - DateTime.Now.Day);
                    }
                }
                else
                {
                    Years = (DateTime.Now.Year - 1) - hs.Year;
                    Months = DateTime.Now.Month + (11 - hs.Month) + Math.Abs(DaysRemain / DaysInBdayMonth);
                    Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                }
            }
            else
            {
                throw new ArgumentException("Date must be earlier than current date");
            }
            return Years + " ano(s)," +  Months + " mês(es)," + Days + " dia(s)";
        }

        public static double ConvertMillisecondsToSeconds(double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds).TotalSeconds;
        }

        public static double ConvertSecondsToMilliseconds(double seconds)
        {
            return TimeSpan.FromSeconds(seconds).TotalMilliseconds;
        }
    }
}
