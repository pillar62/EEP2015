using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;
using System.Drawing.Design;
using System.Drawing;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Resources;
using System.Globalization;

namespace Srvtools
{
    #region INFOLIGHT_DATAMODULE_DESIGNER

    // Nearly all methods' bodys are placed in try-catch blocks
    // as a small error may lead to Visual Studio's forcely shutting up, 
    // without asking for saving the code and documents, 
    // that will be a nightmare for any developers.
    //
    internal class InfoDMDesigner : ComponentDocumentDesigner
    {
        private string ThisModuleName = "Srvtools";
        private string ThisComponentName = "InfoDMDesigner";

        // Conponent Design Time View
        //
        internal Control view;

        // Design Time Services
        internal IComponentChangeService componentChangeService;
        internal IDesignerHost designerHost;

        // To store relation information between components
        //
        internal ArrayList relations;

        [DllImport("KERNEL32.DLL", EntryPoint = "GetThreadLocale", SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern uint GetThreadLocale();

        internal SYS_LANGUAGE language = SYS_LANGUAGE.ENG;

        public InfoDMDesigner()
        {
            // Get language type
            language = GetClientLanguage();

            // Add By Chenjian 2005-12-26
            // For Save/Load Component Location

            // Save Component Location
            EventHandler saveComponentLocationEventHandler = new EventHandler(
                delegate(object sender, EventArgs e)
                {
                    SaveComponentLocation();
                }
            );

            DesignerVerb saveComponentLocation = new DesignerVerb("Save Location", saveComponentLocationEventHandler);
            this.Verbs.Insert(0, saveComponentLocation);

            // Load Component Location
            EventHandler loadComponentLocationEventHandler = new EventHandler(
                delegate(object sender, EventArgs e)
                {
                    LoadComponentLocation();
                }
            );
            DesignerVerb loadComponentLocation = new DesignerVerb("Load Location", loadComponentLocationEventHandler);
            this.Verbs.Insert(0, loadComponentLocation);

            // End Add 2005-12-26

            // Use a thread to load the design time view and apply Service-Event
            // because that can not be done in constructor
            //
            Thread t = new Thread(new ThreadStart(ViewOperation));
            t.Priority = ThreadPriority.Highest;
            t.Start();
        }

        private void LoadComponentLocation()
        {
            try
            {
                CultureInfo culture = new CultureInfo("vi-VN");
                IResourceService resourceService =
                    designerHost.GetService(typeof(IResourceService)) as IResourceService;
                IResourceReader resourceReader = resourceService.GetResourceReader(culture);
                IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    try
                    {
                        Control ctrl = this.GetViewedControl(enumerator.Key.ToString());
                        if (ctrl != null)
                        {
                            ctrl.Location = (Point)enumerator.Value;
                        }
                    }
                    catch
                    {
                    }
                }

                resourceReader.Close();
            }
            catch
            {
            }
        }

        private void SaveComponentLocation()
        {
            try
            {
                CultureInfo culture = new CultureInfo("vi-VN");
                IResourceService resourceService =
                    designerHost.GetService(typeof(IResourceService)) as IResourceService;
                IResourceWriter resourceWriter = resourceService.GetResourceWriter(culture);
                foreach (Control ctrl in view.Controls)
                {
                    resourceWriter.AddResource(ctrl.Text, ctrl.Location);
                }
                resourceWriter.Close();
            }
            catch
            {
            }
        }

        private SYS_LANGUAGE GetClientLanguage()
        {
            try
            {
                uint dwlang = GetThreadLocale();
                ushort wlang = (ushort)dwlang;
                ushort wprilangid = (ushort)(wlang & 0x3FF);
                ushort wsublangid = (ushort)(wlang >> 10);

                if (0x09 == wprilangid)
                    return SYS_LANGUAGE.ENG;
                else if (0x04 == wprilangid)
                {
                    if (0x01 == wsublangid)
                        return SYS_LANGUAGE.TRA;
                    else if (0x02 == wsublangid)
                        return SYS_LANGUAGE.SIM;
                    else if (0x03 == wsublangid)
                        return SYS_LANGUAGE.HKG;
                    else
                        return SYS_LANGUAGE.TRA;
                }
                else if (0x11 == wprilangid)
                    return SYS_LANGUAGE.JPN;
                else
                    return SYS_LANGUAGE.ENG;
            }
            catch (Exception err)
            {
                MessageBox.Show("InfoDMDesigner Error: " + err.Message);
            }
            return SYS_LANGUAGE.ENG;
        }

        /*public void designerHost_LoadComplete(object sender, EventArgs e)
        {
            LoadComponentLocation();
        }*/
        
        private void ViewOperation()
        {
            try
            {
                // designerHost
                //
                designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
                while (designerHost == null)
                {
                    Thread.Sleep(100);
                    designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
                }

                // Wait for loading document
                //
                //designerHost.LoadComplete += new EventHandler(designerHost_LoadComplete);
                while (designerHost.Loading)
                {
                    Thread.Sleep(100);
                }

                // view
                //
                view = this.Control;
                while (view == null)
                {
                    Thread.Sleep(100);
                    view = this.Control;
                }
                view.Paint += new PaintEventHandler(view_Paint);
                view.Click += new EventHandler(view_Click);
                this.TrayAutoArrange = false;

                // Add a Rectangle border to every control in design view
                //
                foreach (Control ctrl in view.Controls)
                {
                    ctrl.Paint += new PaintEventHandler(ctrl_Paint);
                }

                // componnetChangeService
                //
                componentChangeService = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                while (componentChangeService == null)
                {
                    Thread.Sleep(100);
                    componentChangeService = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                }

                componentChangeService.ComponentAdded += new ComponentEventHandler(componentChangeService_ComponentAdded);
                componentChangeService.ComponentChanged += new ComponentChangedEventHandler(componentChangeService_ComponentChanged);
                componentChangeService.ComponentRemoved += new ComponentEventHandler(componentChangeService_ComponentRemoved);
                componentChangeService.ComponentRename += new ComponentRenameEventHandler(componentChangeService_ComponentRename);

                // Load Relations
                //
                relations = new ArrayList();

                Type componentType = typeof(System.ComponentModel.Component);
            // Reload relations if loading fails
            StartLoadRelation:
                try
                {
                    foreach (IComponent comp in designerHost.Container.Components)
                    {
                        string src = comp.ToString();
                        if (src.IndexOf(' ') != -1)
                        {
                            src = src.Substring(0, src.IndexOf(' '));
                        }
                        Control source = GetViewedControl(src);
                        if (source == null)
                        {
                            continue;
                        }

                        Type t = comp.GetType();
                        PropertyInfo[] properties = t.GetProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            // A property can be System.Object-Type that have relation with other component
                            // Such as ComboBox.DataSource
                            // 
                            //if (property.PropertyType.GetInterface("System.ComponentModel.IComponent") == null)
                            //{
                            //    continue;
                            //}

                            // To filter properties such as TextBox.Text
                            if (property.PropertyType.FullName == "System.String")
                            {
                                continue;
                            }

                            // To filter collections properties
                            if (property.PropertyType.GetInterface("System.Collections.ICollection") != null)
                            {
                                continue;
                            }

                            if (!property.CanWrite)
                            {
                                continue;
                            }

                            MethodInfo mi = property.GetGetMethod();
                            //// To filter write-only properties
                            //if (mi == null)
                            //{
                            //    continue;
                            //}

                            // To filter indexer perperties
                            if (mi.GetParameters().GetLength(0) != 0)
                            {
                                continue;
                            }

                            // To filter properties with no value
                            if (property.GetValue(comp, null) == null)
                            {
                                continue;
                            }

                            // To filter Connection Properties
                            if (property.GetValue(comp, null) is System.Data.IDbConnection)
                            {
                                continue;
                            }

                            string dst = property.GetValue(comp, null).ToString();
                            if (dst.IndexOf(' ') != -1)
                            {
                                dst = dst.Substring(0, dst.IndexOf(' '));
                            }

                            if (src == dst)
                            {
                                continue;
                            }

                            Control destination = GetViewedControl(dst);
                            if (source != null && destination != null)
                            {
                                relations.Add(new Relation(this, src, dst));
                            }
                        }
                        // A special case for InfoTransaction
                        //if (comp is InfoTransaction)
                        //{
                        //    InfoTransaction infoTrans = comp as InfoTransaction;
                        //    foreach (Transaction trans in infoTrans.Transactions)
                        //    {
                        //        if (trans.AutoNumber != null && trans.AutoNumber.Site != null)
                        //        {
                        //            string dst = trans.AutoNumber.Site.Name;
                        //            if (dst.IndexOf(' ') != -1)
                        //            {
                        //                dst = dst.Substring(0, dst.IndexOf(' '));
                        //            }
                        //            Control destination = GetViewedControl(dst);
                        //            if (destination != null)
                        //            {
                        //                relations.Add(new Relation(this, src, dst));
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
                catch
                {
                    relations.Clear();
                    goto StartLoadRelation;
                }

                view.Refresh();
            }
            catch (Exception err)
            {
                language = CliUtils.fClientLang;
                string sMess = SysMsg.GetSystemMessage(language, ThisModuleName, ThisComponentName, "msg_Exception");
                MessageBox.Show(string.Format(sMess, ThisComponentName, "ViewOperation", err.Message));
            }
        }

        // Get a design time view control of a component by name
        //
        public Control GetViewedControl(string componentName)
        {
            try
            {
                foreach (Control ctrl in view.Controls)
                {
                    if (ctrl.Text == componentName)
                    {
                        return ctrl;
                    }
                }
            }
            catch (Exception err)
            {
                //language = CliUtils.fClientLang;
                //string sMess = SysMsg.GetSystemMessage(language, ThisModuleName, ThisComponentName, "msg_Exception");
                //MessageBox.Show(string.Format(sMess, ThisComponentName, "GetViewedControl", err.Message));
            }
            return null;
        }

        // Occurs when a component is renamed
        //
        void componentChangeService_ComponentRename(object sender, ComponentRenameEventArgs e)
        {
            try
            {
                // search and change the modified name in relations
                //
                for (int i = relations.Count - 1; i >= 0; --i)
                {
                    Relation relation = relations[i] as Relation;

                    if (relation.Source == e.OldName)
                    {
                        relation.Source = e.NewName;
                    }
                    if (relation.Destination == e.OldName)
                    {
                        relation.Destination = e.NewName;
                    }
                }
            }
            catch (Exception err)
            {
                language = CliUtils.fClientLang;
                string sMess = SysMsg.GetSystemMessage(language, ThisModuleName, ThisComponentName, "msg_Exception");
                MessageBox.Show(string.Format(sMess, ThisComponentName, "componentChangeService_ComponentRename", err.Message));
            }
        }

        // Occurs when a component is removed
        //
        void componentChangeService_ComponentRemoved(object sender, ComponentEventArgs e)
        {
            try
            {
                // remove the removed component from relations
                //
                if (e.Component.Site == null)
                {
                    return;
                }
                string obj = e.Component.Site.Name;
                if (obj.IndexOf(' ') != -1)
                {
                    obj = obj.Substring(0, obj.IndexOf(' '));
                }

                for (int i = relations.Count - 1; i >= 0; --i)
                {
                    Relation relation = relations[i] as Relation;

                    if (relation.Source == obj
                        || relation.Destination == obj)
                    {
                        relations.RemoveAt(i);
                    }
                }

                view.Refresh();
            }
            catch (Exception err)
            {
                language = CliUtils.fClientLang;
                string sMess = SysMsg.GetSystemMessage(language, ThisModuleName, ThisComponentName, "msg_Exception");
                MessageBox.Show(string.Format(sMess, ThisComponentName, "componentChangeService_ComponentRemoved", err.Message));
            }
        }

        // Occurs when change happens the a componnet
        //
        void componentChangeService_ComponentChanged(object sender, ComponentChangedEventArgs e)
        {
            try
            {
                string src = e.Component.ToString();
                if (src.IndexOf(' ') != -1)
                {
                    src = src.Substring(0, src.IndexOf(' '));
                }

                if (GetViewedControl(src) == null)
                {
                    return;
                }

                if (e.OldValue != null)
                {
                    string dst = e.OldValue.ToString();
                    if (dst.IndexOf(' ') != -1)
                    {
                        dst = dst.Substring(0, dst.IndexOf(' '));
                    }

                    for (int i = relations.Count - 1; i >= 0; --i)
                    {
                        Relation relation = relations[i] as Relation;
                        if (relation.Source == src
                            && relation.Destination == dst)
                        {
                            relations.RemoveAt(i);
                        }
                    }
                }

                if (e.NewValue != null)
                {
                    // To filter Connection Properties
                    if (e.NewValue is System.Data.Common.DbConnection)
                    {
                        return;
                    }

                    string dst = e.NewValue.ToString();
                    if (dst.IndexOf(' ') != -1)
                    {
                        dst = dst.Substring(0, dst.IndexOf(' '));
                    }

                    if (src == dst)
                    {
                        return;
                    }

                    Control source = GetViewedControl(src);
                    Control destination = GetViewedControl(dst);

                    if (source != null && destination != null)
                    {
                        relations.Add(new Relation(this, src, dst));
                    }
                }

                // A special case for InfoTransaction
                //if (e.Component is InfoTransaction && e.Member != null && e.Member.Name == "Transactions")
                //{
                //    InfoTransaction infoTrans = e.Component as InfoTransaction;
                //    // Clear the old AutoNumber - InfoTransaction Relations
                //    List<Transaction> oldTransactions = e.OldValue as List<Transaction>;
                //    if (infoTrans != null && oldTransactions != null)
                //    {
                //        foreach (Transaction trans in oldTransactions)
                //        {
                //            if (trans.AutoNumber != null && trans.AutoNumber.Site != null)
                //            {
                //                string dst = trans.AutoNumber.Site.Name;
                //                if (dst.IndexOf(' ') != -1)
                //                {
                //                    dst = dst.Substring(0, dst.IndexOf(' '));
                //                }
                //                for (int i = relations.Count - 1; i >= 0; --i)
                //                {
                //                    Relation relation = relations[i] as Relation;

                //                    if (relation.Source == infoTrans.Name || relation.Destination == dst)
                //                    {
                //                        relations.RemoveAt(i);
                //                    }
                //                }
                //            }
                //        }

                //        // Add the new AutoNumber - InfoTransactions Relations
                //        foreach (Transaction trans in infoTrans.Transactions)
                //        {
                //            if (trans.AutoNumber != null
                //                && trans.AutoNumber.Site != null)
                //            {
                //                string dst = trans.AutoNumber.Site.Name;
                //                if (dst.IndexOf(' ') != -1)
                //                {
                //                    dst = dst.Substring(0, dst.IndexOf(' '));
                //                }
                //                Control destination = GetViewedControl(dst);
                //                if (destination != null)
                //                {
                //                    relations.Add(new Relation(this, src, dst));
                //                }
                //            }
                //        }
                //    }
                //}
                view.Refresh();
            }
            catch (Exception err)
            {
                language = CliUtils.fClientLang;
                string sMess = SysMsg.GetSystemMessage(language, ThisModuleName, ThisComponentName, "msg_Exception");
                MessageBox.Show(string.Format(sMess, ThisComponentName, "componentChangeService_ComponentChanged", err.Message));
            }
        }

        // Occurs when a component is added
        //
        void componentChangeService_ComponentAdded(object sender, ComponentEventArgs e)
        {
            try
            {
                if (e.Component.Site != null)
                {
                    Control ctrl = this.GetViewedControl(e.Component.Site.Name);
                    if (ctrl != null)
                    {
                        ctrl.Paint += new PaintEventHandler(ctrl_Paint);
                    }

                    if (e.Component is InfoCommand)
                    {
                        foreach (Component comp in designerHost.Container.Components)
                        {
                            if (comp is InfoConnection)
                            {
                                ((InfoCommand)(e.Component)).InfoConnection = (InfoConnection)comp;
                                break;
                            }
                        }
                    }

                    // Add By Chenjian 2006-01-11
                    //string src = e.Component.ToString();
                    //if (src.IndexOf(' ') != -1)
                    //{
                    //    src = src.Substring(0, src.IndexOf(' '));
                    //}
                    //Control source = GetViewedControl(src);
                    //if (source != null)
                    //{

                    //    Type t = e.Component.GetType();
                    //    PropertyInfo[] properties = t.GetProperties();
                    //    foreach (PropertyInfo property in properties)
                    //    {
                    //        // A property can be System.Object-Type that have relation with other component
                    //        // Such as ComboBox.DataSource
                    //        // 
                    //        //if (property.PropertyType.GetInterface("System.ComponentModel.IComponent") == null)
                    //        //{
                    //        //    continue;
                    //        //}

                    //        // To filter properties such as TextBox.Text
                    //        if (property.PropertyType.FullName == "System.String")
                    //        {
                    //            continue;
                    //        }

                    //        // To filter collections properties
                    //        if (property.PropertyType.GetInterface("System.Collections.ICollection") != null)
                    //        {
                    //            continue;
                    //        }

                    //        MethodInfo mi = property.GetGetMethod();
                    //        // To filter write-only properties
                    //        if (mi == null)
                    //        {
                    //            continue;
                    //        }

                    //        // To filter indexer perperties
                    //        if (mi.GetParameters().GetLength(0) != 0)
                    //        {
                    //            continue;
                    //        }

                    //        // To filter properties with no value
                    //        if (property.GetValue(e.Component, null) == null)
                    //        {
                    //            continue;
                    //        }

                    //        // To filter Connection Properties
                    //        if (property.GetValue(e.Component, null) is System.Data.Common.DbConnection)
                    //        {
                    //            continue;
                    //        }

                    //        string dst = property.GetValue(e.Component, null).ToString();
                    //        if (dst.IndexOf(' ') != -1)
                    //        {
                    //            dst = dst.Substring(0, dst.IndexOf(' '));
                    //        }

                    //        if (src == dst)
                    //        {
                    //            continue;
                    //        }

                    //        Control destination = GetViewedControl(dst);
                    //        if (source != null && destination != null)
                    //        {
                    //            relations.Add(new Relation(this, src, dst));
                    //        }
                    //    }
                    //}
                    // End Add

                }
            }
            catch (Exception err)
            {
                language = CliUtils.fClientLang;
                string sMess = SysMsg.GetSystemMessage(language, ThisModuleName, ThisComponentName, "msg_Exception");
                MessageBox.Show(string.Format(sMess, ThisComponentName, "componentChangeService_ComponentAdded", err.Message));
            }
        }

        void ctrl_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                // Draw a rectangle border on the Control 
                //
                Control ctrl = sender as Control;
                Pen pen = new Pen(Brushes.DarkGray, 1.0f);
                e.Graphics.DrawRectangle(pen, 0, 0, ctrl.Width - 1, ctrl.Height - 1);
            }
            catch (Exception err)
            {
                language = CliUtils.fClientLang;
                string sMess = SysMsg.GetSystemMessage(language, ThisModuleName, ThisComponentName, "msg_Exception");
                MessageBox.Show(string.Format(sMess, ThisComponentName, "ctrl_Paint", err.Message));
            }
        }

        // Paint the line between components
        void view_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Pen pen = new Pen(Brushes.Chocolate, 1.0f);
                // Draw lines between related components
                //
                foreach (Relation relation in relations)
                {
                    Point srcPoint, destPoint;
                    GetCrossPoint(relation.SourceControl, relation.DestinationControl, out srcPoint, out destPoint);

                    pen.Brush = Brushes.Chocolate;
                    if (designerHost.Container.Components[relation.destination] is System.Data.DataSet
                        || designerHost.Container.Components[relation.destination] is InfoDataSet)
                    {
                        pen.Brush = Brushes.Blue;
                    }

                    e.Graphics.DrawLine(pen, srcPoint, destPoint);

                    float x = destPoint.X - srcPoint.X;
                    float y = destPoint.Y - srcPoint.Y;
                    float angle = 0;
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    else if (x > 0 && y == 0)
                    {
                        angle = 180;
                    }
                    else if (x > 0 && y > 0)
                    {
                        angle = (float)(180 + Math.Atan(y / x) / Math.PI * 180);
                    }
                    else if (x == 0 && y > 0)
                    {
                        angle = 270;
                    }
                    else if (x < 0 && y > 0)
                    {
                        angle = (float)(360 - Math.Atan(y / Math.Abs(x)) / Math.PI * 180);
                    }
                    else if (x < 0 && y == 0)
                    {
                        angle = 360;
                    }
                    else if (x < 0 && y < 0)
                    {
                        angle = (float)(Math.Atan(Math.Abs(y) / Math.Abs(x)) / Math.PI * 180);
                    }
                    else if (x == 0 && y < 0)
                    {
                        angle = 90;
                    }
                    else if (x > 0 && y < 0)
                    {
                        angle = (float)(180 - Math.Atan(Math.Abs(y) / x) / Math.PI * 180);
                    }

                    e.Graphics.FillPie(pen.Brush, destPoint.X - 13, destPoint.Y - 13, 26, 26, angle - 20, 40);
                }
            }
            catch (Exception err)
            {
                language = CliUtils.fClientLang;
                string sMess = SysMsg.GetSystemMessage(language, ThisModuleName, ThisComponentName, "msg_Exception");
                MessageBox.Show(string.Format(sMess, ThisComponentName, "view_Paint", err.Message));
            }
        }

        void GetCrossPoint(Control source, Control dest, out Point srcPoint, out Point destPoint)
        {
            srcPoint = new Point((2 * source.Location.X + source.Width) / 2, (2 * source.Location.Y + source.Height) / 2);
            destPoint = new Point((2 * dest.Location.X + dest.Width) / 2, (2 * dest.Location.Y + dest.Height) / 2);
            Point srcCrossPoint = new Point();
            if (IsIntersect(source.Location, new Point(source.Location.X + source.Width, source.Location.Y), srcPoint, destPoint))
            {
                srcCrossPoint.X = (source.Location.X * 2 + source.Width) / 2;
                srcCrossPoint.Y = source.Location.Y;
            }
            else if (IsIntersect(new Point(source.Location.X + source.Width, source.Location.Y),
                new Point(source.Location.X + source.Width, source.Location.Y + source.Height),
                srcPoint, destPoint))
            {
                srcCrossPoint.X = source.Location.X + source.Width;
                srcCrossPoint.Y = (source.Location.Y * 2 + source.Height) / 2;
            }
            else if (IsIntersect(new Point(source.Location.X + source.Width, source.Location.Y + source.Height),
                new Point(source.Location.X, source.Location.Y + source.Height),
                srcPoint, destPoint))
            {
                srcCrossPoint.X = (source.Location.X * 2 + source.Width) / 2;
                srcCrossPoint.Y = source.Location.Y + source.Height;
            }
            else
            {
                srcCrossPoint.X = source.Location.X;
                srcCrossPoint.Y = (source.Location.Y * 2 + source.Height) / 2;
            }

            Point dstCrossPoint = new Point();
            if (IsIntersect(dest.Location, new Point(dest.Location.X + dest.Width, dest.Location.Y), srcPoint, destPoint))
            {
                dstCrossPoint.X = (dest.Location.X * 2 + dest.Width) / 2;
                dstCrossPoint.Y = dest.Location.Y;
            }
            else if (IsIntersect(new Point(dest.Location.X + dest.Width, dest.Location.Y),
                new Point(dest.Location.X + dest.Width, dest.Location.Y + dest.Height),
                srcPoint, destPoint))
            {
                dstCrossPoint.X = dest.Location.X + dest.Width;
                dstCrossPoint.Y = (dest.Location.Y * 2 + dest.Height) / 2;
            }
            else if (IsIntersect(new Point(dest.Location.X + dest.Width, dest.Location.Y + dest.Height),
                new Point(dest.Location.X, dest.Location.Y + dest.Height),
                srcPoint, destPoint))
            {
                dstCrossPoint.X = (dest.Location.X * 2 + dest.Width) / 2;
                dstCrossPoint.Y = dest.Location.Y + dest.Height;
            }
            else
            {
                dstCrossPoint.X = dest.Location.X;
                dstCrossPoint.Y = (dest.Location.Y * 2 + dest.Height) / 2;
            }
            srcPoint = srcCrossPoint;
            destPoint = dstCrossPoint;
        }

        // Cross Product
        int Cross(Point a, Point b, Point o)
        {
            return (a.X - o.X) * (b.Y - o.Y) - (b.X - o.X) * (a.Y - o.Y);
        }

        // Determine wether two segments intersect
        private bool IsIntersect(Point a, Point b, Point x, Point y)
        {
            bool result =
                (Cross(a, y, x) * Cross(y, b, x) >= 0)
                && (Cross(x, b, a) * Cross(b, y, a) >= 0)
                && (Math.Max(x.X, y.X) >= Math.Min(a.X, b.X))
                && (Math.Max(a.X, b.X) >= Math.Min(x.X, y.X))
                && (Math.Max(x.Y, y.Y) >= Math.Min(a.Y, b.Y))
                && (Math.Max(a.Y, b.Y) >= Math.Min(x.Y, y.Y));

            return result;
        }

        void view_Click(object sender, EventArgs e)
        {
            //foreach (Control ctrl in view.Controls)
            //{
            //    MessageBox.Show(ctrl.Text);
            //}
        }

        // Used to stroe Component Relation
        internal class Relation
        {
            public Relation(InfoDMDesigner designer, string src, string dst)
            {
                Designer = designer;
                Source = src;
                Destination = dst;
            }
            // Both of Source and Destination are componnet name
            //
            public string source;
            public string destination;
            InfoDMDesigner Designer;

            public string Source
            {
                get
                {
                    return source;
                }
                set
                {
                    if (this.sourceControl == null)
                    {
                        this.sourceControl = Designer.GetViewedControl(value);
                        if (this.sourceControl == null)
                        {
                            string sMess = SysMsg.GetSystemMessage(Designer.language, Designer.ThisModuleName, Designer.ThisComponentName, "msg_Exception");
                            MessageBox.Show(string.Format(sMess, Designer.ThisComponentName + "+ Relation", "get_Source", "Control not find"));
                        }
                    }
                    source = value;
                }
            }

            public string Destination
            {
                get
                {
                    return destination;
                }
                set
                {
                    if (this.destinationControl == null)
                    {
                        this.destinationControl = Designer.GetViewedControl(value);
                        if (this.destinationControl == null)
                        {
                            string sMess = SysMsg.GetSystemMessage(Designer.language, Designer.ThisModuleName, Designer.ThisComponentName, "msg_Exception");
                            MessageBox.Show(string.Format(sMess, Designer.ThisComponentName + "+ Relation", "get_Destination", "Control not find"));
                        }
                    }
                    destination = value;
                }
            }

            private Control sourceControl;
            private Control destinationControl;

            public Control SourceControl
            {
                get
                {
                    return sourceControl;
                }
            }

            public Control DestinationControl
            {
                get
                {
                    return destinationControl;
                }
            }
        }
    }
    #endregion INFOLIGHT_DATAMODULE_DESIGNER
}