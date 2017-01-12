using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Srvtools
{
    public class TransKeyField : TransFieldBase
    {
        #region Constructor

        public TransKeyField() : base("")
        {
            _wherMode = WhereMode.Both;
        }

        public TransKeyField(String desField) : base(desField)
        {
            _wherMode = WhereMode.Both;
        }

        #endregion

        #region Propeties

        [Category("Design")]
        public WhereMode WhereMode
        {
            set { _wherMode = value; }
            get { return _wherMode; }
        }

        #endregion

        #region Vars

        private WhereMode _wherMode;

        #endregion
    }

    public enum WhereMode
    {
        WhereOnly = 0,

        InsertOnly = 1,

        Both = 2
    }
}
