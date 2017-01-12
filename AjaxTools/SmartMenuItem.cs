using System.Web.UI;
using System.ComponentModel;

namespace AjaxTools
{
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public class SmartMenuItem : InfoOwnerCollectionItem
    {
        private string _menuid = "";
        private string _parent = "";
        private string _text = "";
        private string _imgUrl = "";
        private string _hoverImgUrl = "";
        private string _url = "";
        //private string _target = "";
        private string _ind = "";

        public SmartMenuItem()
        {
        }

        public SmartMenuItem(string menuid, string parent, string text, string imgUrl, string hoverImgUrl, string url/*, string target*/)
        {
            _menuid = menuid;
            _parent = parent;
            _text = text;
            _imgUrl = imgUrl;
            _hoverImgUrl = hoverImgUrl;
            _url = url;
            //_target = target;
        }

        [NotifyParentProperty(true)]
        public string MenuId
        {
            get
            {
                return _menuid;
            }
            set
            {
                SmartMenuItemCollection collection = this.Collection as SmartMenuItemCollection;
                if (collection != null)
                {
                    foreach (SmartMenuItem item in collection)
                    {
                        if (item.MenuId == value)
                        {
                            System.Windows.Forms.MessageBox.Show("menu id must be unique");
                            return;
                        }
                    }
                }
                _menuid = value;
                if (value != "" && _text != "")
                {
                    _ind = _menuid + "(" + _text + ")";
                }
                else
                {
                    _ind = "";
                }
            }
        }

        [NotifyParentProperty(true)]
        public string Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        [NotifyParentProperty(true)]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                if (value != "" && _menuid != "")
                {
                    _ind = _menuid + "(" + _text + ")";
                }
                else
                {
                    _ind = "";
                }
            }
        }

        [NotifyParentProperty(true)]
        public string ImgUrl
        {
            get
            {
                return _imgUrl;
            }
            set
            {
                _imgUrl = value;
            }
        }

        [NotifyParentProperty(true)]
        public string HoverImgUrl
        {
            get
            {
                return _hoverImgUrl;
            }
            set
            {
                _hoverImgUrl = value;
            }
        }

        [NotifyParentProperty(true)]
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        //public string Target
        //{
        //    get { return _target; }
        //    set { _target = value; }
        //}

        [NotifyParentProperty(true)]
        [Browsable(false)]
        public string Ind
        {
            get { return _ind; }
            set { _ind = value; }
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get { return Ind; }
            set { Ind = value; }
        }

        public override string ToString()
        {
            return Ind;
        }
    }
}
