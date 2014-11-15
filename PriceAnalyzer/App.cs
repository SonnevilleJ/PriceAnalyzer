using System;
using System.Windows.Forms;
using Sonneville.PriceTools.Fidelity;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    static class App
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var transactionHistoryCsvFile = new FidelityTransactionHistoryCsvFile();
            var form = new MainForm(transactionHistoryCsvFile);
            Application.Run(form);
        }
    }
}
