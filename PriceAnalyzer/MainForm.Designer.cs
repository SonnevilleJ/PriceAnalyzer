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
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.chart1 = new Sonneville.PriceTools.PriceAnalyzer.Chart();
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
            this.tradeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.mainMenu.SuspendLayout();
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
            this.tableToolStripMenuItem});
            this.insertToolStripMenuItem1.Name = "insertToolStripMenuItem1";
            this.insertToolStripMenuItem1.Size = new System.Drawing.Size(44, 20);
            this.insertToolStripMenuItem1.Text = "&Tabs";
            // 
            // chartToolStripMenuItem
            // 
            this.chartToolStripMenuItem.Name = "chartToolStripMenuItem";
            this.chartToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.chartToolStripMenuItem.Text = "New &Chart tab";
            this.chartToolStripMenuItem.Click += new System.EventHandler(this.chartToolStripMenuItem_Click);
            // 
            // tableToolStripMenuItem
            // 
            this.tableToolStripMenuItem.Name = "tableToolStripMenuItem";
            this.tableToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.tableToolStripMenuItem.Text = "New &Table tab";
            this.tableToolStripMenuItem.Click += new System.EventHandler(this.tableToolStripMenuItem_Click);
            // 
            // tradeToolStripMenuItem
            // 
            this.tradeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buyToolStripMenuItem,
            this.sellToolStripMenuItem});
            this.tradeToolStripMenuItem.Name = "tradeToolStripMenuItem";
            this.tradeToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.tradeToolStripMenuItem.Text = "T&rade";
            // 
            // buyToolStripMenuItem
            // 
            this.buyToolStripMenuItem.Name = "buyToolStripMenuItem";
            this.buyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.buyToolStripMenuItem.Text = "Buy";
            this.buyToolStripMenuItem.Click += new System.EventHandler(this.buyToolStripMenuItem_Click);
            // 
            // sellToolStripMenuItem
            // 
            this.sellToolStripMenuItem.Name = "sellToolStripMenuItem";
            this.sellToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sellToolStripMenuItem.Text = "Sell";
            this.sellToolStripMenuItem.Click += new System.EventHandler(this.sellToolStripMenuItem_Click);
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
            this.Text = "PriceAnalyzer";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private IRenderer renderer1;
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
    }
}

