using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Srvtools
{
    /// <summary>
    /// The class descrip exception catched by eep
    /// </summary>
    [Serializable]
    public class EEPException: Exception
    {
        /************************************************************************
         * type:        抛出的异常类型
         * sourceType:  抛出异常所在的类
         * sourceID:    抛出异常所在的控件ID,没有ID则为null
         * key:         参数的名称(参数错误)
         *              属性的名称(属性错误)
         *              方法的名称(方法错误)
         *              控件的类型名(控件错误)
         *              字段的名称(字段错误)
         * value:       参数的值(参数错误)
         *              属性的值(属性错误)
         *              null(方法错误)
         *              控件的ID(Component没有ID则为null)(控件错误)
         *              字段的值(字段错误)
        ************************************************************************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sourceType"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="controlID"></param>
        public EEPException(ExceptionType type, Type sourceType, string sourceID, string key, string value)
        {
            _type = type;
            _sourceType = sourceType;
            _sourceID = sourceID;
            _key = key;
            _value = value;
        }

        private ExceptionType _type;

        public ExceptionType Type
        {
            get { return _type; }
        }

        private Type _sourceType;

        public Type SourceType
        {
            get { return _sourceType; }
        }

        private string _sourceID;

        public string SourceID
        {
            get { return _sourceID; }
        }

        private string _key;

        public string Key
        {
            get { return _key; }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
        }

        public override string Message
        {
            get
            {
                bool isWeb = string.Compare(CliUtils.fClientSystem, "Web", true) == 0;
                SYS_LANGUAGE language = CliUtils.fLoginUser.Length == 0? CliSysMegLag.GetClientLanguage(): CliUtils.fClientLang;
                string format = SysMsg.GetSystemMessage(language, "Srvtools", "EEPException", "SourceDescription", isWeb)
                                + SysMsg.GetSystemMessage(language, "Srvtools", "EEPException", Type.ToString(), isWeb);
                return string.Format(format, SourceType, SourceID, Key, Value);
            }
        }

        public enum ExceptionType
        {
            #region Argument
            /// <summary>
            /// 参数为空
            /// </summary>
            ArgumentNull,
            /// <summary>
            /// 参数无效
            /// </summary>
            ArgumentInvalid,
            #endregion

            #region Column
            /// <summary>
            /// 栏位不存在
            /// </summary>
            ColumnNotFound,
            /// <summary>
            /// 栏位类型无效
            /// </summary>
            ColumnTypeInvalid,
            /// <summary>
            /// 未能找到栏位值
            /// </summary>
            ColumnValueNotFound,
            ///// <summary>
            ///// 主键不匹配
            ///// </summary>
            //ColumnPrimaryKeyNotMatch,
            #endregion

            #region Control
            /// <summary>
            /// 找不到控件
            /// </summary>
            ControlNotFound,
            /// <summary>
            /// 控件类型不正确
            /// </summary>
            ControlTypeNotMatch,
            /// <summary>
            /// 控件没有初始化
            /// </summary>
            ControlNotInitial,
            #endregion

            DataBaseNotDefined,

            #region Property
            /// <summary>
            /// 属性为空
            /// </summary>
            PropertyNull,
            /// <summary>
            /// 属性值无效
            /// </summary>
            PropertyInvalid,
            /// <summary>
            /// 不支持这个属性
            /// </summary>
            PropertyNotSupported,
            #endregion

            #region Method
            /// <summary>
            /// 不支持这个方法
            /// </summary>
            MethodNotSupported
            #endregion
        }
    }
}
