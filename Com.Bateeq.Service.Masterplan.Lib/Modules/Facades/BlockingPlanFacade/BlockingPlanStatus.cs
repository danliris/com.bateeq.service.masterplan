using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BlockingPlanFacade
{
    public static class BlockingPlanStatus
    {
        public const string BOOKING = "Booking";
        public const string HALF_CONFIRMED = "Confirm Sebagian";
        public const string FULL_CONFIRMED = "Confirm Full";
        public const string CHANGED = "Booking Ada Perubahan";
        public const string CANCELLED = "Booking Dibatalkan";
        public const string DELETED = "Booking Dihapus";
        public const string EXPIRED = "Booking Expired";
    }
}
