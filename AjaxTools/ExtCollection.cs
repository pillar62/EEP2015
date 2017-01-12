using System;

namespace AjaxTools
{
    public class ExtResourceFileCollection : InfoOwnerCollection
    {
        public ExtResourceFileCollection(object owner, Type itemType)
            : base(owner, typeof(ExtResourceFile))
        {
        }

        public new ExtResourceFile this[int index]
        {
            get
            {
                return (ExtResourceFile)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is ExtResourceFile)
                    {
                        ((ExtResourceFile)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((ExtResourceFile)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class ExtGridColumnCollection : InfoOwnerCollection
    {
        public ExtGridColumnCollection(object owner, Type itemType)
            : base(owner, typeof(ExtGridColumn))
        {
        }

        public new ExtGridColumn this[int index]
        {
            get
            {
                return (ExtGridColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is ExtGridColumn)
                    {
                        ((ExtGridColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((ExtGridColumn)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class ExtGridToolItemCollection : InfoOwnerCollection
    {
        public ExtGridToolItemCollection(object owner, Type itemType)
            : base(owner, typeof(ExtGridToolItem))
        {
        }

        public new ExtGridToolItem this[int index]
        {
            get
            {
                return (ExtGridToolItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is ExtGridToolItem)
                    {
                        ((ExtGridToolItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((ExtGridToolItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class ExtSimpleColumnCollection : InfoOwnerCollection
    {
        public ExtSimpleColumnCollection(object owner, Type itemType)
            : base(owner, typeof(ExtSimpleColumn))
        {
        }

        public new ExtSimpleColumn this[int index]
        {
            get
            {
                return (ExtSimpleColumn)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is ExtSimpleColumn)
                    {
                        ((ExtSimpleColumn)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((ExtSimpleColumn)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class ExtColumnMatchCollection : InfoOwnerCollection
    {
        public ExtColumnMatchCollection(object owner, Type itemType)
            : base(owner, typeof(ExtColumnMatch))
        {
        }

        public new ExtColumnMatch this[int index]
        {
            get
            {
                return (ExtColumnMatch)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is ExtColumnMatch)
                    {
                        ((ExtColumnMatch)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((ExtColumnMatch)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class AjaxFormFieldCollection : InfoOwnerCollection
    {
        public AjaxFormFieldCollection(object owner, Type itemType)
            : base(owner, typeof(AjaxFormField))
        {
        }

        public new AjaxFormField this[int index]
        {
            get
            {
                return (AjaxFormField)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is AjaxFormField)
                    {
                        ((AjaxFormField)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((AjaxFormField)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class ExtWhereItemCollection : InfoOwnerCollection
    {
        public ExtWhereItemCollection(object owner, Type itemType)
            : base(owner, typeof(ExtWhereItem))
        {
        }

        public new ExtWhereItem this[int index]
        {
            get
            {
                return (ExtWhereItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is ExtWhereItem)
                    {
                        ((ExtWhereItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((ExtWhereItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class MultiViewItemCollection : InfoOwnerCollection
    {
        public MultiViewItemCollection(object owner, Type itemType)
            : base(owner, typeof(MultiViewItem))
        {
        }

        public new MultiViewItem this[int index]
        {
            get
            {
                return (MultiViewItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is MultiViewItem)
                    {
                        ((MultiViewItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((MultiViewItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class ExtQueryFieldCollection : InfoOwnerCollection
    {
        public ExtQueryFieldCollection(object owner, Type itemType)
            : base(owner, typeof(ExtQueryField))
        {
        }

        public new ExtQueryField this[int index]
        {
            get
            {
                return (ExtQueryField)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is ExtQueryField)
                    {
                        ((ExtQueryField)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((ExtQueryField)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class ExtRefButtonColumnMatchCollection : InfoOwnerCollection
    {
        public ExtRefButtonColumnMatchCollection(object owner, Type itemType)
            : base(owner, typeof(ExtRefButtonColumnMatch))
        {
        }

        public new ExtRefButtonColumnMatch this[int index]
        {
            get
            {
                return (ExtRefButtonColumnMatch)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is ExtRefButtonColumnMatch)
                    {
                        ((ExtRefButtonColumnMatch)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        ((ExtRefButtonColumnMatch)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
}