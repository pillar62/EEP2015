using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Infolight.EasilyReportTools.Tools;

namespace Infolight.EasilyReportTools.UI
{
    public partial class fmPictures : Form
    {
        private IReport report;
        internal string SelectedIndex;
        //private List<DictionaryEntry> pictureList = new List<DictionaryEntry>();
        //private DataTable dtImageList;

        public fmPictures(IReport rpt)
        {
            InitializeComponent();

            #region Init Language
            this.Text = ERptMultiLanguage.GetLanValue("fmPictures");
            this.btAdd.Text = ERptMultiLanguage.GetLanValue("btAdd");
            this.btRemove.Text = ERptMultiLanguage.GetLanValue("btRemove");
            this.btChange.Text = ERptMultiLanguage.GetLanValue("btChange");
            this.btSelect.Text = ERptMultiLanguage.GetLanValue("btSelect");
            this.lbPictures.Text = ERptMultiLanguage.GetLanValue("lbPictures");
            #endregion

            this.report = rpt;
        }

        private void InitializeListBox()
        {
            lbxPicutres.Items.Clear();
            for (int i = 0; i < report.Images.Count; i++)
            {
                lbxPicutres.Items.Add(report.Images[i].Name);
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.gif;*.jpg;*.jpeg;*.bmp;*.wmf;*.png)|*.gif;*.jpg;*.jpeg;*.bmp;*.wmf;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ImageItem imageItem = new ImageItem();
                imageItem.Name = "ImageItem";
                imageItem.Image = Image.FromFile(openFileDialog.FileName);
                this.report.Images.Add(imageItem);
                InitializeListBox();
                lbxPicutres.SelectedIndex = this.report.Images.IndexOf(imageItem);
                lbxPicutres_SelectedIndexChanged(lbxPicutres, new EventArgs());
            }
        }


        private void btRemove_Click(object sender, EventArgs e)
        {
            if (lbxPicutres.SelectedIndex != -1)
            {
                this.report.Images.RemoveAt(lbxPicutres.SelectedIndex);
                InitializeListBox();
                lbxPicutres.SelectedIndex = Math.Min(lbxPicutres.SelectedIndex, lbxPicutres.Items.Count - 1);
                lbxPicutres_SelectedIndexChanged(lbxPicutres, new EventArgs());
            }
        }

        private void btChange_Click(object sender, EventArgs e)
        {
            if (lbxPicutres.SelectedIndex != -1)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files(*.gif;*.jpg;*.jpeg;*.bmp;*.wmf;*.png)|*.gif;*.jpg;*.jpeg;*.bmp;*.wmf;*.png";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.report.Images[lbxPicutres.SelectedIndex].Image = Image.FromFile(openFileDialog.FileName);
                    lbxPicutres_SelectedIndexChanged(lbxPicutres, new EventArgs());
                }
            }
        }

        private void fmPictures_Load(object sender, EventArgs e)
        {
            InitializeListBox();

        }

        private void lbxPicutres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxPicutres.SelectedIndex != -1)
            {
                this.pictureBox.Image = this.report.Images[lbxPicutres.SelectedIndex].Image;
            }
            else
            {
                this.pictureBox.Image = null;
            }
        }

        private void tbPictureName_TextChanged(object sender, EventArgs e)
        {
            if (lbxPicutres.SelectedIndex != -1)
            {
                this.report.Images[0].Name = tbPictureName.Text.Trim();
            }
        }

        private void btSelect_Click(object sender, EventArgs e)
        {
            if (lbxPicutres.SelectedIndex != -1)
            {
                this.SelectedIndex = lbxPicutres.SelectedIndex.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        

       
    }
}