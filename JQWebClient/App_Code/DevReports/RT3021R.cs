using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for RT3021R
/// </summary>
public class RT3021R : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private XRRichText xrRichText1;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel53;
    private XRControlStyle Title;
    private XRControlStyle FieldCaption;
    private XRControlStyle PageInfo;
    private XRControlStyle DataField;
    private XRLabel xrLabel6;
    private XRShape xrShape1;
    private XRRichText xrRichText3;
    private XRLabel xrLabel2;
    private XRLabel xrLabel46;
    private XRLabel xrLabel43;
    private XRLabel xrLabel40;
    private XRLabel xrLabel54;
    private XRLabel xrLabel55;
    private XRLabel xrLabel56;
    private XRLabel xrLabel57;
    private XRLabel xrLabel58;
    private XRLabel xrLabel59;
    private XRLabel xrLabel60;
    private XRLabel xrLabel61;
    private XRLabel xrLabel62;
    private XRLabel xrLabel63;
    private XRLabel xrLabel64;
    private XRLabel xrLabel65;
    private XRLabel xrLabel66;
    private XRRichText xrRichText2;
    private XRLabel xrLabel67;
    private XRLabel xrLabel68;
    private XRLabel xrLabel4;
    private XRLabel xrLabel3;
    private XRLabel xrLabel5;
    private XRBarCode xrBarCode3;
    private XRBarCode xrBarCode2;
    private XRLabel xrLabel7;
    
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTable xrTable2;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell4;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell5;
    private XRLabel xrLabel1;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell6;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell2;
    private XRBarCode xrBarCode5;
    private XRLabel xrLabel8;
    private XRLabel xrLabel9;
    private XRBarCode xrBarCode4;
    private XRBarCode xrBarCode6;
    private XRLabel xrLabel10;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell3;
    private XRBarCode xrBarCode7;
    private XRLabel xrLabel11;
    private XRLabel xrLabel12;
    private XRBarCode xrBarCode8;
    private XRBarCode xrBarCode9;
    private XRLabel xrLabel13;
    private XRTable xrTable3;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell7;
    private XRLabel xrLabel70;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell11;
    private XRTableRow xrTableRow10;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell13;
    private XRTableRow xrTableRow9;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell9;
    private XRTable xrTable4;
    private XRTableRow xrTableRow11;
    private XRTableCell xrTableCell14;
    private XRTableRow xrTableRow14;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell25;
    private XRTableRow xrTableRow13;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell22;
    private XRTableRow xrTableRow12;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell26;
    private XRLabel xrLabel14;
    private XRLabel xrLabel15;
    private XRLabel xrLabel16;
    private XRLabel xrLabel17;
    private XRLabel xrLabel18;
    private XRLabel xrLabel19;
    private XRLabel xrLabel21;
    private XRLabel xrLabel20;
    private XRLabel xrLabel22;
    private XRLabel xrLabel23;
    private XRLabel xrLabel24;
    private XRLabel xrLabel25;
    private XRPictureBox xrPictureBox2;
    private XRBarCode xrBarCode1;
    private int ii = 0;
    public RT3021R()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            string resourceFileName = "RT3021R.resx";
            System.Resources.ResourceManager resources = global::Resources.RT3021R.ResourceManager;
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraPrinting.Shape.ShapeRectangle shapeRectangle1 = new DevExpress.XtraPrinting.Shape.ShapeRectangle();
            DevExpress.XtraPrinting.BarCode.Code39Generator code39Generator1 = new DevExpress.XtraPrinting.BarCode.Code39Generator();
            DevExpress.XtraPrinting.BarCode.Code39Generator code39Generator2 = new DevExpress.XtraPrinting.BarCode.Code39Generator();
            DevExpress.XtraPrinting.BarCode.Code39Generator code39Generator3 = new DevExpress.XtraPrinting.BarCode.Code39Generator();
            DevExpress.XtraPrinting.BarCode.Code39Generator code39Generator4 = new DevExpress.XtraPrinting.BarCode.Code39Generator();
            DevExpress.XtraPrinting.BarCode.Code39Generator code39Generator5 = new DevExpress.XtraPrinting.BarCode.Code39Generator();
            DevExpress.XtraPrinting.BarCode.Code39Generator code39Generator6 = new DevExpress.XtraPrinting.BarCode.Code39Generator();
            DevExpress.XtraPrinting.BarCode.Code39Generator code39Generator7 = new DevExpress.XtraPrinting.BarCode.Code39Generator();
            DevExpress.XtraPrinting.BarCode.Code39Generator code39Generator8 = new DevExpress.XtraPrinting.BarCode.Code39Generator();
            DevExpress.XtraPrinting.BarCode.Code39Generator code39Generator9 = new DevExpress.XtraPrinting.BarCode.Code39Generator();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel56 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel57 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel60 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel65 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel58 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel62 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel61 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel64 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel59 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel63 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel70 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrRichText3 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrShape1 = new DevExpress.XtraReports.UI.XRShape();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrBarCode2 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrBarCode3 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrBarCode5 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrBarCode4 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrBarCode6 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrBarCode7 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrBarCode8 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrBarCode9 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel68 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel67 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrRichText2 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrLabel66 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel53 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrLabel55 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel54 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox2,
            this.xrTable4,
            this.xrTable3,
            this.xrTable2,
            this.xrTable1,
            this.xrLabel68,
            this.xrLabel67,
            this.xrRichText2,
            this.xrLabel66,
            this.xrPictureBox1,
            this.xrLabel53,
            this.xrRichText1,
            this.xrLabel55,
            this.xrLabel54,
            this.xrLabel43,
            this.xrLabel46});
            this.Detail.HeightF = 1100F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StyleName = "DataField";
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox2.Image")));
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(7.807032F, 973.6818F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(211.5724F, 53.20837F);
            // 
            // xrTable4
            // 
            this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable4.BorderWidth = 2F;
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(6.681269F, 144.3182F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow11,
            this.xrTableRow14,
            this.xrTableRow13,
            this.xrTableRow12});
            this.xrTable4.SizeF = new System.Drawing.SizeF(719.89F, 100F);
            this.xrTable4.StylePriority.UseBorders = false;
            this.xrTable4.StylePriority.UseBorderWidth = false;
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell14});
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Weight = 1D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel56});
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Weight = 2.256268418168097D;
            // 
            // xrLabel56
            // 
            this.xrLabel56.AutoWidth = true;
            this.xrLabel56.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel56.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel56.LocationFloat = new DevExpress.Utils.PointFloat(3.318717F, 5.999986F);
            this.xrLabel56.Name = "xrLabel56";
            this.xrLabel56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel56.SizeF = new System.Drawing.SizeF(375.2917F, 14.91667F);
            this.xrLabel56.StylePriority.UseBorders = false;
            this.xrLabel56.StylePriority.UseFont = false;
            this.xrLabel56.Text = "基本資料(若基本資料有異動，請來電本客服中心)";
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell23,
            this.xrTableCell15,
            this.xrTableCell24,
            this.xrTableCell25});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel57});
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Weight = 0.36135531573478663D;
            // 
            // xrLabel57
            // 
            this.xrLabel57.AutoWidth = true;
            this.xrLabel57.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel57.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel57.LocationFloat = new DevExpress.Utils.PointFloat(4.681223F, 4.083304F);
            this.xrLabel57.Name = "xrLabel57";
            this.xrLabel57.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel57.SizeF = new System.Drawing.SizeF(79.13783F, 13.91668F);
            this.xrLabel57.StylePriority.UseBorders = false;
            this.xrLabel57.StylePriority.UseFont = false;
            this.xrLabel57.Text = "姓名/公司";
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel40});
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Weight = 1.1422633890098832D;
            // 
            // xrLabel40
            // 
            this.xrLabel40.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.cusnc")});
            this.xrLabel40.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(3.467907E-05F, 3.083304F);
            this.xrLabel40.Name = "xrLabel40";
            this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel40.SizeF = new System.Drawing.SizeF(265.646F, 14.00001F);
            this.xrLabel40.StylePriority.UseBorders = false;
            this.xrLabel40.StylePriority.UseFont = false;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel60});
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.Weight = 0.36506823350419193D;
            // 
            // xrLabel60
            // 
            this.xrLabel60.AutoWidth = true;
            this.xrLabel60.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel60.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel60.LocationFloat = new DevExpress.Utils.PointFloat(2.166679F, 4.083304F);
            this.xrLabel60.Name = "xrLabel60";
            this.xrLabel60.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel60.SizeF = new System.Drawing.SizeF(79.13783F, 13.91668F);
            this.xrLabel60.StylePriority.UseBorders = false;
            this.xrLabel60.StylePriority.UseFont = false;
            this.xrLabel60.Text = "繳費期限";
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel65});
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.Weight = 1.139670952641934D;
            // 
            // xrLabel65
            // 
            this.xrLabel65.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel65.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.duedat", "{0:yyyy\'年\'M\'月\'d\'日\'}")});
            this.xrLabel65.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel65.LocationFloat = new DevExpress.Utils.PointFloat(6.544287F, 4F);
            this.xrLabel65.Name = "xrLabel65";
            this.xrLabel65.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel65.SizeF = new System.Drawing.SizeF(175.4543F, 14.00001F);
            this.xrLabel65.StylePriority.UseBorders = false;
            this.xrLabel65.StylePriority.UseFont = false;
            this.xrLabel65.Text = "xrLabel43";
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell20,
            this.xrTableCell16,
            this.xrTableCell21,
            this.xrTableCell22});
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 1D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel58});
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.Weight = 0.36135531573478663D;
            // 
            // xrLabel58
            // 
            this.xrLabel58.AutoWidth = true;
            this.xrLabel58.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel58.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel58.LocationFloat = new DevExpress.Utils.PointFloat(3.318717F, 4.083304F);
            this.xrLabel58.Name = "xrLabel58";
            this.xrLabel58.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel58.SizeF = new System.Drawing.SizeF(79.13783F, 13.91667F);
            this.xrLabel58.StylePriority.UseBorders = false;
            this.xrLabel58.StylePriority.UseFont = false;
            this.xrLabel58.Text = "社區名稱";
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel62});
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Weight = 1.1422631977144018D;
            // 
            // xrLabel62
            // 
            this.xrLabel62.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel62.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.comn")});
            this.xrLabel62.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel62.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4.083304F);
            this.xrLabel62.Name = "xrLabel62";
            this.xrLabel62.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel62.SizeF = new System.Drawing.SizeF(268.1731F, 14F);
            this.xrLabel62.StylePriority.UseBorders = false;
            this.xrLabel62.StylePriority.UseFont = false;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel61});
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.Weight = 0.36506842479967372D;
            // 
            // xrLabel61
            // 
            this.xrLabel61.AutoWidth = true;
            this.xrLabel61.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel61.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel61.LocationFloat = new DevExpress.Utils.PointFloat(0.2786116F, 4.083304F);
            this.xrLabel61.Name = "xrLabel61";
            this.xrLabel61.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel61.SizeF = new System.Drawing.SizeF(79.13783F, 13.91669F);
            this.xrLabel61.StylePriority.UseBorders = false;
            this.xrLabel61.StylePriority.UseFont = false;
            this.xrLabel61.Text = "續約單號";
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel64});
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.Weight = 1.139670952641934D;
            // 
            // xrLabel64
            // 
            this.xrLabel64.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel64.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.NOTICEID")});
            this.xrLabel64.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel64.LocationFloat = new DevExpress.Utils.PointFloat(3.370944F, 4.000006F);
            this.xrLabel64.Name = "xrLabel64";
            this.xrLabel64.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel64.SizeF = new System.Drawing.SizeF(253.1254F, 14F);
            this.xrLabel64.StylePriority.UseBorders = false;
            this.xrLabel64.StylePriority.UseFont = false;
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell17,
            this.xrTableCell26});
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Weight = 1D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel59});
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Weight = 0.36135537949994723D;
            // 
            // xrLabel59
            // 
            this.xrLabel59.AutoWidth = true;
            this.xrLabel59.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel59.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel59.LocationFloat = new DevExpress.Utils.PointFloat(2.32589F, 4.083304F);
            this.xrLabel59.Name = "xrLabel59";
            this.xrLabel59.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel59.SizeF = new System.Drawing.SizeF(79.13783F, 13.91669F);
            this.xrLabel59.StylePriority.UseBorders = false;
            this.xrLabel59.StylePriority.UseFont = false;
            this.xrLabel59.Text = "聯絡電話";
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel63});
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.Weight = 2.6470025113908489D;
            // 
            // xrLabel63
            // 
            this.xrLabel63.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel63.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.contacttel")});
            this.xrLabel63.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel63.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4.083304F);
            this.xrLabel63.Name = "xrLabel63";
            this.xrLabel63.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel63.SizeF = new System.Drawing.SizeF(617.1956F, 14.00002F);
            this.xrLabel63.StylePriority.UseBorders = false;
            this.xrLabel63.StylePriority.UseFont = false;
            // 
            // xrTable3
            // 
            this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable3.BorderWidth = 2F;
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(7.807021F, 402.2727F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7,
            this.xrTableRow8,
            this.xrTableRow10,
            this.xrTableRow9});
            this.xrTable3.SizeF = new System.Drawing.SizeF(715.2142F, 136.3636F);
            this.xrTable3.StylePriority.UseBorders = false;
            this.xrTable3.StylePriority.UseBorderWidth = false;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7});
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel70,
            this.xrLabel7});
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Weight = 3D;
            // 
            // xrLabel70
            // 
            this.xrLabel70.AutoWidth = true;
            this.xrLabel70.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel70.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel70.LocationFloat = new DevExpress.Utils.PointFloat(9.999987F, 10.17426F);
            this.xrLabel70.Name = "xrLabel70";
            this.xrLabel70.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel70.SizeF = new System.Drawing.SizeF(217.6097F, 13.91666F);
            this.xrLabel70.StylePriority.UseBorders = false;
            this.xrLabel70.StylePriority.UseFont = false;
            this.xrLabel70.Text = "請選擇方案類型 (前期使用方案: ";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.codenc", "{0})")});
            this.xrLabel7.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(229.7345F, 11.09075F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(188.4886F, 13.00015F);
            this.xrLabel7.StylePriority.UseBorders = false;
            this.xrLabel7.StylePriority.UseFont = false;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell10,
            this.xrTableCell11});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel14});
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Weight = 0.85692521709330349D;
            // 
            // xrLabel14
            // 
            this.xrLabel14.AutoWidth = true;
            this.xrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.P01")});
            this.xrLabel14.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(2.429719F, 6.090851F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(191.8653F, 18.00003F);
            this.xrLabel14.StylePriority.UseBorders = false;
            this.xrLabel14.StylePriority.UseFont = false;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel21,
            this.xrLabel20,
            this.xrLabel17});
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Weight = 2.1430747829066967D;
            // 
            // xrLabel21
            // 
            this.xrLabel21.AutoWidth = true;
            this.xrLabel21.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.Q1", "共({0})個月")});
            this.xrLabel21.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(392.1822F, 10.00004F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(112.8979F, 18.00003F);
            this.xrLabel21.StylePriority.UseBorders = false;
            this.xrLabel21.StylePriority.UseFont = false;
            // 
            // xrLabel20
            // 
            this.xrLabel20.AutoWidth = true;
            this.xrLabel20.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel20.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.D1", "{0:yyyy\'年\'MM\'月\'dd\'日\'}")});
            this.xrLabel20.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(241.7498F, 9.999974F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(145.8525F, 18.00003F);
            this.xrLabel20.StylePriority.UseBorders = false;
            this.xrLabel20.StylePriority.UseFont = false;
            // 
            // xrLabel17
            // 
            this.xrLabel17.AutoWidth = true;
            this.xrLabel17.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.newBillingDat", "{0:yyyy\'年\'MM\'月\'dd\'日\'}")});
            this.xrLabel17.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(3.178914E-05F, 10.00004F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(241.7498F, 18.00003F);
            this.xrLabel17.StylePriority.UseBorders = false;
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel17_BeforePrint);
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell12,
            this.xrTableCell13});
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Weight = 1D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel15});
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Weight = 0.85692521709330349D;
            // 
            // xrLabel15
            // 
            this.xrLabel15.AutoWidth = true;
            this.xrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.P02")});
            this.xrLabel15.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(2.429719F, 6.090698F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(191.8653F, 18.00003F);
            this.xrLabel15.StylePriority.UseBorders = false;
            this.xrLabel15.StylePriority.UseFont = false;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel22,
            this.xrLabel23,
            this.xrLabel18});
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.Weight = 2.1430747829066967D;
            // 
            // xrLabel22
            // 
            this.xrLabel22.AutoWidth = true;
            this.xrLabel22.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel22.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.D2", "{0:yyyy\'年\'MM\'月\'dd\'日\'}")});
            this.xrLabel22.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(241.7498F, 6.0908F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(145.8525F, 18.00003F);
            this.xrLabel22.StylePriority.UseBorders = false;
            this.xrLabel22.StylePriority.UseFont = false;
            // 
            // xrLabel23
            // 
            this.xrLabel23.AutoWidth = true;
            this.xrLabel23.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.Q2", "共({0})個月")});
            this.xrLabel23.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(392.1821F, 6.0908F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(112.8979F, 18.00003F);
            this.xrLabel23.StylePriority.UseBorders = false;
            this.xrLabel23.StylePriority.UseFont = false;
            // 
            // xrLabel18
            // 
            this.xrLabel18.AutoWidth = true;
            this.xrLabel18.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.newBillingDat", "{0:yyyy\'年\'MM\'月\'dd\'日\'}")});
            this.xrLabel18.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(0F, 6.0908F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(241.7498F, 18.00003F);
            this.xrLabel18.StylePriority.UseBorders = false;
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel18_BeforePrint);
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell8,
            this.xrTableCell9});
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 1D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel16});
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Weight = 0.85692521709330349D;
            // 
            // xrLabel16
            // 
            this.xrLabel16.AutoWidth = true;
            this.xrLabel16.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.P03")});
            this.xrLabel16.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(2.429719F, 6.090851F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(191.8653F, 18.00003F);
            this.xrLabel16.StylePriority.UseBorders = false;
            this.xrLabel16.StylePriority.UseFont = false;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel24,
            this.xrLabel25,
            this.xrLabel19});
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Weight = 2.1430747829066967D;
            // 
            // xrLabel24
            // 
            this.xrLabel24.AutoWidth = true;
            this.xrLabel24.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.D3", "{0:yyyy\'年\'MM\'月\'dd\'日\'}")});
            this.xrLabel24.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(241.7498F, 6.090864F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(145.8525F, 18.00003F);
            this.xrLabel24.StylePriority.UseBorders = false;
            this.xrLabel24.StylePriority.UseFont = false;
            // 
            // xrLabel25
            // 
            this.xrLabel25.AutoWidth = true;
            this.xrLabel25.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel25.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.Q3", "共({0})個月")});
            this.xrLabel25.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(392.182F, 6.090673F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(112.8979F, 18.00003F);
            this.xrLabel25.StylePriority.UseBorders = false;
            this.xrLabel25.StylePriority.UseFont = false;
            // 
            // xrLabel19
            // 
            this.xrLabel19.AutoWidth = true;
            this.xrLabel19.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.newBillingDat", "{0:yyyy\'年\'MM\'月\'dd\'日\'}")});
            this.xrLabel19.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(0F, 6.090864F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(241.7498F, 18F);
            this.xrLabel19.StylePriority.UseBorders = false;
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel19_BeforePrint);
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.BorderWidth = 2F;
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(6.681269F, 554.5455F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5,
            this.xrTableRow6});
            this.xrTable2.SizeF = new System.Drawing.SizeF(715.2143F, 130F);
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseBorderWidth = false;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 0.57391328715747592D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6});
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Weight = 1D;
            // 
            // xrLabel6
            // 
            this.xrLabel6.AutoWidth = true;
            this.xrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel6.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(6.957592F, 7.999986F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(431.1439F, 13.91666F);
            this.xrLabel6.StylePriority.UseBorders = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "信用卡繳款專區";
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1.9130442905249194D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrRichText3,
            this.xrShape1});
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Weight = 1D;
            // 
            // xrRichText3
            // 
            this.xrRichText3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrRichText3.Font = new System.Drawing.Font("標楷體", 14F);
            this.xrRichText3.LocationFloat = new DevExpress.Utils.PointFloat(9.292707F, 10F);
            this.xrRichText3.Name = "xrRichText3";
            this.xrRichText3.SerializableRtfString = resources.GetString("xrRichText3.SerializableRtfString");
            this.xrRichText3.SizeF = new System.Drawing.SizeF(504.1285F, 72.91681F);
            this.xrRichText3.StylePriority.UseBorders = false;
            this.xrRichText3.StylePriority.UseFont = false;
            // 
            // xrShape1
            // 
            this.xrShape1.BorderWidth = 1F;
            this.xrShape1.LocationFloat = new DevExpress.Utils.PointFloat(523.7416F, 45.41681F);
            this.xrShape1.Name = "xrShape1";
            this.xrShape1.Shape = shapeRectangle1;
            this.xrShape1.SizeF = new System.Drawing.SizeF(180.2084F, 37.5F);
            this.xrShape1.StylePriority.UseBorderWidth = false;
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.BorderWidth = 2F;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(5.555517F, 700.6818F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow2,
            this.xrTableRow3,
            this.xrTableRow4});
            this.xrTable1.SizeF = new System.Drawing.SizeF(716.34F, 270F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseBorderWidth = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 0.56531047525377665D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrLabel2});
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Weight = 3D;
            // 
            // xrLabel1
            // 
            this.xrLabel1.AutoWidth = true;
            this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel1.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(9.999986F, 8.000021F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(131.7122F, 14.99998F);
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "便利商店繳款專區";
            // 
            // xrLabel2
            // 
            this.xrLabel2.AutoWidth = true;
            this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel2.Font = new System.Drawing.Font("標楷體", 8F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(147.8063F, 10.08338F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(214.6859F, 11.91663F);
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "(請自行勾選一項適宜的資費方案)";
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell6});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1.5074945826601542D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrBarCode1,
            this.xrBarCode2,
            this.xrLabel4,
            this.xrBarCode3,
            this.xrLabel5});
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Weight = 3D;
            // 
            // xrLabel3
            // 
            this.xrLabel3.AutoWidth = true;
            this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.P01")});
            this.xrLabel3.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(4.681223F, 7.999939F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(249.2524F, 18.00006F);
            this.xrLabel3.StylePriority.UseBorders = false;
            this.xrLabel3.StylePriority.UseFont = false;
            // 
            // xrBarCode1
            // 
            this.xrBarCode1.AutoModule = true;
            this.xrBarCode1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrBarCode1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.COD11")});
            this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(10.41846F, 27.99995F);
            this.xrBarCode1.Name = "xrBarCode1";
            this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode1.SizeF = new System.Drawing.SizeF(174.9999F, 46.20502F);
            this.xrBarCode1.StylePriority.UseBorders = false;
            code39Generator1.CalcCheckSum = false;
            code39Generator1.WideNarrowRatio = 3F;
            this.xrBarCode1.Symbology = code39Generator1;
            // 
            // xrBarCode2
            // 
            this.xrBarCode2.AutoModule = true;
            this.xrBarCode2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrBarCode2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.COD12")});
            this.xrBarCode2.LocationFloat = new DevExpress.Utils.PointFloat(197.0003F, 27.99991F);
            this.xrBarCode2.Name = "xrBarCode2";
            this.xrBarCode2.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode2.SizeF = new System.Drawing.SizeF(240.625F, 46.20501F);
            this.xrBarCode2.StylePriority.UseBorders = false;
            code39Generator2.CalcCheckSum = false;
            code39Generator2.WideNarrowRatio = 3F;
            this.xrBarCode2.Symbology = code39Generator2;
            // 
            // xrLabel4
            // 
            this.xrLabel4.AutoWidth = true;
            this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CSNOTICEID01", "帳單編號：{0}")});
            this.xrLabel4.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(272.0003F, 9.999939F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(165.625F, 18F);
            this.xrLabel4.StylePriority.UseBorders = false;
            this.xrLabel4.StylePriority.UseFont = false;
            // 
            // xrBarCode3
            // 
            this.xrBarCode3.AutoModule = true;
            this.xrBarCode3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrBarCode3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.COD13")});
            this.xrBarCode3.LocationFloat = new DevExpress.Utils.PointFloat(453.7148F, 27.99991F);
            this.xrBarCode3.Name = "xrBarCode3";
            this.xrBarCode3.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode3.SizeF = new System.Drawing.SizeF(251.3609F, 46.20501F);
            this.xrBarCode3.StylePriority.UseBorders = false;
            code39Generator3.CalcCheckSum = false;
            code39Generator3.WideNarrowRatio = 3F;
            this.xrBarCode3.Symbology = code39Generator3;
            // 
            // xrLabel5
            // 
            this.xrLabel5.AutoWidth = true;
            this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CSCUSID01", "用戶編號:{0}")});
            this.xrLabel5.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(457.094F, 9.999951F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(251.3609F, 18.00002F);
            this.xrLabel5.StylePriority.UseBorders = false;
            this.xrLabel5.StylePriority.UseFont = false;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1.5074945826601542D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrBarCode5,
            this.xrLabel8,
            this.xrLabel9,
            this.xrBarCode4,
            this.xrBarCode6,
            this.xrLabel10});
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Weight = 3D;
            // 
            // xrBarCode5
            // 
            this.xrBarCode5.AutoModule = true;
            this.xrBarCode5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrBarCode5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.COD22")});
            this.xrBarCode5.LocationFloat = new DevExpress.Utils.PointFloat(197.0003F, 23.79501F);
            this.xrBarCode5.Name = "xrBarCode5";
            this.xrBarCode5.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode5.SizeF = new System.Drawing.SizeF(240.625F, 46.20501F);
            this.xrBarCode5.StylePriority.UseBorders = false;
            code39Generator4.CalcCheckSum = false;
            code39Generator4.WideNarrowRatio = 3F;
            this.xrBarCode5.Symbology = code39Generator4;
            // 
            // xrLabel8
            // 
            this.xrLabel8.AutoWidth = true;
            this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.P02")});
            this.xrLabel8.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(4.681223F, 3.795105F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(249.2524F, 18F);
            this.xrLabel8.StylePriority.UseBorders = false;
            this.xrLabel8.StylePriority.UseFont = false;
            // 
            // xrLabel9
            // 
            this.xrLabel9.AutoWidth = true;
            this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CSNOTICEID02", "帳單編號：{0}")});
            this.xrLabel9.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(272.0003F, 5.795044F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(165.625F, 18F);
            this.xrLabel9.StylePriority.UseBorders = false;
            this.xrLabel9.StylePriority.UseFont = false;
            // 
            // xrBarCode4
            // 
            this.xrBarCode4.AutoModule = true;
            this.xrBarCode4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrBarCode4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.COD21")});
            this.xrBarCode4.LocationFloat = new DevExpress.Utils.PointFloat(10.41846F, 23.79508F);
            this.xrBarCode4.Name = "xrBarCode4";
            this.xrBarCode4.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode4.SizeF = new System.Drawing.SizeF(174.9999F, 46.20502F);
            this.xrBarCode4.StylePriority.UseBorders = false;
            code39Generator5.CalcCheckSum = false;
            code39Generator5.WideNarrowRatio = 3F;
            this.xrBarCode4.Symbology = code39Generator5;
            // 
            // xrBarCode6
            // 
            this.xrBarCode6.AutoModule = true;
            this.xrBarCode6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrBarCode6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.COD23")});
            this.xrBarCode6.LocationFloat = new DevExpress.Utils.PointFloat(453.7148F, 23.79501F);
            this.xrBarCode6.Name = "xrBarCode6";
            this.xrBarCode6.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode6.SizeF = new System.Drawing.SizeF(251.3609F, 46.20501F);
            this.xrBarCode6.StylePriority.UseBorders = false;
            code39Generator6.CalcCheckSum = false;
            code39Generator6.WideNarrowRatio = 3F;
            this.xrBarCode6.Symbology = code39Generator6;
            // 
            // xrLabel10
            // 
            this.xrLabel10.AutoWidth = true;
            this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CSCUSID02", "用戶編號:{0}")});
            this.xrLabel10.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(457.094F, 5.795045F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(251.3609F, 18.00002F);
            this.xrLabel10.StylePriority.UseBorders = false;
            this.xrLabel10.StylePriority.UseFont = false;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1.5074945826601542D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrBarCode7,
            this.xrLabel11,
            this.xrLabel12,
            this.xrBarCode8,
            this.xrBarCode9,
            this.xrLabel13});
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Weight = 3D;
            // 
            // xrBarCode7
            // 
            this.xrBarCode7.AutoModule = true;
            this.xrBarCode7.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrBarCode7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.COD31")});
            this.xrBarCode7.LocationFloat = new DevExpress.Utils.PointFloat(12.02041F, 25.89749F);
            this.xrBarCode7.Name = "xrBarCode7";
            this.xrBarCode7.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode7.SizeF = new System.Drawing.SizeF(174.9999F, 46.20502F);
            this.xrBarCode7.StylePriority.UseBorders = false;
            code39Generator7.CalcCheckSum = false;
            code39Generator7.WideNarrowRatio = 3F;
            this.xrBarCode7.Symbology = code39Generator7;
            // 
            // xrLabel11
            // 
            this.xrLabel11.AutoWidth = true;
            this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.P03")});
            this.xrLabel11.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(6.283174F, 4.897522F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(247.6505F, 18F);
            this.xrLabel11.StylePriority.UseBorders = false;
            this.xrLabel11.StylePriority.UseFont = false;
            // 
            // xrLabel12
            // 
            this.xrLabel12.AutoWidth = true;
            this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CSNOTICEID03", "帳單編號：{0}")});
            this.xrLabel12.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(272.0003F, 7.897522F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(167.2269F, 18F);
            this.xrLabel12.StylePriority.UseBorders = false;
            this.xrLabel12.StylePriority.UseFont = false;
            // 
            // xrBarCode8
            // 
            this.xrBarCode8.AutoModule = true;
            this.xrBarCode8.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrBarCode8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.COD32")});
            this.xrBarCode8.LocationFloat = new DevExpress.Utils.PointFloat(198.6022F, 25.89745F);
            this.xrBarCode8.Name = "xrBarCode8";
            this.xrBarCode8.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode8.SizeF = new System.Drawing.SizeF(240.625F, 46.20501F);
            this.xrBarCode8.StylePriority.UseBorders = false;
            code39Generator8.CalcCheckSum = false;
            code39Generator8.WideNarrowRatio = 3F;
            this.xrBarCode8.Symbology = code39Generator8;
            // 
            // xrBarCode9
            // 
            this.xrBarCode9.AutoModule = true;
            this.xrBarCode9.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrBarCode9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.COD33")});
            this.xrBarCode9.LocationFloat = new DevExpress.Utils.PointFloat(455.3168F, 25.89745F);
            this.xrBarCode9.Name = "xrBarCode9";
            this.xrBarCode9.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode9.SizeF = new System.Drawing.SizeF(251.3609F, 46.20501F);
            this.xrBarCode9.StylePriority.UseBorders = false;
            code39Generator9.CalcCheckSum = false;
            code39Generator9.WideNarrowRatio = 3F;
            this.xrBarCode9.Symbology = code39Generator9;
            // 
            // xrLabel13
            // 
            this.xrLabel13.AutoWidth = true;
            this.xrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CSCUSID03", "用戶編號:{0}")});
            this.xrLabel13.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(458.696F, 7.897491F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(251.3609F, 18.00002F);
            this.xrLabel13.StylePriority.UseBorders = false;
            this.xrLabel13.StylePriority.UseFont = false;
            // 
            // xrLabel68
            // 
            this.xrLabel68.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel68.LocationFloat = new DevExpress.Utils.PointFloat(7.807021F, 372.5417F);
            this.xrLabel68.Name = "xrLabel68";
            this.xrLabel68.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel68.SizeF = new System.Drawing.SizeF(706.9999F, 16F);
            this.xrLabel68.StylePriority.UseFont = false;
            this.xrLabel68.Text = "◎使用信用卡付費者，請填妥下面信用卡相關資料後，將此通知書傳真回本公司，並請您來電確認。";
            // 
            // xrLabel67
            // 
            this.xrLabel67.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel67.LocationFloat = new DevExpress.Utils.PointFloat(7.807021F, 353.5417F);
            this.xrLabel67.Name = "xrLabel67";
            this.xrLabel67.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel67.SizeF = new System.Drawing.SizeF(706.9999F, 16F);
            this.xrLabel67.StylePriority.UseFont = false;
            this.xrLabel67.Text = "◎採用便利商店代繳，請於繳費期限前，持本單至全省7-11,全家,萊爾富,OK等便利商店繳納。";
            // 
            // xrRichText2
            // 
            this.xrRichText2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
            this.xrRichText2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrRichText2.BorderWidth = 2F;
            this.xrRichText2.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrRichText2.LocationFloat = new DevExpress.Utils.PointFloat(7.807021F, 271.0834F);
            this.xrRichText2.Name = "xrRichText2";
            this.xrRichText2.SerializableRtfString = resources.GetString("xrRichText2.SerializableRtfString");
            this.xrRichText2.SizeF = new System.Drawing.SizeF(709.375F, 71.87498F);
            this.xrRichText2.StylePriority.UseBorderDashStyle = false;
            this.xrRichText2.StylePriority.UseBorders = false;
            this.xrRichText2.StylePriority.UseBorderWidth = false;
            this.xrRichText2.StylePriority.UseFont = false;
            // 
            // xrLabel66
            // 
            this.xrLabel66.AutoWidth = true;
            this.xrLabel66.BackColor = System.Drawing.Color.Black;
            this.xrLabel66.Font = new System.Drawing.Font("標楷體", 12F);
            this.xrLabel66.ForeColor = System.Drawing.Color.White;
            this.xrLabel66.LocationFloat = new DevExpress.Utils.PointFloat(310.2648F, 251.1667F);
            this.xrLabel66.Name = "xrLabel66";
            this.xrLabel66.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel66.SizeF = new System.Drawing.SizeF(87.71216F, 17.91667F);
            this.xrLabel66.StylePriority.UseBackColor = false;
            this.xrLabel66.StylePriority.UseFont = false;
            this.xrLabel66.StylePriority.UseForeColor = false;
            this.xrLabel66.Text = "重要訊息欄";
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(235.4167F, 46.875F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.AutoSize;
            // 
            // xrLabel53
            // 
            this.xrLabel53.AutoWidth = true;
            this.xrLabel53.Font = new System.Drawing.Font("標楷體", 18F, System.Drawing.FontStyle.Bold);
            this.xrLabel53.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.xrLabel53.LocationFloat = new DevExpress.Utils.PointFloat(237.5415F, 11.87501F);
            this.xrLabel53.Name = "xrLabel53";
            this.xrLabel53.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel53.SizeF = new System.Drawing.SizeF(283.6867F, 35F);
            this.xrLabel53.StyleName = "Title";
            this.xrLabel53.StylePriority.UseFont = false;
            this.xrLabel53.StylePriority.UseForeColor = false;
            this.xrLabel53.StylePriority.UseTextAlignment = false;
            this.xrLabel53.Text = "用戶網路續約繳款通知書";
            this.xrLabel53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrRichText1
            // 
            this.xrRichText1.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(546.0727F, 0F);
            this.xrRichText1.Name = "xrRichText1";
            this.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString");
            this.xrRichText1.SizeF = new System.Drawing.SizeF(180.4985F, 79.58334F);
            // 
            // xrLabel55
            // 
            this.xrLabel55.AutoWidth = true;
            this.xrLabel55.Font = new System.Drawing.Font("標楷體", 12F);
            this.xrLabel55.LocationFloat = new DevExpress.Utils.PointFloat(3.415021F, 107.7993F);
            this.xrLabel55.Name = "xrLabel55";
            this.xrLabel55.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel55.SizeF = new System.Drawing.SizeF(720.8064F, 23F);
            this.xrLabel55.StylePriority.UseFont = false;
            this.xrLabel55.Text = "到期，為避免影響上網權益，請於繳費期限前，選擇您方便的繳款方式前往繳納，謝謝您的合作！";
            // 
            // xrLabel54
            // 
            this.xrLabel54.AutoWidth = true;
            this.xrLabel54.Font = new System.Drawing.Font("標楷體", 12F);
            this.xrLabel54.LocationFloat = new DevExpress.Utils.PointFloat(47.10732F, 84.79926F);
            this.xrLabel54.Name = "xrLabel54";
            this.xrLabel54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel54.SizeF = new System.Drawing.SizeF(471.7715F, 23F);
            this.xrLabel54.StylePriority.UseFont = false;
            this.xrLabel54.Text = "感謝您對本公司產品的愛護與支持，提醒您的網路使用期限將於";
            // 
            // xrLabel43
            // 
            this.xrLabel43.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.duedat", "{0:yyyy\'年\'M\'月\'d\'日\'}")});
            this.xrLabel43.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(530.8788F, 84.79926F);
            this.xrLabel43.Name = "xrLabel43";
            this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel43.SizeF = new System.Drawing.SizeF(175.4543F, 18F);
            this.xrLabel43.StylePriority.UseFont = false;
            this.xrLabel43.Text = "xrLabel43";
            // 
            // xrLabel46
            // 
            this.xrLabel46.AutoWidth = true;
            this.xrLabel46.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.mydear")});
            this.xrLabel46.Font = new System.Drawing.Font("標楷體", 12F);
            this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(9.41502F, 63.67427F);
            this.xrLabel46.Name = "xrLabel46";
            this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel46.SizeF = new System.Drawing.SizeF(250.0741F, 21.125F);
            this.xrLabel46.StylePriority.UseFont = false;
            this.xrLabel46.Text = "xrLabel46";
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 19.54166F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 12F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = ".\\SQLEXPRESS_RTLibConnection";
            this.sqlDataSource1.Name = "sqlDataSource1";
            customSqlQuery1.Name = "Query";
            customSqlQuery1.Sql = resources.GetString("customSqlQuery1.Sql");
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.BorderColor = System.Drawing.Color.Black;
            this.Title.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Title.BorderWidth = 1F;
            this.Title.Font = new System.Drawing.Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold);
            this.Title.ForeColor = System.Drawing.Color.Maroon;
            this.Title.Name = "Title";
            // 
            // FieldCaption
            // 
            this.FieldCaption.BackColor = System.Drawing.Color.Transparent;
            this.FieldCaption.BorderColor = System.Drawing.Color.Black;
            this.FieldCaption.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.FieldCaption.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.FieldCaption.BorderWidth = 1F;
            this.FieldCaption.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.FieldCaption.ForeColor = System.Drawing.Color.Maroon;
            this.FieldCaption.Name = "FieldCaption";
            // 
            // PageInfo
            // 
            this.PageInfo.BackColor = System.Drawing.Color.Transparent;
            this.PageInfo.BorderColor = System.Drawing.Color.Black;
            this.PageInfo.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.PageInfo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.PageInfo.BorderWidth = 1F;
            this.PageInfo.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.PageInfo.ForeColor = System.Drawing.Color.Black;
            this.PageInfo.Name = "PageInfo";
            // 
            // DataField
            // 
            this.DataField.BackColor = System.Drawing.Color.Transparent;
            this.DataField.BorderColor = System.Drawing.Color.Black;
            this.DataField.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.DataField.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DataField.BorderWidth = 1F;
            this.DataField.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.DataField.ForeColor = System.Drawing.Color.Black;
            this.DataField.Name = "DataField";
            // 
            // RT3021R
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "Query";
            this.DataSource = this.sqlDataSource1;
            this.Margins = new System.Drawing.Printing.Margins(50, 50, 20, 12);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.FieldCaption,
            this.PageInfo,
            this.DataField});
            this.Version = "15.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
    }

    private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
    }

    private void xrLabel17_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        xrLabel17.Text = "網路使用期限：" + xrLabel17.Text + "  ～  ";
    }

    private void xrLabel18_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        xrLabel18.Text = "網路使用期限：" + xrLabel18.Text + "  ～  ";
    }

    private void xrLabel19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        xrLabel19.Text = "網路使用期限：" + xrLabel19.Text + "  ～  ";
    }
}
