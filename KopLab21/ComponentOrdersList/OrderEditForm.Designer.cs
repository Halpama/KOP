namespace ComponentOrdersList;

partial class OrderEditForm
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblCustomerName;
    private System.Windows.Forms.TextBox txtCustomerName;
    private System.Windows.Forms.Label lblCity;
    private System.Windows.Forms.ComboBox cmbCities;
    private System.Windows.Forms.Label lblMarks;
    private System.Windows.Forms.ListBox lstMarks;
    private System.Windows.Forms.TextBox txtNewMark;
    private System.Windows.Forms.Button btnAddMark;
    private System.Windows.Forms.Button btnRemoveMark;
    private System.Windows.Forms.Label lblDate;
    private System.Windows.Forms.DateTimePicker dtpReceiptDate;
    private System.Windows.Forms.Button btnSaveOrder;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    private void InitializeComponent()
    {
        this.lblCustomerName = new System.Windows.Forms.Label();
        this.txtCustomerName = new System.Windows.Forms.TextBox();
        this.lblCity = new System.Windows.Forms.Label();
        this.cmbCities = new System.Windows.Forms.ComboBox();
        this.lblMarks = new System.Windows.Forms.Label();
        this.lstMarks = new System.Windows.Forms.ListBox();
        this.txtNewMark = new System.Windows.Forms.TextBox();
        this.btnAddMark = new System.Windows.Forms.Button();
        this.btnRemoveMark = new System.Windows.Forms.Button();
        this.lblDate = new System.Windows.Forms.Label();
        this.dtpReceiptDate = new System.Windows.Forms.DateTimePicker();
        this.btnSaveOrder = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // lblCustomerName
        // 
        this.lblCustomerName.AutoSize = true;
        this.lblCustomerName.Location = new System.Drawing.Point(20, 20);
        this.lblCustomerName.Name = "lblCustomerName";
        this.lblCustomerName.Size = new System.Drawing.Size(116, 20);
        this.lblCustomerName.TabIndex = 0;
        this.lblCustomerName.Text = "ФИО заказчика:";
        // 
        // txtCustomerName
        // 
        this.txtCustomerName.Location = new System.Drawing.Point(160, 17);
        this.txtCustomerName.Name = "txtCustomerName";
        this.txtCustomerName.Size = new System.Drawing.Size(300, 27);
        this.txtCustomerName.TabIndex = 1;
        // 
        // lblCity
        // 
        this.lblCity.AutoSize = true;
        this.lblCity.Location = new System.Drawing.Point(20, 65);
        this.lblCity.Name = "lblCity";
        this.lblCity.Size = new System.Drawing.Size(128, 20);
        this.lblCity.TabIndex = 2;
        this.lblCity.Text = "Город назначения:";
        // 
        // cmbCities
        // 
        this.cmbCities.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cmbCities.FormattingEnabled = true;
        this.cmbCities.Location = new System.Drawing.Point(160, 62);
        this.cmbCities.Name = "cmbCities";
        this.cmbCities.Size = new System.Drawing.Size(300, 28);
        this.cmbCities.TabIndex = 3;
        // 
        // lblMarks
        // 
        this.lblMarks.AutoSize = true;
        this.lblMarks.Location = new System.Drawing.Point(20, 110);
        this.lblMarks.Name = "lblMarks";
        this.lblMarks.Size = new System.Drawing.Size(189, 20);
        this.lblMarks.TabIndex = 4;
        this.lblMarks.Text = "Отметки о движении (≤6):";
        // 
        // lstMarks
        // 
        this.lstMarks.FormattingEnabled = true;
        this.lstMarks.ItemHeight = 20;
        this.lstMarks.Location = new System.Drawing.Point(20, 135);
        this.lstMarks.Name = "lstMarks";
        this.lstMarks.Size = new System.Drawing.Size(300, 144);
        this.lstMarks.TabIndex = 5;
        // 
        // txtNewMark
        // 
        this.txtNewMark.Location = new System.Drawing.Point(340, 135);
        this.txtNewMark.Name = "txtNewMark";
        this.txtNewMark.PlaceholderText = "Введите новую отметку";
        this.txtNewMark.Size = new System.Drawing.Size(220, 27);
        this.txtNewMark.TabIndex = 6;
        // 
        // btnAddMark
        // 
        this.btnAddMark.Location = new System.Drawing.Point(340, 175);
        this.btnAddMark.Name = "btnAddMark";
        this.btnAddMark.Size = new System.Drawing.Size(100, 30);
        this.btnAddMark.TabIndex = 7;
        this.btnAddMark.Text = "Добавить";
        this.btnAddMark.UseVisualStyleBackColor = true;
        this.btnAddMark.Click += new System.EventHandler(this.btnAddMark_Click);
        // 
        // btnRemoveMark
        // 
        this.btnRemoveMark.Location = new System.Drawing.Point(460, 175);
        this.btnRemoveMark.Name = "btnRemoveMark";
        this.btnRemoveMark.Size = new System.Drawing.Size(100, 30);
        this.btnRemoveMark.TabIndex = 8;
        this.btnRemoveMark.Text = "Удалить";
        this.btnRemoveMark.UseVisualStyleBackColor = true;
        this.btnRemoveMark.Click += new System.EventHandler(this.btnRemoveMark_Click);
        // 
        // lblDate
        // 
        this.lblDate.AutoSize = true;
        this.lblDate.Location = new System.Drawing.Point(20, 300);
        this.lblDate.Name = "lblDate";
        this.lblDate.Size = new System.Drawing.Size(185, 20);
        this.lblDate.TabIndex = 9;
        this.lblDate.Text = "Дата получения (1–3 дня):";
        // 
        // dtpReceiptDate
        // 
        this.dtpReceiptDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
        this.dtpReceiptDate.Location = new System.Drawing.Point(220, 296);
        this.dtpReceiptDate.Name = "dtpReceiptDate";
        this.dtpReceiptDate.Size = new System.Drawing.Size(150, 27);
        this.dtpReceiptDate.TabIndex = 10;
        // 
        // btnSaveOrder
        // 
        this.btnSaveOrder.Location = new System.Drawing.Point(20, 350);
        this.btnSaveOrder.Name = "btnSaveOrder";
        this.btnSaveOrder.Size = new System.Drawing.Size(150, 35);
        this.btnSaveOrder.TabIndex = 11;
        this.btnSaveOrder.Text = "Сохранить заказ";
        this.btnSaveOrder.UseVisualStyleBackColor = true;
        this.btnSaveOrder.Click += new System.EventHandler(this.btnSaveOrder_Click);
        // 
        // OrderEditForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.Controls.Add(this.btnSaveOrder);
        this.Controls.Add(this.dtpReceiptDate);
        this.Controls.Add(this.lblDate);
        this.Controls.Add(this.btnRemoveMark);
        this.Controls.Add(this.btnAddMark);
        this.Controls.Add(this.txtNewMark);
        this.Controls.Add(this.lstMarks);
        this.Controls.Add(this.lblMarks);
        this.Controls.Add(this.cmbCities);
        this.Controls.Add(this.lblCity);
        this.Controls.Add(this.txtCustomerName);
        this.Controls.Add(this.lblCustomerName);
        this.Name = "OrderEditForm";
        this.Size = new System.Drawing.Size(600, 410);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion
}