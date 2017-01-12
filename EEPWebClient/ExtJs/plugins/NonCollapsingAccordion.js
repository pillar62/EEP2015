Ext.ux.NonCollapsingAccordionLayout = Ext.extend(Ext.layout.Accordion, {
    animate: false,

    // A reference to the currently expanded panel that I can change without altering the this.activeItem object
    currentlyExpandedPanel: null,


    renderItem: function(c) {
        // Call super.renderItem
        Ext.ux.NonCollapsingAccordionLayout.superclass.renderItem.apply(this, arguments);

        // If not set yet, initialize this.currentlyExpandedPanel to the first panel
        if (!this.currentlyExpandedPanel) this.currentlyExpandedPanel = this.container.items.items[0];

        // Setup event listeners for beforeexpand and beforecollapse to run the functionality
        c.on('beforeexpand', this.beforeExpandPanel, this);
        c.on('beforecollapse', this.beforeCollapsePanel, this);
    },

    beforeExpandPanel: function(panel) {
        var panelToCollapse = this.currentlyExpandedPanel;  // A holder for the previously selected panel
        this.currentlyExpandedPanel = panel;                // Set the new panel as the currently expanded one
        panelToCollapse.collapse();                         // Collapse the previously selected panel
    },

    beforeCollapsePanel: function(panel) {
        // Cancel the collapse if the panel to collapse is the currently expanded panel
        if (panel == this.currentlyExpandedPanel) return false;
    }
});
Ext.Container.LAYOUTS['noncollapsingaccordion'] = Ext.ux.NonCollapsingAccordionLayout;