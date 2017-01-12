using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxTools
{
    public class AjaxMenuRootItem
    {
        string _menuId = "";
        string _caption = "";
        string _imageUrl = "";
        List<AjaxMenuLeafItem> _items = new List<AjaxMenuLeafItem>();

        public string MenuId 
        {
            get { return _menuId; }
            set { _menuId = value; }
        }

        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        public List<AjaxMenuLeafItem> Items
        {
            get { return _items; }
        }
    }

    public class AjaxMenuLeafItem
    {
        string _menuId = "";
        string _parent = "";
        string _caption = "";
        string _imageUrl = "";
        string _href = "";

        public string MenuId
        {
            get { return _menuId; }
            set { _menuId = value; }
        }

        public string Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        public string Href
        {
            get { return _href; }
            set { _href = value; }
        }
    }
}
