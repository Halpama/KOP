namespace ComponentOrdersReport
{
    partial class OrdersReportControl
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dgvOrdersReport;
        private System.Windows.Forms.DateTimePicker dtpReceiptFilter;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblTotalOrders;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer Code

        private void InitializeComponent()
        {
            this.dgvOrdersReport = new System.Windows.Forms.DataGridView();
            this.dtpReceiptFilter = new System.Windows.Forms.DateTimePicker();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblTotalOrders = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdersReport)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOrdersReport
            // 
            this.dgvOrdersReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrdersReport.Location = new System.Drawing.Point(10, 50);
            this.dgvOrdersReport.Name = "dgvOrdersReport";
            this.dgvOrdersReport.RowHeadersWidth = 51;
            this.dgvOrdersReport.RowTemplate.Height = 29;
            this.dgvOrdersReport.Size = new System.Drawing.Size(600, 350);
            this.dgvOrdersReport.TabIndex = 0;
            // 
            // dtpReceiptFilter
            // 
            this.dtpReceiptFilter.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpReceiptFilter.Location = new System.Drawing.Point(10, 10);
            this.dtpReceiptFilter.Name = "dtpReceiptFilter";
            this.dtpReceiptFilter.Size = new System.Drawing.Size(150, 27);
            this.dtpReceiptFilter.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(170, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 27);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblTotalOrders
            // 
            this.lblTotalOrders.AutoSize = true;
            this.lblTotalOrders.Location = new System.Drawing.Point(310, 14);
            this.lblTotalOrders.Name = "lblTotalOrders";
            this.lblTotalOrders.Size = new System.Drawing.Size(129, 20);
            this.lblTotalOrders.TabIndex = 3;
            this.lblTotalOrders.Text = "Всего заказов: 0";
            // 
            // OrdersReportControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTotalOrders);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dtpReceiptFilter);
            this.Controls.Add(this.dgvOrdersReport);
            this.Name = "OrdersReportControl";
            this.Size = new System.Drawing.Size(630, 410);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdersReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
