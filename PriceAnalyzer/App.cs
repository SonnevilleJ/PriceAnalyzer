﻿using System;
using System.Windows.Forms;

namespace Sonneville.PriceTools.PriceAnalyzer
{
    static class App
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
