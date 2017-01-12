using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace InitEEP
{
    public partial class InitEEP : Form
    {
//#if VS90
//        const string REGISTRYNAME = "infolight\\eep.net2008";
//#else
        const string REGISTRYNAME = "infolight\\eep.net";
//#endif

        public InitEEP()
        {
            InitializeComponent();

            AssemblyTitleAttribute attribute = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute));

            this.Text = attribute.Title;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dirDlg.SelectedPath = textBox1.Text;
            if (dirDlg.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dirDlg.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dirDlg.SelectedPath = textBox2.Text;
            if (dirDlg.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dirDlg.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(string.Format("SOFTWARE\\{0}", REGISTRYNAME), true);

            if (rk == null)
            {
                rk = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);                
                rk = rk.CreateSubKey(REGISTRYNAME, RegistryKeyPermissionCheck.ReadWriteSubTree);
            }

            String s = textBox1.Text.Trim();
            rk.SetValue("Server Path", s);
            s = textBox2.Text.Trim();
            rk.SetValue("Client Path", s);
            s = textBox4.Text.Trim();
            rk.SetValue("WebClient Path", s);
            rk.SetValue("DatabaseType", String.Empty);
            rk.SetValue("WizardConnectionString", String.Empty);
            s = textBox5.Text.Trim();
            rk.SetValue("Addins Path", s);
            rk.Close();
            
            if (IntPtr.Size == 8)
            {
                rk = Registry.LocalMachine.OpenSubKey(string.Format("SOFTWARE\\Wow6432Node\\{0}", REGISTRYNAME), true);
                if (rk == null)
                {
                    rk = Registry.LocalMachine.OpenSubKey(string.Format("SOFTWARE\\Wow6432Node", REGISTRYNAME), true);
                    rk = rk.CreateSubKey(REGISTRYNAME, RegistryKeyPermissionCheck.ReadWriteSubTree);
                }
                s = textBox1.Text.Trim();
                rk.SetValue("Server Path", s);
                s = textBox2.Text.Trim();
                rk.SetValue("Client Path", s);
                s = textBox4.Text.Trim();
                rk.SetValue("WebClient Path", s);
                rk.SetValue("DatabaseType", String.Empty);
                rk.SetValue("WizardConnectionString", String.Empty);
                s = textBox5.Text.Trim();
                rk.SetValue("Addins Path", s);
                rk.Close();
            }            

            rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\AssemblyFolders\\EEP2015", true);
            if (rk == null)
            { 
                rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\AssemblyFolders",true);
                if (rk != null)
                {
                    rk = rk.CreateSubKey("EEP2015", RegistryKeyPermissionCheck.ReadWriteSubTree);
                }
            }
            if (rk != null)
            {
                rk.SetValue("", textBox1.Text.Trim());
                rk.Close();
            }
        }

        private void InitEEP_Load(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(string.Format("SOFTWARE\\{0}", REGISTRYNAME));

            if (IntPtr.Size == 8)
            {
                rk = Registry.LocalMachine.OpenSubKey(string.Format("SOFTWARE\\Wow6432Node\\{0}", REGISTRYNAME));
            }

            if (null == rk) return;

            object o = rk.GetValue("Server Path");
            if (null != o)
                textBox1.Text = (string)o;
            else
            {
                textBox1.Text = "C:\\Program Files (x86)\\InfoLight\\EEP2015\\EEPNetServer";
            }

            o = rk.GetValue("Client Path");
            if (null != o)
                textBox2.Text = (string)o;
            else
            {
                textBox2.Text = "C:\\Program Files (x86)\\InfoLight\\EEP2015\\EEPNetClient";
            }

            o = rk.GetValue("WebClient Path");
            if (null != o)
                textBox4.Text = (string)o;
            else
            {
                textBox4.Text = "C:\\Program Files (x86)\\InfoLight\\EEP2015\\EEPWebClient";
            }
            o = rk.GetValue("Addins Path");
            if (null != o)
                textBox5.Text = (string)o;
            else
            {
                textBox5.Text = "C:\\Program Files (x86)\\InfoLight\\EEP2015\\Addins";
            }

            rk.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                string filename, arguments;
                string systemDrive = System.Environment.SystemDirectory.Substring(0, 1);       
                filename = systemDrive + @":\Program Files\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools\GACUtil.exe";

                if (IntPtr.Size == 8)
                {
                    filename = systemDrive + @":\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools\GACUtil.exe";
                }

                ProcessStartInfo psi;
                string result = "";

                string sSys = System.Environment.SystemDirectory;
                int iPos = sSys.LastIndexOf('\\');
                sSys = sSys.Substring(0, iPos);
                sSys = sSys + "\\Assembly\\";// now we get global assembly cache path...

                if (!File.Exists(textBox1.Text.Trim() + "\\" + "InfoRemoteModule.dll"))
                {
                    MessageBox.Show("Not found 'InfoRemoteModule.dll' in server directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!File.Exists(textBox1.Text.Trim() + "\\" + "Srvtools.dll"))
                {
                    MessageBox.Show("Not found 'Srvtools.dll' in server directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                arguments = " -u InfoRemoteModule";
                psi = new ProcessStartInfo(filename, arguments);
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                result = Process.Start(psi).StandardOutput.ReadToEnd();
                result += "\r\n------------------------------------------------------------------------------\r\n";

                arguments = " -i \"" + textBox1.Text.Trim() + "\\" + "InfoRemoteModule.dll\"";
                psi = new ProcessStartInfo(filename, arguments);
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                result = (result + Process.Start(psi).StandardOutput.ReadToEnd());
                result += "\r\n------------------------------------------------------------------------------\r\n";

                arguments = " -u Srvtools";
                psi = new ProcessStartInfo(filename, arguments);
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                result = (result + Process.Start(psi).StandardOutput.ReadToEnd());
                result += "\r\n------------------------------------------------------------------------------\r\n";

                arguments = " -i \"" + textBox1.Text.Trim() + "\\" + "Srvtools.dll\"";
                psi = new ProcessStartInfo(filename, arguments);
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                result = (result + Process.Start(psi).StandardOutput.ReadToEnd());

                MessageBox.Show(result, "InitEEP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dirDlg.SelectedPath = textBox4.Text;
            if (dirDlg.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = dirDlg.SelectedPath;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox4.Text.Trim() != "")
            {
                if (!File.Exists(textBox4.Text + "\\Init\\InfoRemoteModule.dll"))
                {
                    MessageBox.Show(textBox4.Text + "\\Init\\InfoRemoteModule.dll does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!File.Exists(textBox4.Text + "\\Init\\Srvtools.dll"))
                {
                    MessageBox.Show(textBox4.Text + "\\Init\\Srvtools.dll does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string filename, arguments;
                string systemDrive = System.Environment.SystemDirectory.Substring(0, 1);
                filename = systemDrive + @":\Program Files\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\GACUtil.exe";

                ProcessStartInfo psi;
                string result = "";

                string sSys = System.Environment.SystemDirectory;
                int iPos = sSys.LastIndexOf('\\');
                sSys = sSys.Substring(0, iPos);
                sSys = sSys + "\\Assembly\\";// now we get global assembly cache path...

                //arguments = " -u InfoRemoteModule";
                //psi = new ProcessStartInfo(filename, arguments);
                //psi.UseShellExecute = false;
                //Process.Start(psi);

                //arguments = " -i \"" + textBox4.Text.Trim() + "\\Init\\" + "InfoRemoteModule.dll\"";
                //psi = new ProcessStartInfo(filename, arguments);
                //Process.Start(psi);

                ////---------------------------------------------------------------------

                //arguments = " -u Srvtools";
                //psi = new ProcessStartInfo(filename, arguments);
                //psi.UseShellExecute = false;
                //Process.Start(psi);

                //arguments = " -i \"" + textBox4.Text.Trim() + "\\Init\\" + "Srvtools.dll\"";
                //psi = new ProcessStartInfo(filename, arguments);
                //Process.Start(psi);


                arguments = " -u InfoRemoteModule";
                psi = new ProcessStartInfo(filename, arguments);
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                result = Process.Start(psi).StandardOutput.ReadToEnd();
                result += "\r\n------------------------------------------------------------------------------\r\n";

                arguments = " -i \"" + textBox4.Text.Trim() + "\\Init\\" + "InfoRemoteModule.dll\"";
                psi = new ProcessStartInfo(filename, arguments);
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                result = (result + Process.Start(psi).StandardOutput.ReadToEnd());
                result += "\r\n------------------------------------------------------------------------------\r\n";

                arguments = " -u Srvtools";
                psi = new ProcessStartInfo(filename, arguments);
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                result = (result + Process.Start(psi).StandardOutput.ReadToEnd());
                result += "\r\n------------------------------------------------------------------------------\r\n";

                arguments = " -i \"" + textBox4.Text.Trim() + "\\Init\\" + "Srvtools.dll\"";
                psi = new ProcessStartInfo(filename, arguments);
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                result = (result + Process.Start(psi).StandardOutput.ReadToEnd());

                //---------------------------------------------------------------------

                arguments = " -u envdte80";
                psi = new ProcessStartInfo(filename, arguments);
                psi.UseShellExecute = false;
                Process.Start(psi);

                arguments = " -i \"" + textBox4.Text.Trim() + "\\Init\\" + "envdte80.dll\"";
                psi = new ProcessStartInfo(filename, arguments);
                Process.Start(psi);

                //---------------------------------------------------------------------

                arguments = " -u envdte";
                psi = new ProcessStartInfo(filename, arguments);
                psi.UseShellExecute = false;
                Process.Start(psi);

                arguments = " -i \"" + textBox4.Text.Trim() + "\\Init\\" + "envdte.dll\"";
                psi = new ProcessStartInfo(filename, arguments);
                Process.Start(psi);

                //---------------------------------------------------------------------

                MessageBox.Show(result, "InitEEP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dirDlg.SelectedPath = textBox5.Text;
            if (dirDlg.ShowDialog() == DialogResult.OK)
            {
                textBox5.Text = dirDlg.SelectedPath;
            }
        }
    }
}