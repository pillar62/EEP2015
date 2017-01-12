using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
using System.ComponentModel;

/// <summary>
/// Summary description for JqDataObject2
/// </summary>
public class JqDataObject
{
    public JqDataObject(string data)
    {
        Data = data;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public string Data { get; set; }

    public string RemoteName { get; set; }

    public string TableName { get; set; }

    public string AssemblyName 
    { 
        get 
        {
            if (string.IsNullOrEmpty(Data))
            {
                throw new ArgumentNullException("RemoteName is empty.");
            }
            return Data.Split('.')[0];
        } 
    }

    public bool IsMaster
    {
        get
        {
            return TableName == CommandName;
        }
    }

    public string CommandName
    {
        get
        {
            if (string.IsNullOrEmpty(Data))
            {
                throw new ArgumentNullException("RemoteName is empty.");
            }
            var remoteNames = Data.Split('.');
            if (remoteNames.Length == 1)
            {
                throw new ArgumentException("RemoteName is invalid.");
            }
            return remoteNames[1];
        } 
    }

    /// <summary>
    /// 呼叫server method
    /// </summary>
    public object CallMethod(string methodName, List<object> parameters)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        return EFClientTools.ClientUtility.Client.CallServerMethod(clientInfo, AssemblyName, methodName, parameters);
    }

    /// <summary>
    /// 取主档数据
    /// </summary>
    public DataTable GetDataTable(string whereString, string sortfield, string sortorder, int startIndex, int count)
    {
        return GetDataTable(whereString, sortfield,sortorder,"", null, startIndex, count);
    }

    /// <summary>
    /// 取从档数据
    /// </summary>
    public DataTable GetDataTable(string whereString, string sortfield, string sortorder, string parentTableName, Dictionary<string, object> parentRow, int startIndex, int count)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        EFClientTools.ClientUtility.RemoteName = RemoteName;
        var packetInfo = new EFClientTools.EFServerReference.PacketInfo()
        {
            OrderParameters = new List<EFClientTools.EFServerReference.OrderParameter>(),
            WhereParameters = new List<EFClientTools.EFServerReference.WhereParameter>(),
        };
        if (sortfield != null && sortfield != "" && IsMaster)
        {
            EFClientTools.EFServerReference.OrderParameter OrderParameter = new EFClientTools.EFServerReference.OrderParameter();
            OrderParameter.Field = sortfield;
            if (sortorder.ToLower() == "asc")
                OrderParameter.Direction = EFClientTools.EFServerReference.OrderDirection.Ascending;
            else
                OrderParameter.Direction = EFClientTools.EFServerReference.OrderDirection.Descending;
            packetInfo.OrderParameters.Add(OrderParameter);
        }
        var commandName = CommandName;
        if (IsMaster)
        {
            packetInfo.StartIndex = startIndex;
            packetInfo.Count = count;
            packetInfo.WhereString = whereString;
        }
        else
        {
            if (parentRow == null)
            {
                packetInfo.OnlySchema = true;
            }
            else
            {
                packetInfo.StartIndex = 0;
                packetInfo.Count = -1;  //detail packetrecord无效
                commandName = parentTableName;
                foreach (var item in parentRow)
                {
                    packetInfo.WhereParameters.Add(new EFClientTools.EFServerReference.WhereParameter() { And = true, Field = item.Key, Value = item.Value });
                }
            }
        }

        var xml = (string)EFClientTools.ClientUtility.Client.GetDataSet(clientInfo, AssemblyName, commandName, packetInfo);
        StringReader reader = new StringReader(xml);
        var dataSet = new DataSet();
        dataSet.ReadXml(reader, XmlReadMode.Auto);
        if (dataSet.Tables[TableName] == null)
        {
            throw new Exception(string.Format("Can not find table:'{0}'.(RemoteName:'{1}')", TableName, Data));
        }
        if (!IsMaster)
        {
            DataView view = new DataView(dataSet.Tables[TableName]);
            if (!string.IsNullOrEmpty(sortfield))
            {
                view.Sort = string.Format("{0} {1}", sortfield, sortorder);
            }
            var table = view.ToTable(TableName);
            if (dataSet.Tables[TableName].PrimaryKey.Length > 0)
            {
                var primaryKey = new DataColumn[dataSet.Tables[TableName].PrimaryKey.Length];
                for (int i = 0; i < dataSet.Tables[TableName].PrimaryKey.Length; i++)
                {
                    primaryKey[i] = table.Columns[dataSet.Tables[TableName].PrimaryKey[i].ColumnName];
                }
                table.PrimaryKey = primaryKey;
            }
            return table;
        }
        else
        {
            return dataSet.Tables[TableName];
        }
    }

    /// <summary>
    /// 取得主从档多个DataTable数据的DataSet
    /// </summary>
    /// <returns></returns>
    public DataSet GetDataSetAll(string whereString, string sortfield, string sortorder, int startIndex, int count)
    {
        return GetDataSetAll(whereString, sortfield, sortorder,"", null, startIndex, count);
    }

    /// <summary>
    /// 取得主从档多个DataTable数据的DataSet
    /// </summary>
    /// <returns></returns>
    public DataSet GetDataSetAll(string whereString, string sortfield, string sortorder, string parentTableName, Dictionary<string, object> parentRow, int startIndex, int count)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        EFClientTools.ClientUtility.RemoteName = RemoteName;
        var packetInfo = new EFClientTools.EFServerReference.PacketInfo()
        {
            OrderParameters = new List<EFClientTools.EFServerReference.OrderParameter>(),
            WhereParameters = new List<EFClientTools.EFServerReference.WhereParameter>(),
        };
        if (sortfield != null && sortfield != "" && IsMaster)
        {
            EFClientTools.EFServerReference.OrderParameter OrderParameter = new EFClientTools.EFServerReference.OrderParameter();
            OrderParameter.Field = sortfield;
            if (sortorder.ToLower() == "asc")
                OrderParameter.Direction = EFClientTools.EFServerReference.OrderDirection.Ascending;
            else
                OrderParameter.Direction = EFClientTools.EFServerReference.OrderDirection.Descending;
            packetInfo.OrderParameters.Add(OrderParameter);
        }
        var commandName = CommandName;
        if (IsMaster)
        {
            packetInfo.StartIndex = startIndex;
            packetInfo.Count = count;
            packetInfo.WhereString = whereString;
        }
        else
        {
            if (parentRow == null)
            {
                packetInfo.OnlySchema = true;
            }
            else
            {
                packetInfo.StartIndex = 0;
                packetInfo.Count = -1;  //detail packetrecord无效
                commandName = parentTableName;
                foreach (var item in parentRow)
                {
                    packetInfo.WhereParameters.Add(new EFClientTools.EFServerReference.WhereParameter() { And = true, Field = item.Key, Value = item.Value });
                }
            }
        }

        var xml = (string)EFClientTools.ClientUtility.Client.GetDataSet(clientInfo, AssemblyName, commandName, packetInfo);
        StringReader reader = new StringReader(xml);
        var dataSet = new DataSet();
        dataSet.ReadXml(reader, XmlReadMode.Auto);
        return dataSet;
    }

    public string GetTableName()
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        var commandName = CommandName;
        EFClientTools.ClientUtility.RemoteName = RemoteName;
        return EFClientTools.ClientUtility.Client.GetObjectClassName(clientInfo, AssemblyName, commandName, string.Empty);
    }

    public DataTable GetTotalTable(string whereString, Dictionary<string, string> totals)
    {
        return GetTotalTable(whereString, "", null, totals);
    }

    public DataTable GetTotalTable(string whereString, string parentTableName, Dictionary<string, object> parentRow, Dictionary<string, string> totals)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        var packetInfo = new EFClientTools.EFServerReference.PacketInfo()
        {
            OrderParameters = new List<EFClientTools.EFServerReference.OrderParameter>(),
            WhereParameters = new List<EFClientTools.EFServerReference.WhereParameter>(),
        };
        var commandName = CommandName;
        if (IsMaster)
        {
            packetInfo.StartIndex = 0;
            packetInfo.Count = -1;
            packetInfo.WhereString = whereString;
        }
        else
        {
            if (parentRow == null)
            {
                packetInfo.OnlySchema = true;
            }
            else
            {
                packetInfo.StartIndex = 0;
                packetInfo.Count = -1;  //detail packetrecord无效
                commandName = parentTableName;
                foreach (var item in parentRow)
                {
                    packetInfo.WhereParameters.Add(new EFClientTools.EFServerReference.WhereParameter() { And = true, Field = item.Key, Value = item.Value });
                }
            }
        }

        var xml = (string)EFClientTools.ClientUtility.Client.GetDataTotal(clientInfo, AssemblyName, TableName, packetInfo, totals);
        StringReader reader = new StringReader(xml);
        var dataSet = new DataSet();
        dataSet.ReadXml(reader, XmlReadMode.Auto);
        return dataSet.Tables[TableName];
    }


    /// <summary>
    /// 取结构和更新数据
    /// </summary>
    public DataSet GetDataSet(Dictionary<string, object> keyValues)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        var packetInfo = new EFClientTools.EFServerReference.PacketInfo()
        {
            OrderParameters = new List<EFClientTools.EFServerReference.OrderParameter>(),
            WhereParameters = new List<EFClientTools.EFServerReference.WhereParameter>(),
        };
        if (keyValues == null)
        {
            packetInfo.OnlySchema = true;
        }
        else
        {
            foreach (var item in keyValues)
            {
                packetInfo.WhereParameters.Add(new EFClientTools.EFServerReference.WhereParameter() { And = true, Field = item.Key, Value = item.Value });
            }
        }

        var xml = (string)EFClientTools.ClientUtility.Client.GetDataSet(clientInfo, AssemblyName, TableName, packetInfo);
        StringReader reader = new StringReader(xml);
        var dataSet = new DataSet();
        dataSet.ReadXml(reader, XmlReadMode.Auto);
        return dataSet;
    }

    /// <summary>
    /// 取Detail结构和更新数据
    /// </summary>
    //public DataSet GetDetailDataSet(Dictionary<string, object> keyValues)
    //{
    //    var clientInfo = EFClientTools.ClientUtility.ClientInfo;
    //    var packetInfo = new EFClientTools.EFServerReference.PacketInfo()
    //    {
    //        OrderParameters = new List<EFClientTools.EFServerReference.OrderParameter>(),
    //        WhereParameters = new List<EFClientTools.EFServerReference.WhereParameter>(),
    //    };
    //    if (keyValues == null)
    //    {
    //        packetInfo.OnlySchema = true;
    //    }
    //    else
    //    {
    //        foreach (var item in keyValues)
    //        {
    //            packetInfo.WhereParameters.Add(new EFClientTools.EFServerReference.WhereParameter() { And = true, Field = item.Key, Value = item.Value });
    //        }
    //    }

    //    var xml = (string)EFClientTools.ClientUtility.Client.GetDataSet(clientInfo, AssemblyName, TableName, packetInfo);
    //    StringReader reader = new StringReader(xml);
    //    var dataSet = new DataSet();
    //    dataSet.ReadXml(reader, XmlReadMode.Auto);
    //    return dataSet;
    //}

    /// <summary>
    /// 取得主键
    /// </summary>
    /// <returns></returns>
    public List<string> GetPrimaryKeys()
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        return EFClientTools.ClientUtility.Client.GetEntityPrimaryKeys(clientInfo, AssemblyName, CommandName, TableName);
    }
        
    /// <summary>
    /// 取主档数据笔数
    /// </summary>
    public int GetDataCount(string whereString)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        var packetInfo = new EFClientTools.EFServerReference.PacketInfo()
        {
            OrderParameters = new List<EFClientTools.EFServerReference.OrderParameter>(),
            WhereParameters = new List<EFClientTools.EFServerReference.WhereParameter>(),
            WhereString = whereString
        };
        return EFClientTools.ClientUtility.Client.GetDataCount(clientInfo, AssemblyName, CommandName, packetInfo);
    }


    public EFClientTools.EFServerReference.LockStatus DoRecordLock(Dictionary<string, object> row, EFClientTools.EFServerReference.LockType type)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        var packetInfo = new EFClientTools.EFServerReference.PacketInfo()
        {
            OrderParameters = new List<EFClientTools.EFServerReference.OrderParameter>(),
            WhereParameters = new List<EFClientTools.EFServerReference.WhereParameter>(),
        };
        foreach (var item in row)
        {
            packetInfo.WhereParameters.Add(new EFClientTools.EFServerReference.WhereParameter() { And = true, Field = item.Key, Value = item.Value });
        }
        return EFClientTools.ClientUtility.Client.DoRecordLock(clientInfo, AssemblyName, CommandName, packetInfo, type);
    }

    /// <summary>
    ///  保存数据
    /// </summary>
    public DataSet ApplyUpdates(DataSet dataSet)
    {
        var clientInfo = EFClientTools.ClientUtility.ClientInfo;
        var xml = string.Empty;
        XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Encoding = new UnicodeEncoding(false, false);
        settings.Indent = false;
        settings.OmitXmlDeclaration = false;
        using (StringWriter textWriter = new StringWriter())
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
            {
                serializer.Serialize(xmlWriter, dataSet);
            }
            xml = textWriter.ToString();
        }
        var returnXml = (string)EFClientTools.ClientUtility.Client.UpdateDataSet(clientInfo, AssemblyName, TableName, xml);
        StringReader reader = new StringReader(returnXml);
        var returnDataSet = new DataSet();
        returnDataSet.ReadXml(reader, XmlReadMode.Auto);
        return returnDataSet;
    }
}