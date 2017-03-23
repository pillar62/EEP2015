using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;

namespace sRT206
{
    public partial class Component : DataModule
    {
        public Component()
        {
            InitializeComponent();
        }

        public Component(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public double myval01()
        {
            double aamt = Convert.ToDouble(ucRTInvoiceSub.GetFieldCurrentValue("SALEAMT"));
            double rhrs = Convert.ToDouble(ucRTInvoiceSub.GetFieldCurrentValue("TAXAMT"));
            return aamt + rhrs;
        }
    }
}
