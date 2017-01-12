using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.ComponentModel;

using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
#if MySql
using MySql.Data.MySqlClient;
#endif

namespace Srvtools
{
    public class InfoParameter : DbParameter
    {
        private DbType _dbType;
        private ParameterDirection _direction;
        private bool _isNullable;
        private string _parameterName;
        private int _size;
        private string _sourceColumn;
        private bool _sourceColumnNullMapping;
        private DataRowVersion _sourceVersion;
        private byte _scale;
        private byte _precision;
        private object _value;
        private InfoDbType _infoDbType;
        private string _xmlSchemaCollectionDatabase;
        private string _xmlSchemaCollectionName;
        private string _xmlSchemaCollectionOwningSchema;

        public InfoParameter()
        {
            _sourceVersion = DataRowVersion.Current;
            _direction = ParameterDirection.Input;
            _infoDbType = InfoDbType.VarChar;
        }

        public override string ToString()
        {
            return this.ParameterName;
        }

        public override DbType DbType
        {
            get
            {
                return _dbType;
            }
            set
            {
                _dbType = value;
            }
        }

        [Category("Data")]
        public InfoDbType InfoDbType
        {
            get
            {
                return _infoDbType;
            }
            set
            {
                _infoDbType = value;
            }
        }

        [Category("Data")]
        public byte Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
            }
        }

        [Category("Data")]
        public byte Precision
        {
            get
            {
                return _precision;
            }
            set
            {
                _precision = value;
            }
        }

        [Category("XML")]
        public string XmlSchemaCollectionDatabase
        {
            get
            {
                return _xmlSchemaCollectionDatabase;
            }
            set
            {
                _xmlSchemaCollectionDatabase = value;
            }
        }

        [Category("XML")]
        public string XmlSchemaCollectionName
        {
            get
            {
                return _xmlSchemaCollectionName;
            }
            set
            {
                _xmlSchemaCollectionName = value;
            }
        }

        [Category("XML")]
        public string XmlSchemaCollectionOwningSchema
        {
            get
            {
                return _xmlSchemaCollectionOwningSchema;
            }
            set
            {
                _xmlSchemaCollectionOwningSchema = value;
            }
        }

        public override ParameterDirection Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
            }
        }

        public override bool IsNullable
        {
            get
            {
                return _isNullable;
            }
            set
            {
                _isNullable = value;
            }
        }

        public override string ParameterName
        {
            get
            {
                if (_parameterName == null || _parameterName.Length == 0)
                {
                    string s = base.ToString();
                    string[] ss = s.Split(".".ToCharArray());
                    return ss[1];
                }
                else
                {
                    return _parameterName;
                }
            }
            set
            {
                _parameterName = value;
            }
        }

        public override int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }

        public override string SourceColumn
        {
            get
            {
                return _sourceColumn;
            }
            set
            {
                _sourceColumn = value;
            }
        }

        public override bool SourceColumnNullMapping
        {
            get
            {
                return _sourceColumnNullMapping;
            }
            set
            {
                _sourceColumnNullMapping = value;
            }
        }

        public override DataRowVersion SourceVersion
        {
            get
            {
                return _sourceVersion;
            }
            set
            {
                _sourceVersion = value;
            }
        }

        public override object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public override void ResetDbType()
        {

        }
    }

    public enum InfoDbType
    {
        BFile,
        BigInt,
        Binary,
        Bit,
        Blob,
        Boolean,
        BSTR,
        Byte,
        Char,
        Clob,
        Currency,
        Cursor,
        Date,
        DateTime,
        Datetime,
        DBDate,
        DBTime,
        DBTimeStamp,
        Decimal,
        Double,
        Empty,
        Enum,
        Error,
        Filetime,
        Float,
        Guid,
        Geometry,
        IDispatch,
        Image,
        Int,
        Int16,
        Int24,
        Int32,
        Int64,
        Integer,
        IntervalDayToSecond,
        IntervalYearToMonth,
        IUnknown,
        LongBlob,
        LongRaw,
        LongText,
        LongVarBinary,
        LongVarChar,
        LongVarWChar,
        MediumBlob,
        MediumText,
        Money,
        NChar,
        NClob,
        Newdate,
        NewDecimal,
        NText,
        Number,
        Numeric,
        NVarChar,
        PropVariant,
        Raw,
        Real,
        RowId,
        SamllInt,
        SByte,
        Set,
        Single,
        SmallDateTime,
        SmallInt,
        SmallMoney,
        String,
        Text,
        Time,
        Timestamp,
        TimestampLocal,
        TimestampWithTZ,
        TinyBlob,
        TinyInt,
        TinyText,
        UByte,
        Udt,
        UInt16,
        UInt24,
        UInt32,
        UInt64,
        UniqueIdentifier,
        UnsignedBigInt,
        UnsignedInt,
        UnsignedSmallInt,
        UnsignedTinyInt,
        VarBinary,
        VarChar,
        Variant,
        VarNumeric,
        VarString,
        VarWChar,
        WChar,
        Year,
        Xml
    }

    internal static class InfoDbTypeConverter
    {
        #region GetDbType(SqlDbType, OleDbType, OdbcType, OracleType)

        public static SqlDbType GetSqlDbType(InfoDbType infoDbType)
        {
            try
            {
                return (SqlDbType)Enum.Parse(typeof(SqlDbType), infoDbType.ToString(), true);
            }
            catch 
            {
                return SqlDbType.VarChar;
            }
        }

        public static OleDbType GetOleDbType(InfoDbType infoDbType)
        {
            try
            {
                return (OleDbType)Enum.Parse(typeof(OleDbType), infoDbType.ToString(), true);
            }
            catch
            {
                return OleDbType.VarChar;
            }
        }

        public static OdbcType GetOdbcType(InfoDbType infoDbType)
        {
            try
            {
                return (OdbcType)Enum.Parse(typeof(OdbcType), infoDbType.ToString(), true);
            }
            catch
            {
                return OdbcType.VarChar;
            }
        }

        public static OracleType GetOracleType(InfoDbType infoDbType)
        {
            try
            {
                return (OracleType)Enum.Parse(typeof(OracleType), infoDbType.ToString(), true);
            }
            catch
            {
                return OracleType.VarChar;
            }
        }

#if MySql
        public static MySqlDbType GetMySqlDbType(InfoDbType infoDbType)
        {
            try
            {
                return (MySqlDbType)Enum.Parse(typeof(MySqlDbType), infoDbType.ToString(), true);
            }
            catch 
            {
                return MySqlDbType.VarChar;
            }
        }
#endif

#if Informix
        public static IBM.Data.Informix.IfxType GetIfxType(InfoDbType infoDbType)
        {
            try
            {
                return (IBM.Data.Informix.IfxType)Enum.Parse(typeof(IBM.Data.Informix.IfxType), infoDbType.ToString(), true);
            }
            catch 
            {
                return IBM.Data.Informix.IfxType.VarChar;
            }
        }
#endif

#if Sybase
        public static Sybase.Data.AseClient.AseDbType GetAseType(InfoDbType infoDbType)
        {
            try
            {
                return (Sybase.Data.AseClient.AseDbType)Enum.Parse(typeof(Sybase.Data.AseClient.AseDbType), infoDbType.ToString(), true);
            }
            catch 
            {
                return Sybase.Data.AseClient.AseDbType.VarChar;
            }
        }
#endif

        #endregion
    }
}