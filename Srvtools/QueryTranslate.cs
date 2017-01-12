using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Srvtools
{
    public class QueryTranslate
    {
        public static string Translate(object cq)
        {
            ArrayList listcaption = new ArrayList();
            ArrayList listoperator = new ArrayList();
            ArrayList listtext = new ArrayList();
            ArrayList listcondition = new ArrayList();
            if (cq is ClientQuery)
            {
                foreach (QueryColumns qc in (cq as ClientQuery).Columns)
                {
                    listcaption.Add(qc.Caption);
                    listoperator.Add(qc.Operator);
                    listtext.Add(qc.Text);
                    listcondition.Add(qc.Condition);
                }
            }
            else if (cq is WebClientQuery)
            {
                foreach (WebQueryColumns qc in (cq as WebClientQuery).Columns)
                {
                    listcaption.Add(qc.Caption);
                    listoperator.Add(qc.Operator);
                    listtext.Add(qc.Text);
                    listcondition.Add(qc.Condition);
                }
            }
            return BuildString(listcaption, listoperator, listtext, listcondition);
        }

        public static string BuildString(ArrayList listcaption, ArrayList listoperator, ArrayList listtext, ArrayList listcondition)
        {
            string strrtn = "";
            int count = listcaption.Count;
            Hashtable tableopr = new Hashtable();
            Hashtable tablecon = new Hashtable();

            string stropr = SysMsg.GetSystemMessage(CliUtils.fClientLang, "Srvtools", "QueryTranslate", "Operator");
            string[] arropr = stropr.Split(';');
            string[] arroprkey = new string[]{"=", "!=", ">", "<", ">=", "<=", "%", "%%", "in"};
            for (int i = 0; i < arroprkey.Length; i++)
			{
		        tableopr.Add(arroprkey[i], arropr[i]);
			}
            string strcon = SysMsg.GetSystemMessage(CliUtils.fClientLang,"Srvtools", "QueryTranslate", "Condition");
            string[] arrcon = strcon.Split(';');
            string[] arrconkey = new string[]{"And", "Or"};

            string strempty = SysMsg.GetSystemMessage(CliUtils.fClientLang,"Srvtools", "QueryTranslate", "Empty");

            for (int i = 0; i < arrconkey.Length; i++)
			{
                tablecon.Add(arrconkey[i], arrcon[i]);
			}

            for (int i = 0; i < count; i++)
			{
                if (listtext[i].ToString() != "")
                {
                    if (strrtn != "")
                    {
                        strrtn += "," + tablecon[listcondition[i].ToString()].ToString();
                    }
                    if (string.Compare(listtext[i].ToString().Trim(), "null", true) == 0)//IgnoreCase
                    {
                        strrtn += listcaption[i].ToString() + strempty;
                    }
                    else
                    {
                        strrtn += listcaption[i].ToString() + string.Format(tableopr[listoperator[i].ToString()].ToString(),listtext[i].ToString());
                    }
                }
            }
            return strrtn;
        }

    }
}
