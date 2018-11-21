using Com.Bateeq.Service.Masterplan.Lib.Utils;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels.BookingOrder;
using System;

namespace Com.Bateeq.Service.Masterplan.Lib.ViewModels.BlockingPlan
{
    public class BlockingPlanViewModel : BaseViewModel, IValidatableObject
    {
        public int? BookingOrderId { get; set; }
        public DateTimeOffset BookingDate { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
        public List<BlockingPlanWorkScheduleViewModel> WorkSchedules { get; set; }
        public BookingOrderViewModel BookingOrder { get; set; }
        public string Status { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.BookingOrderId == null || this.BookingOrderId == 0)
            {
                yield return new ValidationResult("NB: Booking Order harus diisi", new List<string> { "ValidateBookingOrder" });
            }
          

            if (this.WorkSchedules.Count == 0)
            {
                yield return new ValidationResult("NB : Tabel Jadwal Pengerjaan harus diisi", new List<string> { "ValidateWorkSchedules" });
            }
            else
            {
                int count = 0;
                string error = "[";

                foreach (var workSchedule in this.WorkSchedules)
                {
                    error += "{";
                    if (workSchedule.RO != null)
                    {
                        if (workSchedule.RO.Length > 8)
                        {
                            error += "RO : 'RO Maksimal 8 Karakter', ";
                            count++;
                        }
                    }
                    if (workSchedule.RO == null)
                    {
                        count++;
                        error += "RO: 'RO harus diisi', ";
                    }
                    if (workSchedule.Unit == null)
                    {
                        count++;
                        error += "Unit: 'Unit harus diisi', ";
                    }
                    if (workSchedule.Year == null)
                    {
                        count++;
                        error += "Year: 'Tahun harus diisi', ";
                    }
                    if (workSchedule.Week == null)
                    {
                        count++;
                        error += "Week: 'Minggu harus diisi', ";
                    }
                    if (workSchedule.TotalOrder == null)
                    {
                        count++;
                        error += "TotalOrder: 'Jumlah Order harus diisi', ";
                    }
                    else if (workSchedule.TotalOrder <= 0)
                    {
                        count++;
                        error += "TotalOrder: 'Jumlah Order harus lebih besar dari 0', ";
                    }
                    if (workSchedule.DeliveryDate == null || workSchedule.DeliveryDate.UtcDateTime == DateTime.MinValue)
                    {
                        count++;
                        error += "DeliveryDate: 'Tanggal Pengiriman harus diisi', ";
                    }
                    else if (workSchedule.DeliveryDate <= this.BookingDate)
                    {
                        count++;
                        error += "DeliveryDate: 'Tanggal Pengiriman harus lebih dari Tanggal Booking', ";
                    }
                    else if (workSchedule.DeliveryDate > this.DeliveryDate)
                    {
                        count++;
                        error += "DeliveryDate: 'Tanggal Pengiriman tidak boleh lebih dari Tanggal Pengiriman di header', ";
                    }
                    error += "},";
                }

                error += "]";
                if (count > 0)
                {
                    yield return new ValidationResult(error, new List<string> { "WorkSchedules" });
                }
            }
        }
    }
}
