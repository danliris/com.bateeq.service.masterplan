using Com.Bateeq.Service.Masterplan.Lib.Utils;
using System;

namespace Com.Bateeq.Service.Masterplan.Lib.ViewModels
{
    public class WeeklyPlanItemViewModel : BaseViewModel
    {
        public string WeekNumber { get; set; }
        public string WeekText { get; set; }
        public int Month { get; set; }
        public int Efficiency { get; set; }
        public int Operator { get; set; }
        public int WorkingHours { get; set; }
        public double AhTotal { get; set; }
        public double EhTotal { get; set; }
        public double UsedEh { get; set; }
        public double RemainingEh { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
