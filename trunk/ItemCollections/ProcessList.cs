using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace netCommander
{
    class ProcessList : FileCollectionBase
    {
        private readonly string SORT_NAME = Options.GetLiteral(Options.LANG_PROCESS_NAME);
        private readonly string SORT_ID = Options.GetLiteral(Options.LANG_PROCESS_ID);
        private readonly string SORT_CPU_TIME = Options.GetLiteral(Options.LANG_TOTAL_CPU_TIME);
        private readonly string SORT_WORKING_SET = Options.GetLiteral(Options.LANG_WORKING_SET);

        private PanelCommandProcessInfo cmd_process_info = new PanelCommandProcessInfo();
        private PanelCommandTerminateProcess cmd_terminate = new PanelCommandTerminateProcess();

        private List<Process> intern_list;
        private IComparer<Process> internal_comparer;
        private string comp_name;

        public ProcessList(int sort_index, bool sort_reverse, string computer_name)
            : base(sort_index, sort_reverse)
        {
            internal_comparer = new InternalComparer(sort_index, sort_reverse);
            intern_list = new List<Process>();
            comp_name = computer_name;

            AvailableCommands.Add(cmd_process_info);
            AvailableCommands.Add(cmd_terminate);
        }

        public Process this[int index]
        {
            get
            {
                return intern_list[index];
            }
        }

        protected override void internal_dispose()
        {
            // nothing to do
        }

        protected override void internal_sort(int criteria_index, bool reverse_order)
        {
            IComparer<Process> new_comparer = new InternalComparer(criteria_index, reverse_order);
            intern_list.Sort(new_comparer);
            internal_comparer = new_comparer;
        }

        protected override void internal_refill()
        {
            // automatic updates must be done in main window thread!

            Process[] new_procs_array = string.IsNullOrEmpty(comp_name) ? Process.GetProcesses() : Process.GetProcesses(comp_name);
           
            intern_list.Clear();
            intern_list.AddRange(new_procs_array);
            intern_list.Sort(internal_comparer);
        }

        public override string[] SortCriteriaAvailable
        {
            get { return new string[] { SORT_NAME, SORT_ID, SORT_CPU_TIME, SORT_WORKING_SET }; }
        }

        public override string GetItemDisplayName(int index)
        {
            return intern_list[index].ProcessName;
        }

        public override string GetItemDisplayNameLong(int index)
        {
            try
            {
                return intern_list[index].MainModule.FileName;
            }
            catch (Exception)
            {
                //return ex.Message;
                return "<Unknown>";
            }
        }

        public override string GetItemDisplaySummaryInfo(int index)
        {
            Process p = intern_list[index];
            string total_cpu_time = string.Empty;
            try
            {
                total_cpu_time = p.TotalProcessorTime.ToString();
            }
            catch(Exception)
            {
                total_cpu_time = "<Unknown>";
            }
            string working_set = string.Empty;
            try
            {
                working_set = IOhelper.SizeToString(p.WorkingSet64);
            }
            catch (Exception)
            {
                working_set = "<Unknown>";
            }
            return string.Format
                ("PID:{0} [{1}] [{2}]",
                p.Id,
                total_cpu_time,
                working_set);
        }

        public override string GetSummaryInfo()
        {
            return intern_list.Count.ToString();
        }

        public override string GetSummaryInfo(int[] indices)
        {
            return string.Format
                (Options.GetLiteral(Options.LANG_SELECTED)+": {0}",
                indices.Length);
        }

        public override int ItemCount
        {
            get { return intern_list.Count; }
        }

        public override bool GetItemSelectEnable(int index)
        {
            return true;
        }

        public override bool GetItemIsContainer(int index)
        {
            return false;
        }

        public override void GetChildCollection(int index, ref FileCollectionBase new_collection, ref bool use_new, ref string preferred_focused_text)
        {
            //throw new NotImplementedException();
            // there is not child collection
        }

        public override int FindIndexOfName(string name)
        {
            int ret = -1;
            for (int i = 0; i < ItemCount; i++)
            {
                if (intern_list[i].ProcessName == name)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }

        public override int[] FindItems()
        {
            throw new NotImplementedException();
        }

        public override string GetStatusText()
        {
            return Options.GetLiteral(Options.LANG_PROCESSES);
        }

        public override string GetCommandlineTextShort(int index)
        {
            return this[index].ProcessName;
        }

        public override string GetCommandlineTextLong(int index)
        {
            try
            {
                return this[index].MainModule.FileName;
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
                return string.Empty;
            }
        }

        #region comparer
        private class InternalComparer : IComparer<Process>
        {
            private int sort_criteria = 0;
            private int _order = 1;

            public InternalComparer(int sort_criteria, bool sort_reverse)
            {
                this.sort_criteria = sort_criteria;
                _order = sort_reverse ? -1 : 1;
            }
            #region IComparer<Process> Members

            public int Compare(Process x, Process y)
            {
                //Process px = x.Process;
                //Process py = y.Process;

                    switch (sort_criteria)
                    {
                        case 0: //process name
                            return string.Compare(x.ProcessName, y.ProcessName) * _order;

                        case 1: //process id
                            return (x.Id - y.Id) * _order;

                        case 2: //cpu time
                            TimeSpan tx=TimeSpan.Zero;
                            TimeSpan ty=TimeSpan.Zero;
                            try
                            {
                                tx=x.TotalProcessorTime;
                            }
                            catch(Exception){}
                            try
                            {
                                ty=y.TotalProcessorTime;
                            }
                            catch(Exception){}
                            return TimeSpan.Compare(tx, ty) * _order;

                        case 3: //mem (working set)
                            return ((int)(x.WorkingSet64 - y.WorkingSet64)) * _order;

                        default:
                            return 0;
                    }
                }

            #endregion
        }
        #endregion

        
    }

    
}
