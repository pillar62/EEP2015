using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Drawing.Design;
using System.ComponentModel.Design;

namespace AjaxTools
{
    public class AjaxNumericUpDown : TextBox, INamingContainer
    {
        public AjaxNumericUpDown()
        {
            if (this.NumericUpDownWidth == 0)
                this.NumericUpDownWidth = 120;
        }

        private NumericUpDownExtender _numericUpDownExtender = new NumericUpDownExtender();

        #region properties
        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
                EnsureChildControls();
                this._numericUpDownExtender.ID = "_numericUpDownExtender";
                this._numericUpDownExtender.TargetControlID = this.ClientID;
            }
        }

        [Category("Infolight")]
        public string BehaviorID
        {
            get
            {
                EnsureChildControls();
                return _numericUpDownExtender.BehaviorID;
            }
            set
            {
                EnsureChildControls();
                _numericUpDownExtender.BehaviorID = value;
            }
        }

        [Category("Infolight")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string RefValues
        {
            get
            {
                EnsureChildControls();
                return _numericUpDownExtender.RefValues;
            }
            set
            {
                EnsureChildControls();
                _numericUpDownExtender.RefValues = value;
            }
        }

        [Category("Infolight")]
        public string ServiceDownMethod
        {
            get
            {
                EnsureChildControls();
                return _numericUpDownExtender.ServiceDownMethod;
            }
            set
            {
                EnsureChildControls();
                _numericUpDownExtender.ServiceDownMethod = value;
            }
        }

        [Category("Infolight")]
        [TypeConverter(typeof(ServicePathConverter))]
        [UrlProperty()]
        [Editor(typeof(UrlEditor), typeof(UITypeEditor))]
        public string ServiceDownPath
        {
            get
            {
                EnsureChildControls();
                return _numericUpDownExtender.ServiceDownPath;
            }
            set
            {
                EnsureChildControls();
                _numericUpDownExtender.ServiceDownPath = value;
            }
        }

        [Category("Infolight")]
        public string ServiceUpMethod
        {
            get
            {
                EnsureChildControls();
                return _numericUpDownExtender.ServiceUpMethod;
            }
            set
            {
                EnsureChildControls();
                _numericUpDownExtender.ServiceUpMethod = value;
            }
        }

        [Category("Infolight")]
        [TypeConverter(typeof(ServicePathConverter))]
        [UrlProperty()]
        [Editor(typeof(UrlEditor), typeof(UITypeEditor))]
        public string ServiceUpPath
        {
            get
            {
                EnsureChildControls();
                return _numericUpDownExtender.ServiceUpPath;
            }
            set
            {
                EnsureChildControls();
                _numericUpDownExtender.ServiceUpPath = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(1.0)]
        public double Step
        {
            get
            {
                EnsureChildControls();
                return _numericUpDownExtender.Step;
            }
            set
            {
                EnsureChildControls();
                _numericUpDownExtender.Step = value;
            }
        }

        [Category("Infolight")]
        public string Tag
        {
            get
            {
                EnsureChildControls();
                return _numericUpDownExtender.Tag;
            }
            set
            {
                EnsureChildControls();
                _numericUpDownExtender.Tag = value;
            }
        }

        [Category("Infolight")]
        [IDReferenceProperty(typeof(Control))]
        public string TargetButtonDownID
        {
            get
            {
                EnsureChildControls();
                return _numericUpDownExtender.TargetButtonDownID;
            }
            set
            {
                EnsureChildControls();
                _numericUpDownExtender.TargetButtonDownID = value;
            }
        }

        [Category("Infolight")]
        [IDReferenceProperty(typeof(Control))]
        public string TargetButtonUpID
        {
            get
            {
                EnsureChildControls();
                return _numericUpDownExtender.TargetButtonUpID;
            }
            set
            {
                EnsureChildControls();
                _numericUpDownExtender.TargetButtonUpID = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(120)]
        public int NumericUpDownWidth
        {
            get
            {
                EnsureChildControls();
                return _numericUpDownExtender.Width;
            }
            set
            {
                EnsureChildControls();
                _numericUpDownExtender.Width = value;
            }
        }
        #endregion

        protected override void CreateChildControls()
        {
            Controls.Clear();
            if (!this.DesignMode)
            {
                _numericUpDownExtender = new NumericUpDownExtender();
                _numericUpDownExtender.ID = "_numericUpDownExtender";
                this.Controls.Add(_numericUpDownExtender);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (!this.DesignMode)
                _numericUpDownExtender.RenderControl(writer);
        }
    }
}
