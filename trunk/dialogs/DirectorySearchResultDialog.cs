using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using netCommander.FileSystemEx;

namespace netCommander
{
    public partial class DirectorySearchResultDialog : Form
    {
        public DirectorySearchResultDialog()
        {
            InitializeComponent();

            //Text = "Find files";

            update_current_dir_invoker = new MethodInvoker_string(update_current_dir);
            add_result_invoker = new MethodInvoker_string_findData(add_result);

            engine.SearchResult += new SearchResultEventHandler(engine_SearchResult);

            set_lang();
        }

        public DirectorySearchResultDialog(DirectoryListFilter filter)
            : this()
        {
            BeginSearch(filter);
        }

        private void set_lang()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            Text = Options.GetLiteral(Options.LANG_FIND_RESULTS);
            columnHeaderAccess.Text = Options.GetLiteral(Options.LANG_ACCESS_TIME);
            columnHeaderAttr.Text = Options.GetLiteral(Options.LANG_FILE_ATTRIBUTES);
            columnHeaderCreate.Text = Options.GetLiteral(Options.LANG_CREATE_TIME);
            columnHeaderModification.Text = Options.GetLiteral(Options.LANG_MODIFICATION_TIME);
            columnHeaderName.Text = Options.GetLiteral(Options.LANG_FILE_NAME);
            columnHeaderSize.Text = Options.GetLiteral(Options.LANG_FILE_SIZE);
            buttonStop.Text = Options.GetLiteral(Options.LANG_ABORT);
            buttonGoto.Text = Options.GetLiteral(Options.LANG_GOTO);
            bClose.Text = Options.GetLiteral(Options.LANG_CLOSE);

        }

        private MethodInvoker_string update_current_dir_invoker;
        private MethodInvoker_string_findData add_result_invoker;

        void engine_SearchResult(object sender, SearchResultEventArgs e)
        {
            try
            {
                switch (e.Reason)
                {
                    case SearchResultReason.ChangeDirectory:
                        if (InvokeRequired)
                        {
                            Invoke(update_current_dir_invoker, new object[] { e.DirectoryPath });
                        }
                        else
                        {
                            update_current_dir(e.DirectoryPath);
                        }
                        break;

                    case SearchResultReason.FindFile:
                        if (InvokeRequired)
                        {
                            Invoke(add_result_invoker, new object[] { e.DirectoryPath, e.Find });
                        }
                        else
                        {
                            add_result(e.DirectoryPath, e.Find);
                        }
                        break;

                    case SearchResultReason.SearchFinish:
                        if (InvokeRequired)
                        {
                            Invoke(update_current_dir_invoker, new object[] { Options.GetLiteral(Options.LANG_DONE) });
                        }
                        else
                        {
                            update_current_dir(Options.GetLiteral(Options.LANG_DONE));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex);
            }
        }

        private DirectorySearchEngine engine = new DirectorySearchEngine();

        public void BeginSearch(DirectoryListFilter filter)
        {
            if (!engine.Active)
            {
                engine.BeginSearch(filter);
            }
        }

        private void update_current_dir(string dir_path)
        {
            toolStripStatusLabelCurrentDir.Text = dir_path;
        }

        private int result_count = 0;
        private string add_lit = Options.GetLiteral(Options.LANG_FOUND_0_FILES);
        private void add_result(string dir_path, WIN32_FIND_DATA result)
        {
            result_count++;
            toolStripStatusLabelCount.Text = string.Format(add_lit, result_count);

            InternalListViewItem new_item = new InternalListViewItem(dir_path, result);
            listViewResult.Items.Add(new_item);
            //listViewResult.EnsureVisible(listViewResult.Items.Count - 1);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            engine.StopSearch();
        }

        private void buttonGoto_Click(object sender, EventArgs e)
        {
            if (engine.Active)
            {
                engine.StopSearch();
            }
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            if (engine.Active)
            {
                engine.StopSearch();
            }
        }

        public string SelectedResult
        {
            get
            {
                if (listViewResult.SelectedIndices.Count > 0)
                {

                    return listViewResult.SelectedItems[0].Text;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private bool last_revers = false;
        private int last_column_clicked = -1;
        private InternalComparer internal_comparer = null;
        private void listViewResult_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (engine.Active)
            {
                return;
            }

            if (last_column_clicked == e.Column)
            {
                last_revers = !last_revers;
            }

            internal_comparer = new InternalComparer(e.Column, last_revers);
            last_column_clicked = e.Column;
            listViewResult.ListViewItemSorter = internal_comparer;
        }

        private class InternalListViewItem : ListViewItem
        {
            public WIN32_FIND_DATA InternalData { get; private set; }

            public InternalListViewItem(string dir_path,WIN32_FIND_DATA data)
            {
                InternalData = data;

                Text = System.IO.Path.Combine(dir_path, data.cFileName);
                SubItems.Add(data.FileSize.ToString("#,##0"));
                SubItems.Add(IOhelper.FileAttributes2String(data.dwFileAttributes));
                SubItems.Add(DateTime.FromFileTime(data.ftCreationTime).ToString());
                SubItems.Add(DateTime.FromFileTime(data.ftLastWriteTime).ToString());
                SubItems.Add(DateTime.FromFileTime(data.ftLastAccessTime).ToString());
            }
        }

        private class InternalComparer : IComparer
        {

            public InternalComparer(int column_index, bool reverse)
            {
                col_index = column_index;
                if (reverse)
                {
                    _order = -1;
                }
            }

            private int col_index = 0;
            private int _order = 1;

            #region IComparer Members

            public int Compare(object x, object y)
            {
                InternalListViewItem item_x = (InternalListViewItem)x;
                InternalListViewItem item_y = (InternalListViewItem)y;
                long long_delta = 0L;

                switch (col_index)
                {
                    case 0:
                        //path name
                        return (string.Compare(item_x.Text, item_y.Text) * _order);

                    case 1:
                        //size
                        long_delta = (long)item_x.InternalData.FileSize - (long)item_y.InternalData.FileSize;
                        if (long_delta > 0L)
                        {
                            return _order;
                        }
                        else if (long_delta < 0L)
                        {
                            return -_order;
                        }
                        else
                        {
                            return 0;
                        }

                    case 2:
                        //attr
                        return (string.Compare(item_x.SubItems[2].Text, item_y.SubItems[2].Text) * _order);

                    case 3:
                        //create
                        long_delta = item_x.InternalData.ftCreationTime - item_y.InternalData.ftCreationTime;
                        if (long_delta > 0L)
                        {
                            return _order;
                        }
                        else if (long_delta < 0L)
                        {
                            return -_order;
                        }
                        else
                        {
                            return 0;
                        }
                        

                    case 4:
                        long_delta = item_x.InternalData.ftLastWriteTime - item_y.InternalData.ftLastWriteTime;
                        if (long_delta > 0L)
                        {
                            return _order;
                        }
                        else if (long_delta < 0L)
                        {
                            return -_order;
                        }
                        else
                        {
                            return 0;
                        }

                    case 5:
                        //access
                        long_delta = item_x.InternalData.ftLastAccessTime - item_y.InternalData.ftLastAccessTime;
                        if (long_delta > 0L)
                        {
                            return _order;
                        }
                        else if (long_delta < 0L)
                        {
                            return -_order;
                        }
                        else
                        {
                            return 0;
                        }
                }

                return 0;
            }

            #endregion
        }

        private void DirectorySearchResultDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (engine.Active)
            {
                engine.StopSearch();
            }
        }

        
    }

    public delegate void MethodInvoker_string(string arg);
    public delegate void MethodInvoker_string_findData(string arg0, WIN32_FIND_DATA arg1);
}
