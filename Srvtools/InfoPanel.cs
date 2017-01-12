using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;

namespace Srvtools
{
    [ToolboxBitmap(typeof(InfoPanel), "Resources.InfoHyperLink.ico")]
    public class InfoPanel : Panel,ISupportInitialize,IGetValues
    {
        System.ComponentModel.ComponentResourceManager resources;
        public InfoPanel()
        {
            resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoPanel));
            orientation = "asLeft";
            minsize = 40;
            this.MaxSize = 200;
            autohide = true;
            this.HideTime = 0;
            this.CloseButton = true;
            this.AutoHideButton = true;
            this.hide = false;
           
        }

        private int minsize;
        [Category("Infolight"),
        Description("Specifies the size when panel is hiding")]
        public int MinSize
        {
            get
            {
                return minsize;
            }
            set
            {
                minsize = value;
                if (value > this.MaxSize)
                {
                    MessageBox.Show("MinSize can not larger than MaxSize");
                    return;
                }
                if (this.DesignHide == true || this.Site == null)
                {
                    if (this.Orientation.Equals("asLeft") || this.Orientation.Equals("asRight"))
                    {
                        this.Width = minsize;
                    }
                    else
                    {
                        this.Height = minsize;
                    }
                }
            }
        }

        private int maxsize;
        [Category("Infolight"),
        Description("Specifies the size when panel is displaying")]
        public int MaxSize
        {
            get
            {
                return maxsize;
            }
            set
            {
                if (value < this.MinSize)
                {
                    MessageBox.Show("MaxSize can not smaller than MinSize");
                    return;
                }
                maxsize = value;
                if (this.DesignHide == false || this.Site == null)
                {
                    if (this.Orientation.Equals("asLeft") || this.Orientation.Equals("asRight"))
                    {
                        this.Width = maxsize;
                    }
                    else
                    {
                        this.Height = maxsize;
                    }
                }
            }
        }

        private string orientation;
        [Category("Infolight"),
        Description("Orientation of panel hiding")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string Orientation
        {
            get
            {
                return orientation;
            }
            set
            {
                orientation = value;
                if (value.Equals("asLeft") || value.Equals("asRight"))
                {
                    this.Width = this.MaxSize;
                }
                else
                {
                    this.Height = this.MaxSize;
                }
            }
        }

        private bool closebutton;
        [Category("Infolight"),
        Description("Indicates whether display the button of close on panel")]
        public bool CloseButton
        {
            get
            {
                return closebutton;
            }
            set
            {
                closebutton = value;
                if (value)
                {
                    if (this.Controls.ContainsKey("btnClose"))
                    {
                        this.Controls.RemoveByKey("btnClose");
                    }
                    Button btnClose = new Button();
                    btnClose.Name = "btnClose";
                    btnClose.Size = new Size(20, 20);
                    //btnClose.Text = "X";
                    btnClose.ImageAlign = ContentAlignment.MiddleCenter;
                    btnClose.Image = (Image)resources.GetObject("InfoPanelClose");
                    btnClose.Click += delegate(object sender, EventArgs e)
                    {
                        this.Visible = false;
                    };

                    //btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                    btnClose.Location = new Point(this.Width - 20, 0);
                    if (this.AutoHideButton)
                    {
                        this.Controls["btnAutoHide"].Location = new Point(this.Width - 40, 0);
                    }
                    this.Controls.Add(btnClose);
                }
                else
                {
                    if (this.Controls.ContainsKey("btnClose"))
                    {
                        this.Controls.RemoveByKey("btnClose");
                        if (this.AutoHideButton)
                        {
                            this.Controls["btnAutoHide"].Location = new Point(this.Width - 20, 0);
                        }
                    }
                }
            }
        }

        private bool autohidebutton;
        [Category("Infolight"),
        Description("Indicates whether display the button of auto hide on panel")]
        public bool AutoHideButton
        {
            get
            {
                return autohidebutton;
            }
            set
            {
                autohidebutton = value;
                if (value)
                {
                    if (this.Controls.ContainsKey("btnAutoHide"))
                    {
                        this.Controls.RemoveByKey("btnAutoHide");
                    }
                    Button btnAutoHide = new Button();
                    btnAutoHide.Name = "btnAutoHide";
                    btnAutoHide.Size = new Size(20, 20);
                    //btnAutoHide.Text = "H";
                    btnAutoHide.ImageAlign = ContentAlignment.MiddleCenter;
                    if (this.AutoHide)
                    {
                        //btnAutoHide.FlatStyle = FlatStyle.Popup;
                        btnAutoHide.Image = (Image)resources.GetObject("InfoPanelAutoHide");
                    }
                    else
                    {
                        //btnAutoHide.FlatStyle = FlatStyle.Standard;
                        btnAutoHide.Image = (Image)resources.GetObject("InfoPanelFixed");
                    }

                    btnAutoHide.Click += delegate(object sender,EventArgs e)
                    {
                        if (this.AutoHide)
                        {
                            this.AutoHide = false;
                            //((Button)sender).FlatStyle = FlatStyle.Standard;
                            btnAutoHide.Image = (Image)resources.GetObject("InfoPanelFixed");
                        }
                        else
                        {
                            this.AutoHide = true;
                            //((Button)sender).FlatStyle = FlatStyle.Popup;
                            btnAutoHide.Image = (Image)resources.GetObject("InfoPanelAutoHide");
                        }
                    };
                    //btnAutoHide.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                    if (this.CloseButton)
                    {
                        btnAutoHide.Location = new Point(this.Width - 40, 0);
                    }
                    else
                    {
                        btnAutoHide.Location = new Point(this.Width - 20, 0);
                    }
                    this.Controls.Add(btnAutoHide);
                }
                else
                {
                    if (this.Controls.ContainsKey("btnAutoHide"))
                    {
                        this.Controls.RemoveByKey("btnAutoHide");
                    }
                }
            }        
        }

        private bool autohide;
        [Category("Infolight"),
        Description("Indicates whether panel hides automatically")]
        public bool AutoHide
        {
            get
            {
                return autohide;
            }
            set
            {
                autohide = value;
                if (!value)
                {
                    this.Display(0);
                    if (this.timedisplay != null)
                    {
                        this.timedisplay.Enabled = false;
                    }
                    if (this.timehide != null)
                    {
                        this.timehide.Enabled = false;
                    }
                }
                if (this.AutoHideButton)
                {
                    if (value)
                    {
                        //((Button)this.Controls["btnAutoHide"]).FlatStyle = FlatStyle.Popup;
                        ((Button)this.Controls["btnAutoHide"]).Image = (Image)resources.GetObject("InfoPanelAutoHide");

                    }
                    else
                    {
                        //((Button)this.Controls["btnAutoHide"]).FlatStyle = FlatStyle.Standard;
                        ((Button)this.Controls["btnAutoHide"]).Image = (Image)resources.GetObject("InfoPanelFixed");
                    }
                }
            }
        }

        private int hidetime;
        [Category("Infolight"),
        Description("Specifies the time of panel hide")]
        public int HideTime
        {
            get
            {
                return hidetime;
            }
            set
            {
                hidetime = value;
            }
        }

       // private bool hideflag = false;
        private bool hide;
        [Category("Infolight"),
        Description("Indicates whether panel hides or display in design time")]
        public bool DesignHide
        {
            get
            {
                return hide;
            }
            set
            {
                hide = value;
                //hideflag = true;
                if (hide)
                {
                    this.FastHide();
                }
                else
                {
                    this.FastDisplay();
                }
               // hideflag = false;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {

            if (this.Site != null && hide == false)
            {
                if (this.Orientation.Equals("asLeft") || this.Orientation.Equals("asRight"))
                {
                    if (this.Width < this.MinSize)
                    {
                        MessageBox.Show("MaxSize can not be smaller than MinSize");
                        this.Width = this.MaxSize;
                    }
                    else
                    {
                        this.MaxSize = this.Width;
                    }
                }
                else
                {
                    if (this.Height < this.MinSize)
                    {
                        MessageBox.Show(("MaxSize can not be smaller than MinSize"));
                        this.Height = this.MaxSize;
                    }
                    else
                    {
                        this.MaxSize = this.Height;
                    }
                }
            }
            else if(this.Site != null && hide == true)
            {
                if (this.Orientation.Equals("asLeft") || this.Orientation.Equals("asRight"))
                {
                    if (this.Width > this.MaxSize)
                    {
                        MessageBox.Show("MinSize can not be lager than MaxSize");
                        this.Width = this.MinSize;
                    }
                    else
                    {
                        this.MinSize = this.Width;
                    }
                }
                else
                {
                    if (this.Height > this.MaxSize)
                    {
                        MessageBox.Show("MinSize can not be lager than MaxSize");
                        this.Height = this.MinSize;
                    }
                    else
                    {
                        this.MinSize = this.Height;
                    }
                }
            }
            
            if (this.CloseButton && this.Controls.ContainsKey("btnClose"))
            {
                this.Controls["btnClose"].Location = new Point(this.Width - 20, 0);
                if (this.AutoHideButton && this.Controls.ContainsKey("btnAutoHide"))
                {
                    this.Controls["btnAutoHide"].Location = new Point(this.Width - 40, 0);
                }
            }
            else
            {
                if (this.AutoHideButton && this.Controls.ContainsKey("btnAutoHide"))
                {
                    this.Controls["btnAutoHide"].Location = new Point(this.Width - 20, 0);
                }
            }
        }

        private bool _ControlByUser = false;
        [Category("Infolight"),
        Description("Indicates whether panel is Controled by User or not")]
        public bool ControlByUser
        {
            get
            {
                return _ControlByUser;
            }
            set
            {
                _ControlByUser = value;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (this.AutoHide && !ControlByUser)
            {
                this.timehide.Enabled = false;
                this.Display(this.HideTime);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {

            if (this.AutoHide && !ControlByUser)
            {
                this.timedisplay.Enabled = false;
                this.Hide(this.HideTime);         
            }
        }

        public bool Finish(string sKind)
        {
            if (string.Compare(sKind, "hide", true) == 0)//IgnoreCase
            {
                if (this.Orientation.Equals("asLeft") || this.Orientation.Equals("asRight"))
                {
                    if (this.Width <= this.MinSize)
                    {
                        return true;
                    }
                }
                else
                {
                    if (this.Height <= this.MinSize)
                    {
                        return true;
                    }
                }
            }
            else if (string.Compare(sKind, "display", true) == 0)//IgnoreCase
            {
                if (this.Orientation.Equals("asLeft") || this.Orientation.Equals("asRight"))
                {
                    if (this.Width >= this.MaxSize)
                    {
                        return true;
                    }
                }
                else
                {
                    if (this.Height >= this.MaxSize)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        Timer timehide;
        public void Hide(int timems)
        {
            timehide = new Timer();
            int distance = 0;
            if(this.Orientation.Equals("asLeft") || this.Orientation.Equals("asRight"))
            {
                distance = this.MaxSize - this.MinSize;
            }
            else
            {
                distance = this.MaxSize - this.MinSize;
            }
            if(distance > 0)
            {
                //int timeperpoint =  timems / distance;
                if (timems > 0)
                {
                    #region slowhide
                    timehide.Interval = timems;
                    timehide.Tick += delegate(object sender, EventArgs e)
                    {
                        if (Finish("hide"))
                        {
                            timehide.Enabled = false;
                            return;
                        }
                        this.SlowHide();
                    };
                    timehide.Enabled = true;
                    //while (distance > 0) ;
                    //timehide.Enabled = false;
                    #endregion
                }
                else
                {
                    timehide.Interval = 50;
                    timehide.Tick += delegate(object sender, EventArgs e)
                    {
                        timehide.Enabled = false;
                        this.FastHide();
                    };
                    timehide.Enabled = true;
                }
            }
        }

        private void SlowHide()
        {
            int value;
            switch (this.Orientation)
            {
                case "asLeft":
                    {
                        value = this.Width - 10;
                        if (value < this.MinSize)
                        {
                            this.Width = this.MinSize;
                        }
                        else
                        {
                            this.Width = value;
                        }
                        break;
                    }
                case "asRight":
                    {
                        value = this.Width - 10;
                        if (value < this.MinSize)
                        {
                            this.Width = this.MinSize;
                            this.Location = new Point(this.Location.X + 10 - this.MinSize + value, this.Location.Y);
                        }
                        else
                        {
                            this.Width = value;
                            this.Location = new Point(this.Location.X + 10, this.Location.Y);
                        }
                        
                        break;
                    }
                case "asTop":
                    {
                        value = this.Height - 10;
                        if (value < this.MinSize)
                        {
                            this.Height = this.MinSize;
                        }
                        else
                        {
                            this.Height = value;
                        }
                        break;
                    }
                case "asBottom":
                    {
                        value = this.Height - 10;
                        if (value < this.MinSize)
                        {
                            this.Height = this.MinSize;
                            this.Location = new Point(this.Location.X, this.Location.Y + 10 - this.MinSize + value);
                        }
                        else
                        {
                            this.Height = value;
                            this.Location = new Point(this.Location.X, this.Location.Y + 10);
                        }
                        
                        break;
                    }
            }
        }

        private void FastHide()
        {
            switch (this.Orientation)
            {
                case "asLeft":
                    {
                        this.Width = this.MinSize;
                        break;
                    }
                case "asRight":
                    {              
                        this.Location = new Point(this.Location.X + this.Width - this.MinSize, this.Location.Y);
                        this.Width = this.MinSize;
                        break;
                    }
                case "asTop":
                    {
                        this.Height = this.MinSize;
                        break;
                    }
                case "asBottom":
                    {   
                        this.Location = new Point(this.Location.X, this.Location.Y + this.Height - this.MinSize);
                        this.Height = this.MinSize;
                        break;
                    }
            }
        }

        Timer timedisplay;
        public void Display(int timems)
        {
            timedisplay = new Timer();
            int distance = 0;
            if (this.Orientation.Equals("asLeft") || this.Orientation.Equals("asRight"))
            {
                distance = this.MaxSize - this.MinSize;
            }
            else
            {
                distance = this.MaxSize - this.MinSize;
            }
            if (distance > 0)
            {
                //int timeperpoint = timems / distance;
                if (timems > 0)
                {
                    #region slowdisplay
                    timedisplay.Interval = timems;
                    timedisplay.Tick += delegate(object sender, EventArgs e)
                    {
                        if (Finish("display"))
                        {
                            timedisplay.Enabled = false;
                            return;
                        }
                        this.SlowDisplay();
                    };
                    timedisplay.Enabled = true;
                    //while (distance > 0) ;
                    //timehide.Enabled = false;
                    #endregion
                }
                else
                {
                    this.FastDisplay();
                }
            }
        }

        private void SlowDisplay()
        {
            int value;
            switch (this.Orientation)
            {
                case "asLeft":
                    {
                        value = this.Width + 10;
                        if (value > this.MaxSize)
                        {
                            this.Width = this.MaxSize;

                        }
                        else
                        {
                            this.Width = value;
                        }
                        break;
                    }
                case "asRight":
                    {
                        value = this.Width + 10;
                        if (value > this.MaxSize)
                        {
                            this.Width = this.MaxSize;
                            this.Location = new Point(this.Location.X - 10 - this.MaxSize + value, this.Location.Y);

                        }
                        else
                        {
                            this.Width = value;
                            this.Location = new Point(this.Location.X - 10, this.Location.Y);
                        }
                      
                        break;
                    }
                case "asTop":
                    {
                        value = this.Height + 10;
                        if (value > this.MaxSize)
                        {
                            this.Height = this.MaxSize;
                        }
                        else
                        {
                            this.Height = value;
                        }
                        break;
                    }
                case "asBottom":
                    {
                        value = this.Height + 10;
                        if (value > this.MaxSize)
                        {
                            this.Height = this.MaxSize;
                            this.Location = new Point(this.Location.X, this.Location.Y - 10 - this.MaxSize + value);
                        }
                        else
                        {
                            this.Height = value;
                            this.Location = new Point(this.Location.X, this.Location.Y - 10);
                           
                        }
                        break;
                    }
            }
        }

        private void FastDisplay()
        {
            switch (this.Orientation)
            {
                case "asLeft":
                    {
                        this.Width = this.MaxSize;
                        break;
                    }
                case "asRight":
                    {
   
                        this.Location = new Point(this.Location.X - this.MaxSize + this.Width, this.Location.Y);
                        this.Width = this.MaxSize;
                        break;
                    }
                case "asTop":
                    {
                        this.Height = this.MaxSize;
                        break;
                    }
                case "asBottom":
                    {
                        this.Location = new Point(this.Location.X, this.Location.Y - this.MaxSize + this.Height);
                        this.Height = this.MaxSize; 
                        break;
                    }
            }
        }

        #region ISupportInitialize Members

        public void BeginInit()
        {
           
        }

        public void EndInit()
        {
            //if (this.AutoHide && this.AutoHideButton)
            //{
            //    //((Button)this.Controls["btnAutoHide"]).FlatStyle = FlatStyle.Popup;
            //}
            //else if(!this.AutoHideButton)
            //{
            //    //((Button)this.Controls["btnAutoHide"]).FlatStyle = FlatStyle.Standard;
            //}
         //   this.OriginalSize = this.Size;
            if (this.Site == null )
            {
                if (this.AutoHide)
                {
                    this.FastHide();
                }
                else
                {
                    this.FastDisplay();
                }
                this.timedisplay = new Timer();
                this.timehide = new Timer();
                foreach (Control ct in this.Controls)
                {
                    //if (ct.Name != "btnClose" && ct.Name != "btnAutoHide")
                    //{
                        ct.MouseEnter += delegate(object sender, EventArgs e)
                        {
                            this.OnMouseEnter(e);
                        };
                        ct.MouseLeave += delegate(object sender, EventArgs e)
                        {
                            this.OnMouseLeave(e);
                        };
                    //}
                
                }
                this.ControlAdded +=delegate( object sender, ControlEventArgs e)
                {
                    e.Control.MouseEnter += delegate(object sender2, EventArgs e2)
                    {
                        this.OnMouseEnter(e2);
                    };
                    e.Control.MouseLeave += delegate(object sender2, EventArgs e2)
                    {
                        this.OnMouseLeave(e2);
                    };
                };
            }
        }

        #endregion

        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this is InfoPanel)
            {
                if (string.Compare(sKind, "orientation", true) == 0)//IgnoreCase
                {
                    values.Add("asLeft");
                    values.Add("asRight");
                    values.Add("asTop");
                    values.Add("asBottom");
                }
            }
            if (values.Count > 0)
            {
                int i = values.Count;
                retList = new string[i];
                for (int j = 0; j < i; j++)
                {
                    retList[j] = values[j];
                }
            }
            return retList;

        }

        #endregion
    }
}
