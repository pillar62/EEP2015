/// <reference path="../../adapter/ext/ext-base-debug.js"/>
/// <reference path="../../ext-all-debug.js"/>

function renderMenuTree() {
    var subpath = '';
    for (i = 0; i < window.location.pathname.split('/').length - 3; i++) {
        subpath += '../';
    }
    Ext.each(this.menuCls, function(cls) {
        Ext.util.CSS.createStyleSheet('.' + cls.iconCls + '{background-image:url(' + subpath + 'Image/MenuTree/' + cls.iconFile + ');}');
    });

    var accordion = new Ext.Panel({
        layout: 'noncollapsingaccordion',
        layoutConfig: {
            //hideCollapseTool: true,
            titleCollapse: true,
            animate: true
        },
        items: this.items
    });

    var container = new Ext.Panel({
        layout: 'fit',
        title: this.title,
        frame: true,
        width: this.width,
        height: this.height,
        items: [accordion],
        renderTo: this.renderTo
    });
}

function genTree(title, iconCls, items) {
    var root = new Ext.tree.AsyncTreeNode({
        expanded: true,
        children: items
    });
    var tree = new Ext.tree.TreePanel({
        title:title,
        loader: new Ext.tree.TreeLoader(),
        root: root,
        animate: true,
        rootVisible: false,
        trackMouseOver: false,
        autoScroll: true,
        lines: true
    });
    if (iconCls && iconCls != '') {
        tree.iconCls = iconCls;
    }
    return tree;
}

//function genTree(items) {
//    var root = new Ext.tree.AsyncTreeNode({
//        expanded: true,
//        children: items
//    });
//    var tree = new Ext.tree.TreePanel({
//        loader: new Ext.tree.TreeLoader(),
//        root: root,
//        animate: true,
//        rootVisible: false,
//        trackMouseOver: false,
//        autoScroll: true,
//        lines: true
//    });
//    return tree;
//}

//[
//    new Ext.Panel({ title: 'Accordion Item 1', items: [genTree()] }),
//    new Ext.Panel({ title: 'Accordion Item 2', items: [genTree()] }),
//    new Ext.Panel({ title: 'Accordion Item 3', items: [genTree()] }),
//    new Ext.Panel({ title: 'Accordion Item 4', items: [genTree()] }),
//    new Ext.Panel({ title: 'Accordion Item 5', items: [genTree()] })
//]