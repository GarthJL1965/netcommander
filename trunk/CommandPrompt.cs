using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace netCommander
{
    public partial class CommandPrompt : UserControl
    {
        public CommandPrompt()
        {
            InitializeComponent();
            set_lang();
            //Font cmd_font = new Font(FontFamily.GenericMonospace, textBox1.Font.Size, FontStyle.Regular);
            //Font cmd_font = new Font(Options.FontFilePanel, FontStyle.Regular);
            
            //textBox1.Font = cmd_font;
        }

        private void set_lang()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            buttonExecute.Text = Options.GetLiteral(Options.LANG_EXECUTE);
        }

        public string CommandText 
        {
            get
            {
                return textBox1.Text;   
            }
            set
            {
                textBox1.Text = value;
            }
        }

        public void AddCommandChunk(string text)
        {
            textBox1.AppendText(" ");

            if (text.Contains(" "))
            {
                text = '"' + text + '"';
            }

            textBox1.AppendText(text);
        }

        public void Execute()
        {
            intern_execute();
        }

        private void intern_execute()
        {
            string reg_pattern = Options.REGEX_PARSE_COMMAND;
            Regex rex = new Regex(reg_pattern);

            try
            {
                string[] splitted = rex.Split(textBox1.Text, 2);
                if (splitted.Length < 2)
                {
                	throw new ArgumentException(Options.GetLiteral(Options.LANG_CANNOT_PARSE_COMMAND_LINE));
//                  throw new ApplicationException(Options.GetLiteral(Options.LANG_CANNOT_PARSE_COMMAND_LINE));
                }

                string command_name = splitted[1];
                string command_args = splitted.Length > 2 ? splitted[2] : string.Empty;

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.Arguments = command_args;
                psi.CreateNoWindow = false;
                psi.ErrorDialog = false;
                psi.FileName = command_name;
                psi.UseShellExecute = true;
                psi.WorkingDirectory = Directory.GetCurrentDirectory();

                Process p = new Process();
                p.StartInfo = psi;
                p.Start();
            }
            catch (Exception ex)
            {
                Messages.ShowException(ex, string.Format(Options.GetLiteral(Options.LANG_CANNOT_EXECUTE_0), textBox1.Text));
            }
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            intern_execute();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    intern_execute();
                    break;
            }
        }
    }
}
