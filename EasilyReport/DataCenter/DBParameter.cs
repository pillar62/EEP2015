using System;
using System.Collections.Generic;
using System.Text;
using Srvtools;
using System.Data;

namespace Infolight.EasilyReportTools.DataCenter
{
    internal class DBParameter
    {
        public DBParameter(ClientType clientType)
        {
            GetParamTag(clientType);
            _clientType = clientType;
        }

        private ClientType _clientType;

        public ClientType ClientType
        {
            get { return _clientType; }
            set { _clientType = value; }
        }
	

        private string paramTag;

        public string ParamTag
        {
            get { return paramTag; }
        }

        public string GetParamTag(ClientType clientType)
        {
            switch (clientType)
            {
                case ClientType.ctMsSql:
                    paramTag = "@";
                    break;
                case ClientType.ctMySql:
                    paramTag = "@";
                    break;
                case ClientType.ctNone:
                    paramTag = "@";
                    break;
                case ClientType.ctODBC:
                    paramTag = "@";
                    break;
                case ClientType.ctOleDB:
                    paramTag = "?";
                    break;
                case ClientType.ctOracle:
                    paramTag = ":";
                    break;
                case ClientType.ctInformix:
                    paramTag = "?";
                    break;
            }

            return paramTag;
        }

        public IDbDataParameter CreateParameter()
        {
            switch (ClientType)
            {
                case ClientType.ctMsSql:
                    return new System.Data.SqlClient.SqlParameter();
                case ClientType.ctODBC:
                    return new System.Data.Odbc.OdbcParameter();
                case ClientType.ctOleDB:
                    return new System.Data.OleDb.OleDbParameter();
                default:
                    throw new Exception("not support database type");
            }
        }
    }
}
