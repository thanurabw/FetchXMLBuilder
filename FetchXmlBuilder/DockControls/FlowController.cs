using System;
using System.Windows.Forms;

namespace Cinteros.Xrm.FetchXmlBuilder.DockControls
{
    public partial class FlowController : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private FetchXmlBuilder fxb;

        public FlowController(FetchXmlBuilder fetchXmlBuilder)
        {
            fxb = fetchXmlBuilder;
            InitializeComponent();
        }

        internal string GetStepToApplyConditions()
        {
            string stepToApplyConditions = "triggerBody()";

            if (radiobtnCustom.Checked) stepToApplyConditions = txtStep.Text;

            return stepToApplyConditions;
        }

        internal void DisplayFlowConditions(string conditions)
        {
            condtionsText.Text = conditions;
        }

        private void FlowControl_DockStateChanged(object sender, EventArgs e)
        {
            DockPanel.DockBottomPortion = 80;
            DockPanel.DockTopPortion = 80;
            if (!IsHidden)
            {
                DisplayFlowConditions(fxb.GetFlowConditions());
            }
        }

        private void menuODataCopy_Click(object sender, EventArgs e)
        {
            if (condtionsText.Text.Length > 0 && !condtionsText.Text.Equals("Flow Conditions:"))
            {
                Clipboard.SetText(condtionsText.Text);
                fxb.LogUse("CopyFlowConditions");
            }
            else
            {
                MessageBox.Show("No conditions to copy");
            }
        }

        /// <summary>
        /// This event occurs when the default scope radio button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radiobtnDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtnDefault.Checked)
            {
                radiobtnCustom.Checked = false;
                txtStep.ReadOnly = true;
            }
            else if (radiobtnCustom.Checked == false) // cannot untick if other option is also not ticked
            {
                radiobtnDefault.Checked = true;
                txtStep.ReadOnly = true;
            }

            if (!IsHidden)
            {
                DisplayFlowConditions(fxb.GetFlowConditions());
            }

        }

        /// <summary>
        /// This event occurs when the custom scope radio button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radiobtnCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtnCustom.Checked)
            {
                radiobtnDefault.Checked = false;
                txtStep.ReadOnly = false;
            }
            else if (radiobtnDefault.Checked == false) // cannot untick if other option is also not ticked
            {
                radiobtnCustom.Checked = true;
                txtStep.ReadOnly = false;
            }

            if (!IsHidden)
            {
                DisplayFlowConditions(fxb.GetFlowConditions());
            }
        }

        private void txtStep_TextChanged(object sender, EventArgs e)
        {
            if (!IsHidden)
            {
                DisplayFlowConditions(fxb.GetFlowConditions());
            }
        }
    }
}