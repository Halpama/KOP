namespace ImplApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listOfValue = new KopLab21.ListOfValue();
            inputInt1 = new KopLab21.InputInt();
            objectListControl1 = new KopLab21.ObjectListControl();
            SuspendLayout();
            // 
            // listOfValue
            // 
            listOfValue.Location = new Point(12, 27);
            listOfValue.Name = "listOfValue";
            listOfValue.Size = new Size(196, 139);
            listOfValue.TabIndex = 0;
            // 
            // inputInt1
            // 
            inputInt1.Location = new Point(12, 193);
            inputInt1.Name = "inputInt1";
            inputInt1.Size = new Size(292, 106);
            inputInt1.TabIndex = 1;
            // 
            // objectListControl1
            // 
            objectListControl1.Location = new Point(300, 27);
            objectListControl1.Margin = new Padding(2, 2, 2, 2);
            objectListControl1.Name = "objectListControl1";
            objectListControl1.SelectedValue = "";
            objectListControl1.Size = new Size(630, 392);
            objectListControl1.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(944, 502);
            Controls.Add(objectListControl1);
            Controls.Add(inputInt1);
            Controls.Add(listOfValue);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private KopLab21.ListOfValue listOfValue;
        private KopLab21.InputInt inputInt1;
        private KopLab21.ObjectListControl objectListControl1;
    }
}
