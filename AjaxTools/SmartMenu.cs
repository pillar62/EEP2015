using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using Srvtools;

//stm_bm 没有研究，只知道是代码的整体设置
//stm_em(); 相对应上面的闭合标签，应该在代码的最后

//stm_bp
//[]内的说明：主菜单的样式内容
//01.主菜单排列方式 0 横排 1 竖排
//02.未知
//03.未知
//04.未知
//05.主菜单的内间距
//06.主菜单的内间距
//07.未知
//08.未知
//09.主菜单的透明度
//10.下级菜单的探出的样式代码
//11.下级菜单的探出的样式代码时间
//12.下级菜单的关闭的样式代码
//13.下级菜单的关闭的样式代码时间
//14.未知
//15.未知
//16.主菜单的阴影大小
//17.主菜单的边框颜色 右下阴影
//18.主菜单的边框颜色 左上阴影
//19.未知 ""
//20.未知
//21.未知
//22.未知
//23.边框的颜色

//stm_ai
//[]内的说明 文字样式
//01.显示的内容 0 文本 1 html 2 图片
//02.文字内容
//03.鼠标移出时的图片地址 显示内容为2时有效
//04.鼠标悬停时的图片地址 显示内容为2时有效
//05.图片的宽 显示内容为2时有效
//06.图片的高 显示内容为2时有效
//07.图片的边框 显示内容为2时有效
//08.连接地址
//09.连接方式 用html代码
//10.鼠标悬停时状态栏显示的内容 空值为显示连接地址
//11.鼠标悬停时探出的提示内容 ""
//12.鼠标悬停时前面图标的文件路径 ""
//13.鼠标移出时前面图标的文件路径 ""
//14.前面图标的宽
//15.前面图标的高
//16.前面图标的边框
//17.鼠标悬停时后面图标的文件路径 ""
//18.鼠标移出时后面图标的文件路径 ""
//19.后面图标的宽
//20.后面图标的高
//21.后面图标的边框
//22.边框的样式 1-6 实线 双线 点线 虚线 菱形凹槽 菱形凸起
//23.边框的厚度
//24.鼠标移出的背景色 颜色
//25.鼠标移出的文字颜色
//26.鼠标放置的背景色 颜色
//27.鼠标放置的文字颜色
//28.鼠标移出的背景文件路径
//29.鼠标悬停的背景文件路径
//30.鼠标移出的文字字体以及样式
//31.鼠标悬停的文字字体以及样式
//32.鼠标移出的文字修饰方式 下划线 加粗 上划线 等等
//33.鼠标悬停的文字修饰方式 同上

//stm_bp
//方括号内的说明
//01.下级菜单的排列方式 1 竖排 0 横排
//02.下级菜单探出方向 2 → 1 ← 3 ↑ 4 ↓
//03.下级菜单左右偏移多少像素 
//04.下级菜单上下偏移多少像素
//05.下级菜单表格的间距
//06.下级菜单表格的间距
//07.下级菜单前有多少空格
//08.下级菜单后有多少空格
//09.下级菜单的透明度
//10.下级菜单的探出的样式代码
//11.下级菜单的探出的样式代码时间
//12.下级菜单的关闭的样式代码
//13.下级菜单的关闭的样式代码时间

//stm_ep();闭合标签，每出现一个stm_bp将必须有一个相对应的闭合标签，在关闭这个标签之前再次出现stm_bp表示出现了一个下级菜单


namespace AjaxTools
{
    [Designer(typeof(SmartMenuDesigner), typeof(IDesigner))]
    public class SmartMenu : AjaxBaseWebControl, INamingContainer, IAjaxDataSource
    {
        #region variate
        private SmartMenuItemCollection _items;
        private bool _showBoder = false;
        private int _opacity = 100;
        private float _duration = 0.30f;
        private string _target = "";
        private Color _menuItemBackColor = Color.Empty;
        private Color _menuItemHoverBackColor = Color.Empty;
        private Color _menuItemFontColor = Color.Empty;
        private Color _menuItemHoverFontColor = Color.Empty;
        private string _dataSourceId = "";
        private string _menuIdField = "";
        private string _menuTextField = "";
        private string _menuParentField = "";
        private string _menuImageUrlField = "";
        private string _menuHoverImageUrlField = "";
        private string _menuUrlField = "";
        private int _itemDivision = 0;

        private int block = 1;
        private List<SmartMenuItem> RootMenu = new List<SmartMenuItem>();
        private List<SmartMenuItem> ChildMenu = new List<SmartMenuItem>();
        private bool pStartSet = false;
        private string pStart = "";
        private bool pEndSet = false;
        private string pEnd = "";
        private bool rootBringP1 = false;
        private string c0i0 = "";
        private string c1i0 = "";
        private string c0i1 = "";
        private string c1i1 = "";
        private bool setArrow = false;
        private bool setLong = false;
        #endregion

        public SmartMenu()
        {
            _items = new SmartMenuItemCollection(this, typeof(SmartMenuItem));
        }

        #region Properties
        [Category("Infolight")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [TypeConverter(typeof(CollectionConverter))]
        public SmartMenuItemCollection Items
        {
            get { return _items; }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        public bool ShowBoder
        {
            get { return _showBoder; }
            set { _showBoder = value; }
        }

        [Category("Infolight")]
        [DefaultValue(100)]
        public int Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }

        [Category("Infolight")]
        [DefaultValue(0.30f)]
        public float Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        [Category("Infolight")]
        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }

        [Category("Infolight")]
        public int ItemDivision
        {
            get { return _itemDivision; }
            set { _itemDivision = value; }
        }

        [Category("Infolight")]
        public Color MenuItemBackColor
        {
            get 
            {
                if (_menuItemBackColor == Color.Empty)
                    return ColorTranslator.FromHtml("#ffffff");
                return _menuItemBackColor; 
            }
            set 
            { 
                _menuItemBackColor = value; 
            }
        }

        [Category("Infolight")]
        public Color MenuItemHoverBackColor
        {
            get 
            {
                if (_menuItemHoverBackColor == Color.Empty)
                    return ColorTranslator.FromHtml("#000000");
                return _menuItemHoverBackColor; 
            }
            set 
            { 
                _menuItemHoverBackColor = value;
            }
        }

        [Category("Infolight")]
        public Color MenuItemFontColor
        {
            get 
            {
                if (_menuItemFontColor == Color.Empty)
                    return ColorTranslator.FromHtml("#000000");
                return _menuItemFontColor; 
            }
            set
            {
                _menuItemFontColor = value; 
            }
        }

        [Category("Infolight")]
        public Color MenuItemHoverFontColor
        {
            get 
            {
                if (_menuItemFontColor == Color.Empty)
                    return ColorTranslator.FromHtml("#ffffff");
                return _menuItemHoverFontColor; 
            }
            set
            {
                _menuItemHoverFontColor = value; 
            }
        }

        [Category("Data")]
        [Editor(typeof(AjaxTools.DataSourceEditor), typeof(UITypeEditor))]
        public string DataSourceID
        {
            get { return _dataSourceId; }
            set { _dataSourceId = value; }
        }

        [Category("Data")]
        [Editor(typeof(AjaxTools.FieldNameEditor), typeof(UITypeEditor))]
        public string MenuIDField
        {
            get { return _menuIdField; }
            set { _menuIdField = value; }
        }

        [Category("Data")]
        [Editor(typeof(AjaxTools.FieldNameEditor), typeof(UITypeEditor))]
        public string MenuTextField
        {
            get { return _menuTextField; }
            set { _menuTextField = value; }
        }

        [Category("Data")]
        [Editor(typeof(AjaxTools.FieldNameEditor), typeof(UITypeEditor))]
        public string MenuParentField
        {
            get { return _menuParentField; }
            set { _menuParentField = value; }
        }

        [Category("Data")]
        [Editor(typeof(AjaxTools.FieldNameEditor), typeof(UITypeEditor))]
        public string MenuImageUrlField
        {
            get { return _menuImageUrlField; }
            set { _menuImageUrlField = value; }
        }

        [Category("Data")]
        [Editor(typeof(AjaxTools.FieldNameEditor), typeof(UITypeEditor))]
        public string MenuHoverImageUrlField
        {
            get { return _menuHoverImageUrlField; }
            set { _menuHoverImageUrlField = value; }
        }

        [Category("Data")]
        [Editor(typeof(AjaxTools.FieldNameEditor), typeof(UITypeEditor))]
        public string MenuUrlField
        {
            get { return _menuUrlField; }
            set { _menuUrlField = value; }
        }
        #endregion

        private bool isBaseFieldSet()
        {
            return !String.IsNullOrEmpty(this.MenuIDField) && !String.IsNullOrEmpty(this.MenuParentField) && !String.IsNullOrEmpty(this.MenuTextField);
        }

        private bool IsImageFieldSet()
        {
            return !String.IsNullOrEmpty(this.MenuImageUrlField);
        }

        private bool IsHoverImageFieldSet()
        {
            return !String.IsNullOrEmpty(this.MenuHoverImageUrlField);
        }

        private bool IsUrlFieldSet()
        {
            return !String.IsNullOrEmpty(this.MenuUrlField);
        }

        protected override void OnLoad(EventArgs e)
        {
            object objDs = this.GetObjByID(this.DataSourceID);
            if (objDs != null && objDs is WebDataSource)
            {
                WebDataSource wds = (WebDataSource)objDs;
                DataTable menuTable = null;
                if (wds.SelectAlias != null && wds.SelectAlias != "" && wds.SelectCommand != null && wds.SelectCommand != "")
                    menuTable = wds.CommandTable;
                else if (wds.DataMember != null && wds.DataMember != "")
                    menuTable = wds.InnerDataSet.Tables[wds.DataMember];
                if (menuTable != null && isBaseFieldSet())
                {
                    foreach (DataRow row in menuTable.Rows)
                    {
                        string id = row[this.MenuIDField] == null ? "" : row[this.MenuIDField].ToString();
                        string parent = row[this.MenuParentField] == null ? "" : row[this.MenuParentField].ToString();
                        string text = row[this.MenuTextField] == null ? "" : row[this.MenuTextField].ToString();
                        string url = "", imgUrl = "", hoverImgUrl = "";
                        if (IsUrlFieldSet())
                            url = row[this.MenuUrlField] == null ? "" : row[this.MenuUrlField].ToString();
                        if (IsImageFieldSet())
                            imgUrl = row[this.MenuImageUrlField] == null ? "" : row[this.MenuImageUrlField].ToString();
                        if (IsHoverImageFieldSet())
                            hoverImgUrl = row[this.MenuHoverImageUrlField] == null ? "" : row[this.MenuHoverImageUrlField].ToString();
                        SmartMenuItem item = new SmartMenuItem(id, parent, text, imgUrl, hoverImgUrl, url);
                        this.Items.Add(item);
                    }
                }
            }
            base.OnLoad(e);
            ClientScriptManager csm = Page.ClientScript;
            Type cstype = this.Page.GetType();
            if (!csm.IsClientScriptIncludeRegistered(cstype, "refScript"))
            {
                if (IsClientMain())
                    csm.RegisterClientScriptInclude(cstype, "refScript", "stm31.js");
                else
                    csm.RegisterClientScriptInclude(cstype, "refScript", "../stm31.js");
            }
        }

        private bool IsClientMain()
        {
            string path = this.Page.Request.FilePath;
            if (path.Contains("webClientMain.aspx"))
                return true;
            else
                return false;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ClientScriptManager csm = Page.ClientScript;
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            if (!this.DesignMode)
            {
                if (!csm.IsClientScriptBlockRegistered("createMenuScript") && this.Items.Count > 0)
                {
                    writer.Write(this.MenuScript());
                    csm.RegisterClientScriptBlock(this.GetType(), "createMenuScript", "");
                }
            }
            writer.RenderEndTag();
        }

        protected virtual string MenuScript()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<script language=\"javascript\" type=\"text/javascript\">");
            string borderString = "0,0", opacityString = GetOpacity();
            if (ShowBoder)
                borderString = "6,3";
            string blank = "";
            if(IsClientMain())
                blank = "Image/Ajax/blank.gif";
            else
                blank = "../Image/Ajax/blank.gif";
            builder.AppendLine("stm_bm([\"gloMenu\",400,\"\",\"" + blank + "\",0,\"\",\"\",0,0,250,0,1000,1,0,0,\"\",\"\",0],this);");
            builder.AppendLine("stm_bp(\"p0\",[0,4,0,0," + this.ItemDivision + ",2,7,7," + opacityString + ",\"\",-2,\"\",-2,90,2,3,\"#000000\",\"transparent\",\"\",3," + borderString + ",\"#ffffff\"]);");
            foreach (SmartMenuItem item in this.Items)
            {
                if (item.Parent == "")
                    RootMenu.Add(item);
                else
                    ChildMenu.Add(item);
            }
            bool IsFirstRoot = true;

            int rootIndex = 0;
            foreach (SmartMenuItem item in RootMenu)
            {
                List<SmartMenuItem> childMenu = GetChildren(item.MenuId, ChildMenu);
                string id = "p0i" + rootIndex.ToString(), url = "", imgArrow = "", refer = "", imgUrl = item.ImgUrl, hoverImgUrl = item.HoverImgUrl;

                if (item.Url != null && item.Url != "")
                    url = item.Url;

                if (childMenu.Count > 0 && !setArrow)
                {
                    if (IsClientMain())
                        imgArrow = "Image/Ajax/arrow_r.gif";
                    else
                        imgArrow = "../Image/Ajax/arrow_r.gif";
                    setArrow = true;
                }
                bool ci = SetCI(childMenu.Count, id, imgUrl);
                if (IsFirstRoot)
                {
                    builder.AppendLine("stm_ai(\"" + id + "\",[0,\"" + item.Text + "\",\"\",\"\",-1,-1,0,\"" + url + "\",\"" + this.Target + "\",\"\",\"\",\"" + imgUrl + "\",\"" + hoverImgUrl + "\",-1,-1,0,\"" + imgArrow + "\",\"" + imgArrow + "\",7,7,0,0,1,\"" + GloFix.ToHtmlColor(this.MenuItemBackColor) + "\",0,\"" + GloFix.ToHtmlColor(this.MenuItemHoverBackColor) + "\",0,\"\",\"\",3,3,0,0,\"#ffffff\",\"#ffffff\",\"" + GloFix.ToHtmlColor(this.MenuItemFontColor) + "\",\"" + GloFix.ToHtmlColor(this.MenuItemHoverFontColor) + "\",\"9pt Verdana\",\"9pt Verdana\",0,0]);");
                    if (childMenu.Count > 0)
                    {
                        pStart = "p" + block.ToString();
                        builder.AppendLine("stm_bpx(\"" + pStart + "\",\"p0\",[1,4,0,0,0,2,7,7," + opacityString + ",\"progid:DXImageTransform.Microsoft.Stretch(stretchStyle=spin,enabled=0,Duration=" + this.Duration.ToString() + ")\",14,\"\",-2,90,0,0]);");
                        pStartSet = true;
                        rootBringP1 = true;
                    }
                }
                else
                {
                    refer = this.GetRefer(childMenu.Count, id, imgUrl);
                    if (imgArrow != "")
                        builder.AppendLine("stm_aix(\"" + id + "\",\"" + refer + "\",[0,\"" + item.Text + "\",\"\",\"\",-1,-1,0,\"" + url + "\",\"" + this.Target + "\",\"\",\"\",\"" + imgUrl + "\",\"" + hoverImgUrl + "\",-1,-1,0,\"" + imgArrow + "\",\"" + imgArrow + "\",7,7]);");
                    else
                    {
                        if (!ci)
                        {
                            if (imgUrl != "")
                                builder.AppendLine("stm_aix(\"" + id + "\",\"" + refer + "\",[0,\"" + item.Text + "\",\"\",\"\",-1,-1,0,\"" + url + "\",\"" + this.Target + "\",\"\",\"\",\"" + imgUrl + "\",\"" + hoverImgUrl + "\"]);");
                            else
                            {
                                if (url != "")
                                    builder.AppendLine("stm_aix(\"" + id + "\",\"" + refer + "\",[0,\"" + item.Text + "\",\"\",\"\",-1,-1,0,\"" + url + "\",\"" + this.Target + "\"]);");
                                else
                                    builder.AppendLine("stm_aix(\"" + id + "\",\"" + refer + "\",[0,\"" + item.Text + "\"]);");
                            }
                        }
                        else
                        {
                            if (childMenu.Count == 0 && !setLong)
                            {
                                builder.AppendLine("stm_aix(\"" + id + "\",\"" + refer + "\",[0,\"" + item.Text + "\",\"\",\"\",-1,-1,0,\"" + url + "\",\"" + this.Target + "\",\"\",\"\",\"" + imgUrl + "\",\"" + hoverImgUrl + "\",-1,-1,0,\"\",\"\",0,0]);");
                                setLong = true;
                            }
                            else
                                builder.AppendLine("stm_aix(\"" + id + "\",\"" + refer + "\",[0,\"" + item.Text + "\",\"\",\"\",-1,-1,0,\"" + url + "\",\"" + this.Target + "\",\"\",\"\",\"" + imgUrl + "\",\"" + hoverImgUrl + "\",-1,-1]);");
                        }
                    }
                    if (!pStartSet)
                    {
                        pStart = "p" + block.ToString();
                        builder.AppendLine("stm_bpx(\"" + pStart + "\",\"p0\",[1,4,0,0,0,2,7,7," + opacityString + ",\"progid:DXImageTransform.Microsoft.Stretch(stretchStyle=spin,enabled=0,Duration=" + this.Duration.ToString() + ")\",14,\"\",-2,90,0,0]);");
                        pStartSet = true;
                        rootBringP1 = true;
                    }
                }
                // 生成子menu
                builder.Append(CreateChildMenu(childMenu, true));
                rootIndex++;
                IsFirstRoot = false;
            }
            builder.AppendLine("stm_em();");
            builder.AppendLine("</script>");

            return builder.ToString();
        }

        private StringDictionary ppDic = new StringDictionary();
        private string CreateChildMenu(List<SmartMenuItem> childMenu, bool isRootChild)
        {
            StringBuilder builder = new StringBuilder();
            if (isRootChild)
            {
                if (!rootBringP1)
                {
                    if (pStartSet && childMenu.Count > 0)
                    {
                        block++;
                        builder.AppendLine("stm_bpx(\"p" + block.ToString() + "\",\"" + pStart + "\",[]);");
                    }
                }
                else
                    rootBringP1 = false;
            }
            else
            {
                if (childMenu.Count > 0)
                    if (!pEndSet)
                    {
                        block++;
                        string opacityString = GetOpacity();
                        builder.AppendLine("stm_bpx(\"p" + block.ToString() + "\",\"" + pStart + "\",[1,2,0,0,0,2,7,7," + opacityString + ",\"progid:DXImageTransform.Microsoft.Stretch(stretchStyle=hide,enabled=0,Duration=" + this.Duration.ToString() + ")\"]);");
                        pEnd = "p" + block.ToString();
                        pEndSet = true;
                    }
                    else
                    {
                        block++;
                        builder.Append("stm_bpx(\"p" + block.ToString() + "\",\"" + pEnd + "\",[]);");
                    }
            }
            int rootIndex = 0;
            foreach (SmartMenuItem item in childMenu)
            {
                string plevel = "";
                if (ppDic.ContainsKey(item.Parent))
                {
                    plevel = ppDic[item.Parent];
                }
                else
                {
                    plevel = "p" + block.ToString();
                    ppDic.Add(item.Parent, plevel);
                }

                List<SmartMenuItem> _childMenu = GetChildren(item.MenuId, ChildMenu);
                string id = plevel + "i" + rootIndex.ToString(), url = "", imgArrow = "", refer = "", imgUrl = item.ImgUrl, hoverImgUrl = item.HoverImgUrl;
                if (item.Url != null && item.Url != "")
                    url = item.Url;

                if (_childMenu.Count > 0 && !setArrow)
                {
                    if (IsClientMain())
                        imgArrow = "Image/Ajax/arrow_r.gif";
                    else
                        imgArrow = "../Image/Ajax/arrow_r.gif"; 
                    setArrow = true;
                }
                bool ci = SetCI(_childMenu.Count, id, imgUrl);
                refer = GetRefer(_childMenu.Count, id, imgUrl);
                if (imgArrow != "")
                    builder.AppendLine("stm_aix(\"" + id + "\",\"" + refer + "\",[0,\"" + item.Text + "\",\"\",\"\",-1,-1,0,\"" + url + "\",\"" + this.Target + "\",\"\",\"\",\"" + imgUrl + "\",\"" + hoverImgUrl + "\",-1,-1,0,\"" + imgArrow + "\",\"" + imgArrow + "\"]);");
                else
                {
                    if (!ci)
                    {
                        if (imgUrl != "")
                            builder.AppendLine("stm_aix(\"" + id + "\",\"" + refer + "\",[0,\"" + item.Text + "\",\"\",\"\",-1,-1,0,\"" + url + "\",\"" + this.Target + "\",\"\",\"\",\"" + imgUrl + "\",\"" + hoverImgUrl + "\"]);");
                        else
                        {
                            if (url != "")
                                builder.AppendLine("stm_aix(\"" + id + "\",\"" + refer + "\",[0,\"" + item.Text + "\",\"\",\"\",-1,-1,0,\"" + url + "\",\"" + this.Target + "\"]);");
                            else
                                builder.AppendLine("stm_aix(\"" + id + "\",\"" + refer + "\",[0,\"" + item.Text + "\"]);");
                        }
                    }
                    else
                    {
                        if (_childMenu.Count == 0 && !setLong)
                        {
                            builder.AppendLine("stm_aix(\"" + id + "\",\"" + refer + "\",[0,\"" + item.Text + "\",\"\",\"\",-1,-1,0,\"" + url + "\",\"" + this.Target + "\",\"\",\"\",\"" + imgUrl + "\",\"" + hoverImgUrl + "\",-1,-1,0,\"\",\"\",0,0]);");
                            setLong = true;
                        }
                        else
                            builder.AppendLine("stm_aix(\"" + id + "\",\"" + refer + "\",[0,\"" + item.Text + "\",\"\",\"\",-1,-1,0,\"" + url + "\",\"" + this.Target + "\",\"\",\"\",\"" + imgUrl + "\",\"" + hoverImgUrl + "\",-1,-1]);");
                    }
                }
                builder.AppendLine(CreateChildMenu(_childMenu, false));
                rootIndex++;
            }
            if (childMenu.Count > 0)
                builder.AppendLine("stm_ep();");
            return builder.ToString();
        }

        private bool SetCI(int childrenNum, string id, string imgUrl)
        {
            bool ci = false;
            if (childrenNum > 0)
            {
                if (imgUrl == "")
                {
                    if (c1i0 == "")
                    {
                        c1i0 = id;
                        ci = true;
                    }
                }
                else
                {
                    if (c1i1 == "")
                    {
                        c1i1 = id;
                        ci = true;
                    }
                }
            }
            else
            {
                if (imgUrl == "")
                {
                    if (c0i0 == "")
                    {
                        c0i0 = id;
                        ci = true;
                    }
                }
                else
                {
                    if (c0i1 == "")
                    {
                        c0i1 = id;
                        ci = true;
                    }
                }
            }
            return ci;
        }

        private string GetRefer(int childrenNum, string id, string imgUrl)
        {
            string refer = "";
            if (childrenNum > 0)
            {
                if (imgUrl == "")
                    refer = c1i0;
                else
                    refer = c1i1;
            }
            else
            {
                if (imgUrl == "")
                    refer = c0i0;
                else
                    refer = c0i1;
            }

            if (id == refer)
            {
                if (id == c0i0)
                {
                    if (c0i1 == "")
                    {
                        if (imgUrl == "")
                        {
                            if (c1i0 != "")
                                refer = c1i0;
                            else
                                refer = c1i1;
                        }
                        else
                        {
                            if (c1i1 != "")
                                refer = c1i1;
                            else
                                refer = c1i0;
                        }
                    }
                    else
                        refer = c0i1;
                }
                else if (id == c0i1)
                {
                    if (c0i0 == "")
                    {
                        if (imgUrl == "")
                        {
                            if (c1i0 != "")
                                refer = c1i0;
                            else
                                refer = c1i1;
                        }
                        else
                        {
                            if (c1i1 != "")
                                refer = c1i1;
                            else
                                refer = c1i0;
                        }
                    }
                    else
                        refer = c0i0;
                }
                else if (id == c1i0)
                {
                    if (c1i1 == "")
                    {
                        if (imgUrl == "")
                        {
                            if (c0i0 != "")
                                refer = c0i0;
                            else
                                refer = c0i1;
                        }
                        else
                        {
                            if (c0i1 != "")
                                refer = c0i1;
                            else
                                refer = c0i0;
                        }
                    }
                    else
                        refer = c1i1;
                }
                else if (id == c1i1)
                {
                    if (c1i0 == "")
                    {
                        if (imgUrl == "")
                        {
                            if (c0i0 != "")
                                refer = c0i0;
                            else
                                refer = c0i1;
                        }
                        else
                        {
                            if (c0i1 != "")
                                refer = c0i1;
                            else
                                refer = c0i0;
                        }
                    }
                    else
                        refer = c1i0;
                }
            }
            return refer;
        }

        private List<SmartMenuItem> GetChildren(string id, List<SmartMenuItem> childMenu)
        {
            List<SmartMenuItem> OwnedChildren = new List<SmartMenuItem>();
            foreach (SmartMenuItem item in childMenu)
            {
                if (item.Parent == id)
                {
                    OwnedChildren.Add(item);
                }
            }
            return OwnedChildren;
        }

        private string GetOpacity()
        {
            string opacityString = "";
            int opacityInt = this.Opacity;
            if (opacityInt >= 0 && opacityInt <= 100)
                opacityString = opacityInt.ToString();
            else
                opacityString = "100";
            return opacityString;
        }
    }
}
