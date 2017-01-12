<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JQueryMasterDetail.aspx.cs" Inherits="Template_JQueryMasterDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../js/themes/default/easyui.css" rel="stylesheet" />
    <link href="../js/themes/icon.css" rel="stylesheet" />
    <script src="../js/jquery-1.8.0.min.js"></script>
    <script src="../js/jquery.easyui.min.js"></script>
    <script src="../js/jquery.json.js"></script>
    <script src="../js/jquery.infolight.js"></script>
    <script src="../js/datagrid-detailview.js"></script>
    <script src="../js/locale/easyui-lang-zh_TW.js"></script>
</head>
<body>
    <form name="form1" method="post" id="form1">
        <div>
            <div id="querydataGridMaster" class="easyui-window" data-options="iconCls:'icon-search',closed:true,collapsible:false,maximizable:false,minimizable:false" title="Query" style="padding: 10px">
                <table>  
                    <!-- 填寫查詢欄位  -->
                                      
                    <tr>
                        <td></td>
                        <td align="right"><a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="query('#dataGridMaster', 'Window')">Query</a></td>
                        <td><a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-undo'" onclick="clearQuery('#dataGridMaster', 'Window', false)">Clear</a></td>
                    </tr>
                </table>
            </div>

            <!-- Master所在Grid -->
            <table id="dataGridMaster" class="info-datagrid" title="Orders" data-options="toolbar:'#toolbardataGridMaster',pagination:true,view:commandview" infolight-options="autoApply:true,alwaysClose:false,remoteName:'',tableName:'',duplicateCheck:false,queryDialog:'#querydataGridMaster',editDialog:'#JQDialog1',editMode:'Dialog',queryAutoColumn:false,commandButtons:'vud',totalCaption:'Total:',allowInsert:true,allowUpdate:true,allowDelete:true">
            <!-- Master顯示的欄位 -->

            </table>

            <div id="toolbardataGridMaster" height="auto">
                <a id="toolItemdataGridMaster新增" href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="insertItem('#dataGridMaster')">新增</a>
                <a id="toolItemdataGridMaster存檔" href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save',plain:true" onclick="apply('#dataGridMaster')">存檔</a>
                <a id="toolItemdataGridMaster取消" href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-undo',plain:true" onclick="cancel('#dataGridMaster')">取消</a>
                <a id="toolItemdataGridMaster查詢" href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-search',plain:true" onclick="openQuery('#dataGridMaster')">查詢</a>
                <a id="toolItemdataGridMaster输出" href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-print',plain:true" onclick="exportGrid('#dataGridMaster')">输出</a>
            </div>

            <div id="JQDialog1" class="easyui-dialog" title="JQDialog" data-options="closed:'True'" infolight-options="containForm:'#dataFormMaster'" style="width: 600px; padding: 10px">
                <div style="padding: 5px 0 5px 20px">
                    <!-- Master新增、修改以及顯示的div -->
                    <div id="dataFormMaster" infolight-options="remoteName:'',tableName:'',duplicateCheck:false">
                        <table>
                            <!-- 設定Master編輯及新增的欄位 -->
                        </table>
                    </div>

                    <!-- Detail顯示的Grid -->
                    <table id="dataGridDetail" class="info-datagrid" title="明細資料" data-options="toolbar:'#toolbardataGridDetail',pagination:false,view:defaultview" infolight-options="autoApply:false,alwaysClose:false,remoteName:'',tableName:'',duplicateCheck:true,parent:'dataFormMaster',parentRelations:[{field:'',parentField:''}],queryAutoColumn:false,commandButtons:'vud',allowInsert:true,allowUpdate:true,allowDelete:true">
                        <!-- Detail編輯的欄位 -->

                    </table>
                    <div id="toolbardataGridDetail" height="auto">
                        <a id="toolItemdataGridDetail新增" href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="insertItem('#dataGridDetail')">新增</a>
                        <a id="toolItemdataGridDetail更改" href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="updateItem('#dataGridDetail')">更改</a>
                        <a id="toolItemdataGridDetail刪除" href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="deleteItem('#dataGridDetail')">刪除</a>
                    </div>
                    <br />

                    <br />

                    <br />

                    <br />


                </div>
                <div style="text-align: center; padding: 5px">
                    <a href="javascript:void(0)" class="easyui-linkbutton" onclick="submitForm('#JQDialog1')">Submit</a><a href="javascript:void(0)" class="easyui-linkbutton" onclick="closeForm('#JQDialog1')">Close</a>
                </div>
            </div>
            <div id="dlgdetail" class="easyui-dialog" style="width: 600px; padding: 10px 20px; display: none" closed="true"
                buttons="#dlg-buttons" infolight-options="containForm:'#dataFormDetail'">
                <!-- Detail編輯Form所在畫面 -->
                <div id="dataFormDetail" style="padding: 5px 0 5px 20px" infolight-options="remoteName:'',tableName:''">
                    <!--detail編輯的欄位-->
                    
                </div>
                <div style="text-align: center; padding: 5px">
                    <a href="javascript:void(0)" class="easyui-linkbutton" onclick="submitForm('#dlgdetail')">Submit</a>
                    <a href="javascript:void(0)" class="easyui-linkbutton" onclick="closeForm('#dlgdetail')">Close</a>
                </div>
            </div>
        </div>
    </form>

</body>
</html>
