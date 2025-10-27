using System;
using System.Collections.Generic;

namespace ComponentOrdersList
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public List<string> MovementMarks { get; set; } = new List<string>();
        public string MovementMarksDisplay => string.Join(", ", MovementMarks);
        public string DestinationCity { get; set; } = string.Empty;
        private DateTime _receiptDate = DateTime.Now.Date.AddDays(1);
        public DateTime ReceiptDate
        {
            get => _receiptDate;
            set
            {
                DateTime min = DateTime.Now.Date.AddDays(1);
                DateTime max = DateTime.Now.Date.AddDays(3);
                if (value < min || value > max)
                    throw new ArgumentOutOfRangeException("Дата получения должна быть в пределах от 1 до 3 дней.");

                _receiptDate = value;
            }
        }

        public Order(int orderId, string customerName, DateTime receiptDate)
        {
            OrderId = orderId;
            CustomerName = customerName;
            ReceiptDate = receiptDate; // пройдет валидацию
        }

        public Order()
        {
        }

        public void AddMark(string mark)
        {
            if (string.IsNullOrWhiteSpace(mark))
                return;

            if (MovementMarks.Count >= 6)
                throw new InvalidOperationException("Нельзя добавить более 6 отметок о движении заказа.");

            MovementMarks.Add(mark);
        }
    }
}
