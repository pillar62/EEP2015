using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JQClientTools
{
    public class JQProperyNullException: Exception
    {
        public JQProperyNullException(string controlID, Type controlType, string propertyName)
        {
            ControlID = controlID;
            ControlType = controlType;
            PropertyName = propertyName;
        }

        public string ControlID { get; set; }

        public Type ControlType { get; set; }

        public string PropertyName { get; set; }

        public override string Message
        {
            get
            {
                return string.Format("Property of Control is Empty.(ControlID:'{0}', ControlType:'{1}', PropertyName:'{2}')"
                    , ControlID, ControlType.Name, PropertyName);
            }
        }
    }
}
