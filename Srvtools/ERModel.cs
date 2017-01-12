#define NewStyle
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;


namespace Srvtools
{
    /// <summary>
    /// Get EIModel of a dataset
    /// </summary>
    public class ERModel: Component
    {
        #region Font
        /// <summary>
        /// Font of Text.
        /// </summary>
        readonly Font TEXT_FONT = new Font("SimSun", 9.0f);
        #endregion

        #region Color
        /// <summary>
        /// Color of graph background.
        /// </summary>
        readonly Color BACK_COLOR = Color.White;
        /// <summary>
        /// Color of border.
        /// </summary>
        readonly Color BORDER_COLOR = Color.Blue;
        /// <summary>
        /// Color of cell border.
        /// </summary>
        readonly Color CELL_BORDER_COLOR = Color.Gray;
        /// <summary>
        /// Color of relation line.
        /// </summary>
        readonly Color RELATION_COLOR = Color.Black;
        /// <summary>
        /// Color of column name.
        /// </summary>
        readonly Color TEXT_COLOR = Color.Black;
        /// <summary>
        /// Color of table name.
        /// </summary>
        readonly Color TITLE_COLOR = Color.White;
        /// <summary>
        /// Color of title background.
        /// </summary>
        readonly Color TITLE_BACK_COLOR = Color.Gray;
        /// <summary>
        /// Color of table background
        /// </summary>
        readonly Color TEXT_BACK_COLOR = Color.White;
        #endregion

        #region Width & Length
        /// <summary>
        /// Width of border.
        /// </summary>
        const float BORDER_WIDTH = 2.0f;
        /// <summary>
        /// Width of cell border.
        /// </summary>
        const float CELL_BORDER_WIDTH = 0.5f;
        /// <summary>
        /// Width of main relation line.
        /// </summary>
        const float RELATION_M_WIDTH = 2.0f;
        /// <summary>
        /// Width of detail relation line.
        /// </summary>
        const float RELATION_D_WIDTH = 1.0F;
        /// <summary>
        /// Length of main relation line.
        /// </summary>
        const float RELATION_M_LENGTH = 40.0f;
        /// <summary>
        /// Length of detail relation line.
        /// </summary>
        const float RELATION_D_LENGTH = 30.0F;
        #endregion

        #region Margin & Padding
        /// <summary>
        /// Top margin of graph.
        /// </summary>
        const float MARGIN_TOP = 50.0f;
        /// <summary>
        /// Left margin of graph.
        /// </summary>
        const float MARGIN_LEFT = 50.0f;
        /// <summary>
        /// Right margin of graph.
        /// </summary>
        const float MARGIN_RIGHT = 50.0f;
        /// <summary>
        /// Bottom margin of graph.
        /// </summary>
        const float MARGIN_BOTTOM = 50.0f;
        /// <summary>
        /// Top padding of table.
        /// </summary>
        const float PADDING_TOP = 3.0f;
        /// <summary>
        /// Left padding of table.
        /// </summary>
        const float PADDING_LEFT = 20.0f;
        /// <summary>
        /// Right padding of table.
        /// </summary>
        const float PADDING_RIGHT = 25.0f;
        /// <summary>
        /// Bottom padding of table.
        /// </summary>
        const float PADDING_BOTTOM = 3.0f;
        /// <summary>
        /// Space between tables.
        /// </summary>
        const float TABLE_SPACE = 20.0f;
        #endregion

        private ImageList imageList;
        private IContainer components;
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ERModel));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "table");
            this.imageList.Images.SetKeyName(1, "key");
        }

        /// <summary>
        /// Create a instance of EIModel on a dataset.
        /// </summary>
        /// <param name="ds">dataset of EIModel</param>
        public ERModel(DataSet ds)
        {
            InitializeComponent();
            DataSet = ds;
        }

        /// <summary>
        /// Create a instance of EIModel on a series of dataset.
        /// </summary>
        /// <param name="ds">Series dataset of EIModel.</param>
        public ERModel(List<DataSet> listds)
        {
            InitializeComponent();
            DataSet ds = new DataSet();
            
            foreach (DataSet dsv in listds)
            {
                DataSet dataSetClone = dsv.Clone();
                foreach (DataTable table in dataSetClone.Tables)
                {
                    int i = 0;
                    while (ds.Tables.Contains(table.TableName))
                    {
                        table.TableName = string.Format("{0}{1}", table.TableName, i);
                        i++;
                    }
                }
                foreach (DataRelation relation in dataSetClone.Relations)
                {
                    int i = 0;
                    while (ds.Relations.Contains(relation.RelationName))
                    {
                        relation.RelationName = string.Format("{0}{1}", relation.RelationName, i);
                        i++;
                    }
                }
                ds.Merge(dataSetClone);
            }
            DataSet = ds;
        }

        private Graphics graph;
        /// <summary>
        /// Gets or sets graph of EIModel.
        /// </summary>
        [Browsable(false)]
        public Graphics Graph
        {
            get { return graph; }
            set { graph = value; }
        }

        private DataSet dataSet;
        /// <summary>
        /// Gets of sets dataset of EIModel.
        /// </summary>
        public DataSet DataSet
        {
            get { return dataSet; }
            set
            {
                dataSet = value;
                tablePosition.Clear();
                columnPosition.Clear();
                if (value != null)
                {
                    AllocateTables(value);
                }
            }
        }

        private SizeF graphSize;
        /// <summary>
        /// Gets the size of graph.
        /// </summary>
        [Browsable(false)]
        public SizeF GraphSize
        {
            get { return graphSize; }
        }

        /// <summary>
        /// Paint graph.
        /// </summary>
        public void Paint()
        {
            if (DataSet == null)
            { 

            }
            else if (Graph == null)
            {

            }
            else
            {
                Graph.FillRectangle(new SolidBrush(BACK_COLOR), 0.0f, 0.0f, graphSize.Width, graphSize.Height);
                foreach (DataTable table in DataSet.Tables)
                {
                    PaintTable(table);
                }
                foreach (DataRelation relation in DataSet.Relations)
                {
                    PaintRelation(relation);
                }
            }
        }

        private void AllocateTables(DataSet ds)
        {
            List<DataTable> parentTables = new List<DataTable>();
            foreach (DataTable table in ds.Tables)
            {
                if (table.ParentRelations.Count == 0)
                {
                    parentTables.Add(table);
                }
            }
            float ypos = MARGIN_TOP;
            float xmax = MARGIN_LEFT;//算出所有Detail最右的位置
            foreach (DataTable table in parentTables)//递归定所有Table的位置
            {
                RectangleF recParent = new RectangleF();
                recParent.X = MARGIN_LEFT;
                recParent.Y = ypos;
                SizeF sizeParent = ComputeTableSize(table);
                recParent.Width = sizeParent.Width;
                recParent.Height = sizeParent.Height;
                tablePosition.Add(table.TableName, recParent);
                PointF detailEnd = AllocateDetailTable(table);
                ypos = detailEnd.Y + TABLE_SPACE;
                xmax = Math.Max(xmax, detailEnd.X);
            }
            graphSize = new SizeF(xmax + MARGIN_RIGHT, ypos - TABLE_SPACE + MARGIN_BOTTOM);//取得画布的大小
        }

        private PointF AllocateDetailTable(DataTable table)
        {
            RectangleF recTable = (RectangleF)tablePosition[table.TableName];//取得表格的位置
            if (table.ChildRelations.Count == 0)
            {
                return new PointF(recTable.Right, recTable.Bottom);
            }
            else
            {
                float ypos = recTable.Top;
                float xmax = recTable.Right;//算出所有Detail最右的位置
                foreach (DataRelation relation in table.ChildRelations)
                {
                    RectangleF recChild = new RectangleF();
                    recChild.X = recTable.Right + RELATION_D_LENGTH + RELATION_M_LENGTH + RELATION_D_LENGTH;
                    recChild.Y = ypos;
                    SizeF sizeChild = ComputeTableSize(relation.ChildTable);
                    recChild.Width = sizeChild.Width;
                    recChild.Height = sizeChild.Height;
                    tablePosition.Add(relation.ChildTable.TableName, recChild);
                    PointF detailEnd = AllocateDetailTable(relation.ChildTable);
                    ypos = detailEnd.Y + TABLE_SPACE;
                    xmax = Math.Max(xmax, detailEnd.X);
                }
                return new PointF(xmax, Math.Max(ypos - TABLE_SPACE, recTable.Bottom));
            }
        }

        private SizeF ComputeTableSize(DataTable table)
        {
            float height = 0.0f;
            float width = 0.0f;
            Size tableSize = TextRenderer.MeasureText(table.TableName, TEXT_FONT);
            height += PADDING_TOP + (float)tableSize.Height + PADDING_BOTTOM;
            width = Math.Max((float)tableSize.Width, width);
            List<string> keys = GetKeys(table);
            foreach (DataColumn column in table.Columns)
            {
#if NewStyle
                if (!keys.Contains(column.ColumnName))
                {
                    continue;
                }
#endif
                Size columnSize = TextRenderer.MeasureText(column.ColumnName, TEXT_FONT);
                height += CELL_BORDER_WIDTH + PADDING_TOP + (float)columnSize.Height + PADDING_BOTTOM;
                width = Math.Max((float)columnSize.Width, width);
            }
#if NewStyle
            height += CELL_BORDER_WIDTH;
            Size textSize = TextRenderer.MeasureText("...", TEXT_FONT);
            height += PADDING_TOP + (float)textSize.Height + PADDING_BOTTOM;
            width = Math.Max((float)textSize.Width, width);
#endif
            width = PADDING_LEFT + width + PADDING_RIGHT;
            return new SizeF(width, height);
        }

        private List<string> GetKeys(DataTable table)
        {
            List<string> keys = new List<string>();
            foreach (DataColumn column in table.PrimaryKey)
            {
                if (!keys.Contains(column.ColumnName))
                {
                    keys.Add(column.ColumnName);
                }
            }
            foreach (DataRelation relation in table.ChildRelations)
            {
                foreach (DataColumn column in relation.ParentColumns)
                {
                    if (!keys.Contains(column.ColumnName))
                    {
                        keys.Add(column.ColumnName);
                    }
                }
            }
            foreach (DataRelation relation in table.ParentRelations)
            {
                foreach (DataColumn column in relation.ChildColumns)
                {
                    if (!keys.Contains(column.ColumnName))
                    {
                        keys.Add(column.ColumnName);
                    }
                }
            }
            return keys;
        }

        private Hashtable tablePosition = new Hashtable();//存table的rectangle

        private Hashtable columnPosition = new Hashtable();//存column的y

        private void PaintTable(DataTable table)
        {
            RectangleF recTable = (RectangleF)tablePosition[table.TableName];//取得表格的位置
            Graph.FillRectangle(new SolidBrush(BACK_COLOR), recTable);//填充表格背景的颜色
            float ypos = recTable.Top;
            Size tableSize = TextRenderer.MeasureText(table.TableName, TEXT_FONT);
            ypos += PADDING_TOP + (float)tableSize.Height + PADDING_BOTTOM;
            Graph.FillRectangle(new SolidBrush(TITLE_BACK_COLOR), recTable.X, recTable.Y, recTable.Width, ypos - recTable.Y);//填充标题背景的颜色
            Image imageTable = imageList.Images["table"];
            Graph.DrawImage(imageTable, recTable.Left + PADDING_LEFT - imageTable.Width, recTable.Top + PADDING_TOP);//画表的图案
            Graph.DrawString(table.TableName, TEXT_FONT, new SolidBrush(TITLE_COLOR), recTable.Left + PADDING_LEFT, recTable.Top + PADDING_TOP);//写标题
            List<DataColumn> primaryColumns = new List<DataColumn>(table.PrimaryKey);
            Image imagekey = imageList.Images["key"];
            List<string> keys = GetKeys(table);
            foreach (DataColumn column in table.Columns)
            {
#if NewStyle
                if (!keys.Contains(column.ColumnName))
                {
                    continue;
                }
#endif
                Graph.DrawLine(new Pen(CELL_BORDER_COLOR, CELL_BORDER_WIDTH)
                    , recTable.Left, ypos + CELL_BORDER_WIDTH / 2, recTable.Right, ypos + CELL_BORDER_WIDTH / 2);//内边框
                ypos += CELL_BORDER_WIDTH;
                if (primaryColumns.Contains(column))
                {
                    Graph.DrawImage(imagekey, recTable.Left + PADDING_LEFT - imagekey.Width , ypos + PADDING_TOP);//画主键的图案
                }
                Graph.DrawString(column.ColumnName, TEXT_FONT, new SolidBrush(TEXT_COLOR), recTable.Left + PADDING_LEFT, ypos + PADDING_TOP);//写Column名字
                Size columnSize = TextRenderer.MeasureText(column.ColumnName, TEXT_FONT);
                columnPosition.Add(string.Format("{0}.{1}", table.TableName, column.ColumnName), ypos + PADDING_TOP + (float)columnSize.Height / 2);//记录Column的位置
                ypos += PADDING_TOP + (float)columnSize.Height + PADDING_BOTTOM;
            }
#if NewStyle
            Graph.DrawLine(new Pen(CELL_BORDER_COLOR, CELL_BORDER_WIDTH)
                  , recTable.Left, ypos + CELL_BORDER_WIDTH / 2, recTable.Right, ypos + CELL_BORDER_WIDTH / 2);//内边框
            ypos += CELL_BORDER_WIDTH;
            Graph.DrawString("...", TEXT_FONT, new SolidBrush(TEXT_COLOR), recTable.Left + PADDING_LEFT, ypos + PADDING_TOP);//写Column名字
#endif
            Graph.DrawRectangle(new Pen(BORDER_COLOR, BORDER_WIDTH), recTable.X, recTable.Y, recTable.Width, recTable.Height);//外边框
        }

        private void PaintRelation(DataRelation relation)
        {
            RectangleF recParentTable = (RectangleF)tablePosition[relation.ParentTable.TableName];
            RectangleF recChildTable = (RectangleF)tablePosition[relation.ChildTable.TableName];
            List<float> listParentPos = new List<float>();
            List<float> listChildPos = new List<float>();
            foreach (DataColumn column in relation.ParentColumns)
            {
                listParentPos.Add((float)columnPosition[string.Format("{0}.{1}", relation.ParentTable.TableName, column.ColumnName)]);//取出Master的点的位置
            }
            foreach (DataColumn column in relation.ChildColumns)
            {
                listChildPos.Add((float)columnPosition[string.Format("{0}.{1}", relation.ChildTable.TableName, column.ColumnName)]);//取出Detail的点的位置
            }

            float relationY = GetCenterValue(listChildPos);//算出Detail点集合中间的位置
            float relationLeft = recParentTable.Right + RELATION_D_LENGTH;
            float relationRight = recChildTable.Left - RELATION_D_LENGTH;
            Graph.DrawLine(new Pen(RELATION_COLOR, RELATION_M_WIDTH), relationLeft, relationY, relationRight, relationY);//画连接线主部分
            Size symbolSize = TextRenderer.MeasureText("∞", TEXT_FONT);
            Graph.DrawString("∞", TEXT_FONT, new SolidBrush(RELATION_COLOR), relationRight - symbolSize.Width, relationY - RELATION_M_WIDTH / 2 - symbolSize.Height);//写∞
            foreach (float pos in listParentPos)//连接所有的master
            {
                Graph.DrawLine(new Pen(RELATION_COLOR, RELATION_D_WIDTH), recParentTable.Right, pos, relationLeft, relationY);
            }
            foreach (float pos in listChildPos)//连接所有的detail
            {
                Graph.DrawLine(new Pen(RELATION_COLOR, RELATION_D_WIDTH), recChildTable.Left, pos, relationRight, relationY);
            }
        }

        private float GetCenterValue(List<float> ypos)
        {
            List<float> temp = new List<float>(ypos);
            temp.Sort();
            return (temp[0] + temp[temp.Count - 1]) / 2;
        }
    }
}
