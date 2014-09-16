namespace Sonneville.PriceTools.PriceAnalyzer
{
    partial class TradeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.stockTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.volumeTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.priceTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.radioDateTime = new System.Windows.Forms.RadioButton();
            this.radioNextMarketClose = new System.Windows.Forms.RadioButton();
            this.radioNoExpiration = new System.Windows.Forms.RadioButton();
            this.submitButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // stockTextBox
            // 
            this.stockTextBox.Location = new System.Drawing.Point(88, 6);
            this.stockTextBox.Name = "stockTextBox";
            this.stockTextBox.Size = new System.Drawing.Size(58, 20);
            this.stockTextBox.TabIndex = 0;
            this.stockTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Stock";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Volume";
            // 
            // volumeTextBox
            // 
            this.volumeTextBox.Location = new System.Drawing.Point(88, 32);
            this.volumeTextBox.Name = "volumeTextBox";
            this.volumeTextBox.Size = new System.Drawing.Size(58, 20);
            this.volumeTextBox.TabIndex = 2;
            this.volumeTextBox.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            this.volumeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.textBox2_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Price";
            // 
            // priceTextBox
            // 
            this.priceTextBox.Location = new System.Drawing.Point(88, 58);
            this.priceTextBox.Name = "priceTextBox";
            this.priceTextBox.Size = new System.Drawing.Size(58, 20);
            this.priceTextBox.TabIndex = 4;
            this.priceTextBox.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            this.priceTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.textBox3_Validating);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dateTimePicker);
            this.groupBox1.Controls.Add(this.radioDateTime);
            this.groupBox1.Controls.Add(this.radioNextMarketClose);
            this.groupBox1.Controls.Add(this.radioNoExpiration);
            this.groupBox1.Location = new System.Drawing.Point(15, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(151, 124);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Expiration";
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Checked = false;
            this.dateTimePicker.Enabled = false;
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker.Location = new System.Drawing.Point(32, 89);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(102, 20);
            this.dateTimePicker.TabIndex = 7;
            this.dateTimePicker.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // radioDateTime
            // 
            this.radioDateTime.AutoSize = true;
            this.radioDateTime.Location = new System.Drawing.Point(7, 66);
            this.radioDateTime.Name = "radioDateTime";
            this.radioDateTime.Size = new System.Drawing.Size(48, 17);
            this.radioDateTime.TabIndex = 2;
            this.radioDateTime.Text = "Date";
            this.radioDateTime.UseVisualStyleBackColor = true;
            this.radioDateTime.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioNextMarketClose
            // 
            this.radioNextMarketClose.AutoSize = true;
            this.radioNextMarketClose.Location = new System.Drawing.Point(7, 43);
            this.radioNextMarketClose.Name = "radioNextMarketClose";
            this.radioNextMarketClose.Size = new System.Drawing.Size(110, 17);
            this.radioNextMarketClose.TabIndex = 1;
            this.radioNextMarketClose.Text = "Next market close";
            this.radioNextMarketClose.UseVisualStyleBackColor = true;
            this.radioNextMarketClose.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioNoExpiration
            // 
            this.radioNoExpiration.AutoSize = true;
            this.radioNoExpiration.Checked = true;
            this.radioNoExpiration.Location = new System.Drawing.Point(7, 20);
            this.radioNoExpiration.Name = "radioNoExpiration";
            this.radioNoExpiration.Size = new System.Drawing.Size(87, 17);
            this.radioNoExpiration.TabIndex = 0;
            this.radioNoExpiration.TabStop = true;
            this.radioNoExpiration.Text = "No expiration";
            this.radioNoExpiration.UseVisualStyleBackColor = true;
            this.radioNoExpiration.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(33, 218);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(116, 23);
            this.submitButton.TabIndex = 9;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(33, 247);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(116, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // TradeForm
            // 
            this.AcceptButton = this.submitButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(183, 282);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.priceTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.volumeTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.stockTextBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TradeForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TradeForm";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox stockTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox volumeTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox priceTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.RadioButton radioDateTime;
        private System.Windows.Forms.RadioButton radioNextMarketClose;
        private System.Windows.Forms.RadioButton radioNoExpiration;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button cancelButton;
    }
}