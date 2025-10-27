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

        // ���� ��������� ������ (����� ���� ������ �� 1 �� 3 ���� �� �������)
        private DateTime _receiptDate = DateTime.Now.Date.AddDays(1);
        public DateTime ReceiptDate
        {
            get => _receiptDate;
            set
            {
                var min = DateTime.Now.Date.AddDays(1);
                var max = DateTime.Now.Date.AddDays(3);
                if (value < min || value > max)
                    throw new ArgumentOutOfRangeException("���� ��������� ������ ���� � ��������� 1�3 ��� �� ������� ����");

                _receiptDate = value;
            }
        }

        public Order(int orderId, string customerName, DateTime receiptDate)
        {
            OrderId = orderId;
            CustomerName = customerName;
            ReceiptDate = receiptDate; // ������ ����� ���������
        }

        public Order() { }

        public void AddMovementMark(string mark)
        {
            if (string.IsNullOrWhiteSpace(mark))
                return;

            if (MovementMarks.Count >= 6)
                throw new InvalidOperationException("������ �������� ������ 6 ������� � �������� ������.");

            MovementMarks.Add(mark);
        }
    }
}
