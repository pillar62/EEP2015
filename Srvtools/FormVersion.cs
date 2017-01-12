using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Srvtools
{
    public partial class FormVersion : Form
    {
        public FormVersion(string text)
        {
            InitializeComponent();
            this.Text = text;
        }

        private void FormVersion_Load(object sender, EventArgs e)
        {
            labelApplication.Text = GetFileInfomation(Assembly.GetEntryAssembly());
            labelSrvtools.Text = GetFileInfomation(Assembly.GetExecutingAssembly());
            labelInfoRemoteModule.Text = GetFileInfomation(Assembly.GetAssembly(typeof(EEPRemoteModule)));
            if (File.Exists(Application.StartupPath + "\\FLRunTime.dll"))
            {
                labelFLRunTime.Text = GetFileInfomation(Application.StartupPath + "\\FLRunTime.dll", false);
            }
            else
            {
                this.Height = 298;
                groupBox4.Visible = false;
            }
            if (this.Owner != null && this.Owner is Form)
            {
                Form owner = this.Owner as Form;
                if (owner.Icon != null)
                {
                    pictureBoxApplication.Image = owner.Icon.ToBitmap();
                }
            }
            Icon icon = GetLargeIcon(Environment.SystemDirectory + "\\shell32.dll");
            if (icon != null)
            {
                Bitmap bmp = icon.ToBitmap();
                pictureBoxSrvtools.Image = bmp;
                pictureBoxInfoRemoteModule.Image = bmp;
                pictureBoxFLRunTime.Image = bmp;
            }
        }

        private void buttonAssembly_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Application.StartupPath;
            dialog.Title = "Select a file:";
            dialog.Filter = "Assembly File(*.exe;*.dll)|*.exe;*.dll";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                MessageBox.Show(this, GetFileInfomation(dialog.FileName, true), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GetFileInfomation(string filename, bool includecaption)
        {
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(filename);
            StringBuilder builder = new StringBuilder();
            if (includecaption)
            {
                builder.AppendLine(string.Format("Name:\t\t{0}", Path.GetFileName(filename)));
                builder.AppendLine(string.Format("Version:\t\t{0}", info.FileVersion));
                builder.AppendLine(string.Format("Description:\t{0}", info.Comments));
            }
            else
            {
                builder.AppendLine(Path.GetFileName(filename));
                builder.AppendLine(info.FileVersion);
                builder.AppendLine(info.Comments);
            }
            return builder.ToString();
        }

        private string GetFileInfomation(Assembly assembly)
        {
            return GetFileInfomation(assembly.Location, false);
        }

        [DllImport("shell32.dll ", EntryPoint = "SHGetFileInfo")]
        public static extern int GetFileInfo(string pszPath, int dwFileAttributes,
            ref   FileInfoStruct psfi, int cbFileInfo, int uFlags);

        public enum FileInfoFlags
        {
            SHGFI_ICON = 0x000000100,   //   get icon  
            SHGFI_DISPLAYNAME = 0x000000200,   //   get display name  
            SHGFI_TYPENAME = 0x000000400,   //   get type name  
            SHGFI_ATTRIBUTES = 0x000000800,   //   get attributes  
            SHGFI_ICONLOCATION = 0x000001000,   //   get icon location  
            SHGFI_EXETYPE = 0x000002000,   //   return exe type  
            SHGFI_SYSICONINDEX = 0x000004000,   //   get system icon index  
            SHGFI_LINKOVERLAY = 0x000008000,   //   put a link overlay on icon   SHGFI_SELECTED   = 0x000010000 ,   //   show icon in selected state  
            SHGFI_ATTR_SPECIFIED = 0x000020000,   //   get only specified attributes  
            SHGFI_LARGEICON = 0x000000000,   //   get large icon  
            SHGFI_SMALLICON = 0x000000001,   //   get small icon  
            SHGFI_OPENICON = 0x000000002,   //   get open icon  
            SHGFI_SHELLICONSIZE = 0x000000004,   //   get shell size icon  
            SHGFI_PIDL = 0x000000008,   //   pszPath is a pidl  
            SHGFI_USEFILEATTRIBUTES = 0x000000010,   //   use passed dwFileAttribute  
            SHGFI_ADDOVERLAYS = 0x000000020,   //   apply the appropriate overlays  
            SHGFI_OVERLAYINDEX = 0x000000040    //   Get the index of the overlay  
        }

        [StructLayout( LayoutKind.Sequential)]  
        public struct FileInfoStruct
        {  
            public IntPtr hIcon;  
            public int iIcon;  
            public int dwAttributes;  
            [ MarshalAs( UnmanagedType.ByValTStr, SizeConst = 260 )]  
            public string szDisplayName;  
            [ MarshalAs( UnmanagedType.ByValTStr, SizeConst = 80 )]  
            public string szTypeName;  
        }

        public static Icon GetLargeIcon(string pFilePath)
        {
            FileInfoStruct _info = new FileInfoStruct();
            GetFileInfo(pFilePath, 0, ref _info, Marshal.SizeOf(_info),
                (int)(FileInfoFlags.SHGFI_ICON | FileInfoFlags.SHGFI_LARGEICON));
            try
            {
                return Icon.FromHandle(_info.hIcon);
            }
            catch
            {
                return null;
            }
        }

        public static Icon GetSmallIcon(string pFilePath)
        {
            FileInfoStruct _info = new FileInfoStruct();
            GetFileInfo(pFilePath, 0, ref _info, Marshal.SizeOf(_info),
                (int)(FileInfoFlags.SHGFI_ICON | FileInfoFlags.SHGFI_SMALLICON));
            try
            {
                return Icon.FromHandle(_info.hIcon);
            }
            catch
            {
                return null;
            }
        }
    }
  
   
  
}