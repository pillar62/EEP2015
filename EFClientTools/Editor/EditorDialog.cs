using System.Windows.Forms;

namespace EFClientTools.Editor
{
    public class EditorDialog: Form
    {
        public object ReturnValue { get; set; }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // EditorDialog
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditorDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }
    }
}
