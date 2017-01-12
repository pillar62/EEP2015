using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Srvtools
{
    public class TransField : TransFieldBase
    {
        #region Constructor

        public TransField() : base("")
        {
            
        }

        public TransField(String desField) : base(desField)
        {
            _updateMode = UpdateMode.Inc;
        }

        #endregion

        #region Propeties

        [Category("Design")]
        public UpdateMode UpdateMode
        {
            set { _updateMode = value; }
            get { return _updateMode; }
        }

        #endregion

        #region Vars

        private UpdateMode _updateMode;

        #endregion
    }

    public enum UpdateMode
    {
        Inc = 0,

        Dec = 1,

        Replace = 2,

        WriteBack = 3,

        Disable = 4
    }
}
