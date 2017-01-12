using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Web.UI.HtmlControls;

namespace AjaxTools
{
    public class AjaxTextBoxWatermark : TextBox, INamingContainer
    {
        public AjaxTextBoxWatermark()
        {
            if (this.WatermarkCssClass == null || this.WatermarkCssClass == "")
                this.WatermarkCssClass = "ajax_watermarked";
        }

        private TextBoxWatermarkExtender _textBoxWatermarkExtender = new TextBoxWatermarkExtender();

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
                this._textBoxWatermarkExtender.ID = "_textBoxWatermarkExtender";
                this._textBoxWatermarkExtender.TargetControlID = this.ClientID;
            }
        }

        [Category("Infolight")]
        public string BehaviorID
        {
            get
            {
                EnsureChildControls();
                return _textBoxWatermarkExtender.BehaviorID;
            }
            set
            {
                EnsureChildControls();
                _textBoxWatermarkExtender.BehaviorID = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("ajax_watermarked")]
        public string WatermarkCssClass
        {
            get
            {
                EnsureChildControls();
                return _textBoxWatermarkExtender.WatermarkCssClass;
            }
            set
            {
                EnsureChildControls();
                _textBoxWatermarkExtender.WatermarkCssClass = value;
            }
        }

        [Category("Infolight")]
        public string WatermarkText
        {
            get
            {
                EnsureChildControls();
                return _textBoxWatermarkExtender.WatermarkText;
            }
            set
            {
                EnsureChildControls();
                _textBoxWatermarkExtender.WatermarkText = value;
            }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            if (!this.DesignMode)
            {
                _textBoxWatermarkExtender = new TextBoxWatermarkExtender();
                _textBoxWatermarkExtender.ID = "_textBoxWatermarkExtender";
                this.Controls.Add(_textBoxWatermarkExtender);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (!this.DesignMode)
                _textBoxWatermarkExtender.RenderControl(writer);
        }
    }
}
