using System.Text;
using Srvtools;

namespace AjaxTools
{
    public class ExtValidator
    {
        public static string GenValidateConfig(bool allowNull, ValidateType validType, string validMethod, string validMsg)
        {
            StringBuilder builder = new StringBuilder();
            if (!allowNull)
            {
                builder.Append("allowBlank:false,");
            }
            string message = "";
            switch (validType)
            {
                case ValidateType.None:
                    break;
                case ValidateType.Method:
                    if (!string.IsNullOrEmpty(validMethod))
                    {
                        if (validMethod.IndexOf('.') != -1)
                        {
                            builder.AppendFormat("srvValid:'{0}',msg:'{1}',", validMethod, validMsg);
                        }
                        else
                        {
                            builder.AppendFormat("validator:{0},", validMethod);
                        }
                    }
                    break;
                case ValidateType.Alpha:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidAlphaMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    builder.AppendFormat("vtype:'alpha',alphaText:'{0}',", message);
                    break;
                case ValidateType.AlphaNumber:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidAlphaNumMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    builder.AppendFormat("vtype:'alphanum',alphanumText:'{0}',", message);
                    break;
                case ValidateType.Email:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidEmailMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    builder.AppendFormat("vtype:'email',emailText:'{0}',", message);
                    break;
                case ValidateType.Url:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidUrlMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    builder.AppendFormat("vtype:'url',urlText:'{0}',", message);
                    break;
                case ValidateType.Int:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidIntMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    builder.AppendFormat("vtype:'isint',vtypeText:'{0}',", message);
                    break;
                case ValidateType.Float:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidFloatMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    builder.AppendFormat("vtype:'isfloat',vtypeText:'{0}',", message);
                    break;
                case ValidateType.IPAddress:
                    if (string.IsNullOrEmpty(validMsg))
                    {
                        message = SysMsg.GetSystemMessage(CliUtils.fClientLang, "AjaxTools", "ExtGrid", "ValidIPMsg", true);
                    }
                    else
                    {
                        message = validMsg;
                    }
                    builder.AppendFormat("vtype:'ip',vtypeText:'{0}',", message);
                    break;
            }
            if (builder.ToString().EndsWith(","))
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }
    }
}
