namespace Srvtools
{
    partial class frmWebClientQueryEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWebClientQueryEditor));
            this.infoRefValQuery = new Srvtools.InfoRefVal();
            ((System.ComponentModel.ISupportInitialize)(this.infoRefValQuery)).BeginInit();
            this.SuspendLayout();
            // 
            // infoRefValQuery
            // 
            this.infoRefValQuery.AutoGridSize = false;
            this.infoRefValQuery.Caption = null;
            this.infoRefValQuery.DataSource = null;
            this.infoRefValQuery.DisplayMember = null;
            this.infoRefValQuery.EditingDisplayMember = null;
            this.infoRefValQuery.RefByWhere = false;
            this.infoRefValQuery.SelectAlias = null;
            this.infoRefValQuery.SelectCommand = null;
            this.infoRefValQuery.SelectTop = null;
            this.infoRefValQuery.Styles = Srvtools.InfoRefVal.ShowStyle.gridStyle;
            this.infoRefValQuery.ValueMember = null;
            // 
            // frmWebClientQueryEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(485, 282);
            this.Name = "frmWebClientQueryEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WebClientQueryPreviewer";
            ((System.ComponentModel.ISupportInitialize)(this.infoRefValQuery)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private InfoRefVal infoRefValQuery;


    }
}