using System;
using Ninject;
using NUnit.Framework;

namespace Sonneville.PriceTools.PriceAnalyzer.Test
{
    [TestFixture]
    public class KernelBuilderTest
    {
        [STAThread]
        [Test]
        public void CanGetMainForm()
        {
            var kernel = KernelBuilder.Build();

            var mainForm = kernel.Get<MainForm>();

            Assert.IsNotNull(mainForm);
        }
    }
}