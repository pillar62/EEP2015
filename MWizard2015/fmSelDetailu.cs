using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;

namespace MWizard2015
{
    public partial class fmSelDetail : Form
    {
        private TStringList FDetailList;

        public fmSelDetail()
        {
            InitializeComponent();
            FDetailList = new TStringList();
        }

        public bool ShowSelDetail(InfoBindingSource aBindingSource, ref DataRelation Relation)
        {
            Init(aBindingSource, Relation);
            bool Result = base.ShowDialog() == DialogResult.OK;
            if (Result)
            {
                int Index = FDetailList.IndexOf((string)lbDetail.SelectedItem);
                Relation = (DataRelation)FDetailList.Objects(Index);
            }
            return Result;
        }

        private void Init(InfoBindingSource aBindingSource, DataRelation Relation)
        {
            DataRelation R1;
            lbDetail.Items.Clear();
            if (aBindingSource.DataSource.GetType().Equals(typeof(InfoDataSet)))
            {
                InfoDataSet set1 = (InfoDataSet)aBindingSource.DataSource;
                for (int I = 0; I < set1.RealDataSet.Tables[0].ChildRelations.Count; I++)
                {
                    R1 = set1.RealDataSet.Tables[0].ChildRelations[I];
                    lbDetail.Items.Add(R1.ChildTable.TableName);
                    FDetailList.AddObject(R1.ChildTable.TableName, R1);
                }
            }
            if (aBindingSource.DataSource.GetType().Equals(typeof(InfoBindingSource)))
            {
                while (!aBindingSource.DataSource.GetType().Equals(typeof(InfoDataSet)))
                {
                    aBindingSource = (InfoBindingSource)aBindingSource.DataSource;
                }
                InfoDataSet set2 = (InfoDataSet)aBindingSource.DataSource;
                for (int num2 = 0; num2 < set2.RealDataSet.Tables.Count; num2++)
                {
                    if (set2.RealDataSet.Tables[num2].TableName.Equals(Relation.ChildTable.TableName))
                    {
                        for (int num3 = 0; num3 < set2.RealDataSet.Tables[num2].ChildRelations.Count; num3++)
                        {
                            R1 = set2.RealDataSet.Tables[num2].ChildRelations[num3];
                            lbDetail.Items.Add(R1.ChildTable.TableName);
                            FDetailList.AddObject(R1.ChildTable.TableName, R1);
                        }
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lbDetail.SelectedIndex == -1)
                throw new Exception("Please select a child relation !!");
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

    }
}