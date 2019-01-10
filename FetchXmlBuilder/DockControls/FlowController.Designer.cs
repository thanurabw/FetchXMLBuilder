namespace Cinteros.Xrm.FetchXmlBuilder.DockControls
{
    partial class FlowController
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlowController));
            this.panFlow = new System.Windows.Forms.Panel();
            this.txtStep = new System.Windows.Forms.TextBox();
            this.radiobtnCustom = new System.Windows.Forms.RadioButton();
            this.lblScope = new System.Windows.Forms.Label();
            this.radiobtnDefault = new System.Windows.Forms.RadioButton();
            this.condtionsText = new System.Windows.Forms.Label();
            this.menuFlow = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuFlowCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.panFlowLabel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panFlow.SuspendLayout();
            this.menuFlow.SuspendLayout();
            this.panFlowLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panFlow
            // 
            this.panFlow.BackColor = System.Drawing.SystemColors.Window;
            this.panFlow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panFlow.Controls.Add(this.txtStep);
            this.panFlow.Controls.Add(this.radiobtnCustom);
            this.panFlow.Controls.Add(this.lblScope);
            this.panFlow.Controls.Add(this.radiobtnDefault);
            this.panFlow.Controls.Add(this.condtionsText);
            this.panFlow.Controls.Add(this.panFlowLabel);
            this.panFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panFlow.Location = new System.Drawing.Point(0, 0);
            this.panFlow.Name = "panFlow";
            this.panFlow.Padding = new System.Windows.Forms.Padding(4);
            this.panFlow.Size = new System.Drawing.Size(787, 124);
            this.panFlow.TabIndex = 28;
            // 
            // txtStep
            // 
            this.txtStep.Location = new System.Drawing.Point(523, 4);
            this.txtStep.Name = "txtStep";
            this.txtStep.ReadOnly = true;
            this.txtStep.Size = new System.Drawing.Size(257, 20);
            this.txtStep.TabIndex = 20;
            this.txtStep.Text = "body(\'step_name_goes_here\')";
            this.txtStep.TextChanged += new System.EventHandler(this.txtStep_TextChanged);
            // 
            // radiobtnCustom
            // 
            this.radiobtnCustom.AutoSize = true;
            this.radiobtnCustom.Location = new System.Drawing.Point(432, 5);
            this.radiobtnCustom.Name = "radiobtnCustom";
            this.radiobtnCustom.Size = new System.Drawing.Size(85, 17);
            this.radiobtnCustom.TabIndex = 19;
            this.radiobtnCustom.Text = "Custom Step";
            this.radiobtnCustom.UseVisualStyleBackColor = true;
            this.radiobtnCustom.CheckedChanged += new System.EventHandler(this.radiobtnCustom_CheckedChanged);
            // 
            // lblScope
            // 
            this.lblScope.AutoSize = true;
            this.lblScope.Location = new System.Drawing.Point(46, 4);
            this.lblScope.Name = "lblScope";
            this.lblScope.Size = new System.Drawing.Size(237, 13);
            this.lblScope.TabIndex = 18;
            this.lblScope.Text = "Record scope (Flow step to apply conditions on):";
            // 
            // radiobtnDefault
            // 
            this.radiobtnDefault.AutoSize = true;
            this.radiobtnDefault.Checked = true;
            this.radiobtnDefault.Location = new System.Drawing.Point(289, 4);
            this.radiobtnDefault.Name = "radiobtnDefault";
            this.radiobtnDefault.Size = new System.Drawing.Size(125, 17);
            this.radiobtnDefault.TabIndex = 17;
            this.radiobtnDefault.TabStop = true;
            this.radiobtnDefault.Text = "Default (TriggerBody)";
            this.radiobtnDefault.UseVisualStyleBackColor = true;
            this.radiobtnDefault.CheckedChanged += new System.EventHandler(this.radiobtnDefault_CheckedChanged);
            // 
            // condtionsText
            // 
            this.condtionsText.ContextMenuStrip = this.menuFlow;
            this.condtionsText.Location = new System.Drawing.Point(46, 27);
            this.condtionsText.Name = "condtionsText";
            this.condtionsText.Size = new System.Drawing.Size(734, 91);
            this.condtionsText.TabIndex = 3;
            this.condtionsText.Text = "Flow Conditions:";
            // 
            // menuFlow
            // 
            this.menuFlow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFlowCopy});
            this.menuFlow.Name = "menuOData";
            this.menuFlow.Size = new System.Drawing.Size(164, 26);
            // 
            // menuFlowCopy
            // 
            this.menuFlowCopy.Name = "menuFlowCopy";
            this.menuFlowCopy.Size = new System.Drawing.Size(163, 22);
            this.menuFlowCopy.Text = "Copy Conditions";
            this.menuFlowCopy.Click += new System.EventHandler(this.menuODataCopy_Click);
            // 
            // panFlowLabel
            // 
            this.panFlowLabel.Controls.Add(this.pictureBox1);
            this.panFlowLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.panFlowLabel.Location = new System.Drawing.Point(4, 4);
            this.panFlowLabel.Name = "panFlowLabel";
            this.panFlowLabel.Size = new System.Drawing.Size(42, 114);
            this.panFlowLabel.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 38);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // FlowController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 124);
            this.Controls.Add(this.panFlow);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Name = "FlowController";
            this.TabText = "Flow Conditions";
            this.Text = "Flow Conditions";
            this.DockStateChanged += new System.EventHandler(this.FlowControl_DockStateChanged);
            this.panFlow.ResumeLayout(false);
            this.panFlow.PerformLayout();
            this.menuFlow.ResumeLayout(false);
            this.panFlowLabel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panFlow;
        private System.Windows.Forms.Panel panFlowLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip menuFlow;
        private System.Windows.Forms.ToolStripMenuItem menuFlowCopy;
        private System.Windows.Forms.Label condtionsText;
        private System.Windows.Forms.TextBox txtStep;
        private System.Windows.Forms.RadioButton radiobtnCustom;
        private System.Windows.Forms.Label lblScope;
        private System.Windows.Forms.RadioButton radiobtnDefault;
    }
}
