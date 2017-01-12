using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFBase.Design;
using Database = EFWCFModule.EEPAdapter.DatabaseProvider;

namespace EFServerTools.Design
{
    internal class DatabaseEditor : PropertyDropDownEditor
    {
        public override List<string> GetListOfValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            return Database.GetDatabases(null);
        }
    }
}
