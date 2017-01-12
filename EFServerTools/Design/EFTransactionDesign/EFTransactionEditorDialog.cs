using System;
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel.Design;
using EFBase;

namespace EFServerTools.Design.EFTransactionDesign
{
    #region EFTransactionEditorDialog
	public partial class EFTransactionEditorDialog : Form
	{
		IDesignerHost DesignerHost = null;

		EFTransaction transaction = null;
		EFTransaction transPrivateCopy = null;

		SrcTablePanel srcTablePanel = null;
		ArrayList TransTablePanels = null;
		ArrayList LinkPointsList = null;

		public string SrcTableName = "";

		public EFTransactionEditorDialog(EFTransaction trans, IDesignerHost host)
		{
			transaction = trans;
			DesignerHost = host;
            
			InitializeComponent();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void InfoTransactionEditorDialog_Load(object sender, EventArgs e)
		{
			// Copy the editing EFTransaction
			//
			SetInfoTransactionPrivateCopy();

			// Set up UI
			//
			SetUpUI();
		}


		private void SetInfoTransactionPrivateCopy()
		{
            transPrivateCopy = transaction.Copy();
		}

        private void SetUpUI()
        {
            // Set up Timing Selection
            //
            // SetTimingSelection();

            // Get SrcTableName
            //
            if (transPrivateCopy.UpdateComponent != null
             && transPrivateCopy.UpdateComponent.Command != null)
            {
                this.SrcTableName = transPrivateCopy.UpdateComponent.Command.EntitySetName;
            }
            else
            {
                this.SrcTableName = "";
            }

            // Set Transactions view
            //
            this.pnlTableView.Controls.Clear();

            this.srcTablePanel = new SrcTablePanel(this.transPrivateCopy, this.SrcTableName, this);
            srcTablePanel.Location = new Point(30, 10);
            // Event to move scrTablePanel 
            this.srcTablePanel.MouseMove += new MouseEventHandler(scrTablePanel_MouseMove);
            this.srcTablePanel.MouseDown += new MouseEventHandler(scrTablePanel_MouseDown);
            this.srcTablePanel.MouseUp += new MouseEventHandler(scrTablePanel_MouseUp);
            this.pnlTableView.Controls.Add(srcTablePanel);
            this.srcTablePanel.ResizePanel();

            this.TransTablePanels = new ArrayList();

            for (int i = 0; i < this.transPrivateCopy.Transactions.Count; ++i)
            {
                TablePanel tp = new TablePanel(this.transPrivateCopy.Transactions[i], this.DesignerHost, this);

                tp.Location = new Point(this.srcTablePanel.Location.X + this.srcTablePanel.Width + 100 - i * 5,
                 this.srcTablePanel.Location.Y + i * (30 + 10 + 60));

                // Event to Select or Move TransTable
                tp.MouseMove += new MouseEventHandler(tp_MouseMove);
                tp.MouseDown += new MouseEventHandler(tp_MouseDown);
                tp.MouseUp += new MouseEventHandler(tp_MouseUp);

                this.pnlTableView.Controls.Add(tp);
                this.TransTablePanels.Add(tp);
            }
        }

		int TablePanelX, TablePanelY;

        bool panelmove = false;
		void tp_MouseUp(object sender, MouseEventArgs e)
		{
			TablePanel tp = sender as TablePanel;
			if (tp == null)
			{
				return;
			}
            if (panelmove == true)
            {
                if (tp.Location.X + e.X - TablePanelX >= 0
                        && tp.Location.Y + e.Y - TablePanelY >= 0
                    &&
                    (tp.Location.X + e.X - TablePanelX > srcTablePanel.Location.X + srcTablePanel.Width
                    || tp.Location.Y + e.Y - TablePanelY > srcTablePanel.Location.Y + srcTablePanel.Height))
                {
                    tp.Location = new Point(tp.Location.X + e.X - TablePanelX,
                        tp.Location.Y + e.Y - TablePanelY);
                    this.pnlTableView.Refresh();
                }
                panelmove = false;
            }
			tp.Cursor = Cursors.Arrow;
		}

		void tp_MouseDown(object sender, MouseEventArgs e)
		{
			TablePanel tp = sender as TablePanel;

			if (tp == null)
			{
				return;
			}
			if (e.X > 12 && e.X <= tp.Size.Width - 5)
			{
				tp.Cursor = Cursors.SizeAll;
				TablePanelX = e.X;
				TablePanelY = e.Y;
                panelmove = true;
			}
			UnSelectAllTransTablePanels();
			tp.Selected = true;
		}

		private void UnSelectAllTransTablePanels()
		{
			foreach (TablePanel tp in this.TransTablePanels)
			{
				tp.Selected = false;
			}
		}

		void tp_MouseMove(object sender, MouseEventArgs e)
		{
		}

		void tp_Click(object sender, EventArgs e)
		{
			(sender as TablePanel).Selected = true;
		}

		void scrTablePanel_MouseUp(object sender, MouseEventArgs e)
		{
			
		}

		void scrTablePanel_MouseDown(object sender, MouseEventArgs e)
		{
			UnSelectAllTransTablePanels();
		}

		void scrTablePanel_MouseMove(object sender, MouseEventArgs e)
		{
		}

		private void pnlTableView_Click(object sender, EventArgs e)
		{
			try
			{
				UnSelectAllTransTablePanels();
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			Transaction trans = new Transaction();
			int currentTransCount = this.transPrivateCopy.Transactions.Count;

			if (currentTransCount == 0)
			{
				trans.TransStep = 1;
				trans.TransTableName = "TransTableName1";
			}
			else
			{
				trans.TransStep = this.transPrivateCopy.Transactions[currentTransCount - 1].TransStep + 1;			
				trans.TransTableName = "TransTableName" + trans.TransStep;
			}
			this.transPrivateCopy.Transactions.Add(trans);

			TablePanel tp = new TablePanel(trans, this.DesignerHost,this);
			tp.Location = new Point(this.srcTablePanel.Location.X + this.srcTablePanel.Width + 100 - 5 * currentTransCount,
				this.srcTablePanel.Location.Y + currentTransCount * (30 + 10 + 60));

			// Event to Select or Move TransTable
			tp.MouseMove += new MouseEventHandler(tp_MouseMove);
			tp.MouseDown += new MouseEventHandler(tp_MouseDown);
			tp.MouseUp += new MouseEventHandler(tp_MouseUp);

			this.pnlTableView.Controls.Add(tp);
			this.TransTablePanels.Add(tp);
            
            this.srcTablePanel.ResizePanel();
			this.pnlTableView.Refresh();
           
		}

		private void btnDel_Click(object sender, EventArgs e)
		{
			try
			{
				// Remove the selected TransTable
				//
				for (int i = 0; i < this.TransTablePanels.Count; ++i)
				{
					if ((this.TransTablePanels[i] as TablePanel).Selected)
					{
						this.pnlTableView.Controls.Remove(this.TransTablePanels[i] as Control);
						this.TransTablePanels.RemoveAt(i);
						this.transPrivateCopy.Transactions.RemoveAt(i);
                        
                        this.srcTablePanel.ResizePanel();
						this.pnlTableView.Refresh();
                        
						break;
					}
				}
			}
			catch (Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			IComponentChangeService ComponentChangeService = 
				this.DesignerHost.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			
			// Copy from transPrivateCopy to transaction

			object oldValue = null;
			object newValue = null;

			// Transactions
			PropertyDescriptor descTransactions = TypeDescriptor.GetProperties(this.transaction)["Transactions"];
			ComponentChangeService.OnComponentChanging(this.transaction, descTransactions);
			oldValue = transaction.Transactions;
			transaction.Transactions = transPrivateCopy.Transactions;
			newValue = transaction.Transactions;
			ComponentChangeService.OnComponentChanged(this.transaction, descTransactions, oldValue, newValue);

			// UpdateComp
            if (transaction.UpdateComponent != transPrivateCopy.UpdateComponent)
			{
				PropertyDescriptor descUpdateComp = TypeDescriptor.GetProperties(this.transaction)["UpdateComp"];
				ComponentChangeService.OnComponentChanging(this.transaction, descUpdateComp);
                oldValue = transaction.UpdateComponent;
                transaction.UpdateComponent = transPrivateCopy.UpdateComponent;
                newValue = transaction.UpdateComponent;
				ComponentChangeService.OnComponentChanged(this.transaction, descUpdateComp, oldValue, newValue);
			}

			this.Close();
		}

		private void pnlTableView_Paint(object sender, PaintEventArgs e)
		{
			// Set up links
			if (this.LinkPointsList == null)
			{
				this.LinkPointsList = new ArrayList();
			}
			else
			{
				this.LinkPointsList.Clear();
			}

			int count = 2;
			for (int i = 0; i < this.TransTablePanels.Count; ++i)
			{
				TablePanel tp = this.TransTablePanels[i] as TablePanel;
				ArrayList points = new ArrayList();


				Point StartPoint = new Point(this.srcTablePanel.Location.X + this.srcTablePanel.Width,
					this.srcTablePanel.Location.Y + 36 + i * 52 + 13);
				Point EndPoint = new Point(tp.Location.X, tp.Location.Y + 15);

				// Start Point of the link
				points.Add(StartPoint);

				// Points Along the link

				if (EndPoint.X >= srcTablePanel.Location.X 
					&& EndPoint.X <= srcTablePanel.Location.X + srcTablePanel.Width
					&& EndPoint.Y >= srcTablePanel.Location.Y 
					&& EndPoint.Y <= srcTablePanel.Location.Y + srcTablePanel.Height)
				{
				}
				else if (EndPoint.X > StartPoint.X)
				{
					points.Add(new Point((StartPoint.X + EndPoint.X) / 2, StartPoint.Y));
					points.Add(new Point((StartPoint.X + EndPoint.X) / 2, EndPoint.Y));
				}
				else if (EndPoint.X < StartPoint.X
					&& tp.Location.Y > srcTablePanel.Location.Y + srcTablePanel.Height)
				{
					points.Add(new Point(StartPoint.X + count * 5, StartPoint.Y));
					points.Add(new Point(StartPoint.X + count * 5, 
						(srcTablePanel.Location.Y + srcTablePanel.Height + tp.Location.Y) / 2));
					points.Add(new Point(EndPoint.X - 10,
						(srcTablePanel.Location.Y + srcTablePanel.Height + tp.Location.Y) / 2));
					points.Add(new Point(EndPoint.X - 10, EndPoint.Y));

					count++;
				}

				// End Point of the link
				points.Add(EndPoint);
				this.LinkPointsList.Add(points);
			}

			Pen pen = new Pen(Brushes.Gray, 1.0f);
			foreach (ArrayList points in LinkPointsList)
			{
				for (int i = 1; i < points.Count; ++i)
				{
					e.Graphics.DrawLine(pen, (Point)points[i - 1], (Point)points[i]);
				}
			}
		}

		private void pnlTableView_Scroll(object sender, ScrollEventArgs e)
		{
			pnlTableView.Refresh();
		}

        private void cbdetail_CheckedChanged(object sender, EventArgs e)
        {
            pnlTableView.Refresh();
        }
	}
	#endregion InfoTransactionEditorDialog

	#region SrcTablePanel
    [ToolboxItem(false)]
	internal class SrcTablePanel : Panel
	{
		public EFTransaction transaction = null;
		public string SrcTableName = "";
        private Form frminfotransaction = null;
        private int intPannelWidth = 0;


        public SrcTablePanel(EFTransaction trans, string scrTableName, Form frm)
		{
			this.transaction = trans;
			this.SrcTableName = scrTableName;
			this.BackColor = Color.Wheat;
            frminfotransaction = frm;
            intPannelWidth = ResizePanel();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

            //ResizePanel();

			Font font = new Font("Arial", 12.0f);
			Pen pen = new Pen(Brushes.Gray, 1.0f);
            

			e.Graphics.DrawString(this.SrcTableName,font, Brushes.Black, 10 + (this.Size.Width - intPannelWidth) / 2, 10);

			e.Graphics.DrawLine(pen, 0, 0, 0, this.Height - 1);
			e.Graphics.DrawLine(pen, 0, this.Height - 1, this.Width - 1 - 7, this.Height - 1);
			e.Graphics.DrawLine(pen, this.Width - 1 - 7, this.Height - 1, this.Width - 1 - 7, 0);
			e.Graphics.DrawLine(pen, this.Width - 1 - 7, 0, 0, 0);

			e.Graphics.FillRectangle(Brushes.Lavender, this.Width - 7, 0, this.Width - 1, this.Height);
			for (int i = 0; i < this.transaction.Transactions.Count; ++i)
			{
				e.Graphics.DrawLine(pen, 0, 36 + i * 52, this.Width - 1 - 7, 36 + i * 52);
        		e.Graphics.DrawString(this.transaction.Transactions[i].TransStep.ToString("00"), font, Brushes.Black, this.Width/2 - 20, 36 + 52 * i + 5);

                if (((CheckBox)frminfotransaction.Controls["cbdetail"]).Checked == true)
                {
                    if (((Transaction)this.transaction.Transactions[i]).TransKeyFields.Count > 0)
                    {
                        string strTranskf = "";
                        strTranskf = ((TransKeyField)((Transaction)this.transaction.Transactions[i]).TransKeyFields[0]).DesField
                        + " = " + ((TransKeyField)((Transaction)this.transaction.Transactions[i]).TransKeyFields[0]).SrcField;

                        strTranskf += " ";
                        Size TextSize = new Size();
                        do{
                            strTranskf = strTranskf.Substring(0, strTranskf.Length - 1);
                          TextSize = TextRenderer.MeasureText(strTranskf, new Font("Arial", 6.5f));
                        
                        }while(TextSize.Width + 2 > this.Size.Width);

                       
                        e.Graphics.DrawString(strTranskf, new Font("Arial", 6.5f), Brushes.Black, 2, 36 + 52 * i + 5 + 26);
                    }
                }
                e.Graphics.FillPie(Brushes.Gray,  new Rectangle(this.Width - 1 - 14, 36 + 6 + i * 52, 14, 14), 0.0f, 360);
			}
		}

		public int ResizePanel()
		{
			Size TextSize = TextRenderer.MeasureText(this.SrcTableName, new Font("Arial", 12.0f));

			if (this.SrcTableName == "")
			{
				this.Size = new Size(TextSize.Width + 20 + 7 + (40), 36 + 52 * this.transaction.Transactions.Count);
			}
			else
			{
				this.Size = new Size(TextSize.Width + 20 + 7, 36 + 52 * this.transaction.Transactions.Count);
			}
            return this.Size.Width;
		}


        private int X_down = 0;
        private int Y_down = 0;
        private bool mousedown = false; 
        protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left)
			{
			    if (e.X > this.Width - 14 && e.Y > 36)
				{
					// Find the position the mouse
					int i = 0;
					for (i = 0; i < this.transaction.Transactions.Count; ++i)
					{
						if (e.Y - 36 - i * 52 >= 5 && e.Y - 36 - i * 52 <= 14 + 5)
						{
							break;
						}
					}
					if (i < this.transaction.Transactions.Count)
					{
						EFTransactionEditorKeyFieldsDialog itekfd =
                            new EFTransactionEditorKeyFieldsDialog(this.transaction.Transactions[i], this.transaction.UpdateComponent, this.transaction.Transactions[i].TransTableName
                            ,  ((EFTransactionEditorDialog)frminfotransaction).SrcTableName);
						itekfd.ShowDialog();
                        this.Refresh();
					}
				}
                else if (e.X > this.Width - 14 && e.Y < 36)
                {
                    this.Cursor = Cursors.SizeAll;
                    X_down = e.X;
                    Y_down = e.Y;
                    mousedown = true;
                }



			}
		}

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (mousedown == true)
            {
                this.Cursor = Cursors.Arrow;
                if (this.Size.Width + e.X - X_down >= intPannelWidth && this.Size.Width + e.X - X_down <= intPannelWidth + 50)
                {
                    this.Size = new Size(this.Size.Width + e.X - X_down, this.Size.Height);
                }
                else if (this.Size.Width + e.X - X_down < intPannelWidth)
                {
                    this.Size = new Size(intPannelWidth, this.Size.Height);
                }
                else
                {
                    this.Size = new Size(intPannelWidth + 50, this.Size.Height);
                
                }

                mousedown = false;
            }
            this.Refresh();
            this.Parent.Refresh();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (e.X > this.Width - 14 && e.Y < 36)
            {
                this.Size = new Size(intPannelWidth, this.Size.Height);
                mousedown = false;
            }
            this.Refresh();
            this.Parent.Refresh();
        }

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Button != MouseButtons.Left)
			{
                if (e.X > this.Width - 14 && e.Y < 36)
                {
                    this.Cursor = Cursors.VSplit;
                }
                             
                else if (e.X > this.Width - 14 && e.Y > 36)
				{
					int i = 0;
					for (i = 0; i < this.transaction.Transactions.Count; ++i)
					{
						if (e.Y - 36 - i * 52 >= 5 && e.Y - 36 - i * 52 <= 14 + 5)
						{
							break;
						}
					}
					if (i < this.transaction.Transactions.Count)
					{
						this.Cursor = Cursors.Hand;
					}
					else
					{
						this.Cursor = Cursors.Arrow;
					}
				}
				else
				{
					this.Cursor = Cursors.Arrow;
				}
			}
		}
	}
#endregion SrcTablePanel

	#region TablePanel
    [ToolboxItem(false)]
	internal class TablePanel : Panel
	{
		public Transaction transaction;
		public IDesignerHost DesignerHost = null;
        private Form frminfotransaction = null;
        private RichTextBox rtbfield = new RichTextBox();
        private int intPannelWidth = 0;
              
        

		bool selected = false;
		public bool Selected
		{
			get
			{
				return selected;
			}
			set
			{
				if (this.selected == value)
				{
					return;
				}
				this.selected = value;
				this.Refresh();
			}
		}

		public TablePanel(Transaction trans, IDesignerHost host,Form frm)
		{
			this.transaction = trans;
			DesignerHost = host;
            this.BackColor = Color.Pink;
            frminfotransaction = frm;
            this.Controls.Add(rtbfield);
            this.rtbfield.WordWrap = false;
            this.rtbfield.Text = "";
            this.rtbfield.ReadOnly = true;
            intPannelWidth = InitialPanelWidth();

          
            foreach (TransField field in this.transaction.TransFields)
            {
                this.rtbfield.Text += field.DesField + UpdateModeToString(field.UpdateMode) + field.SrcField + "\n";
            }
         
        }

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			ResizePanel();

            Font font = new Font("Arial", 12.0f);
			Pen pen = new Pen(Brushes.Gray, 1.0f);
			Pen selPen = new Pen(Brushes.Blue, 1.0f);
			Pen unSelPen = new Pen(Brushes.Lavender, 1.0f);

			e.Graphics.DrawString(transaction.TransTableName,
				font, Brushes.Black, 12 + 5 + (this.Width - intPannelWidth) / 2, 1 + 5);

			e.Graphics.DrawLine(pen, 12, 1, this.Width - 1 - 1, 1);
			e.Graphics.DrawLine(pen, this.Width - 1 - 1, 1, this.Width - 1 - 1, this.Height - 1 - 1);
			e.Graphics.DrawLine(pen, this.Width - 1 - 1, this.Height - 1 - 1, 12, this.Height - 1 - 1);
			e.Graphics.DrawLine(pen, 12, this.Height - 1 - 1, 12, 1);
           
			if (this.Selected)
			{
				e.Graphics.DrawLine(selPen, 11, 0, this.Width - 1, 0);
				e.Graphics.DrawLine(selPen, this.Width - 1, 0, this.Width - 1, this.Height - 1);
				e.Graphics.DrawLine(selPen, this.Width - 1, this.Height - 1, 11, this.Height - 1);
				e.Graphics.DrawLine(selPen, 11, this.Height - 1, 11, 0);
			}
			else
			{
				e.Graphics.DrawLine(unSelPen, 11, 0, this.Width - 1, 0);
				e.Graphics.DrawLine(unSelPen, this.Width - 1, 0, this.Width - 1, this.Height - 1);
				e.Graphics.DrawLine(unSelPen, this.Width - 1, this.Height - 1, 11, this.Height - 1);
				e.Graphics.DrawLine(unSelPen, 11, this.Height - 1, 11, 0);
			}
            
			e.Graphics.FillRectangle(Brushes.Lavender, 0, 0, 11, this.Height);

			e.Graphics.FillPolygon(Brushes.Gray, new Point[] { new Point(0, 8), new Point(0, 22), new Point(12, 15) });
		}

        private int ResizePanel()
        {
            Size TextSize = TextRenderer.MeasureText(transaction.TransTableName + " ", new Font("Arial", 12.0f));
            if (((CheckBox)frminfotransaction.Controls["cbdetail"]).Checked == false)
            {
                this.Size = new Size(this.Size.Width, TextSize.Height + 10 + 2);
                if (this.rtbfield.Visible == true)
                    this.rtbfield.Visible = false;
            }
            else
            {
                this.Size = new Size(this.Size.Width, TextSize.Height + 10 + 2 + 60);
                this.rtbfield.Location = new Point(13, TextSize.Height + 10 + 2);
                this.rtbfield.Size = new Size(this.Width - 15, 58);
                if (this.rtbfield.Visible == false)
                {
                    this.rtbfield.Visible = true;
                }
            }

            return this.Width;
        }

        private int InitialPanelWidth()
        {
            Size TextSize = TextRenderer.MeasureText(transaction.TransTableName + " ", new Font("Arial", 12.0f));
            this.Size = new Size(12 + TextSize.Width + 10 + 1, TextSize.Height + 10 + 2);
            return this.Width;
        }


        private int X_down = 0;
        private int Y_down = 0;
        private bool mousedown = false; 
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left)
			{
				if (e.X < 12 && e.Y > 8 && e.Y < 22)
				{
					EFTransactionEditorTransFieldsDialog itetfd =
                        new EFTransactionEditorTransFieldsDialog(this.transaction, ((EFTransaction)((IEFProperty)this.transaction).ParentProperty.Component).UpdateComponent,this.transaction.TransTableName
                        ,((EFTransactionEditorDialog)frminfotransaction).SrcTableName);
					itetfd.ShowDialog();

                    this.rtbfield.Text = "";
                    foreach (TransField field in this.transaction.TransFields)                        
                    {
                     this.rtbfield.Text += field.DesField + UpdateModeToString(field.UpdateMode) + field.SrcField +"\n";
                    }
    			}
                else if (e.X > this.Width - 5)
                {
                    this.Cursor = Cursors.SizeAll;
                    X_down = e.X;
                    Y_down = e.Y;
                    mousedown = true;
                }
			}
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (mousedown == true)
            {
                this.Cursor = Cursors.Arrow;
                if (this.Size.Width + e.X - X_down >= intPannelWidth && this.Size.Width + e.X - X_down <= intPannelWidth + 130)
                {
                    this.Size = new Size(this.Size.Width + e.X - X_down, this.Size.Height);
                }
                else if (this.Size.Width + e.X - X_down < intPannelWidth)
                {
                    this.Size = new Size(intPannelWidth, this.Size.Height);
                }
                else
                {
                    this.Size = new Size(intPannelWidth + 130, this.Size.Height);

                }

                mousedown = false;
            }
            this.Refresh();
            this.Parent.Refresh();
        }

        private string UpdateModeToString(UpdateMode um)
        {
            string strvalue = "";
            switch(um)
            {
                case UpdateMode.Decrease: strvalue = " - ";break;
                case UpdateMode.Disable:strvalue = " X ";break;
                case UpdateMode.Increase: strvalue = " + ";break;
                case UpdateMode.Replace: strvalue = " = ";break;
                case UpdateMode.WriteBack: strvalue = " <- ";break;
                default: break;
            }
            return strvalue;
        
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (e.X > this.Width - 5)
            {
                this.Size = new Size(intPannelWidth, this.Size.Height);
                mousedown = false;
            }
            else
            {
                EFTransactionEditorTTransactionDialog itettd =
                  new EFTransactionEditorTTransactionDialog(this.transaction, this.DesignerHost
                  , ((EFTransactionEditorDialog)frminfotransaction).SrcTableName);
                itettd.ShowDialog();
                this.rtbfield.Text = "";
                foreach (TransField field in this.transaction.TransFields)
                {
                    this.rtbfield.Text += field.DesField + UpdateModeToString(field.UpdateMode) + field.SrcField + "\n";
                }
                intPannelWidth = InitialPanelWidth();
                this.Refresh();
            }
        }

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

            if (e.Button != MouseButtons.Left && e.Button != MouseButtons.Right)
			{
                if (e.X > this.Width - 5)
                {
                    this.Cursor = Cursors.VSplit;
                }
                else if (e.X < 12 && e.Y > 8 && e.Y < 22)
				{
					this.Cursor = Cursors.Hand;
				}
				else
				{
					this.Cursor = Cursors.Arrow;
				}
			}
		}
	}
	#endregion TablePanel
}
