using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Models;

namespace Woopin.SGC.Common.Helpers
{
    public static class DateHelper
    {
        public static DateRange GetWeek(DateTime date)
        {
            DateRange ret = new DateRange();

            int offset = date.DayOfWeek - DayOfWeek.Monday;

            ret.Start = date.AddDays(-offset);
            ret.End = ret.Start.AddDays(6);

            return ret;
        }

        public static DateRange GetWeekSatToFri(DateTime date)
        {
            DateRange ret = new DateRange();

            int offset = date.DayOfWeek - DayOfWeek.Saturday;
            if (offset < 0)
            {
                offset += 7;
            }


            ret.Start = date.AddDays(-offset);
            ret.End = ret.Start.AddDays(6);

            return ret;
        }

        public static int GetWeekNumber(DateTime date)
        {
            // Cambia la fecha para estar mas adelantados en la semana, el numero va a ser el mismo.
            // Pero controla para los distintos comienzos de años.
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static string GetMonthName(int MonthNumber)
        {
            return new DateTime(2000, MonthNumber, 1).ToString("MMMM", new CultureInfo("ES"));
        }

        public static DateTime AddBusinessDays(DateTime date, int days)
        {
            if (days < 0)
            {
                throw new ArgumentException("days cannot be negative", "days");
            }

            if (days == 0) return date;

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(2);
                days -= 1;
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
                days -= 1;
            }

            date = date.AddDays(days / 5 * 7);
            int extraDays = days % 5;

            if ((int)date.DayOfWeek + extraDays > 5)
            {
                extraDays += 2;
            }

            return date.AddDays(extraDays);

        }

        public static int GetBusinessDays(DateTime start, DateTime end)
        {
            if (start.DayOfWeek == DayOfWeek.Saturday)
            {
                start = start.AddDays(2);
            }
            else if (start.DayOfWeek == DayOfWeek.Sunday)
            {
                start = start.AddDays(1);
            }

            if (end.DayOfWeek == DayOfWeek.Saturday)
            {
                end = end.AddDays(-1);
            }
            else if (end.DayOfWeek == DayOfWeek.Sunday)
            {
                end = end.AddDays(-2);
            }

            int diff = (int)end.Subtract(start).TotalDays;

            int result = diff / 7 * 5 + diff % 7;

            if (end.DayOfWeek < start.DayOfWeek)
            {
                return result - 2;
            }
            else
            {
                return result;
            }
        }
    }
}
