namespace netCommander
{
    partial class ProcessInfoDialog
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageProcessInfo = new System.Windows.Forms.TabPage();
            this.labelProcessStartTime = new System.Windows.Forms.Label();
            this.textBoxProcessStartTime = new System.Windows.Forms.TextBox();
            this.labelProcessMainModule = new System.Windows.Forms.Label();
            this.textBoxProcessMainModule = new System.Windows.Forms.TextBox();
            this.checkBoxProcessResponding = new System.Windows.Forms.CheckBox();
            this.checkBoxProcessPriorityBoostEnable = new System.Windows.Forms.CheckBox();
            this.labelProcessWindowTitle = new System.Windows.Forms.Label();
            this.textBoxProcessWindowTitle = new System.Windows.Forms.TextBox();
            this.labelProcessPriorityClass = new System.Windows.Forms.Label();
            this.textBoxProcessPriorityClass = new System.Windows.Forms.TextBox();
            this.labelProcessId = new System.Windows.Forms.Label();
            this.textBoxProcessId = new System.Windows.Forms.TextBox();
            this.labelProcessName = new System.Windows.Forms.Label();
            this.textBoxProcessName = new System.Windows.Forms.TextBox();
            this.tabPagePerformance = new System.Windows.Forms.TabPage();
            this.labelProcessTotalProcessorTime = new System.Windows.Forms.Label();
            this.labelProcessPrivilegedProcessorTime = new System.Windows.Forms.Label();
            this.textBoxProcessTotalProcessorTime = new System.Windows.Forms.TextBox();
            this.textBoxProcessPrivilegedProcessorTime = new System.Windows.Forms.TextBox();
            this.labelProcessUserProcessorTime = new System.Windows.Forms.Label();
            this.textBoxprocessorUserProcessorTime = new System.Windows.Forms.TextBox();
            this.labelProcessWorkingSet = new System.Windows.Forms.Label();
            this.textBoxProcessWorkingSet = new System.Windows.Forms.TextBox();
            this.labelProcessVirtualMemorySize = new System.Windows.Forms.Label();
            this.textBoxProcessVirtualMemorySize = new System.Windows.Forms.TextBox();
            this.labelProcessPrivateMemorySize = new System.Windows.Forms.Label();
            this.textBoxProcessPrivateMemorySize = new System.Windows.Forms.TextBox();
            this.labelProcessPagedSystemMemorySize = new System.Windows.Forms.Label();
            this.textBoxProcessPagedSystemMemorySize = new System.Windows.Forms.TextBox();
            this.labelProcessPagedMemorySize = new System.Windows.Forms.Label();
            this.textBoxProcessPagedMemorySize = new System.Windows.Forms.TextBox();
            this.labelProcessNonpagedSystemMemorySize = new System.Windows.Forms.Label();
            this.textBoxProcessNonpagedSystemMemorySize = new System.Windows.Forms.TextBox();
            this.tabPageModulesInfo = new System.Windows.Forms.TabPage();
            this.listViewModulesInfo = new System.Windows.Forms.ListView();
            this.columnHeaderModuleFileName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderModuleCompanyName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderModuleProductName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderModuleProductVersion = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderModuleMemorySize = new System.Windows.Forms.ColumnHeader();
            this.tabPageThreadsInfo = new System.Windows.Forms.TabPage();
            this.listViewThreadsInfo = new System.Windows.Forms.ListView();
            this.columnHeaderThreadId = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderThreadState = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderThreadPrioriryLevel = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderThreadCpuLoad = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderStartTime = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderThreadPrivilegedProcessorTime = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderThreadUserProcessorTime = new System.Windows.Forms.ColumnHeader();
            this.buttonClose = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPageProcessInfo.SuspendLayout();
            this.tabPagePerformance.SuspendLayout();
            this.tabPageModulesInfo.SuspendLayout();
            this.tabPageThreadsInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageProcessInfo);
            this.tabControl1.Controls.Add(this.tabPagePerformance);
            this.tabControl1.Controls.Add(this.tabPageModulesInfo);
            this.tabControl1.Controls.Add(this.tabPageThreadsInfo);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(594, 263);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageProcessInfo
            // 
            this.tabPageProcessInfo.Controls.Add(this.labelProcessStartTime);
            this.tabPageProcessInfo.Controls.Add(this.textBoxProcessStartTime);
            this.tabPageProcessInfo.Controls.Add(this.labelProcessMainModule);
            this.tabPageProcessInfo.Controls.Add(this.textBoxProcessMainModule);
            this.tabPageProcessInfo.Controls.Add(this.checkBoxProcessResponding);
            this.tabPageProcessInfo.Controls.Add(this.checkBoxProcessPriorityBoostEnable);
            this.tabPageProcessInfo.Controls.Add(this.labelProcessWindowTitle);
            this.tabPageProcessInfo.Controls.Add(this.textBoxProcessWindowTitle);
            this.tabPageProcessInfo.Controls.Add(this.labelProcessPriorityClass);
            this.tabPageProcessInfo.Controls.Add(this.textBoxProcessPriorityClass);
            this.tabPageProcessInfo.Controls.Add(this.labelProcessId);
            this.tabPageProcessInfo.Controls.Add(this.textBoxProcessId);
            this.tabPageProcessInfo.Controls.Add(this.labelProcessName);
            this.tabPageProcessInfo.Controls.Add(this.textBoxProcessName);
            this.tabPageProcessInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageProcessInfo.Name = "tabPageProcessInfo";
            this.tabPageProcessInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProcessInfo.Size = new System.Drawing.Size(586, 237);
            this.tabPageProcessInfo.TabIndex = 0;
            this.tabPageProcessInfo.Text = "tabPage1";
            this.tabPageProcessInfo.UseVisualStyleBackColor = true;
            // 
            // labelProcessStartTime
            // 
            this.labelProcessStartTime.Location = new System.Drawing.Point(9, 181);
            this.labelProcessStartTime.Name = "labelProcessStartTime";
            this.labelProcessStartTime.Size = new System.Drawing.Size(275, 23);
            this.labelProcessStartTime.TabIndex = 17;
            this.labelProcessStartTime.Text = "start time";
            this.labelProcessStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProcessStartTime
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxProcessStartTime, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxProcessStartTime.Location = new System.Drawing.Point(290, 184);
            this.textBoxProcessStartTime.Name = "textBoxProcessStartTime";
            this.textBoxProcessStartTime.ReadOnly = true;
            this.textBoxProcessStartTime.Size = new System.Drawing.Size(288, 20);
            this.textBoxProcessStartTime.TabIndex = 18;
            // 
            // labelProcessMainModule
            // 
            this.labelProcessMainModule.Location = new System.Drawing.Point(9, 129);
            this.labelProcessMainModule.Name = "labelProcessMainModule";
            this.labelProcessMainModule.Size = new System.Drawing.Size(275, 23);
            this.labelProcessMainModule.TabIndex = 15;
            this.labelProcessMainModule.Text = "main module";
            this.labelProcessMainModule.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProcessMainModule
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxProcessMainModule, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxProcessMainModule.Location = new System.Drawing.Point(290, 132);
            this.textBoxProcessMainModule.Name = "textBoxProcessMainModule";
            this.textBoxProcessMainModule.ReadOnly = true;
            this.textBoxProcessMainModule.Size = new System.Drawing.Size(288, 20);
            this.textBoxProcessMainModule.TabIndex = 16;
            // 
            // checkBoxProcessResponding
            // 
            this.checkBoxProcessResponding.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxProcessResponding.Enabled = false;
            this.checkBoxProcessResponding.Location = new System.Drawing.Point(12, 102);
            this.checkBoxProcessResponding.Name = "checkBoxProcessResponding";
            this.checkBoxProcessResponding.Size = new System.Drawing.Size(292, 24);
            this.checkBoxProcessResponding.TabIndex = 14;
            this.checkBoxProcessResponding.Text = "responding";
            this.checkBoxProcessResponding.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxProcessResponding.UseVisualStyleBackColor = true;
            // 
            // checkBoxProcessPriorityBoostEnable
            // 
            this.checkBoxProcessPriorityBoostEnable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxProcessPriorityBoostEnable.Enabled = false;
            this.checkBoxProcessPriorityBoostEnable.Location = new System.Drawing.Point(12, 81);
            this.checkBoxProcessPriorityBoostEnable.Name = "checkBoxProcessPriorityBoostEnable";
            this.checkBoxProcessPriorityBoostEnable.Size = new System.Drawing.Size(292, 24);
            this.checkBoxProcessPriorityBoostEnable.TabIndex = 13;
            this.checkBoxProcessPriorityBoostEnable.Text = "priority boost";
            this.checkBoxProcessPriorityBoostEnable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxProcessPriorityBoostEnable.UseVisualStyleBackColor = true;
            // 
            // labelProcessWindowTitle
            // 
            this.labelProcessWindowTitle.Location = new System.Drawing.Point(9, 155);
            this.labelProcessWindowTitle.Name = "labelProcessWindowTitle";
            this.labelProcessWindowTitle.Size = new System.Drawing.Size(275, 23);
            this.labelProcessWindowTitle.TabIndex = 11;
            this.labelProcessWindowTitle.Text = "window title";
            this.labelProcessWindowTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProcessWindowTitle
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxProcessWindowTitle, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxProcessWindowTitle.Location = new System.Drawing.Point(290, 158);
            this.textBoxProcessWindowTitle.Name = "textBoxProcessWindowTitle";
            this.textBoxProcessWindowTitle.ReadOnly = true;
            this.textBoxProcessWindowTitle.Size = new System.Drawing.Size(288, 20);
            this.textBoxProcessWindowTitle.TabIndex = 12;
            // 
            // labelProcessPriorityClass
            // 
            this.labelProcessPriorityClass.Location = new System.Drawing.Point(9, 55);
            this.labelProcessPriorityClass.Name = "labelProcessPriorityClass";
            this.labelProcessPriorityClass.Size = new System.Drawing.Size(275, 23);
            this.labelProcessPriorityClass.TabIndex = 9;
            this.labelProcessPriorityClass.Text = "priority";
            this.labelProcessPriorityClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProcessPriorityClass
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxProcessPriorityClass, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxProcessPriorityClass.Location = new System.Drawing.Point(290, 58);
            this.textBoxProcessPriorityClass.Name = "textBoxProcessPriorityClass";
            this.textBoxProcessPriorityClass.ReadOnly = true;
            this.textBoxProcessPriorityClass.Size = new System.Drawing.Size(288, 20);
            this.textBoxProcessPriorityClass.TabIndex = 10;
            // 
            // labelProcessId
            // 
            this.labelProcessId.Location = new System.Drawing.Point(9, 29);
            this.labelProcessId.Name = "labelProcessId";
            this.labelProcessId.Size = new System.Drawing.Size(275, 23);
            this.labelProcessId.TabIndex = 7;
            this.labelProcessId.Text = "process id";
            this.labelProcessId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProcessId
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxProcessId, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxProcessId.Location = new System.Drawing.Point(290, 32);
            this.textBoxProcessId.Name = "textBoxProcessId";
            this.textBoxProcessId.ReadOnly = true;
            this.textBoxProcessId.Size = new System.Drawing.Size(288, 20);
            this.textBoxProcessId.TabIndex = 8;
            // 
            // labelProcessName
            // 
            this.labelProcessName.Location = new System.Drawing.Point(9, 3);
            this.labelProcessName.Name = "labelProcessName";
            this.labelProcessName.Size = new System.Drawing.Size(275, 23);
            this.labelProcessName.TabIndex = 5;
            this.labelProcessName.Text = "process name";
            this.labelProcessName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProcessName
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxProcessName, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxProcessName.Location = new System.Drawing.Point(290, 6);
            this.textBoxProcessName.Name = "textBoxProcessName";
            this.textBoxProcessName.ReadOnly = true;
            this.textBoxProcessName.Size = new System.Drawing.Size(288, 20);
            this.textBoxProcessName.TabIndex = 6;
            // 
            // tabPagePerformance
            // 
            this.tabPagePerformance.Controls.Add(this.labelProcessTotalProcessorTime);
            this.tabPagePerformance.Controls.Add(this.labelProcessPrivilegedProcessorTime);
            this.tabPagePerformance.Controls.Add(this.textBoxProcessTotalProcessorTime);
            this.tabPagePerformance.Controls.Add(this.textBoxProcessPrivilegedProcessorTime);
            this.tabPagePerformance.Controls.Add(this.labelProcessUserProcessorTime);
            this.tabPagePerformance.Controls.Add(this.textBoxprocessorUserProcessorTime);
            this.tabPagePerformance.Controls.Add(this.labelProcessWorkingSet);
            this.tabPagePerformance.Controls.Add(this.textBoxProcessWorkingSet);
            this.tabPagePerformance.Controls.Add(this.labelProcessVirtualMemorySize);
            this.tabPagePerformance.Controls.Add(this.textBoxProcessVirtualMemorySize);
            this.tabPagePerformance.Controls.Add(this.labelProcessPrivateMemorySize);
            this.tabPagePerformance.Controls.Add(this.textBoxProcessPrivateMemorySize);
            this.tabPagePerformance.Controls.Add(this.labelProcessPagedSystemMemorySize);
            this.tabPagePerformance.Controls.Add(this.textBoxProcessPagedSystemMemorySize);
            this.tabPagePerformance.Controls.Add(this.labelProcessPagedMemorySize);
            this.tabPagePerformance.Controls.Add(this.textBoxProcessPagedMemorySize);
            this.tabPagePerformance.Controls.Add(this.labelProcessNonpagedSystemMemorySize);
            this.tabPagePerformance.Controls.Add(this.textBoxProcessNonpagedSystemMemorySize);
            this.tabPagePerformance.Location = new System.Drawing.Point(4, 22);
            this.tabPagePerformance.Name = "tabPagePerformance";
            this.tabPagePerformance.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePerformance.Size = new System.Drawing.Size(586, 237);
            this.tabPagePerformance.TabIndex = 1;
            this.tabPagePerformance.Text = "tabPage2";
            this.tabPagePerformance.UseVisualStyleBackColor = true;
            // 
            // labelProcessTotalProcessorTime
            // 
            this.labelProcessTotalProcessorTime.Location = new System.Drawing.Point(11, 209);
            this.labelProcessTotalProcessorTime.Name = "labelProcessTotalProcessorTime";
            this.labelProcessTotalProcessorTime.Size = new System.Drawing.Size(275, 23);
            this.labelProcessTotalProcessorTime.TabIndex = 15;
            this.labelProcessTotalProcessorTime.Text = "total processor time";
            this.labelProcessTotalProcessorTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelProcessPrivilegedProcessorTime
            // 
            this.labelProcessPrivilegedProcessorTime.Location = new System.Drawing.Point(11, 185);
            this.labelProcessPrivilegedProcessorTime.Name = "labelProcessPrivilegedProcessorTime";
            this.labelProcessPrivilegedProcessorTime.Size = new System.Drawing.Size(275, 23);
            this.labelProcessPrivilegedProcessorTime.TabIndex = 21;
            this.labelProcessPrivilegedProcessorTime.Text = "kernel cpu time";
            this.labelProcessPrivilegedProcessorTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProcessTotalProcessorTime
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxProcessTotalProcessorTime, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxProcessTotalProcessorTime.Location = new System.Drawing.Point(292, 212);
            this.textBoxProcessTotalProcessorTime.Name = "textBoxProcessTotalProcessorTime";
            this.textBoxProcessTotalProcessorTime.ReadOnly = true;
            this.textBoxProcessTotalProcessorTime.Size = new System.Drawing.Size(288, 20);
            this.textBoxProcessTotalProcessorTime.TabIndex = 16;
            // 
            // textBoxProcessPrivilegedProcessorTime
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxProcessPrivilegedProcessorTime, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxProcessPrivilegedProcessorTime.Location = new System.Drawing.Point(292, 188);
            this.textBoxProcessPrivilegedProcessorTime.Name = "textBoxProcessPrivilegedProcessorTime";
            this.textBoxProcessPrivilegedProcessorTime.ReadOnly = true;
            this.textBoxProcessPrivilegedProcessorTime.Size = new System.Drawing.Size(288, 20);
            this.textBoxProcessPrivilegedProcessorTime.TabIndex = 22;
            // 
            // labelProcessUserProcessorTime
            // 
            this.labelProcessUserProcessorTime.Location = new System.Drawing.Point(11, 159);
            this.labelProcessUserProcessorTime.Name = "labelProcessUserProcessorTime";
            this.labelProcessUserProcessorTime.Size = new System.Drawing.Size(275, 23);
            this.labelProcessUserProcessorTime.TabIndex = 19;
            this.labelProcessUserProcessorTime.Text = "user cpu time";
            this.labelProcessUserProcessorTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxprocessorUserProcessorTime
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxprocessorUserProcessorTime, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxprocessorUserProcessorTime.Location = new System.Drawing.Point(292, 162);
            this.textBoxprocessorUserProcessorTime.Name = "textBoxprocessorUserProcessorTime";
            this.textBoxprocessorUserProcessorTime.ReadOnly = true;
            this.textBoxprocessorUserProcessorTime.Size = new System.Drawing.Size(288, 20);
            this.textBoxprocessorUserProcessorTime.TabIndex = 20;
            // 
            // labelProcessWorkingSet
            // 
            this.labelProcessWorkingSet.Location = new System.Drawing.Point(11, 133);
            this.labelProcessWorkingSet.Name = "labelProcessWorkingSet";
            this.labelProcessWorkingSet.Size = new System.Drawing.Size(275, 23);
            this.labelProcessWorkingSet.TabIndex = 17;
            this.labelProcessWorkingSet.Text = "working set";
            this.labelProcessWorkingSet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProcessWorkingSet
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxProcessWorkingSet, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxProcessWorkingSet.Location = new System.Drawing.Point(292, 136);
            this.textBoxProcessWorkingSet.Name = "textBoxProcessWorkingSet";
            this.textBoxProcessWorkingSet.ReadOnly = true;
            this.textBoxProcessWorkingSet.Size = new System.Drawing.Size(288, 20);
            this.textBoxProcessWorkingSet.TabIndex = 18;
            // 
            // labelProcessVirtualMemorySize
            // 
            this.labelProcessVirtualMemorySize.Location = new System.Drawing.Point(11, 107);
            this.labelProcessVirtualMemorySize.Name = "labelProcessVirtualMemorySize";
            this.labelProcessVirtualMemorySize.Size = new System.Drawing.Size(275, 23);
            this.labelProcessVirtualMemorySize.TabIndex = 15;
            this.labelProcessVirtualMemorySize.Text = "virtual memory";
            this.labelProcessVirtualMemorySize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProcessVirtualMemorySize
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxProcessVirtualMemorySize, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxProcessVirtualMemorySize.Location = new System.Drawing.Point(292, 110);
            this.textBoxProcessVirtualMemorySize.Name = "textBoxProcessVirtualMemorySize";
            this.textBoxProcessVirtualMemorySize.ReadOnly = true;
            this.textBoxProcessVirtualMemorySize.Size = new System.Drawing.Size(288, 20);
            this.textBoxProcessVirtualMemorySize.TabIndex = 16;
            // 
            // labelProcessPrivateMemorySize
            // 
            this.labelProcessPrivateMemorySize.Location = new System.Drawing.Point(11, 81);
            this.labelProcessPrivateMemorySize.Name = "labelProcessPrivateMemorySize";
            this.labelProcessPrivateMemorySize.Size = new System.Drawing.Size(275, 23);
            this.labelProcessPrivateMemorySize.TabIndex = 13;
            this.labelProcessPrivateMemorySize.Text = "private memory";
            this.labelProcessPrivateMemorySize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProcessPrivateMemorySize
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxProcessPrivateMemorySize, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxProcessPrivateMemorySize.Location = new System.Drawing.Point(292, 84);
            this.textBoxProcessPrivateMemorySize.Name = "textBoxProcessPrivateMemorySize";
            this.textBoxProcessPrivateMemorySize.ReadOnly = true;
            this.textBoxProcessPrivateMemorySize.Size = new System.Drawing.Size(288, 20);
            this.textBoxProcessPrivateMemorySize.TabIndex = 14;
            // 
            // labelProcessPagedSystemMemorySize
            // 
            this.labelProcessPagedSystemMemorySize.Location = new System.Drawing.Point(11, 55);
            this.labelProcessPagedSystemMemorySize.Name = "labelProcessPagedSystemMemorySize";
            this.labelProcessPagedSystemMemorySize.Size = new System.Drawing.Size(275, 23);
            this.labelProcessPagedSystemMemorySize.TabIndex = 11;
            this.labelProcessPagedSystemMemorySize.Text = "paged system memory";
            this.labelProcessPagedSystemMemorySize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProcessPagedSystemMemorySize
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxProcessPagedSystemMemorySize, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxProcessPagedSystemMemorySize.Location = new System.Drawing.Point(292, 58);
            this.textBoxProcessPagedSystemMemorySize.Name = "textBoxProcessPagedSystemMemorySize";
            this.textBoxProcessPagedSystemMemorySize.ReadOnly = true;
            this.textBoxProcessPagedSystemMemorySize.Size = new System.Drawing.Size(288, 20);
            this.textBoxProcessPagedSystemMemorySize.TabIndex = 12;
            // 
            // labelProcessPagedMemorySize
            // 
            this.labelProcessPagedMemorySize.Location = new System.Drawing.Point(11, 29);
            this.labelProcessPagedMemorySize.Name = "labelProcessPagedMemorySize";
            this.labelProcessPagedMemorySize.Size = new System.Drawing.Size(275, 23);
            this.labelProcessPagedMemorySize.TabIndex = 9;
            this.labelProcessPagedMemorySize.Text = "paged memory";
            this.labelProcessPagedMemorySize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProcessPagedMemorySize
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxProcessPagedMemorySize, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxProcessPagedMemorySize.Location = new System.Drawing.Point(292, 32);
            this.textBoxProcessPagedMemorySize.Name = "textBoxProcessPagedMemorySize";
            this.textBoxProcessPagedMemorySize.ReadOnly = true;
            this.textBoxProcessPagedMemorySize.Size = new System.Drawing.Size(288, 20);
            this.textBoxProcessPagedMemorySize.TabIndex = 10;
            // 
            // labelProcessNonpagedSystemMemorySize
            // 
            this.labelProcessNonpagedSystemMemorySize.Location = new System.Drawing.Point(11, 3);
            this.labelProcessNonpagedSystemMemorySize.Name = "labelProcessNonpagedSystemMemorySize";
            this.labelProcessNonpagedSystemMemorySize.Size = new System.Drawing.Size(275, 23);
            this.labelProcessNonpagedSystemMemorySize.TabIndex = 7;
            this.labelProcessNonpagedSystemMemorySize.Text = "nonpaged system memory";
            this.labelProcessNonpagedSystemMemorySize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProcessNonpagedSystemMemorySize
            // 
            this.errorProvider1.SetIconAlignment(this.textBoxProcessNonpagedSystemMemorySize, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.textBoxProcessNonpagedSystemMemorySize.Location = new System.Drawing.Point(292, 6);
            this.textBoxProcessNonpagedSystemMemorySize.Name = "textBoxProcessNonpagedSystemMemorySize";
            this.textBoxProcessNonpagedSystemMemorySize.ReadOnly = true;
            this.textBoxProcessNonpagedSystemMemorySize.Size = new System.Drawing.Size(288, 20);
            this.textBoxProcessNonpagedSystemMemorySize.TabIndex = 8;
            // 
            // tabPageModulesInfo
            // 
            this.tabPageModulesInfo.Controls.Add(this.listViewModulesInfo);
            this.tabPageModulesInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageModulesInfo.Name = "tabPageModulesInfo";
            this.tabPageModulesInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageModulesInfo.Size = new System.Drawing.Size(586, 237);
            this.tabPageModulesInfo.TabIndex = 2;
            this.tabPageModulesInfo.Text = "tabPage1";
            this.tabPageModulesInfo.UseVisualStyleBackColor = true;
            // 
            // listViewModulesInfo
            // 
            this.listViewModulesInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderModuleFileName,
            this.columnHeaderModuleCompanyName,
            this.columnHeaderModuleProductName,
            this.columnHeaderModuleProductVersion,
            this.columnHeaderModuleMemorySize});
            this.listViewModulesInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.listViewModulesInfo.FullRowSelect = true;
            this.listViewModulesInfo.GridLines = true;
            this.listViewModulesInfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.errorProvider1.SetIconAlignment(this.listViewModulesInfo, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.listViewModulesInfo.Location = new System.Drawing.Point(29, 3);
            this.listViewModulesInfo.Name = "listViewModulesInfo";
            this.listViewModulesInfo.Size = new System.Drawing.Size(554, 231);
            this.listViewModulesInfo.TabIndex = 0;
            this.listViewModulesInfo.UseCompatibleStateImageBehavior = false;
            this.listViewModulesInfo.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderModuleFileName
            // 
            this.columnHeaderModuleFileName.Text = "file name";
            this.columnHeaderModuleFileName.Width = 190;
            // 
            // columnHeaderModuleCompanyName
            // 
            this.columnHeaderModuleCompanyName.Text = "company name";
            this.columnHeaderModuleCompanyName.Width = 115;
            // 
            // columnHeaderModuleProductName
            // 
            this.columnHeaderModuleProductName.Text = "product name";
            this.columnHeaderModuleProductName.Width = 113;
            // 
            // columnHeaderModuleProductVersion
            // 
            this.columnHeaderModuleProductVersion.Text = "version";
            // 
            // columnHeaderModuleMemorySize
            // 
            this.columnHeaderModuleMemorySize.Text = "memory";
            this.columnHeaderModuleMemorySize.Width = 68;
            // 
            // tabPageThreadsInfo
            // 
            this.tabPageThreadsInfo.Controls.Add(this.listViewThreadsInfo);
            this.tabPageThreadsInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageThreadsInfo.Name = "tabPageThreadsInfo";
            this.tabPageThreadsInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageThreadsInfo.Size = new System.Drawing.Size(586, 237);
            this.tabPageThreadsInfo.TabIndex = 3;
            this.tabPageThreadsInfo.Text = "tabPage1";
            this.tabPageThreadsInfo.UseVisualStyleBackColor = true;
            // 
            // listViewThreadsInfo
            // 
            this.listViewThreadsInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderThreadId,
            this.columnHeaderThreadState,
            this.columnHeaderThreadPrioriryLevel,
            this.columnHeaderThreadCpuLoad,
            this.columnHeaderStartTime,
            this.columnHeaderThreadPrivilegedProcessorTime,
            this.columnHeaderThreadUserProcessorTime});
            this.listViewThreadsInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.listViewThreadsInfo.FullRowSelect = true;
            this.listViewThreadsInfo.GridLines = true;
            this.listViewThreadsInfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.errorProvider1.SetIconAlignment(this.listViewThreadsInfo, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.listViewThreadsInfo.Location = new System.Drawing.Point(29, 3);
            this.listViewThreadsInfo.Name = "listViewThreadsInfo";
            this.listViewThreadsInfo.Size = new System.Drawing.Size(554, 231);
            this.listViewThreadsInfo.TabIndex = 1;
            this.listViewThreadsInfo.UseCompatibleStateImageBehavior = false;
            this.listViewThreadsInfo.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderThreadId
            // 
            this.columnHeaderThreadId.Text = "id";
            this.columnHeaderThreadId.Width = 58;
            // 
            // columnHeaderThreadState
            // 
            this.columnHeaderThreadState.Text = "state";
            this.columnHeaderThreadState.Width = 140;
            // 
            // columnHeaderThreadPrioriryLevel
            // 
            this.columnHeaderThreadPrioriryLevel.Text = "priority";
            this.columnHeaderThreadPrioriryLevel.Width = 73;
            // 
            // columnHeaderThreadCpuLoad
            // 
            this.columnHeaderThreadCpuLoad.Text = "cpu load";
            this.columnHeaderThreadCpuLoad.Width = 70;
            // 
            // columnHeaderStartTime
            // 
            this.columnHeaderStartTime.Text = "start time";
            // 
            // columnHeaderThreadPrivilegedProcessorTime
            // 
            this.columnHeaderThreadPrivilegedProcessorTime.Text = "kernel time";
            this.columnHeaderThreadPrivilegedProcessorTime.Width = 85;
            // 
            // columnHeaderThreadUserProcessorTime
            // 
            this.columnHeaderThreadUserProcessorTime.Text = "user time";
            this.columnHeaderThreadUserProcessorTime.Width = 74;
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(515, 269);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "close";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ProcessInfoDialog
            // 
            this.AcceptButton = this.buttonClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(594, 304);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ProcessInfoDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProcessInfoDialog";
            this.Load += new System.EventHandler(this.ProcessInfoDialog_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageProcessInfo.ResumeLayout(false);
            this.tabPageProcessInfo.PerformLayout();
            this.tabPagePerformance.ResumeLayout(false);
            this.tabPagePerformance.PerformLayout();
            this.tabPageModulesInfo.ResumeLayout(false);
            this.tabPageThreadsInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageProcessInfo;
        private System.Windows.Forms.TabPage tabPagePerformance;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelProcessName;
        private System.Windows.Forms.TextBox textBoxProcessName;
        private System.Windows.Forms.Label labelProcessId;
        private System.Windows.Forms.TextBox textBoxProcessId;
        private System.Windows.Forms.Label labelProcessPriorityClass;
        private System.Windows.Forms.TextBox textBoxProcessPriorityClass;
        private System.Windows.Forms.Label labelProcessWindowTitle;
        private System.Windows.Forms.TextBox textBoxProcessWindowTitle;
        private System.Windows.Forms.CheckBox checkBoxProcessPriorityBoostEnable;
        private System.Windows.Forms.CheckBox checkBoxProcessResponding;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label labelProcessMainModule;
        private System.Windows.Forms.TextBox textBoxProcessMainModule;
        private System.Windows.Forms.Label labelProcessNonpagedSystemMemorySize;
        private System.Windows.Forms.TextBox textBoxProcessNonpagedSystemMemorySize;
        private System.Windows.Forms.Label labelProcessPagedMemorySize;
        private System.Windows.Forms.TextBox textBoxProcessPagedMemorySize;
        private System.Windows.Forms.Label labelProcessPagedSystemMemorySize;
        private System.Windows.Forms.TextBox textBoxProcessPagedSystemMemorySize;
        private System.Windows.Forms.Label labelProcessPrivateMemorySize;
        private System.Windows.Forms.TextBox textBoxProcessPrivateMemorySize;
        private System.Windows.Forms.Label labelProcessVirtualMemorySize;
        private System.Windows.Forms.TextBox textBoxProcessVirtualMemorySize;
        private System.Windows.Forms.Label labelProcessWorkingSet;
        private System.Windows.Forms.TextBox textBoxProcessWorkingSet;
        private System.Windows.Forms.Label labelProcessPrivilegedProcessorTime;
        private System.Windows.Forms.TextBox textBoxProcessPrivilegedProcessorTime;
        private System.Windows.Forms.Label labelProcessUserProcessorTime;
        private System.Windows.Forms.TextBox textBoxprocessorUserProcessorTime;
        private System.Windows.Forms.Label labelProcessStartTime;
        private System.Windows.Forms.TextBox textBoxProcessStartTime;
        private System.Windows.Forms.Label labelProcessTotalProcessorTime;
        private System.Windows.Forms.TextBox textBoxProcessTotalProcessorTime;
        private System.Windows.Forms.TabPage tabPageModulesInfo;
        private System.Windows.Forms.ListView listViewModulesInfo;
        private System.Windows.Forms.ColumnHeader columnHeaderModuleFileName;
        private System.Windows.Forms.ColumnHeader columnHeaderModuleCompanyName;
        private System.Windows.Forms.ColumnHeader columnHeaderModuleProductName;
        private System.Windows.Forms.ColumnHeader columnHeaderModuleProductVersion;
        private System.Windows.Forms.ColumnHeader columnHeaderModuleMemorySize;
        private System.Windows.Forms.TabPage tabPageThreadsInfo;
        private System.Windows.Forms.ListView listViewThreadsInfo;
        private System.Windows.Forms.ColumnHeader columnHeaderThreadId;
        private System.Windows.Forms.ColumnHeader columnHeaderThreadPrioriryLevel;
        private System.Windows.Forms.ColumnHeader columnHeaderThreadPrivilegedProcessorTime;
        private System.Windows.Forms.ColumnHeader columnHeaderThreadUserProcessorTime;
        private System.Windows.Forms.ColumnHeader columnHeaderThreadCpuLoad;
        private System.Windows.Forms.ColumnHeader columnHeaderThreadState;
        private System.Windows.Forms.ColumnHeader columnHeaderStartTime;
    }
}