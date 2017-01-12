using System;
using System.Collections.Generic;
using System.Text;

namespace Srvtools
{
    internal class FilterBuilder
    {
        public static string GetDateTimeFormat(ClientType dbtype)
        {
            switch (dbtype)
            {
                case ClientType.ctMsSql: return "'{0:yyyy-MM-dd HH:mm:ss}'";
                case ClientType.ctOracle: return "to_date('{0:yyyy-MM-dd HH:mm:ss}','yyyy-mm-dd hh24:mi:ss')";
                case ClientType.ctODBC: return "to_Date('{{{0}:yyyyMMddHHmmss}}', '%Y%m%d%H%M%S')";
                case ClientType.ctInformix: return "to_Date('{{{0}:yyyyMMddHHmmss}}', '%Y%m%d%H%M%S')";
                default: return "'{0:yyyy-MM-dd HH:mm:ss}'";
            }
        }
    }
}
