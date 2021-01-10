using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ShareBooks.Core.Convertors
{
   public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc=new PersianCalendar();
            return pc.GetYear(value).ToString() + "/" + pc.GetMonth(value).ToString("00") + "/"+
                   pc.GetDayOfMonth(value).ToString("00");

        }

        public static string ToShamsiForEdit(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetMonth(value).ToString() + "/" + pc.GetDayOfMonth(value).ToString("00") + "/" +
                   pc.GetYear(value);

        }

        public static string ConvertMiladiToShamsi(this DateTime date, string Format)
        {
            PersianDateTime persianDateTime = new PersianDateTime(date);
            return persianDateTime.ToString(Format);
        }

    }
}
