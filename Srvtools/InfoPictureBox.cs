using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Drawing.Design;

namespace Srvtools
{
    public class InfoPictureBox: PictureBox, ISupportInitialize
    {
        public InfoPictureBox()
        {
            _ImagePath = "";
            DefaultPath = "";
        }

        
        public enum WinImageStyle
        {
            ImageField,
            VarCharField
        }

        private string _ImagePath;
        [Category("Infolight"),
        Description("The path to bind")]
        [Bindable(true)]
        [Browsable(false)]
        public string ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }

        private WinImageStyle _ImageStyle;
        [Category("Infolight"),
        Description("The style of the image stored in server")]
        public WinImageStyle ImageStyle
        {
            get { return _ImageStyle; }
            set { _ImageStyle = value; }
        }

        private string _DefaultPath;
        [Category("Infolight"),
        Description("The path of the image stored in server")]
        [Editor(typeof(System.Windows.Forms.Design.FolderNameEditor),typeof(UITypeEditor))]
	    public string DefaultPath
	    {
		    get { return _DefaultPath;}
		    set { _DefaultPath = value;}
	    }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            string filename = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "BMP(*.bmp)|*.bmp|JPEG(*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG(*.png)|*.png|ICO(*.ico)|*.ico";
            ofd.Title = "Select a new Image file";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = ofd.FileName;
                
                if (this.ImageStyle == WinImageStyle.VarCharField)
                {
                    if ((this.DataBindings["ImagePath"].DataSource as InfoBindingSource).BeginEdit())
                    {
                        this.Image = System.Drawing.Image.FromFile(filename);
                        this.ImagePath = UploadImage(filename);
                        this.DataBindings["ImagePath"].WriteValue();//new add to solve the problem when insert image to datasource
                    }
                }
                else
                {
                    if ((this.DataBindings["Image"].DataSource as InfoBindingSource).BeginEdit())
                    {
                        this.Image = System.Drawing.Image.FromFile(filename);
                        this.DataBindings["Image"].WriteValue();//new add to solve the problem when insert image to datasource
                    }
                }
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (this.ImageStyle == WinImageStyle.ImageField)
            {
                this.DataBindings["Image"].WriteValue();
            }
            else
            {
                this.DataBindings["ImagePath"].WriteValue();
            }

        }

        private string UploadImage(string filename)
        {
            string file = this.DefaultPath + "\\" + Path.GetFileName(filename);
            if (!CliUtils.UpLoad(filename,file))
            {
                file = this.DefaultPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(filename);
                CliUtils.UpLoad(filename, file);
            }
            return Path.GetFileName(file);
        }

        #region ISupportInitialize Members

        void ISupportInitialize.BeginInit()
        {
        }

        void ISupportInitialize.EndInit()
        {
            if(!this.DesignMode)
            {
                if (this.ImageStyle == WinImageStyle.VarCharField)
                {
                    if (this.DataBindings["ImagePath"] == null)
                    {
                        throw new Exception("Binding of ImagePath is null");
                    }
                    this.DataBindings["ImagePath"].BindingComplete += new BindingCompleteEventHandler(InfoPictureBox_BindingComplete);
                }
                else
                {
                    if (this.DataBindings["Image"] == null)
                    {
                        throw new Exception("Binding of Image is null");
                    }
                }
            }
        }

        void InfoPictureBox_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            object[] myRet = CliUtils.CallMethod("GLModule", "DownLoadFile", new object[] { this.DefaultPath + "\\" + this.ImagePath });
            if (myRet != null && (int)myRet[0] == 0)
            {
                if ((int)myRet[1] == 0)
                {
                    byte[] bfile = (byte[])myRet[2];
                    MemoryStream ms = new MemoryStream(bfile);
                    this.Image = System.Drawing.Image.FromStream(ms);
                }
                else
                {
                    this.Image = null;
                }
            }       
        }

        #endregion
    }
}
