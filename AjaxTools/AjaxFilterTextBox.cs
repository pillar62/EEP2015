using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.ComponentModel;

namespace AjaxTools
{
    public class AjaxFilterTextBox : TextBox, INamingContainer
    {
        private FilteredTextBoxExtender _filterTextBoxExtender = new FilteredTextBoxExtender();

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
                this._filterTextBoxExtender.ID = "_filterTextBoxExtender";
                this._filterTextBoxExtender.TargetControlID = this.ClientID;
            }
        }

        [Category("Infolight")]
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        [Category("Infolight")]
        public string BehaviorID
        {
            get
            {
                EnsureChildControls();
                return _filterTextBoxExtender.BehaviorID;
            }
            set
            {
                EnsureChildControls();
                _filterTextBoxExtender.BehaviorID = value;
            }
        }

        [Category("Infolight")]
        public FilterTypes FilterType
        {
            get
            {
                EnsureChildControls();
                return _filterTextBoxExtender.FilterType;
            }
            set
            {
                EnsureChildControls();
                _filterTextBoxExtender.FilterType = value;
            }
        }

        [Category("Infolight")]
        public string ValidChars
        {
            get
            {
                EnsureChildControls();
                return _filterTextBoxExtender.ValidChars;
            }
            set
            {
                EnsureChildControls();
                _filterTextBoxExtender.ValidChars = value;
            }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            if (!this.DesignMode)
            {
                _filterTextBoxExtender = new FilteredTextBoxExtender();
                _filterTextBoxExtender.ID = "_filterTextBoxExtender";
                this.Controls.Add(_filterTextBoxExtender);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (!this.DesignMode)
                _filterTextBoxExtender.RenderControl(writer);
        }
    }
}
