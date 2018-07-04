using Com.Bateeq.Service.Masterplan.Lib.Utils;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels.Integration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Bateeq.Service.Masterplan.Lib.ViewModels.BookingOrder
{
    public class BookingOrderViewModel : BaseViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public int SerialNumber { get; set; }
        public SectionViewModel Section { get; set; }
        public DateTimeOffset BookingDate { get; set; }
        public BuyerViewModel Buyer { get; set; }
        public int? InitialOrderQuantity { get; set; }
        public int? OrderQuantity { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
        public string Remark { get; set; }
        public List<BookingOrderDetailViewModel> DetailConfirms { get; set; }
        public string StatusRemainingOrder { get; set; }
        public string Status { get; set; }
        public string StatusTotalConfirm { get; set; }
        public int? BlockingPlanId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Section == null || this.Section.Id == 0)
                yield return new ValidationResult("Seksi harus diisi", new List<string> { "Section" });

            if (this.BookingDate == null)
                yield return new ValidationResult("Tanggal Booking harus diisi", new List<string> { "BookingDate" });

            if (this.Buyer == null || this.Buyer.Id == 0)
                yield return new ValidationResult("Pembeli harus diisi", new List<string> { "Pembeli" });

            if (this.OrderQuantity == null)
                yield return new ValidationResult("Jumlah Order harus diisi", new List<string> { "OrderQuantity" });
            else if (this.OrderQuantity <= 0)
                yield return new ValidationResult("Jumlah Order harus lebih besar dari 0", new List<string> { "OrderQuantity" });

            if (this.DeliveryDate == null)
                yield return new ValidationResult("Tanggal Pengiriman harus diisi", new List<string> { "DeliveryDate" });
            else if (this.DeliveryDate <= this.BookingDate.AddDays(45))
                yield return new ValidationResult("Tanggal pengiriman harus > 45 hari dari tanggal hari ini", new List<string> { "DeliveryDate" });

            if (this.DetailConfirms != null)
            {
                int Count = 0;
                string detailConfirmError = "[";

                foreach (BookingOrderDetailViewModel detailConfirm in this.DetailConfirms)
                {
                    detailConfirmError += "{";
                    if (detailConfirm.RO == null)
                    {
                        Count++;
                        detailConfirmError += "RO: 'RO harus diisi', ";
                    }
                    if (detailConfirm.Total == null)
                    {
                        Count++;
                        detailConfirmError += "Total: 'Jumlah harus diisi', ";
                    }
                    else if (detailConfirm.Total <= 0)
                    {
                        Count++;
                        detailConfirmError += "Total: 'Jumlah harus lebih besar dari 0', ";
                    }
                    if (detailConfirm.DeliveryDate == null || detailConfirm.DeliveryDate.UtcDateTime == DateTime.MinValue)
                    {
                        Count++;
                        detailConfirmError += "DeliveryDate: 'Tanggal Pengiriman harus diisi', ";
                    }
                    else if (detailConfirm.DeliveryDate <= this.BookingDate)
                    {
                        Count++;
                        detailConfirmError += "DeliveryDate: 'Tanggal Pengiriman harus lebih dari Tanggal Booking', ";
                    }
                    else if (detailConfirm.DeliveryDate > this.DeliveryDate)
                    {
                        Count++;
                        detailConfirmError += "DeliveryDate: 'Tanggal Pengiriman tidak boleh lebih dari Tanggal Pengiriman di header', ";
                    }
                    detailConfirmError += "},";
                }

                detailConfirmError += "]";
                if (Count > 0)
                {
                    yield return new ValidationResult(detailConfirmError, new List<string> { "DetailConfirms" });
                }
            }
        }
    }
}
