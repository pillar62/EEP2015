using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Srvtools
{
    [ToolboxItem(true)]
    [Designer(typeof(infoCommandDesigner), typeof(IDesigner))]
    //[ToolboxBitmap(typeof(InfoCommand), "Resources.InfoCommand.ico")]
    public class InfoIOTReceiver : InfoBaseComp
    {
        public InfoIOTReceiver()
        {
            _keyFields = new KeyItems(this, typeof(KeyItem));
            _columnMatch = new ColumnMatchItems(this, typeof(KeyItem));
        }

        public InfoIOTReceiver(System.ComponentModel.IContainer container)
        {
            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            container.Add(this);

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            _keyFields = new KeyItems(this, typeof(KeyItem));
            _columnMatch = new ColumnMatchItems(this, typeof(KeyItem));
        }

        private UpdateComponent _updateComp;
        [Category("Infolight"), Description("The UpdateComponent which the control is bound to")]
        public UpdateComponent UpdateComp
        {
            set { _updateComp = value; }
            get { return _updateComp; }
        }

        private bool _alwaysInsert = true;
        [Category("Infolight"), Description("The UpdateComponent which the control is bound to")]
        [DefaultValue(true)]
        public bool AlwaysInsert
        {
            set { _alwaysInsert = value; }
            get { return _alwaysInsert; }
        }

        private KeyItems _keyFields;
        /// <summary>
        /// Gets or sets primary key of InfoCommand.
        /// </summary>
        [Category("Infolight"),
        Description("Primary key of InfoCommand")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public KeyItems KeyFields
        {
            get
            {
                return _keyFields;
            }
        }

        private ColumnMatchItems _columnMatch;
        /// <summary>
        /// Gets or sets primary key of InfoCommand.
        /// </summary>
        [Category("Infolight"), Description("ColumnMatch")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColumnMatchItems ColumnMatch
        {
            get
            {
                return _columnMatch;
            }
        }
    }

    public class ColumnMatchItems : InfoOwnerCollection
    {
        public ColumnMatchItems(Component aOwner, Type aItemType)
            : base(aOwner, typeof(ColumnMatchItem))
        {

        }

        new public ColumnMatchItem this[int index]
        {
            get
            {
                return (ColumnMatchItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is ColumnMatchItem)
                    {
                        //原来的Collection设置为0
                        ((ColumnMatchItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((ColumnMatchItem)InnerList[index]).Collection = this;
                    }

            }
        }
    }

    public class ColumnMatchItem : InfoOwnerCollectionItem, IGetValues
    {
        private string fSourceField = "";
        private string fTargetField = "";

        [Editor(typeof(PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string SourceField
        {
            get
            {
                return fSourceField;
            }
            set
            {
                fSourceField = value;
                if (String.IsNullOrEmpty(TargetField))
                {
                    TargetField = value;
                }
            }
        }

        public string TargetField
        {
            get
            {
                return fTargetField;
            }
            set
            {
                fTargetField = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Name
        {
            get
            {
                return SourceField;
            }

            set
            {
                SourceField = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            if (sKind.Equals("SourceField"))
            {
                InfoCommand cmd = null;
                if (Owner is InfoCommand)
                {
                    cmd = (InfoCommand)Owner;
                    return cmd.GetFields();
                }
                else if (Owner is InfoIOTReceiver)
                {
                    InfoIOTReceiver iot = (InfoIOTReceiver)Owner;
                    UpdateComponent uc = iot.UpdateComp;
                    cmd = uc.SelectCmd;
                }
                return cmd.GetFields();
            }
            else
                return new string[] { };
        }

        //pending...
    }
}
