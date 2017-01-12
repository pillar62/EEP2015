using System;
using System.ComponentModel.Design;
using System.Data;
using System.Web.UI.Design;
using System.Windows.Forms;
using Srvtools;


namespace AjaxTools
{
    public class AjaxScheduleDesigner : ControlDesigner
    {
        IDesignerHost host = null;
        IComponentChangeService svcCompChange = null;
        AjaxSchedule schedule = null;

        public override System.ComponentModel.Design.DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerbCollection verbs = new DesignerVerbCollection();
                verbs.Add(new DesignerVerb("Generate default schedule buttons...", new EventHandler(GenButtons)));
                return verbs;
            }
        }

        public void GenButtons(object sender, EventArgs e)
        {
            if (schedule == null)
            {
                schedule = this.Component as AjaxSchedule;
            }
            if (schedule.ScheduleButtons.Count == 0)
            {
                if (host == null)
                {
                    host = (IDesignerHost)this.GetService(typeof(IDesignerHost));
                }
                if (svcCompChange == null)
                {
                    svcCompChange = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                }

                DesignerTransaction trans = host.CreateTransaction("Generate buttons");
                this.addButton(AjaxScheduleButtonType.PreviousYear, "<<");
                this.addButton(AjaxScheduleButtonType.Previous, "<");
                this.addButton(AjaxScheduleButtonType.NextYear, ">>");
                this.addButton(AjaxScheduleButtonType.Next, ">");
                this.addButton(AjaxScheduleButtonType.Today, "today");
                this.addButton(AjaxScheduleButtonType.Month, "month");
                this.addButton(AjaxScheduleButtonType.Week, "week");
                this.addButton(AjaxScheduleButtonType.Day, "day");
                trans.Commit();
                MessageBox.Show("generate buttons successful!");
            }
        }

        void addButton(AjaxScheduleButtonType buttonType, string text)
        {
            AjaxScheduleToolItem item = new AjaxScheduleToolItem();
            item.ButtonType = buttonType;
            item.ButtonText = text;
            svcCompChange.OnComponentChanging(schedule, null);
            schedule.ScheduleButtons.Add(item);
            svcCompChange.OnComponentChanged(schedule, null, null, null);
        }
    }
}
