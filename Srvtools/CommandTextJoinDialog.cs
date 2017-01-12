using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Data.Odbc;

namespace Srvtools
{
    public partial class CommandTextJoinDialog : Form
    {
        private String _joinCondition;
        private String _joinTableName;
        private List<String> _allColumnList;
        private IDbConnection _conn;

        private String _quotePrefix = "[";
        private String _quoteSuffix = "]";

        private String Quote(String table_or_column)
        {
            if (_conn is SqlConnection)
            {
                if (_quotePrefix == null || _quoteSuffix == null)
                    return table_or_column;
                return _quotePrefix + table_or_column + _quoteSuffix;
            }
            else if (_conn is OdbcConnection)
            {
                return table_or_column;
            }
            else if (_conn is OracleConnection)
            {
                return table_or_column;
            }
            else if (_conn is OleDbConnection)
            {
                if (_quotePrefix == null || _quoteSuffix == null)
                    return table_or_column;
                return _quotePrefix + table_or_column + _quoteSuffix;
            }
            else if (_conn.GetType().Name == "MySqlConnection")
            {
                return table_or_column;
            }
            else if (_conn.GetType().Name == "IfxConnection")
            {
                return table_or_column;
            }
            return _quotePrefix + table_or_column + _quoteSuffix;
        }

        public CommandTextJoinDialog(String joinTableName, List<String> columnList)
        {
            InitializeComponent();

            _allColumnList = columnList;

            ArrayList a = new ArrayList();
            foreach (String c in columnList)
            { a.Add(c); }

            cmbLeft.DataSource = a;
            
            cmbOperator.DataSource = new String[] { "=" , "<>"};
            _joinTableName = joinTableName;
        }

        public CommandTextJoinDialog(String joinTableName, List<String> columnList, IDbConnection conn)
        {
            InitializeComponent();

            _allColumnList = columnList;

            ArrayList a = new ArrayList();
            foreach (String c in columnList)
            { a.Add(c); }

            cmbLeft.DataSource = a;

            cmbOperator.DataSource = new String[] { "=", "<>" };
            _joinTableName = joinTableName;

            _conn = conn;
        }

        public String JoinCondition
        { get { return _joinCondition; } }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbLeft.SelectedItem == null)
            { MessageBox.Show(""); return; }

            if (cmbRight.SelectedItem == null)
            { MessageBox.Show(""); return; }

            if (cmbOperator.SelectedItem == null)
            { MessageBox.Show(""); return; }


            _joinCondition = " " + GetJoinType() + " " + _joinTableName + " on " + cmbLeft.SelectedItem.ToString()
                + cmbOperator.SelectedItem.ToString() + cmbRight.SelectedItem.ToString();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _joinCondition = "";
            _joinTableName = "";
            this.Close();
        }

        private String GetJoinType()
        {
            if(rbdInner.Checked)
            {
                if (_conn is SqlConnection)
                    return "inner join";
                else if (_conn is OdbcConnection)
                    return "inner join";
                else if (_conn is OracleConnection)
                    return "join";
                else if (_conn is OleDbConnection)
                    return "inner join";
                else if (_conn.GetType().Name == "MySqlConnection")
                    return "inner join";
                else if (_conn.GetType().Name == "IfxConnection")
                    return "inner join";
                else
                    return "inner join";
            }
            else if(rbdLeft.Checked)
            {return "left join";}
            else if(rbdRight.Checked)
            {return "right join";}
            else if(rbdCross.Checked)
            {return "cross join";}
            else if (rbdFull.Checked)
            { return "full join"; }
            else
            { return "inner join"; }
        }

        private void rbdInner_Click(object sender, EventArgs e)
        {
            rbdInner.Checked = true;
            rbdLeft.Checked = false;
            rbdRight.Checked = false;
            rbdFull.Checked = false;
            rbdCross.Checked = false;
        }

        private void rbdLeft_Click(object sender, EventArgs e)
        {
            rbdInner.Checked = false;
            rbdLeft.Checked = true;
            rbdRight.Checked = false;
            rbdFull.Checked = false;
            rbdCross.Checked = false;
        }

        private void rbdRight_Click(object sender, EventArgs e)
        {
            rbdInner.Checked = false;
            rbdLeft.Checked = false;
            rbdRight.Checked = true;
            rbdFull.Checked = false;
            rbdCross.Checked = false;
        }

        private void rbdFull_Click(object sender, EventArgs e)
        {
            rbdInner.Checked = false;
            rbdLeft.Checked = false;
            rbdRight.Checked = false;
            rbdFull.Checked = true;
            rbdCross.Checked = false;
        }

        private void rbdCross_Click(object sender, EventArgs e)
        {
            rbdInner.Checked = false;
            rbdLeft.Checked = false;
            rbdRight.Checked = false;
            rbdFull.Checked = false;
            rbdCross.Checked = true;
        }

        private void cmbLeft_SelectedIndexChanged(object sender, EventArgs e)
        {
            Object o = cmbLeft.SelectedItem;
            if (o != null)
            {
                String s = o.ToString();
                if (s != null && s.Length != 0)
                {
                    String[] ss = s.Split(".".ToCharArray());
                    //String leftTableName = ss[0]; 为了让有owner的表也能找到Leftjoin的表名 by Rei
                    String leftTableName = ss[ss.Length - 2];

                    List<String> rightColumnsList = new List<string>();
                    foreach (String c in _allColumnList)
                    {
                        String[] cs = c.Split(".".ToCharArray());
                        //cs[0] 为了让有owner的表也能找到Leftjoin的表名 by Rei
                        if (string.Compare(cs[cs.Length - 2], leftTableName, true) == 0)//IgnoreCase
                        { continue; }
                        else
                        { rightColumnsList.Add(c); }
                    }

                    cmbRight.DataSource = rightColumnsList;
                }
            }
        }
    }
}