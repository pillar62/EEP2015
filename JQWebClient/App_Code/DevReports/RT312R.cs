using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for RT312R
/// </summary>
public class RT312R : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRTable xrTable2;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell30;
    private XRTableCell xrTableCell32;
    private XRTableCell xrTableCell34;
    private XRTableCell xrTableCell36;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell40;
    private XRTableCell xrTableCell42;
    private XRTableCell xrTableCell44;
    private XRTableCell xrTableCell46;
    private XRTableCell xrTableCell48;
    private XRTableCell xrTableCell50;
    private XRTableCell xrTableCell52;
    private XRTableCell xrTableCell54;
    private XRTableCell xrTableCell56;
    private XRTableCell xrTableCell58;
    private XRTableCell xrTableCell60;
    private XRTableCell xrTableCell62;
    private XRTableCell xrTableCell64;
    private XRTableCell xrTableCell66;
    private XRTableCell xrTableCell68;
    private XRTableCell xrTableCell70;
    private XRTableCell xrTableCell72;
    private XRTableCell xrTableCell74;
    private XRTableCell xrTableCell76;
    private XRTableCell xrTableCell78;
    private XRTableCell xrTableCell80;
    private XRTableCell xrTableCell82;
    private XRTableCell xrTableCell84;
    private XRTableCell xrTableCell86;
    private XRTableCell xrTableCell88;
    private XRTableCell xrTableCell90;
    private XRTableCell xrTableCell92;
    private XRTableCell xrTableCell94;
    private XRTableCell xrTableCell96;
    private XRTableCell xrTableCell98;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private PageHeaderBand pageHeaderBand1;
    private XRTable xrTable1;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell29;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell41;
    private XRTableCell xrTableCell43;
    private XRTableCell xrTableCell45;
    private XRTableCell xrTableCell47;
    private XRTableCell xrTableCell49;
    private XRTableCell xrTableCell51;
    private XRTableCell xrTableCell53;
    private XRTableCell xrTableCell55;
    private XRTableCell xrTableCell57;
    private XRTableCell xrTableCell59;
    private XRTableCell xrTableCell61;
    private XRTableCell xrTableCell63;
    private XRTableCell xrTableCell65;
    private XRTableCell xrTableCell67;
    private XRTableCell xrTableCell69;
    private XRTableCell xrTableCell71;
    private XRTableCell xrTableCell73;
    private XRTableCell xrTableCell75;
    private XRTableCell xrTableCell77;
    private XRTableCell xrTableCell79;
    private XRTableCell xrTableCell81;
    private XRTableCell xrTableCell83;
    private XRTableCell xrTableCell85;
    private XRTableCell xrTableCell87;
    private XRTableCell xrTableCell89;
    private XRTableCell xrTableCell91;
    private XRTableCell xrTableCell93;
    private XRTableCell xrTableCell95;
    private XRTableCell xrTableCell97;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell3;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell6;
    private PageFooterBand pageFooterBand1;
    private XRPageInfo xrPageInfo1;
    private XRPageInfo xrPageInfo2;
    private ReportHeaderBand reportHeaderBand1;
    private XRLabel xrLabel1;
    private XRControlStyle Title;
    private XRControlStyle FieldCaption;
    private XRControlStyle PageInfo;
    private XRControlStyle DataField;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public RT312R()
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
            string resourceFileName = "RT312R.resx";
            System.Resources.ResourceManager resources = global::Resources.RT312R.ResourceManager;
            this.components = new System.ComponentModel.Container();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell54 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell64 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell68 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell72 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell74 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell76 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell78 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell80 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell82 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell84 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell86 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell88 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell90 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell92 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell94 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell96 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell98 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.pageHeaderBand1 = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell49 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell63 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell67 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell69 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell71 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell73 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell75 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell77 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell79 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell81 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell83 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell85 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell87 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell89 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell91 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell93 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell95 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell97 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.pageFooterBand1 = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.reportHeaderBand1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 23F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable2
            // 
            this.xrTable2.AnchorVertical = ((DevExpress.XtraReports.UI.VerticalAnchorStyles)((DevExpress.XtraReports.UI.VerticalAnchorStyles.Top | DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xrTable2.SizeF = new System.Drawing.SizeF(1438F, 23F);
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell8,
            this.xrTableCell10,
            this.xrTableCell12,
            this.xrTableCell14,
            this.xrTableCell16,
            this.xrTableCell18,
            this.xrTableCell20,
            this.xrTableCell22,
            this.xrTableCell24,
            this.xrTableCell26,
            this.xrTableCell28,
            this.xrTableCell30,
            this.xrTableCell32,
            this.xrTableCell34,
            this.xrTableCell36,
            this.xrTableCell38,
            this.xrTableCell40,
            this.xrTableCell42,
            this.xrTableCell44,
            this.xrTableCell46,
            this.xrTableCell48,
            this.xrTableCell50,
            this.xrTableCell52,
            this.xrTableCell54,
            this.xrTableCell56,
            this.xrTableCell58,
            this.xrTableCell60,
            this.xrTableCell62,
            this.xrTableCell64,
            this.xrTableCell66,
            this.xrTableCell68,
            this.xrTableCell70,
            this.xrTableCell72,
            this.xrTableCell74,
            this.xrTableCell76,
            this.xrTableCell78,
            this.xrTableCell80,
            this.xrTableCell82,
            this.xrTableCell84,
            this.xrTableCell86,
            this.xrTableCell88,
            this.xrTableCell90,
            this.xrTableCell92,
            this.xrTableCell94,
            this.xrTableCell96,
            this.xrTableCell98});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.CanGrow = false;
            this.xrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.applydat")});
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StyleName = "DataField";
            this.xrTableCell8.Text = "xrTableCell8";
            this.xrTableCell8.Weight = 0.69230769230769229D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.CanGrow = false;
            this.xrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.docketdat")});
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StyleName = "DataField";
            this.xrTableCell10.Text = "xrTableCell10";
            this.xrTableCell10.Weight = 0.80769230769230771D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.CanGrow = false;
            this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.applykind")});
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StyleName = "DataField";
            this.xrTableCell12.Text = "xrTableCell12";
            this.xrTableCell12.Weight = 0.80769230769230771D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.CanGrow = false;
            this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.updcode")});
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StyleName = "DataField";
            this.xrTableCell14.Text = "xrTableCell14";
            this.xrTableCell14.Weight = 0.69230769230769229D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.CanGrow = false;
            this.xrTableCell16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.updtel")});
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StyleName = "DataField";
            this.xrTableCell16.Text = "xrTableCell16";
            this.xrTableCell16.Weight = 0.5D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.CanGrow = false;
            this.xrTableCell18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.memberid")});
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StyleName = "DataField";
            this.xrTableCell18.Text = "xrTableCell18";
            this.xrTableCell18.Weight = 0.80769230769230771D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.CanGrow = false;
            this.xrTableCell20.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.mak_id")});
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StyleName = "DataField";
            this.xrTableCell20.Text = "xrTableCell20";
            this.xrTableCell20.Weight = 0.53846153846153844D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.CanGrow = false;
            this.xrTableCell22.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.sale_id")});
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.StyleName = "DataField";
            this.xrTableCell22.Text = "xrTableCell22";
            this.xrTableCell22.Weight = 0.53846153846153844D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.CanGrow = false;
            this.xrTableCell24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.cust_kind")});
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StyleName = "DataField";
            this.xrTableCell24.Text = "xrTableCell24";
            this.xrTableCell24.Weight = 0.73076923076923073D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.CanGrow = false;
            this.xrTableCell26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.company_name")});
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.StyleName = "DataField";
            this.xrTableCell26.Text = "xrTableCell26";
            this.xrTableCell26.Weight = 1.2692307692307692D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.CanGrow = false;
            this.xrTableCell28.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.coboss")});
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StyleName = "DataField";
            this.xrTableCell28.Text = "xrTableCell28";
            this.xrTableCell28.Weight = 0.57692307692307687D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.CanGrow = false;
            this.xrTableCell30.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.cobossid")});
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.StyleName = "DataField";
            this.xrTableCell30.Text = "xrTableCell30";
            this.xrTableCell30.Weight = 0.73076923076923073D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.CanGrow = false;
            this.xrTableCell32.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.company_id")});
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.StyleName = "DataField";
            this.xrTableCell32.Text = "xrTableCell32";
            this.xrTableCell32.Weight = 0.96153846153846156D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.CanGrow = false;
            this.xrTableCell34.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.case_no")});
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.StyleName = "DataField";
            this.xrTableCell34.Text = "xrTableCell34";
            this.xrTableCell34.Weight = 0.61538461538461542D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.CanGrow = false;
            this.xrTableCell36.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.cusnc")});
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.StyleName = "DataField";
            this.xrTableCell36.Text = "xrTableCell36";
            this.xrTableCell36.Weight = 0.5D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.CanGrow = false;
            this.xrTableCell38.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.codenc")});
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.StyleName = "DataField";
            this.xrTableCell38.Text = "xrTableCell38";
            this.xrTableCell38.Weight = 0.57692307692307687D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.CanGrow = false;
            this.xrTableCell40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.socialid")});
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.StyleName = "DataField";
            this.xrTableCell40.Text = "xrTableCell40";
            this.xrTableCell40.Weight = 0.61538461538461542D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.CanGrow = false;
            this.xrTableCell42.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.sex_KIND")});
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.StyleName = "DataField";
            this.xrTableCell42.Text = "xrTableCell42";
            this.xrTableCell42.Weight = 0.73076923076923073D;
            // 
            // xrTableCell44
            // 
            this.xrTableCell44.CanGrow = false;
            this.xrTableCell44.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.contact_name")});
            this.xrTableCell44.Name = "xrTableCell44";
            this.xrTableCell44.StyleName = "DataField";
            this.xrTableCell44.Text = "xrTableCell44";
            this.xrTableCell44.Weight = 1.1153846153846154D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.CanGrow = false;
            this.xrTableCell46.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.contact_tel")});
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.StyleName = "DataField";
            this.xrTableCell46.Text = "xrTableCell46";
            this.xrTableCell46.Weight = 0.84615384615384615D;
            // 
            // xrTableCell48
            // 
            this.xrTableCell48.CanGrow = false;
            this.xrTableCell48.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.contact_birth")});
            this.xrTableCell48.Name = "xrTableCell48";
            this.xrTableCell48.StyleName = "DataField";
            this.xrTableCell48.Text = "xrTableCell48";
            this.xrTableCell48.Weight = 1.0384615384615386D;
            // 
            // xrTableCell50
            // 
            this.xrTableCell50.CanGrow = false;
            this.xrTableCell50.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.contact_mobile")});
            this.xrTableCell50.Name = "xrTableCell50";
            this.xrTableCell50.StyleName = "DataField";
            this.xrTableCell50.Text = "xrTableCell50";
            this.xrTableCell50.Weight = 1.2307692307692308D;
            // 
            // xrTableCell52
            // 
            this.xrTableCell52.CanGrow = false;
            this.xrTableCell52.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.agent_cardtype")});
            this.xrTableCell52.Name = "xrTableCell52";
            this.xrTableCell52.StyleName = "DataField";
            this.xrTableCell52.Text = "xrTableCell52";
            this.xrTableCell52.Weight = 1.2307692307692308D;
            // 
            // xrTableCell54
            // 
            this.xrTableCell54.CanGrow = false;
            this.xrTableCell54.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.agent_idno")});
            this.xrTableCell54.Name = "xrTableCell54";
            this.xrTableCell54.StyleName = "DataField";
            this.xrTableCell54.Text = "xrTableCell54";
            this.xrTableCell54.Weight = 0.88461538461538458D;
            // 
            // xrTableCell56
            // 
            this.xrTableCell56.CanGrow = false;
            this.xrTableCell56.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.agent_callname")});
            this.xrTableCell56.Name = "xrTableCell56";
            this.xrTableCell56.StyleName = "DataField";
            this.xrTableCell56.Text = "xrTableCell56";
            this.xrTableCell56.Weight = 1.2307692307692308D;
            // 
            // xrTableCell58
            // 
            this.xrTableCell58.CanGrow = false;
            this.xrTableCell58.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.agent_name")});
            this.xrTableCell58.Name = "xrTableCell58";
            this.xrTableCell58.StyleName = "DataField";
            this.xrTableCell58.Text = "xrTableCell58";
            this.xrTableCell58.Weight = 0.96153846153846156D;
            // 
            // xrTableCell60
            // 
            this.xrTableCell60.CanGrow = false;
            this.xrTableCell60.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.agent_birth")});
            this.xrTableCell60.Name = "xrTableCell60";
            this.xrTableCell60.StyleName = "DataField";
            this.xrTableCell60.Text = "xrTableCell60";
            this.xrTableCell60.Weight = 0.88461538461538458D;
            // 
            // xrTableCell62
            // 
            this.xrTableCell62.CanGrow = false;
            this.xrTableCell62.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.zip3")});
            this.xrTableCell62.Name = "xrTableCell62";
            this.xrTableCell62.StyleName = "DataField";
            this.xrTableCell62.Text = "xrTableCell62";
            this.xrTableCell62.Weight = 0.34615384615384615D;
            // 
            // xrTableCell64
            // 
            this.xrTableCell64.CanGrow = false;
            this.xrTableCell64.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.cutnc3")});
            this.xrTableCell64.Name = "xrTableCell64";
            this.xrTableCell64.StyleName = "DataField";
            this.xrTableCell64.Text = "xrTableCell64";
            this.xrTableCell64.Weight = 0.53846153846153844D;
            // 
            // xrTableCell66
            // 
            this.xrTableCell66.CanGrow = false;
            this.xrTableCell66.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.township3")});
            this.xrTableCell66.Name = "xrTableCell66";
            this.xrTableCell66.StyleName = "DataField";
            this.xrTableCell66.Text = "xrTableCell66";
            this.xrTableCell66.Weight = 0.84615384615384615D;
            // 
            // xrTableCell68
            // 
            this.xrTableCell68.CanGrow = false;
            this.xrTableCell68.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.raddr3")});
            this.xrTableCell68.Name = "xrTableCell68";
            this.xrTableCell68.StyleName = "DataField";
            this.xrTableCell68.Text = "xrTableCell68";
            this.xrTableCell68.Weight = 0.53846153846153844D;
            // 
            // xrTableCell70
            // 
            this.xrTableCell70.CanGrow = false;
            this.xrTableCell70.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.zip2")});
            this.xrTableCell70.Name = "xrTableCell70";
            this.xrTableCell70.StyleName = "DataField";
            this.xrTableCell70.Text = "xrTableCell70";
            this.xrTableCell70.Weight = 0.34615384615384615D;
            // 
            // xrTableCell72
            // 
            this.xrTableCell72.CanGrow = false;
            this.xrTableCell72.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.cutnc2")});
            this.xrTableCell72.Name = "xrTableCell72";
            this.xrTableCell72.StyleName = "DataField";
            this.xrTableCell72.Text = "xrTableCell72";
            this.xrTableCell72.Weight = 0.53846153846153844D;
            // 
            // xrTableCell74
            // 
            this.xrTableCell74.CanGrow = false;
            this.xrTableCell74.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.township2")});
            this.xrTableCell74.Name = "xrTableCell74";
            this.xrTableCell74.StyleName = "DataField";
            this.xrTableCell74.Text = "xrTableCell74";
            this.xrTableCell74.Weight = 0.84615384615384615D;
            // 
            // xrTableCell76
            // 
            this.xrTableCell76.CanGrow = false;
            this.xrTableCell76.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.raddr2")});
            this.xrTableCell76.Name = "xrTableCell76";
            this.xrTableCell76.StyleName = "DataField";
            this.xrTableCell76.Text = "xrTableCell76";
            this.xrTableCell76.Weight = 0.53846153846153844D;
            // 
            // xrTableCell78
            // 
            this.xrTableCell78.CanGrow = false;
            this.xrTableCell78.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.zip1")});
            this.xrTableCell78.Name = "xrTableCell78";
            this.xrTableCell78.StyleName = "DataField";
            this.xrTableCell78.Text = "xrTableCell78";
            this.xrTableCell78.Weight = 0.34615384615384615D;
            // 
            // xrTableCell80
            // 
            this.xrTableCell80.CanGrow = false;
            this.xrTableCell80.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.cutnc1")});
            this.xrTableCell80.Name = "xrTableCell80";
            this.xrTableCell80.StyleName = "DataField";
            this.xrTableCell80.Text = "xrTableCell80";
            this.xrTableCell80.Weight = 0.53846153846153844D;
            // 
            // xrTableCell82
            // 
            this.xrTableCell82.CanGrow = false;
            this.xrTableCell82.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.township1")});
            this.xrTableCell82.Name = "xrTableCell82";
            this.xrTableCell82.StyleName = "DataField";
            this.xrTableCell82.Text = "xrTableCell82";
            this.xrTableCell82.Weight = 0.84615384615384615D;
            // 
            // xrTableCell84
            // 
            this.xrTableCell84.CanGrow = false;
            this.xrTableCell84.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.raddr1")});
            this.xrTableCell84.Name = "xrTableCell84";
            this.xrTableCell84.StyleName = "DataField";
            this.xrTableCell84.Text = "xrTableCell84";
            this.xrTableCell84.Weight = 0.53846153846153844D;
            // 
            // xrTableCell86
            // 
            this.xrTableCell86.CanGrow = false;
            this.xrTableCell86.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.comn")});
            this.xrTableCell86.Name = "xrTableCell86";
            this.xrTableCell86.StyleName = "DataField";
            this.xrTableCell86.Text = "xrTableCell86";
            this.xrTableCell86.Weight = 0.46153846153846156D;
            // 
            // xrTableCell88
            // 
            this.xrTableCell88.CanGrow = false;
            this.xrTableCell88.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.ip11s")});
            this.xrTableCell88.Name = "xrTableCell88";
            this.xrTableCell88.StyleName = "DataField";
            this.xrTableCell88.Text = "xrTableCell88";
            this.xrTableCell88.Weight = 0.42307692307692307D;
            // 
            // xrTableCell90
            // 
            this.xrTableCell90.CanGrow = false;
            this.xrTableCell90.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.ip11e")});
            this.xrTableCell90.Name = "xrTableCell90";
            this.xrTableCell90.StyleName = "DataField";
            this.xrTableCell90.Text = "xrTableCell90";
            this.xrTableCell90.Weight = 0.42307692307692307D;
            // 
            // xrTableCell92
            // 
            this.xrTableCell92.CanGrow = false;
            this.xrTableCell92.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.ncicdate")});
            this.xrTableCell92.Name = "xrTableCell92";
            this.xrTableCell92.StyleName = "DataField";
            this.xrTableCell92.Text = "xrTableCell92";
            this.xrTableCell92.Weight = 0.69230769230769229D;
            // 
            // xrTableCell94
            // 
            this.xrTableCell94.CanGrow = false;
            this.xrTableCell94.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.codenc2")});
            this.xrTableCell94.Name = "xrTableCell94";
            this.xrTableCell94.StyleName = "DataField";
            this.xrTableCell94.Text = "xrTableCell94";
            this.xrTableCell94.Weight = 0.69230769230769229D;
            // 
            // xrTableCell96
            // 
            this.xrTableCell96.CanGrow = false;
            this.xrTableCell96.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.secondno")});
            this.xrTableCell96.Name = "xrTableCell96";
            this.xrTableCell96.StyleName = "DataField";
            this.xrTableCell96.Text = "xrTableCell96";
            this.xrTableCell96.Weight = 0.80769230769230771D;
            // 
            // xrTableCell98
            // 
            this.xrTableCell98.CanGrow = false;
            this.xrTableCell98.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.apply_no")});
            this.xrTableCell98.Name = "xrTableCell98";
            this.xrTableCell98.StyleName = "DataField";
            this.xrTableCell98.Text = "xrTableCell98";
            this.xrTableCell98.Weight = 0.69230769230769229D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 100F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 100F;
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
            // pageHeaderBand1
            // 
            this.pageHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.pageHeaderBand1.HeightF = 42.00001F;
            this.pageHeaderBand1.Name = "pageHeaderBand1";
            // 
            // xrTable1
            // 
            this.xrTable1.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 6.00001F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
            this.xrTable1.SizeF = new System.Drawing.SizeF(1438F, 36F);
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell9,
            this.xrTableCell11,
            this.xrTableCell13,
            this.xrTableCell15,
            this.xrTableCell17,
            this.xrTableCell19,
            this.xrTableCell21,
            this.xrTableCell23,
            this.xrTableCell25,
            this.xrTableCell27,
            this.xrTableCell29,
            this.xrTableCell31,
            this.xrTableCell33,
            this.xrTableCell35,
            this.xrTableCell37,
            this.xrTableCell39,
            this.xrTableCell41,
            this.xrTableCell43,
            this.xrTableCell45,
            this.xrTableCell47,
            this.xrTableCell49,
            this.xrTableCell51,
            this.xrTableCell53,
            this.xrTableCell55,
            this.xrTableCell57,
            this.xrTableCell59,
            this.xrTableCell61,
            this.xrTableCell63,
            this.xrTableCell65,
            this.xrTableCell67,
            this.xrTableCell69,
            this.xrTableCell71,
            this.xrTableCell73,
            this.xrTableCell75,
            this.xrTableCell77,
            this.xrTableCell79,
            this.xrTableCell81,
            this.xrTableCell83,
            this.xrTableCell85,
            this.xrTableCell87,
            this.xrTableCell89,
            this.xrTableCell91,
            this.xrTableCell93,
            this.xrTableCell95,
            this.xrTableCell97});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.CanGrow = false;
            this.xrTableCell7.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StyleName = "FieldCaption";
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.Text = "流水號";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell7.Weight = 0.69230769230769229D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.CanGrow = false;
            this.xrTableCell9.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StyleName = "FieldCaption";
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.Text = "申請日期";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell9.Weight = 0.80769230769230771D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.CanGrow = false;
            this.xrTableCell11.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StyleName = "FieldCaption";
            this.xrTableCell11.StylePriority.UseFont = false;
            this.xrTableCell11.Text = "申請種類";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell11.Weight = 0.80769230769230771D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.CanGrow = false;
            this.xrTableCell13.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StyleName = "FieldCaption";
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.Text = "異動代碼";
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell13.Weight = 0.69230769230769229D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.CanGrow = false;
            this.xrTableCell15.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StyleName = "FieldCaption";
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.Text = "電話號碼";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell15.Weight = 0.5D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.CanGrow = false;
            this.xrTableCell17.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.StyleName = "FieldCaption";
            this.xrTableCell17.StylePriority.UseFont = false;
            this.xrTableCell17.Text = "對帳號碼";
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell17.Weight = 0.80769230769230771D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.CanGrow = false;
            this.xrTableCell19.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StyleName = "FieldCaption";
            this.xrTableCell19.StylePriority.UseFont = false;
            this.xrTableCell19.Text = "協力商代碼";
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell19.Weight = 0.53846153846153844D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.CanGrow = false;
            this.xrTableCell21.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StyleName = "FieldCaption";
            this.xrTableCell21.StylePriority.UseFont = false;
            this.xrTableCell21.Text = "業務員代碼";
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell21.Weight = 0.53846153846153844D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.CanGrow = false;
            this.xrTableCell23.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.StyleName = "FieldCaption";
            this.xrTableCell23.StylePriority.UseFont = false;
            this.xrTableCell23.Text = "客戶類別";
            this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell23.Weight = 0.73076923076923073D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.CanGrow = false;
            this.xrTableCell25.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.StyleName = "FieldCaption";
            this.xrTableCell25.StylePriority.UseFont = false;
            this.xrTableCell25.Text = "公司名稱";
            this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell25.Weight = 1.2692307692307692D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.CanGrow = false;
            this.xrTableCell27.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.StyleName = "FieldCaption";
            this.xrTableCell27.StylePriority.UseFont = false;
            this.xrTableCell27.Text = "公司負責人";
            this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell27.Weight = 0.57692307692307687D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.CanGrow = false;
            this.xrTableCell29.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.StyleName = "FieldCaption";
            this.xrTableCell29.StylePriority.UseFont = false;
            this.xrTableCell29.Text = "負責人身份證字號";
            this.xrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell29.Weight = 0.73076923076923073D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.CanGrow = false;
            this.xrTableCell31.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.StyleName = "FieldCaption";
            this.xrTableCell31.StylePriority.UseFont = false;
            this.xrTableCell31.Text = "公司統編";
            this.xrTableCell31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell31.Weight = 0.96153846153846156D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.CanGrow = false;
            this.xrTableCell33.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.StyleName = "FieldCaption";
            this.xrTableCell33.StylePriority.UseFont = false;
            this.xrTableCell33.Text = "服務方案";
            this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell33.Weight = 0.61538461538461542D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.CanGrow = false;
            this.xrTableCell35.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.StyleName = "FieldCaption";
            this.xrTableCell35.StylePriority.UseFont = false;
            this.xrTableCell35.Text = "優惠代碼";
            this.xrTableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell35.Weight = 0.5D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.CanGrow = false;
            this.xrTableCell37.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.StyleName = "FieldCaption";
            this.xrTableCell37.StylePriority.UseFont = false;
            this.xrTableCell37.Text = "用戶名稱";
            this.xrTableCell37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell37.Weight = 0.57692307692307687D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.CanGrow = false;
            this.xrTableCell39.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.StyleName = "FieldCaption";
            this.xrTableCell39.StylePriority.UseFont = false;
            this.xrTableCell39.Text = "聯絡人證照類別";
            this.xrTableCell39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell39.Weight = 0.61538461538461542D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.CanGrow = false;
            this.xrTableCell41.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.StyleName = "FieldCaption";
            this.xrTableCell41.StylePriority.UseFont = false;
            this.xrTableCell41.Text = "聯絡人稱謂";
            this.xrTableCell41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell41.Weight = 0.73076923076923073D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.CanGrow = false;
            this.xrTableCell43.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.StyleName = "FieldCaption";
            this.xrTableCell43.StylePriority.UseFont = false;
            this.xrTableCell43.Text = "聯絡人名稱";
            this.xrTableCell43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell43.Weight = 1.1153846153846154D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.CanGrow = false;
            this.xrTableCell45.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.StyleName = "FieldCaption";
            this.xrTableCell45.StylePriority.UseFont = false;
            this.xrTableCell45.Text = "聯絡人聯絡電話";
            this.xrTableCell45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell45.Weight = 0.84615384615384615D;
            // 
            // xrTableCell47
            // 
            this.xrTableCell47.CanGrow = false;
            this.xrTableCell47.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell47.Name = "xrTableCell47";
            this.xrTableCell47.StyleName = "FieldCaption";
            this.xrTableCell47.StylePriority.UseFont = false;
            this.xrTableCell47.Text = "聯絡人出生日期";
            this.xrTableCell47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell47.Weight = 1.0384615384615386D;
            // 
            // xrTableCell49
            // 
            this.xrTableCell49.CanGrow = false;
            this.xrTableCell49.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell49.Name = "xrTableCell49";
            this.xrTableCell49.StyleName = "FieldCaption";
            this.xrTableCell49.StylePriority.UseFont = false;
            this.xrTableCell49.Text = "聯絡人行動電話";
            this.xrTableCell49.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell49.Weight = 1.2307692307692308D;
            // 
            // xrTableCell51
            // 
            this.xrTableCell51.CanGrow = false;
            this.xrTableCell51.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell51.Name = "xrTableCell51";
            this.xrTableCell51.StyleName = "FieldCaption";
            this.xrTableCell51.StylePriority.UseFont = false;
            this.xrTableCell51.Text = "代理人證照類別";
            this.xrTableCell51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell51.Weight = 1.2307692307692308D;
            // 
            // xrTableCell53
            // 
            this.xrTableCell53.CanGrow = false;
            this.xrTableCell53.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell53.Name = "xrTableCell53";
            this.xrTableCell53.StyleName = "FieldCaption";
            this.xrTableCell53.StylePriority.UseFont = false;
            this.xrTableCell53.Text = "代理人證照號碼";
            this.xrTableCell53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell53.Weight = 0.88461538461538458D;
            // 
            // xrTableCell55
            // 
            this.xrTableCell55.CanGrow = false;
            this.xrTableCell55.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell55.Name = "xrTableCell55";
            this.xrTableCell55.StyleName = "FieldCaption";
            this.xrTableCell55.StylePriority.UseFont = false;
            this.xrTableCell55.Text = "代理人稱謂";
            this.xrTableCell55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell55.Weight = 1.2307692307692308D;
            // 
            // xrTableCell57
            // 
            this.xrTableCell57.CanGrow = false;
            this.xrTableCell57.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell57.Name = "xrTableCell57";
            this.xrTableCell57.StyleName = "FieldCaption";
            this.xrTableCell57.StylePriority.UseFont = false;
            this.xrTableCell57.Text = "代理人名稱";
            this.xrTableCell57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell57.Weight = 0.96153846153846156D;
            // 
            // xrTableCell59
            // 
            this.xrTableCell59.CanGrow = false;
            this.xrTableCell59.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell59.Name = "xrTableCell59";
            this.xrTableCell59.StyleName = "FieldCaption";
            this.xrTableCell59.StylePriority.UseFont = false;
            this.xrTableCell59.Text = "代理人出生日期";
            this.xrTableCell59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell59.Weight = 0.88461538461538458D;
            // 
            // xrTableCell61
            // 
            this.xrTableCell61.CanGrow = false;
            this.xrTableCell61.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell61.Name = "xrTableCell61";
            this.xrTableCell61.StyleName = "FieldCaption";
            this.xrTableCell61.StylePriority.UseFont = false;
            this.xrTableCell61.Text = "帳寄郵遞區號";
            this.xrTableCell61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell61.Weight = 0.34615384615384615D;
            // 
            // xrTableCell63
            // 
            this.xrTableCell63.CanGrow = false;
            this.xrTableCell63.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell63.Name = "xrTableCell63";
            this.xrTableCell63.StyleName = "FieldCaption";
            this.xrTableCell63.StylePriority.UseFont = false;
            this.xrTableCell63.Text = "帳寄縣市";
            this.xrTableCell63.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell63.Weight = 0.53846153846153844D;
            // 
            // xrTableCell65
            // 
            this.xrTableCell65.CanGrow = false;
            this.xrTableCell65.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell65.Name = "xrTableCell65";
            this.xrTableCell65.StyleName = "FieldCaption";
            this.xrTableCell65.StylePriority.UseFont = false;
            this.xrTableCell65.Text = "帳寄鄉鎮市區";
            this.xrTableCell65.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell65.Weight = 0.84615384615384615D;
            // 
            // xrTableCell67
            // 
            this.xrTableCell67.CanGrow = false;
            this.xrTableCell67.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell67.Name = "xrTableCell67";
            this.xrTableCell67.StyleName = "FieldCaption";
            this.xrTableCell67.StylePriority.UseFont = false;
            this.xrTableCell67.Text = "帳寄地址";
            this.xrTableCell67.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell67.Weight = 0.53846153846153844D;
            // 
            // xrTableCell69
            // 
            this.xrTableCell69.CanGrow = false;
            this.xrTableCell69.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell69.Name = "xrTableCell69";
            this.xrTableCell69.StyleName = "FieldCaption";
            this.xrTableCell69.StylePriority.UseFont = false;
            this.xrTableCell69.Text = "戶籍郵遞區號";
            this.xrTableCell69.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell69.Weight = 0.34615384615384615D;
            // 
            // xrTableCell71
            // 
            this.xrTableCell71.CanGrow = false;
            this.xrTableCell71.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell71.Name = "xrTableCell71";
            this.xrTableCell71.StyleName = "FieldCaption";
            this.xrTableCell71.StylePriority.UseFont = false;
            this.xrTableCell71.Text = "戶籍縣市";
            this.xrTableCell71.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell71.Weight = 0.53846153846153844D;
            // 
            // xrTableCell73
            // 
            this.xrTableCell73.CanGrow = false;
            this.xrTableCell73.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell73.Name = "xrTableCell73";
            this.xrTableCell73.StyleName = "FieldCaption";
            this.xrTableCell73.StylePriority.UseFont = false;
            this.xrTableCell73.Text = "戶籍鄉鎮市區";
            this.xrTableCell73.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell73.Weight = 0.84615384615384615D;
            // 
            // xrTableCell75
            // 
            this.xrTableCell75.CanGrow = false;
            this.xrTableCell75.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell75.Name = "xrTableCell75";
            this.xrTableCell75.StyleName = "FieldCaption";
            this.xrTableCell75.StylePriority.UseFont = false;
            this.xrTableCell75.Text = "戶籍地址";
            this.xrTableCell75.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell75.Weight = 0.53846153846153844D;
            // 
            // xrTableCell77
            // 
            this.xrTableCell77.CanGrow = false;
            this.xrTableCell77.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell77.Name = "xrTableCell77";
            this.xrTableCell77.StyleName = "FieldCaption";
            this.xrTableCell77.StylePriority.UseFont = false;
            this.xrTableCell77.Text = "裝機郵遞區號";
            this.xrTableCell77.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell77.Weight = 0.34615384615384615D;
            // 
            // xrTableCell79
            // 
            this.xrTableCell79.CanGrow = false;
            this.xrTableCell79.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell79.Name = "xrTableCell79";
            this.xrTableCell79.StyleName = "FieldCaption";
            this.xrTableCell79.StylePriority.UseFont = false;
            this.xrTableCell79.Text = "裝機縣市";
            this.xrTableCell79.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell79.Weight = 0.53846153846153844D;
            // 
            // xrTableCell81
            // 
            this.xrTableCell81.CanGrow = false;
            this.xrTableCell81.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell81.Name = "xrTableCell81";
            this.xrTableCell81.StyleName = "FieldCaption";
            this.xrTableCell81.StylePriority.UseFont = false;
            this.xrTableCell81.Text = "裝機鄉鎮市區";
            this.xrTableCell81.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell81.Weight = 0.84615384615384615D;
            // 
            // xrTableCell83
            // 
            this.xrTableCell83.CanGrow = false;
            this.xrTableCell83.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell83.Name = "xrTableCell83";
            this.xrTableCell83.StyleName = "FieldCaption";
            this.xrTableCell83.StylePriority.UseFont = false;
            this.xrTableCell83.Text = "裝機地址";
            this.xrTableCell83.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell83.Weight = 0.53846153846153844D;
            // 
            // xrTableCell85
            // 
            this.xrTableCell85.CanGrow = false;
            this.xrTableCell85.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell85.Name = "xrTableCell85";
            this.xrTableCell85.StyleName = "FieldCaption";
            this.xrTableCell85.StylePriority.UseFont = false;
            this.xrTableCell85.Text = "社區名稱";
            this.xrTableCell85.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell85.Weight = 0.46153846153846156D;
            // 
            // xrTableCell87
            // 
            this.xrTableCell87.CanGrow = false;
            this.xrTableCell87.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell87.Name = "xrTableCell87";
            this.xrTableCell87.StyleName = "FieldCaption";
            this.xrTableCell87.StylePriority.UseFont = false;
            this.xrTableCell87.Text = "IP ADDRESS FROM";
            this.xrTableCell87.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell87.Weight = 0.42307692307692307D;
            // 
            // xrTableCell89
            // 
            this.xrTableCell89.CanGrow = false;
            this.xrTableCell89.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell89.Name = "xrTableCell89";
            this.xrTableCell89.StyleName = "FieldCaption";
            this.xrTableCell89.StylePriority.UseFont = false;
            this.xrTableCell89.Text = "IP ADDRESS END";
            this.xrTableCell89.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell89.Weight = 0.42307692307692307D;
            // 
            // xrTableCell91
            // 
            this.xrTableCell91.CanGrow = false;
            this.xrTableCell91.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell91.Name = "xrTableCell91";
            this.xrTableCell91.StyleName = "FieldCaption";
            this.xrTableCell91.StylePriority.UseFont = false;
            this.xrTableCell91.Text = "NCIC預處理日期";
            this.xrTableCell91.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell91.Weight = 0.69230769230769229D;
            // 
            // xrTableCell93
            // 
            this.xrTableCell93.CanGrow = false;
            this.xrTableCell93.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell93.Name = "xrTableCell93";
            this.xrTableCell93.StyleName = "FieldCaption";
            this.xrTableCell93.StylePriority.UseFont = false;
            this.xrTableCell93.Text = "第二證照類別";
            this.xrTableCell93.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell93.Weight = 0.69230769230769229D;
            // 
            // xrTableCell95
            // 
            this.xrTableCell95.CanGrow = false;
            this.xrTableCell95.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell95.Name = "xrTableCell95";
            this.xrTableCell95.StyleName = "FieldCaption";
            this.xrTableCell95.StylePriority.UseFont = false;
            this.xrTableCell95.Text = "第二證照號碼";
            this.xrTableCell95.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell95.Weight = 0.80769230769230771D;
            // 
            // xrTableCell97
            // 
            this.xrTableCell97.CanGrow = false;
            this.xrTableCell97.Font = new System.Drawing.Font("新細明體", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell97.Name = "xrTableCell97";
            this.xrTableCell97.StyleName = "FieldCaption";
            this.xrTableCell97.StylePriority.UseFont = false;
            this.xrTableCell97.Text = "申請書編號";
            this.xrTableCell97.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell97.Weight = 0.69230769230769229D;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "xrTableCell1";
            this.xrTableCell1.Weight = 1D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Text = "xrTableCell2";
            this.xrTableCell2.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Text = "xrTableCell3";
            this.xrTableCell3.Weight = 1D;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Text = "xrTableCell4";
            this.xrTableCell4.Weight = 1D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Text = "xrTableCell5";
            this.xrTableCell5.Weight = 1D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Text = "xrTableCell6";
            this.xrTableCell6.Weight = 1D;
            // 
            // pageFooterBand1
            // 
            this.pageFooterBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrPageInfo2});
            this.pageFooterBand1.HeightF = 29.00001F;
            this.pageFooterBand1.Name = "pageFooterBand1";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("新細明體", 10F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(6F, 6F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(438F, 23F);
            this.xrPageInfo1.StyleName = "PageInfo";
            this.xrPageInfo1.StylePriority.UseFont = false;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Format = "Page {0} of {1}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(1006F, 6.00001F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(438F, 23F);
            this.xrPageInfo2.StyleName = "PageInfo";
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // reportHeaderBand1
            // 
            this.reportHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.reportHeaderBand1.HeightF = 53F;
            this.reportHeaderBand1.Name = "reportHeaderBand1";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("新細明體", 20F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(6F, 6F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(888F, 35F);
            this.xrLabel1.StyleName = "Title";
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "遠傳大寬頻報竣客戶一覽表";
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
            this.FieldCaption.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
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
            // RT312R
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.pageHeaderBand1,
            this.pageFooterBand1,
            this.reportHeaderBand1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "Query";
            this.DataSource = this.sqlDataSource1;
            this.Landscape = true;
            this.PageHeight = 1169;
            this.PageWidth = 1654;
            this.PaperKind = System.Drawing.Printing.PaperKind.A3;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.FieldCaption,
            this.PageInfo,
            this.DataField});
            this.Version = "15.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
