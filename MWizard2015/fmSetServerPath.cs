using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using System.IO;
using System.Xml;

namespace MWizard2015
{
    public partial class fmSetServerPath : Form
    {
        private AddIn FAddIn;

        public fmSetServerPath()
        {
            InitializeComponent();
            
        }

        public fmSetServerPath(AddIn addIn)
        {
            FAddIn = addIn;
            InitializeComponent();

            //String strAddinPath = WzdUtils.GetAddinsPath() + "\\ServerPath.xml";
            //if (File.Exists(strAddinPath))
            //{
            //    XmlDocument x = new XmlDocument();
            //    x.Load(strAddinPath);
            //    this.tbServerPath.Text = x.FirstChild.ChildNodes[0].Attributes["Value"].Value;
            //}
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.tbServerPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            String strAddinPath = WzdUtils.GetAddinsPath() + "\\ServerPath.xml";
            if (!File.Exists(strAddinPath))
            {
                FileStream fs = File.Open(strAddinPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("<Infolight></Infolight>");
                sw.Close();
                fs.Close();
                XmlDocument x = new XmlDocument();
                x.Load(strAddinPath);
                XmlNode xn = x.CreateNode(XmlNodeType.Element, "ServerPath", "");
                XmlAttribute xa = x.CreateAttribute("Value");
                xa.Value = this.tbServerPath.Text;

                xn.Attributes.Append(xa);
                x.ChildNodes[0].AppendChild(xn);

                xn = x.CreateNode(XmlNodeType.Element, "WebClientPath", "");
                xa = x.CreateAttribute("Value");
                xa.Value = this.tbWebClientPath.Text;

                xn.Attributes.Append(xa);
                x.ChildNodes[0].AppendChild(xn);
                x.Save(strAddinPath);
            }
            else
            {
                XmlDocument x = new XmlDocument();
                x.Load(strAddinPath);
                x.FirstChild.ChildNodes[0].Attributes["Value"].Value = this.tbServerPath.Text;

                if (x.FirstChild.ChildNodes.Count < 2)
                {
                    XmlNode xn = x.CreateNode(XmlNodeType.Element, "WebClientPath", "");
                    XmlAttribute xa = x.CreateAttribute("Value");
                    xa.Value = this.tbWebClientPath.Text;
                    xn.Attributes.Append(xa);
                    x.ChildNodes[0].AppendChild(xn);
                }
                else
                {
                    x.FirstChild.ChildNodes[1].Attributes["Value"].Value = this.tbWebClientPath.Text;
                }
                x.Save(strAddinPath);
            }
            this.Hide();
        }

        private void fmSetServerPath_Load(object sender, EventArgs e)
        {
            this.tbServerPath.Text = WzdUtils.GetServerPath(FAddIn, false);

            this.tbWebClientPath.Text = WzdUtils.GetWebClientPath(FAddIn, false);
        }

        private void btnSelectFolder2_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.tbWebClientPath.Text = this.folderBrowserDialog2.SelectedPath;
            }
        }
    }
}
