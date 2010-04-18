using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace netCommander
{
    public partial class ProcessInfoDialog : Form
    {
        public ProcessInfoDialog()
        {
            InitializeComponent();

            set_lang();
        }

        private void set_lang()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            tabPageModulesInfo.Text = Options.GetLiteral(Options.LANG_PROCESS_LOADED_MODULES_INFO);
            tabPagePerformance.Text = Options.GetLiteral(Options.LANG_PROCESS_MEM_AND_CPU_USAGE);
            tabPageProcessInfo.Text = Options.GetLiteral(Options.LANG_PROCESS_INFO);
            tabPageThreadsInfo.Text = Options.GetLiteral(Options.LANG_PROCESS_THREADS_INFO);

            labelProcessId.Text = Options.GetLiteral(Options.LANG_PROCESS_ID);
            labelProcessMainModule.Text = Options.GetLiteral(Options.LANG_PROCESS_MAIN_MODULE);
            labelProcessName.Text = Options.GetLiteral(Options.LANG_PROCESS_NAME);
            labelProcessNonpagedSystemMemorySize.Text = Options.GetLiteral(Options.LANG_NONPAGED_SYSTEM_MEMORY);
            labelProcessPagedMemorySize.Text = Options.GetLiteral(Options.LANG_PAGED_MEMORY);
            labelProcessPagedSystemMemorySize.Text = Options.GetLiteral(Options.LANG_PAGED_SYSTEM_MEMORY);
            labelProcessPriorityClass.Text = Options.GetLiteral(Options.LANG_PRIORITY);
            labelProcessPrivateMemorySize.Text = Options.GetLiteral(Options.LANG_PRIVATE_MEMORY);
            labelProcessPrivilegedProcessorTime.Text = Options.GetLiteral(Options.LANG_PRIVILEGED_CPU_TIME);
            labelProcessStartTime.Text = Options.GetLiteral(Options.LANG_START_TIME);
            labelProcessTotalProcessorTime.Text = Options.GetLiteral(Options.LANG_TOTAL_CPU_TIME);
            labelProcessUserProcessorTime.Text = Options.GetLiteral(Options.LANG_USER_CPU_TIME);
            labelProcessVirtualMemorySize.Text = Options.GetLiteral(Options.LANG_VIRTUAL_MEMORY);
            labelProcessWindowTitle.Text = Options.GetLiteral(Options.LANG_PROCESS_WINDOW_TITLE);
            labelProcessWorkingSet.Text = Options.GetLiteral(Options.LANG_WORKING_SET);

            checkBoxProcessPriorityBoostEnable.Text = Options.GetLiteral(Options.LANG_PRIORITY_BOOST);
            checkBoxProcessResponding.Text = Options.GetLiteral(Options.LANG_PROCESS_RESPONDING);

            buttonClose.Text = Options.GetLiteral(Options.LANG_CLOSE);

            columnHeaderModuleCompanyName.Text = Options.GetLiteral(Options.LANG_MODULE_COMPANY_NAME);
            columnHeaderModuleFileName.Text = Options.GetLiteral(Options.LANG_FILE_NAME);
            columnHeaderModuleMemorySize.Text = Options.GetLiteral(Options.LANG_MEMORY);
            columnHeaderModuleProductName.Text = Options.GetLiteral(Options.LANG_MODULE_PRODUCT_NAME);
            columnHeaderModuleProductVersion.Text = Options.GetLiteral(Options.LANG_VERSION);
            columnHeaderStartTime.Text = Options.GetLiteral(Options.LANG_START_TIME);
            columnHeaderThreadCpuLoad.Text = Options.GetLiteral(Options.LANG_CPU_LOAD);
            columnHeaderThreadId.Text = Options.GetLiteral(Options.LANG_THREAD_ID);
            columnHeaderThreadPrioriryLevel.Text = Options.GetLiteral(Options.LANG_PRIORITY);
            columnHeaderThreadPrivilegedProcessorTime.Text = Options.GetLiteral(Options.LANG_PRIVILEGED_CPU_TIME);
            columnHeaderThreadState.Text = Options.GetLiteral(Options.LANG_STATE);
            columnHeaderThreadUserProcessorTime.Text = Options.GetLiteral(Options.LANG_USER_CPU_TIME);
        }

        public void Fill(Process p)
        {
            Text = p.ProcessName;
            textBoxProcessId.Text = p.Id.ToString();
            textBoxProcessName.Text = p.ProcessName;
            try
            {
                textBoxProcessPriorityClass.Text = p.PriorityClass.ToString();
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBoxProcessPriorityClass, ex.Message);
            }
            try
            {
                textBoxProcessWindowTitle.Text = p.MainWindowTitle;
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBoxProcessWindowTitle, ex.Message);
            }
            try
            {
                checkBoxProcessPriorityBoostEnable.Checked = p.PriorityBoostEnabled;
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(checkBoxProcessPriorityBoostEnable, ex.Message);
            }
            try
            {
                checkBoxProcessResponding.Checked = p.Responding;
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(checkBoxProcessResponding, ex.Message);
            }
            try
            {
                textBoxProcessMainModule.Text = p.MainModule.FileName;
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBoxProcessMainModule, ex.Message);
            }
            try
            {
                textBoxProcessNonpagedSystemMemorySize.Text = IOhelper.SizeToString(p.NonpagedSystemMemorySize64);
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBoxProcessNonpagedSystemMemorySize, ex.Message);
            }
            try
            {
                textBoxprocessorUserProcessorTime.Text = p.UserProcessorTime.ToString();
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBoxprocessorUserProcessorTime, ex.Message);
            }
            try
            {
                textBoxProcessPagedMemorySize.Text = IOhelper.SizeToString(p.PagedMemorySize64);
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBoxProcessPagedMemorySize, ex.Message);
            }
            try
            {
                textBoxProcessPagedSystemMemorySize.Text = IOhelper.SizeToString(p.PagedSystemMemorySize64);
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBoxProcessPagedSystemMemorySize, ex.Message);
            }
            try
            {
                textBoxProcessPrivateMemorySize.Text = IOhelper.SizeToString(p.PrivateMemorySize64);
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBoxProcessPrivateMemorySize, ex.Message);
            }
            try
            {
                textBoxProcessPrivilegedProcessorTime.Text = p.PrivilegedProcessorTime.ToString();
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBoxProcessPrivilegedProcessorTime, ex.Message);
            }
            try
            {
                textBoxProcessStartTime.Text = p.StartTime.ToString();
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBoxProcessStartTime, ex.Message);
            }
            try
            {
                TimeSpan time_from_start = DateTime.Now.Subtract(p.StartTime);
                double secs_from_start = time_from_start.TotalSeconds;
                double secs_cpu_time = p.TotalProcessorTime.TotalSeconds;
                double cpu_load = secs_cpu_time / secs_from_start;
                textBoxProcessTotalProcessorTime.Text = string.Format("{0} <{1:P4}>", p.TotalProcessorTime.ToString(), cpu_load);
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBoxProcessTotalProcessorTime, ex.Message);
            }
            try
            {
                textBoxProcessVirtualMemorySize.Text = IOhelper.SizeToString(p.VirtualMemorySize64);
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBoxProcessVirtualMemorySize, ex.Message);
            }
            try
            {
                textBoxProcessWorkingSet.Text = IOhelper.SizeToString(p.WorkingSet64);
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(textBoxProcessWorkingSet, ex.Message);
            }

            try
            {
                ProcessModuleCollection pmc = p.Modules;
                foreach (ProcessModule pm in pmc)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = pm.FileName;
                    lvi.SubItems.Add(pm.FileVersionInfo.CompanyName);
                    lvi.SubItems.Add(pm.FileVersionInfo.ProductName);
                    lvi.SubItems.Add(pm.FileVersionInfo.ProductVersion);
                    lvi.SubItems.Add(IOhelper.SizeToString((long)pm.ModuleMemorySize));
                    listViewModulesInfo.Items.Add(lvi);
                }
                listViewModulesInfo.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(listViewModulesInfo, ex.Message);
            }

            try
            {
                ProcessThreadCollection ptc = p.Threads;
                foreach (ProcessThread pt in ptc)
                {
                    ListViewItem lvi = new ListViewItem();
                    try
                    {
                        lvi.Text = pt.Id.ToString();

                        ThreadState ts = pt.ThreadState;
                        if (ts == ThreadState.Wait)
                        {
                            lvi.SubItems.Add(pt.ThreadState.ToString() + " " + pt.WaitReason.ToString());
                        }
                        else
                        {
                            lvi.SubItems.Add(pt.ThreadState.ToString());
                        }

                        if (pt.ThreadState != ThreadState.Terminated)
                        {
                            lvi.SubItems.Add(pt.PriorityLevel.ToString());

                            //load
                            double total_secs = DateTime.Now.Subtract(pt.StartTime).TotalSeconds;
                            double cpu_secs = pt.TotalProcessorTime.TotalSeconds;
                            double cpu_load = cpu_secs / total_secs;
                            lvi.SubItems.Add(string.Format("{0:P4}", cpu_load));

                            lvi.SubItems.Add(pt.StartTime.ToString());
                            lvi.SubItems.Add(pt.PrivilegedProcessorTime.ToString());
                            lvi.SubItems.Add(pt.UserProcessorTime.ToString());
                        }
                    }
                    catch (Exception ex_int)
                    {
                        lvi.SubItems.Add(ex_int.Message);
                    }
                    listViewThreadsInfo.Items.Add(lvi);
                }
                listViewThreadsInfo.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(listViewThreadsInfo, ex.Message);
            }
        }

        private void ProcessInfoDialog_Load(object sender, EventArgs e)
        {

        }


    }
}
