namespace ImplApp
{
    partial class ExtensionsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboBoxPdf;
        private System.Windows.Forms.ComboBox comboBoxWord;
        private System.Windows.Forms.Label labelPdf;
        private System.Windows.Forms.Label labelWord;
        private System.Windows.Forms.Button buttonCreatePdf;
        private System.Windows.Forms.Button buttonCreateWord;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboBoxPdf = new System.Windows.Forms.ComboBox();
            this.comboBoxWord = new System.Windows.Forms.ComboBox();
            this.labelPdf = new System.Windows.Forms.Label();
            this.labelWord = new System.Windows.Forms.Label();
            this.buttonCreatePdf = new System.Windows.Forms.Button();
            this.buttonCreateWord = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelPdf
            // 
            this.labelPdf.AutoSize = true;
            this.labelPdf.Location = new System.Drawing.Point(30, 30);
            this.labelPdf.Name = "labelPdf";
            this.labelPdf.Size = new System.Drawing.Size(150, 20);
            this.labelPdf.Text = "Плагин отчёта (PDF):";
            // 
            // comboBoxPdf
            // 
            this.comboBoxPdf.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPdf.FormattingEnabled = true;
            this.comboBoxPdf.Location = new System.Drawing.Point(200, 27);
            this.comboBoxPdf.Name = "comboBoxPdf";
            this.comboBoxPdf.Size = new System.Drawing.Size(300, 28);
            // 
            // buttonCreatePdf
            // 
            this.buttonCreatePdf.Location = new System.Drawing.Point(520, 26);
            this.buttonCreatePdf.Name = "buttonCreatePdf";
            this.buttonCreatePdf.Size = new System.Drawing.Size(180, 30);
            this.buttonCreatePdf.Text = "Создать PDF отчёт";
            this.buttonCreatePdf.UseVisualStyleBackColor = true;
            this.buttonCreatePdf.Click += new System.EventHandler(this.buttonCreatePdf_Click);
            // 
            // labelWord
            // 
            this.labelWord.AutoSize = true;
            this.labelWord.Location = new System.Drawing.Point(30, 90);
            this.labelWord.Name = "labelWord";
            this.labelWord.Size = new System.Drawing.Size(158, 20);
            this.labelWord.Text = "Плагин диаграммы:";
            // 
            // comboBoxWord
            // 
            this.comboBoxWord.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWord.FormattingEnabled = true;
            this.comboBoxWord.Location = new System.Drawing.Point(200, 87);
            this.comboBoxWord.Name = "comboBoxWord";
            this.comboBoxWord.Size = new System.Drawing.Size(300, 28);
            // 
            // buttonCreateWord
            // 
            this.buttonCreateWord.Location = new System.Drawing.Point(520, 86);
            this.buttonCreateWord.Name = "buttonCreateWord";
            this.buttonCreateWord.Size = new System.Drawing.Size(180, 30);
            this.buttonCreateWord.Text = "Создать Word отчёт";
            this.buttonCreateWord.UseVisualStyleBackColor = true;
            this.buttonCreateWord.Click += new System.EventHandler(this.buttonCreateWord_Click);
            // 
            // ExtensionsFormVariant21
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 150);
            this.Controls.Add(this.buttonCreatePdf);
            this.Controls.Add(this.buttonCreateWord);
            this.Controls.Add(this.comboBoxPdf);
            this.Controls.Add(this.comboBoxWord);
            this.Controls.Add(this.labelPdf);
            this.Controls.Add(this.labelWord);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExtensionsFormVariant21";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Плагины отчетов (вариант 21)";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
