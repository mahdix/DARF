namespace AdminConsole
{
    partial class ctlValuesPicker
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
            this.lstParameters = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmdAddParam = new System.Windows.Forms.Button();
            this.cmdEdit = new System.Windows.Forms.Button();
            this.cmdDeleteParam = new System.Windows.Forms.Button();
            this.tblGeneralArgPicker = new System.Windows.Forms.TableLayoutPanel();
            this.tblTypedArgPicker = new System.Windows.Forms.TableLayoutPanel();
            this.tblGeneralArgPicker.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstParameters
            // 
            this.lstParameters.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lstParameters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.tblGeneralArgPicker.SetColumnSpan(this.lstParameters, 3);
            this.lstParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstParameters.FullRowSelect = true;
            this.lstParameters.GridLines = true;
            this.lstParameters.Location = new System.Drawing.Point(3, 3);
            this.lstParameters.Name = "lstParameters";
            this.lstParameters.Size = new System.Drawing.Size(324, 86);
            this.lstParameters.TabIndex = 4;
            this.lstParameters.UseCompatibleStateImageBehavior = false;
            this.lstParameters.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 130;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Type";
            this.columnHeader3.Width = 130;
            // 
            // cmdAddParam
            // 
            this.cmdAddParam.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmdAddParam.Location = new System.Drawing.Point(26, 95);
            this.cmdAddParam.Name = "cmdAddParam";
            this.cmdAddParam.Size = new System.Drawing.Size(56, 23);
            this.cmdAddParam.TabIndex = 7;
            this.cmdAddParam.Text = "Add";
            this.cmdAddParam.UseVisualStyleBackColor = true;
            this.cmdAddParam.Click += new System.EventHandler(this.cmdAddParam_Click);
            // 
            // cmdEdit
            // 
            this.cmdEdit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmdEdit.Location = new System.Drawing.Point(136, 95);
            this.cmdEdit.Name = "cmdEdit";
            this.cmdEdit.Size = new System.Drawing.Size(56, 23);
            this.cmdEdit.TabIndex = 6;
            this.cmdEdit.Text = "Edit";
            this.cmdEdit.UseVisualStyleBackColor = true;
            this.cmdEdit.Click += new System.EventHandler(this.cmdEdit_Click);
            // 
            // cmdDeleteParam
            // 
            this.cmdDeleteParam.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmdDeleteParam.Location = new System.Drawing.Point(246, 95);
            this.cmdDeleteParam.Name = "cmdDeleteParam";
            this.cmdDeleteParam.Size = new System.Drawing.Size(56, 23);
            this.cmdDeleteParam.TabIndex = 5;
            this.cmdDeleteParam.Text = "Remove";
            this.cmdDeleteParam.UseVisualStyleBackColor = true;
            this.cmdDeleteParam.Click += new System.EventHandler(this.cmdDeleteParam_Click);
            // 
            // tblGeneralArgPicker
            // 
            this.tblGeneralArgPicker.ColumnCount = 3;
            this.tblGeneralArgPicker.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tblGeneralArgPicker.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tblGeneralArgPicker.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tblGeneralArgPicker.Controls.Add(this.cmdAddParam, 0, 1);
            this.tblGeneralArgPicker.Controls.Add(this.lstParameters, 0, 0);
            this.tblGeneralArgPicker.Controls.Add(this.cmdDeleteParam, 2, 1);
            this.tblGeneralArgPicker.Controls.Add(this.cmdEdit, 1, 1);
            this.tblGeneralArgPicker.Location = new System.Drawing.Point(3, 3);
            this.tblGeneralArgPicker.Name = "tblGeneralArgPicker";
            this.tblGeneralArgPicker.RowCount = 2;
            this.tblGeneralArgPicker.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblGeneralArgPicker.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblGeneralArgPicker.Size = new System.Drawing.Size(330, 122);
            this.tblGeneralArgPicker.TabIndex = 8;
            // 
            // tblTypedArgPicker
            // 
            this.tblTypedArgPicker.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tblTypedArgPicker.ColumnCount = 2;
            this.tblTypedArgPicker.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tblTypedArgPicker.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblTypedArgPicker.Location = new System.Drawing.Point(6, 140);
            this.tblTypedArgPicker.Name = "tblTypedArgPicker";
            this.tblTypedArgPicker.RowCount = 1;
            this.tblTypedArgPicker.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblTypedArgPicker.Size = new System.Drawing.Size(327, 30);
            this.tblTypedArgPicker.TabIndex = 9;
            // 
            // ctlValuesPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tblTypedArgPicker);
            this.Controls.Add(this.tblGeneralArgPicker);
            this.Name = "ctlValuesPicker";
            this.Size = new System.Drawing.Size(402, 277);
            this.Load += new System.EventHandler(this.ctlValuesPicker_Load);
            this.tblGeneralArgPicker.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstParameters;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button cmdAddParam;
        private System.Windows.Forms.Button cmdEdit;
        private System.Windows.Forms.Button cmdDeleteParam;
        private System.Windows.Forms.TableLayoutPanel tblGeneralArgPicker;
        private System.Windows.Forms.TableLayoutPanel tblTypedArgPicker;
    }
}
