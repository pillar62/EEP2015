using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;

namespace JQClientTools
{
    public abstract class JQShapeBase
    {
        [Category("Infolight")]
        public abstract string text { get; set; }
        [Category("Infolight")]
        public abstract string description { get; set; }
    }

    public class JQShape : JQShapeBase
    {
        public enum DirectionType
        {
            vertical, horizontal
        }

        public override string text
        {
            get;
            set;
        }
        public override string description
        {
            get;
            set;
        }
        [Editor(typeof(JQGetMenuID), typeof(UITypeEditor))]
        public string linkDiagram
        {
            get;
            set;
        }

        [Category("Appearance"), Browsable(false)]
        public string align { get; set; }
        [Category("Appearance"), Browsable(false)]
        public string fontFamily { get; set; }
        [Category("Appearance"), Browsable(false)]
        public string textStroke { get; set; }
        [Category("Appearance"), Browsable(false)]
        public int fontSize { get; set; }
        [Category("Appearance"), Browsable(false)]
        public string fill { get; set; }
        [Category("Appearance"), Browsable(false)]
        public string strokeWidth { get; set; }
        [Category("Appearance"), Browsable(false)]
        public string stroke { get; set; }
        [Category("Appearance")]
        public string height { get; set; }
        [Category("Appearance")]
        public string width { get; set; }
        [Category("Appearance"), Browsable(false)]
        public string gradient { get; set; }
        [Category("Appearance"), Browsable(false)]
        public DirectionType gradientDirection { get; set; }
        [Category("Appearance"), Browsable(false)]
        public bool shadow { get; set; }
        [Category("Appearance"), Browsable(false)]
        public string shadowSize { get; set; }
    }

    public class JQLine : JQShape
    {
        public enum ArrowType
        {
            None, Left, Right, Both
        }

        public enum LineType
        {
            line, curve
        }

        public enum ArrowStyle
        {
            none, arrow, simplearrow, diamond, circle
        }

        public enum LineStyle
        {
            solid, dash
        }

        [Category("Infolight")]
        public ArrowType arrowType { get; set; }

        [Category("Infolight")]
        public string fromText { get; set; }
        [Category("Infolight")]
        public string lineText { get; set; }
        [Category("Infolight")]
        public string toText { get; set; }

        [Category("Infolight"), Browsable(false)]
        public ArrowStyle fromStyle { get; set; }
        [Category("Infolight"), Browsable(false)]
        public ArrowStyle toStyle { get; set; }

        [Category("Infolight")]
        public LineType type { get; set; }
        [Category("Infolight")]
        public LineStyle lineStyle { get; set; }
    }

    public class JQERLine : JQLine
    {
        public enum RelationType
        {
            One2Many, One2One, Many2One, Many2Many
        }

        public JQERLine()
        {
            relationFields = new JQCollection<AnalysisRelationFieldItem>(this);
        }

        [Category("Infolight")]
        public RelationType relationType { get; set; }

        [Category("Infolight"), Browsable(false)]
        public new ArrowType arrowType { get; set; }
        [Category("Infolight"), Browsable(false)]
        public new string lineText { get; set; }

        [Category("Infolight")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public JQCollection<AnalysisRelationFieldItem> relationFields { get; set; }
    }

    public class JQInOutPut : JQShape
    {
    }

    public class JQTable : JQShape
    {
        public JQTable()
        {
            fields = new JQCollection<AnalysisFieldItem>(this);
            primaryKeys = new JQCollection<AnalysisPrimaryKey>(this);
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string text
        {
            get;
            set;
        }

        [Category("Infolight")]
        public string title { get; set; }


        [Category("Infolight")]
        public string tableName { get; set; }

        [Category("Infolight")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public JQCollection<AnalysisFieldItem> fields { get; set; }

        [Category("Infolight")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public JQCollection<AnalysisPrimaryKey> primaryKeys { get; set; }
    }

    public class JQSegment : JQShapeBase
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string text
        {
            get;
            set;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string description
        {
            get;
            set;
        }

        [Category("Infolight")]
        public string title { get; set; }

        [Category("Appearance")]
        public int width { get; set; }
    }

    public class JQProject : JQShape
    {
        private bool _ExportMenu = true;
        [DefaultValue(true)]
        public bool ExportMenu
        {
            get
            {
                return _ExportMenu;
            }
            set
            {
                _ExportMenu = value;
            }
        }
    }

    //public class AnalysisFieldItems : InfoOwnerCollection
    //{
    //    public AnalysisFieldItems(Component aOwner, Type aItemType)
    //        : base(aOwner, typeof(AnalysisFieldItem))
    //    {

    //    }

    //    new public AnalysisFieldItem this[int index]
    //    {
    //        get
    //        {
    //            return (AnalysisFieldItem)InnerList[index];
    //        }
    //        set
    //        {
    //            if (index > -1 && index < Count)
    //                if (value is AnalysisFieldItem)
    //                {
    //                    //原来的Collection设置为0
    //                    ((AnalysisFieldItem)InnerList[index]).Collection = null;
    //                    InnerList[index] = value;
    //                    //Collection设置为this
    //                    ((AnalysisFieldItem)InnerList[index]).Collection = this;
    //                }

    //        }
    //    }
    //}

    public class AnalysisFieldItem //: InfoOwnerCollectionItem, IGetValues
    {
        public enum FieldType
        {
            Bit, Char, Datetime, Dcimal, Float, Int, Nvarchar, Varchar
        }

        private string _FieldName = "";
        private string _Caption = "";
        private int _Width = 30;
        private FieldType _FieldType = FieldType.Varchar;
        private bool _AllowNull = true;

        public string fieldName
        {
            get
            {
                return _FieldName;
            }
            set
            {
                _FieldName = value;
            }
        }

        public string caption
        {
            get
            {
                return _Caption;
            }
            set
            {
                _Caption = value;
            }
        }

        public int width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
            }
        }

        public FieldType fieldType
        {
            get
            {
                return _FieldType;
            }
            set
            {
                _FieldType = value;
            }
        }

        public bool allowNull
        {
            get
            {
                return _AllowNull;
            }
            set
            {
                _AllowNull = value;
            }
        }

        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public override string Name
        //{
        //    get
        //    {
        //        return fieldName;
        //    }

        //    set
        //    {
        //        fieldName = value;
        //    }
        //}

        public string[] GetValues(string sKind)
        {
            //if (sKind.Equals("FieldName"))
            //{
            //    InfoCommand cmd = (InfoCommand)Owner;
            //    return cmd.GetFields();
            //}
            //else
            return new string[] { };
        }

        //pending...
    }

    public class AnalysisRelationFieldItem //: InfoOwnerCollectionItem, IGetValues
    {
        private string _FromField = "";
        private string _ToField = "";

        [Editor(typeof(JQGetMenuID), typeof(UITypeEditor))]
        public string fromField
        {
            get
            {
                return _FromField;
            }
            set
            {
                _FromField = value;
            }
        }

        [Editor(typeof(JQGetMenuID), typeof(UITypeEditor))]
        public string toField
        {
            get
            {
                return _ToField;
            }
            set
            {
                _ToField = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            return new string[] { };
        }

    }

    //public class AnalysisPrimaryKeys : InfoOwnerCollection
    //{
    //    public AnalysisPrimaryKeys(Component aOwner, Type aItemType)
    //        : base(aOwner, typeof(AnalysisPrimaryKey))
    //    {

    //    }

    //    new public AnalysisPrimaryKey this[int index]
    //    {
    //        get
    //        {
    //            return (AnalysisPrimaryKey)InnerList[index];
    //        }
    //        set
    //        {
    //            if (index > -1 && index < Count)
    //                if (value is AnalysisPrimaryKey)
    //                {
    //                    //原来的Collection设置为0
    //                    ((AnalysisPrimaryKey)InnerList[index]).Collection = null;
    //                    InnerList[index] = value;
    //                    //Collection设置为this
    //                    ((AnalysisPrimaryKey)InnerList[index]).Collection = this;
    //                }

    //        }
    //    }
    //}

    public class AnalysisPrimaryKey //: InfoOwnerCollectionItem, IGetValues
    {
        public enum FieldType
        {
            Bit, Char, Datetime, Dcimal, Float, Int, Nvarchar, Varchar
        }

        private string _PrimaryKey = "";

        [Editor(typeof(JQGetMenuID), typeof(UITypeEditor))]
        public string primaryKey
        {
            get
            {
                return _PrimaryKey;
            }
            set
            {
                _PrimaryKey = value;
            }
        }

        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public override string Name
        //{
        //    get
        //    {
        //        return primaryKey;
        //    }

        //    set
        //    {
        //        primaryKey = value;
        //    }
        //}

        public string[] GetValues(string sKind)
        {
            //if (sKind.Equals("FieldName"))
            //{
            //    InfoCommand cmd = (InfoCommand)Owner;
            //    return cmd.GetFields();
            //}
            //else
            return new string[] { };
        }

        //pending...
    }

}
