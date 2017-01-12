using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ServiceModel.Channels;
using System.ServiceModel;
using EFClientTools.EFServerReference;
using EFClientTools.Web;
using System.Reflection;
using System.Runtime.Serialization;
using EFClientTools.Common;

namespace EFClientTools.Editor
{
    public class EntityClassEditor: StringSelectorEditor
    {
        protected override List<string> GetListToSelect(ITypeDescriptorContext context)
        {
            List<string> lst = new List<string>();
            EFDataSource eds = context.Instance as EFDataSource;
            Assembly assembly = typeof(EntityObject).Assembly;

            lst = EntityProvider.GetClientEntityProperties(eds, assembly);

            return lst;
        }
    }
}
