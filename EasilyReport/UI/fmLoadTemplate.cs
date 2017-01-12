using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infolight.EasilyReportTools.DataCenter;
using Infolight.EasilyReportTools.Tools;
using System.IO;
using System.Collections;

namespace Infolight.EasilyReportTools.UI
{
    public partial class fmLoadTemplate : Form
    {
        //private IReport report;
        //private bool isDesignTime;
        private DBGateway dbGateway;
        //private TemplateLoadMode templateLoadMode;
        private bool designMode;
        public fmLoadTemplate(IReport rpt, bool designTime)
        {
            InitializeComponent();
            //   this.report = rpt;
            //this.isDesignTime = designTime;
            dbGateway = new DBGateway(rpt);
            //  serializer = new BinarySerialize();
            // templateLoadMode = loadMode;
            designMode = designTime;
            if (!designTime)
            {
                this.Text = ERptMultiLanguage.GetLanValue("fmLoadTemplate");
                lbSelectTemplate.Text = ERptMultiLanguage.GetLanValue("lbSelectTemplate");
                btOK.Text = ERptMultiLanguage.GetLanValue("btOK");
                btCancel.Text = ERptMultiLanguage.GetLanValue("btClose");
                buttonDelete.Text = ERptMultiLanguage.GetLanValue("btDelete");
            }
        }

        private string templateName;

        public string TemplateName
        {
            get { return templateName; }
        }

        private void fmLoadTemplate_Load(object sender, EventArgs e)
        {
            BindingData();
            //foreach (string filename in filenames)
            //{
            //    lbxTemplate.Items.Add(filename);
            //}
           
            //if (this.lbxTemplate.Items.Count == 0)
            //{
            //    this.Close();
            //}
        }

        private void BindingData()
        {
            DictionaryEntry[] filenames = dbGateway.GetTemplates(designMode);
            if (filenames.Length == 0)
            {
                this.Close();
            }
            lbxTemplate.DataSource = filenames;
            lbxTemplate.DisplayMember = "Value";
            lbxTemplate.ValueMember = "Key";

        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (this.lbxTemplate.SelectedItem != null)
            {
                DictionaryEntry entry = (DictionaryEntry)this.lbxTemplate.SelectedItem;
                string filename = entry.Value.ToString();
                string reportid = entry.Key.ToString();
                dbGateway.LoadTemplate(reportid, filename);
                templateName = filename;
            }
            else
            {
                DialogResult = DialogResult.None;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (this.lbxTemplate.SelectedItem != null)
            {
                if (MessageBox.Show(MessageInfo.TemplateDeleteConfirm, "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    DictionaryEntry entry = (DictionaryEntry)this.lbxTemplate.SelectedItem;
                    string filename = entry.Value.ToString();
                    string reportid = entry.Key.ToString();
                    dbGateway.DeleteTemplate(reportid, filename);
                    //lbxTemplate.Items.Remove(this.lbxTemplate.SelectedItem);
                    BindingData();
                }
            }
        }
    }
}