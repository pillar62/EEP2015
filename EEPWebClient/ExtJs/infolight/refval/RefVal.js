/// <reference path="../../adapter/ext/ext-base-debug.js"/>
/// <reference path="../../ext-all-debug.js"/>

//Infolight.RefVal = Ext.extend(Ext.form.ComboBox, {
//    constructor: function(config) {
//        if (config) {
//            if (config.embededIn) {
//                this.embededIn = config.embededIn;
//            }
//        }
//        Infolight.ComboBox.superclass.constructor.call(this, config);
//    },
//    embededIn: ''
//});
Ext.define("Infolight.RefVal", {
    extend: "Ext.form.field.Text",
    alias: "widget.infoRefVal",
    inputTyle: 'reffield',
    isLoader: true,
    buttonText: "a",
    constructor: function (config) {
        this.callParent(arguments);//Calling the parent class constructor
        this.initConfig(config);//Initializing the component
    },
    onRender: function (ct, position) {
        this.callParent(arguments);
        this.codeEl = ct.createChild({ tag: 'img' ,value:'asd',width:20});
        this.codeEl.addCls('x-form-code');
        this.codeEl.value = this.buttonText;
    }
});


Infolight.RefValBoxHelper = function() {
    return {
        createRefValBox: function(config) {
        
        
            var param = new setRefvalparam();
            param.refvalID = "GexRefVal_WAREID";
            param.refTitle = "倉庫選擇";
            param.refModuleName = "GLModule";
            param.refCommandName = "cmdRefValUse";
            param.refDynamic = "Y";
            param.refCmdsql = "SELECT * FROM USERS";
            param.refShowFields = "USERID,USERNAME";  //這裡注意，因為後端的 SQL,有的有left join多表，有別名等，這裡欄位可能出現在對錶中，所以要加上別名或表頭，如BASPERSON.PERSONID
            param.refShowFieldsWidth = "80,120,120,120,100,150,120";
            param.refBindControlID = "GexRefValWAREID";
            param.refBindValueColumn = "USERID";
            param.refBindTextColumn = "USERNAME";
            param.refAutoShow = true;
            //ColumnMatch屬性設定
            //param.refMatchSrcColumns = ['WARECNAME'];   //可支持自定義function
            //param.refMatchDestcontrolIDs = ['GexRefValWAREID_Name'];

            var GexRef_WareID = createGexRef(param);
            return GexRef_WareID;
//            Ext.apply(config.comboConfig, { renderTo: config.renderTo });
//            return new Infolight.RefVal(config.comboConfig);
        }
    };
} ();