using System;
using System.Collections.Generic;
using System.Web.UI.Design;
using System.ComponentModel.Design;
using System.Text;
using System.ComponentModel;

namespace Srvtools
{
    public class WebClientQueryEditor: DataSourceDesigner
    {
        private frmWebClientQueryEditor previewer = null;
        private DesignerActionListCollection _actionLists;
        
        public WebClientQueryEditor()
        {
            DesignerVerb previewVerb = new DesignerVerb("Preview", new EventHandler(OnPreview));
            this.Verbs.Add(previewVerb);
        }

        public void OnPreview(object sender, EventArgs e)
        {
            if (previewer == null)
            {
                previewer = new frmWebClientQueryEditor(this.Component as WebClientQuery);
            }
            previewer.ShowDialog();
            previewer = null;
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                _actionLists = base.ActionLists;

                if (_actionLists != null)
                    _actionLists.Add(new WebClientQueryActionList(this.Component));

                return _actionLists;
            }
        }
    }

    public class WebClientQueryActionList : DesignerActionList
    {
        private WebClientQuery wcq;
        private frmWebClientQueryEditor previewer = null; 
        
        //public WebClientQueryActionList()
        //{ 
            
        //}

        public WebClientQueryActionList(IComponent component):base(component)
        {
            wcq = component as WebClientQuery;
        }

        public void OnPreview()
        {
            if (previewer == null)
            {
                previewer = new frmWebClientQueryEditor(wcq);
            }
            previewer.ShowDialog();
            previewer = null;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {   
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(this, "OnPreview", "Preview", "UsePreview", true));
            return items;
        }
    
    
    
    
    }

}
