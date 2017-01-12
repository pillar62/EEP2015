using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infolight.EasilyReportTools.Tools;
using Srvtools;
using Infolight.EasilyReportTools.Config;
using System.Drawing;
using System.Collections;
using AjaxControlToolkit;
using System.Drawing.Text;
using Infolight.EasilyReportTools.DataCenter;
using System.Data;
using System.IO;
using System.Net.Mail;

namespace Infolight.EasilyReportTools.UI
{
    internal class AspNetRender: IRender
    {
        private WebEasilyReport report;

        public AspNetRender(WebEasilyReport _report)
        {
            report = _report;
        }

        #region Constant
        #region Define Name
        const string HEADER = "Header";
        const string FOOTER = "Footer";
        const string GROUP = "Group";
         #endregion

        #region Define ControlID
        const string MULTIVIEW = "multiview";

        const string TABLE_ITEM = "tableItem";
        const string DROPDOWNLIST_ITEM_CONTENT = "dropdownlistItemContent";
        const string DROPDOWNLIST_ITEM_CONTENT_ALIGNMENT = "dropdownlistItemContentAlignment";
        const string DROPDOWNLIST_ITEM_POSITION = "dropdownlistItemPosition";
        const string TEXTBOX_ITEM_CELLS = "textboxItemCells";
        const string CHECKBOX_ITEM_NEWLINE = "checkboxItemNewLine";
        const string TEXTBOX_ITEM_FORMAT = "textboxItemFormat";
        const string BUTTON_ITEM_DETAIL_FONT = "buttonItemDetailFont";
        const string LABEL_ITEM_DETAIL_FONT_SAMPLE = "labelItemDetailFont";
        const string BUTTON_ITEM_FONT = "buttonItemFont";
        const string LABEL_ITEM_FONT_SAMPLE = "labelItemFont";
        const string DROPDOWNLIST_ITEM_TYPE = "dropdownlistItemType";
        const string BUTTON_ITEM_ADD = "buttonItemAdd";
        const string BUTTON_ITEM_REMOVE = "buttonItemRemove";
        const string LISTBOX_ITEM = "listboxItem";
        const string BUTTON_ITEM_UP = "buttonItemUp";
        const string BUTTON_ITEM_DOWN = "buttonItemDown";

        const string MULTIVIEW_FIELD_CONTENT = "multiviewFieldContent";
        const string MULTIVIEW_FIELD = "multiviewField";
        const string DROPDOWNLIST_FIELD = "dropdownlistField";
        const string TEXTBOX_FIELD_CAPTION = "textboxFieldCaption";
        const string DROPDOWNLIST_FIELD_CAPTION_ALIGNMENT = "dropdownlistFieldCaptionAligment";
        const string DROPDOWNLIST_FIELD_COLUMN_ALIGNMENT = "dropdownlistFieldColumnAligment";
        const string TEXTBOX_FIELD_WIDTH = "textboxFieldWidth";
        const string TEXTBOX_FIELD_FORMAT = "textboxFieldFormat";
        const string CHECKBOX_FIELD_NEWLINE = "checkFieldNewLine";
        const string CHECKBOX_FIELD_SUPPRESSIFDUPLICATED = "checkboxFieldSuppressIfDuplicated";
        const string TEXTBOX_FIELD_CELLS = "textboxFieldCells";
        const string TEXTBOX_FIELD_NEWLINE_POSITION = "textboxFieldNewlinePosition";
        const string DROPDOWNLIST_FIELD_SUM_TYPE = "dropdownlistFieldSumType";
        const string DROPDOWNLIST_FIELD_ORDERBY = "dropdownlistFieldOrderby";
        const string CHECKBOX_FIELD_GROUP_TOTAL = "checkboxFieldGroupTotal";
        const string TEXTBOX_FIELD_GROUP_TOTAL_CAPTION = "textboxFieldGroupTotalCaption";
        const string DROPDOWNLIST_FIELD_GROUP_GAP = "dropdownlistFieldGroupGap";
        const string DROPDOWNLIST_FIELD_GROUP_TYPE = "dropdownlistFieldGroupType";
        const string TEXTBOX_FIELD_TOTAL_CAPTION = "textboxFieldTotalCaption";
        const string DROPDOWNLIST_CAPTION_STYLE = "dropdownlistFieldCaptionStyle";
        const string BUTTON_FIELD_FONT = "buttonFieldFont";
        const string LABEL_FIELD_FONT_SAMPLE = "labelFieldFont";
        const string LISTBOX_FIELD_ALL = "listboxFieldAll";
        const string BUTTON_FIELD_ADD = "buttonFieldAdd";
        const string BUTTON_FIELD_REMOVE = "buttonFieldRemove";
        const string BUTTON_FIELD_ADD_ALL = "buttonFieldAddAll";
        const string BUTTON_FIELD_REMOVE_ALL = "buttonFieldRemoveAll";
        const string LISTBOX_FIELD_SELECTED = "listboxFieldSelected";
        const string BUTTON_FIELD_UP = "buttonFieldUp";
        const string BUTTON_FIELD_DOWN = "buttonFieldDown";

        const string RADIOBUTTONLIST_PAGE_SIZE = "radiobuttonlistPageSize";
        const string RADIOBUTTONLIST_PRINT_ORIENTATION = "radiobuttonlistPrintOrientation";
        const string TEXTBOX_PAGE_RECORDS = "textboxPageRecords";
        const string DROPDOWNLIST_EXPORT_FORMAT = "dropdownlistExportFormat";
        const string TEXTBOX_MAIL_TO = "textboxMailTo";
        const string TEXTBOX_MAIL_FROM = "textboxMailFrom";
        const string TEXTBOX_MAIL_PASSWORD = "textboxPassword";
        const string TEXTBOX_MAIL_SERVER = "textboxServer";
        const string TEXTBOX_MAIL_TITLE = "textboxMailTitle";

        const string DROPDOWNLIST_FONT = "dropdownlistFont";
        const string TEXTBOX_FONT_SIZE = "textboxFontSize";
        const string CHECKBOX_FONT_BOLD = "checkboxFontBold";
        const string CHECKBOX_FONT_ITALIC = "checkboxFontItalic";
        const string CHECKBOX_FONT_UNDERLINE = "checkboxFontUnderline";
        const string CHECKBOX_FONT_STRIKEOUT = "checkboxFontStrikeout";

        const string BUTTON_TEMPLATE_OK = "buttonTemplateOK";
        const string BUTTON_TEMPLATE_CANCEL = "buttonTemplateCancel";
        const string BUTTON_TEMPLATE_DELETE = "buttonTemplateDelete";

        const string TEXTBOX_TEMPLATE_SAVE_AS_FILENAME = "textboxTemplateSaveAsFileName";

        const string IMAGE_PICTURE = "imagePicture";
        const string LISTBOX_TEMPLATES = "listboxTemplates";
        const string BUTTON_ADD_PICTURE = "buttonAddPicture";
        const string BUTTON_PREVIEW_PICTURE = "buttonPreviewPicture";
        const string FILEUPLOAD_PICTURE = "fileUploadPicture";
        const string BUTTON_FILEUPLOAD_OK = "buttonFileUploadOK";
        const string LISTBOX_PICTURES = "listboxPictures";

        const string CHECKBOX_COLUMNGRID = "checkboxColumnGrid";
        const string CHECKBOX_ROWGRID = "checkboxRowGrid";
        const string CHECKBOX_INNER_COLUMNGRID = "checkboxInnerColumnGrid";

        //Database View
        const string PANEL_DATABASE = "panelDatabase";
        const string BUTTON_TEMPLATE_LOAD = "buttonTemplateLoad";
        const string BUTTON_POPUP_TEMPLATE = "buttonPopupTemplate";
        const string MODEL_POPUP_EXTENDER_TEMPLATE_LOAD = "modelPopupExtenderTemplateLoad";

        //Font View
        const string PANEL_FONT = "panelFontView";
        const string BUTTON_OPEN_FONT = "buttonOpenFontView";
        const string BUTTON_POPUP_FONTVIEW = "buttonPopupFontView";
        const string MODEL_POPUP_EXTENDER_FONTVIEW = "modelPopupExtenderFontView";
        const string BUTTON_FONT_OK = "buttonFontOK";
        const string BUTTON_FONT_CANCEL = "buttonFontCancel";

        //Save as View
        const string PANEL_SAVE_AS = "panelSaveAsView";
        const string BUTTON_OPEN_SAVEASVIEW = "buttonOpenSaveAsView";
        const string BUTTON_POPUP_SAVEASVIEW = "buttonPopupSaveAsView";
        const string MODEL_POPUP_EXTENDER_SAVEASVIEW = "modelPopupExtenderSaveAsView";
        const string BUTTON_SAVEAS_TEMPLATE_OK = "buttonSaveAsTemplateOK";
        const string BUTTON_SAVEAS_TEMPLATE_CANCEL = "buttonSaveAsTemplateCancel";

        //Picture View
        const string PANEL_PICTURE_VIEW = "panelPictureView";
        const string BUTTON_OPEN_PICTUREVIEW = "buttonOpenPictureView";
        const string BUTTON_POPUP_PICTUREVIEW = "buttonPopupPictureView";
        const string MODEL_POPUP_EXTENDER_PICTUREVIEW = "modelPopupExtenderPictureView";
        const string BUTTON_PICTURE_CANCEL = "buttonPictureCancel";

        //FileUpload View
        const string PANEL_FILEUPLOAD_VIEW = "panelFileUploadView";

        //Download View
        const string PANEL_DOWNLOAD_VIEW = "panelDownloadView";
        const string BUTTON_POPUP_DOWNLOADVIEW = "buttonPopupDownloadView";
        const string MODEL_POPUP_EXTENDER_DOWNLOADVIEW = "modelPopupExtenderDownloadView";
        const string HYPERLINK_DOWNLOAD = "hyperlinkDownload_eReport";
        const string BUTTON_CLOSE_DOWNLOADVIEW = "buttonCloseDownloadView";

        //OutputView
        const string MULTIVIEW_OUTPUTVIEW = "multiViewOutputView";

        //SettingView
        const string TEXTBOX_LEFTMARGIN = "textboxLeftMargin";
        const string TEXTBOX_RIGHTMARGIN = "textboxRightMargin";
        const string TEXTBOX_TOPMARGIN = "textboxTopMargin";
        const string TEXTBOX_BOTTOMMARGIN = "textboxBottomMargin";
        const string PANEL_PAGE_MARGIN = "panelPageMargin";
        const string CHECKBOX_SEND_MAIL = "checkboxSendMail";
        const string CHECKBOX_HEADER_REPEAT = "checkboxHeaderRepeat";

        //SendMailView
        const string TEXTBOX_SENDMAIL_TO = "textboxSendMailTo";
        const string TEXTBOX_SENDMAIL_FROM = "textboxSendMailFrom";
        const string TEXTBOX_SENDMAIL_PASSWORD = "textboxSendMailPassword";
        const string TEXTBOX_SENDMAIL_SERVER = "textboxSendMailServer";
        const string TEXTBOX_SENDMAIL_TITLE = "textboxSendMailTitle";
        const string BUTTON_SENDMAIL = "buttonSendMail";
        #endregion
        #endregion

        public HtmlTextWriter RenderIframe(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "file");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "file");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, "../InnerPages/frmUploadFile.aspx");
            writer.RenderBeginTag(HtmlTextWriterTag.Iframe);
            writer.RenderEndTag();
            return writer;
        }

        public void CreateChildControls()
        {
            #region Create Child Controls
            WebMultiViewCaptions webMultiViewCaptions = new WebMultiViewCaptions();
            webMultiViewCaptions.ID = "webMultiViewCaptions";
            webMultiViewCaptions.MultiViewID = MULTIVIEW;
            webMultiViewCaptions.TableStyle = WebMultiViewCaptionStyle.Style3;
            webMultiViewCaptions.CssClass = "eReport_multiViewCaption";
            string[] caption = new string[] { "Header", "Field", "Footer", "Setting" };
            foreach (string str in caption)
            {
                WebMultiViewCaption viewcaption = new WebMultiViewCaption();
                viewcaption.Caption = str;
                webMultiViewCaptions.Captions.Add(viewcaption);
            }
            this.report.Controls.Add(webMultiViewCaptions);

            MultiView multiview = new MultiView();
            multiview.ID = MULTIVIEW;
            //multiview
            multiview.Views.Add(CreateItemView(HEADER));
            multiview.Views.Add(CreateFieldView());
            multiview.Views.Add(CreateItemView(FOOTER));
            multiview.Views.Add(CreateSettingView());
            this.report.Controls.Add(multiview);
            multiview.ActiveViewIndex = ViewNum.HeaderItemView;

            Table tableButtons = CreateTable(1, 4, null, null);

            Button buttonLoad = new Button();
            buttonLoad.CssClass = "eReport_button eReport_normal_button";
            buttonLoad.Text = ERptMultiLanguage.GetLanValue("MenuReadTemplate");
            buttonLoad.ID = BUTTON_TEMPLATE_LOAD;
            buttonLoad.Click += new EventHandler(buttonLoad_Click);
            //this.report.Controls.Add(buttonLoad);
            tableButtons.Rows[0].Cells[0].Controls.Add(buttonLoad);

            Button buttonSave = new Button();
            buttonSave.CssClass = "eReport_button eReport_normal_button";
            buttonSave.Text = ERptMultiLanguage.GetLanValue("MenuSaveTemplate");
            buttonSave.Click += new EventHandler(buttonSave_Click);
            //this.report.Controls.Add(buttonSave);
            tableButtons.Rows[0].Cells[1].Controls.Add(buttonSave);

            //Button buttonSaveAs = new Button();
            //buttonSaveAs.CssClass = "eReport_button eReport_normal_button";
            //buttonSaveAs.Text = ERptMultiLanguage.GetLanValue("MenuSaveAsTemplate");
            //buttonSaveAs.Click += new EventHandler(buttonSaveAs_Click);
            //buttonSaveAs.Visible = false;
            //this.report.Controls.Add(buttonSaveAs);

            Button buttonExport = new Button();
            buttonExport.ID = "buttonExport";
            buttonExport.CssClass = "eReport_button eReport_normal_button";
            buttonExport.Text = ERptMultiLanguage.GetLanValue("btExport");
            buttonExport.Click += new EventHandler(buttonExport_Click);
            //this.report.Controls.Add(buttonExport);
            tableButtons.Rows[0].Cells[2].Controls.Add(buttonExport);

            Button buttonClose = new Button();
            buttonClose.ID = "buttonClose";
            buttonClose.CssClass = "eReport_button eReport_normal_button";
            buttonClose.Text = ERptMultiLanguage.GetLanValue("btClose");
            buttonClose.Click += new EventHandler(buttonClose_Click);
            //this.report.Controls.Add(buttonClose);
            tableButtons.Rows[0].Cells[3].Controls.Add(buttonClose);
            this.report.Controls.Add(tableButtons);


            UpdatePanel panel = AspNetScriptsProvider.GetUpdatePanel(this.report) as UpdatePanel;
            if (panel != null)
            {
                string buttonID = GetUpdatePanelControlID(buttonExport);
                PostBackTrigger triger = null;
                foreach (UpdatePanelTrigger trig in panel.Triggers)
                {
                    if (trig is PostBackTrigger && (trig as PostBackTrigger).ControlID == buttonID)
                    {
                        triger = trig as PostBackTrigger;
                        break;
                    }
                }
                if (triger == null)
                {
                    triger = new PostBackTrigger();
                    triger.ControlID = buttonID;
                    panel.Triggers.Add(triger);
                }
            }
            #endregion

            #region Template View
            Panel templatePanel = new Panel();
            templatePanel.ID = PANEL_DATABASE;
            templatePanel.Style.Add(HtmlTextWriterStyle.Display, "none");
            templatePanel.Controls.Add(CreateDataBaseView());

            Button popupButton = new Button();
            popupButton.ID = BUTTON_POPUP_TEMPLATE;
            popupButton.Style.Add(HtmlTextWriterStyle.Display, "none");
            this.report.Controls.Add(popupButton);
            this.report.Controls.Add(templatePanel);
            ModalPopupExtender modelPopupExtender = new ModalPopupExtender();
            modelPopupExtender.ID = MODEL_POPUP_EXTENDER_TEMPLATE_LOAD;
            modelPopupExtender.TargetControlID = BUTTON_POPUP_TEMPLATE;
            modelPopupExtender.PopupControlID = templatePanel.ID;
            modelPopupExtender.CancelControlID = BUTTON_TEMPLATE_CANCEL;
            //modelPopupExtender.OkControlID = BUTTON_TEMPLATE_OK;
            modelPopupExtender.BackgroundCssClass = WebEasilyReportCSS.ModelBackground;
            //  modelPopupExtender.Drag = WebEasilyReportConfig.ModelPanelDrag;
            //modelPopupExtender.Hide();
            this.report.Controls.Add(modelPopupExtender);
            #endregion

            #region Font View
            Panel fontPanel = new Panel();
            fontPanel.ID = PANEL_FONT;
            fontPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
            fontPanel.Controls.Add(CreateFontView());

            Button popupFontButton = new Button();
            popupFontButton.ID = BUTTON_POPUP_FONTVIEW;
            popupFontButton.Style.Add(HtmlTextWriterStyle.Display, "none");
            this.report.Controls.Add(popupFontButton);
            this.report.Controls.Add(fontPanel);
            ModalPopupExtender modelPopupExtenderFontView = new ModalPopupExtender();
            modelPopupExtenderFontView.ID = MODEL_POPUP_EXTENDER_FONTVIEW;
            modelPopupExtenderFontView.TargetControlID = BUTTON_POPUP_FONTVIEW;
            modelPopupExtenderFontView.PopupControlID = fontPanel.ID;
            modelPopupExtenderFontView.CancelControlID = BUTTON_FONT_CANCEL;
            modelPopupExtenderFontView.BackgroundCssClass = WebEasilyReportCSS.ModelBackground;
            //  modelPopupExtenderFontView.Drag = WebEasilyReportConfig.ModelPanelDrag;
            this.report.Controls.Add(modelPopupExtenderFontView);
            #endregion

            #region Save As View
            Panel saveAsPanel = new Panel();
            saveAsPanel.ID = PANEL_SAVE_AS;
            saveAsPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
            saveAsPanel.Controls.Add(CreateSaveAsView());

            Button popupSaveAsButton = new Button();
            popupSaveAsButton.ID = BUTTON_POPUP_SAVEASVIEW;
            popupSaveAsButton.Style.Add(HtmlTextWriterStyle.Display, "none");
            this.report.Controls.Add(popupSaveAsButton);
            this.report.Controls.Add(saveAsPanel);
            ModalPopupExtender modelPopupExtenderSaveAsView = new ModalPopupExtender();
            modelPopupExtenderSaveAsView.ID = MODEL_POPUP_EXTENDER_SAVEASVIEW;
            modelPopupExtenderSaveAsView.TargetControlID = BUTTON_POPUP_SAVEASVIEW;
            modelPopupExtenderSaveAsView.PopupControlID = saveAsPanel.ID;
            modelPopupExtenderSaveAsView.CancelControlID = BUTTON_SAVEAS_TEMPLATE_CANCEL;
            modelPopupExtenderSaveAsView.BackgroundCssClass = WebEasilyReportCSS.ModelBackground;
            //  modelPopupExtenderSaveAsView.Drag = WebEasilyReportConfig.ModelPanelDrag;
            this.report.Controls.Add(modelPopupExtenderSaveAsView);
            #endregion

            #region Picture View
            Panel picturePanel = new Panel();

            picturePanel.ID = PANEL_PICTURE_VIEW;
            picturePanel.Style.Add(HtmlTextWriterStyle.Display, "none");
            picturePanel.Controls.Add(CreatePicutreView());

            Button popupPictureButton = new Button();
            popupPictureButton.ID = BUTTON_POPUP_PICTUREVIEW;
            popupPictureButton.Style.Add(HtmlTextWriterStyle.Display, "none");

            this.report.Controls.Add(popupPictureButton);
            this.report.Controls.Add(picturePanel);
            ModalPopupExtender modelPopupExtenderPictureView = new ModalPopupExtender();
            modelPopupExtenderPictureView.ID = MODEL_POPUP_EXTENDER_PICTUREVIEW;
            modelPopupExtenderPictureView.TargetControlID = BUTTON_POPUP_PICTUREVIEW;
            modelPopupExtenderPictureView.PopupControlID = picturePanel.ID;
            modelPopupExtenderPictureView.CancelControlID = BUTTON_PICTURE_CANCEL;
            modelPopupExtenderPictureView.BackgroundCssClass = WebEasilyReportCSS.ModelBackground;
            // modelPopupExtenderPictureView.Drag = WebEasilyReportConfig.ModelPanelDrag;
            this.report.Controls.Add(modelPopupExtenderPictureView);
            #endregion

            #region Download View
            Panel downloadPanel = new Panel();

            downloadPanel.ID = PANEL_DOWNLOAD_VIEW;
            downloadPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
            downloadPanel.Controls.Add(CreateOutputView());

            Button popupDownloadButton = new Button();
            popupDownloadButton.ID = BUTTON_POPUP_DOWNLOADVIEW;
            popupDownloadButton.Style.Add(HtmlTextWriterStyle.Display, "none");

            this.report.Controls.Add(popupDownloadButton);
            this.report.Controls.Add(downloadPanel);
            ModalPopupExtender modelPopupExtenderDownloadView = new ModalPopupExtender();
            modelPopupExtenderDownloadView.ID = MODEL_POPUP_EXTENDER_DOWNLOADVIEW;
            modelPopupExtenderDownloadView.TargetControlID = BUTTON_POPUP_DOWNLOADVIEW;
            modelPopupExtenderDownloadView.PopupControlID = downloadPanel.ID;
            //modelPopupExtenderDownloadView.CancelControlID = BUTTON_CLOSE_DOWNLOADVIEW;
            modelPopupExtenderDownloadView.BackgroundCssClass = WebEasilyReportCSS.ModelBackground;
            //  modelPopupExtenderDownloadView.Drag = WebEasilyReportConfig.ModelPanelDrag;
            this.report.Controls.Add(modelPopupExtenderDownloadView);
            #endregion
        }

        private Table CreateTable(int rowCount, int columnCount, string cssClass, CellCssClass[] lstCellClasses)
        {
            Table table = new Table();
            table.CellSpacing = 2;
            table.CellPadding = 2;
            table.CssClass = cssClass;
            for (int i = 0; i < rowCount; i++)
            {
                TableRow row = new TableRow();
                for (int j = 0; j < columnCount; j++)
                {
                    TableCell cell = new TableCell();
                    if (lstCellClasses != null)
                    {
                        foreach (CellCssClass cellClass in lstCellClasses)
                        {
                            if (cellClass.RowIndex == i && cellClass.ColumnIndex == j)
                            {
                                cell.CssClass = cellClass.CssClass;
                                break;
                            }
                        }
                    }
                    row.Cells.Add(cell);
                }
                table.Rows.Add(row);
            }
            return table;
        }

        private View CreateItemView(string name)
        {
            View view = new View();
            Table table1 = CreateTable(1, 2, "eReportBase_table", null);//1行2列
            Table table2 = CreateTable(4, 1, "", null);//4行1列
            Table table3 = CreateTable(14, 1, "eReportPart_table", null);//14行1列
            Table table4 = CreateTable(1, 2, "eReportPart_table eReportFullWidth_table", null);//1行2列
            Table table5 = CreateTable(4, 3, "eReportPart_table eReportFullWidth_table",
                new CellCssClass[] { 
                    new CellCssClass(0, 0, "eReport_topcenter"),
                    new CellCssClass(0, 2, "eReport_topcenter")
                    });//1行3列
            Table table6 = CreateTable(3, 1, "",
                new CellCssClass[] { 
                    new CellCssClass(0, 0, "eReport_middleleft"),
                    new CellCssClass(1, 0, "eReport_middleleft"),
                    new CellCssClass(2, 0, "eReport_middleleft")
                    });//2行1列
            Table table7 = CreateTable(2, 1, "", null);//2行1列
            Table table8 = CreateTable(2, 1, "", null);//2行1列

            #region table1
            view.Controls.Add(table1);
            table1.Rows[0].Cells[0].Controls.Add(table2);
            table1.Rows[0].Cells[0].VerticalAlign = VerticalAlign.Top;
            table1.Rows[0].Cells[1].Controls.Add(table3);
            #endregion

            #region table2
            Label labelStyleSetting = new Label();
            labelStyleSetting.Text = ERptMultiLanguage.GetLanValue("gbStyleSetting");
            table2.Rows[0].Cells[0].Controls.Add(labelStyleSetting);
            table2.Rows[0].Cells[0].VerticalAlign = VerticalAlign.Top;
            table2.Rows[1].Cells[0].Controls.Add(table4);
            Label labelContentSetting = new Label();
            labelContentSetting.Text = ERptMultiLanguage.GetLanValue("gbContentSetting");
            table2.Rows[2].Cells[0].Controls.Add(labelContentSetting);
            table2.Rows[3].Cells[0].Controls.Add(table5);
            #endregion

            #region table3
            table3.ID = string.Format("{0}{1}", TABLE_ITEM, name);
            table3.Visible = false;

            Label labelItemProperties = new Label();
            labelItemProperties.Text = ERptMultiLanguage.GetLanValue("TabControl" + name + "ItemProperties");
            table3.Rows[0].Cells[0].Controls.Add(labelItemProperties);

            Label labelItemContent = new Label();
            labelItemContent.Text = ERptMultiLanguage.GetLanValue("lbContent");
            table3.Rows[1].Cells[0].Controls.Add(labelItemContent);

            DropDownList dropdownlistItemContent = new DropDownList();
            dropdownlistItemContent.ID = string.Format("{0}{1}", DROPDOWNLIST_ITEM_CONTENT, name);
            dropdownlistItemContent.AutoPostBack = true;
            dropdownlistItemContent.SelectedIndexChanged += new EventHandler(dropdownlistContent_SelectedIndexChanged);
            table3.Rows[2].Cells[0].Controls.Add(dropdownlistItemContent);

            //Sjj Added
            Button buttonAddPicutre = new Button();
            buttonAddPicutre.ID = string.Format("{0}{1}", BUTTON_ADD_PICTURE, name);
            buttonAddPicutre.Visible = false;
            buttonAddPicutre.Text = ERptMultiLanguage.GetLanValue("btSelectPicture");
            buttonAddPicutre.CssClass = WebEasilyReportCSS.Button;
            buttonAddPicutre.Click += new EventHandler(buttonAddPicture_Click);
            table3.Rows[2].Cells[0].Controls.Add(buttonAddPicutre);

            Label labelItemContentAlignment = new Label();
            labelItemContentAlignment.Text = ERptMultiLanguage.GetLanValue("lbContentAlignment");
            table3.Rows[3].Cells[0].Controls.Add(labelItemContentAlignment);

            DropDownList dropdownlistItemContentAlignment = new DropDownList();
            dropdownlistItemContentAlignment.ID = string.Format("{0}{1}", DROPDOWNLIST_ITEM_CONTENT_ALIGNMENT, name);
            foreach (object item in Enum.GetValues(typeof(System.Windows.Forms.HorizontalAlignment)))
            {
                dropdownlistItemContentAlignment.Items.Add(item.ToString());
            }
            table3.Rows[4].Cells[0].Controls.Add(dropdownlistItemContentAlignment);
            Label labelItemPosition = new Label();
            labelItemPosition.Text = ERptMultiLanguage.GetLanValue("lbPosition");
            table3.Rows[5].Cells[0].Controls.Add(labelItemPosition);
            DropDownList dropdownlistItemPosition = new DropDownList();
            dropdownlistItemPosition.ID = string.Format("{0}{1}", DROPDOWNLIST_ITEM_POSITION, name);
            foreach (object item in Enum.GetValues(typeof(ReportItem.PositionAlign)))
            {
                dropdownlistItemPosition.Items.Add(item.ToString());
            }
            table3.Rows[6].Cells[0].Controls.Add(dropdownlistItemPosition);
            Label labelItemCells = new Label();
            labelItemCells.Text = ERptMultiLanguage.GetLanValue("lbCells");
            table3.Rows[7].Cells[0].Controls.Add(labelItemCells);
            TextBox textboxItemCells = new TextBox();
            textboxItemCells.ID = string.Format("{0}{1}", TEXTBOX_ITEM_CELLS, name);
            table3.Rows[8].Cells[0].Controls.Add(textboxItemCells);
            CheckBox checkboxItemNewLine = new CheckBox();
            checkboxItemNewLine.ID = string.Format("{0}{1}", CHECKBOX_ITEM_NEWLINE, name);
            checkboxItemNewLine.Text = ERptMultiLanguage.GetLanValue("cbNewLine"); ;
            table3.Rows[9].Cells[0].Controls.Add(checkboxItemNewLine);
            Label labelItemFormat = new Label();
            labelItemFormat.Text = ERptMultiLanguage.GetLanValue("lbFormat");
            table3.Rows[10].Cells[0].Controls.Add(labelItemFormat);
            TextBox textboxItemFormat = new TextBox();
            textboxItemFormat.ID = string.Format("{0}{1}", TEXTBOX_ITEM_FORMAT, name);
            table3.Rows[11].Cells[0].Controls.Add(textboxItemFormat);
            Button buttonItemDetailFont = new Button();
            buttonItemDetailFont.CssClass = WebEasilyReportCSS.Button;
            buttonItemDetailFont.ID = string.Format("{0}{1}", BUTTON_ITEM_DETAIL_FONT, name);
            buttonItemDetailFont.Text = ERptMultiLanguage.GetLanValue("btFont");
            buttonItemDetailFont.Click += new EventHandler(buttonFont_Click);
            table3.Rows[12].Cells[0].Controls.Add(buttonItemDetailFont);

            Label labelFontSample = new Label();
            labelFontSample.ID = string.Format("{0}{1}", LABEL_ITEM_DETAIL_FONT_SAMPLE, name);
            labelFontSample.Text = EasilyReportConfig.FontSample;
            table3.Rows[12].Cells[0].Controls.Add(labelFontSample);
            #endregion

            #region table4
            Button buttonItemFont = new Button();
            buttonItemFont.CssClass = WebEasilyReportCSS.Button;
            buttonItemFont.ID = string.Format("{0}{1}", BUTTON_ITEM_FONT, name);
            buttonItemFont.Text = ERptMultiLanguage.GetLanValue("btFont");
            buttonItemFont.Click += new EventHandler(buttonFont_Click);
            table4.Rows[0].Cells[0].Controls.Add(buttonItemFont);

            Label labelSample = new Label();
            labelSample.ID = string.Format("{0}{1}", LABEL_ITEM_FONT_SAMPLE, name);
            labelSample.Text = EasilyReportConfig.FontSample;
            table4.Rows[0].Cells[0].Controls.Add(labelSample);
            #endregion

            #region table5
            table5.Rows[0].Cells[0].Controls.Add(table6);
            table5.Rows[0].Cells[1].Controls.Add(table7);
            table5.Rows[0].Cells[2].Controls.Add(table8);
            #endregion

            #region table6
            Label labelHeaderItemType = new Label();
            labelHeaderItemType.Text = ERptMultiLanguage.GetLanValue("lb" + name + "ItemType");
            table6.Rows[0].Cells[0].Controls.Add(labelHeaderItemType);
            DropDownList dropdownlistItemType = new DropDownList();
            dropdownlistItemType.ID = string.Format("{0}{1}", DROPDOWNLIST_ITEM_TYPE, name);
            ListItem[] items = new ListItem[]
            {
                new ListItem(ERptMultiLanguage.GetLanValue("ConstantItem"), "ReportConstantItem")
                ,new ListItem(ERptMultiLanguage.GetLanValue("DataSourceItem"), "ReportDataSourceItem")
                ,new ListItem(ERptMultiLanguage.GetLanValue("ImageItem"), "ReportImageItem")
            };
            dropdownlistItemType.Items.AddRange(items);

            table6.Rows[1].Cells[0].Controls.Add(dropdownlistItemType);
            Button buttonItemAdd = new Button();
            buttonItemAdd.CssClass = WebEasilyReportCSS.Button;
            buttonItemAdd.ID = string.Format("{0}{1}", BUTTON_ITEM_ADD, name);
            buttonItemAdd.Text = ERptMultiLanguage.GetLanValue("btAdd");
            buttonItemAdd.Click += new EventHandler(buttoItemnAdd_Click);
            table6.Rows[2].Cells[0].Controls.Add(buttonItemAdd);
            Button buttonItemRemove = new Button();
            buttonItemRemove.CssClass = WebEasilyReportCSS.Button;
            buttonItemRemove.ID = string.Format("{0}{1}", BUTTON_ITEM_REMOVE, name);
            buttonItemRemove.Click += new EventHandler(buttonItemRemove_Click);
            buttonItemRemove.Text = ERptMultiLanguage.GetLanValue("btRemove");
            table6.Rows[2].Cells[0].Controls.Add(buttonItemRemove);
            #endregion

            #region table7
            Label labelHeaderItem = new Label();
            table7.Rows[0].Cells[0].Controls.Add(labelHeaderItem);
            ListBox listboxItem = new ListBox();
            listboxItem.ID = string.Format("{0}{1}", LISTBOX_ITEM, name);
            listboxItem.AutoPostBack = true;
            listboxItem.CssClass = "eReport_listbox";
            listboxItem.SelectedIndexChanged += new EventHandler(listboxItem_SelectedIndexChanged);
            table7.Rows[1].Cells[0].Controls.Add(listboxItem);
            #endregion

            #region table8
            Button buttonItemUp = new Button();
            buttonItemUp.CssClass = "eReport_button eReport_navigator_button";
            buttonItemUp.ID = string.Format("{0}{1}", BUTTON_ITEM_UP, name);
            buttonItemUp.Click += new EventHandler(buttonItemUp_Click);
            buttonItemUp.Text = @"/\";
            table8.Rows[0].Cells[0].Controls.Add(buttonItemUp);
            Button buttonItemDown = new Button();
            buttonItemDown.CssClass = "eReport_button eReport_navigator_button";
            buttonItemDown.ID = string.Format("{0}{1}", BUTTON_ITEM_DOWN, name);
            buttonItemDown.Click += new EventHandler(buttonItemDown_Click);
            buttonItemDown.Text = @"\/";
            table8.Rows[1].Cells[0].Controls.Add(buttonItemDown);
            #endregion
            return view;
        }

        void buttonItemUp_Click(object sender, EventArgs e)
        {
            string name = (sender as Control).ID.Replace(BUTTON_ITEM_UP, string.Empty);
            Control container = this.report.MultiView;
            ListBox listboxItem = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_ITEM, name));
            ListItemUp(listboxItem);
        }

        void buttonItemDown_Click(object sender, EventArgs e)
        {
            string name = (sender as Control).ID.Replace(BUTTON_ITEM_DOWN, string.Empty);
            Control container = this.report.MultiView;
            ListBox listboxItem = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_ITEM, name));
            ListItemDown(listboxItem);
        }

        private View CreateFieldView()
        {
            View view = new View();
            Table table1 = CreateTable(1, 2, "eReportBase_table",
                new CellCssClass[] {
                    new CellCssClass(0, 1, "eReport_topleft")
                });//1行2列
            Table table2 = CreateTable(6, 1, "", null);//4行1列
            Table table3 = CreateTable(26, 1, "eReportPart_table eReportFullWidth_table", null);//21行1列
            Table table4 = CreateTable(9, 1, "eReportPart_table eReportFullWidth_table", null);//9行1列
            Table table5 = CreateTable(1, 4, "eReportPart_table eReportFullWidth_table", null);//1行3列

            #region table1
            view.Controls.Add(table1);
            table1.Rows[0].Cells[0].Controls.Add(table2);
            table1.Rows[0].Cells[0].VerticalAlign = VerticalAlign.Top;
            MultiView multiviewColumnsContent = new MultiView();
            multiviewColumnsContent.ID = MULTIVIEW_FIELD_CONTENT;
            View view1 = new View();
            view1.Controls.Add(table3);
            multiviewColumnsContent.Views.Add(view1);
            View view2 = new View();
            view2.Controls.Add(table4);
            multiviewColumnsContent.Views.Add(view2);
            table1.Rows[0].Cells[1].Controls.Add(multiviewColumnsContent);

            #endregion

            #region table2


            Label labelStyleSetting = new Label();
            labelStyleSetting.Text = ERptMultiLanguage.GetLanValue("gbStyleSetting");
            table2.Rows[0].Cells[0].Controls.Add(labelStyleSetting);
            table2.Rows[0].Cells[0].VerticalAlign = VerticalAlign.Top;
            table2.Rows[1].Cells[0].Controls.Add(table5);

            //WebMultiViewCaptions webMultiViewCaptions = new WebMultiViewCaptions();
            //webMultiViewCaptions.CssClass = "eReport_multiViewCaption";
            //webMultiViewCaptions.MultiViewID = MULTIVIEW_FIELD;
            //webMultiViewCaptions.TableStyle = WebMultiViewCaptionStyle.Style3;
            //string[] caption = new string[] { "Columns" };
            //foreach (string str in caption)
            //{
            //    WebMultiViewCaption viewcaption = new WebMultiViewCaption();
            //    viewcaption.Caption = str;
            //    webMultiViewCaptions.Captions.Add(viewcaption);
            //}
            //webMultiViewCaptions.TabChanged += new TabChangedEventHandler(webMultiViewCaptions_TabChanged);
            //table2.Rows[2].Cells[0].Controls.Add(webMultiViewCaptions);

            DropDownList dropdownlistField = new DropDownList();
            dropdownlistField.ID = DROPDOWNLIST_FIELD;
            dropdownlistField.SelectedIndexChanged += new EventHandler(dropdownlistField_SelectedIndexChanged);
            dropdownlistField.Width = new Unit(125);
            table2.Rows[2].Cells[0].Controls.Add(dropdownlistField);

            MultiView multiviewColumns = new MultiView();
            multiviewColumns.ID = MULTIVIEW_FIELD;
            multiviewColumns.Views.Add(CreateColumnView(string.Empty));
            multiviewColumns.ActiveViewIndex = 0;
            // multiviewColumns.Views.Add(CreateColumnView(GROUP));
            table2.Rows[2].Cells[0].Controls.Add(multiviewColumns);

            //add group total
            Label labelFieldCaptionStyle = new Label();
            labelFieldCaptionStyle.Text = ERptMultiLanguage.GetLanValue("lbCaptionStyle");
            table2.Rows[3].Cells[0].Controls.Add(labelFieldCaptionStyle);

            DropDownList dropdownlistFieldCaptionStyle = new DropDownList();
            dropdownlistFieldCaptionStyle.ID = DROPDOWNLIST_CAPTION_STYLE;
            dropdownlistFieldCaptionStyle.Width = new Unit(125);
            foreach (object item in Enum.GetValues(typeof(DataSourceItem.CaptionStyleType)))
            {
                dropdownlistFieldCaptionStyle.Items.Add(item.ToString());
            }
            table2.Rows[3].Cells[0].Controls.Add(dropdownlistFieldCaptionStyle);

            CheckBox checkboxFieldGroupTotal = new CheckBox();
            checkboxFieldGroupTotal.ID = CHECKBOX_FIELD_GROUP_TOTAL;
            checkboxFieldGroupTotal.Text = ERptMultiLanguage.GetLanValue("cbGroupTotal");
            table2.Rows[4].Cells[0].Controls.Add(checkboxFieldGroupTotal);

            Label labelFieldGroupGap = new Label();
            labelFieldGroupGap.Text = ERptMultiLanguage.GetLanValue("lbGroupGap");
            table2.Rows[5].Cells[0].Controls.Add(labelFieldGroupGap);
            DropDownList dropdownlistFieldGroupGap = new DropDownList();
            dropdownlistFieldGroupGap.Width = new Unit(125);
            dropdownlistFieldGroupGap.ID = DROPDOWNLIST_FIELD_GROUP_GAP;
            foreach (object item in Enum.GetValues(typeof(DataSourceItem.GroupGapType)))
            {
                dropdownlistFieldGroupGap.Items.Add(item.ToString());
            }
            table2.Rows[5].Cells[0].Controls.Add(dropdownlistFieldGroupGap);

            #endregion

            #region table3
            Label labelFieldCaption = new Label();
            labelFieldCaption.Text = ERptMultiLanguage.GetLanValue("lbColumnCaption");
            table3.Rows[0].Cells[0].Controls.Add(labelFieldCaption);
            TextBox textboxFieldCaption = new TextBox();
            textboxFieldCaption.Width = new Unit(120);
            textboxFieldCaption.TextMode = TextBoxMode.MultiLine;
            textboxFieldCaption.ID = "textboxFieldCaption";
            table3.Rows[1].Cells[0].Controls.Add(textboxFieldCaption);
            Label labelFieldCaptionAlignment = new Label();
            labelFieldCaptionAlignment.Text = ERptMultiLanguage.GetLanValue("lbCaptionAlignment");
            table3.Rows[2].Cells[0].Controls.Add(labelFieldCaptionAlignment);
            DropDownList dropdownlistFieldCaptionAligment = new DropDownList();
            dropdownlistFieldCaptionAligment.Width = new Unit(125);
            dropdownlistFieldCaptionAligment.ID = DROPDOWNLIST_FIELD_CAPTION_ALIGNMENT;
            foreach (object item in Enum.GetValues(typeof(System.Windows.Forms.HorizontalAlignment)))
            {
                dropdownlistFieldCaptionAligment.Items.Add(item.ToString());
            }
            table3.Rows[3].Cells[0].Controls.Add(dropdownlistFieldCaptionAligment);
            Label labelFieldColumnAlignment = new Label();
            labelFieldColumnAlignment.Text = ERptMultiLanguage.GetLanValue("lbColumnAlignment");
            table3.Rows[4].Cells[0].Controls.Add(labelFieldColumnAlignment);
            DropDownList dropdownlistFieldColumnAligment = new DropDownList();
            dropdownlistFieldColumnAligment.Width = new Unit(125);
            dropdownlistFieldColumnAligment.ID = DROPDOWNLIST_FIELD_COLUMN_ALIGNMENT;
            foreach (object item in Enum.GetValues(typeof(System.Windows.Forms.HorizontalAlignment)))
            {
                dropdownlistFieldColumnAligment.Items.Add(item.ToString());
            }
            table3.Rows[5].Cells[0].Controls.Add(dropdownlistFieldColumnAligment);
            Label labelFieldWidth = new Label();
            labelFieldWidth.Text = ERptMultiLanguage.GetLanValue("lbWidth");
            table3.Rows[6].Cells[0].Controls.Add(labelFieldWidth);
            TextBox textboxFieldWidth = new TextBox();
            textboxFieldWidth.Width = new Unit(120);
            textboxFieldWidth.ID = TEXTBOX_FIELD_WIDTH;
            table3.Rows[7].Cells[0].Controls.Add(textboxFieldWidth);

            //Format
            Label labelFieldFormat = new Label();
            labelFieldFormat.Text = ERptMultiLanguage.GetLanValue("lbFormat");
            table3.Rows[8].Cells[0].Controls.Add(labelFieldFormat);
            TextBox textboxFieldFormat = new TextBox();
            textboxFieldFormat.Width = new Unit(120);
            textboxFieldFormat.ID = TEXTBOX_FIELD_FORMAT;
            table3.Rows[9].Cells[0].Controls.Add(textboxFieldFormat);

            CheckBox checkFieldNewLine = new CheckBox();
            checkFieldNewLine.ID = CHECKBOX_FIELD_NEWLINE;
            checkFieldNewLine.Text = ERptMultiLanguage.GetLanValue("cbNewLine");
            table3.Rows[10].Cells[0].Controls.Add(checkFieldNewLine);
            Label labelFieldNewlinePosition = new Label();
            labelFieldNewlinePosition.Text = ERptMultiLanguage.GetLanValue("lbNewLinePosition");
            table3.Rows[11].Cells[0].Controls.Add(labelFieldNewlinePosition);
            TextBox textboxFieldNewlinePosition = new TextBox();
            textboxFieldNewlinePosition.Width = new Unit(120);
            textboxFieldNewlinePosition.ID = TEXTBOX_FIELD_NEWLINE_POSITION;
            table3.Rows[12].Cells[0].Controls.Add(textboxFieldNewlinePosition);


            Label labelFieldCells = new Label();
            labelFieldCells.Text = ERptMultiLanguage.GetLanValue("lbCells");
            table3.Rows[13].Cells[0].Controls.Add(labelFieldCells);
            TextBox textBoxFieldCells = new TextBox();
            textBoxFieldCells.Width = new Unit(120);
            textBoxFieldCells.ID = TEXTBOX_FIELD_CELLS;
            table3.Rows[14].Cells[0].Controls.Add(textBoxFieldCells);

            Label labelFieldOrderby = new Label();
            labelFieldOrderby.Text = ERptMultiLanguage.GetLanValue("lbOrderType");
            //set text
            table3.Rows[15].Cells[0].Controls.Add(labelFieldOrderby);
            DropDownList dropdownlistFieldOrderby = new DropDownList();
            dropdownlistFieldOrderby.Width = new Unit(125);
            dropdownlistFieldOrderby.ID = DROPDOWNLIST_FIELD_ORDERBY;
            foreach (object item in Enum.GetValues(typeof(FieldItem.OrderType)))
            {
                dropdownlistFieldOrderby.Items.Add(item.ToString());
            }
            table3.Rows[16].Cells[0].Controls.Add(dropdownlistFieldOrderby);

            CheckBox checkboxFieldSuppressIfDuplicated = new CheckBox();
            checkboxFieldSuppressIfDuplicated.ID = CHECKBOX_FIELD_SUPPRESSIFDUPLICATED;
            checkboxFieldSuppressIfDuplicated.Text = ERptMultiLanguage.GetLanValue("cbSuppressIfDuplicated");
            table3.Rows[17].Cells[0].Controls.Add(checkboxFieldSuppressIfDuplicated);


            Label labelFieldGroupType = new Label();
            labelFieldGroupType.Text = ERptMultiLanguage.GetLanValue("lbGroupType");
            table3.Rows[18].Cells[0].Controls.Add(labelFieldGroupType);
            DropDownList dropdownlistFieldGroupType = new DropDownList();
            dropdownlistFieldGroupType.Width = new Unit(125);
            dropdownlistFieldGroupType.ID = DROPDOWNLIST_FIELD_GROUP_TYPE;
            foreach (object item in Enum.GetValues(typeof(FieldItem.GroupType)))
            {
                dropdownlistFieldGroupType.Items.Add(item.ToString());
            }
            table3.Rows[19].Cells[0].Controls.Add(dropdownlistFieldGroupType);

            Label labelFieldGroupTotalCaption = new Label();
            labelFieldGroupTotalCaption.Text = ERptMultiLanguage.GetLanValue("lbGroupTotalCaption");
            table3.Rows[20].Cells[0].Controls.Add(labelFieldGroupTotalCaption);
            TextBox textboxFieldGroupTotalCaption = new TextBox();
            textboxFieldGroupTotalCaption.Width = new Unit(120);
            textboxFieldGroupTotalCaption.ID = TEXTBOX_FIELD_GROUP_TOTAL_CAPTION;
            table3.Rows[21].Cells[0].Controls.Add(textboxFieldGroupTotalCaption);

            Label labelFieldSumType = new Label();
            labelFieldSumType.Text = ERptMultiLanguage.GetLanValue("lbSumType");
            table3.Rows[22].Cells[0].Controls.Add(labelFieldSumType);
            DropDownList dropdownlistFieldSumType = new DropDownList();
            dropdownlistFieldSumType.Width = new Unit(125);
            dropdownlistFieldSumType.ID = DROPDOWNLIST_FIELD_SUM_TYPE;
            foreach (object item in Enum.GetValues(typeof(FieldItem.SumType)))
            {
                dropdownlistFieldSumType.Items.Add(item.ToString());
            }
            table3.Rows[23].Cells[0].Controls.Add(dropdownlistFieldSumType);
            Label labelFieldTotalCaption = new Label();
            labelFieldTotalCaption.Text = ERptMultiLanguage.GetLanValue("lbTotalCaption");
            table3.Rows[24].Cells[0].Controls.Add(labelFieldTotalCaption);
            TextBox textboxFieldTotalCaption = new TextBox();
            textboxFieldTotalCaption.Width = new Unit(120);
            textboxFieldTotalCaption.ID = TEXTBOX_FIELD_TOTAL_CAPTION;
            table3.Rows[25].Cells[0].Controls.Add(textboxFieldTotalCaption);

            #endregion

            #region table4


            #endregion

            #region table5
            Button buttonItemFont = new Button();
            buttonItemFont.CssClass = WebEasilyReportCSS.Button;
            buttonItemFont.ID = BUTTON_FIELD_FONT;
            buttonItemFont.Text = ERptMultiLanguage.GetLanValue("btFont");
            buttonItemFont.Click += new EventHandler(buttonFont_Click);
            table5.Rows[0].Cells[0].Controls.Add(buttonItemFont);

            Label labelSample = new Label();
            labelSample.ID = LABEL_FIELD_FONT_SAMPLE;
            labelSample.Text = EasilyReportConfig.FontSample;
            table5.Rows[0].Cells[0].Controls.Add(labelSample);

            CheckBox cbxColumnGrid = new CheckBox();
            cbxColumnGrid.ID = CHECKBOX_COLUMNGRID;
            cbxColumnGrid.Text = ERptMultiLanguage.GetLanValue("cbColumnGridLine");
            table5.Rows[0].Cells[1].Controls.Add(cbxColumnGrid);

            CheckBox cbxInnerColumnGrid = new CheckBox();
            cbxInnerColumnGrid.ID = CHECKBOX_INNER_COLUMNGRID;
            cbxInnerColumnGrid.Text = ERptMultiLanguage.GetLanValue("cbColumnInsideLine");
            table5.Rows[0].Cells[2].Controls.Add(cbxInnerColumnGrid);

            CheckBox cbxRowGrid = new CheckBox();
            cbxRowGrid.ID = CHECKBOX_ROWGRID;
            cbxRowGrid.Text = ERptMultiLanguage.GetLanValue("cbRowGridLine");
            table5.Rows[0].Cells[3].Controls.Add(cbxRowGrid);

            #endregion

            return view;
        }

        private View CreateColumnView(string name)
        {
            View view = new View();
            Table table1 = CreateTable(1, 4, "eReportPart_table eReportFullWidth_table",
                new CellCssClass[] {
                    new CellCssClass(0,3,"eReport_topleft")
                });
            Table table2 = CreateTable(2, 1, "", null);
            Table table3 = CreateTable(4, 1, "", null);
            Table table4 = CreateTable(2, 1, "", null);
            Table table5 = CreateTable(2, 1, "", null);

            #region table1
            view.Controls.Add(table1);
            table1.Rows[0].Cells[0].Controls.Add(table2);
            table1.Rows[0].Cells[1].Controls.Add(table3);
            table1.Rows[0].Cells[2].Controls.Add(table4);
            table1.Rows[0].Cells[3].Controls.Add(table5);
            #endregion

            #region table2
            Label labelFieldAll = new Label();
            labelFieldAll.Text = ERptMultiLanguage.GetLanValue("gb" + name + "Columns");
            table2.Rows[0].Cells[0].Controls.Add(labelFieldAll);
            ListBox listboxFieldAll = new ListBox();
            listboxFieldAll.CssClass = "eReport_listbox";
            listboxFieldAll.ID = string.Format("{0}{1}", LISTBOX_FIELD_ALL, name);
            table2.Rows[1].Cells[0].Controls.Add(listboxFieldAll);
            #endregion

            #region table3
            Button buttonFieldAdd = new Button();
            buttonFieldAdd.CssClass = "eReport_button eReport_navigator_button";
            buttonFieldAdd.ID = string.Format("{0}{1}", BUTTON_FIELD_ADD, name);
            buttonFieldAdd.Text = ">";
            buttonFieldAdd.Click += new EventHandler(buttonFieldAdd_Click);
            table3.Rows[0].Cells[0].Controls.Add(buttonFieldAdd);
            Button buttonFieldRemove = new Button();
            buttonFieldRemove.CssClass = "eReport_button eReport_navigator_button";
            buttonFieldRemove.ID = string.Format("{0}{1}", BUTTON_FIELD_REMOVE, name);
            buttonFieldRemove.Text = "<";
            buttonFieldRemove.Click += new EventHandler(buttonFieldRemove_Click);
            table3.Rows[1].Cells[0].Controls.Add(buttonFieldRemove);
            Button buttonFieldAddAll = new Button();
            buttonFieldAddAll.CssClass = "eReport_button eReport_navigator_button";
            buttonFieldAddAll.ID = string.Format("{0}{1}", BUTTON_FIELD_ADD_ALL, name);
            buttonFieldAddAll.Text = ">>";
            buttonFieldAddAll.Click += new EventHandler(buttonFieldAddAll_Click);
            table3.Rows[2].Cells[0].Controls.Add(buttonFieldAddAll);
            Button buttonFieldRemoveAll = new Button();
            buttonFieldRemoveAll.CssClass = "eReport_button eReport_navigator_button";
            buttonFieldRemoveAll.ID = string.Format("{0}{1}", BUTTON_FIELD_REMOVE_ALL, name);
            buttonFieldRemoveAll.Text = "<<";
            buttonFieldRemoveAll.Click += new EventHandler(buttonFieldRemoveAll_Click);
            table3.Rows[3].Cells[0].Controls.Add(buttonFieldRemoveAll);
            #endregion

            #region table4
            Label labelFieldSelected = new Label();
            labelFieldSelected.Text = ERptMultiLanguage.GetLanValue("gbSelected" + name + "Columns");
            table4.Rows[0].Cells[0].Controls.Add(labelFieldSelected);
            ListBox listboxFieldSelected = new ListBox();
            listboxFieldSelected.CssClass = "eReport_listbox";
            listboxFieldSelected.ID = string.Format("{0}{1}", LISTBOX_FIELD_SELECTED, name);
            listboxFieldSelected.AutoPostBack = true;
            listboxFieldSelected.SelectedIndexChanged += new EventHandler(listboxFieldSelected_SelectedIndexChanged);
            table4.Rows[1].Cells[0].Controls.Add(listboxFieldSelected);
            #endregion

            #region table5
            Button buttonFieldUp = new Button();
            buttonFieldUp.CssClass = "eReport_button eReport_navigator_button";
            buttonFieldUp.ID = string.Format("{0}{1}", BUTTON_FIELD_UP, name);
            buttonFieldUp.Click += new EventHandler(buttonFieldUp_Click);
            buttonFieldUp.Text = @"/\";
            table5.Rows[0].Cells[0].Controls.Add(buttonFieldUp);
            Button buttonFieldDown = new Button();
            buttonFieldDown.CssClass = "eReport_button eReport_navigator_button";
            buttonFieldDown.ID = string.Format("{0}{1}", BUTTON_FIELD_DOWN, name);
            buttonFieldDown.Text = @"\/";
            buttonFieldDown.Click += new EventHandler(buttonFieldDown_Click);
            table5.Rows[1].Cells[0].Controls.Add(buttonFieldDown);
            #endregion

            return view;
        }

        void buttonFieldUp_Click(object sender, EventArgs e)
        {
            string name = (sender as Control).ID.Replace(BUTTON_FIELD_UP, string.Empty);
            Control container = this.report.MultiView;
            ListBox listboxItem = null;
            listboxItem = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_SELECTED, name));
            ListItemUp(listboxItem);
        }

        void buttonFieldDown_Click(object sender, EventArgs e)
        {
            string name = (sender as Control).ID.Replace(BUTTON_FIELD_DOWN, string.Empty);
            Control container = this.report.MultiView;
            ListBox listboxItem = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_SELECTED, name));
            ListItemDown(listboxItem);
        }

        private Control CreateDataBaseView()
        {
            Table baseTable = CreateTable(2, 1, WebEasilyReportCSS.BaseTable, null);
            Table table = CreateTable(3, 1, WebEasilyReportCSS.PartTable, null);
            baseTable.Rows[1].Cells[0].Controls.Add(table);

            Label lbSelectTemplate = new Label();
            lbSelectTemplate.Text = ERptMultiLanguage.GetLanValue("lbSelectTemplate");
            baseTable.Rows[0].Cells[0].Controls.Add(lbSelectTemplate);

            //ListBox
            ListBox lbxTemplate = new ListBox();
            lbxTemplate.ID = LISTBOX_TEMPLATES;
            lbxTemplate.CssClass = "eReport_label";
            lbxTemplate.Width = new Unit(200, UnitType.Pixel);
            lbxTemplate.Height = new Unit(200, UnitType.Pixel);
            table.Rows[1].Cells[0].Controls.Add(lbxTemplate);

            //btOK
            Button buttonTemplateOK = new Button();
            buttonTemplateOK.Text = ERptMultiLanguage.GetLanValue("btOK");
            buttonTemplateOK.ID = BUTTON_TEMPLATE_OK;
            buttonTemplateOK.CssClass = WebEasilyReportCSS.Button;
            buttonTemplateOK.Click += new EventHandler(buttonTemplateOK_Click);
            table.Rows[2].Cells[0].Controls.Add(buttonTemplateOK);

            //btDelete
            Button buttonTemplateDelete = new Button();
            buttonTemplateDelete.Text = ERptMultiLanguage.GetLanValue("btDelete");
            buttonTemplateDelete.ID = BUTTON_TEMPLATE_DELETE;
            buttonTemplateDelete.OnClientClick = "if(!confirm('Are you sure to delete it?')) return false";
            buttonTemplateDelete.CssClass = WebEasilyReportCSS.Button;
            buttonTemplateDelete.Click += new EventHandler(buttonTemplateDelete_Click);
            table.Rows[2].Cells[0].Controls.Add(buttonTemplateDelete);

            //btCancel
            Button buttonTemplateCancel = new Button();
            buttonTemplateCancel.Text = ERptMultiLanguage.GetLanValue("btClose");
            buttonTemplateCancel.ID = BUTTON_TEMPLATE_CANCEL;
            buttonTemplateCancel.CssClass = WebEasilyReportCSS.Button;
            buttonTemplateCancel.Click += new EventHandler(buttonTemplateCancel_Click);
            table.Rows[2].Cells[0].Controls.Add(buttonTemplateCancel);

            return baseTable;
            //return view;
        }

        private Control CreateFontView()
        {
            //View view = new View();
            Table table = CreateTable(7, 1, "eReportBase_table", null);
            //view.Controls.Add(table);
            InstalledFontCollection collection = new InstalledFontCollection();
            //fontname
            DropDownList dropdownlistFont = new DropDownList();
            dropdownlistFont.Width = new Unit(125);
            dropdownlistFont.ID = DROPDOWNLIST_FONT;
            //set item;
            foreach (FontFamily font in collection.Families)
            {
                dropdownlistFont.Items.Add(font.Name);
            }
            table.Rows[0].Cells[0].Controls.Add(dropdownlistFont);

            //size
            TextBox textboxSize = new TextBox();
            textboxSize.Width = new Unit(120);
            textboxSize.ID = TEXTBOX_FONT_SIZE;
            table.Rows[1].Cells[0].Controls.Add(textboxSize);

            //bold
            CheckBox checkboxBold = new CheckBox();
            checkboxBold.ID = CHECKBOX_FONT_BOLD;
            checkboxBold.Text = "Bold";
            table.Rows[2].Cells[0].Controls.Add(checkboxBold);

            //Italic
            CheckBox checkboxItalic = new CheckBox();
            checkboxItalic.ID = CHECKBOX_FONT_ITALIC;
            checkboxItalic.Text = "Italic";
            table.Rows[3].Cells[0].Controls.Add(checkboxItalic);

            //underline
            CheckBox checkboxUnderline = new CheckBox();
            checkboxUnderline.ID = CHECKBOX_FONT_UNDERLINE;
            checkboxUnderline.Text = "Underline";
            table.Rows[4].Cells[0].Controls.Add(checkboxUnderline);

            //StrikeOut
            CheckBox checkboxStrikeout = new CheckBox();
            checkboxStrikeout.ID = CHECKBOX_FONT_STRIKEOUT;
            checkboxStrikeout.Text = "StrikeOut";
            table.Rows[5].Cells[0].Controls.Add(checkboxStrikeout);

            Button buttonFontOK = new Button();
            buttonFontOK.CssClass = WebEasilyReportCSS.Button;
            buttonFontOK.Text = ERptMultiLanguage.GetLanValue("btOK");
            buttonFontOK.ID = BUTTON_FONT_OK;
            buttonFontOK.Click += new EventHandler(buttonFontOK_Click);
            table.Rows[6].Cells[0].Controls.Add(buttonFontOK);

            Button buttonFontCancel = new Button();
            buttonFontCancel.CssClass = WebEasilyReportCSS.Button;
            buttonFontCancel.Text = ERptMultiLanguage.GetLanValue("btClose");
            buttonFontCancel.ID = BUTTON_FONT_CANCEL;
            buttonFontCancel.Click += new EventHandler(buttonFontCancel_Click);
            table.Rows[6].Cells[0].Controls.Add(buttonFontCancel);

            return table;
        }

        private Control CreateSaveAsView()
        {
            Table baseTable = CreateTable(1, 1, "eReportBase_table", null);
            Table table = CreateTable(3, 1, "eReportPart_table", null);
            baseTable.Rows[0].Cells[0].Controls.Add(table);

            Label lbFileName = new Label();
            lbFileName.Text = ERptMultiLanguage.GetLanValue("lbTemplateName");
            table.Rows[0].Cells[0].Controls.Add(lbFileName);

            //TextBox
            TextBox tbFileName = new TextBox();
            tbFileName.ID = TEXTBOX_TEMPLATE_SAVE_AS_FILENAME;
            tbFileName.Width = new Unit(200, UnitType.Pixel);
            table.Rows[1].Cells[0].Controls.Add(tbFileName);

            //btOK
            Button buttonTemplateOK = new Button();
            buttonTemplateOK.Text = ERptMultiLanguage.GetLanValue("btOK");
            buttonTemplateOK.CssClass = WebEasilyReportCSS.Button;
            buttonTemplateOK.ID = BUTTON_SAVEAS_TEMPLATE_OK;
            buttonTemplateOK.Click += new EventHandler(buttonTemplateSaveAsOK_Click);
            table.Rows[2].Cells[0].Controls.Add(buttonTemplateOK);

            //btCancel
            Button buttonTemplateCancel = new Button();
            buttonTemplateCancel.ID = BUTTON_SAVEAS_TEMPLATE_CANCEL;
            buttonTemplateCancel.Text = ERptMultiLanguage.GetLanValue("btClose");
            buttonTemplateCancel.CssClass = WebEasilyReportCSS.Button;
            buttonTemplateCancel.Click += new EventHandler(buttonTemplateCancel_Click);
            table.Rows[2].Cells[0].Controls.Add(buttonTemplateCancel);
            return baseTable;
        }

        private Control CreatePicutreView()
        {
            Table baseTable = CreateTable(2, 2, WebEasilyReportCSS.BaseTable, null);

            Table tableLeft = CreateTable(5, 1, WebEasilyReportCSS.PartTable, null);
            baseTable.Rows[0].Cells[0].Controls.Add(tableLeft);

            Table tableRight = CreateTable(1, 1, WebEasilyReportCSS.PartTable, null);
            baseTable.Rows[0].Cells[1].Controls.Add(tableRight);

            Label lbPictures = new Label();
            lbPictures.Text = ERptMultiLanguage.GetLanValue("lbPictures");
            lbPictures.CssClass = WebEasilyReportCSS.Label;
            tableLeft.Rows[0].Cells[0].Controls.Add(lbPictures);

            //ListBox
            ListBox lbxPicutres = new ListBox();
            lbxPicutres.ID = LISTBOX_PICTURES;
            lbxPicutres.AutoPostBack = true;
            lbxPicutres.CssClass = WebEasilyReportCSS.ListBoxPicture;
            lbxPicutres.SelectedIndexChanged += new EventHandler(listboxPictures_SelectedIndexChanged);
            tableLeft.Rows[1].Cells[0].Controls.Add(lbxPicutres);

            //btAdd
            Button btAdd = new Button();
            btAdd.Text = ERptMultiLanguage.GetLanValue("btAdd");
            btAdd.Click += new EventHandler(buttonPictureAdd_Click);
            btAdd.CssClass = WebEasilyReportCSS.Button;
            tableLeft.Rows[2].Cells[0].Controls.Add(btAdd);

            //btRemove
            Button btRemove = new Button();
            btRemove.Text = ERptMultiLanguage.GetLanValue("btRemove");
            btRemove.Click += new EventHandler(buttonPictureRemove_Click);
            btRemove.CssClass = WebEasilyReportCSS.Button;
            tableLeft.Rows[2].Cells[0].Controls.Add(btRemove);

            //btChange
            Button btChange = new Button();
            btChange.Text = ERptMultiLanguage.GetLanValue("btChange");
            btChange.Click += new EventHandler(buttonPictureChange_Click);
            btChange.CssClass = WebEasilyReportCSS.Button;
            tableLeft.Rows[3].Cells[0].Controls.Add(btChange);

            //btSelect
            Button btSelect = new Button();
            btSelect.Text = ERptMultiLanguage.GetLanValue("btSelect");
            btSelect.Click += new EventHandler(buttonPictureSelect_Click);
            btSelect.CssClass = WebEasilyReportCSS.Button;
            tableLeft.Rows[3].Cells[0].Controls.Add(btSelect);

            //btClose
            Button btClose = new Button();
            btClose.Text = ERptMultiLanguage.GetLanValue("btClose");
            btClose.CssClass = WebEasilyReportCSS.Button;
            btClose.ID = BUTTON_PICTURE_CANCEL;
            tableLeft.Rows[4].Cells[0].Controls.Add(btClose);

            //Image
            System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();
            image.ID = IMAGE_PICTURE;
            tableRight.Rows[0].Cells[0].Controls.Add(image);

            baseTable.Rows[1].Cells[0].ColumnSpan = 2;
            baseTable.Rows[1].Cells[0].Controls.Add(CreateFileUploadView());

            return baseTable;
        }

        private Control CreateFileUploadView()
        {
            Panel panelUpload = new Panel();
            panelUpload.Style.Add(HtmlTextWriterStyle.Display, "none");
            panelUpload.ID = PANEL_FILEUPLOAD_VIEW;
            Table baseTable = CreateTable(2, 1, "eReportBase_table", null);
            Table table = CreateTable(2, 1, "eReportPart_table", null);
            baseTable.Rows[1].Cells[0].Controls.Add(table);
            panelUpload.Controls.Add(baseTable);

            Anthem.FileUpload fileUpload = new Anthem.FileUpload();
            fileUpload.AutoUpdateAfterCallBack = true;
            fileUpload.ID = FILEUPLOAD_PICTURE;
            table.Rows[0].Cells[0].Controls.Add(fileUpload);

            //btOK
            Anthem.Button buttonFileUploadOK = new Anthem.Button();
            buttonFileUploadOK.EnabledDuringCallBack = false;
            buttonFileUploadOK.ID = BUTTON_FILEUPLOAD_OK;
            buttonFileUploadOK.Text = ERptMultiLanguage.GetLanValue("btOK");
            buttonFileUploadOK.CssClass = WebEasilyReportCSS.Button;
            buttonFileUploadOK.Click += new EventHandler(buttonFileUploadOK_Click);
            table.Rows[1].Cells[0].Controls.Add(buttonFileUploadOK);

            //btCancel
            Button buttonFileUploadCancel = new Button();
            buttonFileUploadCancel.Text = ERptMultiLanguage.GetLanValue("btClose");
            buttonFileUploadCancel.CssClass = WebEasilyReportCSS.Button;
            buttonFileUploadCancel.Click += new EventHandler(buttonFileUploadCancel_Click);
            table.Rows[1].Cells[0].Controls.Add(buttonFileUploadCancel);

            return panelUpload;
        }

        private View CreateSettingView()
        {
            View view = new View();
            Table tableSettingMain = CreateTable(2, 2, WebEasilyReportCSS.BaseTable,
                new CellCssClass[] { 
                    new CellCssClass(0, 0, WebEasilyReportCSS.TopCenter),
                    new CellCssClass(0, 1, WebEasilyReportCSS.TopCenter),
                    new CellCssClass(1, 0, WebEasilyReportCSS.TopCenter),
                    new CellCssClass(1, 1, WebEasilyReportCSS.TopCenter)
                });
            Table tablePageSize = CreateTable(2, 1, WebEasilyReportCSS.PartTableNoBackColor + " " + WebEasilyReportCSS.SettingTable,
                new CellCssClass[]{
                    new CellCssClass(0, 0, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(1, 0, WebEasilyReportCSS.TopLeft)
                });
            Table tablePrintOrientation = CreateTable(4, 1, WebEasilyReportCSS.PartTableNoBackColor,
                new CellCssClass[]{
                    new CellCssClass(0, 0, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(1, 0, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(2, 0, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(3, 0, WebEasilyReportCSS.TopLeft)
                });
            Table tableMailSetting = CreateTable(5, 2, WebEasilyReportCSS.PartTableNoBackColor + " " + WebEasilyReportCSS.MailSettingTable,
                new CellCssClass[]{
                    new CellCssClass(0, 0, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableLeft),
                    new CellCssClass(1, 0, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableLeft),
                    new CellCssClass(1, 1, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableRight),
                    new CellCssClass(2, 0, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableLeft),
                    new CellCssClass(2, 1, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableRight),
                    new CellCssClass(3, 0, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableLeft),
                    new CellCssClass(3, 1, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableRight),
                    new CellCssClass(4, 0, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableLeft),
                    new CellCssClass(4, 1, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableRight)
                });

            Table tablePageSetting = CreateTable(4, 2, WebEasilyReportCSS.ContainerTable,
               new CellCssClass[]{
                    new CellCssClass(0, 0, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(0, 1, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(1, 0, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(1, 1, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(2, 0, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(2, 1, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(3, 0, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(3, 1, WebEasilyReportCSS.TopLeft)
                });

            Table tableOutputSetting = CreateTable(3, 2, "eReportFullWidth_table",
              new CellCssClass[]{
                    new CellCssClass(0, 0, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(0, 1, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(1, 0, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(1, 1, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(2, 0, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(2, 1, WebEasilyReportCSS.TopLeft)
                });

            Table tablePageMargin = CreateTable(2, 4, "eReportContainer_table",
              new CellCssClass[]{
                    new CellCssClass(0, 0, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(0, 1, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(0, 2, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(0, 3, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(1, 0, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(1, 1, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(1, 2, WebEasilyReportCSS.TopLeft),
                    new CellCssClass(1, 3, WebEasilyReportCSS.TopLeft)
                });

            #region tableSettingMain
            view.Controls.Add(tableSettingMain);
            Panel panelPageSetting = new Panel();
            panelPageSetting.CssClass = WebEasilyReportCSS.Panel_PageSetting;
            panelPageSetting.GroupingText = ERptMultiLanguage.GetLanValue("gbPageSetting");
            panelPageSetting.Controls.Add(tablePageSetting);

            Panel panelPageSize = new Panel();
            panelPageSize.CssClass = WebEasilyReportCSS.Panel_PageSize;
            panelPageSize.GroupingText = ERptMultiLanguage.GetLanValue("gbPageSize");
            panelPageSize.Controls.Add(tablePageSize);

            Panel panelPrintOrientation = new Panel();
            panelPrintOrientation.CssClass = WebEasilyReportCSS.Panel_PrintOrientation;
            panelPrintOrientation.GroupingText = ERptMultiLanguage.GetLanValue("gbPrintOrientation");
            panelPrintOrientation.Controls.Add(tablePrintOrientation);

            Panel panelOutputSetting = new Panel();
            panelOutputSetting.CssClass = WebEasilyReportCSS.Panel_OutputSetting;
            panelOutputSetting.GroupingText = ERptMultiLanguage.GetLanValue("gbOutputSetting");
            panelOutputSetting.Controls.Add(tableOutputSetting);

            Panel panelMailSetting = new Panel();
            panelMailSetting.CssClass = WebEasilyReportCSS.Panel_MailSetting;
            panelMailSetting.GroupingText = ERptMultiLanguage.GetLanValue("gbMailConfig");
            panelMailSetting.Controls.Add(tableMailSetting);

            Panel panelPageMargin = new Panel();
            panelPageMargin.CssClass = WebEasilyReportCSS.Panel_PageMargin;
            panelPageMargin.ID = PANEL_PAGE_MARGIN;
            panelPageMargin.GroupingText = ERptMultiLanguage.GetLanValue("gbPageMargin");
            panelPageMargin.Controls.Add(tablePageMargin);

            tableSettingMain.Rows[0].Cells[0].Controls.Add(panelPageSetting);
            tableSettingMain.Rows[0].Cells[1].Controls.Add(panelMailSetting);
            tableSettingMain.Rows[1].Cells[0].ColumnSpan = 2;
            tableSettingMain.Rows[1].Cells[0].Controls.Add(panelOutputSetting);

            #endregion

            #region tablePageSetting
            tablePageSetting.Rows[0].Cells[0].RowSpan = 3;
            tablePageSetting.Rows[0].Cells[0].Controls.Add(panelPageSize);

            CheckBox checkboxHeaderRepeat = new CheckBox();
            checkboxHeaderRepeat.ID = CHECKBOX_HEADER_REPEAT;
            checkboxHeaderRepeat.Text = ERptMultiLanguage.GetLanValue("cbHeaderRepeat");
            tablePageSetting.Rows[3].Cells[0].Controls.Add(checkboxHeaderRepeat);

            tablePageSetting.Rows[1].Cells[1].Controls.Add(panelPrintOrientation);
            tablePageSetting.Rows[2].Cells[1].Controls.Add(panelPageMargin);
            #endregion

            #region tablePageSize
            RadioButtonList radiobuttonlistPageSize = new RadioButtonList();
            radiobuttonlistPageSize.ID = RADIOBUTTONLIST_PAGE_SIZE;
            foreach (object item in Enum.GetValues(typeof(ReportFormat.PageType)))
            {
                radiobuttonlistPageSize.Items.Add(item.ToString());
            }

            tablePageSize.Rows[1].Cells[0].Controls.Add(radiobuttonlistPageSize);
            #endregion

            #region tablePrintOrientation
            RadioButtonList radiobuttonlistPrintOrientation = new RadioButtonList();
            radiobuttonlistPrintOrientation.ID = RADIOBUTTONLIST_PRINT_ORIENTATION;
            foreach (object item in Enum.GetValues(typeof(Orientation)))
            {
                radiobuttonlistPrintOrientation.Items.Add(item.ToString());
            }
            tablePrintOrientation.Rows[1].Cells[0].Controls.Add(radiobuttonlistPrintOrientation);
            #endregion

            #region tablePageMargin
            Label lbLeft = new Label();
            lbLeft.CssClass = "eReport_label";
            lbLeft.Text = ERptMultiLanguage.GetLanValue("lbLeft");
            tablePageMargin.Rows[0].Cells[0].Controls.Add(lbLeft);

            TextBox tbLeft = new TextBox();
            tbLeft.ID = TEXTBOX_LEFTMARGIN;
            tbLeft.Attributes.Add("onkeydown", "CheckDecimal(this, event);");
            tbLeft.Width = new Unit(25);
            tablePageMargin.Rows[0].Cells[1].Controls.Add(tbLeft);

            Label lbTop = new Label();
            lbTop.CssClass = "eReport_label";
            lbTop.Text = ERptMultiLanguage.GetLanValue("lbTop");
            tablePageMargin.Rows[0].Cells[2].Controls.Add(lbTop);

            TextBox tbTop = new TextBox();
            tbTop.ID = TEXTBOX_TOPMARGIN;
            tbTop.Attributes.Add("onkeydown", "CheckDecimal(this, event);");
            tbTop.Width = new Unit(25);
            tablePageMargin.Rows[0].Cells[3].Controls.Add(tbTop);

            Label lbRight = new Label();
            lbRight.CssClass = "eReport_label";
            lbRight.Text = ERptMultiLanguage.GetLanValue("lbRight");
            tablePageMargin.Rows[1].Cells[0].Controls.Add(lbRight);

            TextBox tbRight = new TextBox();
            tbRight.ID = TEXTBOX_RIGHTMARGIN;
            tbRight.Attributes.Add("onkeydown", "CheckDecimal(this, event);");
            tbRight.Width = new Unit(25);
            tablePageMargin.Rows[1].Cells[1].Controls.Add(tbRight);

            Label lbBottom = new Label();
            lbBottom.CssClass = "eReport_label";
            lbBottom.Text = ERptMultiLanguage.GetLanValue("lbBottom");
            tablePageMargin.Rows[1].Cells[2].Controls.Add(lbBottom);

            TextBox tbBottom = new TextBox();
            tbBottom.ID = TEXTBOX_BOTTOMMARGIN;
            tbBottom.Attributes.Add("onkeydown", "CheckDecimal(this, event);");
            tbBottom.Width = new Unit(25);
            tablePageMargin.Rows[1].Cells[3].Controls.Add(tbBottom);
            #endregion

            #region tableMailSetting
            Label labelMailTitle = new Label();
            labelMailTitle.Text = ERptMultiLanguage.GetLanValue("lbMailTitle");
            labelMailTitle.CssClass = WebEasilyReportCSS.Label;
            tableMailSetting.Rows[0].Cells[0].Controls.Add(labelMailTitle);
            TextBox textboxMailTitle = new TextBox();
            textboxMailTitle.ID = TEXTBOX_MAIL_TITLE;
            textboxMailTitle.Width = new Unit(100);
            tableMailSetting.Rows[0].Cells[1].Controls.Add(textboxMailTitle);
            Label labelMailTo = new Label();
            labelMailTo.CssClass = WebEasilyReportCSS.Label;
            labelMailTo.Text = ERptMultiLanguage.GetLanValue("lbMailTo");
            tableMailSetting.Rows[1].Cells[0].Controls.Add(labelMailTo);
            TextBox textboxMailTo = new TextBox();
            textboxMailTo.Width = new Unit(100);
            textboxMailTo.ID = TEXTBOX_MAIL_TO;
            tableMailSetting.Rows[1].Cells[1].Controls.Add(textboxMailTo);
            Label labelMailFrom = new Label();
            labelMailFrom.CssClass = WebEasilyReportCSS.Label;
            labelMailFrom.Text = ERptMultiLanguage.GetLanValue("lbMailFrom");
            tableMailSetting.Rows[2].Cells[0].Controls.Add(labelMailFrom);
            TextBox textboxMailFrom = new TextBox();
            textboxMailFrom.Width = new Unit(100);
            textboxMailFrom.ID = TEXTBOX_MAIL_FROM;
            tableMailSetting.Rows[2].Cells[1].Controls.Add(textboxMailFrom);
            Label labelMailPassword = new Label();
            labelMailPassword.CssClass = WebEasilyReportCSS.Label;
            labelMailPassword.Text = ERptMultiLanguage.GetLanValue("lbPassword");
            tableMailSetting.Rows[3].Cells[0].Controls.Add(labelMailPassword);
            TextBox textboxMailPassword = new TextBox();
            textboxMailPassword.Width = new Unit(100);
            textboxMailPassword.ID = TEXTBOX_MAIL_PASSWORD;
            tableMailSetting.Rows[3].Cells[1].Controls.Add(textboxMailPassword);
            Label labelMailServer = new Label();
            labelMailServer.CssClass = WebEasilyReportCSS.Label;
            labelMailServer.Text = ERptMultiLanguage.GetLanValue("lbSmtpServer");
            tableMailSetting.Rows[4].Cells[0].Controls.Add(labelMailServer);
            TextBox textboxMailServer = new TextBox();
            textboxMailServer.Width = new Unit(100);
            textboxMailServer.ID = TEXTBOX_MAIL_SERVER;
            tableMailSetting.Rows[4].Cells[1].Controls.Add(textboxMailServer);
            #endregion

            #region tableOutputSetting
            Label labelExportFormat = new Label();
            //set text
            labelExportFormat.CssClass = "eReport_label";
            labelExportFormat.Text = ERptMultiLanguage.GetLanValue("lbExportFormat");
            tableOutputSetting.Rows[0].Cells[0].Controls.Add(labelExportFormat);
            DropDownList dropdownlistExportFormat = new DropDownList();
            dropdownlistExportFormat.ID = DROPDOWNLIST_EXPORT_FORMAT;

            dropdownlistExportFormat.AutoPostBack = true;
            dropdownlistExportFormat.SelectedIndexChanged += new EventHandler(dropdownlistExportFormat_SelectedIndexChanged);

            foreach (object item in Enum.GetValues(typeof(ReportFormat.ExportType)))
            {
                dropdownlistExportFormat.Items.Add(item.ToString());
            }
            tableOutputSetting.Rows[0].Cells[0].Controls.Add(dropdownlistExportFormat);

            Label labelPageRecords = new Label();
            labelPageRecords.CssClass = WebEasilyReportCSS.Label;
            labelPageRecords.Text = ERptMultiLanguage.GetLanValue("lbPageRecords");
            tableOutputSetting.Rows[1].Cells[0].Controls.Add(labelPageRecords);
            TextBox textboxPageRecords = new TextBox();
            textboxPageRecords.Width = new Unit(80);
            textboxPageRecords.ID = TEXTBOX_PAGE_RECORDS;
            textboxPageRecords.Attributes.Add("onkeypress", "CheckNum();");
            tableOutputSetting.Rows[1].Cells[0].Controls.Add(textboxPageRecords);

            //Label labelOutputMode = new Label();
            //labelOutputMode.CssClass = WebEasilyReportCSS.Label;
            //labelOutputMode.Text = ERptMultiLanguage.GetLanValue("lbOutputMode");
            //tableOutputSetting.Rows[2].Cells[0].Controls.Add(labelOutputMode);
            CheckBox checkboxSendMail = new CheckBox();
            checkboxSendMail.Text = ERptMultiLanguage.GetLanValue("cbxSendMail");
            checkboxSendMail.ID = CHECKBOX_SEND_MAIL;
            tableOutputSetting.Rows[2].Cells[0].Controls.Add(checkboxSendMail);
            #endregion

            return view;

        }

        private Control CreateDownLoadView()
        {
            View view = new View();
            Table baseTable = CreateTable(1, 1, WebEasilyReportCSS.BaseTable, null);
            Table table = CreateTable(3, 1, WebEasilyReportCSS.PartTable + " " + WebEasilyReportCSS.DownloadTable,
                new CellCssClass[] {
                    new CellCssClass(0,0,WebEasilyReportCSS.MiddleCenter),
                    new CellCssClass(1,0,WebEasilyReportCSS.BotttomCenter),
                    new CellCssClass(2,0,WebEasilyReportCSS.BottomRight)
                });
            baseTable.Rows[0].Cells[0].Controls.Add(table);

            HyperLink hyperLink = new HyperLink();
            hyperLink.Text = ERptMultiLanguage.GetLanValue("hlDownload");
            hyperLink.ID = HYPERLINK_DOWNLOAD;
            table.Rows[1].Cells[0].Controls.Add(hyperLink);

            Button btClose = new Button();
            btClose.Text = ERptMultiLanguage.GetLanValue("btClose");
            btClose.CssClass = WebEasilyReportCSS.Button;
            btClose.ID = BUTTON_CLOSE_DOWNLOADVIEW;
            btClose.Click += new EventHandler(btClose_Click);
            table.Rows[2].Cells[0].Controls.Add(btClose);

            view.Controls.Add(baseTable);
            return view;
        }

        void btClose_Click(object sender, EventArgs e)
        {
            HideModelPopup(ModelPopupView.DownloadView);
        }

        private Control CreateProgressView()
        {
            View view = new View();
            //UpdateProgress updateProgess = new UpdateProgress();
            //UpdatePanel updatePanel = (UpdatePanel)this.GetUpdatePanel();
            //updateProgess.AssociatedUpdatePanelID = updatePanel.ID;

            //System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();
            //image.ImageUrl = Infolight.EasilyReportTools.Tools.ImageFilePath.ProgressBarUrl;

            //updateProgess.Controls.Add(image);

            //updatePanel.Parent.Controls.Add(updateProgess);
            //view.Controls.Add(updateProgess);
            return view;
        }

        private Control CreateSendMailView()
        {
            View view = new View();

            Table baseTable = CreateTable(1, 1, WebEasilyReportCSS.BaseTable, null);

            Table tableMailSetting = CreateTable(6, 2, WebEasilyReportCSS.PartTable + " " + WebEasilyReportCSS.MailSettingTable,
               new CellCssClass[]{
                    new CellCssClass(0, 0, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableLeft),
                    new CellCssClass(1, 0, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableLeft),
                    new CellCssClass(1, 1, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableRight),
                    new CellCssClass(2, 0, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableLeft),
                    new CellCssClass(2, 1, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableRight),
                    new CellCssClass(3, 0, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableLeft),
                    new CellCssClass(3, 1, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableRight),
                    new CellCssClass(4, 0, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableLeft),
                    new CellCssClass(4, 1, WebEasilyReportCSS.MiddleLeft + " " + WebEasilyReportCSS.SettingTableRight),
                    new CellCssClass(5, 0, WebEasilyReportCSS.MiddleRight),
                    new CellCssClass(5, 1, WebEasilyReportCSS.MiddleLeft)
                });

            Panel panelMailSend = new Panel();
            panelMailSend.CssClass = WebEasilyReportCSS.Panel_MailSend;
            panelMailSend.GroupingText = ERptMultiLanguage.GetLanValue("gbMailSend");
            panelMailSend.Controls.Add(tableMailSetting);

            baseTable.Rows[0].Cells[0].Controls.Add(panelMailSend);
            view.Controls.Add(baseTable);

            #region tableMailSetting
            Label labelMailTitle = new Label();
            labelMailTitle.Text = ERptMultiLanguage.GetLanValue("lbMailTitle");
            labelMailTitle.CssClass = WebEasilyReportCSS.Label;
            tableMailSetting.Rows[0].Cells[0].Controls.Add(labelMailTitle);
            TextBox textboxMailTitle = new TextBox();
            textboxMailTitle.ID = TEXTBOX_SENDMAIL_TITLE;
            textboxMailTitle.Width = new Unit(100);
            tableMailSetting.Rows[0].Cells[1].Controls.Add(textboxMailTitle);
            Label labelMailTo = new Label();
            labelMailTo.CssClass = WebEasilyReportCSS.Label;
            labelMailTo.Text = ERptMultiLanguage.GetLanValue("lbMailTo");
            tableMailSetting.Rows[1].Cells[0].Controls.Add(labelMailTo);
            TextBox textboxMailTo = new TextBox();
            textboxMailTo.Width = new Unit(100);
            textboxMailTo.ID = TEXTBOX_SENDMAIL_TO;
            tableMailSetting.Rows[1].Cells[1].Controls.Add(textboxMailTo);
            Label labelMailFrom = new Label();
            labelMailFrom.CssClass = WebEasilyReportCSS.Label;
            labelMailFrom.Text = ERptMultiLanguage.GetLanValue("lbMailFrom");
            tableMailSetting.Rows[2].Cells[0].Controls.Add(labelMailFrom);
            TextBox textboxMailFrom = new TextBox();
            textboxMailFrom.Width = new Unit(100);
            textboxMailFrom.ID = TEXTBOX_SENDMAIL_FROM;
            tableMailSetting.Rows[2].Cells[1].Controls.Add(textboxMailFrom);
            Label labelMailPassword = new Label();
            labelMailPassword.CssClass = WebEasilyReportCSS.Label;
            labelMailPassword.Text = ERptMultiLanguage.GetLanValue("lbPassword");
            tableMailSetting.Rows[3].Cells[0].Controls.Add(labelMailPassword);
            TextBox textboxMailPassword = new TextBox();
            textboxMailPassword.Width = new Unit(100);
            textboxMailPassword.ID = TEXTBOX_SENDMAIL_PASSWORD;
            tableMailSetting.Rows[3].Cells[1].Controls.Add(textboxMailPassword);
            Label labelMailServer = new Label();
            labelMailServer.CssClass = WebEasilyReportCSS.Label;
            labelMailServer.Text = ERptMultiLanguage.GetLanValue("lbSmtpServer");
            tableMailSetting.Rows[4].Cells[0].Controls.Add(labelMailServer);
            TextBox textboxMailServer = new TextBox();
            textboxMailServer.Width = new Unit(100);
            textboxMailServer.ID = TEXTBOX_SENDMAIL_SERVER;
            tableMailSetting.Rows[4].Cells[1].Controls.Add(textboxMailServer);

            Button buttonSendMail = new Button();
            buttonSendMail.ID = BUTTON_SENDMAIL;
            buttonSendMail.CssClass = WebEasilyReportCSS.Button;
            buttonSendMail.Text = ERptMultiLanguage.GetLanValue("btWebSendMail");
            buttonSendMail.Click += new EventHandler(buttonSendMail_Click);
            tableMailSetting.Rows[5].Cells[0].Controls.Add(buttonSendMail);

            Button buttonClose = new Button();
            buttonClose.CssClass = WebEasilyReportCSS.Button;
            buttonClose.Text = ERptMultiLanguage.GetLanValue("btClose");
            buttonClose.Click += new EventHandler(buttonSendMailClose_Click);
            tableMailSetting.Rows[5].Cells[1].Controls.Add(buttonClose);
            #endregion

            return view;
        }

        void buttonSendMailClose_Click(object sender, EventArgs e)
        {
            HideModelPopup(ModelPopupView.DownloadView);
        }

        void buttonSendMail_Click(object sender, EventArgs e)
        {
            ShowModelPopup(ModelPopupView.DownloadView);
            ActiveView(OutputView.SendMailView);
        }

        private void SendMail()
        {
            MailSender mailSender = new MailSender(this.report);
            mailSender.SendMail();
        }

        private Control CreateOutputView()
        {
            MultiView multiViewOutput = new MultiView();
            multiViewOutput.ID = MULTIVIEW_OUTPUTVIEW;
            multiViewOutput.Controls.Add((View)CreateProgressView());
            multiViewOutput.Controls.Add((View)CreateDownLoadView());
            multiViewOutput.Controls.Add((View)CreateSendMailView());
            return multiViewOutput;
        }

        #region CreateView
        void buttoItemnAdd_Click(object sender, EventArgs e)
        {
            string name = (sender as Control).ID.Replace(BUTTON_ITEM_ADD, string.Empty);
            Control container = this.report.MultiView;
            DropDownList dropdownlistItemType = (DropDownList)container.FindControl(string.Format("{0}{1}", DROPDOWNLIST_ITEM_TYPE, name));
            string typeName = dropdownlistItemType.SelectedValue;
            Type type = this.GetType().Assembly.GetType(string.Format(ComponentInfo.NameSpace + ".{0}", typeName));
            ReportItem item = (ReportItem)Activator.CreateInstance(type);
            Guid id = Guid.NewGuid();
            report.ItemTable.Add(id, XmlConverter.ConvertFrom(item));
            ListBox listboxItem = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_ITEM, name));
            listboxItem.Items.Add(new ListItem(item.ToString(), id.ToString()));
            listboxItem.SelectedValue = id.ToString();
            listboxItem_SelectedIndexChanged(listboxItem, e);//触发事件
        }

        void buttonItemRemove_Click(object sender, EventArgs e)
        {
            string name = (sender as Control).ID.Replace(BUTTON_ITEM_REMOVE, string.Empty);
            Control container = this.report.MultiView;
            ListBox listboxItem = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_ITEM, name));
            if (!string.IsNullOrEmpty(listboxItem.SelectedValue))
            {
                Guid id = new Guid(listboxItem.SelectedValue);
                report.ItemTable.Remove(id);
                listboxItem.Items.Remove(listboxItem.SelectedItem);
                this.report.ItemIndexTable[name] = Guid.Empty;//上一笔清空
                listboxItem_SelectedIndexChanged(listboxItem, e);//触发事件
            }
        }

        void buttonFont_Click(object sender, EventArgs e)
        {
            this.ShowModelPopup(ModelPopupView.FontView);

            report.FontButtonID = (sender as Control).ID;
            InitialFontView((Font)report.FontTable[report.FontButtonID]);
        }

        void listboxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            //保存上一笔
            ListBox listboxItem = sender as ListBox;
            string name = listboxItem.ID.Replace(LISTBOX_ITEM, string.Empty);
            if (this.report.ItemIndexTable.Contains(name) && !this.report.ItemIndexTable[name].Equals(Guid.Empty))
            {
                //保存
                Guid id = (Guid)this.report.ItemIndexTable[name];
                SaveItem(name, id);
            }
            //设置当前笔
            if (!string.IsNullOrEmpty(listboxItem.SelectedValue))
            {
                Guid id = new Guid(listboxItem.SelectedValue);
                this.report.ItemIndexTable[name] = id;
                LoadItem(name, id);
            }
            else
            {
                LoadItem(name, Guid.Empty);
            }
        }

        void dropdownlistContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control container = this.report.MultiView;
            DropDownList dropDownList = (sender as DropDownList);
            string name = dropDownList.ID.Replace(DROPDOWNLIST_ITEM_CONTENT, string.Empty);

            TextBox textboxFormat = (TextBox)container.FindControl(string.Format("{0}{1}", TEXTBOX_ITEM_FORMAT, name));
            textboxFormat.Text = dropDownList.SelectedItem.Text + ":{0}";

            if (this.report.ItemIndexTable.Contains(name) && !this.report.ItemIndexTable[name].Equals(Guid.Empty))
            {
                ListBox listboxItem = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_ITEM, name));

                Guid id = (Guid)this.report.ItemIndexTable[name];
                byte[] value = (byte[])report.ItemTable[id];
                ReportItem reportItem = (ReportItem)XmlConverter.ConvertTo(value);
                if (reportItem is ReportConstantItem)
                {
                    listboxItem.SelectedItem.Text = dropDownList.SelectedItem.Text;
                }
                else if (reportItem is ReportImageItem)
                {
                    listboxItem.SelectedItem.Text = string.Format("{0}({1})", ERptMultiLanguage.GetLanValue("ImageItem"), dropDownList.SelectedValue);
                }
                else if (reportItem is ReportDataSourceItem)
                {
                    listboxItem.SelectedItem.Text = string.Format("{0}({1})", ERptMultiLanguage.GetLanValue("DataSourceItem"), dropDownList.SelectedValue);
                }
            }
        }

        private void LoadItem(string name, Guid id)
        {
            Control container = this.report.MultiView;
            Table tableItem = (Table)container.FindControl(string.Format("{0}{1}", TABLE_ITEM, name));

            Button buttonAddPicture = (Button)container.FindControl(string.Format("{0}{1}", BUTTON_ADD_PICTURE, name));

            if (id != Guid.Empty)
            {
                buttonAddPicture.Visible = false;

                tableItem.Visible = true;
                byte[] value = (byte[])report.ItemTable[id];
                ReportItem reportItem = (ReportItem)XmlConverter.ConvertTo(value);
                DropDownList dropdownlistContent = (DropDownList)container.FindControl(string.Format("{0}{1}", DROPDOWNLIST_ITEM_CONTENT, name));
                dropdownlistContent.Items.Clear();

                dropdownlistContent.Enabled = true;

                if (reportItem is ReportConstantItem)
                {
                    foreach (object item in Enum.GetValues(typeof(ReportConstantItem.StyleType)))
                    {
                        dropdownlistContent.Items.Add(new ListItem(ERptMultiLanguage.GetLanValue(item.ToString()), item.ToString()));
                    }

                    dropdownlistContent.SelectedValue = (reportItem as ReportConstantItem).Style.ToString();

                }
                else if (reportItem is ReportImageItem)
                {
                    //for (int i = 0; i < Images.Count; i++)
                    //{
                    //    dropdownlistContent.Items.Add(new ListItem(string.Format("Image{0}", i), i.ToString()));
                    //}
                    int index = (reportItem as ReportImageItem).Index;
                    if (index >= 0 && index < report.ImageItemTable.Count)
                    //if (index >= 0 && index < Images.Count)
                    {
                        if (!dropdownlistContent.SelectedValue.Contains(index.ToString()))
                        {
                            dropdownlistContent.Items.Add(index.ToString());
                        }
                        dropdownlistContent.SelectedValue = index.ToString();
                    }

                    buttonAddPicture.Visible = true;
                    dropdownlistContent.Enabled = false;
                }
                else if (reportItem is ReportParameterItem)
                {
                    for (int i = 0; i < report.Parameters.Count; i++)
                    {
                        dropdownlistContent.Items.Add(new ListItem(string.Format("Parameter{0}", i), i.ToString()));
                    }
                    int index = (reportItem as ReportParameterItem).Index;
                    if (index >= 0 && index < report.Parameters.Count)
                    {
                        dropdownlistContent.SelectedValue = index.ToString();
                    }
                }
                else if (reportItem is ReportDataSourceItem)
                {
                    WebDataSource wds = (WebDataSource)report.HeaderDataSource;
                    if (wds != null)
                    {
                        DataView view = wds.View;
                        if (view != null)
                        {
                            DataTable table = view.Table;
                            //DD
                            DataSet ds = DBUtils.GetDataDictionary(wds, false);
                            foreach (DataColumn column in table.Columns)
                            {
                                DataRow[] drDD = ds.Tables[0].Select(string.Format("FIELD_NAME='{0}'", column.ColumnName));
                                if (drDD.Length > 0)
                                {
                                    dropdownlistContent.Items.Add(new ListItem(drDD[0]["CAPTION"].ToString(), column.ColumnName));
                                }
                                else
                                {
                                    dropdownlistContent.Items.Add(new ListItem(column.ColumnName, column.ColumnName));
                                }
                            }
                            string field = (reportItem as ReportDataSourceItem).ColumnName;
                            if (!string.IsNullOrEmpty(field) && table.Columns.Contains(field))
                            {
                                dropdownlistContent.SelectedValue = field;
                            }
                        }
                    }
                }
                DropDownList dropdownlistContentAlignment = (DropDownList)container.FindControl(string.Format("{0}{1}", DROPDOWNLIST_ITEM_CONTENT_ALIGNMENT, name));
                dropdownlistContentAlignment.SelectedValue = reportItem.ContentAlignment.ToString();
                DropDownList dropdownlistPosition = (DropDownList)container.FindControl(string.Format("{0}{1}", DROPDOWNLIST_ITEM_POSITION, name));
                dropdownlistPosition.SelectedValue = reportItem.Position.ToString();
                TextBox textboxCells = (TextBox)container.FindControl(string.Format("{0}{1}", TEXTBOX_ITEM_CELLS, name));
                textboxCells.Text = reportItem.Cells.ToString();
                CheckBox checkboxNewLine = (CheckBox)container.FindControl(string.Format("{0}{1}", CHECKBOX_ITEM_NEWLINE, name));
                checkboxNewLine.Checked = reportItem.NewLine;
                TextBox textboxFormat = (TextBox)container.FindControl(string.Format("{0}{1}", TEXTBOX_ITEM_FORMAT, name));
                textboxFormat.Text = reportItem.Format;
                string fontButtonID = string.Format("{0}{1}", BUTTON_ITEM_DETAIL_FONT, name);
                report.FontTable[fontButtonID] = reportItem.Font;

                Label labelFontSample = (Label)container.FindControl(string.Format("{0}{1}", LABEL_ITEM_DETAIL_FONT_SAMPLE, name));
                labelFontSample.Font.CopyFrom((FontInfo)WebFontConverter.ConvertFrom(reportItem.Font));
            }
            else
            {
                tableItem.Visible = false;
            }
        }

        private void SaveItem(string name, Guid id)
        {
            byte[] value = (byte[])report.ItemTable[id];
            ReportItem reportItem = (ReportItem)XmlConverter.ConvertTo(value);
            Control container = this.report.MultiView;
            DropDownList dropdownlistContent = (DropDownList)container.FindControl(string.Format("{0}{1}", DROPDOWNLIST_ITEM_CONTENT, name));
            if (reportItem is ReportConstantItem)
            {
                (reportItem as ReportConstantItem).Style = (ReportConstantItem.StyleType)Enum.Parse(typeof(ReportConstantItem.StyleType), dropdownlistContent.SelectedValue);
            }
            else if (reportItem is ReportImageItem)
            {
                if (!string.IsNullOrEmpty(dropdownlistContent.SelectedValue))
                {
                    (reportItem as ReportImageItem).Index = Convert.ToInt32(dropdownlistContent.SelectedValue);
                }
            }
            else if (reportItem is ReportParameterItem)
            {
                if (!string.IsNullOrEmpty(dropdownlistContent.SelectedValue))
                {
                    (reportItem as ReportParameterItem).Index = Convert.ToInt32(dropdownlistContent.SelectedValue);
                }
            }
            else if (reportItem is ReportDataSourceItem)
            {
                (reportItem as ReportDataSourceItem).ColumnName = dropdownlistContent.SelectedValue;
            }
            DropDownList dropdownlistContentAlignment = (DropDownList)container.FindControl(string.Format("{0}{1}", DROPDOWNLIST_ITEM_CONTENT_ALIGNMENT, name));
            reportItem.ContentAlignment = (System.Windows.Forms.HorizontalAlignment)Enum.Parse(typeof(System.Windows.Forms.HorizontalAlignment), dropdownlistContentAlignment.SelectedValue);
            DropDownList dropdownlistPosition = (DropDownList)container.FindControl(string.Format("{0}{1}", DROPDOWNLIST_ITEM_POSITION, name));
            reportItem.Position = (ReportItem.PositionAlign)Enum.Parse(typeof(ReportItem.PositionAlign), dropdownlistPosition.SelectedValue);
            TextBox textboxCells = (TextBox)container.FindControl(string.Format("{0}{1}", TEXTBOX_ITEM_CELLS, name));
            reportItem.Cells = Convert.ToInt32(textboxCells.Text);
            CheckBox checkboxNewLine = (CheckBox)container.FindControl(string.Format("{0}{1}", CHECKBOX_ITEM_NEWLINE, name));
            reportItem.NewLine = checkboxNewLine.Checked;
            TextBox textboxFormat = (TextBox)container.FindControl(string.Format("{0}{1}", TEXTBOX_ITEM_FORMAT, name));
            reportItem.Format = textboxFormat.Text;
            string fontButtonID = string.Format("{0}{1}", BUTTON_ITEM_DETAIL_FONT, name);
            if (report.FontTable.Contains(fontButtonID))
            {
                reportItem.Font = (Font)report.FontTable[fontButtonID];
            }
            report.ItemTable[id] = XmlConverter.ConvertFrom(reportItem);
        }

        

        void dropdownlistField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (report.LastIndex != -1)
            {
                SaveField(report.LastIndex);
            }
            int index = (sender as DropDownList).SelectedIndex;
            InitialFieldView(this.report, report.FieldFont, index);
            report.LastIndex = index;
        }

        void webMultiViewCaptions_TabChanged(object sender, TabChangedEventArgs e)
        {
            Control container = this.report.MultiView;
            MultiView multiviewFieldContent = (MultiView)container.FindControl(MULTIVIEW_FIELD_CONTENT);
            MultiView multiviewField = (MultiView)container.FindControl(MULTIVIEW_FIELD);
            multiviewFieldContent.ActiveViewIndex = multiviewField.ActiveViewIndex;
        }

        void buttonFieldAdd_Click(object sender, EventArgs e)
        {
            string name = (sender as Control).ID.Replace(BUTTON_FIELD_ADD, string.Empty);
            Control container = this.report.MultiView;
            ListBox listboxFieldAll = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_ALL, name));
            if (!string.IsNullOrEmpty(listboxFieldAll.SelectedValue))
            {
                ListBox listboxFieldSelected = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_SELECTED, name));
                if (name.Length == 0)
                {
                    FieldItem item = new FieldItem();
                    item.ColumnName = listboxFieldAll.SelectedValue;
                    item.Caption = listboxFieldAll.SelectedItem.Text;
                    Guid id = Guid.NewGuid();
                    report.ItemTable.Add(id, XmlConverter.ConvertFrom(item));
                    listboxFieldSelected.Items.Add(new ListItem(listboxFieldAll.SelectedItem.Text, id.ToString()));
                    listboxFieldSelected.SelectedValue = id.ToString();
                }
                else
                {
                    listboxFieldSelected.Items.Add(new ListItem(listboxFieldAll.SelectedItem.Text, listboxFieldAll.SelectedItem.Value));
                }
                listboxFieldAll.Items.Remove(listboxFieldAll.SelectedItem);
                listboxFieldSelected_SelectedIndexChanged(listboxFieldSelected, e);
            }
        }

        void buttonFieldRemove_Click(object sender, EventArgs e)
        {
            string name = (sender as Control).ID.Replace(BUTTON_FIELD_REMOVE, string.Empty);
            Control container = this.report.MultiView;
            ListBox listboxFieldSelected = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_SELECTED, name));
            if (!string.IsNullOrEmpty(listboxFieldSelected.SelectedValue))
            {
                ListBox listboxFieldAll = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_ALL, name));
                if (name.Length == 0)
                {
                    Guid id = new Guid(listboxFieldSelected.SelectedValue);
                    byte[] value = (byte[])report.ItemTable[id];
                    FieldItem fieldItem = (FieldItem)XmlConverter.ConvertTo(value);
                    report.ItemTable.Remove(id);
                    this.report.ItemIndexTable[name] = Guid.Empty;//上一笔清空
                    listboxFieldAll.Items.Add(new ListItem(listboxFieldSelected.SelectedItem.Text, fieldItem.ColumnName));
                }
                else
                {
                    listboxFieldAll.Items.Add(new ListItem(listboxFieldSelected.SelectedItem.Text, listboxFieldSelected.SelectedItem.Value));
                }
                listboxFieldSelected.Items.Remove(listboxFieldSelected.SelectedItem);
                listboxFieldSelected_SelectedIndexChanged(listboxFieldSelected, e);
            }
        }

        void buttonFieldAddAll_Click(object sender, EventArgs e)
        {
            string name = (sender as Control).ID.Replace(BUTTON_FIELD_ADD_ALL, string.Empty);
            Control container = this.report.MultiView;
            ListBox listboxFieldAll = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_ALL, name));
            ListBox listboxFieldSelected = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_SELECTED, name));
            foreach (ListItem li in listboxFieldAll.Items)
            {
                if (name.Length == 0)
                {
                    FieldItem item = new FieldItem();
                    item.ColumnName = li.Value;
                    item.Caption = li.Text;
                    Guid id = Guid.NewGuid();
                    report.ItemTable.Add(id, XmlConverter.ConvertFrom(item));
                    listboxFieldSelected.Items.Add(new ListItem(li.Text, id.ToString()));
                }
                else
                {
                    listboxFieldSelected.Items.Add(new ListItem(li.Text, li.Value));
                }
            }
            listboxFieldAll.Items.Clear();
        }

        void buttonFieldRemoveAll_Click(object sender, EventArgs e)
        {
            string name = (sender as Control).ID.Replace(BUTTON_FIELD_REMOVE_ALL, string.Empty);
            Control container = this.report.MultiView;
            ListBox listboxFieldAll = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_ALL, name));
            ListBox listboxFieldSelected = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_SELECTED, name));
            foreach (ListItem li in listboxFieldSelected.Items)
            {
                if (name.Length == 0)
                {
                    Guid id = new Guid(li.Value);
                    byte[] value = (byte[])report.ItemTable[id];
                    FieldItem fieldItem = (FieldItem)XmlConverter.ConvertTo(value);
                    report.ItemTable.Remove(id);
                    this.report.ItemIndexTable[name] = Guid.Empty;//上一笔清空
                    listboxFieldAll.Items.Add(new ListItem(li.Text, fieldItem.ColumnName));
                }
                else
                {
                    listboxFieldAll.Items.Add(new ListItem(li.Text, li.Value));
                }
            }
            listboxFieldSelected.Items.Clear();
        }

        void listboxFieldSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            //保存上一笔
            ListBox listboxField = sender as ListBox;
            string name = listboxField.ID.Replace(LISTBOX_FIELD_SELECTED, string.Empty);
            if (name.Length == 0)
            {
                if (this.report.ItemIndexTable.Contains(name) && !this.report.ItemIndexTable[name].Equals(Guid.Empty))
                {
                    //保存
                    Guid id = (Guid)this.report.ItemIndexTable[name];
                    SaveFieldItem(name, id);
                }
                //设置当前笔
                if (!string.IsNullOrEmpty(listboxField.SelectedValue))
                {
                    Guid id = new Guid(listboxField.SelectedValue);
                    this.report.ItemIndexTable[name] = id;
                    LoadFieldItem(name, id);
                }
                else
                {
                    LoadFieldItem(name, Guid.Empty);
                }
            }
        }

        private void LoadFieldItem(string name, Guid id)
        {
            Control container = this.report.MultiView;
            MultiView multiview = (MultiView)container.FindControl(MULTIVIEW_FIELD_CONTENT);
            if (id != Guid.Empty)
            {
                if (name.Length == 0)
                {
                    multiview.ActiveViewIndex = ViewNum.HeaderItemView;
                    byte[] value = (byte[])report.ItemTable[id];
                    FieldItem fieldItem = (FieldItem)XmlConverter.ConvertTo(value);

                    TextBox textboxCaption = (TextBox)container.FindControl(TEXTBOX_FIELD_CAPTION);
                    textboxCaption.Text = fieldItem.Caption;

                    DropDownList dropdownlistCaptionAlignment = (DropDownList)container.FindControl(DROPDOWNLIST_FIELD_CAPTION_ALIGNMENT);
                    dropdownlistCaptionAlignment.SelectedValue = fieldItem.CaptionAlignment.ToString();
                    DropDownList dropdownlistColumnAlignment = (DropDownList)container.FindControl(DROPDOWNLIST_FIELD_COLUMN_ALIGNMENT);
                    dropdownlistColumnAlignment.SelectedValue = fieldItem.ColumnAlignment.ToString();
                    TextBox textboxWidth = (TextBox)container.FindControl(TEXTBOX_FIELD_WIDTH);
                    textboxWidth.Text = fieldItem.Width.ToString();

                    //Format
                    TextBox textboxFormat = (TextBox)container.FindControl(TEXTBOX_FIELD_FORMAT);
                    textboxFormat.Text = fieldItem.Format;

                    CheckBox checkboxNewLine = (CheckBox)container.FindControl(CHECKBOX_FIELD_NEWLINE);
                    checkboxNewLine.Checked = fieldItem.NewLine;
                    TextBox textboxNewLinePosition = (TextBox)container.FindControl(TEXTBOX_FIELD_NEWLINE_POSITION);
                    textboxNewLinePosition.Text = fieldItem.NewLinePostion.ToString();

                    TextBox textboxCells = (TextBox)container.FindControl(TEXTBOX_FIELD_CELLS);
                    textboxCells.Text = fieldItem.Cells.ToString();

                    DropDownList dropdownlistOrderBy = (DropDownList)container.FindControl(DROPDOWNLIST_FIELD_ORDERBY);
                    dropdownlistOrderBy.SelectedValue = fieldItem.Order.ToString();

                    CheckBox checkboxFieldSuppressIfDuplicated = (CheckBox)container.FindControl(CHECKBOX_FIELD_SUPPRESSIFDUPLICATED);
                    checkboxFieldSuppressIfDuplicated.Checked = fieldItem.SuppressIfDuplicated;

                    DropDownList dropdownlistGroupType = (DropDownList)container.FindControl(DROPDOWNLIST_FIELD_GROUP_TYPE);
                    dropdownlistGroupType.SelectedValue = fieldItem.Group.ToString();
                    TextBox textboxGroupTotalCaption = (TextBox)container.FindControl(TEXTBOX_FIELD_GROUP_TOTAL_CAPTION);
                    textboxGroupTotalCaption.Text = fieldItem.GroupTotalCaption;

                    DropDownList dropdownlistSumType = (DropDownList)container.FindControl(DROPDOWNLIST_FIELD_SUM_TYPE);
                    dropdownlistSumType.SelectedValue = fieldItem.Sum.ToString();
                    TextBox textboxTotalCaption = (TextBox)container.FindControl(TEXTBOX_FIELD_TOTAL_CAPTION);
                    textboxTotalCaption.Text = fieldItem.TotalCaption;

                }
            }
            else
            {
                multiview.ActiveViewIndex = -1;
            }
        }

        private void SaveFieldItem(string name, Guid id)
        {
            if (name.Length == 0)
            {
                byte[] value = (byte[])report.ItemTable[id];
                FieldItem fieldItem = (FieldItem)XmlConverter.ConvertTo(value);

                Control container = this.report.MultiView;

                TextBox textboxCaption = (TextBox)container.FindControl(TEXTBOX_FIELD_CAPTION);
                fieldItem.Caption = textboxCaption.Text;


                DropDownList dropdownlistCaptionAlignment = (DropDownList)container.FindControl(DROPDOWNLIST_FIELD_CAPTION_ALIGNMENT);
                fieldItem.CaptionAlignment = (System.Windows.Forms.HorizontalAlignment)Enum.Parse(typeof(System.Windows.Forms.HorizontalAlignment), dropdownlistCaptionAlignment.SelectedValue);
                DropDownList dropdownlistColumnAlignment = (DropDownList)container.FindControl(DROPDOWNLIST_FIELD_COLUMN_ALIGNMENT);
                fieldItem.ColumnAlignment = (System.Windows.Forms.HorizontalAlignment)Enum.Parse(typeof(System.Windows.Forms.HorizontalAlignment), dropdownlistColumnAlignment.SelectedValue);
                TextBox textboxWidth = (TextBox)container.FindControl(TEXTBOX_FIELD_WIDTH);
                fieldItem.Width = Convert.ToInt32(textboxWidth.Text);

                //Format
                TextBox textboxFormat = (TextBox)container.FindControl(TEXTBOX_FIELD_FORMAT);
                fieldItem.Format = textboxFormat.Text;

                CheckBox checkboxNewLine = (CheckBox)container.FindControl(CHECKBOX_FIELD_NEWLINE);
                fieldItem.NewLine = checkboxNewLine.Checked;
                TextBox textboxNewLinePosition = (TextBox)container.FindControl(TEXTBOX_FIELD_NEWLINE_POSITION);
                fieldItem.NewLinePostion = Convert.ToInt32(textboxNewLinePosition.Text);

                TextBox textboxCells = (TextBox)container.FindControl(TEXTBOX_FIELD_CELLS);
                fieldItem.Cells = Convert.ToInt32(textboxCells.Text);

                DropDownList dropdownlistOrderBy = (DropDownList)container.FindControl(DROPDOWNLIST_FIELD_ORDERBY);
                fieldItem.Order = (FieldItem.OrderType)Enum.Parse(typeof(FieldItem.OrderType), dropdownlistOrderBy.SelectedValue);

                CheckBox checkboxFieldSuppressIfDuplicated = (CheckBox)container.FindControl(CHECKBOX_FIELD_SUPPRESSIFDUPLICATED);
                fieldItem.SuppressIfDuplicated = checkboxFieldSuppressIfDuplicated.Checked;


                DropDownList dropdownlistGroupType = (DropDownList)container.FindControl(DROPDOWNLIST_FIELD_GROUP_TYPE);
                fieldItem.Group = (FieldItem.GroupType)Enum.Parse(typeof(FieldItem.GroupType), dropdownlistGroupType.SelectedValue);
                TextBox textboxGroupTotalCaption = (TextBox)container.FindControl(TEXTBOX_FIELD_GROUP_TOTAL_CAPTION);
                fieldItem.GroupTotalCaption = textboxGroupTotalCaption.Text;

                DropDownList dropdownlistSumType = (DropDownList)container.FindControl(DROPDOWNLIST_FIELD_SUM_TYPE);
                fieldItem.Sum = (FieldItem.SumType)Enum.Parse(typeof(FieldItem.SumType), dropdownlistSumType.SelectedValue);
                TextBox textboxTotalCaption = (TextBox)container.FindControl(TEXTBOX_FIELD_TOTAL_CAPTION);
                fieldItem.TotalCaption = textboxTotalCaption.Text;



                report.ItemTable[id] = XmlConverter.ConvertFrom(fieldItem);
            }
        }

        void buttonTemplateOK_Click(object sender, EventArgs e)
        {
            this.LoadTemplate();
            report.TransImages();

            InitialView(this.report);
            HideModelPopup(ModelPopupView.TemplateView);
        }

        private void DeleteTemplate()
        {
            Control container = this.report.MultiView;
            ListBox lbxTemplate = (ListBox)container.FindControl(LISTBOX_TEMPLATES);

            if (lbxTemplate.SelectedValue != null)
            {
                string filename = lbxTemplate.SelectedValue;
                report.dbGateway.DeleteTemplate(filename);
                lbxTemplate.Items.Remove(filename);
            }
        }

        private void LoadTemplate()
        {
            Control container = this.report.MultiView;
            ListBox lbxTemplate = (ListBox)container.FindControl(LISTBOX_TEMPLATES);
            string filename = lbxTemplate.SelectedValue;
            report.dbGateway.LoadTemplate(filename);
        }

        

        void buttonTemplateDelete_Click(object sender, EventArgs e)
        {
            ShowModelPopup(ModelPopupView.TemplateView);
            this.DeleteTemplate();
        }

        void buttonTemplateCancel_Click(object sender, EventArgs e)
        {

        }

        void buttonFontOK_Click(object sender, EventArgs e)
        {
            Control container = this.report.MultiView;
            DropDownList dropdownlistFont = (DropDownList)container.FindControl(DROPDOWNLIST_FONT);
            TextBox textboxSize = (TextBox)container.FindControl(TEXTBOX_FONT_SIZE);

            FontStyle style = FontStyle.Regular;
            //bold
            CheckBox checkboxBold = (CheckBox)container.FindControl(CHECKBOX_FONT_BOLD);
            if (checkboxBold.Checked)
            {
                style |= FontStyle.Bold;
            }
            //italic
            CheckBox checkboxItalic = (CheckBox)container.FindControl(CHECKBOX_FONT_ITALIC);
            if (checkboxItalic.Checked)
            {
                style |= FontStyle.Italic;
            }
            //underline
            CheckBox checkboxUnderline = (CheckBox)container.FindControl(CHECKBOX_FONT_UNDERLINE);
            if (checkboxUnderline.Checked)
            {
                style |= FontStyle.Underline;
            }

            //strikeout
            CheckBox checkboxStrikeout = (CheckBox)container.FindControl(CHECKBOX_FONT_STRIKEOUT);
            if (checkboxStrikeout.Checked)
            {
                style |= FontStyle.Strikeout;
            }
            report.FontTable[report.FontButtonID] = new Font(dropdownlistFont.SelectedValue, float.Parse(textboxSize.Text), style);

            string name = String.Empty;
            name = report.FontButtonID.Replace("button", "label");

            Label labelFontSample = (Label)container.FindControl(name);
            labelFontSample.Font.CopyFrom((FontInfo)WebFontConverter.ConvertFrom((Font)report.FontTable[report.FontButtonID]));

            HideModelPopup(ModelPopupView.FontView);
        }

        void buttonFontCancel_Click(object sender, EventArgs e)
        {

        }

        void dropdownlistExportFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control container = this.report.MultiView;
            DropDownList dropdownlistExportFormat = sender as DropDownList;

            RadioButtonList radiobuttonlistPageSize = (RadioButtonList)container.FindControl(RADIOBUTTONLIST_PAGE_SIZE);
            RadioButtonList radiobuttonlistPrintOrientation = (RadioButtonList)container.FindControl(RADIOBUTTONLIST_PRINT_ORIENTATION);
            Panel panelPageMargin = (Panel)container.FindControl(PANEL_PAGE_MARGIN);


            if (dropdownlistExportFormat.Text == ReportFormat.ExportType.Pdf.ToString())
            {
                radiobuttonlistPageSize.Enabled = true;
                //radiobuttonlistPrintOrientation.Enabled = true;
                panelPageMargin.Enabled = true;
            }
            else
            {
                radiobuttonlistPageSize.Enabled = false;
                //radiobuttonlistPrintOrientation.Enabled = false;
                panelPageMargin.Enabled = false;
            }
        }

        void buttonTemplateSaveAsOK_Click(object sender, EventArgs e)
        {
            string message = String.Empty;
            Control container = this.report.MultiView;
            Save();
            TextBox tbFileName = (TextBox)container.FindControl(TEXTBOX_TEMPLATE_SAVE_AS_FILENAME);

            ExecutionResult exeRes = report.dbGateway.SaveTemplate(tbFileName.Text.Trim());

            if (exeRes.Status)
            {
                message = MessageInfo.SaveSuccess;

                AspNetScriptsProvider.ShowMessage(this.report, message);

                HideModelPopup(ModelPopupView.SaveAsView);
            }
            else
            {
                message = exeRes.Message;

                AspNetScriptsProvider.ShowMessage(this.report, message);
            }
        }

        #region Add Pictures
        void buttonAddPicture_Click(object sender, EventArgs e)
        {
            report.LastViewIndex = this.report.MultiView.ActiveViewIndex;

            ShowModelPopup(ModelPopupView.PictureView);

            HideFileUpload();
        }

        void buttonPictureAdd_Click(object sender, EventArgs e)
        {
            this.report.PictureChangeMode = ImageMode.Add.ToString();
            ShowFileUpload();
            ShowModelPopup(ModelPopupView.PictureView);
        }

        private void AddPicture(EventArgs e)
        {
            Control container = this.report.MultiView;
            string typeName = "ImageItem";
            Type type = this.GetType().Assembly.GetType(string.Format(ComponentInfo.NameSpace + ".{0}", typeName));
            ImageItem item = (ImageItem)Activator.CreateInstance(type);
            Guid id = Guid.NewGuid();
            report.ImageItemTable.Add(id, XmlConverter.ConvertFrom(item));
            ListBox lbxPictures = (ListBox)container.FindControl(LISTBOX_PICTURES);
            lbxPictures.Items.Add(new ListItem(typeName, id.ToString()));
            lbxPictures.SelectedValue = id.ToString();
            listboxPictures_SelectedIndexChanged(lbxPictures, e);//触发事件
        }

        void listboxPictures_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowModelPopup(ModelPopupView.PictureView);

            //保存上一笔
            ListBox listboxPictures = sender as ListBox;
            string name = listboxPictures.ID;
            if (report.ImageItemIndexTable.Contains(name) && !report.ImageItemIndexTable[name].Equals(Guid.Empty))
            {
                //保存
                Guid id = (Guid)report.ImageItemIndexTable[name];
                SaveImageItem(name, id);
            }
            //设置当前笔
            if (!string.IsNullOrEmpty(listboxPictures.SelectedValue))
            {
                Guid id = new Guid(listboxPictures.SelectedValue);
                report.ImageItemIndexTable[name] = id;
                LoadImageItem(name, id);
            }
            else
            {
                LoadImageItem(name, Guid.Empty);
            }
        }


        private void SaveImageItem(string name, Guid id)
        {
            //string path = this.Page.MapPath(this.Page.Request.Path);
            //string strPath = path.Substring(0, path.LastIndexOf('\\') + 1);
            byte[] value = (byte[])report.ImageItemTable[id];
            ImageItem imageItem = (ImageItem)XmlConverter.ConvertTo(value);
            Control container = this.report.MultiView;
            System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image)container.FindControl(IMAGE_PICTURE);

            imageItem.ImageUrl = image.ImageUrl;
            //imageItem.ImagePath = strPath + image.ImageUrl.Substring(image.ImageUrl.LastIndexOf('/') + 1);
            //if (imageItem.ImagePath.Substring(imageItem.ImagePath.Length - 1) != "\\")
            //{
            //    imageItem.Image = System.Drawing.Image.FromFile(imageItem.ImagePath);
            //}
            report.ImageItemTable[id] = XmlConverter.ConvertFrom(imageItem);
        }

        private void LoadImageItem(string name, Guid id)
        {
            Control container = this.report.MultiView;

            if (id != Guid.Empty)
            {
                byte[] value = (byte[])report.ImageItemTable[id];
                ImageItem imageItem = (ImageItem)XmlConverter.ConvertTo(value);
                System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image)container.FindControl(IMAGE_PICTURE);
                image.ImageUrl = imageItem.ImageUrl;
            }
        }

        private void SaveImageItem(ImageItemCollection items, string name)
        {
            Control container = this.report.MultiView;
            if (report.ImageItemIndexTable.Contains(name) && !report.ImageItemIndexTable[name].Equals(Guid.Empty))
            {
                Guid id = (Guid)report.ImageItemIndexTable[name];
                SaveImageItem(name, id);
            }
            items.Clear();
            ListBox listboxItem = (ListBox)container.FindControl(LISTBOX_PICTURES);
            foreach (ListItem li in listboxItem.Items)
            {
                Guid id = new Guid(li.Value);
                byte[] value = (byte[])report.ImageItemTable[id];
                ImageItem imageItem = (ImageItem)XmlConverter.ConvertTo(value);
                items.Add(imageItem);
            }
        }

        void buttonPictureRemove_Click(object sender, EventArgs e)
        {
            string name = LISTBOX_PICTURES;
            Control container = this.report.MultiView;
            ListBox listboxItem = (ListBox)container.FindControl(LISTBOX_PICTURES);
            if (!string.IsNullOrEmpty(listboxItem.SelectedValue))
            {
                Guid id = new Guid(listboxItem.SelectedValue);
                report.ImageItemTable.Remove(id);
                listboxItem.Items.Remove(listboxItem.SelectedItem);
                report.ImageItemIndexTable[name] = Guid.Empty;//上一笔清空

                System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image)container.FindControl(IMAGE_PICTURE);
                image.ImageUrl = String.Empty;

                listboxPictures_SelectedIndexChanged(listboxItem, e);//触发事件
            }

            ShowModelPopup(ModelPopupView.PictureView);
        }

        void buttonPictureChange_Click(object sender, EventArgs e)
        {
            this.report.PictureChangeMode = ImageMode.Change.ToString();
            ShowModelPopup(ModelPopupView.PictureView);
            ShowFileUpload();
        }

        void buttonPictureSelect_Click(object sender, EventArgs e)
        {
            Control container = this.report.MultiView;
            string name = String.Empty;

            ListBox listboxPictures = (ListBox)container.FindControl(LISTBOX_PICTURES);
            if (!string.IsNullOrEmpty(listboxPictures.SelectedValue))
            {
                if (report.LastViewIndex == ViewNum.HeaderItemView)
                {
                    name = HEADER;
                }
                else if (report.LastViewIndex == ViewNum.FooterItemView)
                {
                    name = FOOTER;
                }

                DropDownList dropdownlistContent = (DropDownList)container.FindControl(string.Format("{0}{1}", DROPDOWNLIST_ITEM_CONTENT, name));
                dropdownlistContent.Items.Clear();
                dropdownlistContent.Items.Add(new ListItem(listboxPictures.SelectedIndex.ToString()));

                ListBox listboxItem = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_ITEM, name));
                listboxItem.SelectedItem.Text = string.Format("{0}({1})", ERptMultiLanguage.GetLanValue("ImageItem"), listboxPictures.SelectedIndex);

                this.report.MultiView.ActiveViewIndex = report.LastViewIndex;
            }
        }

        void buttonFileUploadOK_Click(object sender, EventArgs e)
        {

            ShowModelPopup(ModelPopupView.PictureView);

            Control container = this.report.MultiView;
            Anthem.FileUpload fileUpload = (Anthem.FileUpload)container.FindControl(FILEUPLOAD_PICTURE);
            //string path = this.Page.MapPath(this.Page.Request.Path);
            //string strUrl = path.Substring(0, path.LastIndexOf('\\') + 1) + fileUpload.FileName;

            if (fileUpload.HasFile)
            {
                if (CheckUploadFileType(fileUpload))
                {
                    string fileName = fileUpload.FileName;

                    string path = this.report.Page.MapPath("Image");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    fileUpload.SaveAs(string.Format("{0}\\{1}", path, fileName));

                    //fileUpload.SaveAs(strUrl);

                    //this.ImageFilePath = this.Page.Request.AppRelativeCurrentExecutionFilePath.Substring(0, this.Page.Request.AppRelativeCurrentExecutionFilePath.LastIndexOf('/') + 1) + fileName;

                    //此處順序不能顛倒

                    if (this.report.PictureChangeMode == ImageMode.Add.ToString())
                    {
                        this.AddPicture(e);
                    }

                    System.Web.UI.WebControls.Image image = (System.Web.UI.WebControls.Image)container.FindControl(IMAGE_PICTURE);
                    image.ImageUrl = string.Format("Image\\{0}", fileName);

                    HideFileUpload();
                }
                else
                {
                    string script = "alert('" + MessageInfo.FileTypeNotSupport + "')";
                    Anthem.Manager.RegisterClientScriptBlock(this.report.Page.GetType(), new Guid().ToString(), script, true);
                }
            }
            //else
            //{
            //    this.ImageFilePath = String.Empty;
            //}

        }

        private bool CheckUploadFileType(FileUpload fileUpload)
        {
            string fileName = fileUpload.FileName;
            string extension = System.IO.Path.GetExtension(fileName).ToLower();
            bool exeRes = false;

            switch (extension)
            {
                case ".jpg":

                case ".jpeg":

                case ".gif":

                case "png":
                    exeRes = true;
                    break;
            }

            return exeRes;
        }

        void buttonFileUploadCancel_Click(object sender, EventArgs e)
        {
            ShowModelPopup(ModelPopupView.PictureView);
            HideFileUpload();
        }
        #endregion


        #endregion

        #region InitialView
        public void InitialView(WebEasilyReport report)
        {
            //initial fonttable
            report.FontTable = new Hashtable();
            report.ItemTable = new Hashtable();
            this.report.ItemIndexTable = new Hashtable();
            report.ImageItemTable = new Hashtable();
            report.ImageItemIndexTable = new Hashtable();

            //intial header
            #region header
            InitialItemView(report.HeaderItems, report.HeaderFont, HEADER);
            #endregion

            //initial field
            #region field
            //InitialFieldView(report, report.FieldFont);

            ArrayList list = new ArrayList();

            int count = report.DataSourceCount - report.FieldItems.Count;
            for (int i = 0; i < count; i++)
            {
                report.FieldItems.Add(new DataSourceItem());
            }

            foreach (DataSourceItem item in report.FieldItems)
            {
                list.Add(XmlConverter.ConvertFrom(item));
            }
            report.LastIndex = -1;
            report.TempFieldItems = list;

            Control container = this.report.MultiView;
            DropDownList dropdownlistField = (DropDownList)container.FindControl(DROPDOWNLIST_FIELD);
            dropdownlistField.Items.Clear();
            for (int i = 0; i < report.DataSourceCount; i++)
            {
                dropdownlistField.Items.Add(i.ToString());
            }
            dropdownlistField_SelectedIndexChanged(dropdownlistField, new EventArgs());

            #endregion

            //initial footer
            #region footer
            InitialItemView(report.FooterItems, report.FooterFont, FOOTER);
            #endregion
            //initial setting
            #region settting
            InitialSettingView(report);
            #endregion

            //initial database
            #region template
            //InitialDataBaseView();
            #endregion

            InitialFileUploadView();

            InitialPictureView();
        }

        private void InitialItemView(ReportItemCollection items, Font font, string name)
        {
            string fontButtonID = string.Format("{0}{1}", BUTTON_ITEM_FONT, name);
            report.FontTable.Add(fontButtonID, font);
            Control container = this.report.MultiView;
            ListBox listboxItem = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_ITEM, name));
            listboxItem.Items.Clear();
            foreach (ReportItem item in items)
            {
                Guid key = Guid.NewGuid(); ;
                report.ItemTable.Add(key, XmlConverter.ConvertFrom(item));
                listboxItem.Items.Add(new ListItem(item.ToString(), key.ToString()));
            }

            Label labelFontSample = (Label)container.FindControl(string.Format("{0}{1}", LABEL_ITEM_FONT_SAMPLE, name));
            labelFontSample.Font.CopyFrom((FontInfo)WebFontConverter.ConvertFrom(font));
        }

        private void InitialFieldView(WebEasilyReport report, Font font, int dataSourceIndex)
        {
            string fontButtonID = BUTTON_FIELD_FONT;
            report.FontTable.Add(fontButtonID, font);
            Control container = this.report.MultiView;
            ListBox listboxItem = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_SELECTED, string.Empty));
            listboxItem.Items.Clear();
            //ListBox listboxItemGroup = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_SELECTED, GROUP));
            //listboxItemGroup.Items.Clear();
            ListBox listboxItemAll = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_ALL, string.Empty));
            listboxItemAll.Items.Clear();
            //ListBox listboxItemAllGroup = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_ALL, GROUP));
            //listboxItemAllGroup.Items.Clear();
            byte[] value = (byte[])report.TempFieldItems[dataSourceIndex];
            DataSourceItem fieldItem = (DataSourceItem)XmlConverter.ConvertTo(value);

            //反序列化出来的DataSourceItem没有Collection属性
            //  fieldItem.SetCollection(report.FieldItems);
            WebDataSource wds = null;
            if (dataSourceIndex < report.DataSource.Count)
            {
                string id = report.DataSource[dataSourceIndex].DataSourceID;
                wds = report.Parent.FindControl(id) as WebDataSource;
                if (wds == null)
                {
                    wds = report.Page.FindControl(id) as WebDataSource;
                }
            }

            if (wds != null)
            {
                DataView view = wds.View;
                if (view != null)
                {
                    DataTable table = view.Table;
                    //DD
                    DataSet ds = DBUtils.GetDataDictionary(wds, false);

                    foreach (FieldItem field in fieldItem.Fields)
                    {
                        string columnName = field.ColumnName;
                        DataRow[] drDD = ds.Tables[0].Select(string.Format("FIELD_NAME='{0}'", columnName.Replace("'", "''")));
                        string caption = drDD.Length > 0 ? drDD[0]["CAPTION"].ToString() : columnName;
                        Guid key = Guid.NewGuid(); ;
                        report.ItemTable.Add(key, XmlConverter.ConvertFrom(field));
                        listboxItem.Items.Add(new ListItem(caption, key.ToString()));
                    }
                    foreach (DataColumn column in table.Columns)
                    {
                        FieldItem field = fieldItem.Fields[column.ColumnName];
                        string columnName = column.ColumnName;
                        DataRow[] drDD = ds.Tables[0].Select(string.Format("FIELD_NAME='{0}'", columnName.Replace("'", "''")));
                        string caption = drDD.Length > 0 ? drDD[0]["CAPTION"].ToString() : columnName;
                        if (field == null)
                        {
                            listboxItemAll.Items.Add(new ListItem(caption, columnName));
                        }

                    }
                }
            }

            Label labelFontSample = (Label)container.FindControl(LABEL_FIELD_FONT_SAMPLE);
            labelFontSample.Font.CopyFrom((FontInfo)WebFontConverter.ConvertFrom(report.FieldFont));

            CheckBox checkboxFieldGroupTotal = (CheckBox)container.FindControl(CHECKBOX_FIELD_GROUP_TOTAL);
            checkboxFieldGroupTotal.Checked = fieldItem.GroupTotal;

            DropDownList dropdownlistFieldGroupGap = (DropDownList)container.FindControl(DROPDOWNLIST_FIELD_GROUP_GAP);
            dropdownlistFieldGroupGap.SelectedValue = fieldItem.GroupGap.ToString();

            DropDownList dropdownlistCaptionStyle = (DropDownList)container.FindControl(DROPDOWNLIST_CAPTION_STYLE);
            dropdownlistCaptionStyle.SelectedValue = fieldItem.CaptionStyle.ToString();
        }

        private void InitialSendMailView(WebEasilyReport report)
        {
            Control container = report.FindControl(MULTIVIEW_OUTPUTVIEW);
            TextBox textboxMailTitle = (TextBox)container.FindControl(TEXTBOX_SENDMAIL_TITLE);
            textboxMailTitle.Text = report.MailSetting.Subject;

            TextBox textboxMailTo = (TextBox)container.FindControl(TEXTBOX_SENDMAIL_TO);
            textboxMailTo.Text = report.MailSetting.MailTo;

            TextBox textboxMailFrom = (TextBox)container.FindControl(TEXTBOX_SENDMAIL_FROM);
            textboxMailFrom.Text = report.MailSetting.MailFrom;

            TextBox textboxMailPassword = (TextBox)container.FindControl(TEXTBOX_SENDMAIL_PASSWORD);
            textboxMailPassword.Text = report.MailSetting.Password;

            TextBox textboxMailServer = (TextBox)container.FindControl(TEXTBOX_SENDMAIL_SERVER);
            textboxMailServer.Text = report.MailSetting.Host;
        }

        private void InitialSettingView(WebEasilyReport report)
        {
            Control container = this.report.MultiView;

            CheckBox cbxColumnGrid = (CheckBox)container.FindControl(CHECKBOX_COLUMNGRID);
            cbxColumnGrid.Checked = report.Format.ColumnGridLine;

            CheckBox cbxInnerColumnGrid = (CheckBox)container.FindControl(CHECKBOX_INNER_COLUMNGRID);
            cbxInnerColumnGrid.Checked = report.Format.ColumnInsideGridLine;

            CheckBox cbxRowGrid = (CheckBox)container.FindControl(CHECKBOX_ROWGRID);
            cbxRowGrid.Checked = report.Format.RowGridLine;

            CheckBox checkboxHeaderRepeater = (CheckBox)container.FindControl(CHECKBOX_HEADER_REPEAT);
            checkboxHeaderRepeater.Checked = report.HeaderRepeat;

            RadioButtonList radiobuttonlistPageSize = (RadioButtonList)container.FindControl(RADIOBUTTONLIST_PAGE_SIZE);
            radiobuttonlistPageSize.SelectedValue = report.Format.PageSize.ToString();

            RadioButtonList radiobuttonlistPrintOrientation = (RadioButtonList)container.FindControl(RADIOBUTTONLIST_PRINT_ORIENTATION);
            radiobuttonlistPrintOrientation.SelectedValue = report.Format.Orientation.ToString();

            TextBox textboxPageRecords = (TextBox)container.FindControl(TEXTBOX_PAGE_RECORDS);
            textboxPageRecords.Text = report.Format.PageRecords.ToString();

            DropDownList dropdownlistExportFormat = (DropDownList)container.FindControl(DROPDOWNLIST_EXPORT_FORMAT);
            dropdownlistExportFormat.SelectedValue = report.Format.ExportFormat.ToString();

            Panel panelPageMargin = (Panel)container.FindControl(PANEL_PAGE_MARGIN);

            if (dropdownlistExportFormat.SelectedValue == ReportFormat.ExportType.Pdf.ToString())
            {
                radiobuttonlistPageSize.Enabled = true;
                //radiobuttonlistPrintOrientation.Enabled = true;
                panelPageMargin.Enabled = true;
            }
            else
            {
                radiobuttonlistPageSize.Enabled = false;
                //radiobuttonlistPrintOrientation.Enabled = false;
                panelPageMargin.Enabled = false;
            }

            TextBox textboxMailTitle = (TextBox)container.FindControl(TEXTBOX_MAIL_TITLE);
            textboxMailTitle.Text = report.MailSetting.Subject;

            TextBox textboxMailTo = (TextBox)container.FindControl(TEXTBOX_MAIL_TO);
            textboxMailTo.Text = report.MailSetting.MailTo;

            TextBox textboxMailFrom = (TextBox)container.FindControl(TEXTBOX_MAIL_FROM);
            textboxMailFrom.Text = report.MailSetting.MailFrom;

            TextBox textboxMailPassword = (TextBox)container.FindControl(TEXTBOX_MAIL_PASSWORD);
            textboxMailPassword.Text = report.MailSetting.Password;

            TextBox textboxMailServer = (TextBox)container.FindControl(TEXTBOX_MAIL_SERVER);
            textboxMailServer.Text = report.MailSetting.Host;

            TextBox textboxLeftMargin = (TextBox)container.FindControl(TEXTBOX_LEFTMARGIN);
            textboxLeftMargin.Text = report.Format.MarginLeft.ToString();

            TextBox textboxRightMargin = (TextBox)container.FindControl(TEXTBOX_RIGHTMARGIN);
            textboxRightMargin.Text = report.Format.MarginRight.ToString();

            TextBox textboxTopMargin = (TextBox)container.FindControl(TEXTBOX_TOPMARGIN);
            textboxTopMargin.Text = report.Format.MarginTop.ToString();

            TextBox textboxBottomMargin = (TextBox)container.FindControl(TEXTBOX_BOTTOMMARGIN);
            textboxBottomMargin.Text = report.Format.MarginBottom.ToString();

            CheckBox checkboxSendMail = (CheckBox)container.FindControl(CHECKBOX_SEND_MAIL);
            if (this.report.OutputMode == OutputModeType.Email)
            {
                checkboxSendMail.Checked = true;
            }
            else
            {
                checkboxSendMail.Checked = false;
            }
        }

        private void InitialDataBaseView()
        {
            //WebDataSource wds = new WebDataSource();
            //DataSet ds = new DataSet();

            //Control container = this.report.MultiView;

            Control container = report.FindControl(PANEL_DATABASE);
            ListBox lbxTemplate = (ListBox)container.FindControl(LISTBOX_TEMPLATES);

            //if (CliUtils.fLoginDB == String.Empty)
            //{
            //    throw new Exception("75FF57F7-7AC0-43c8-9454-C92B4A2723BB");
            //}
            //else
            //{
            //ds.Tables.Add(dbGateway.Copy());
            //wds.InnerDataSet = ds;
            //lbxTemplate.DataSource = wds;
            //lbxTemplate.DataMember = wds.InnerDataSet.Tables[0].TableName;
            //lbxTemplate.DataTextField = SysRptDB.FileName;
            //lbxTemplate.DataValueField = SysRptDB.FileName;
            //lbxTemplate.DataBind();
            //}
            DictionaryEntry[] filenames = this.report.dbGateway.GetTemplates(false);
            lbxTemplate.Items.Clear();
            foreach (DictionaryEntry filename in filenames)
            {
                lbxTemplate.Items.Add(filename.Value.ToString());
            }
        }

        private void InitialFontView(Font font)
        {
            if (font == null)
            {
                return;
            }
            Control container = this.report.MultiView;
            //name
            DropDownList dropdownlistFont = (DropDownList)container.FindControl(DROPDOWNLIST_FONT);
            dropdownlistFont.SelectedValue = font.Name;

            //size 
            TextBox textboxFontSize = (TextBox)container.FindControl(TEXTBOX_FONT_SIZE);
            textboxFontSize.Text = font.Size.ToString();

            //bold
            CheckBox checkboxFontBold = (CheckBox)container.FindControl(CHECKBOX_FONT_BOLD);
            checkboxFontBold.Checked = font.Bold;
            //italic
            CheckBox checkboxFontItalic = (CheckBox)container.FindControl(CHECKBOX_FONT_ITALIC);
            checkboxFontItalic.Checked = font.Italic;
            //underline
            CheckBox checkboxFontUnderline = (CheckBox)container.FindControl(CHECKBOX_FONT_UNDERLINE);
            checkboxFontUnderline.Checked = font.Underline;

            //strikeout
            CheckBox checkboxFontStrikeout = (CheckBox)container.FindControl(CHECKBOX_FONT_STRIKEOUT);
            checkboxFontStrikeout.Checked = font.Strikeout;
        }

        private void InitialPictureView()
        {
            Control container = this.report.MultiView;
            ListBox listboxItem = (ListBox)container.FindControl(LISTBOX_PICTURES);
            listboxItem.Items.Clear();
            foreach (ImageItem item in this.report.Images)
            {
                Guid key = Guid.NewGuid(); ;
                report.ImageItemTable.Add(key, XmlConverter.ConvertFrom(item));
                listboxItem.Items.Add(new ListItem(item.GetType().Name, key.ToString()));
            }
        }

        private void InitialFileUploadView()
        {
            HideFileUpload();

            //Control control = this.GetUpdatePanel();
            //if (control != null)
            //{
            //    Control container = this.report.MultiView;
            //    Button btUpload = (Button)container.FindControl(BUTTON_FILEUPLOAD_OK);

            //    if (btUpload != null)
            //    {
            //        UpdatePanel updatePanel = (UpdatePanel)control;
            //        //PostBackTrigger tigger = new PostBackTrigger();
            //        //tigger.ControlID = btUpload.ID;

            //        //foreach (UpdatePanelControlTrigger upTigger in updatePanel.Triggers)
            //        //{
            //        //    if (upTigger.ControlID == tigger.ControlID)
            //        //    {
            //        //        return;
            //        //    }
            //        //}
            //        ScriptManager scriptManager = (ScriptManager)updatePanel.Parent.FindControl("AjaxScriptManager1");
            //        scriptManager.RegisterPostBackControl(btUpload);
            //        //updatePanel.Triggers.Add(tigger);
            //    }
            //}


            //Control control = this.GetUpdatePanel();
            //if (control != null)
            //{
            //    UpdatePanel updatePanel = (UpdatePanel)control;
            //    PostBackTrigger tigger = new PostBackTrigger();
            //    tigger.ControlID = BUTTON_FILEUPLOAD_OK;

            //    foreach (UpdatePanelControlTrigger upTigger in updatePanel.Triggers)
            //    {
            //        if (upTigger.ControlID == tigger.ControlID)
            //        {
            //            return;
            //        }
            //    }

            //    try
            //    {
            //        updatePanel.Triggers.Add(tigger);
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
        }
        #endregion

        #region SaveView
        private void Save()
        {
            SaveItem(this.report.HeaderItems, HEADER);
            if (report.LastIndex != -1)
            {
                SaveField(report.LastIndex);
            }

            this.report.FieldItems.Clear();
            for (int i = 0; i < report.TempFieldItems.Count; i++)
            {
                byte[] value = (byte[])report.TempFieldItems[i];
                this.report.FieldItems.Add((DataSourceItem)XmlConverter.ConvertTo(value));
            }

            SaveItem(this.report.FooterItems, FOOTER);
            SaveSetting(this.report);
            SaveFont(this.report);
            SaveImageItem(this.report.Images, LISTBOX_PICTURES);
        }

        private void SaveItem(ReportItemCollection items, string name)
        {
            Control container = this.report.MultiView;
            if (this.report.ItemIndexTable.Contains(name) && !this.report.ItemIndexTable[name].Equals(Guid.Empty))
            {
                Guid id = (Guid)this.report.ItemIndexTable[name];
                SaveItem(name, id);
            }
            items.Clear();
            ListBox listboxItem = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_ITEM, name));
            foreach (ListItem li in listboxItem.Items)
            {
                Guid id = new Guid(li.Value);
                byte[] value = (byte[])report.ItemTable[id];
                ReportItem reportItem = (ReportItem)XmlConverter.ConvertTo(value);
                items.Add(reportItem);
            }
        }

        private void SaveField(int dataSourceIndex)
        {
            Control container = this.report.MultiView;
            byte[] datasourcevalue = (byte[])report.TempFieldItems[dataSourceIndex];
            DataSourceItem fieldItem = (DataSourceItem)XmlConverter.ConvertTo(datasourcevalue);
            fieldItem.Fields.Clear();
            ListBox listboxItem = (ListBox)container.FindControl(string.Format("{0}{1}", LISTBOX_FIELD_SELECTED, string.Empty));
            string name = listboxItem.ID.Replace(LISTBOX_FIELD_SELECTED, string.Empty);

            if (this.report.ItemIndexTable.Contains(name) && !this.report.ItemIndexTable[name].Equals(Guid.Empty))
            {
                //保存上一笔
                Guid id = (Guid)this.report.ItemIndexTable[name];
                SaveFieldItem(name, id);
            }

            foreach (ListItem li in listboxItem.Items)
            {
                Guid id = new Guid(li.Value);
                byte[] value = (byte[])report.ItemTable[id];
                FieldItem field = (FieldItem)XmlConverter.ConvertTo(value);
                fieldItem.Fields.Add(field);
            }

            CheckBox checkboxFieldGroupTotal = (CheckBox)container.FindControl(CHECKBOX_FIELD_GROUP_TOTAL);
            fieldItem.GroupTotal = checkboxFieldGroupTotal.Checked;
            DropDownList dropdownlistFieldGroupGap = (DropDownList)container.FindControl(DROPDOWNLIST_FIELD_GROUP_GAP);
            fieldItem.GroupGap = (DataSourceItem.GroupGapType)Enum.Parse(typeof(DataSourceItem.GroupGapType), dropdownlistFieldGroupGap.SelectedValue);
            DropDownList dropdownlistCaptionStyle = (DropDownList)container.FindControl(DROPDOWNLIST_CAPTION_STYLE);
            fieldItem.CaptionStyle = (DataSourceItem.CaptionStyleType)Enum.Parse(typeof(DataSourceItem.CaptionStyleType), dropdownlistCaptionStyle.SelectedValue);
            report.TempFieldItems[dataSourceIndex] = XmlConverter.ConvertFrom(fieldItem);
        }

        private void SaveSetting(WebEasilyReport report)
        {
            Control container = this.report.MultiView;

            CheckBox checkboxColumnGrid = (CheckBox)container.FindControl(CHECKBOX_COLUMNGRID);
            this.report.Format.ColumnGridLine = checkboxColumnGrid.Checked;

            CheckBox cbxInnerColumnGrid = (CheckBox)container.FindControl(CHECKBOX_INNER_COLUMNGRID);
            report.Format.ColumnInsideGridLine = cbxInnerColumnGrid.Checked;

            CheckBox checkboxRowGrid = (CheckBox)container.FindControl(CHECKBOX_ROWGRID);
            this.report.Format.RowGridLine = checkboxRowGrid.Checked;


            CheckBox checkboxHeaderRepeater = (CheckBox)container.FindControl(CHECKBOX_HEADER_REPEAT);
            this.report.HeaderRepeat = checkboxHeaderRepeater.Checked;

            RadioButtonList radiobuttonlistPageSize = (RadioButtonList)container.FindControl(RADIOBUTTONLIST_PAGE_SIZE);
            report.Format.PageSize = (ReportFormat.PageType)Enum.Parse(typeof(ReportFormat.PageType), radiobuttonlistPageSize.SelectedValue);

            RadioButtonList radiobuttonlistPrintOrientation = (RadioButtonList)container.FindControl(RADIOBUTTONLIST_PRINT_ORIENTATION);
            report.Format.Orientation = (System.Windows.Forms.Orientation)Enum.Parse(typeof(System.Windows.Forms.Orientation), radiobuttonlistPrintOrientation.SelectedValue);

            TextBox textboxPageRecords = (TextBox)container.FindControl(TEXTBOX_PAGE_RECORDS);
            report.Format.PageRecords = Convert.ToInt32(textboxPageRecords.Text);

            DropDownList dropdownlistExportFormat = (DropDownList)container.FindControl(DROPDOWNLIST_EXPORT_FORMAT);
            report.Format.ExportFormat = (ReportFormat.ExportType)Enum.Parse(typeof(ReportFormat.ExportType), dropdownlistExportFormat.SelectedValue);

            TextBox textboxMailTitle = (TextBox)container.FindControl(TEXTBOX_MAIL_TITLE);
            report.MailSetting.Subject = textboxMailTitle.Text;

            TextBox textboxMailTo = (TextBox)container.FindControl(TEXTBOX_MAIL_TO);
            report.MailSetting.MailTo = textboxMailTo.Text;

            TextBox textboxMailFrom = (TextBox)container.FindControl(TEXTBOX_MAIL_FROM);
            report.MailSetting.MailFrom = textboxMailFrom.Text;

            TextBox textboxMailPassword = (TextBox)container.FindControl(TEXTBOX_MAIL_PASSWORD);
            report.MailSetting.Password = textboxMailPassword.Text;

            TextBox textboxMailServer = (TextBox)container.FindControl(TEXTBOX_MAIL_SERVER);
            report.MailSetting.Host = textboxMailServer.Text;

            TextBox textboxLeftMargin = (TextBox)container.FindControl(TEXTBOX_LEFTMARGIN);
            if (!string.IsNullOrEmpty(textboxLeftMargin.Text.Trim()))
            {
                report.Format.MarginLeft = Convert.ToInt32(textboxLeftMargin.Text.Trim());
            }

            TextBox textboxRightMargin = (TextBox)container.FindControl(TEXTBOX_RIGHTMARGIN);
            if (!string.IsNullOrEmpty(textboxRightMargin.Text.Trim()))
            {
                report.Format.MarginRight = Convert.ToInt32(textboxRightMargin.Text.Trim());
            }

            TextBox textboxTopMargin = (TextBox)container.FindControl(TEXTBOX_TOPMARGIN);
            if (!string.IsNullOrEmpty(textboxTopMargin.Text.Trim()))
            {
                report.Format.MarginTop = Convert.ToInt32(textboxTopMargin.Text.Trim());
            }

            TextBox textboxBottomMargin = (TextBox)container.FindControl(TEXTBOX_BOTTOMMARGIN);
            if (!string.IsNullOrEmpty(textboxBottomMargin.Text.Trim()))
            {
                report.Format.MarginBottom = Convert.ToInt32(textboxBottomMargin.Text.Trim());
            }

            CheckBox checkboxSendMail = (CheckBox)container.FindControl(CHECKBOX_SEND_MAIL);
            if (checkboxSendMail.Checked)
            {
                report.OutputMode = OutputModeType.Email;
            }
            else
            {
                report.OutputMode = OutputModeType.None;
            }

        }

        private void SaveFont(WebEasilyReport report)
        {
            report.HeaderFont = (Font)report.FontTable[string.Format("{0}{1}", BUTTON_ITEM_FONT, HEADER)];
            report.FooterFont = (Font)report.FontTable[string.Format("{0}{1}", BUTTON_ITEM_FONT, FOOTER)];
            report.FieldFont = (Font)report.FontTable[BUTTON_FIELD_FONT];
        }
        #endregion

        #region Common Function
       
       

        private string GetUpdatePanelControlID(Control control)
        {
            StringBuilder builder = new StringBuilder(control.ID);
            Control parent = control.Parent;
            while (parent != null && parent.GetType() != typeof(UpdatePanel))
            {
                if (parent.GetType().GetInterface("INamingContainer") != null)
                {
                    builder.Insert(0, parent.ID + "$");
                }
                parent = parent.Parent;
            }
            return builder.ToString();
        }

        class ViewNum
        {
            public const int HeaderItemView = 0;
            public const int FieldView = 1;
            public const int FooterItemView = 2;
            public const int SettingView = 3;
            //public const int TemplateView = 4;
            //public const int FontView = 5;
            //public const int SaveAsView = 6;
            public const int PictureView = 4;
            public const int FileUploadView = 5;
        }

        class ModelPopupView
        {
            public const string PictureView = MODEL_POPUP_EXTENDER_PICTUREVIEW;
            public const string TemplateView = MODEL_POPUP_EXTENDER_TEMPLATE_LOAD;
            public const string SaveAsView = MODEL_POPUP_EXTENDER_SAVEASVIEW;
            public const string FontView = MODEL_POPUP_EXTENDER_FONTVIEW;
            public const string DownloadView = MODEL_POPUP_EXTENDER_DOWNLOADVIEW;
        }

        enum ImageMode
        {
            Add,
            Change
        }

        private void ShowModelPopup(string modelPopupViewID)
        {
            ModalPopupExtender modelPopupExtender = (ModalPopupExtender)report.FindControl(modelPopupViewID);
            modelPopupExtender.Show();
        }

        private void HideModelPopup(string modelPopupViewID)
        {
            ModalPopupExtender modelPopupExtender = (ModalPopupExtender)report.FindControl(modelPopupViewID);
            modelPopupExtender.Hide();
        }

        private void ShowFileUpload()
        {
            Panel panel = (Panel)report.FindControl(PANEL_FILEUPLOAD_VIEW);
            panel.Style[HtmlTextWriterStyle.Display] = "block";
        }

        private void HideFileUpload()
        {
            Panel panel = (Panel)report.FindControl(PANEL_FILEUPLOAD_VIEW);
            panel.Style[HtmlTextWriterStyle.Display] = "none";
        }

        private void ActiveView(int viewIndex)
        {
            MultiView multiViewOutput = (MultiView)report.FindControl(MULTIVIEW_OUTPUTVIEW);
            multiViewOutput.ActiveViewIndex = viewIndex;
        }

        class OutputView
        {
            public const int ProgressView = 0;
            public const int DownloadView = 1;
            public const int SendMailView = 2;
        }

        private void ListItemUp(ListBox listBox)
        {
            int selectedIndex = listBox.SelectedIndex;
            ListItem item = null;

            if (selectedIndex != 0)
            {
                item = listBox.Items[selectedIndex - 1];
                listBox.Items.RemoveAt(selectedIndex - 1);
                listBox.Items.Insert(selectedIndex, item);
            }
        }

        private void ListItemDown(ListBox listBox)
        {
            int selectedIndex = listBox.SelectedIndex;
            ListItem item = null;
            if (selectedIndex != listBox.Items.Count - 1)
            {
                item = listBox.Items[selectedIndex];
                listBox.Items.RemoveAt(selectedIndex);
                listBox.Items.Insert(selectedIndex + 1, item);
            }
        }

        #endregion

        #region EVENT
        void buttonLoad_Click(object sender, EventArgs e)
        {
            ShowModelPopup(ModelPopupView.TemplateView);
            InitialDataBaseView();
        }

        void buttonSave_Click(object sender, EventArgs e)
        {
            string message = String.Empty;

            ExecutionResult exeRes = null;

            if (this.report.selectedTemplateName != String.Empty && this.report.selectedTemplateName != null)
            {
                Save();
                exeRes = report.dbGateway.SaveTemplate(this.report.selectedTemplateName);

                if (exeRes.Status)
                {
                    message = MessageInfo.SaveSuccess;
                    AspNetScriptsProvider.ShowMessage(this.report, message);
                }
                else
                {
                    message = exeRes.Message;
                    AspNetScriptsProvider.ShowMessage(this.report, message);
                }
            }
            else
            {
                SaveAs();
            }
        }

        void buttonSaveAs_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        void SaveAs()
        {
            ShowModelPopup(ModelPopupView.SaveAsView);

            //Save();
        }

        void buttonExport_Click(object sender, EventArgs e)
        {
            Save();
            //ActiveView(OutputView.ProgressView);

            string strUrl = String.Empty;
            string strPath = this.report.GetOutputPath(this.report.Format.ExportFormat);

            strUrl = report.GetOutputUrl(this.report.Format.ExportFormat);

            this.report.FilePath = strPath;

            IReportExport exporter = null;
            if (this.report.Format.ExportFormat == ReportFormat.ExportType.Excel)
            {
                exporter = new ExcelReportExporter(this.report, ExportMode.Export, false);
                exporter.FileName = this.report.FilePath;
            }
            else
            {
                exporter = new PdfReportExporter(this.report, ExportMode.Export, false);
            }

            exporter.Export();

            Control container = this.report.MultiView;
            CheckBox cbxSendMail = (CheckBox)container.FindControl(CHECKBOX_SEND_MAIL);
            if (cbxSendMail.Checked)
            {
                ShowModelPopup(ModelPopupView.DownloadView);
                ActiveView(OutputView.SendMailView);
                InitialSendMailView(this.report);

                MemoryStream ms = new MemoryStream(File.ReadAllBytes(this.report.FilePath));
                this.report.MailSetting.Attachments.Add(new Attachment(ms, Path.GetFileName(this.report.FilePath)));
            }
            else
            {
                AspNetScriptsProvider.DownLoadFile(this.report);
            }
        }

        void buttonClose_Click(object sender, EventArgs e)
        {
            this.report.Visible = false;
        }

        #endregion
    }
}
