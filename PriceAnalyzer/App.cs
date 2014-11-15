using System;
using System.Windows.Forms;
using Ninject;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    static class App
    {
        private static readonly IKernel Kernel;

        static App()
        {
            Kernel = KernelBuilder.Build();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = Kernel.Get<MainForm>();
            Application.Run(form);
        }
    }
}
