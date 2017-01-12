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
    public class AjaxMaskedEdit : WebControl, INamingContainer
    {
        private MaskedEditExtender _maskedEditExtender = new MaskedEditExtender();
        private MaskedEditValidator _maskedEditValidator = new MaskedEditValidator();
        private TextBox _maskedEdit = new TextBox();

        #region properties
        [Bindable(true)]
        public string Text
        {
            get
            {
                return _maskedEdit.Text;
            }
            set
            {
                _maskedEdit.Text = value;
            }
        }

        //public override string ID
        //{
        //    get
        //    {
        //        return base.ID;
        //    }
        //    set
        //    {
        //        base.ID = value;
        //        EnsureChildControls();
        //        this._maskedEditExtender.ID = "_maskedEditExtender";
        //        this._maskedEditExtender.TargetControlID = this.ClientID;

        //        this._maskedEditValidator.ID = "_maskedEditValidator";
        //        this._maskedEditValidator.ControlToValidate = this.ClientID;
        //    }
        //}
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        #region Extender
        [Category("InfolightExtender")]
        public bool AcceptAMPM
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.AcceptAMPM;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.AcceptAMPM = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue(MaskedEditShowSymbol.None)]
        public MaskedEditShowSymbol AcceptNegative
        {
            get
            {
                EnsureChildControls(); 
                return _maskedEditExtender.AcceptNegative;
            }
            set
            {
                EnsureChildControls(); 
                _maskedEditExtender.AcceptNegative = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue(true)]
        public bool AutoComplete
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.AutoComplete;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.AutoComplete = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue("")]
        public string AutoCompleteValue
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.AutoCompleteValue;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.AutoCompleteValue = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue("")]
        public string BehaviorID
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.BehaviorID;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.BehaviorID = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue(1900)]
        public int Century
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.Century;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.Century = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue(true)]
        public bool ClearMaskOnLostFocus
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.ClearMaskOnLostFocus;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.ClearMaskOnLostFocus = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue(false)]
        public bool ClearTextOnInvalid
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.ClearTextOnInvalid;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.ClearTextOnInvalid = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue("")]
        public string CultureName
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.CultureName;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.CultureName = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue(MaskedEditShowSymbol.None)]
        public MaskedEditShowSymbol DisplayMoney
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.DisplayMoney;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.DisplayMoney = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue("")]
        public string Filtered
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.Filtered;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.Filtered = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue(MaskedEditInputDirection.LeftToRight)]
        public MaskedEditInputDirection InputDirection
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.InputDirection;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.InputDirection = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue("")]
        public string Mask
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.Mask;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.Mask = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue(MaskedEditType.None)]
        public MaskedEditType MaskType
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.MaskType;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.MaskType = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue(true)]
        public bool MessageValidatorTip
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.MessageValidatorTip;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.MessageValidatorTip = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue("MaskedEditBlurNegative")]
        public string OnBlurCssNegative
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.OnBlurCssNegative;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.OnBlurCssNegative = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue("MaskedEditFocus")]
        public string OnFocusCssClass
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.OnFocusCssClass;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.OnFocusCssClass = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue("MaskedEditFocusNegative")]
        public string OnFocusCssNegative
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.OnFocusCssNegative;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.OnFocusCssNegative = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue("MaskedEditError")]
        public string OnInvalidCssClass
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.OnInvalidCssClass;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.OnInvalidCssClass = value;
            }
        }

        [Category("InfolightExtender")]
        [DefaultValue("_")]
        public string PromptCharacter
        {
            get
            {
                EnsureChildControls();
                return _maskedEditExtender.PromptCharacter;
            }
            set
            {
                EnsureChildControls();
                _maskedEditExtender.PromptCharacter = value;
            }
        }
        #endregion

        #region Validator
        [Category("InfolightValidator")]
        [DefaultValue("")]
        public string ClientValidationFunction
        {
            get
            {
                EnsureChildControls();
                return _maskedEditValidator.ClientValidationFunction;
            }
            set
            {
                EnsureChildControls();
                _maskedEditValidator.ClientValidationFunction = value;
            }
        }

        [Category("InfolightValidator")]
        [DefaultValue("")]
        public string EmptyValueMessage
        {
            get
            {
                EnsureChildControls();
                return _maskedEditValidator.EmptyValueMessage;
            }
            set
            {
                EnsureChildControls();
                _maskedEditValidator.EmptyValueMessage = value;
            }
        }

        [Category("InfolightValidator")]
        [DefaultValue("")]
        public string InitialValue
        {
            get
            {
                EnsureChildControls();
                return _maskedEditValidator.InitialValue;
            }
            set
            {
                EnsureChildControls();
                _maskedEditValidator.InitialValue = value;
            }
        }

        [Category("InfolightValidator")]
        [DefaultValue("")]
        public string InvalidValueMessage
        {
            get
            {
                EnsureChildControls();
                return _maskedEditValidator.InvalidValueMessage;
            }
            set
            {
                EnsureChildControls();
                _maskedEditValidator.InvalidValueMessage = value;
            }
        }

        [Category("InfolightValidator")]
        [DefaultValue(true)]
        public bool IsValidEmpty
        {
            get
            {
                EnsureChildControls();
                return _maskedEditValidator.IsValidEmpty;
            }
            set
            {
                EnsureChildControls();
                _maskedEditValidator.IsValidEmpty = value;
            }
        }

        [Category("InfolightValidator")]
        [DefaultValue("")]
        public string MaximumValue
        {
            get
            {
                EnsureChildControls();
                return _maskedEditValidator.MaximumValue;
            }
            set
            {
                EnsureChildControls();
                _maskedEditValidator.MaximumValue = value;
            }
        }

        [Category("InfolightValidator")]
        [DefaultValue("")]
        public string MaximumValueMessage
        {
            get
            {
                EnsureChildControls();
                return _maskedEditValidator.MaximumValueMessage;
            }
            set
            {
                EnsureChildControls();
                _maskedEditValidator.MaximumValueMessage = value;
            }
        }

        [Category("InfolightValidator")]
        [DefaultValue("")]
        public string MinimumValue
        {
            get
            {
                EnsureChildControls();
                return _maskedEditValidator.MinimumValue;
            }
            set
            {
                EnsureChildControls();
                _maskedEditValidator.MinimumValue = value;
            }
        }

        [Category("InfolightValidator")]
        [DefaultValue("")]
        public string MinimumValueMessage
        {
            get
            {
                EnsureChildControls();
                return _maskedEditValidator.MinimumValueMessage;
            }
            set
            {
                EnsureChildControls();
                _maskedEditValidator.MinimumValueMessage = value;
            }
        }

        [Category("InfolightValidator")]
        [DefaultValue("")]
        public string TooltipMessage
        {
            get
            {
                EnsureChildControls();
                return _maskedEditValidator.TooltipMessage;
            }
            set
            {
                EnsureChildControls();
                _maskedEditValidator.TooltipMessage = value;
            }
        }

        [Category("InfolightValidator")]
        [DefaultValue("")]
        public string ValidationExpression
        {
            get
            {
                EnsureChildControls();
                return _maskedEditValidator.ValidationExpression;
            }
            set
            {
                EnsureChildControls();
                _maskedEditValidator.ValidationExpression = value;
            }
        }
        #endregion
        #endregion


        protected override void CreateChildControls()
        {
            Controls.Clear();
            _maskedEdit = new TextBox();
            _maskedEdit.ID = "_maskedEdit";
            this.Controls.Add(_maskedEdit);

            _maskedEditValidator = new MaskedEditValidator();
            _maskedEditValidator.ID = "_maskedEditValidator";
            _maskedEditValidator.ControlToValidate = "_maskedEdit";
            _maskedEditValidator.ControlExtender = "_maskedEditExtender";
            this.Controls.Add(_maskedEditValidator);

            if (!this.DesignMode)
            {
                _maskedEditExtender = new MaskedEditExtender();
                _maskedEditExtender.ID = "_maskedEditExtender";
                _maskedEditExtender.TargetControlID = "_maskedEdit";
                this.Controls.Add(_maskedEditExtender);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            _maskedEdit.Width = this.Width;
            _maskedEdit.RenderControl(writer);
            _maskedEditValidator.RenderControl(writer);
            if (!this.DesignMode)
                _maskedEditExtender.RenderControl(writer);
        }
    }
}
