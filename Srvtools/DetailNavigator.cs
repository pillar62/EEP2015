using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;

namespace Srvtools
{
    public class DetailNavigator: BindingNavigator, ISupportInitialize
    {
        public DetailNavigator()
        {
            DisplayStyle = ToolStripItemDisplayStyle.Image;
            TextImageRelation = TextImageRelation.ImageAboveText;
        }

        private ToolStripItemDisplayStyle _DisplayStyle;
        [Category("Infolight"),
        Description("The display style of items")]
        public ToolStripItemDisplayStyle DisplayStyle
        {
            get
            {
                return _DisplayStyle;
            }
            set
            {
                _DisplayStyle = value;
                foreach (ToolStripItem item in this.Items)
                {
                    item.DisplayStyle = _DisplayStyle;
                }
            }
        }

        private TextImageRelation _TextImageRelation;
        [Category("Infolight"),
        Description("The relation between text and image")]
        public TextImageRelation TextImageRelation
        {
            get 
            {
                return _TextImageRelation; 
            }
            set
            {
                _TextImageRelation = value;
                foreach (ToolStripItem item in this.Items)
                {
                    item.TextImageRelation = _TextImageRelation;
                }
            }
        }

        public InfoNavigator _ParentNavigator;
        [Category("Infolight"),
        Description("The parent navigator to bind to")]
        public InfoNavigator ParentNavigator
        {
            get { return _ParentNavigator; }
            set { _ParentNavigator = value; }
        }

        [Category("Infolight"),
        Description("The button used to add")]
        public new ToolStripItem AddNewItem
        {
            get
            {
                return base.AddNewItem;
            }
            set
            {
                base.AddNewItem = value;
            }
        }

        [Category("Infolight"),
        Description("The button used to delete")]
        public new ToolStripItem DeleteItem
        {
            get
            {
                return base.DeleteItem;
            }
            set
            {
                base.DeleteItem = value;
            }
        }

        private ToolStripItem _OKItem = null;
        [Category("Infolight"),
        Description("The button used to ok")]
        public ToolStripItem OKItem
        {
            get
            {
                return _OKItem;
            }
            set
            {
                _OKItem = value;
            }
        }

        private ToolStripItem _CancelItem = null;
        [Category("Infolight"),
        Description("The button used to cancel")]
        public ToolStripItem CancelItem
        {
            get
            {
                return _CancelItem;
            }
            set
            {
                _CancelItem = value;
            }
        }

        public override void AddStandardItems()
        {
            Assembly assembly = this.GetType().Assembly;
            Bitmap bmpadd = new Bitmap(typeof(BindingNavigator), "BindingNavigator.AddNew.bmp");
            Bitmap bmpdelete = new Bitmap(assembly.GetManifestResourceStream("Srvtools.InfonavigatorImage.Delete.png"));
            Bitmap bmpok = new Bitmap(assembly.GetManifestResourceStream("Srvtools.InfonavigatorImage.OK.png"));
            Bitmap bmpcancel = new Bitmap(assembly.GetManifestResourceStream("Srvtools.InfonavigatorImage.Cancel.png"));

            bmpadd.MakeTransparent(Color.Magenta);
            bmpdelete.MakeTransparent(Color.Magenta);
            bmpok.MakeTransparent(Color.Magenta);
            bmpcancel.MakeTransparent(Color.Magenta);

            AddNewItem = new ToolStripButton("add", bmpadd, null, "detailNavigatorAddItem");
            DeleteItem = new ToolStripButton("delete", bmpdelete, null, "detailNavigatorDeleteItem");
            ToolStripSeparator separator = new ToolStripSeparator();
            OKItem = new ToolStripButton("ok", bmpok,null, "detailNavigatorOKItem");
            CancelItem = new ToolStripButton("cancel", bmpcancel, null, "detailNavigatorCancelItem");

            AddNewItem.DisplayStyle = DisplayStyle;
            DeleteItem.DisplayStyle = DisplayStyle;
            OKItem.DisplayStyle = DisplayStyle;
            CancelItem.DisplayStyle = DisplayStyle;

            AddNewItem.TextImageRelation = TextImageRelation;
            DeleteItem.TextImageRelation = TextImageRelation;
            OKItem.TextImageRelation = TextImageRelation;
            CancelItem.TextImageRelation = TextImageRelation;

            this.Items.AddRange (new ToolStripItem[] { AddNewItem, DeleteItem, separator, OKItem, CancelItem });
        }

        #region ISupportInitialize Members

        void ISupportInitialize.BeginInit() { }

        void ISupportInitialize.EndInit()
        {
            if (AddNewItem != null)
            {
                AddNewItem.Click += new EventHandler(AddNewItem_Click);
            }
            if (OKItem != null)
            {
                OKItem.Click += new EventHandler(OKItem_Click);
            }
            if (CancelItem != null)
            {
                CancelItem.Click += new EventHandler(CancelItem_Click);
            }
            (this.BindingSource as InfoBindingSource).EditBeginning += new EventHandler(Detail_EditBeginning);
            if (this.ParentNavigator.BindingSource.AutoDisableControl)
            {
                DisableNavigatorItems();
                this.ParentNavigator.AfterItemClick += new AfterItemClickEventHandler(ParentNavigator_AfterItemClick);
            }
            else
            {
                SetNavigatorItems(false);
            }
        }

        #endregion


        void ParentNavigator_AfterItemClick(object sender, AfterItemClickEventArgs e)
        {
            if (e.ItemName == "Add" || e.ItemName == "Edit")
            {
                SetNavigatorItems(false);
            }
            else/* if (e.ItemName == "OK" || e.ItemName == "Cancel" || e.ItemName == "Apply" || e.ItemName == "Abort")*/
            {
                DisableNavigatorItems();
            }
        }

        void Detail_EditBeginning(object sender, EventArgs e)
        {
            SetNavigatorItems(true);
        }

        void AddNewItem_Click(object sender, EventArgs e)
        {
            SetNavigatorItems(true);
        }

        void CancelItem_Click(object sender, EventArgs e)
        {
            this.BindingSource.CancelEdit();
            SetNavigatorItems(false);
        }

        void OKItem_Click(object sender, EventArgs e)
        {
            this.Focus();
            if (this.BindingSource is InfoBindingSource)
            {
                (this.BindingSource as InfoBindingSource).bChk = false;
                (this.BindingSource as InfoBindingSource).InEndEdit(false);
                if (!(this.BindingSource as InfoBindingSource).CheckDeplicateSucess || !(this.BindingSource as InfoBindingSource).CheckSucess)
                {
                    SetNavigatorItems(true);
                    return;
                }
            }

            this.BindingSource.EndEdit();
            SetNavigatorItems(false);
        }

        public void SetNavigatorItems(bool isedit)
        {
            if (AddNewItem != null)
            {
                AddNewItem.Enabled = !isedit;
            }
            if (DeleteItem != null)
            {
                DeleteItem.Enabled = !isedit;
            }
            if (OKItem != null)
            {
                OKItem.Enabled = isedit;
            }
            if (CancelItem != null)
            {
                CancelItem.Enabled = isedit;
            }
        }

        public void DisableNavigatorItems()
        {
            if (AddNewItem != null)
            {
                AddNewItem.Enabled = false;
            }
            if (DeleteItem != null)
            {
                DeleteItem.Enabled = false;
            }
            if (OKItem != null)
            {
                OKItem.Enabled = false;
            }
            if (CancelItem != null)
            {
                CancelItem.Enabled = false;
            }
        }

        protected override void RefreshItemsCore()
        {
            base.RefreshItemsCore();
            if (this.ParentNavigator != null && this.ParentNavigator.BindingSource != null
                && this.ParentNavigator.BindingSource.AutoDisableControl 
                && this.ParentNavigator.CurrentState != "Inserting" && this.ParentNavigator.CurrentState != "Editing")
            {
                DisableNavigatorItems();
            }
            else
            {
                if (OKItem != null)
                {
                    OKItem.Enabled = false;
                }
                if (CancelItem != null)
                {
                    CancelItem.Enabled = false;
                }
            }
        }
    }
}
