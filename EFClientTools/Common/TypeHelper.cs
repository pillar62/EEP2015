using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFClientTools.Common
{
    public class TypeHelper
    {
        public static bool IsInt(Type type)
        {
            return (type == typeof(Nullable<UInt16>) || type == typeof(UInt16) ||
                type == typeof(Nullable<UInt32>) || type == typeof(UInt32) ||
                type == typeof(Nullable<UInt64>) || type == typeof(UInt64) ||
                type == typeof(Nullable<Int16>) || type == typeof(Int16) ||
                type == typeof(Nullable<Int32>) || type == typeof(Int32) ||
                type == typeof(Nullable<Int64>) || type == typeof(Int64));
        }

        public static bool IsFloat(Type type)
        {
            return (type == typeof(Nullable<Single>) || type == typeof(Single));
        }

        public static bool IsDouble(Type type)
        {
            return (type == typeof(Nullable<Double>) || type == typeof(Double));
        }

        public static bool IsDecimal(Type type)
        {
            return (type == typeof(Nullable<Decimal>) || type == typeof(Decimal));
        }

        public static bool IsNumeric(Type type)
        {
            return (IsInt(type) || IsFloat(type) || IsDouble(type) || IsDecimal(type));
        }

        public static bool IsChar(Type type)
        {
            return (type == typeof(Nullable<char>) || type == typeof(char));
        }

        public static bool IsBoolean(Type type)
        {
            return (type == typeof(Nullable<Boolean>) || type == typeof(Boolean));
        }

        public static bool IsDateTime(Type type)
        {
            return (type == typeof(Nullable<DateTime>) || type == typeof(DateTime));
        }

        public static bool IsString(Type type)
        {
            return (type == typeof(String));
        }

        public static bool IsPrimitive(Type type)
        {
            return (IsNumeric(type) || IsChar(type) || IsBoolean(type) || IsDateTime(type) || IsString(type));
        }

        public static Type GetNullablePrimitiveType(Type type)
        {
            if (type.IsGenericType && type.Name == "Nullable`1")
            {
                return type.GetGenericArguments()[0];
            }
            return type;
        }

        public static string ReturnDataType(Type t)
        {
            if (TypeHelper.IsString(t))
                return "string";
            else if (TypeHelper.IsInt(t))
                return "int";
            else if (TypeHelper.IsFloat(t))
                return "float";
            else if (TypeHelper.IsBoolean(t))
                return "boolean";
            else if (TypeHelper.IsDateTime(t))
                return "date";
            else if (TypeHelper.IsDouble(t))
                return "float";
            else if (TypeHelper.IsDecimal(t))
                return "float";
            return "string";
        }
    }
}
