using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for RT205R
/// </summary>
public class RT205R : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRLabel xrLabel5;
    private XRLabel xrLabel7;
    private XRLabel xrLabel13;
    private XRLabel xrLabel22;
    private XRLabel xrLabel36;
    private XRLabel xrLabel37;
    private XRLabel xrLabel41;
    private XRLabel xrLabel42;
    private XRLabel xrLabel45;
    private XRLabel xrLabel47;
    private XRLabel xrLabel48;
    private XRLabel xrLabel54;
    private XRLabel xrLabel55;
    private XRLabel xrLabel56;
    private XRLabel xrLabel57;
    private XRLabel xrLabel59;
    private XRLabel xrLabel62;
    private XRLine xrLine1;
    private XRLine xrLine2;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private XRLabel xrLabel31;
    private XRLabel xrLabel63;
    private PageFooterBand pageFooterBand1;
    private XRPageInfo xrPageInfo2;
    private ReportHeaderBand reportHeaderBand1;
    private XRPageInfo xrPageInfo3;
    private XRPageInfo xrPageInfo1;
    private XRLabel xrLabel65;
    private XRControlStyle Title;
    private XRControlStyle FieldCaption;
    private XRControlStyle PageInfo;
    private XRControlStyle DataField;
    private XRLine xrLine4;
    private XRShape xrShape1;
    private XRLine xrLine3;
    private XRLabel xrLabel76;
    private XRLine xrLine12;
    private XRLabel xrLabel75;
    private XRLine xrLine11;
    private XRLabel xrLabel74;
    private XRLine xrLine10;
    private XRLabel xrLabel73;
    private XRLine xrLine9;
    private XRLabel xrLabel72;
    private XRLine xrLine8;
    private XRLabel xrLabel69;
    private XRLabel xrLabel70;
    private XRLabel xrLabel71;
    private XRLabel xrLabel68;
    private XRLabel xrLabel66;
    private XRLabel xrLabel67;
    private XRLabel xrLabel39;
    private XRLabel xrLabel4;
    private XRLine xrLine7;
    private XRLine xrLine6;
    private XRLine xrLine5;
    private XRLabel xrLabel83;
    private XRLabel xrLabel84;
    private XRLabel xrLabel85;
    private XRLabel xrLabel78;
    private XRLabel xrLabel79;
    private XRLabel xrLabel80;
    private XRLabel xrLabel81;
    private XRLabel xrLabel82;
    private XRLabel xrLabel77;
    private XRLabel xrLabel87;
    private XRLabel xrLabel86;
    private XRShape xrShape12;
    private XRShape xrShape13;
    private XRShape xrShape14;
    private XRShape xrShape15;
    private XRShape xrShape16;
    private XRShape xrShape17;
    private XRShape xrShape18;
    private XRShape xrShape19;
    private XRShape xrShape20;
    private XRShape xrShape21;
    private XRShape xrShape22;
    private XRShape xrShape11;
    private XRShape xrShape10;
    private XRShape xrShape9;
    private XRShape xrShape8;
    private XRShape xrShape7;
    private XRShape xrShape6;
    private XRShape xrShape5;
    private XRShape xrShape4;
    private XRShape xrShape3;
    private XRShape xrShape2;
    private XRLabel xrLabel64;
    private XRShape xrShape23;
    private XRShape xrShape24;
    private XRShape xrShape25;
    private XRShape xrShape26;
    private XRShape xrShape27;
    private XRShape xrShape28;
    private XRShape xrShape29;
    private XRShape xrShape30;
    private XRShape xrShape31;
    private XRShape xrShape32;
    private XRShape xrShape33;
    private XRShape xrShape42;
    private XRShape xrShape44;
    private XRShape xrShape45;
    private XRShape xrShape46;
    private XRShape xrShape47;
    private XRShape xrShape48;
    private XRShape xrShape49;
    private XRShape xrShape34;
    private XRShape xrShape36;
    private XRShape xrShape37;
    private XRShape xrShape38;
    private XRShape xrShape39;
    private XRShape xrShape40;
    private XRShape xrShape41;
    private XRShape xrShape60;
    private XRShape xrShape62;
    private XRShape xrShape63;
    private XRShape xrShape64;
    private XRShape xrShape65;
    private XRShape xrShape66;
    private XRShape xrShape67;
    private XRShape xrShape68;
    private XRShape xrShape69;
    private XRShape xrShape50;
    private XRShape xrShape52;
    private XRShape xrShape53;
    private XRShape xrShape54;
    private XRShape xrShape55;
    private XRShape xrShape56;
    private XRShape xrShape57;
    private XRShape xrShape58;
    private XRShape xrShape59;
    private XRLabel xrLabel17;
    private XRLabel xrLabel16;
    private XRLabel xrLabel15;
    private XRLabel xrLabel14;
    private XRLabel xrLabel12;
    private XRLabel xrLabel11;
    private XRLabel xrLabel10;
    private XRLabel xrLabel9;
    private XRLabel xrLabel8;
    private XRLabel xrLabel6;
    private XRLabel xrLabel3;
    private XRLabel xrLabel2;
    private XRLabel xrLabel1;
    private XRShape xrShape70;
    private XRLabel xrLabel18;
    private XRLabel xrLabel25;
    private XRLabel xrLabel24;
    private XRLabel xrLabel23;
    private XRLabel xrLabel21;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public RT205R()
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
            string resourceFileName = "RT205R.resx";
            System.Resources.ResourceManager resources = global::Resources.RT205R.ResourceManager;
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraPrinting.Shape.ShapeRectangle shapeRectangle1 = new DevExpress.XtraPrinting.Shape.ShapeRectangle();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine1 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine2 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine3 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine4 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine5 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine6 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine7 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine8 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine9 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine10 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine11 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine12 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine13 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine14 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine15 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine16 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine17 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine18 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine19 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine20 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine21 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine22 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine23 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine24 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine25 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine26 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine27 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine28 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine29 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine30 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine31 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine32 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine33 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine34 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine35 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine36 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine37 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine38 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine39 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine40 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine41 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine42 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine43 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine44 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine45 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine46 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine47 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine48 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine49 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine50 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine51 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine52 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine53 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine54 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine55 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine56 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine57 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine58 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine59 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine60 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine61 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine62 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine63 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine64 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.XtraPrinting.Shape.ShapeLine shapeLine65 = new DevExpress.XtraPrinting.Shape.ShapeLine();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrShape70 = new DevExpress.XtraReports.UI.XRShape();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrShape60 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape62 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape63 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape64 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape65 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape66 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape67 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape68 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape69 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape50 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape52 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape53 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape54 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape55 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape56 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape57 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape58 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape59 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape42 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape44 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape45 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape46 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape47 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape48 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape49 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape34 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape36 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape37 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape38 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape39 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape40 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape41 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape23 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape24 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape25 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape26 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape27 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape28 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape29 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape30 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape31 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape32 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape33 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape12 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape13 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape14 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape15 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape16 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape17 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape18 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape19 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape20 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape21 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape22 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape11 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape10 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape9 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape8 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape7 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape6 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape5 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape4 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape3 = new DevExpress.XtraReports.UI.XRShape();
            this.xrShape2 = new DevExpress.XtraReports.UI.XRShape();
            this.xrLabel87 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel86 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel83 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel84 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel85 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel78 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel79 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel80 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel81 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel82 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel77 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel76 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine12 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel75 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine11 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel74 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine10 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel73 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine9 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel72 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine8 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel69 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel70 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel71 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel68 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel66 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel67 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine7 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine6 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine5 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.xrShape1 = new DevExpress.XtraReports.UI.XRShape();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel45 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel47 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel48 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel54 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel55 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel56 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel57 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel59 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel62 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel64 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel63 = new DevExpress.XtraReports.UI.XRLabel();
            this.pageFooterBand1 = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.reportHeaderBand1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrPageInfo3 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel65 = new DevExpress.XtraReports.UI.XRLabel();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel25,
            this.xrLabel24,
            this.xrLabel23,
            this.xrLabel21,
            this.xrShape70,
            this.xrLabel18,
            this.xrLabel17,
            this.xrLabel16,
            this.xrLabel15,
            this.xrLabel14,
            this.xrLabel12,
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel6,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1,
            this.xrShape60,
            this.xrShape62,
            this.xrShape63,
            this.xrShape64,
            this.xrShape65,
            this.xrShape66,
            this.xrShape67,
            this.xrShape68,
            this.xrShape69,
            this.xrShape50,
            this.xrShape52,
            this.xrShape53,
            this.xrShape54,
            this.xrShape55,
            this.xrShape56,
            this.xrShape57,
            this.xrShape58,
            this.xrShape59,
            this.xrShape42,
            this.xrShape44,
            this.xrShape45,
            this.xrShape46,
            this.xrShape47,
            this.xrShape48,
            this.xrShape49,
            this.xrShape34,
            this.xrShape36,
            this.xrShape37,
            this.xrShape38,
            this.xrShape39,
            this.xrShape40,
            this.xrShape41,
            this.xrShape23,
            this.xrShape24,
            this.xrShape25,
            this.xrShape26,
            this.xrShape27,
            this.xrShape28,
            this.xrShape29,
            this.xrShape30,
            this.xrShape31,
            this.xrShape32,
            this.xrShape33,
            this.xrShape12,
            this.xrShape13,
            this.xrShape14,
            this.xrShape15,
            this.xrShape16,
            this.xrShape17,
            this.xrShape18,
            this.xrShape19,
            this.xrShape20,
            this.xrShape21,
            this.xrShape22,
            this.xrShape11,
            this.xrShape10,
            this.xrShape9,
            this.xrShape8,
            this.xrShape7,
            this.xrShape6,
            this.xrShape5,
            this.xrShape4,
            this.xrShape3,
            this.xrShape2,
            this.xrLabel87,
            this.xrLabel86,
            this.xrLabel83,
            this.xrLabel84,
            this.xrLabel85,
            this.xrLabel78,
            this.xrLabel79,
            this.xrLabel80,
            this.xrLabel81,
            this.xrLabel82,
            this.xrLabel77,
            this.xrLabel76,
            this.xrLine12,
            this.xrLabel75,
            this.xrLine11,
            this.xrLabel74,
            this.xrLine10,
            this.xrLabel73,
            this.xrLine9,
            this.xrLabel72,
            this.xrLine8,
            this.xrLabel69,
            this.xrLabel70,
            this.xrLabel71,
            this.xrLabel68,
            this.xrLabel66,
            this.xrLabel67,
            this.xrLabel39,
            this.xrLabel4,
            this.xrLine7,
            this.xrLine6,
            this.xrLine5,
            this.xrLine4,
            this.xrShape1,
            this.xrLine3,
            this.xrLabel5,
            this.xrLabel7,
            this.xrLabel13,
            this.xrLabel22,
            this.xrLabel36,
            this.xrLabel37,
            this.xrLabel41,
            this.xrLabel42,
            this.xrLabel45,
            this.xrLabel47,
            this.xrLabel48,
            this.xrLabel54,
            this.xrLabel55,
            this.xrLabel56,
            this.xrLabel57,
            this.xrLabel59,
            this.xrLabel62,
            this.xrLabel64,
            this.xrLine1,
            this.xrLine2});
            this.Detail.HeightF = 748.5F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StyleName = "DataField";
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel25
            // 
            this.xrLabel25.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel25.ForeColor = System.Drawing.Color.Black;
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(547.9167F, 684.375F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(76.12494F, 16F);
            this.xrLabel25.StyleName = "FieldCaption";
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UseForeColor = false;
            this.xrLabel25.Text = "客戶簽名：";
            // 
            // xrLabel24
            // 
            this.xrLabel24.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel24.ForeColor = System.Drawing.Color.Black;
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(358.0416F, 684.375F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(75.08322F, 16F);
            this.xrLabel24.StyleName = "FieldCaption";
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UseForeColor = false;
            this.xrLabel24.Text = "客服人員：";
            // 
            // xrLabel23
            // 
            this.xrLabel23.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel23.ForeColor = System.Drawing.Color.Black;
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(178.1252F, 684.375F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(79.74988F, 16F);
            this.xrLabel23.StyleName = "FieldCaption";
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseForeColor = false;
            this.xrLabel23.Text = "業務助理：";
            // 
            // xrLabel21
            // 
            this.xrLabel21.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel21.ForeColor = System.Drawing.Color.Black;
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(10.17027F, 684.375F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(60.49991F, 16.00002F);
            this.xrLabel21.StyleName = "FieldCaption";
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UseForeColor = false;
            this.xrLabel21.Text = "工程師：";
            // 
            // xrShape70
            // 
            this.xrShape70.BorderWidth = 2F;
            this.xrShape70.LocationFloat = new DevExpress.Utils.PointFloat(4.166667F, 397.9167F);
            this.xrShape70.Name = "xrShape70";
            this.xrShape70.Shape = shapeRectangle1;
            this.xrShape70.SizeF = new System.Drawing.SizeF(722.9166F, 273.9583F);
            this.xrShape70.StylePriority.UseBorderWidth = false;
            // 
            // xrLabel18
            // 
            this.xrLabel18.AutoWidth = true;
            this.xrLabel18.Font = new System.Drawing.Font("標楷體", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel18.ForeColor = System.Drawing.Color.Black;
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(301.0417F, 372.4167F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(166.75F, 25.5F);
            this.xrLabel18.StyleName = "Title";
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseForeColor = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "派工處理措施";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel17
            // 
            this.xrLabel17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.paycycle")});
            this.xrLabel17.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(78.00013F, 128F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(207.9999F, 18F);
            this.xrLabel17.StylePriority.UseFont = false;
            // 
            // xrLabel16
            // 
            this.xrLabel16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.sndwrkusr")});
            this.xrLabel16.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(354.0001F, 249F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(161.9582F, 15.00003F);
            this.xrLabel16.StylePriority.UseFont = false;
            // 
            // xrLabel15
            // 
            this.xrLabel15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.sndwrkdat", "{0:yyyy/MM/dd}")});
            this.xrLabel15.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(358.0416F, 280F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(157.9165F, 16.00003F);
            this.xrLabel15.StylePriority.UseFont = false;
            // 
            // xrLabel14
            // 
            this.xrLabel14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.assigneng")});
            this.xrLabel14.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(600.96F, 279F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(120.0414F, 17F);
            this.xrLabel14.StylePriority.UseFont = false;
            // 
            // xrLabel12
            // 
            this.xrLabel12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.WtlApplyDat", "{0:yyyy/MM/dd}")});
            this.xrLabel12.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(600.96F, 248.44F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(120.0414F, 17F);
            this.xrLabel12.StylePriority.UseFont = false;
            // 
            // xrLabel11
            // 
            this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.RCVDAT", "{0:yyyy/MM/dd}")});
            this.xrLabel11.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(600.96F, 219.7082F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(120.0414F, 17F);
            this.xrLabel11.StylePriority.UseFont = false;
            // 
            // xrLabel10
            // 
            this.xrLabel10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.overdue")});
            this.xrLabel10.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(600.96F, 190F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(116.0402F, 14F);
            this.xrLabel10.StylePriority.UseFont = false;
            // 
            // xrLabel9
            // 
            this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.dropdat", "{0:yyyy/MM/dd}")});
            this.xrLabel9.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(600.96F, 157.5F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(120.0414F, 17F);
            this.xrLabel9.StylePriority.UseFont = false;
            // 
            // xrLabel8
            // 
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.duedat", "{0:yyyy/MM/dd}")});
            this.xrLabel8.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(600.9586F, 127F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(120.0414F, 17F);
            this.xrLabel8.StylePriority.UseFont = false;
            // 
            // xrLabel6
            // 
            this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.strbillingdat", "{0:yyyy/MM/dd}")});
            this.xrLabel6.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(600.96F, 98.17F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(120.0414F, 17F);
            this.xrLabel6.StylePriority.UseFont = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.freecode")});
            this.xrLabel3.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(358.0416F, 129.75F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(157.9166F, 16.25F);
            this.xrLabel3.StylePriority.UseFont = false;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.comtypenc")});
            this.xrLabel2.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(358.0416F, 10.00001F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(157.9166F, 14F);
            this.xrLabel2.StylePriority.UseFont = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CMTYIP")});
            this.xrLabel1.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(78.49989F, 39.99999F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(642.5F, 17.00002F);
            this.xrLabel1.StylePriority.UseFont = false;
            // 
            // xrShape60
            // 
            this.xrShape60.LocationFloat = new DevExpress.Utils.PointFloat(595.9586F, 242.0001F);
            this.xrShape60.Name = "xrShape60";
            this.xrShape60.Shape = shapeLine1;
            this.xrShape60.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape62
            // 
            this.xrShape62.LocationFloat = new DevExpress.Utils.PointFloat(595.9586F, 62.00002F);
            this.xrShape62.Name = "xrShape62";
            this.xrShape62.Shape = shapeLine2;
            this.xrShape62.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape63
            // 
            this.xrShape63.LocationFloat = new DevExpress.Utils.PointFloat(595.9586F, 92.00007F);
            this.xrShape63.Name = "xrShape63";
            this.xrShape63.Shape = shapeLine3;
            this.xrShape63.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape64
            // 
            this.xrShape64.LocationFloat = new DevExpress.Utils.PointFloat(595.9586F, 122.0001F);
            this.xrShape64.Name = "xrShape64";
            this.xrShape64.Shape = shapeLine4;
            this.xrShape64.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape65
            // 
            this.xrShape65.LocationFloat = new DevExpress.Utils.PointFloat(595.9586F, 152F);
            this.xrShape65.Name = "xrShape65";
            this.xrShape65.Shape = shapeLine5;
            this.xrShape65.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape66
            // 
            this.xrShape66.LocationFloat = new DevExpress.Utils.PointFloat(595.9586F, 182F);
            this.xrShape66.Name = "xrShape66";
            this.xrShape66.Shape = shapeLine6;
            this.xrShape66.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape67
            // 
            this.xrShape67.LocationFloat = new DevExpress.Utils.PointFloat(595.9586F, 212F);
            this.xrShape67.Name = "xrShape67";
            this.xrShape67.Shape = shapeLine7;
            this.xrShape67.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape68
            // 
            this.xrShape68.LocationFloat = new DevExpress.Utils.PointFloat(595.9586F, 2.000046F);
            this.xrShape68.Name = "xrShape68";
            this.xrShape68.Shape = shapeLine8;
            this.xrShape68.SizeF = new System.Drawing.SizeF(2F, 28F);
            // 
            // xrShape69
            // 
            this.xrShape69.LocationFloat = new DevExpress.Utils.PointFloat(595.9586F, 272.0001F);
            this.xrShape69.Name = "xrShape69";
            this.xrShape69.Shape = shapeLine9;
            this.xrShape69.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape50
            // 
            this.xrShape50.LocationFloat = new DevExpress.Utils.PointFloat(515.9583F, 1.999964F);
            this.xrShape50.Name = "xrShape50";
            this.xrShape50.Shape = shapeLine10;
            this.xrShape50.SizeF = new System.Drawing.SizeF(2F, 28F);
            // 
            // xrShape52
            // 
            this.xrShape52.LocationFloat = new DevExpress.Utils.PointFloat(515.9583F, 61.99994F);
            this.xrShape52.Name = "xrShape52";
            this.xrShape52.Shape = shapeLine11;
            this.xrShape52.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape53
            // 
            this.xrShape53.LocationFloat = new DevExpress.Utils.PointFloat(515.9583F, 91.99999F);
            this.xrShape53.Name = "xrShape53";
            this.xrShape53.Shape = shapeLine12;
            this.xrShape53.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape54
            // 
            this.xrShape54.LocationFloat = new DevExpress.Utils.PointFloat(515.9583F, 122F);
            this.xrShape54.Name = "xrShape54";
            this.xrShape54.Shape = shapeLine13;
            this.xrShape54.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape55
            // 
            this.xrShape55.LocationFloat = new DevExpress.Utils.PointFloat(515.9583F, 152F);
            this.xrShape55.Name = "xrShape55";
            this.xrShape55.Shape = shapeLine14;
            this.xrShape55.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape56
            // 
            this.xrShape56.LocationFloat = new DevExpress.Utils.PointFloat(515.9583F, 182F);
            this.xrShape56.Name = "xrShape56";
            this.xrShape56.Shape = shapeLine15;
            this.xrShape56.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape57
            // 
            this.xrShape57.LocationFloat = new DevExpress.Utils.PointFloat(515.9583F, 211.9999F);
            this.xrShape57.Name = "xrShape57";
            this.xrShape57.Shape = shapeLine16;
            this.xrShape57.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape58
            // 
            this.xrShape58.LocationFloat = new DevExpress.Utils.PointFloat(515.9583F, 242F);
            this.xrShape58.Name = "xrShape58";
            this.xrShape58.Shape = shapeLine17;
            this.xrShape58.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape59
            // 
            this.xrShape59.LocationFloat = new DevExpress.Utils.PointFloat(515.9583F, 272F);
            this.xrShape59.Name = "xrShape59";
            this.xrShape59.Shape = shapeLine18;
            this.xrShape59.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape42
            // 
            this.xrShape42.LocationFloat = new DevExpress.Utils.PointFloat(352F, 2F);
            this.xrShape42.Name = "xrShape42";
            this.xrShape42.Shape = shapeLine19;
            this.xrShape42.SizeF = new System.Drawing.SizeF(2F, 28F);
            // 
            // xrShape44
            // 
            this.xrShape44.LocationFloat = new DevExpress.Utils.PointFloat(352F, 61.99994F);
            this.xrShape44.Name = "xrShape44";
            this.xrShape44.Shape = shapeLine20;
            this.xrShape44.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape45
            // 
            this.xrShape45.LocationFloat = new DevExpress.Utils.PointFloat(352F, 91.99999F);
            this.xrShape45.Name = "xrShape45";
            this.xrShape45.Shape = shapeLine21;
            this.xrShape45.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape46
            // 
            this.xrShape46.LocationFloat = new DevExpress.Utils.PointFloat(352F, 122F);
            this.xrShape46.Name = "xrShape46";
            this.xrShape46.Shape = shapeLine22;
            this.xrShape46.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape47
            // 
            this.xrShape47.LocationFloat = new DevExpress.Utils.PointFloat(352F, 211.9999F);
            this.xrShape47.Name = "xrShape47";
            this.xrShape47.Shape = shapeLine23;
            this.xrShape47.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape48
            // 
            this.xrShape48.LocationFloat = new DevExpress.Utils.PointFloat(352F, 242F);
            this.xrShape48.Name = "xrShape48";
            this.xrShape48.Shape = shapeLine24;
            this.xrShape48.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape49
            // 
            this.xrShape49.LocationFloat = new DevExpress.Utils.PointFloat(352F, 272F);
            this.xrShape49.Name = "xrShape49";
            this.xrShape49.Shape = shapeLine25;
            this.xrShape49.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape34
            // 
            this.xrShape34.LocationFloat = new DevExpress.Utils.PointFloat(286F, 2F);
            this.xrShape34.Name = "xrShape34";
            this.xrShape34.Shape = shapeLine26;
            this.xrShape34.SizeF = new System.Drawing.SizeF(2F, 28F);
            // 
            // xrShape36
            // 
            this.xrShape36.LocationFloat = new DevExpress.Utils.PointFloat(286F, 62F);
            this.xrShape36.Name = "xrShape36";
            this.xrShape36.Shape = shapeLine27;
            this.xrShape36.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape37
            // 
            this.xrShape37.LocationFloat = new DevExpress.Utils.PointFloat(286F, 92F);
            this.xrShape37.Name = "xrShape37";
            this.xrShape37.Shape = shapeLine28;
            this.xrShape37.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape38
            // 
            this.xrShape38.LocationFloat = new DevExpress.Utils.PointFloat(286F, 122F);
            this.xrShape38.Name = "xrShape38";
            this.xrShape38.Shape = shapeLine29;
            this.xrShape38.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape39
            // 
            this.xrShape39.LocationFloat = new DevExpress.Utils.PointFloat(286F, 212F);
            this.xrShape39.Name = "xrShape39";
            this.xrShape39.Shape = shapeLine30;
            this.xrShape39.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape40
            // 
            this.xrShape40.LocationFloat = new DevExpress.Utils.PointFloat(286F, 242F);
            this.xrShape40.Name = "xrShape40";
            this.xrShape40.Shape = shapeLine31;
            this.xrShape40.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape41
            // 
            this.xrShape41.LocationFloat = new DevExpress.Utils.PointFloat(286F, 272F);
            this.xrShape41.Name = "xrShape41";
            this.xrShape41.Shape = shapeLine32;
            this.xrShape41.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape23
            // 
            this.xrShape23.LocationFloat = new DevExpress.Utils.PointFloat(74F, 2F);
            this.xrShape23.Name = "xrShape23";
            this.xrShape23.Shape = shapeLine33;
            this.xrShape23.SizeF = new System.Drawing.SizeF(2F, 28F);
            // 
            // xrShape24
            // 
            this.xrShape24.LocationFloat = new DevExpress.Utils.PointFloat(74F, 32F);
            this.xrShape24.Name = "xrShape24";
            this.xrShape24.Shape = shapeLine34;
            this.xrShape24.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape25
            // 
            this.xrShape25.LocationFloat = new DevExpress.Utils.PointFloat(74F, 62F);
            this.xrShape25.Name = "xrShape25";
            this.xrShape25.Shape = shapeLine35;
            this.xrShape25.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape26
            // 
            this.xrShape26.LocationFloat = new DevExpress.Utils.PointFloat(74F, 92F);
            this.xrShape26.Name = "xrShape26";
            this.xrShape26.Shape = shapeLine36;
            this.xrShape26.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape27
            // 
            this.xrShape27.LocationFloat = new DevExpress.Utils.PointFloat(74F, 122F);
            this.xrShape27.Name = "xrShape27";
            this.xrShape27.Shape = shapeLine37;
            this.xrShape27.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape28
            // 
            this.xrShape28.LocationFloat = new DevExpress.Utils.PointFloat(74F, 152F);
            this.xrShape28.Name = "xrShape28";
            this.xrShape28.Shape = shapeLine38;
            this.xrShape28.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape29
            // 
            this.xrShape29.LocationFloat = new DevExpress.Utils.PointFloat(74F, 182F);
            this.xrShape29.Name = "xrShape29";
            this.xrShape29.Shape = shapeLine39;
            this.xrShape29.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape30
            // 
            this.xrShape30.LocationFloat = new DevExpress.Utils.PointFloat(74F, 212F);
            this.xrShape30.Name = "xrShape30";
            this.xrShape30.Shape = shapeLine40;
            this.xrShape30.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape31
            // 
            this.xrShape31.LocationFloat = new DevExpress.Utils.PointFloat(74F, 242F);
            this.xrShape31.Name = "xrShape31";
            this.xrShape31.Shape = shapeLine41;
            this.xrShape31.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape32
            // 
            this.xrShape32.LocationFloat = new DevExpress.Utils.PointFloat(74F, 272F);
            this.xrShape32.Name = "xrShape32";
            this.xrShape32.Shape = shapeLine42;
            this.xrShape32.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape33
            // 
            this.xrShape33.LocationFloat = new DevExpress.Utils.PointFloat(74F, 302F);
            this.xrShape33.Name = "xrShape33";
            this.xrShape33.Shape = shapeLine43;
            this.xrShape33.SizeF = new System.Drawing.SizeF(2F, 58F);
            // 
            // xrShape12
            // 
            this.xrShape12.LocationFloat = new DevExpress.Utils.PointFloat(724F, 2F);
            this.xrShape12.Name = "xrShape12";
            this.xrShape12.Shape = shapeLine44;
            this.xrShape12.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape13
            // 
            this.xrShape13.LocationFloat = new DevExpress.Utils.PointFloat(723.9999F, 31.39587F);
            this.xrShape13.Name = "xrShape13";
            this.xrShape13.Shape = shapeLine45;
            this.xrShape13.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape14
            // 
            this.xrShape14.LocationFloat = new DevExpress.Utils.PointFloat(724F, 62.39589F);
            this.xrShape14.Name = "xrShape14";
            this.xrShape14.Shape = shapeLine46;
            this.xrShape14.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape15
            // 
            this.xrShape15.LocationFloat = new DevExpress.Utils.PointFloat(724F, 91.3959F);
            this.xrShape15.Name = "xrShape15";
            this.xrShape15.Shape = shapeLine47;
            this.xrShape15.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape16
            // 
            this.xrShape16.LocationFloat = new DevExpress.Utils.PointFloat(723.9999F, 121.3959F);
            this.xrShape16.Name = "xrShape16";
            this.xrShape16.Shape = shapeLine48;
            this.xrShape16.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape17
            // 
            this.xrShape17.LocationFloat = new DevExpress.Utils.PointFloat(724F, 151.3959F);
            this.xrShape17.Name = "xrShape17";
            this.xrShape17.Shape = shapeLine49;
            this.xrShape17.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape18
            // 
            this.xrShape18.LocationFloat = new DevExpress.Utils.PointFloat(724F, 182.3959F);
            this.xrShape18.Name = "xrShape18";
            this.xrShape18.Shape = shapeLine50;
            this.xrShape18.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape19
            // 
            this.xrShape19.LocationFloat = new DevExpress.Utils.PointFloat(723.9999F, 212.3959F);
            this.xrShape19.Name = "xrShape19";
            this.xrShape19.Shape = shapeLine51;
            this.xrShape19.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape20
            // 
            this.xrShape20.LocationFloat = new DevExpress.Utils.PointFloat(724F, 242.3959F);
            this.xrShape20.Name = "xrShape20";
            this.xrShape20.Shape = shapeLine52;
            this.xrShape20.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape21
            // 
            this.xrShape21.LocationFloat = new DevExpress.Utils.PointFloat(724F, 271.3959F);
            this.xrShape21.Name = "xrShape21";
            this.xrShape21.Shape = shapeLine53;
            this.xrShape21.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape22
            // 
            this.xrShape22.LocationFloat = new DevExpress.Utils.PointFloat(724F, 301.3958F);
            this.xrShape22.Name = "xrShape22";
            this.xrShape22.Shape = shapeLine54;
            this.xrShape22.SizeF = new System.Drawing.SizeF(2F, 58F);
            // 
            // xrShape11
            // 
            this.xrShape11.LocationFloat = new DevExpress.Utils.PointFloat(1.000055F, 302F);
            this.xrShape11.Name = "xrShape11";
            this.xrShape11.Shape = shapeLine55;
            this.xrShape11.SizeF = new System.Drawing.SizeF(2F, 58F);
            // 
            // xrShape10
            // 
            this.xrShape10.LocationFloat = new DevExpress.Utils.PointFloat(1.000055F, 272F);
            this.xrShape10.Name = "xrShape10";
            this.xrShape10.Shape = shapeLine56;
            this.xrShape10.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape9
            // 
            this.xrShape9.LocationFloat = new DevExpress.Utils.PointFloat(1.000055F, 240F);
            this.xrShape9.Name = "xrShape9";
            this.xrShape9.Shape = shapeLine57;
            this.xrShape9.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape8
            // 
            this.xrShape8.LocationFloat = new DevExpress.Utils.PointFloat(0.9999911F, 212F);
            this.xrShape8.Name = "xrShape8";
            this.xrShape8.Shape = shapeLine58;
            this.xrShape8.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape7
            // 
            this.xrShape7.LocationFloat = new DevExpress.Utils.PointFloat(1.000055F, 182F);
            this.xrShape7.Name = "xrShape7";
            this.xrShape7.Shape = shapeLine59;
            this.xrShape7.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape6
            // 
            this.xrShape6.LocationFloat = new DevExpress.Utils.PointFloat(1.000055F, 152F);
            this.xrShape6.Name = "xrShape6";
            this.xrShape6.Shape = shapeLine60;
            this.xrShape6.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape5
            // 
            this.xrShape5.LocationFloat = new DevExpress.Utils.PointFloat(1F, 122F);
            this.xrShape5.Name = "xrShape5";
            this.xrShape5.Shape = shapeLine61;
            this.xrShape5.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape4
            // 
            this.xrShape4.LocationFloat = new DevExpress.Utils.PointFloat(1.000055F, 91.99999F);
            this.xrShape4.Name = "xrShape4";
            this.xrShape4.Shape = shapeLine62;
            this.xrShape4.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape3
            // 
            this.xrShape3.LocationFloat = new DevExpress.Utils.PointFloat(1.000055F, 61.99999F);
            this.xrShape3.Name = "xrShape3";
            this.xrShape3.Shape = shapeLine63;
            this.xrShape3.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrShape2
            // 
            this.xrShape2.LocationFloat = new DevExpress.Utils.PointFloat(1F, 31.99999F);
            this.xrShape2.Name = "xrShape2";
            this.xrShape2.Shape = shapeLine64;
            this.xrShape2.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrLabel87
            // 
            this.xrLabel87.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel87.ForeColor = System.Drawing.Color.Black;
            this.xrLabel87.LocationFloat = new DevExpress.Utils.PointFloat(6F, 340F);
            this.xrLabel87.Name = "xrLabel87";
            this.xrLabel87.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel87.SizeF = new System.Drawing.SizeF(60.49991F, 16.00002F);
            this.xrLabel87.StyleName = "FieldCaption";
            this.xrLabel87.StylePriority.UseFont = false;
            this.xrLabel87.StylePriority.UseForeColor = false;
            this.xrLabel87.Text = "描　　述";
            // 
            // xrLabel86
            // 
            this.xrLabel86.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel86.ForeColor = System.Drawing.Color.Black;
            this.xrLabel86.LocationFloat = new DevExpress.Utils.PointFloat(6F, 310F);
            this.xrLabel86.Name = "xrLabel86";
            this.xrLabel86.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel86.SizeF = new System.Drawing.SizeF(60.49991F, 16.00002F);
            this.xrLabel86.StyleName = "FieldCaption";
            this.xrLabel86.StylePriority.UseFont = false;
            this.xrLabel86.StylePriority.UseForeColor = false;
            this.xrLabel86.Text = "客訴問題";
            // 
            // xrLabel83
            // 
            this.xrLabel83.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel83.ForeColor = System.Drawing.Color.Black;
            this.xrLabel83.LocationFloat = new DevExpress.Utils.PointFloat(290F, 220F);
            this.xrLabel83.Name = "xrLabel83";
            this.xrLabel83.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel83.SizeF = new System.Drawing.SizeF(61F, 14.70824F);
            this.xrLabel83.StyleName = "FieldCaption";
            this.xrLabel83.StylePriority.UseFont = false;
            this.xrLabel83.StylePriority.UseForeColor = false;
            this.xrLabel83.Text = "客訴編號";
            // 
            // xrLabel84
            // 
            this.xrLabel84.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel84.ForeColor = System.Drawing.Color.Black;
            this.xrLabel84.LocationFloat = new DevExpress.Utils.PointFloat(290F, 250F);
            this.xrLabel84.Name = "xrLabel84";
            this.xrLabel84.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel84.SizeF = new System.Drawing.SizeF(61.33325F, 14F);
            this.xrLabel84.StyleName = "FieldCaption";
            this.xrLabel84.StylePriority.UseFont = false;
            this.xrLabel84.StylePriority.UseForeColor = false;
            this.xrLabel84.Text = "派工人員";
            // 
            // xrLabel85
            // 
            this.xrLabel85.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel85.ForeColor = System.Drawing.Color.Black;
            this.xrLabel85.LocationFloat = new DevExpress.Utils.PointFloat(290F, 280F);
            this.xrLabel85.Name = "xrLabel85";
            this.xrLabel85.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel85.SizeF = new System.Drawing.SizeF(60.00003F, 14F);
            this.xrLabel85.StyleName = "FieldCaption";
            this.xrLabel85.StylePriority.UseFont = false;
            this.xrLabel85.StylePriority.UseForeColor = false;
            this.xrLabel85.Text = "派工日";
            // 
            // xrLabel78
            // 
            this.xrLabel78.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel78.ForeColor = System.Drawing.Color.Black;
            this.xrLabel78.LocationFloat = new DevExpress.Utils.PointFloat(6F, 280F);
            this.xrLabel78.Name = "xrLabel78";
            this.xrLabel78.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel78.SizeF = new System.Drawing.SizeF(60.49991F, 16.00002F);
            this.xrLabel78.StyleName = "FieldCaption";
            this.xrLabel78.StylePriority.UseFont = false;
            this.xrLabel78.StylePriority.UseForeColor = false;
            this.xrLabel78.Text = "客訴原因";
            // 
            // xrLabel79
            // 
            this.xrLabel79.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel79.ForeColor = System.Drawing.Color.Black;
            this.xrLabel79.LocationFloat = new DevExpress.Utils.PointFloat(6F, 250F);
            this.xrLabel79.Name = "xrLabel79";
            this.xrLabel79.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel79.SizeF = new System.Drawing.SizeF(60.49991F, 16.54167F);
            this.xrLabel79.StyleName = "FieldCaption";
            this.xrLabel79.StylePriority.UseFont = false;
            this.xrLabel79.StylePriority.UseForeColor = false;
            this.xrLabel79.Text = "派工別";
            // 
            // xrLabel80
            // 
            this.xrLabel80.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel80.ForeColor = System.Drawing.Color.Black;
            this.xrLabel80.LocationFloat = new DevExpress.Utils.PointFloat(6F, 220F);
            this.xrLabel80.Name = "xrLabel80";
            this.xrLabel80.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel80.SizeF = new System.Drawing.SizeF(61.49991F, 13.99992F);
            this.xrLabel80.StyleName = "FieldCaption";
            this.xrLabel80.StylePriority.UseFont = false;
            this.xrLabel80.StylePriority.UseForeColor = false;
            this.xrLabel80.Text = "聯絡人";
            // 
            // xrLabel81
            // 
            this.xrLabel81.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel81.ForeColor = System.Drawing.Color.Black;
            this.xrLabel81.LocationFloat = new DevExpress.Utils.PointFloat(6F, 190F);
            this.xrLabel81.Name = "xrLabel81";
            this.xrLabel81.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel81.SizeF = new System.Drawing.SizeF(60.49991F, 16.99999F);
            this.xrLabel81.StyleName = "FieldCaption";
            this.xrLabel81.StylePriority.UseFont = false;
            this.xrLabel81.StylePriority.UseForeColor = false;
            this.xrLabel81.Text = "聯絡電話";
            // 
            // xrLabel82
            // 
            this.xrLabel82.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel82.ForeColor = System.Drawing.Color.Black;
            this.xrLabel82.LocationFloat = new DevExpress.Utils.PointFloat(6F, 160F);
            this.xrLabel82.Name = "xrLabel82";
            this.xrLabel82.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel82.SizeF = new System.Drawing.SizeF(60.49991F, 14.50001F);
            this.xrLabel82.StyleName = "FieldCaption";
            this.xrLabel82.StylePriority.UseFont = false;
            this.xrLabel82.StylePriority.UseForeColor = false;
            this.xrLabel82.Text = "客戶住址";
            // 
            // xrLabel77
            // 
            this.xrLabel77.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel77.ForeColor = System.Drawing.Color.Black;
            this.xrLabel77.LocationFloat = new DevExpress.Utils.PointFloat(6F, 40F);
            this.xrLabel77.Name = "xrLabel77";
            this.xrLabel77.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel77.SizeF = new System.Drawing.SizeF(60.5F, 14F);
            this.xrLabel77.StyleName = "FieldCaption";
            this.xrLabel77.StylePriority.UseFont = false;
            this.xrLabel77.StylePriority.UseForeColor = false;
            this.xrLabel77.Text = "社區IP ";
            // 
            // xrLabel76
            // 
            this.xrLabel76.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel76.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.xrLabel76.LocationFloat = new DevExpress.Utils.PointFloat(520F, 280.85F);
            this.xrLabel76.Name = "xrLabel76";
            this.xrLabel76.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel76.SizeF = new System.Drawing.SizeF(75.95856F, 14.08354F);
            this.xrLabel76.StyleName = "FieldCaption";
            this.xrLabel76.StylePriority.UseFont = false;
            this.xrLabel76.StylePriority.UseForeColor = false;
            this.xrLabel76.Text = "預定施工人";
            // 
            // xrLine12
            // 
            this.xrLine12.LocationFloat = new DevExpress.Utils.PointFloat(2F, 300F);
            this.xrLine12.Name = "xrLine12";
            this.xrLine12.SizeF = new System.Drawing.SizeF(722F, 2F);
            // 
            // xrLabel75
            // 
            this.xrLabel75.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel75.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.xrLabel75.LocationFloat = new DevExpress.Utils.PointFloat(521F, 248.44F);
            this.xrLabel75.Name = "xrLabel75";
            this.xrLabel75.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel75.SizeF = new System.Drawing.SizeF(74.95856F, 14F);
            this.xrLabel75.StyleName = "FieldCaption";
            this.xrLabel75.StylePriority.UseFont = false;
            this.xrLabel75.StylePriority.UseForeColor = false;
            this.xrLabel75.Text = "WTL申請日";
            // 
            // xrLine11
            // 
            this.xrLine11.LocationFloat = new DevExpress.Utils.PointFloat(2F, 270F);
            this.xrLine11.Name = "xrLine11";
            this.xrLine11.SizeF = new System.Drawing.SizeF(722F, 2F);
            // 
            // xrLabel74
            // 
            this.xrLabel74.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel74.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.xrLabel74.LocationFloat = new DevExpress.Utils.PointFloat(524F, 219.7082F);
            this.xrLabel74.Name = "xrLabel74";
            this.xrLabel74.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel74.SizeF = new System.Drawing.SizeF(67.95825F, 14F);
            this.xrLabel74.StyleName = "FieldCaption";
            this.xrLabel74.StylePriority.UseFont = false;
            this.xrLabel74.StylePriority.UseForeColor = false;
            this.xrLabel74.Text = "受理時間";
            // 
            // xrLine10
            // 
            this.xrLine10.LocationFloat = new DevExpress.Utils.PointFloat(4F, 240F);
            this.xrLine10.Name = "xrLine10";
            this.xrLine10.SizeF = new System.Drawing.SizeF(720.0001F, 2F);
            // 
            // xrLabel73
            // 
            this.xrLabel73.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel73.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.xrLabel73.LocationFloat = new DevExpress.Utils.PointFloat(524F, 190F);
            this.xrLabel73.Name = "xrLabel73";
            this.xrLabel73.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel73.SizeF = new System.Drawing.SizeF(60.00003F, 14F);
            this.xrLabel73.StyleName = "FieldCaption";
            this.xrLabel73.StylePriority.UseFont = false;
            this.xrLabel73.StylePriority.UseForeColor = false;
            this.xrLabel73.Text = "欠拆";
            // 
            // xrLine9
            // 
            this.xrLine9.LocationFloat = new DevExpress.Utils.PointFloat(2F, 210F);
            this.xrLine9.Name = "xrLine9";
            this.xrLine9.SizeF = new System.Drawing.SizeF(722F, 2F);
            // 
            // xrLabel72
            // 
            this.xrLabel72.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel72.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.xrLabel72.LocationFloat = new DevExpress.Utils.PointFloat(521F, 160F);
            this.xrLabel72.Name = "xrLabel72";
            this.xrLabel72.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel72.SizeF = new System.Drawing.SizeF(60.00003F, 14F);
            this.xrLabel72.StyleName = "FieldCaption";
            this.xrLabel72.StylePriority.UseFont = false;
            this.xrLabel72.StylePriority.UseForeColor = false;
            this.xrLabel72.Text = "退租日";
            // 
            // xrLine8
            // 
            this.xrLine8.LocationFloat = new DevExpress.Utils.PointFloat(2F, 180F);
            this.xrLine8.Name = "xrLine8";
            this.xrLine8.SizeF = new System.Drawing.SizeF(722F, 2F);
            // 
            // xrLabel69
            // 
            this.xrLabel69.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel69.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.xrLabel69.LocationFloat = new DevExpress.Utils.PointFloat(524F, 129.75F);
            this.xrLabel69.Name = "xrLabel69";
            this.xrLabel69.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel69.SizeF = new System.Drawing.SizeF(60.00003F, 14F);
            this.xrLabel69.StyleName = "FieldCaption";
            this.xrLabel69.StylePriority.UseFont = false;
            this.xrLabel69.StylePriority.UseForeColor = false;
            this.xrLabel69.Text = "到期日";
            // 
            // xrLabel70
            // 
            this.xrLabel70.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel70.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.xrLabel70.LocationFloat = new DevExpress.Utils.PointFloat(524F, 98.17F);
            this.xrLabel70.Name = "xrLabel70";
            this.xrLabel70.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel70.SizeF = new System.Drawing.SizeF(60.00003F, 14F);
            this.xrLabel70.StyleName = "FieldCaption";
            this.xrLabel70.StylePriority.UseFont = false;
            this.xrLabel70.StylePriority.UseForeColor = false;
            this.xrLabel70.Text = "計費日";
            // 
            // xrLabel71
            // 
            this.xrLabel71.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel71.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.xrLabel71.LocationFloat = new DevExpress.Utils.PointFloat(524F, 67.33331F);
            this.xrLabel71.Name = "xrLabel71";
            this.xrLabel71.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel71.SizeF = new System.Drawing.SizeF(61F, 14F);
            this.xrLabel71.StyleName = "FieldCaption";
            this.xrLabel71.StylePriority.UseFont = false;
            this.xrLabel71.StylePriority.UseForeColor = false;
            this.xrLabel71.Text = "報竣日";
            // 
            // xrLabel68
            // 
            this.xrLabel68.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel68.ForeColor = System.Drawing.Color.Black;
            this.xrLabel68.LocationFloat = new DevExpress.Utils.PointFloat(290F, 130F);
            this.xrLabel68.Name = "xrLabel68";
            this.xrLabel68.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel68.SizeF = new System.Drawing.SizeF(60.00003F, 14F);
            this.xrLabel68.StyleName = "FieldCaption";
            this.xrLabel68.StylePriority.UseFont = false;
            this.xrLabel68.StylePriority.UseForeColor = false;
            this.xrLabel68.Text = "公關機";
            // 
            // xrLabel66
            // 
            this.xrLabel66.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel66.ForeColor = System.Drawing.Color.Black;
            this.xrLabel66.LocationFloat = new DevExpress.Utils.PointFloat(290F, 70F);
            this.xrLabel66.Name = "xrLabel66";
            this.xrLabel66.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel66.SizeF = new System.Drawing.SizeF(61F, 14F);
            this.xrLabel66.StyleName = "FieldCaption";
            this.xrLabel66.StylePriority.UseFont = false;
            this.xrLabel66.StylePriority.UseForeColor = false;
            this.xrLabel66.Text = "客戶 IP";
            // 
            // xrLabel67
            // 
            this.xrLabel67.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel67.ForeColor = System.Drawing.Color.Black;
            this.xrLabel67.LocationFloat = new DevExpress.Utils.PointFloat(290F, 100F);
            this.xrLabel67.Name = "xrLabel67";
            this.xrLabel67.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel67.SizeF = new System.Drawing.SizeF(60.00003F, 14F);
            this.xrLabel67.StyleName = "FieldCaption";
            this.xrLabel67.StylePriority.UseFont = false;
            this.xrLabel67.StylePriority.UseForeColor = false;
            this.xrLabel67.Text = "第二戶";
            // 
            // xrLabel39
            // 
            this.xrLabel39.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel39.ForeColor = System.Drawing.Color.Black;
            this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(6F, 130F);
            this.xrLabel39.Name = "xrLabel39";
            this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel39.SizeF = new System.Drawing.SizeF(60.49991F, 16.00002F);
            this.xrLabel39.StyleName = "FieldCaption";
            this.xrLabel39.StylePriority.UseFont = false;
            this.xrLabel39.StylePriority.UseForeColor = false;
            this.xrLabel39.Text = "繳費週期";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.ForeColor = System.Drawing.Color.Black;
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(6F, 100F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(60.49991F, 16.54167F);
            this.xrLabel4.StyleName = "FieldCaption";
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseForeColor = false;
            this.xrLabel4.Text = "方案類型";
            // 
            // xrLine7
            // 
            this.xrLine7.LocationFloat = new DevExpress.Utils.PointFloat(2F, 150F);
            this.xrLine7.Name = "xrLine7";
            this.xrLine7.SizeF = new System.Drawing.SizeF(722F, 2F);
            // 
            // xrLine6
            // 
            this.xrLine6.LocationFloat = new DevExpress.Utils.PointFloat(2F, 120F);
            this.xrLine6.Name = "xrLine6";
            this.xrLine6.SizeF = new System.Drawing.SizeF(722F, 2F);
            // 
            // xrLine5
            // 
            this.xrLine5.LocationFloat = new DevExpress.Utils.PointFloat(2F, 90F);
            this.xrLine5.Name = "xrLine5";
            this.xrLine5.SizeF = new System.Drawing.SizeF(722F, 2F);
            // 
            // xrLine4
            // 
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(2F, 60F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(722F, 2F);
            // 
            // xrShape1
            // 
            this.xrShape1.LocationFloat = new DevExpress.Utils.PointFloat(1.000046F, 1.999987F);
            this.xrShape1.Name = "xrShape1";
            this.xrShape1.Shape = shapeLine65;
            this.xrShape1.SizeF = new System.Drawing.SizeF(2F, 28.00001F);
            // 
            // xrLine3
            // 
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(2F, 30F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(721.0001F, 2F);
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.ForeColor = System.Drawing.Color.Black;
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(6.000169F, 10.00001F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(64.67F, 14F);
            this.xrLabel5.StyleName = "FieldCaption";
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseForeColor = false;
            this.xrLabel5.Text = "社區名稱";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel7.ForeColor = System.Drawing.Color.Black;
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(290F, 10F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(61F, 14F);
            this.xrLabel7.StyleName = "FieldCaption";
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseForeColor = false;
            this.xrLabel7.Text = "社區類別";
            // 
            // xrLabel13
            // 
            this.xrLabel13.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel13.ForeColor = System.Drawing.Color.Black;
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(6F, 70F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(61.49991F, 16.29165F);
            this.xrLabel13.StyleName = "FieldCaption";
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseForeColor = false;
            this.xrLabel13.Text = "客戶名稱";
            // 
            // xrLabel22
            // 
            this.xrLabel22.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(524F, 10F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(61.04F, 14F);
            this.xrLabel22.StyleName = "FieldCaption";
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UseForeColor = false;
            this.xrLabel22.Text = "主線速率";
            // 
            // xrLabel36
            // 
            this.xrLabel36.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CASEKIND")});
            this.xrLabel36.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(78.00001F, 98.17F);
            this.xrLabel36.Name = "xrLabel36";
            this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel36.SizeF = new System.Drawing.SizeF(207.9999F, 18F);
            this.xrLabel36.StylePriority.UseFont = false;
            // 
            // xrLabel37
            // 
            this.xrLabel37.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.COMN")});
            this.xrLabel37.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(76.5001F, 10.00001F);
            this.xrLabel37.Name = "xrLabel37";
            this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel37.SizeF = new System.Drawing.SizeF(209.4999F, 14F);
            this.xrLabel37.StylePriority.UseFont = false;
            this.xrLabel37.Text = "xrLabel37";
            // 
            // xrLabel41
            // 
            this.xrLabel41.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CUSNC")});
            this.xrLabel41.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(77.4999F, 68.99999F);
            this.xrLabel41.Name = "xrLabel41";
            this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel41.SizeF = new System.Drawing.SizeF(208.5001F, 17.00002F);
            this.xrLabel41.StylePriority.UseFont = false;
            this.xrLabel41.Text = "xrLabel41";
            // 
            // xrLabel42
            // 
            this.xrLabel42.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.CUSTIP")});
            this.xrLabel42.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(358.0416F, 70.00002F);
            this.xrLabel42.Name = "xrLabel42";
            this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel42.SizeF = new System.Drawing.SizeF(157.9166F, 16F);
            this.xrLabel42.StylePriority.UseFont = false;
            this.xrLabel42.Text = "xrLabel42";
            // 
            // xrLabel45
            // 
            this.xrLabel45.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.FAQMAN")});
            this.xrLabel45.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel45.LocationFloat = new DevExpress.Utils.PointFloat(78.00001F, 219.7082F);
            this.xrLabel45.Name = "xrLabel45";
            this.xrLabel45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel45.SizeF = new System.Drawing.SizeF(207.9999F, 17.00002F);
            this.xrLabel45.StylePriority.UseFont = false;
            this.xrLabel45.Text = "xrLabel45";
            // 
            // xrLabel47
            // 
            this.xrLabel47.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.FAQREASONNM")});
            this.xrLabel47.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel47.LocationFloat = new DevExpress.Utils.PointFloat(75.99999F, 277.2499F);
            this.xrLabel47.Name = "xrLabel47";
            this.xrLabel47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel47.SizeF = new System.Drawing.SizeF(209.9999F, 16.75006F);
            this.xrLabel47.StylePriority.UseFont = false;
            this.xrLabel47.Text = "xrLabel47";
            // 
            // xrLabel48
            // 
            this.xrLabel48.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.docketdat", "{0:yyyy/MM/dd}")});
            this.xrLabel48.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel48.LocationFloat = new DevExpress.Utils.PointFloat(600.9586F, 67.00001F);
            this.xrLabel48.Name = "xrLabel48";
            this.xrLabel48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel48.SizeF = new System.Drawing.SizeF(120.0414F, 17F);
            this.xrLabel48.StylePriority.UseFont = false;
            // 
            // xrLabel54
            // 
            this.xrLabel54.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.LINERATE")});
            this.xrLabel54.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel54.LocationFloat = new DevExpress.Utils.PointFloat(600.96F, 10.00001F);
            this.xrLabel54.Name = "xrLabel54";
            this.xrLabel54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel54.SizeF = new System.Drawing.SizeF(120.04F, 14F);
            this.xrLabel54.StylePriority.UseFont = false;
            this.xrLabel54.Text = "xrLabel54";
            // 
            // xrLabel55
            // 
            this.xrLabel55.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.LINKNO")});
            this.xrLabel55.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel55.LocationFloat = new DevExpress.Utils.PointFloat(354F, 219.7082F);
            this.xrLabel55.Name = "xrLabel55";
            this.xrLabel55.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel55.SizeF = new System.Drawing.SizeF(161.9582F, 15.00003F);
            this.xrLabel55.StylePriority.UseFont = false;
            this.xrLabel55.Text = "xrLabel55";
            // 
            // xrLabel56
            // 
            this.xrLabel56.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.MEMO")});
            this.xrLabel56.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel56.LocationFloat = new DevExpress.Utils.PointFloat(76.5001F, 305.854F);
            this.xrLabel56.Name = "xrLabel56";
            this.xrLabel56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel56.SizeF = new System.Drawing.SizeF(640.5001F, 51.33337F);
            this.xrLabel56.StylePriority.UseFont = false;
            this.xrLabel56.Text = "xrLabel56";
            // 
            // xrLabel57
            // 
            this.xrLabel57.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.RADDR")});
            this.xrLabel57.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel57.LocationFloat = new DevExpress.Utils.PointFloat(75.99999F, 158.2292F);
            this.xrLabel57.Name = "xrLabel57";
            this.xrLabel57.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel57.SizeF = new System.Drawing.SizeF(439.9583F, 16.27077F);
            this.xrLabel57.StylePriority.UseFont = false;
            this.xrLabel57.Text = "xrLabel57";
            // 
            // xrLabel59
            // 
            this.xrLabel59.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.SECONDCASE")});
            this.xrLabel59.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel59.LocationFloat = new DevExpress.Utils.PointFloat(358.0416F, 99.91998F);
            this.xrLabel59.Name = "xrLabel59";
            this.xrLabel59.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel59.SizeF = new System.Drawing.SizeF(157.9166F, 16.25F);
            this.xrLabel59.StylePriority.UseFont = false;
            this.xrLabel59.Text = "xrLabel59";
            // 
            // xrLabel62
            // 
            this.xrLabel62.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.TEL")});
            this.xrLabel62.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel62.LocationFloat = new DevExpress.Utils.PointFloat(75.99999F, 190F);
            this.xrLabel62.Name = "xrLabel62";
            this.xrLabel62.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel62.SizeF = new System.Drawing.SizeF(439.9583F, 16.99995F);
            this.xrLabel62.StylePriority.UseFont = false;
            this.xrLabel62.Text = "xrLabel62";
            // 
            // xrLabel64
            // 
            this.xrLabel64.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.WORKTYPE")});
            this.xrLabel64.Font = new System.Drawing.Font("標楷體", 10F);
            this.xrLabel64.LocationFloat = new DevExpress.Utils.PointFloat(76.5001F, 249.5417F);
            this.xrLabel64.Name = "xrLabel64";
            this.xrLabel64.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel64.SizeF = new System.Drawing.SizeF(209.4999F, 16.99995F);
            this.xrLabel64.StylePriority.UseFont = false;
            this.xrLabel64.Text = "xrLabel64";
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(2F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(722F, 2F);
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(2F, 360F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(722F, 2F);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 50F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 50F;
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
            // xrLabel31
            // 
            this.xrLabel31.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel31.ForeColor = System.Drawing.Color.Black;
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(78.49989F, 18F);
            this.xrLabel31.StyleName = "FieldCaption";
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.StylePriority.UseForeColor = false;
            this.xrLabel31.Text = "派工單號：";
            // 
            // xrLabel63
            // 
            this.xrLabel63.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.WORKNO")});
            this.xrLabel63.LocationFloat = new DevExpress.Utils.PointFloat(78.49989F, 0F);
            this.xrLabel63.Name = "xrLabel63";
            this.xrLabel63.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel63.SizeF = new System.Drawing.SizeF(162.75F, 18F);
            this.xrLabel63.Text = "xrLabel63";
            // 
            // pageFooterBand1
            // 
            this.pageFooterBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2});
            this.pageFooterBand1.HeightF = 29F;
            this.pageFooterBand1.Name = "pageFooterBand1";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Format = "Page {0} of {1}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(331F, 6F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(313F, 23F);
            this.xrPageInfo2.StyleName = "PageInfo";
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // reportHeaderBand1
            // 
            this.reportHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo3,
            this.xrPageInfo1,
            this.xrLabel65,
            this.xrLabel31,
            this.xrLabel63});
            this.reportHeaderBand1.HeightF = 53F;
            this.reportHeaderBand1.Name = "reportHeaderBand1";
            // 
            // xrPageInfo3
            // 
            this.xrPageInfo3.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo3.Format = "列印時間：{0:HH:mm:ss}";
            this.xrPageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(580.0001F, 28.00002F);
            this.xrPageInfo3.Name = "xrPageInfo3";
            this.xrPageInfo3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo3.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo3.SizeF = new System.Drawing.SizeF(147F, 14.99998F);
            this.xrPageInfo3.StyleName = "PageInfo";
            this.xrPageInfo3.StylePriority.UseFont = false;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("標楷體", 10F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo1.Format = "列印日期：{0:yyyy/MM/dd}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(580.0001F, 0F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(147F, 18F);
            this.xrPageInfo1.StyleName = "PageInfo";
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrPageInfo1_BeforePrint);
            // 
            // xrLabel65
            // 
            this.xrLabel65.AutoWidth = true;
            this.xrLabel65.Font = new System.Drawing.Font("標楷體", 20F, System.Drawing.FontStyle.Bold);
            this.xrLabel65.ForeColor = System.Drawing.Color.Black;
            this.xrLabel65.LocationFloat = new DevExpress.Utils.PointFloat(280.0001F, 0F);
            this.xrLabel65.Name = "xrLabel65";
            this.xrLabel65.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel65.SizeF = new System.Drawing.SizeF(223F, 35F);
            this.xrLabel65.StyleName = "Title";
            this.xrLabel65.StylePriority.UseFont = false;
            this.xrLabel65.StylePriority.UseForeColor = false;
            this.xrLabel65.StylePriority.UseTextAlignment = false;
            this.xrLabel65.Text = "客訴服務派工單";
            this.xrLabel65.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
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
            // RT205R
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.pageFooterBand1,
            this.reportHeaderBand1});
            this.Bookmark = "RT205R";
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "Query";
            this.DataSource = this.sqlDataSource1;
            this.DisplayName = "客訴服務派工單";
            this.Margins = new System.Drawing.Printing.Margins(48, 48, 50, 50);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.FieldCaption,
            this.PageInfo,
            this.DataField});
            this.Version = "15.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void xrPageInfo1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        
    }
}
