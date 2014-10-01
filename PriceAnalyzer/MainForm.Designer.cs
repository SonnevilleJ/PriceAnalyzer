namespace Sonneville.PriceTools.PriceAnalyzer
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.TickerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VolumeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpirationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CancelColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultChartStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.view_ChartStyle_CandleStick = new System.Windows.Forms.ToolStripMenuItem();
            this.view_ChartStyle_Ohlc = new System.Windows.Forms.ToolStripMenuItem();
            this.view_DefaultChartStyle_Line = new System.Windows.Forms.ToolStripMenuItem();
            this.chartStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.candleStickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oHLCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadStockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importPortfolioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.chartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewPendingOrdersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tradeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.automatedTradingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.chart1 = new Sonneville.PriceTools.PriceAnalyzer.Chart();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.elementHost2 = new System.Windows.Forms.Integration.ElementHost();
            this.positionSummaryPanel1 = new Sonneville.PriceTools.PriceAnalyzer.PositionSummaryPanel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.mainMenu.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "CSV files|*.csv";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(828, 343);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.elementHost1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(820, 317);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(820, 317);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Pending Orders";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TickerColumn,
            this.OrderTypeColumn,
            this.VolumeColumn,
            this.PriceColumn,
            this.ExpirationColumn,
            this.CancelColumn});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(820, 317);
            this.dataGridView1.TabIndex = 9;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // TickerColumn
            // 
            this.TickerColumn.HeaderText = "Ticker";
            this.TickerColumn.Name = "TickerColumn";
            this.TickerColumn.ReadOnly = true;
            // 
            // OrderTypeColumn
            // 
            this.OrderTypeColumn.HeaderText = "OrderType";
            this.OrderTypeColumn.Name = "OrderTypeColumn";
            this.OrderTypeColumn.ReadOnly = true;
            // 
            // VolumeColumn
            // 
            this.VolumeColumn.HeaderText = "Volume";
            this.VolumeColumn.Name = "VolumeColumn";
            this.VolumeColumn.ReadOnly = true;
            // 
            // PriceColumn
            // 
            this.PriceColumn.HeaderText = "Price";
            this.PriceColumn.Name = "PriceColumn";
            this.PriceColumn.ReadOnly = true;
            // 
            // ExpirationColumn
            // 
            this.ExpirationColumn.HeaderText = "Expiration";
            this.ExpirationColumn.Name = "ExpirationColumn";
            this.ExpirationColumn.ReadOnly = true;
            // 
            // CancelColumn
            // 
            this.CancelColumn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CancelColumn.HeaderText = "Cancel Order";
            this.CancelColumn.Name = "CancelColumn";
            this.CancelColumn.Text = "Cancel";
            this.CancelColumn.UseColumnTextForButtonValue = true;
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.viewToolStripMenuItem,
            this.dataToolStripMenuItem,
            this.insertToolStripMenuItem1,
            this.tradeToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(825, 24);
            this.mainMenu.TabIndex = 10;
            this.mainMenu.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "&File";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultChartStyleToolStripMenuItem,
            this.chartStyleToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // defaultChartStyleToolStripMenuItem
            // 
            this.defaultChartStyleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.view_ChartStyle_CandleStick,
            this.view_ChartStyle_Ohlc,
            this.view_DefaultChartStyle_Line});
            this.defaultChartStyleToolStripMenuItem.Name = "defaultChartStyleToolStripMenuItem";
            this.defaultChartStyleToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.defaultChartStyleToolStripMenuItem.Text = "Default Chart Style";
            // 
            // view_ChartStyle_CandleStick
            // 
            this.view_ChartStyle_CandleStick.Name = "view_ChartStyle_CandleStick";
            this.view_ChartStyle_CandleStick.Size = new System.Drawing.Size(135, 22);
            this.view_ChartStyle_CandleStick.Text = "Candlestick";
            this.view_ChartStyle_CandleStick.Click += new System.EventHandler(this.view_ChartStyle_CandleStick_Click);
            // 
            // view_ChartStyle_Ohlc
            // 
            this.view_ChartStyle_Ohlc.Name = "view_ChartStyle_Ohlc";
            this.view_ChartStyle_Ohlc.Size = new System.Drawing.Size(135, 22);
            this.view_ChartStyle_Ohlc.Text = "OHLC";
            this.view_ChartStyle_Ohlc.Click += new System.EventHandler(this.view_ChartStyle_Ohlc_Click);
            // 
            // view_DefaultChartStyle_Line
            // 
            this.view_DefaultChartStyle_Line.Name = "view_DefaultChartStyle_Line";
            this.view_DefaultChartStyle_Line.Size = new System.Drawing.Size(135, 22);
            this.view_DefaultChartStyle_Line.Text = "Line";
            this.view_DefaultChartStyle_Line.Click += new System.EventHandler(this.view_ChartStyle_Line_Click);
            // 
            // chartStyleToolStripMenuItem
            // 
            this.chartStyleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.candleStickToolStripMenuItem,
            this.oHLCToolStripMenuItem,
            this.lineToolStripMenuItem});
            this.chartStyleToolStripMenuItem.Name = "chartStyleToolStripMenuItem";
            this.chartStyleToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.chartStyleToolStripMenuItem.Text = "Chart Style";
            // 
            // candleStickToolStripMenuItem
            // 
            this.candleStickToolStripMenuItem.Name = "candleStickToolStripMenuItem";
            this.candleStickToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.candleStickToolStripMenuItem.Text = "Candle Stick";
            this.candleStickToolStripMenuItem.Click += new System.EventHandler(this.candleStickToolStripMenuItem_Click);
            // 
            // oHLCToolStripMenuItem
            // 
            this.oHLCToolStripMenuItem.Name = "oHLCToolStripMenuItem";
            this.oHLCToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.oHLCToolStripMenuItem.Text = "OHLC";
            this.oHLCToolStripMenuItem.Click += new System.EventHandler(this.oHLCToolStripMenuItem_Click);
            // 
            // lineToolStripMenuItem
            // 
            this.lineToolStripMenuItem.Name = "lineToolStripMenuItem";
            this.lineToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.lineToolStripMenuItem.Text = "Line";
            this.lineToolStripMenuItem.Click += new System.EventHandler(this.lineToolStripMenuItem_Click);
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadStockToolStripMenuItem,
            this.importCSVToolStripMenuItem,
            this.importPortfolioToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.dataToolStripMenuItem.Text = "&Data";
            // 
            // downloadStockToolStripMenuItem
            // 
            this.downloadStockToolStripMenuItem.Name = "downloadStockToolStripMenuItem";
            this.downloadStockToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.downloadStockToolStripMenuItem.Text = "Download Stock...";
            this.downloadStockToolStripMenuItem.Click += new System.EventHandler(this.downloadStockToolStripMenuItem_Click);
            // 
            // importCSVToolStripMenuItem
            // 
            this.importCSVToolStripMenuItem.Name = "importCSVToolStripMenuItem";
            this.importCSVToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.importCSVToolStripMenuItem.Text = "Import CSV...";
            this.importCSVToolStripMenuItem.Click += new System.EventHandler(this.importCSVToolStripMenuItem_Click);
            // 
            // importPortfolioToolStripMenuItem
            // 
            this.importPortfolioToolStripMenuItem.Name = "importPortfolioToolStripMenuItem";
            this.importPortfolioToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.importPortfolioToolStripMenuItem.Text = "Import Portfolio...";
            this.importPortfolioToolStripMenuItem.Click += new System.EventHandler(this.importPortfolioToolStripMenuItem_Click);
            // 
            // insertToolStripMenuItem1
            // 
            this.insertToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chartToolStripMenuItem,
            this.tableToolStripMenuItem,
            this.viewPendingOrdersToolStripMenuItem});
            this.insertToolStripMenuItem1.Name = "insertToolStripMenuItem1";
            this.insertToolStripMenuItem1.Size = new System.Drawing.Size(44, 20);
            this.insertToolStripMenuItem1.Text = "&Tabs";
            // 
            // chartToolStripMenuItem
            // 
            this.chartToolStripMenuItem.Name = "chartToolStripMenuItem";
            this.chartToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.chartToolStripMenuItem.Text = "New &Chart tab";
            this.chartToolStripMenuItem.Click += new System.EventHandler(this.chartToolStripMenuItem_Click);
            // 
            // tableToolStripMenuItem
            // 
            this.tableToolStripMenuItem.Name = "tableToolStripMenuItem";
            this.tableToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.tableToolStripMenuItem.Text = "New &Table tab";
            this.tableToolStripMenuItem.Click += new System.EventHandler(this.tableToolStripMenuItem_Click);
            // 
            // viewPendingOrdersToolStripMenuItem
            // 
            this.viewPendingOrdersToolStripMenuItem.Name = "viewPendingOrdersToolStripMenuItem";
            this.viewPendingOrdersToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.viewPendingOrdersToolStripMenuItem.Text = "Pending &Orders";
            this.viewPendingOrdersToolStripMenuItem.Click += new System.EventHandler(this.viewPendingOrdersToolStripMenuItem_Click);
            // 
            // tradeToolStripMenuItem
            // 
            this.tradeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buyToolStripMenuItem,
            this.sellToolStripMenuItem,
            this.automatedTradingToolStripMenuItem});
            this.tradeToolStripMenuItem.Name = "tradeToolStripMenuItem";
            this.tradeToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.tradeToolStripMenuItem.Text = "T&rade";
            // 
            // buyToolStripMenuItem
            // 
            this.buyToolStripMenuItem.Name = "buyToolStripMenuItem";
            this.buyToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.buyToolStripMenuItem.Text = "Buy";
            this.buyToolStripMenuItem.Click += new System.EventHandler(this.buyToolStripMenuItem_Click);
            // 
            // sellToolStripMenuItem
            // 
            this.sellToolStripMenuItem.Name = "sellToolStripMenuItem";
            this.sellToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.sellToolStripMenuItem.Text = "Sell";
            this.sellToolStripMenuItem.Click += new System.EventHandler(this.sellToolStripMenuItem_Click);
            // 
            // automatedTradingToolStripMenuItem
            // 
            this.automatedTradingToolStripMenuItem.Name = "automatedTradingToolStripMenuItem";
            this.automatedTradingToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.automatedTradingToolStripMenuItem.Text = "Automated Trading";
            this.automatedTradingToolStripMenuItem.Click += new System.EventHandler(this.automatedTradingToolStripMenuItem_Click);
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(3, 3);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(814, 311);
            this.elementHost1.TabIndex = 7;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.chart1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.elementHost2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(820, 317);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Position Summary";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // elementHost2
            // 
            this.elementHost2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost2.Location = new System.Drawing.Point(3, 3);
            this.elementHost2.Name = "elementHost2";
            this.elementHost2.Size = new System.Drawing.Size(814, 311);
            this.elementHost2.TabIndex = 0;
            this.elementHost2.Text = "elementHost2";
            this.elementHost2.Child = this.positionSummaryPanel1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 365);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.tabControl1);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PriceAnalyzer";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private IRenderer renderer;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem chartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importPortfolioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultChartStyleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem view_ChartStyle_CandleStick;
        private System.Windows.Forms.ToolStripMenuItem view_ChartStyle_Ohlc;
        private System.Windows.Forms.ToolStripMenuItem chartStyleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem candleStickToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oHLCToolStripMenuItem;
        private Chart chart1;
        private System.Windows.Forms.ToolStripMenuItem lineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem view_DefaultChartStyle_Line;
        private System.Windows.Forms.ToolStripMenuItem tradeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewPendingOrdersToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn TickerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn VolumeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpirationColumn;
        private System.Windows.Forms.DataGridViewButtonColumn CancelColumn;
        private System.Windows.Forms.ToolStripMenuItem automatedTradingToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Integration.ElementHost elementHost2;
        private PositionSummaryPanel positionSummaryPanel1;
    }
}

