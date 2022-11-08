using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace TopLearn.Core.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(date) + "/" + pc.GetMonth(date).ToString("00") +
                "/" + pc.GetDayOfMonth(date).ToString("00");
        }
        public static DateTime ToMiladi(this string date)
        {
            string[] val = date.Split('/');
            return new DateTime(
                int.Parse(val[0]),
                int.Parse(val[1]),
                int.Parse(val[2]),
                new PersianCalendar());


        }
    }
}
