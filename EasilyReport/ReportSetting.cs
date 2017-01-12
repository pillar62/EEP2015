using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

namespace Infolight.EasilyReportTools
{
    [Obsolete("废除", true)]
    public class ReportSetting
    {
        public ReportSetting()
        {
            this.settingFieldItems = new SettingFieldItemCollection();
        }

        private string totalCaption;

        public string TotalCaption
        {
            get
            {
                return totalCaption;
            }
            set
            {
                totalCaption = value;
            }
        }

        private GroupGapType groupGap;

        public GroupGapType GroupGap
        {
            get { return groupGap; }
            set { groupGap = value; }
        }

        private bool groupTotal;

        public bool GroupTotal
        {
            get
            {
                return groupTotal;
            }
            set
            {
                groupTotal = value;
            }
        }

        private string groupTotalCaption;

        public string GroupTotalCaption
        {
            get
            {
                return groupTotalCaption;
            }
            set
            {
                groupTotalCaption = value;
            }
        }

        private SettingFieldItemCollection settingFieldItems;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
        public SettingFieldItemCollection SettingFieldItems
        {
            get { return settingFieldItems; }
        }

        private CaptionStyleType captionStyle = CaptionStyleType.ColumnHeader;
        [Category("Infolight"),
        Description("The style of the caption.")]
        public CaptionStyleType CaptionStyle
        {
            get { return captionStyle; }
            set { captionStyle = value; }
        }

        public ReportSetting Copy()
        {
            ReportSetting setting;
            setting = new ReportSetting();
            setting.GroupGap = this.GroupGap;
            setting.GroupTotal = this.GroupTotal;
            setting.GroupTotalCaption = this.GroupTotalCaption;
            setting.settingFieldItems = this.SettingFieldItems.Copy();
            setting.TotalCaption = this.TotalCaption;
            setting.CaptionStyle = this.CaptionStyle;
            return setting;
        }

        public enum GroupGapType
        { 
            None,
            EmptyRow,
            SingleLine,
            DoubleLine,
            Page,
            Sheet
        }

        public enum CaptionStyleType
        {
            ColumnHeader,
            RowHeader
        }
    }

    [Obsolete("废除", false)]
    public class SettingFieldItemCollection : IList, ICollection, IEnumerable
    {
        private ArrayList list = new ArrayList();

        public int Add(SettingFieldItem item)
        {
            return list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(SettingFieldItem item)
        {
            return list.Contains(item);
        }

        public int IndexOf(SettingFieldItem item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, SettingFieldItem item)
        {
            list.Insert(index, item);
        }

        public bool IsFixedSize
        {
            get { return list.IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        public void Remove(SettingFieldItem item)
        {
            list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public SettingFieldItem this[int index]
        {
            get
            {
                return (SettingFieldItem)list[index];
            }
            set
            {
                list[index] = value;
            }
        }

        public SettingFieldItemCollection Copy()
        {
            SettingFieldItemCollection newCollection;
            newCollection = new SettingFieldItemCollection();
            foreach (SettingFieldItem sfieldItem in this.list)
            {
                newCollection.Add(sfieldItem.Copy());
            }
            return newCollection;
        }

        public SettingFieldItem Find(string columnName)
        {
            foreach (SettingFieldItem sFieldItem in this.list)
            {
                if (String.Compare(columnName, sFieldItem.ColumnName, true) == 0)
                {
                    return sFieldItem;
                }
            }

            return null;
        }

        #region IList Members

        int IList.Add(object value)
        {
            if (value is SettingFieldItem)
            {
                return list.Add(value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        void IList.Clear()
        {
            list.Clear();
        }

        bool IList.Contains(object value)
        {
            return list.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return list.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            if (value is SettingFieldItem)
            {
                list.Insert(index, value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        bool IList.IsFixedSize
        {
            get { return list.IsFixedSize; }
        }

        bool IList.IsReadOnly
        {
            get { return list.IsReadOnly; }
        }

        void IList.Remove(object value)
        {
            if (value is SettingFieldItem)
            {
                list.Remove(value);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }

        void IList.RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                if (value is ReportItem)
                {
                    list[index] = value;
                }
                else
                {
                    throw new ArgumentException("value");
                }
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            list.CopyTo(array, index);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsSynchronized
        {
            get { return list.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return list.SyncRoot; }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion
        
    }

    [Obsolete("废除", false)]
    public class SettingFieldItem
    {
        public SettingFieldItem()
        {
            this.group = GroupType.None;
        }
        
        private string columnName;
        [Browsable(false)]
        [Category("Infolight"),
        Description("Specifies the column of group.")]
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        private OrderType order;
        [Category("Infolight"),
        Description("Specifies the type of order.")]
        public OrderType Order
        {
            get { return order; }
            set { order = value; }
        }

        private GroupType group;
        [Category("Infolight"),
        Description("Specifies the type of group.")]
        public GroupType Group
        {
            get { return group; }
            set { group = value; }
        }

        public enum OrderType
        {
            Ascend,
            Descend
        }

        public enum GroupType
        {
            None,
            Normal,
            Excel
        }

        public SettingFieldItem Copy()
        {
            SettingFieldItem newItem;
            newItem = new SettingFieldItem();
            newItem.ColumnName = this.ColumnName;
            newItem.Group = this.Group;
            newItem.Order = this.Order;
            return newItem;
        }
    }
}
