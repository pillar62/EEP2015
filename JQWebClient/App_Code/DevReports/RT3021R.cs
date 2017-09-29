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
    private XRLine xrLine21;
    private XRLine xrLine12;
    private XRLabel xrLabel69;
    private XRLine xrLine13;
    private XRLine xrLine14;
    private XRLine xrLine15;
    private XRLine xrLine16;
    private XRLine xrLine17;
    private XRLabel xrLabel70;
    private XRLine xrLine18;
    private XRLabel xrLabel71;
    private XRLabel xrLabel72;
    private XRLabel xrLabel73;
    private XRLabel xrLabel76;
    private XRLabel xrLabel77;
    private XRLine xrLine27;
    private XRLine xrLine26;
    private XRLabel xrLabel6;
    private XRLine xrLine20;
    private XRLine xrLine23;
    private XRLine xrLine25;
    private XRShape xrShape1;
    private XRRichText xrRichText3;
    private XRLine xrLine24;
    private XRLine xrLine1;
    private XRLine xrLine28;
    private XRLine xrLine19;
    private XRLine xrLine22;
    private XRLine xrLine29;
    private XRLine xrLine31;
    private XRLine xrLine32;
    private XRLabel xrLabel1;
    private XRLabel xrLabel2;
    private GroupHeaderBand GroupHeader1;
    private XRLine xrLine2;
    private XRLabel xrLabel46;
    private XRLabel xrLabel43;
    private XRLabel xrLabel40;
    private XRLabel xrLabel54;
    private XRLabel xrLabel55;
    private XRLine xrLine3;
    private XRLine xrLine4;
    private XRLine xrLine5;
    private XRLine xrLine6;
    private XRLine xrLine7;
    private XRLabel xrLabel56;
    private XRLine xrLine9;
    private XRLine xrLine10;
    private XRLine xrLine11;
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
    private GroupFooterBand GroupFooter1;
    private XRLabel xrLabel4;
    private XRBarCode xrBarCode1;
    private XRLabel xrLabel3;
    private XRLabel xrLabel5;
    private XRBarCode xrBarCode3;
    private XRBarCode xrBarCode2;
    private XRLine xrLine38;
    private XRLine xrLine39;
    private XRLine xrLine40;
    private XRLine xrLine41;
    private XRLine xrLine37;
    private XRLine xrLine36;
    private XRLine xrLine35;
    private XRLine xrLine8;
    private XRLine xrLine33;
    private XRLine xrLine34;
    private XRLine xrLine30;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

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
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.XtraPrinting.Shape.ShapeRectangle shapeRectangle1 = new DevExpress.XtraPrinting.Shape.ShapeRectangle();
            DevExpress.XtraPrinting.BarCode.Code39Generator code39Generator3 = new DevExpress.XtraPrinting.BarCode.Code39Generator();
            DevExpress.XtraPrinting.BarCode.Code39Generator code39Generator2 = new DevExpress.XtraPrinting.BarCode.Code39Generator();
            DevExpress.XtraPrinting.BarCode.Code39Generator code39Generator1 = new DevExpress.XtraPrinting.BarCode.Code39Generator();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.xrLabel53 = new DevExpress.XtraReports.UI.XRLabel();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrLine12 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel69 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine13 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine14 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine15 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine16 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine17 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel70 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine18 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel71 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel72 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel73 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel76 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel77 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine21 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine20 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine23 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine25 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine26 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine27 = new DevExpress.XtraReports.UI.XRLine();
            this.xrRichText3 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrShape1 = new DevExpress.XtraReports.UI.XRShape();
            this.xrLine19 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine22 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine28 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine29 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine31 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine32 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel54 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel55 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine5 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine6 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine7 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel56 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine9 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine10 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine11 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel57 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel58 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel59 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel60 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel61 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel62 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel63 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel64 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel65 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel66 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrRichText2 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrLabel67 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel68 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine24 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrBarCode2 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrBarCode3 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine30 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine33 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine34 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine8 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine35 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine36 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine37 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine38 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine39 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine40 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine41 = new DevExpress.XtraReports.UI.XRLine();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.xrBarCode3,
            this.xrBarCode2,
            this.xrLabel4,
            this.xrBarCode1,
            this.xrLabel3,
            this.xrLine24,
            this.xrLine1,
            this.xrLine28});
            this.Detail.HeightF = 70.20834F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StyleName = "DataField";
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
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
            // xrLabel53
            // 
            this.xrLabel53.AutoWidth = true;
            this.xrLabel53.Font = new System.Drawing.Font("標楷體", 18F, System.Drawing.FontStyle.Bold);
            this.xrLabel53.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.xrLabel53.LocationFloat = new DevExpress.Utils.PointFloat(236.4584F, 10.00001F);
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
            // xrPictureBox1
            // 
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(1.04173F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(235.4167F, 46.875F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.AutoSize;
            // 
            // xrRichText1
            // 
            this.xrRichText1.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(547.5432F, 0F);
            this.xrRichText1.Name = "xrRichText1";
            this.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString");
            this.xrRichText1.SizeF = new System.Drawing.SizeF(180.4985F, 79.58334F);
            // 
            // xrLine12
            // 
            this.xrLine12.LocationFloat = new DevExpress.Utils.PointFloat(3.320853F, 397.5F);
            this.xrLine12.Name = "xrLine12";
            this.xrLine12.SizeF = new System.Drawing.SizeF(722.1667F, 2.083336F);
            // 
            // xrLabel69
            // 
            this.xrLabel69.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CSCUSID")});
            this.xrLabel69.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel69.LocationFloat = new DevExpress.Utils.PointFloat(97.95866F, 428.5833F);
            this.xrLabel69.Name = "xrLabel69";
            this.xrLabel69.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel69.SizeF = new System.Drawing.SizeF(609.3219F, 18F);
            this.xrLabel69.StylePriority.UseFont = false;
            this.xrLabel69.Text = "xrLabel40";
            // 
            // xrLine13
            // 
            this.xrLine13.LocationFloat = new DevExpress.Utils.PointFloat(3.320853F, 477.5F);
            this.xrLine13.Name = "xrLine13";
            this.xrLine13.SizeF = new System.Drawing.SizeF(722.1667F, 2.083336F);
            // 
            // xrLine14
            // 
            this.xrLine14.LocationFloat = new DevExpress.Utils.PointFloat(3.320853F, 422.4999F);
            this.xrLine14.Name = "xrLine14";
            this.xrLine14.SizeF = new System.Drawing.SizeF(722.1667F, 2.083336F);
            // 
            // xrLine15
            // 
            this.xrLine15.LocationFloat = new DevExpress.Utils.PointFloat(3.320853F, 449.5F);
            this.xrLine15.Name = "xrLine15";
            this.xrLine15.SizeF = new System.Drawing.SizeF(722.1667F, 2.083336F);
            // 
            // xrLine16
            // 
            this.xrLine16.LocationFloat = new DevExpress.Utils.PointFloat(3.320853F, 504.5F);
            this.xrLine16.Name = "xrLine16";
            this.xrLine16.SizeF = new System.Drawing.SizeF(723.1667F, 2.083336F);
            // 
            // xrLine17
            // 
            this.xrLine17.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine17.LocationFloat = new DevExpress.Utils.PointFloat(725.4879F, 397.5F);
            this.xrLine17.Name = "xrLine17";
            this.xrLine17.SizeF = new System.Drawing.SizeF(2.083337F, 107F);
            // 
            // xrLabel70
            // 
            this.xrLabel70.AutoWidth = true;
            this.xrLabel70.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel70.LocationFloat = new DevExpress.Utils.PointFloat(6.320858F, 403.5834F);
            this.xrLabel70.Name = "xrLabel70";
            this.xrLabel70.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel70.SizeF = new System.Drawing.SizeF(711.0001F, 13.91666F);
            this.xrLabel70.StylePriority.UseFont = false;
            this.xrLabel70.Text = "請選擇方案類型 (前期使用方案:               )";
            // 
            // xrLine18
            // 
            this.xrLine18.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine18.LocationFloat = new DevExpress.Utils.PointFloat(95.79198F, 422.4999F);
            this.xrLine18.Name = "xrLine18";
            this.xrLine18.SizeF = new System.Drawing.SizeF(2.166672F, 82.00003F);
            // 
            // xrLabel71
            // 
            this.xrLabel71.AutoWidth = true;
            this.xrLabel71.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel71.LocationFloat = new DevExpress.Utils.PointFloat(6.320858F, 428.5833F);
            this.xrLabel71.Name = "xrLabel71";
            this.xrLabel71.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel71.SizeF = new System.Drawing.SizeF(79.13783F, 17.91668F);
            this.xrLabel71.StylePriority.UseFont = false;
            this.xrLabel71.Text = "姓名/公司";
            // 
            // xrLabel72
            // 
            this.xrLabel72.AutoWidth = true;
            this.xrLabel72.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel72.LocationFloat = new DevExpress.Utils.PointFloat(6.320858F, 455.5833F);
            this.xrLabel72.Name = "xrLabel72";
            this.xrLabel72.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel72.SizeF = new System.Drawing.SizeF(79.13783F, 17.91668F);
            this.xrLabel72.StylePriority.UseFont = false;
            this.xrLabel72.Text = "社區名稱";
            // 
            // xrLabel73
            // 
            this.xrLabel73.AutoWidth = true;
            this.xrLabel73.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel73.LocationFloat = new DevExpress.Utils.PointFloat(6.320858F, 483.5834F);
            this.xrLabel73.Name = "xrLabel73";
            this.xrLabel73.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel73.SizeF = new System.Drawing.SizeF(79.13783F, 17.91668F);
            this.xrLabel73.StylePriority.UseFont = false;
            this.xrLabel73.Text = "聯絡電話";
            // 
            // xrLabel76
            // 
            this.xrLabel76.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.comn")});
            this.xrLabel76.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel76.LocationFloat = new DevExpress.Utils.PointFloat(97.95866F, 455.5833F);
            this.xrLabel76.Name = "xrLabel76";
            this.xrLabel76.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel76.SizeF = new System.Drawing.SizeF(609.3218F, 18F);
            this.xrLabel76.StylePriority.UseFont = false;
            // 
            // xrLabel77
            // 
            this.xrLabel77.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.contacttel")});
            this.xrLabel77.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel77.LocationFloat = new DevExpress.Utils.PointFloat(100.1253F, 483.5834F);
            this.xrLabel77.Name = "xrLabel77";
            this.xrLabel77.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel77.SizeF = new System.Drawing.SizeF(607.1552F, 18F);
            this.xrLabel77.StylePriority.UseFont = false;
            // 
            // xrLine21
            // 
            this.xrLine21.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine21.LocationFloat = new DevExpress.Utils.PointFloat(1.666855F, 397.5F);
            this.xrLine21.Name = "xrLine21";
            this.xrLine21.SizeF = new System.Drawing.SizeF(2.083337F, 109.0833F);
            // 
            // xrLabel6
            // 
            this.xrLabel6.AutoWidth = true;
            this.xrLabel6.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(5.598054F, 521.5419F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(711.4019F, 13.91666F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "信用卡繳款專區";
            // 
            // xrLine20
            // 
            this.xrLine20.LocationFloat = new DevExpress.Utils.PointFloat(3.598086F, 622.4584F);
            this.xrLine20.Name = "xrLine20";
            this.xrLine20.SizeF = new System.Drawing.SizeF(724.0833F, 2.083336F);
            // 
            // xrLine23
            // 
            this.xrLine23.LocationFloat = new DevExpress.Utils.PointFloat(3.598054F, 540.4584F);
            this.xrLine23.Name = "xrLine23";
            this.xrLine23.SizeF = new System.Drawing.SizeF(723.1667F, 2.083336F);
            // 
            // xrLine25
            // 
            this.xrLine25.LocationFloat = new DevExpress.Utils.PointFloat(3.598054F, 515.4584F);
            this.xrLine25.Name = "xrLine25";
            this.xrLine25.SizeF = new System.Drawing.SizeF(723.1667F, 2.083336F);
            // 
            // xrLine26
            // 
            this.xrLine26.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine26.LocationFloat = new DevExpress.Utils.PointFloat(2.666855F, 517.5417F);
            this.xrLine26.Name = "xrLine26";
            this.xrLine26.SizeF = new System.Drawing.SizeF(2.083337F, 105.9166F);
            // 
            // xrLine27
            // 
            this.xrLine27.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine27.LocationFloat = new DevExpress.Utils.PointFloat(724.6815F, 515.4584F);
            this.xrLine27.Name = "xrLine27";
            this.xrLine27.SizeF = new System.Drawing.SizeF(2.083337F, 107F);
            // 
            // xrRichText3
            // 
            this.xrRichText3.Font = new System.Drawing.Font("標楷體", 14F);
            this.xrRichText3.LocationFloat = new DevExpress.Utils.PointFloat(5.598054F, 544.5416F);
            this.xrRichText3.Name = "xrRichText3";
            this.xrRichText3.SerializableRtfString = resources.GetString("xrRichText3.SerializableRtfString");
            this.xrRichText3.SizeF = new System.Drawing.SizeF(716.4268F, 72.91669F);
            this.xrRichText3.StylePriority.UseFont = false;
            // 
            // xrShape1
            // 
            this.xrShape1.BorderWidth = 2F;
            this.xrShape1.LocationFloat = new DevExpress.Utils.PointFloat(527.0724F, 579.9583F);
            this.xrShape1.Name = "xrShape1";
            this.xrShape1.Shape = shapeRectangle1;
            this.xrShape1.SizeF = new System.Drawing.SizeF(180.2084F, 37.5F);
            this.xrShape1.StylePriority.UseBorderWidth = false;
            // 
            // xrLine19
            // 
            this.xrLine19.BorderColor = System.Drawing.Color.Red;
            this.xrLine19.LocationFloat = new DevExpress.Utils.PointFloat(1.487651F, 632.3333F);
            this.xrLine19.Name = "xrLine19";
            this.xrLine19.SizeF = new System.Drawing.SizeF(724.0833F, 2.083336F);
            this.xrLine19.StylePriority.UseBorderColor = false;
            // 
            // xrLine22
            // 
            this.xrLine22.LocationFloat = new DevExpress.Utils.PointFloat(4F, 640.6251F);
            this.xrLine22.Name = "xrLine22";
            this.xrLine22.SizeF = new System.Drawing.SizeF(722.4879F, 2.083336F);
            // 
            // xrLine28
            // 
            this.xrLine28.LocationFloat = new DevExpress.Utils.PointFloat(3.806164F, 0F);
            this.xrLine28.Name = "xrLine28";
            this.xrLine28.SizeF = new System.Drawing.SizeF(722.6815F, 2.083336F);
            // 
            // xrLine29
            // 
            this.xrLine29.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrLine29.Name = "xrLine29";
            this.xrLine29.SizeF = new System.Drawing.SizeF(724.0833F, 2.083336F);
            // 
            // xrLine31
            // 
            this.xrLine31.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine31.LocationFloat = new DevExpress.Utils.PointFloat(1F, 641.6282F);
            this.xrLine31.Name = "xrLine31";
            this.xrLine31.SizeF = new System.Drawing.SizeF(2.083337F, 25.03845F);
            // 
            // xrLine32
            // 
            this.xrLine32.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine32.LocationFloat = new DevExpress.Utils.PointFloat(725.4879F, 642.7084F);
            this.xrLine32.Name = "xrLine32";
            this.xrLine32.SizeF = new System.Drawing.SizeF(2.083337F, 23.95831F);
            // 
            // xrLabel1
            // 
            this.xrLabel1.AutoWidth = true;
            this.xrLabel1.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(9.27922F, 647.7084F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(131.7122F, 13.91663F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "便利商店繳款專區";
            // 
            // xrLabel2
            // 
            this.xrLabel2.AutoWidth = true;
            this.xrLabel2.Font = new System.Drawing.Font("標楷體", 8F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(151.4459F, 649.7084F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(214.6859F, 11.91663F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "(請自行勾選一項適宜的資費方案)";
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine38,
            this.xrLine39,
            this.xrLine40,
            this.xrLine41,
            this.xrLine37,
            this.xrLine36,
            this.xrLine35,
            this.xrLine8,
            this.xrLine33,
            this.xrLine34,
            this.xrLine30,
            this.xrLine2,
            this.xrLabel46,
            this.xrLabel43,
            this.xrLabel40,
            this.xrLabel54,
            this.xrLabel55,
            this.xrLine3,
            this.xrLine4,
            this.xrLine5,
            this.xrLine6,
            this.xrLine7,
            this.xrLabel56,
            this.xrLine9,
            this.xrLine10,
            this.xrLine11,
            this.xrLabel57,
            this.xrLabel58,
            this.xrLabel59,
            this.xrLabel60,
            this.xrLabel61,
            this.xrLabel62,
            this.xrLabel63,
            this.xrLabel64,
            this.xrLabel65,
            this.xrLabel66,
            this.xrRichText2,
            this.xrLabel67,
            this.xrLabel68,
            this.xrLabel70,
            this.xrLabel76,
            this.xrLabel73,
            this.xrLabel72,
            this.xrLabel71,
            this.xrLine18,
            this.xrLabel77,
            this.xrLine17,
            this.xrLine16,
            this.xrLine15,
            this.xrLine14,
            this.xrLine13,
            this.xrLabel69,
            this.xrLine12,
            this.xrLine21,
            this.xrLine23,
            this.xrRichText3,
            this.xrLine20,
            this.xrLabel6,
            this.xrLine26,
            this.xrLine27,
            this.xrLine25,
            this.xrShape1,
            this.xrLine19,
            this.xrLabel2,
            this.xrLine31,
            this.xrLine32,
            this.xrLabel1,
            this.xrLine22,
            this.xrLabel53,
            this.xrPictureBox1,
            this.xrRichText1});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("NOTICEID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 666.6667F;
            this.GroupHeader1.KeepTogether = true;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(3F, 146F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(721.0001F, 2.083336F);
            // 
            // xrLabel46
            // 
            this.xrLabel46.AutoWidth = true;
            this.xrLabel46.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.mydear")});
            this.xrLabel46.Font = new System.Drawing.Font("標楷體", 12F);
            this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(10.68125F, 67.08336F);
            this.xrLabel46.Name = "xrLabel46";
            this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel46.SizeF = new System.Drawing.SizeF(250.0741F, 21.125F);
            this.xrLabel46.StylePriority.UseFont = false;
            this.xrLabel46.Text = "xrLabel46";
            // 
            // xrLabel43
            // 
            this.xrLabel43.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.duedat", "{0:yyyy\'年\'M\'月\'d\'日\'}")});
            this.xrLabel43.Font = new System.Drawing.Font("標楷體", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(532.1451F, 88.20836F);
            this.xrLabel43.Name = "xrLabel43";
            this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel43.SizeF = new System.Drawing.SizeF(175.4543F, 18F);
            this.xrLabel43.StylePriority.UseFont = false;
            this.xrLabel43.Text = "xrLabel43";
            // 
            // xrLabel40
            // 
            this.xrLabel40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CSCUSID")});
            this.xrLabel40.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(100.8462F, 176.0834F);
            this.xrLabel40.Name = "xrLabel40";
            this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel40.SizeF = new System.Drawing.SizeF(265.646F, 14.00001F);
            this.xrLabel40.StylePriority.UseFont = false;
            this.xrLabel40.Text = "xrLabel40";
            // 
            // xrLabel54
            // 
            this.xrLabel54.AutoWidth = true;
            this.xrLabel54.Font = new System.Drawing.Font("標楷體", 12F);
            this.xrLabel54.LocationFloat = new DevExpress.Utils.PointFloat(48.37354F, 88.20836F);
            this.xrLabel54.Name = "xrLabel54";
            this.xrLabel54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel54.SizeF = new System.Drawing.SizeF(471.7715F, 23F);
            this.xrLabel54.StylePriority.UseFont = false;
            this.xrLabel54.Text = "感謝您對本公司產品的愛護與支持，提醒您的網路使用期限將於";
            // 
            // xrLabel55
            // 
            this.xrLabel55.AutoWidth = true;
            this.xrLabel55.Font = new System.Drawing.Font("標楷體", 12F);
            this.xrLabel55.LocationFloat = new DevExpress.Utils.PointFloat(4.681238F, 111.2084F);
            this.xrLabel55.Name = "xrLabel55";
            this.xrLabel55.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel55.SizeF = new System.Drawing.SizeF(720.8064F, 23F);
            this.xrLabel55.StylePriority.UseFont = false;
            this.xrLabel55.Text = "到期，為避免影響上網權益，請於繳費期限前，選擇您方便的繳款方式前往繳納，謝謝您的合作！";
            // 
            // xrLine3
            // 
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(3F, 218F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(721.0001F, 2.083336F);
            // 
            // xrLine4
            // 
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(3F, 170F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(721.0001F, 2.083336F);
            // 
            // xrLine5
            // 
            this.xrLine5.LocationFloat = new DevExpress.Utils.PointFloat(3F, 194F);
            this.xrLine5.Name = "xrLine5";
            this.xrLine5.SizeF = new System.Drawing.SizeF(721.0001F, 2.083336F);
            // 
            // xrLine6
            // 
            this.xrLine6.LocationFloat = new DevExpress.Utils.PointFloat(3F, 242F);
            this.xrLine6.Name = "xrLine6";
            this.xrLine6.SizeF = new System.Drawing.SizeF(724.9583F, 2.083336F);
            // 
            // xrLine7
            // 
            this.xrLine7.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine7.LocationFloat = new DevExpress.Utils.PointFloat(1.041753F, 146.0833F);
            this.xrLine7.Name = "xrLine7";
            this.xrLine7.SizeF = new System.Drawing.SizeF(2F, 24F);
            // 
            // xrLabel56
            // 
            this.xrLabel56.AutoWidth = true;
            this.xrLabel56.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel56.LocationFloat = new DevExpress.Utils.PointFloat(6.681252F, 152.1667F);
            this.xrLabel56.Name = "xrLabel56";
            this.xrLabel56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel56.SizeF = new System.Drawing.SizeF(375.2917F, 14.91667F);
            this.xrLabel56.StylePriority.UseFont = false;
            this.xrLabel56.Text = "基本資料(若基本資料有異動，請來電本客服中心)";
            // 
            // xrLine9
            // 
            this.xrLine9.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine9.LocationFloat = new DevExpress.Utils.PointFloat(91.15237F, 172.1667F);
            this.xrLine9.Name = "xrLine9";
            this.xrLine9.SizeF = new System.Drawing.SizeF(2F, 21.83328F);
            // 
            // xrLine10
            // 
            this.xrLine10.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine10.LocationFloat = new DevExpress.Utils.PointFloat(366.4922F, 172.0833F);
            this.xrLine10.Name = "xrLine10";
            this.xrLine10.SizeF = new System.Drawing.SizeF(2.166687F, 22.00002F);
            // 
            // xrLine11
            // 
            this.xrLine11.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine11.LocationFloat = new DevExpress.Utils.PointFloat(451.6851F, 172.1667F);
            this.xrLine11.Name = "xrLine11";
            this.xrLine11.SizeF = new System.Drawing.SizeF(2.166687F, 21.91666F);
            // 
            // xrLabel57
            // 
            this.xrLabel57.AutoWidth = true;
            this.xrLabel57.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel57.LocationFloat = new DevExpress.Utils.PointFloat(6.681252F, 177.1667F);
            this.xrLabel57.Name = "xrLabel57";
            this.xrLabel57.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel57.SizeF = new System.Drawing.SizeF(79.13783F, 13.91668F);
            this.xrLabel57.StylePriority.UseFont = false;
            this.xrLabel57.Text = "姓名/公司";
            // 
            // xrLabel58
            // 
            this.xrLabel58.AutoWidth = true;
            this.xrLabel58.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel58.LocationFloat = new DevExpress.Utils.PointFloat(6.681252F, 202.1667F);
            this.xrLabel58.Name = "xrLabel58";
            this.xrLabel58.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel58.SizeF = new System.Drawing.SizeF(79.13783F, 13.91667F);
            this.xrLabel58.StylePriority.UseFont = false;
            this.xrLabel58.Text = "社區名稱";
            // 
            // xrLabel59
            // 
            this.xrLabel59.AutoWidth = true;
            this.xrLabel59.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel59.LocationFloat = new DevExpress.Utils.PointFloat(6.681252F, 227.1667F);
            this.xrLabel59.Name = "xrLabel59";
            this.xrLabel59.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel59.SizeF = new System.Drawing.SizeF(79.13783F, 13.91669F);
            this.xrLabel59.StylePriority.UseFont = false;
            this.xrLabel59.Text = "聯絡電話";
            // 
            // xrLabel60
            // 
            this.xrLabel60.AutoWidth = true;
            this.xrLabel60.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel60.LocationFloat = new DevExpress.Utils.PointFloat(368.6589F, 177.1667F);
            this.xrLabel60.Name = "xrLabel60";
            this.xrLabel60.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel60.SizeF = new System.Drawing.SizeF(79.13783F, 13.91668F);
            this.xrLabel60.StylePriority.UseFont = false;
            this.xrLabel60.Text = "繳費期限";
            // 
            // xrLabel61
            // 
            this.xrLabel61.AutoWidth = true;
            this.xrLabel61.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel61.LocationFloat = new DevExpress.Utils.PointFloat(368.6589F, 202.1667F);
            this.xrLabel61.Name = "xrLabel61";
            this.xrLabel61.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel61.SizeF = new System.Drawing.SizeF(79.13783F, 13.91669F);
            this.xrLabel61.StylePriority.UseFont = false;
            this.xrLabel61.Text = "續約單號";
            // 
            // xrLabel62
            // 
            this.xrLabel62.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.comn")});
            this.xrLabel62.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel62.LocationFloat = new DevExpress.Utils.PointFloat(98.31905F, 202.1667F);
            this.xrLabel62.Name = "xrLabel62";
            this.xrLabel62.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel62.SizeF = new System.Drawing.SizeF(268.1731F, 14F);
            this.xrLabel62.StylePriority.UseFont = false;
            // 
            // xrLabel63
            // 
            this.xrLabel63.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.contacttel")});
            this.xrLabel63.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel63.LocationFloat = new DevExpress.Utils.PointFloat(100.4858F, 227.0834F);
            this.xrLabel63.Name = "xrLabel63";
            this.xrLabel63.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel63.SizeF = new System.Drawing.SizeF(617.1956F, 14.00002F);
            this.xrLabel63.StylePriority.UseFont = false;
            // 
            // xrLabel64
            // 
            this.xrLabel64.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.NOTICEID")});
            this.xrLabel64.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel64.LocationFloat = new DevExpress.Utils.PointFloat(454.4741F, 202.0834F);
            this.xrLabel64.Name = "xrLabel64";
            this.xrLabel64.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel64.SizeF = new System.Drawing.SizeF(253.1254F, 14F);
            this.xrLabel64.StylePriority.UseFont = false;
            // 
            // xrLabel65
            // 
            this.xrLabel65.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.duedat", "{0:yyyy\'年\'M\'月\'d\'日\'}")});
            this.xrLabel65.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel65.LocationFloat = new DevExpress.Utils.PointFloat(453.8518F, 177.1667F);
            this.xrLabel65.Name = "xrLabel65";
            this.xrLabel65.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel65.SizeF = new System.Drawing.SizeF(175.4543F, 14.00001F);
            this.xrLabel65.StylePriority.UseFont = false;
            this.xrLabel65.Text = "xrLabel43";
            // 
            // xrLabel66
            // 
            this.xrLabel66.AutoWidth = true;
            this.xrLabel66.BackColor = System.Drawing.Color.Black;
            this.xrLabel66.Font = new System.Drawing.Font("標楷體", 12F);
            this.xrLabel66.ForeColor = System.Drawing.Color.White;
            this.xrLabel66.LocationFloat = new DevExpress.Utils.PointFloat(330.6396F, 253.1667F);
            this.xrLabel66.Name = "xrLabel66";
            this.xrLabel66.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel66.SizeF = new System.Drawing.SizeF(87.71216F, 17.91667F);
            this.xrLabel66.StylePriority.UseBackColor = false;
            this.xrLabel66.StylePriority.UseFont = false;
            this.xrLabel66.StylePriority.UseForeColor = false;
            this.xrLabel66.Text = "重要訊息欄";
            // 
            // xrRichText2
            // 
            this.xrRichText2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
            this.xrRichText2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrRichText2.BorderWidth = 2F;
            this.xrRichText2.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrRichText2.LocationFloat = new DevExpress.Utils.PointFloat(8.306344F, 271.0834F);
            this.xrRichText2.Name = "xrRichText2";
            this.xrRichText2.SerializableRtfString = resources.GetString("xrRichText2.SerializableRtfString");
            this.xrRichText2.SizeF = new System.Drawing.SizeF(709.375F, 71.87498F);
            this.xrRichText2.StylePriority.UseBorderDashStyle = false;
            this.xrRichText2.StylePriority.UseBorders = false;
            this.xrRichText2.StylePriority.UseBorderWidth = false;
            this.xrRichText2.StylePriority.UseFont = false;
            // 
            // xrLabel67
            // 
            this.xrLabel67.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel67.LocationFloat = new DevExpress.Utils.PointFloat(9.279187F, 353.5417F);
            this.xrLabel67.Name = "xrLabel67";
            this.xrLabel67.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel67.SizeF = new System.Drawing.SizeF(706.9999F, 16F);
            this.xrLabel67.StylePriority.UseFont = false;
            this.xrLabel67.Text = "◎採用便利商店代繳，請於繳費期限前，持本單至全省7-11,全家,萊爾富,OK等便利商店繳納。";
            // 
            // xrLabel68
            // 
            this.xrLabel68.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel68.LocationFloat = new DevExpress.Utils.PointFloat(9.279187F, 372.5417F);
            this.xrLabel68.Name = "xrLabel68";
            this.xrLabel68.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel68.SizeF = new System.Drawing.SizeF(706.9999F, 16F);
            this.xrLabel68.StylePriority.UseFont = false;
            this.xrLabel68.Text = "◎使用信用卡付費者，請填妥下面信用卡相關資料後，將此通知書傳真回本公司，並請您來電確認。";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine29});
            this.GroupFooter1.HeightF = 15.625F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrLine1
            // 
            this.xrLine1.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(1F, 2.08F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(2.083337F, 68.125F);
            // 
            // xrLine24
            // 
            this.xrLine24.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine24.LocationFloat = new DevExpress.Utils.PointFloat(724.8062F, 2.083333F);
            this.xrLine24.Name = "xrLine24";
            this.xrLine24.SizeF = new System.Drawing.SizeF(2.083337F, 68.125F);
            // 
            // xrLabel3
            // 
            this.xrLabel3.AutoWidth = true;
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.casepayd")});
            this.xrLabel3.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(5.63949F, 5.999947F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(180.7372F, 18.00002F);
            this.xrLabel3.StylePriority.UseFont = false;
            // 
            // xrBarCode1
            // 
            this.xrBarCode1.AutoModule = true;
            this.xrBarCode1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CSBARCOD1")});
            this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(11.37673F, 23.99998F);
            this.xrBarCode1.Name = "xrBarCode1";
            this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode1.SizeF = new System.Drawing.SizeF(174.9999F, 46.20502F);
            code39Generator3.WideNarrowRatio = 3F;
            this.xrBarCode1.Symbology = code39Generator3;
            // 
            // xrLabel4
            // 
            this.xrLabel4.AutoWidth = true;
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CSNOTICEID", "帳單編號：{0}")});
            this.xrLabel4.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(212.185F, 5.999947F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(240.625F, 18.00002F);
            this.xrLabel4.StylePriority.UseFont = false;
            // 
            // xrBarCode2
            // 
            this.xrBarCode2.AutoModule = true;
            this.xrBarCode2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CSBARCOD2")});
            this.xrBarCode2.LocationFloat = new DevExpress.Utils.PointFloat(212.185F, 23.99998F);
            this.xrBarCode2.Name = "xrBarCode2";
            this.xrBarCode2.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode2.SizeF = new System.Drawing.SizeF(240.625F, 46.20501F);
            code39Generator2.WideNarrowRatio = 3F;
            this.xrBarCode2.Symbology = code39Generator2;
            // 
            // xrBarCode3
            // 
            this.xrBarCode3.AutoModule = true;
            this.xrBarCode3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CSBARCOD3")});
            this.xrBarCode3.LocationFloat = new DevExpress.Utils.PointFloat(468.8995F, 23.99998F);
            this.xrBarCode3.Name = "xrBarCode3";
            this.xrBarCode3.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode3.SizeF = new System.Drawing.SizeF(251.3609F, 46.20501F);
            code39Generator1.WideNarrowRatio = 3F;
            this.xrBarCode3.Symbology = code39Generator1;
            // 
            // xrLabel5
            // 
            this.xrLabel5.AutoWidth = true;
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CSCUSID", "用戶編號:{0}")});
            this.xrLabel5.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(472.2787F, 5.999947F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(251.3609F, 18.00002F);
            this.xrLabel5.StylePriority.UseFont = false;
            // 
            // xrLine30
            // 
            this.xrLine30.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine30.LocationFloat = new DevExpress.Utils.PointFloat(1F, 171F);
            this.xrLine30.Name = "xrLine30";
            this.xrLine30.SizeF = new System.Drawing.SizeF(2F, 25.08334F);
            // 
            // xrLine33
            // 
            this.xrLine33.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine33.LocationFloat = new DevExpress.Utils.PointFloat(1F, 219F);
            this.xrLine33.Name = "xrLine33";
            this.xrLine33.SizeF = new System.Drawing.SizeF(2F, 25.08334F);
            // 
            // xrLine34
            // 
            this.xrLine34.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine34.LocationFloat = new DevExpress.Utils.PointFloat(1F, 195F);
            this.xrLine34.Name = "xrLine34";
            this.xrLine34.SizeF = new System.Drawing.SizeF(2F, 25.08334F);
            // 
            // xrLine8
            // 
            this.xrLine8.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine8.LocationFloat = new DevExpress.Utils.PointFloat(91.15238F, 195F);
            this.xrLine8.Name = "xrLine8";
            this.xrLine8.SizeF = new System.Drawing.SizeF(2F, 23.00002F);
            // 
            // xrLine35
            // 
            this.xrLine35.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine35.LocationFloat = new DevExpress.Utils.PointFloat(91.15235F, 221.0833F);
            this.xrLine35.Name = "xrLine35";
            this.xrLine35.SizeF = new System.Drawing.SizeF(2F, 23.00002F);
            // 
            // xrLine36
            // 
            this.xrLine36.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine36.LocationFloat = new DevExpress.Utils.PointFloat(366.659F, 195F);
            this.xrLine36.Name = "xrLine36";
            this.xrLine36.SizeF = new System.Drawing.SizeF(2F, 23.00002F);
            // 
            // xrLine37
            // 
            this.xrLine37.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine37.LocationFloat = new DevExpress.Utils.PointFloat(451.8518F, 195.1667F);
            this.xrLine37.Name = "xrLine37";
            this.xrLine37.SizeF = new System.Drawing.SizeF(2F, 23.83331F);
            // 
            // xrLine38
            // 
            this.xrLine38.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine38.LocationFloat = new DevExpress.Utils.PointFloat(724.5712F, 219F);
            this.xrLine38.Name = "xrLine38";
            this.xrLine38.SizeF = new System.Drawing.SizeF(2F, 25.08334F);
            // 
            // xrLine39
            // 
            this.xrLine39.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine39.LocationFloat = new DevExpress.Utils.PointFloat(724.6129F, 146.0833F);
            this.xrLine39.Name = "xrLine39";
            this.xrLine39.SizeF = new System.Drawing.SizeF(2F, 24F);
            // 
            // xrLine40
            // 
            this.xrLine40.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine40.LocationFloat = new DevExpress.Utils.PointFloat(724.5712F, 171F);
            this.xrLine40.Name = "xrLine40";
            this.xrLine40.SizeF = new System.Drawing.SizeF(2F, 25.08334F);
            // 
            // xrLine41
            // 
            this.xrLine41.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine41.LocationFloat = new DevExpress.Utils.PointFloat(724.5712F, 195F);
            this.xrLine41.Name = "xrLine41";
            this.xrLine41.SizeF = new System.Drawing.SizeF(2F, 25.08334F);
            // 
            // RT3021R
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1,
            this.GroupFooter1});
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
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
