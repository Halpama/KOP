namespace KopLab21
{
    partial class ObjectListControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            listBox = new ListBox();
            SuspendLayout();
            // 
            // listBox
            // 
            listBox.FormattingEnabled = true;
            listBox.Location = new Point(6, 3);
            listBox.Name = "listBox";
            listBox.Size = new Size(813, 484);
            listBox.TabIndex = 0;
            // 
            // ObjectListControl
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(listBox);
            Name = "ObjectListControl";
            Size = new Size(819, 502);
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBox;
    }
}
