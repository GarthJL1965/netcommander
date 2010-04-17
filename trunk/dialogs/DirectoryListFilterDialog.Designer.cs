namespace netCommander
{
    partial class DirectoryListFilterDialog
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxNetworkDrives = new System.Windows.Forms.CheckBox();
            this.checkBoxRemovableDrives = new System.Windows.Forms.CheckBox();
            this.checkBoxFixedDrives = new System.Windows.Forms.CheckBox();
            this.radioButtonLocalDrives = new System.Windows.Forms.RadioButton();
            this.radioButtonCurrentDrive = new System.Windows.Forms.RadioButton();
            this.checkBoxIncludeSubdirs = new System.Windows.Forms.CheckBox();
            this.textBoxCurrentDirectory = new System.Windows.Forms.TextBox();
            this.radioButtonCurrentDirectory = new System.Windows.Forms.RadioButton();
            this.textBoxFileMask = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBoxIgnoreAttributes = new System.Windows.Forms.CheckBox();
            this.flagBoxFileAttributes = new netCommander.FlagBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.checkBoxIgnoreSize = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSizeMaximum = new System.Windows.Forms.TextBox();
            this.textBoxSizeMinimum = new System.Windows.Forms.TextBox();
            this.comboBoxSizeCriteria = new System.Windows.Forms.ComboBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dateTimePickerAccessEnd = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePickerAccessBegin = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBoxIgnoreTimeAccess = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dateTimePickerModificationEnd = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePickerModificationBegin = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxIgnoreTimeModification = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateTimePickerCreateEnd = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePickerCreateBegin = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxIgnoreTimeCreate = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSaveDefault = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(376, 241);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.textBoxFileMask);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(368, 215);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "File name";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxNetworkDrives);
            this.groupBox4.Controls.Add(this.checkBoxRemovableDrives);
            this.groupBox4.Controls.Add(this.checkBoxFixedDrives);
            this.groupBox4.Controls.Add(this.radioButtonLocalDrives);
            this.groupBox4.Controls.Add(this.radioButtonCurrentDrive);
            this.groupBox4.Controls.Add(this.checkBoxIncludeSubdirs);
            this.groupBox4.Controls.Add(this.textBoxCurrentDirectory);
            this.groupBox4.Controls.Add(this.radioButtonCurrentDirectory);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox4.Location = new System.Drawing.Point(3, 33);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(362, 179);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Location";
            // 
            // checkBoxNetworkDrives
            // 
            this.checkBoxNetworkDrives.AutoSize = true;
            this.checkBoxNetworkDrives.Location = new System.Drawing.Point(135, 145);
            this.checkBoxNetworkDrives.Name = "checkBoxNetworkDrives";
            this.checkBoxNetworkDrives.Size = new System.Drawing.Size(97, 17);
            this.checkBoxNetworkDrives.TabIndex = 7;
            this.checkBoxNetworkDrives.Text = "Network drives";
            this.checkBoxNetworkDrives.UseVisualStyleBackColor = true;
            // 
            // checkBoxRemovableDrives
            // 
            this.checkBoxRemovableDrives.AutoSize = true;
            this.checkBoxRemovableDrives.Location = new System.Drawing.Point(135, 121);
            this.checkBoxRemovableDrives.Name = "checkBoxRemovableDrives";
            this.checkBoxRemovableDrives.Size = new System.Drawing.Size(111, 17);
            this.checkBoxRemovableDrives.TabIndex = 6;
            this.checkBoxRemovableDrives.Text = "Removable drives";
            this.checkBoxRemovableDrives.UseVisualStyleBackColor = true;
            // 
            // checkBoxFixedDrives
            // 
            this.checkBoxFixedDrives.AutoSize = true;
            this.checkBoxFixedDrives.Location = new System.Drawing.Point(135, 97);
            this.checkBoxFixedDrives.Name = "checkBoxFixedDrives";
            this.checkBoxFixedDrives.Size = new System.Drawing.Size(82, 17);
            this.checkBoxFixedDrives.TabIndex = 5;
            this.checkBoxFixedDrives.Text = "Fixed drives";
            this.checkBoxFixedDrives.UseVisualStyleBackColor = true;
            // 
            // radioButtonLocalDrives
            // 
            this.radioButtonLocalDrives.AutoSize = true;
            this.radioButtonLocalDrives.Location = new System.Drawing.Point(7, 96);
            this.radioButtonLocalDrives.Name = "radioButtonLocalDrives";
            this.radioButtonLocalDrives.Size = new System.Drawing.Size(82, 17);
            this.radioButtonLocalDrives.TabIndex = 4;
            this.radioButtonLocalDrives.TabStop = true;
            this.radioButtonLocalDrives.Text = "Local drives";
            this.radioButtonLocalDrives.UseVisualStyleBackColor = true;
            this.radioButtonLocalDrives.CheckedChanged += new System.EventHandler(this.radioButtonLocalDrives_CheckedChanged);
            // 
            // radioButtonCurrentDrive
            // 
            this.radioButtonCurrentDrive.AutoSize = true;
            this.radioButtonCurrentDrive.Location = new System.Drawing.Point(7, 72);
            this.radioButtonCurrentDrive.Name = "radioButtonCurrentDrive";
            this.radioButtonCurrentDrive.Size = new System.Drawing.Size(85, 17);
            this.radioButtonCurrentDrive.TabIndex = 3;
            this.radioButtonCurrentDrive.TabStop = true;
            this.radioButtonCurrentDrive.Text = "Current drive";
            this.radioButtonCurrentDrive.UseVisualStyleBackColor = true;
            this.radioButtonCurrentDrive.CheckedChanged += new System.EventHandler(this.radioButtonCurrentDrive_CheckedChanged);
            // 
            // checkBoxIncludeSubdirs
            // 
            this.checkBoxIncludeSubdirs.AutoSize = true;
            this.checkBoxIncludeSubdirs.Location = new System.Drawing.Point(135, 46);
            this.checkBoxIncludeSubdirs.Name = "checkBoxIncludeSubdirs";
            this.checkBoxIncludeSubdirs.Size = new System.Drawing.Size(126, 17);
            this.checkBoxIncludeSubdirs.TabIndex = 2;
            this.checkBoxIncludeSubdirs.Text = "And all subdirectories";
            this.checkBoxIncludeSubdirs.UseVisualStyleBackColor = true;
            // 
            // textBoxCurrentDirectory
            // 
            this.textBoxCurrentDirectory.Location = new System.Drawing.Point(135, 19);
            this.textBoxCurrentDirectory.Name = "textBoxCurrentDirectory";
            this.textBoxCurrentDirectory.Size = new System.Drawing.Size(221, 20);
            this.textBoxCurrentDirectory.TabIndex = 1;
            // 
            // radioButtonCurrentDirectory
            // 
            this.radioButtonCurrentDirectory.AutoSize = true;
            this.radioButtonCurrentDirectory.Location = new System.Drawing.Point(7, 20);
            this.radioButtonCurrentDirectory.Name = "radioButtonCurrentDirectory";
            this.radioButtonCurrentDirectory.Size = new System.Drawing.Size(102, 17);
            this.radioButtonCurrentDirectory.TabIndex = 0;
            this.radioButtonCurrentDirectory.TabStop = true;
            this.radioButtonCurrentDirectory.Text = "Current directory";
            this.radioButtonCurrentDirectory.UseVisualStyleBackColor = true;
            this.radioButtonCurrentDirectory.CheckedChanged += new System.EventHandler(this.radioButtonCurrentDirectory_CheckedChanged);
            // 
            // textBoxFileMask
            // 
            this.textBoxFileMask.Location = new System.Drawing.Point(9, 7);
            this.textBoxFileMask.Name = "textBoxFileMask";
            this.textBoxFileMask.Size = new System.Drawing.Size(351, 20);
            this.textBoxFileMask.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBoxIgnoreAttributes);
            this.tabPage2.Controls.Add(this.flagBoxFileAttributes);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(368, 215);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "File attributes";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBoxIgnoreAttributes
            // 
            this.checkBoxIgnoreAttributes.AutoSize = true;
            this.checkBoxIgnoreAttributes.Location = new System.Drawing.Point(247, 7);
            this.checkBoxIgnoreAttributes.Name = "checkBoxIgnoreAttributes";
            this.checkBoxIgnoreAttributes.Size = new System.Drawing.Size(56, 17);
            this.checkBoxIgnoreAttributes.TabIndex = 1;
            this.checkBoxIgnoreAttributes.Text = "Ignore";
            this.checkBoxIgnoreAttributes.UseVisualStyleBackColor = true;
            this.checkBoxIgnoreAttributes.CheckedChanged += new System.EventHandler(this.checkBoxIgnoreAttributes_CheckedChanged);
            // 
            // flagBoxFileAttributes
            // 
            this.flagBoxFileAttributes.Location = new System.Drawing.Point(9, 7);
            this.flagBoxFileAttributes.Name = "flagBoxFileAttributes";
            this.flagBoxFileAttributes.Size = new System.Drawing.Size(231, 200);
            this.flagBoxFileAttributes.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.checkBoxIgnoreSize);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.textBoxSizeMaximum);
            this.tabPage3.Controls.Add(this.textBoxSizeMinimum);
            this.tabPage3.Controls.Add(this.comboBoxSizeCriteria);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(368, 215);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "File size";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // checkBoxIgnoreSize
            // 
            this.checkBoxIgnoreSize.AutoSize = true;
            this.checkBoxIgnoreSize.Location = new System.Drawing.Point(7, 9);
            this.checkBoxIgnoreSize.Name = "checkBoxIgnoreSize";
            this.checkBoxIgnoreSize.Size = new System.Drawing.Size(65, 17);
            this.checkBoxIgnoreSize.TabIndex = 0;
            this.checkBoxIgnoreSize.Text = "Any size";
            this.checkBoxIgnoreSize.UseVisualStyleBackColor = true;
            this.checkBoxIgnoreSize.CheckedChanged += new System.EventHandler(this.checkBoxIgnoreSize_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "and";
            // 
            // textBoxSizeMaximum
            // 
            this.textBoxSizeMaximum.Location = new System.Drawing.Point(104, 56);
            this.textBoxSizeMaximum.Name = "textBoxSizeMaximum";
            this.textBoxSizeMaximum.Size = new System.Drawing.Size(195, 20);
            this.textBoxSizeMaximum.TabIndex = 4;
            // 
            // textBoxSizeMinimum
            // 
            this.textBoxSizeMinimum.Location = new System.Drawing.Point(104, 30);
            this.textBoxSizeMinimum.Name = "textBoxSizeMinimum";
            this.textBoxSizeMinimum.Size = new System.Drawing.Size(195, 20);
            this.textBoxSizeMinimum.TabIndex = 2;
            // 
            // comboBoxSizeCriteria
            // 
            this.comboBoxSizeCriteria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSizeCriteria.FormattingEnabled = true;
            this.comboBoxSizeCriteria.Items.AddRange(new object[] {
            ">",
            "<",
            ">=",
            "<=",
            "==",
            "Between"});
            this.comboBoxSizeCriteria.Location = new System.Drawing.Point(7, 30);
            this.comboBoxSizeCriteria.Name = "comboBoxSizeCriteria";
            this.comboBoxSizeCriteria.Size = new System.Drawing.Size(91, 21);
            this.comboBoxSizeCriteria.TabIndex = 1;
            this.comboBoxSizeCriteria.SelectedIndexChanged += new System.EventHandler(this.comboBoxSizeCriteria_SelectedIndexChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox3);
            this.tabPage4.Controls.Add(this.groupBox2);
            this.tabPage4.Controls.Add(this.groupBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(368, 215);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "File time";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dateTimePickerAccessEnd);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.dateTimePickerAccessBegin);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.checkBoxIgnoreTimeAccess);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 144);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(368, 68);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Access time";
            // 
            // dateTimePickerAccessEnd
            // 
            this.dateTimePickerAccessEnd.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dateTimePickerAccessEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerAccessEnd.Location = new System.Drawing.Point(230, 42);
            this.dateTimePickerAccessEnd.Name = "dateTimePickerAccessEnd";
            this.dateTimePickerAccessEnd.Size = new System.Drawing.Size(130, 20);
            this.dateTimePickerAccessEnd.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(200, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "and";
            // 
            // dateTimePickerAccessBegin
            // 
            this.dateTimePickerAccessBegin.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dateTimePickerAccessBegin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerAccessBegin.Location = new System.Drawing.Point(64, 42);
            this.dateTimePickerAccessBegin.Name = "dateTimePickerAccessBegin";
            this.dateTimePickerAccessBegin.Size = new System.Drawing.Size(130, 20);
            this.dateTimePickerAccessBegin.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Between";
            // 
            // checkBoxIgnoreTimeAccess
            // 
            this.checkBoxIgnoreTimeAccess.AutoSize = true;
            this.checkBoxIgnoreTimeAccess.Location = new System.Drawing.Point(9, 22);
            this.checkBoxIgnoreTimeAccess.Name = "checkBoxIgnoreTimeAccess";
            this.checkBoxIgnoreTimeAccess.Size = new System.Drawing.Size(66, 17);
            this.checkBoxIgnoreTimeAccess.TabIndex = 0;
            this.checkBoxIgnoreTimeAccess.Text = "Any time";
            this.checkBoxIgnoreTimeAccess.UseVisualStyleBackColor = true;
            this.checkBoxIgnoreTimeAccess.CheckedChanged += new System.EventHandler(this.checkBoxIgnoreTimeAccess_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dateTimePickerModificationEnd);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.dateTimePickerModificationBegin);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.checkBoxIgnoreTimeModification);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 71);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(368, 73);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Modification time";
            // 
            // dateTimePickerModificationEnd
            // 
            this.dateTimePickerModificationEnd.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dateTimePickerModificationEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerModificationEnd.Location = new System.Drawing.Point(230, 40);
            this.dateTimePickerModificationEnd.Name = "dateTimePickerModificationEnd";
            this.dateTimePickerModificationEnd.Size = new System.Drawing.Size(130, 20);
            this.dateTimePickerModificationEnd.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(200, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "and";
            // 
            // dateTimePickerModificationBegin
            // 
            this.dateTimePickerModificationBegin.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dateTimePickerModificationBegin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerModificationBegin.Location = new System.Drawing.Point(64, 40);
            this.dateTimePickerModificationBegin.Name = "dateTimePickerModificationBegin";
            this.dateTimePickerModificationBegin.Size = new System.Drawing.Size(130, 20);
            this.dateTimePickerModificationBegin.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Between";
            // 
            // checkBoxIgnoreTimeModification
            // 
            this.checkBoxIgnoreTimeModification.AutoSize = true;
            this.checkBoxIgnoreTimeModification.Location = new System.Drawing.Point(9, 20);
            this.checkBoxIgnoreTimeModification.Name = "checkBoxIgnoreTimeModification";
            this.checkBoxIgnoreTimeModification.Size = new System.Drawing.Size(66, 17);
            this.checkBoxIgnoreTimeModification.TabIndex = 0;
            this.checkBoxIgnoreTimeModification.Text = "Any time";
            this.checkBoxIgnoreTimeModification.UseVisualStyleBackColor = true;
            this.checkBoxIgnoreTimeModification.CheckedChanged += new System.EventHandler(this.checkBoxIgnoreTimeModification_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dateTimePickerCreateEnd);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dateTimePickerCreateBegin);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.checkBoxIgnoreTimeCreate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(368, 71);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Create time";
            // 
            // dateTimePickerCreateEnd
            // 
            this.dateTimePickerCreateEnd.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dateTimePickerCreateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerCreateEnd.Location = new System.Drawing.Point(230, 40);
            this.dateTimePickerCreateEnd.Name = "dateTimePickerCreateEnd";
            this.dateTimePickerCreateEnd.Size = new System.Drawing.Size(130, 20);
            this.dateTimePickerCreateEnd.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(200, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "and";
            // 
            // dateTimePickerCreateBegin
            // 
            this.dateTimePickerCreateBegin.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dateTimePickerCreateBegin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerCreateBegin.Location = new System.Drawing.Point(64, 40);
            this.dateTimePickerCreateBegin.Name = "dateTimePickerCreateBegin";
            this.dateTimePickerCreateBegin.Size = new System.Drawing.Size(130, 20);
            this.dateTimePickerCreateBegin.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Between";
            // 
            // checkBoxIgnoreTimeCreate
            // 
            this.checkBoxIgnoreTimeCreate.AutoSize = true;
            this.checkBoxIgnoreTimeCreate.Location = new System.Drawing.Point(9, 20);
            this.checkBoxIgnoreTimeCreate.Name = "checkBoxIgnoreTimeCreate";
            this.checkBoxIgnoreTimeCreate.Size = new System.Drawing.Size(66, 17);
            this.checkBoxIgnoreTimeCreate.TabIndex = 0;
            this.checkBoxIgnoreTimeCreate.Text = "Any time";
            this.checkBoxIgnoreTimeCreate.UseVisualStyleBackColor = true;
            this.checkBoxIgnoreTimeCreate.CheckedChanged += new System.EventHandler(this.checkBoxIgnoreTimeCreate_CheckedChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(216, 247);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(297, 247);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonSaveDefault
            // 
            this.buttonSaveDefault.Location = new System.Drawing.Point(4, 247);
            this.buttonSaveDefault.Name = "buttonSaveDefault";
            this.buttonSaveDefault.Size = new System.Drawing.Size(117, 23);
            this.buttonSaveDefault.TabIndex = 1;
            this.buttonSaveDefault.Text = "Save as default";
            this.buttonSaveDefault.UseVisualStyleBackColor = true;
            this.buttonSaveDefault.Click += new System.EventHandler(this.buttonSaveDefault_Click);
            // 
            // DirectoryListFilterDialog
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(376, 278);
            this.Controls.Add(this.buttonSaveDefault);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "DirectoryListFilterDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DirectoryListFilterDialog";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBoxFileMask;
        private FlagBox flagBoxFileAttributes;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ComboBox comboBoxSizeCriteria;
        private System.Windows.Forms.TextBox textBoxSizeMinimum;
        private System.Windows.Forms.TextBox textBoxSizeMaximum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.CheckBox checkBoxIgnoreAttributes;
        private System.Windows.Forms.CheckBox checkBoxIgnoreSize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxIgnoreTimeCreate;
        private System.Windows.Forms.DateTimePicker dateTimePickerCreateBegin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dateTimePickerCreateEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePickerModificationEnd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePickerModificationBegin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxIgnoreTimeModification;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DateTimePicker dateTimePickerAccessEnd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateTimePickerAccessBegin;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBoxIgnoreTimeAccess;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSaveDefault;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButtonCurrentDirectory;
        private System.Windows.Forms.CheckBox checkBoxIncludeSubdirs;
        private System.Windows.Forms.RadioButton radioButtonCurrentDrive;
        private System.Windows.Forms.RadioButton radioButtonLocalDrives;
        private System.Windows.Forms.CheckBox checkBoxRemovableDrives;
        private System.Windows.Forms.CheckBox checkBoxFixedDrives;
        private System.Windows.Forms.CheckBox checkBoxNetworkDrives;
        public System.Windows.Forms.TextBox textBoxCurrentDirectory;

    }
}