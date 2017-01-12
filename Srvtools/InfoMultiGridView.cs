using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Collections;

namespace Srvtools
{
    /// <summary>
    /// A gridview can display data in array
    /// </summary>
    public class InfoMultiGridView : DataGridView, ISupportInitialize
    {
        /// <summary>
        /// Create a new instance of infomultigridview
        /// </summary>
        public InfoMultiGridView()
        {
            this.AutoGenerateColumns = false;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.SelectionChanged += delegate(object sender, EventArgs e)
            {
                for (int i = 0; i < this.SelectedCells.Count; i++)
                {
                    this.SelectedCells[i].Selected = false;
                }
            };
        }

        public enum DirectionType
        {
            Vertical,
            Horizontal
        }

        private int _DataRows = 3;
        /// <summary>
        /// Get or set the max rows in vertical direction
        /// </summary>
        [Category("Infolight"),
        Description("Max rows in vertical direction")]
        public int DataRows
        {
            get { return _DataRows; }
            set { _DataRows = value; }
        }

        private int _DataColumns = 1;
        /// <summary>
        /// Get or set the max columns in horizontal direction
        /// </summary>
        [Category("Infolight"),
        Description("Max columns in horizontal direction")]
        public int DataColumns
        {
            get { return _DataColumns; }
            set { _DataColumns = value; }
        }

        private DirectionType _Direction;
        /// <summary>
        /// Get or set the direction of array
        /// </summary>
        [Category("Infolight"),
        Description("Direction of array")]
        public DirectionType Direction
        {
            get { return _Direction; }
            set { _Direction = value; }
        }
	
        private string _ColorValueColumn;
        /// <summary>
        /// Get or set the field apply color
        /// </summary>
        [Category("Infolight"),
        Description("Field apply color")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor,System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
        public string ColorValueColumn
        {
            get { return _ColorValueColumn; }
            set { _ColorValueColumn = value; }
        }

        private string _ColorColumn;
        /// <summary>
        /// Get or set the field determine color
        /// </summary>
        [Category("Infolight"),
        Description("Field determine color")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor,System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
        public string ColorColumn
        {
            get { return _ColorColumn; }
            set { _ColorColumn = value; }
        }

        private ColorCollection _Colors = new ColorCollection();
        /// <summary>
        /// Get the color definations
        /// </summary>
        [Category("Infolight"),
        Description("Color definations")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorCollection Colors
        {
            get { return _Colors; }
        }

        /// <summary>
        /// Get list of data
        /// </summary>
        [Browsable(false)]
        public DataView View
        {
            get 
            {
                if (tempdatasource != null && tempdatasource is BindingSource)
                {
                    return (tempdatasource as BindingSource).List as DataView;
                }
                else
                {
                    return null;
                }
            }
        }
	
        private DataGridViewColumn[] tempcolumns;

        private object tempdatasource;

        #region ISupportInitialize Members

        void ISupportInitialize.BeginInit() { }

        void ISupportInitialize.EndInit()
        {
            if (!this.DesignMode)
            {
                if (tempcolumns == null)
                {
                    tempcolumns = new DataGridViewColumn[this.Columns.Count];
                    this.Columns.CopyTo(tempcolumns, 0);
                }
                tempdatasource = this.DataSource;
                this.DataSource = null;

                this.Columns.Clear();
                this.Rows.Clear();
                if ((Direction == DirectionType.Vertical && DataRows > 0) || (Direction == DirectionType.Horizontal && DataColumns > 0))
                {
                    if (View != null && View.Count > 0)
                    {
                        InitialColumnsAndRows();
                        IntialCells();
                    }
                }
            }
        }

        #endregion

        private void InitialColumnsAndRows()
        {
            int columnscount = 0;
            int rowscount = 0;
            int datacount = View.Count;
            if (Direction == DirectionType.Vertical)
            {
                rowscount = DataRows;
                columnscount = ((datacount - 1) / DataRows + 1) * tempcolumns.Length;
            }
            else
            {
                columnscount = DataColumns * tempcolumns.Length;
                rowscount = (datacount - 1) / DataColumns + 1;
            }
            for (int i = 0; i < columnscount; i++)
            {
                DataGridViewColumn column = tempcolumns[i % tempcolumns.Length].Clone() as DataGridViewColumn;
                column.Tag = column.DataPropertyName;
                column.DataPropertyName = string.Empty;
                column.Name = "Column" + i.ToString();
                this.Columns.Add(column);
            }
            this.Rows.Add(rowscount);
        }

        private void IntialCells()
        {
            int columnindex = 0;
            int rowindex = 0;
            for (int i = 0; i < View.Count; i++)
            {
                SetDataRow(View[i].Row, rowindex, columnindex);
                if (Direction == DirectionType.Vertical)
                {
                    rowindex++;
                    if (rowindex >= this.Rows.Count)
                    {
                        columnindex += tempcolumns.Length;//超出就是异常了
                        rowindex = 0;
                    }
                }
                else
                {
                    columnindex += tempcolumns.Length;
                    if (columnindex >= this.Columns.Count)
                    {
                        rowindex++;//超出就是异常了
                        columnindex = 0;
                    }
                }
            }
        }

        private void SetDataRow(DataRow datarow, int rowindex, int columnindex)
        {
            for (int i = 0; i < tempcolumns.Length; i++)
            {
                if (columnindex + i < this.Columns.Count)
                {
                    string field = this.Columns[columnindex + i].Tag.ToString();
                    this[columnindex + i, rowindex].Value = datarow[field];
                    if (string.Compare(field, ColorColumn, true) == 0)
                    {
                        Color color = GetColor(datarow);
                        if (color != Color.Empty)
                        {
                            this[columnindex + i, rowindex].Style.BackColor = color;
                        }
                    }
                }
            }
        }

        private Color GetColor(DataRow datarow)
        {
            if (datarow[ColorValueColumn] != null)
            {
                ColorItem item = Colors[datarow[ColorValueColumn].ToString()];
                if (item == null)
                {
                    if (datarow[ColorValueColumn].GetType() == typeof(int))
                    {
                        return Color.FromKnownColor((KnownColor)datarow[ColorValueColumn]);
                    }
                    else if (datarow[ColorValueColumn].GetType() == typeof(string))
                    {
                        try
                        {
                            KnownColor kcolor = (KnownColor)Enum.Parse(typeof(KnownColor), datarow[ColorValueColumn].ToString(), true);
                            return Color.FromKnownColor(kcolor);
                        }
                        catch
                        {
                            
                        }
                    }
                }
                else
                {
                    return item.Value;
                }
            }
            return Color.Empty;
        }
    }

    /// <summary>
    /// Color definations collection
    /// </summary>
    public class ColorCollection : System.Collections.CollectionBase
    {
        public ColorCollection()
            : base()
        {

        }

        /// <summary>
        /// Get or set the element 
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns>Value of element</returns>
        public ColorItem this[int index]
        {
            get { return InnerList[index] as ColorItem; }
            set { InnerList[index] = value; }
        }

        /// <summary>
        /// Get or set the element 
        /// </summary>
        /// <param name="name">Name of element</param>
        /// <returns>Value of element</returns>
        public ColorItem this[string name]
        {
            get 
            {
                for (int i = 0; i < Count; i++)
                {
                    if (this[i].Name == name)
                    {
                        return this[i];
                    }
                }
                return null;
            }
            set 
            { 
                ColorItem item = this[name];
                if (item == null)
                {
                    InnerList.Add(value);
                }
                else
                {
                    int index = InnerList.IndexOf(item);
                    InnerList.RemoveAt(index);
                    InnerList.Insert(index, value);
                }
            }
        }

        /// <summary>
        /// Add element to colletion
        /// </summary>
        /// <param name="item">Value of element</param>
        public void Add(ColorItem item)
        {
            if (this[item.Name] == null)
            {
                InnerList.Add(item);
            }
            else
            {
                throw new ArgumentException(string.Format("Color item:{0} has already exsit", item.Name));
            }
        }

        /// <summary>
        /// Insert element into collection
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <param name="value">Value of element</param>
        public void Insert(int index, ColorItem item)
        {
            InnerList.Insert(index, item);
        }

        /// <summary>
        /// Remove element from collection
        /// </summary>
        /// <param name="value">Value of element</param>
        public void Remove(ColorItem item)
        {
            InnerList.Remove(item);
        }

        /////// <summary>
        /////// Remove element from collection by index
        /////// </summary>
        /////// <param name="index">Index of element</param>
        ////public void RemoveAt(int index)
        ////{
        ////    InnerList.RemoveAt(index);
        ////}

        /////// <summary>
        /////// Clear collection
        /////// </summary>
        ////public void Clear()
        ////{
        ////    InnerList.Clear();
        ////}

        /////// <summary>
        /////// Get IEnumerator 
        /////// </summary>
        /////// <returns>IEnumerator</returns>
        ////public IEnumerator GetEnumerator()
        ////{
        ////    return InnerList.GetEnumerator();
        ////}
    }

    /// <summary>
    /// Color defination
    /// </summary>
    public class ColorItem
    {
        private string _Name;
        /// <summary>
        /// Get or set name of item
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private Color _Value;
        /// <summary>
        /// Get or set name of value
        /// </summary>
        public Color Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }
}
