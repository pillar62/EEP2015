using System;
namespace OfficeTools
{
    public interface IOfficePlate
    {
        DataSourceCollections DataSource { get;}
        bool Output(int Mode);
        bool MarkException { get;set;}
        TagCollections Tags { get;}
        void OnAfterOutput(EventArgs value);
    }

    /// <summary>
    /// The enum of reference mode of officeplate
    /// </summary>
    public enum PlateModeType
    {
        Xml,
        Com
    }
}
