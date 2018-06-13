using Com.Bateeq.Service.Masterplan.Lib.Utils;
using System;

namespace Com.Bateeq.Service.Masterplan.Lib.ViewModels
{
    public class WeeklyPlanItemViewModel : BaseViewModel
    {
        public int WeekNumber { get; set; }
        public int Month { get; set; }
        public int Efficiency { get; set; }
        public int Operator { get; set; }
        public int WorkingHours { get; set; }
        public int AhTotal { get; set; }
        public int EhTotal { get; set; }
        public int UsedEh { get; set; }
        public int RemainingEh { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
