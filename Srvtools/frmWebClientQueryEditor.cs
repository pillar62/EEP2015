using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web.UI.WebControls;


namespace Srvtools
{
    public partial class frmWebClientQueryEditor : Form
    {
        private WebClientQuery wcqCopy = new WebClientQuery();
        int colNum = 0;
        float fontsize = 9.0f;
        FontStyle fontStyle = FontStyle.Regular;
        
        public frmWebClientQueryEditor(WebClientQuery wcq)
        {
            InitializeComponent();
            wcqCopy = wcq;
            colNum = wcq.Columns.Count;         
            SetFont();
            InitializeQueryConditionItem();


        }

        private void SetFont()
        {
            switch (wcqCopy.Font.Size.Type)
            {
                case FontSize.XXSmall: fontsize = 5.0f; break;
                case FontSize.XSmall: fontsize = 6.0f; break;
                case FontSize.Smaller: fontsize = 7.0f; break;
                case FontSize.Small: fontsize = 8.0f; break;
                case FontSize.Medium: fontsize = 9.0f; break;
                case FontSize.Large: fontsize = 10.0f; break;
                case FontSize.Larger: fontsize = 11.0f; break;
                case FontSize.XLarge: fontsize = 12.0f; break;
                case FontSize.XXLarge: fontsize = 13.0f; break;
                default: fontsize = 9.0f; break;
            }

            if (wcqCopy.Font.Bold)
            {
                fontStyle |= FontStyle.Bold;
            }
            if (wcqCopy.Font.Italic)
            {
                fontStyle |= FontStyle.Italic;
            }
            if (wcqCopy.Font.Strikeout)
            {
                fontStyle |= FontStyle.Strikeout;
            }
            if (wcqCopy.Font.Underline)
            {
                fontStyle |= FontStyle.Underline;
            }
        }

        private System.Windows.Forms.Label[] captionLabels;
        private InfoComboBox[] textcomboBox;
        private InfoTextBox[] textTextBox;
        private InfoRefvalBox[] textRefValBox;
        private InfoDateTimePicker[] textDateTimePicker;
        private System.Windows.Forms.Button[] textButton;
        private System.Windows.Forms.Button btnCancel = new System.Windows.Forms.Button();
        private System.Windows.Forms.Button btnOk = new System.Windows.Forms.Button();

        private void InitializeQueryConditionItem()
        {
            captionLabels = new System.Windows.Forms.Label[colNum];
            textcomboBox = new InfoComboBox[colNum];
            textTextBox = new InfoTextBox[colNum];
            textRefValBox = new InfoRefvalBox[colNum];
            textDateTimePicker = new InfoDateTimePicker[colNum];
            textButton = new System.Windows.Forms.Button[colNum];

            int LocationX = 130;
            int LocationY = 30;
            int formWidth = LocationX;

            for (int i = 0; i < colNum; i++)
            {
               // HorizontalAlignment txtalign = GetAlignment(((QueryColumns)cqFormQuery.Columns[i]).TextAlign);

                //conditionlabels & newline
                if (((WebQueryColumns)wcqCopy.Columns[i]).Column == string.Empty)
                {
                    MessageBox.Show(string.Format("The columnname of column[{0}] is empty", i.ToString()));
                }
                if (i > 0)
                {
                    if (((WebQueryColumns)wcqCopy.Columns[i]).NewLine == true)
                    {
                        LocationY += wcqCopy.GapVertical + captionLabels[i - 1].PreferredHeight + 9;
                        LocationX = 130;
                    }
                    else
                    {
                        LocationX += ((WebQueryColumns)wcqCopy.Columns[i - 1]).Width + wcqCopy.GapHorizontal + 90;
                    }
                }

                //captionlabels
                captionLabels[i] = new System.Windows.Forms.Label();
                captionLabels[i].AutoSize = true;
                captionLabels[i].TextAlign = ContentAlignment.MiddleRight;
                captionLabels[i].Name = "caplbl" + ((WebQueryColumns)wcqCopy.Columns[i]).Column.ToString();
                captionLabels[i].Text = ((WebQueryColumns)wcqCopy.Columns[i]).Caption;
                captionLabels[i].BackColor = System.Drawing.Color.Transparent;
                captionLabels[i].ForeColor = wcqCopy.LabelColor;
                captionLabels[i].Font = new Font(wcqCopy.Font.Name, fontsize, fontStyle);

                captionLabels[i].Location = new System.Drawing.Point(LocationX - captionLabels[i].PreferredWidth, LocationY + 4);

             //   this.splitContainer1.Panel1.Controls.Add(captionLabels[i]);
                this.Controls.Add(captionLabels[i]);

                //textcolumn
                switch (((WebQueryColumns)wcqCopy.Columns[i]).ColumnType)
                {
                    case "ClientQueryComboBoxColumn":
                        {
                            textcomboBox[i] = new InfoComboBox();
                            textcomboBox[i].RightToLeft = RightToLeft.No;
                            textcomboBox[i].Font = new Font(wcqCopy.Font.Name, fontsize, fontStyle);
                            textcomboBox[i].ForeColor = wcqCopy.TextColor;
                            
                           
                            textcomboBox[i].Location = new System.Drawing.Point(LocationX, LocationY);
                            textcomboBox[i].Name = "txtcbb" + ((WebQueryColumns)wcqCopy.Columns[i]).Column.ToString();
                            textcomboBox[i].Size = new System.Drawing.Size(((WebQueryColumns)wcqCopy.Columns[i]).Width, 20);
                            //this.splitContainer1.Panel1.Controls.Add(textcomboBox[i]);
                            this.Controls.Add(textcomboBox[i]);
                            break;
                        }
                    case "ClientQueryTextBoxColumn":
                        {
                            textTextBox[i] = new InfoTextBox();
                            textTextBox[i].Font =  new Font(wcqCopy.Font.Name, fontsize, fontStyle);
                            textTextBox[i].ForeColor = wcqCopy.TextColor;
                          
                            textTextBox[i].Location = new System.Drawing.Point(LocationX, LocationY);
                            textTextBox[i].Name = "txttb" + ((WebQueryColumns)wcqCopy.Columns[i]).Column.ToString();
                            textTextBox[i].Size = new System.Drawing.Size(((WebQueryColumns)wcqCopy.Columns[i]).Width, 20);
                            //textTextBox[i].TextAlign = txtalign;
                            //this.splitContainer1.Panel1.Controls.Add(textTextBox[i]);
                            this.Controls.Add(textTextBox[i]);
                            break;
                        }
                    case "ClientQueryRefValColumn":
                        {
                            textRefValBox[i] = new InfoRefvalBox();
                            textRefValBox[i].Font = new Font(wcqCopy.Font.Name, fontsize, fontStyle);
                            textRefValBox[i].TextBoxForeColor = wcqCopy.TextColor;
                           
                            textRefValBox[i].Location = new System.Drawing.Point(LocationX, LocationY);
                            textRefValBox[i].Name = "txtrvb" + (((WebQueryColumns)wcqCopy.Columns[i]).Column.ToString());
                            textRefValBox[i].Size = new System.Drawing.Size(((WebQueryColumns)wcqCopy.Columns[i]).Width, 20);
                            textRefValBox[i].RefVal = infoRefValQuery;
                          // textRefValBox[i].TextBoxTextAlign = txtalign;
                           // this.splitContainer1.Panel1.Controls.Add(textRefValBox[i]);
                            this.Controls.Add(textRefValBox[i]);
                            break;
                        }
                    case "ClientQueryCalendarColumn":
                        {
                            textDateTimePicker[i] = new InfoDateTimePicker();
                            //textDateTimePicker[i].Font = cqFormQuery.Font;
                            //textDateTimePicker[i].CalendarTrailingForeColor = cqFormQuery.TextColor;
                            textDateTimePicker[i].Location = new System.Drawing.Point(LocationX, LocationY);
                            textDateTimePicker[i].Name = "txtdtp" + ((WebQueryColumns)wcqCopy.Columns[i]).Column.ToString();
                            textDateTimePicker[i].Size = new System.Drawing.Size(((WebQueryColumns)wcqCopy.Columns[i]).Width, 20);
                          

                            //this.splitContainer1.Panel1.Controls.Add(textDateTimePicker[i]);
                            this.Controls.Add(textDateTimePicker[i]);
                            break;
                        }
                    case "ClientQueryRefButtonColumn":
                        {
                            textTextBox[i] = new InfoTextBox();
                            
                            textTextBox[i].Font = new Font(wcqCopy.Font.Name, fontsize, fontStyle);
                            textTextBox[i].ForeColor = wcqCopy.TextColor;

                            textTextBox[i].Location = new System.Drawing.Point(LocationX, LocationY);
                            textTextBox[i].Name = "txttb" + ((WebQueryColumns)wcqCopy.Columns[i]).Column.ToString();
                            textTextBox[i].Size = new System.Drawing.Size(((WebQueryColumns)wcqCopy.Columns[i]).Width, 20);
                            textButton[i] = new System.Windows.Forms.Button();
                            textButton[i].Size = new Size(20, 20);
                            textButton[i].Location = new Point(LocationX + ((WebQueryColumns)wcqCopy.Columns[i]).Width, LocationY);

                            //textTextBox[i].TextAlign = txtalign;
                            //this.splitContainer1.Panel1.Controls.Add(textTextBox[i]);
                            this.Controls.Add(textTextBox[i]);
                            this.Controls.Add(textButton[i]);
                            break;
                        }
                    default: break;
                }
            }




            //SYS_LANGUAGE language = CliSysMegLag.GetClientLanguage();
            SYS_LANGUAGE language = CliUtils.fClientLang;
            string caption = SysMsg.GetSystemMessage(language, "Srvtools", "InfoNavigator", "NavText");

            for (int i = 0; i < 7; i++)
            {
                caption = caption.Substring(caption.IndexOf(";") + 1);
            }


            btnOk.Name = "btnOk";
            btnOk.Text = caption.Substring(0, caption.IndexOf(";"));
            caption = caption.Substring(caption.IndexOf(";") + 1);
            btnOk.Location = new System.Drawing.Point((this.Width / 2 - 95), LocationY + 30);
           
            btnOk.Size = new System.Drawing.Size(75, 26);
            btnCancel.Name = "btnCancel";
            btnCancel.Text = caption.Substring(0, caption.IndexOf(";"));
            btnCancel.Location = new System.Drawing.Point((this.Width / 2 + 20), LocationY + 30);

            btnCancel.Size = new System.Drawing.Size(75, 26);

            //this.splitContainer1.Panel2.Controls.Add(btnOk);
            //this.splitContainer1.Panel2.Controls.Add(btnCancel);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
            







        }
    }
}