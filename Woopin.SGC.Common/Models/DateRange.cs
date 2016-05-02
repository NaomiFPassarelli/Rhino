using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Woopin.SGC.Common.Models
{
    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        private int DaysRange { get; set; }

        public DateRange() : this(null,null,0)
        {

        }

        public DateRange(DateTime? start, DateTime? end) : this(start,end,0)
        {

        }

        public DateRange(DateTime? start, DateTime? end, int daysRange)
        {
            int MinDiffOfDays = Convert.ToInt32(ConfigurationManager.AppSettings["MinDiffOfDays"]);
            int MaxDiffOfDays = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDiffOfDays"]);
            if (daysRange > 0)
            {
                MinDiffOfDays = daysRange;
                MaxDiffOfDays = daysRange;
            } 

            if(start.HasValue && end.HasValue)
            {
                this.Start = start.Value;
                this.End = end.Value;
            }
            else if(start.HasValue && !end.HasValue)
            {
                this.End = start.Value.AddDays(MaxDiffOfDays);
                this.Start = start.Value;
            }
            else if (!start.HasValue && end.HasValue)
            {
                this.End = end.Value;
                this.Start = this.End.AddDays(-MinDiffOfDays);
            }
            else
            {
                this.End = DateTime.Now;
                this.Start = DateTime.Now.AddDays(-MinDiffOfDays);
            }
            
            //Arreglo para tener el End hasta el ultimo minuto.
            this.End = new DateTime(this.End.Year, this.End.Month, this.End.Day, 23, 59, 59);
        }






        public string ToString(string dateFormat, string dateConnector)
        {
            return this.Start.ToString(dateFormat) + " " + dateConnector + " " + this.End.ToString(dateFormat);
        }
    }
}
