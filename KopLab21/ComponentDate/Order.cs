using System;
using System.Collections.Generic;

namespace ComponentOrdersReport
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public List<string> MovementMarks { get; set; } = new List<string>();
        public string DestinationCity { get; set; } = string.Empty;

        // ƒата получени€ заказа (может быть только от 1 до 3 дней от сегодн€)
        private DateTime _receiptDate = DateTime.Now.Date.AddDays(1);
        public DateTime ReceiptDate
        {
            get => _receiptDate;
            set
            {
                var min = DateTime.Now.Date.AddDays(1);
                var max = DateTime.Now.Date.AddDays(3);
                if (value < min || value > max)
                    throw new ArgumentOutOfRangeException("ƒата получени€ должна быть в диапазоне 1Ц3 дн€ от текущей даты");

                _receiptDate = value;
            }
        }

        public Order(int orderId, string customerName, DateTime receiptDate)
        {
            OrderId = orderId;
            CustomerName = customerName;
            ReceiptDate = receiptDate; // пройдЄт через валидацию
        }

        public Order() { }

        public void AddMovementMark(string mark)
        {
            if (string.IsNullOrWhiteSpace(mark))
                return;

            if (MovementMarks.Count >= 6)
                throw new InvalidOperationException("Ќельз€ добавить больше 6 отметок о движении заказа.");

            MovementMarks.Add(mark);
        }
    }
}
