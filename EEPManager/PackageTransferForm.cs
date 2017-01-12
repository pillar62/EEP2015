using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using Srvtools;

using System.Runtime.Remoting;

using System.Reflection;

namespace EEPManager
{
    internal partial class PackageTransferForm : Form
    {
        //这个文件也基本要全改
        private Thread thread = null;
        private PackageCollection packageCollection = null;
        private string LocalPackagePath;

        internal PackageTransferForm(PackageCollection dlls, string localPackagePath)
        {
            InitializeComponent();
            packageCollection = dlls;
            LocalPackagePath = localPackagePath;
        }

        private void DllTransferForm_Load(object sender, EventArgs e)
        {
            if (packageCollection.Action == PackageAction.Download)
            {
                for (int i = 0; i < this.packageCollection.Packages.Count; ++ i)
                {
                    DataRow row = this.transferTable.NewRow();
                    row["Name"] = packageCollection.Packages[i];
                    row["State"] = "Waiting to download.";
                    row["DateTime"] = packageCollection.DateTimes[i];
                    this.transferTable.Rows.Add(row);
                }
            }
            else if (packageCollection.Action == PackageAction.Upload)
            {
                for (int i = 0; i < this.packageCollection.Packages.Count; ++ i)
                {
                    DataRow row = this.transferTable.NewRow();
                    row["Name"] = packageCollection.Packages[i];
                    row["State"] = "Waiting to upload.";
                    row["DateTime"] = packageCollection.DateTimes[i];
                    this.transferTable.Rows.Add(row);
                }
            }
        }

        private delegate void SetTextMehtod(Control ctrl, string text);

        private void SetText(Control ctrl, string text)
        {
            ctrl.Text = text;
        }
        private void Download()
        {
            SetTextMehtod setTextMethod =
               Delegate.CreateDelegate(typeof(SetTextMehtod), this, "SetText") as SetTextMehtod;
            this.Invoke(setTextMethod, new object[] { this.btn, "Cancel" });

            //btn.Text = "Cancel";
            foreach (DataRow row in this.transferTable.Rows)
            {
                row["State"] = "Downloading ...";

                string fileName = this.LocalPackagePath + "\\" + row["Name"].ToString();
                // call method
                // Download
                object[] myRet = CliUtils.CallMethod("GLModule", "PackageDownLoad", new object[] { packageCollection.Solution, row["Name"].ToString(), row["DateTime"].ToString(), packageCollection.Type});
                if (myRet != null && (int)myRet[0] == 0)
                {
                    if ((int)myRet[1] == 0)
                    {
                        byte[] buffer = (byte[])myRet[2];
                        try
                        {
                            if (File.Exists(fileName))
                            {
                                try
                                {
                                    File.Delete(fileName);
                                }
                                catch
                                {
                                    row["State"] = "Download failed : the local package file currently can not be overwritten.";
                                    continue;
                                }
                            }

                            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                            fs.Write(buffer, 0, buffer.Length);
                            fs.Close();
                            File.SetLastWriteTime(fileName, (DateTime)myRet[3]);
                            row["State"] = "Downloaded successfully.";
                        }
                        catch
                        {
                            row["State"] = "Downloading error";
                        } 
                    }
                    else
                    {
                        row["State"] = "Downloading error";//error

                    }
                }
            }
            //btn.Text = "Close";
          
            this.Invoke(setTextMethod, new object[] { this.btn, "Close" });

            this.nameDataGridViewTextBoxColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            this.stateDataGridViewTextBoxColumn.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        private void Upload()
        {
            SetTextMehtod setTextMethod =
             Delegate.CreateDelegate(typeof(SetTextMehtod), this, "SetText") as SetTextMehtod;
            this.Invoke(setTextMethod, new object[] { this.btn, "Cancel" });
            //btn.Text = "Cancel";

            foreach (DataRow row in this.transferTable.Rows)
            {
                row["State"] = "Uploading ...";

                string fileName = this.LocalPackagePath + "\\" + row["Name"].ToString();

                // Read file
                byte[] buffer = null;
                DateTime dt = new DateTime();
                if (File.Exists(fileName))
                {
                    dt = File.GetLastWriteTime(fileName);
                    try
                    {
                        FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                        buffer = new byte[fileStream.Length];
                        fileStream.Read(buffer, 0, (int)fileStream.Length);
                        fileStream.Close();
                    }
                    catch
                    {
                        row["State"] = "Upload failed : can not load package file.";
                        continue;
                    }
                }
                else
                {
                    row["State"] = "Upload failed : can not find package file.";
                    continue;
                }

                object[] myRet = CliUtils.CallMethod("GLModule", "PackageUpload", new object[] { packageCollection.Solution, row["Name"].ToString(), dt, packageCollection.Type, buffer });
                if (packageCollection.Type == PackageType.Client)
                {
                    string fileNameXml = this.LocalPackagePath + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".xml";
                    if (File.Exists(fileNameXml))
                    {
                        byte[] bufferxml = null;

                        try
                        {
                            FileStream fileStream = new FileStream(fileNameXml, FileMode.Open, FileAccess.Read);
                            bufferxml = new byte[fileStream.Length];
                            fileStream.Read(bufferxml, 0, (int)fileStream.Length);
                            fileStream.Close();

                        }
                        catch
                        {
                            row["State"] = "Upload failed : can not load package xml.";
                            continue;
                        }
                        CliUtils.CallMethod("GLModule", "PackageUpload", new object[] { packageCollection.Solution, Path.GetFileNameWithoutExtension(fileName) + ".xml", dt, packageCollection.Type, bufferxml });

                    }
                }
                if (myRet != null && (int)myRet[0] == 0)
                {
                    row["State"] = "Uploaded successfully.";
                }

            }
            //btn.Text = "Close";
            this.Invoke(setTextMethod, new object[] { this.btn, "Close" });
            this.nameDataGridViewTextBoxColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            this.stateDataGridViewTextBoxColumn.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        private void dgLocalPackage_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void DllTransferForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread != null && thread.ThreadState != ThreadState.Stopped)
            {
                if (MessageBox.Show("Transfer unfinished, package file may be DESTROYED\n\r"
                    + "are you sure to quit?", "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                    == DialogResult.OK)
                {
                    thread.Abort();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (packageCollection.Action == PackageAction.Download)
            {
                thread = new Thread(new ThreadStart(Download));
            }
            else if(packageCollection.Action == PackageAction.Upload)
            {
                thread = new Thread(new ThreadStart(Upload));
            }
            thread.Start();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            if (btn.Text == "Start")
            {
                if (packageCollection.Action == PackageAction.Download)
                {
                    thread = new Thread(new ThreadStart(Download));
                }
                else if (packageCollection.Action == PackageAction.Upload)
                {
                    thread = new Thread(new ThreadStart(Upload));
                }
                thread.Start();
            }
            else
            {
                this.Close();
            }
        }
    }
}