using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;

namespace Hamburgueria
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static void Rearrange(this ObservableCollection<View.Sale> sales)
        {
            for (int i = 0; i < sales.Count - 1; i++)
            {
                View.Sale first = sales[i];
                View.Sale second = sales[i + 1];

                if (first.Date < second.Date)
                {
                    sales.RemoveAt(i + 1);
                    sales.Insert(i + 1, first);
                    sales.RemoveAt(i);
                    sales.Insert(i, second);
                }
            }
        }
    }
}
