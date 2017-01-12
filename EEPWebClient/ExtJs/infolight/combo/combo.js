/// <reference path="../../adapter/ext/ext-base-debug.js"/>
/// <reference path="../../ext-all-debug.js"/>

Ext.define("Infolight.ComboBox", {
    extend: "Ext.form.ComboBox",
    //类似Ext.reg的功能  
    alias: "widget.infoCombo",
    constructor: function(config) {
        this.callParent(arguments);//Calling the parent class constructor
        this.initConfig(config);//Initializing the component
    },
});

Infolight.ComboBoxHelper = function() {
    return {
        createComboBox: function(config) {
            Ext.apply(config.comboConfig, { renderTo: config.renderTo });
            return new Infolight.ComboBox(config.comboConfig);
        }
    };
} ();