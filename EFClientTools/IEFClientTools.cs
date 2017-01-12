using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFClientTools
{
    public interface IEFDataSource
    {
        string RemoteName { get; set; }

        string DataMember { get; set; }

        string MasterDataSourceID { get; set; }

        bool Active { get; set; }

        int PackageRecords { get; set; }

        //string[] GetEntityClasses();

        Type GetEntityType();
    }
}