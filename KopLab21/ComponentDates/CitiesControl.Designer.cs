namespace ComponentCities;

partial class CitiesControl
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.DataGridView dgvCities;

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
        dgvCities = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)dgvCities).BeginInit();
        SuspendLayout();
        // 
        // dgvCities
        // 
        dgvCities.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvCities.Dock = DockStyle.Fill;
        dgvCities.Location = new Point(0, 0);
        dgvCities.Name = "dgvCities";
        dgvCities.RowHeadersWidth = 51;
        dgvCities.Size = new Size(600, 400);
        dgvCities.TabIndex = 0;
        // 
        // CitiesControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(dgvCities);
        Name = "CitiesControl";
        Size = new Size(600, 400);
        ((System.ComponentModel.ISupportInitialize)dgvCities).EndInit();
        ResumeLayout(false);
    }

    #endregion
}
