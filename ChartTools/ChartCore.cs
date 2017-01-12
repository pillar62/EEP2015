using System.Data;
namespace ChartTools
{
    interface IChartDataSource
    {
        string DataSourceID { get; set;}
        WebChartFieldsCollection DataFields { get;}
        string ResxDataSet { get; set;}
        string GetDataSetID();
        DataTable GetDt();
        //SourceNumericType SrcNumType { get; set;}
    }

    interface IChartField
    {
        string FieldName { get; set;}
        string FeildCaption { get;set;}
        string CaptionFieldName { get;set;}
    }
}
