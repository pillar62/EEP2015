using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Srvtools;

namespace sRT2051
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
        public string getFix()
        {
            return string.Format("{0:yyMMdd}", DateTime.Now.Date);
        }
    }
}
