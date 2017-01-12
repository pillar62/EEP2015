Ext.InfoPagingToolbar = Ext.extend(Ext.PagingToolbar, {
    doLoad: function(start) {
    var o = this.store.lastOptions.params, pn = this.getParams();
        o[pn.start] = start;
        o[pn.limit] = this.pageSize;
        if (this.fireEvent('beforechange', this, o) !== false) {
            this.store.load({ params: o });
        }
    }
});
Ext.reg('infopaging', Ext.InfoPagingToolbar);
