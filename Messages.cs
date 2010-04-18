using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace netCommander
{
    class Messages
    {
        public static string GetLogfileName()
        {
            string ret = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "netCommander.log");
            return ret;
        }

        public static void WriteToLog(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Options.GetLiteral(Options.LANG_EXCEPTION));
            sb.Append(Environment.NewLine);
            sb.Append(Options.GetLiteral(Options.LANG_SOURCE));
            sb.Append(Environment.NewLine);
            sb.Append(ex.Source);
            sb.Append(Environment.NewLine);
            sb.Append(Options.GetLiteral(Options.LANG_MESSAGE));
            sb.Append(ex.Message);
            sb.Append(Environment.NewLine);
            sb.Append(Options.GetLiteral(Options.LANG_STACK));
            sb.Append(Environment.NewLine);
            sb.Append(ex.StackTrace);
            Messages.WriteToLog(sb.ToString());
        }

        public static void WriteToLog(string message)
        {
            DateTime now = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append('[');
            sb.Append(now.ToShortDateString());
            sb.Append(' ');
            sb.Append(now.ToLongTimeString());
            sb.Append(']');
            sb.Append(Environment.NewLine);
            sb.Append(message);

            File.AppendAllText(Messages.GetLogfileName(), sb.ToString());
        }

        public static void ShowException(Exception ex, string info)
        {
        	if (string.IsNullOrEmpty(info))
            {
                ShowException(ex);
                return;
            }

            string text = string.Empty;
            if (ex != null)
            {
                text = string.Format(Options.GetLiteral(Options.LANG_0_1_SAVE_TO_LOG), info, ex.Message);
            }
            else
            {
                text = string.Format(Options.GetLiteral(Options.LANG_0_SAVE_TO_LOG), info);
            }
            if (MessageBox.Show
                    (text,
                    ex.Source,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                WriteToLog(ex);
            }
        }

        public static void ShowException(Exception ex)
        {
            if (MessageBox.Show(ex.Message + Environment.NewLine + Options.GetLiteral(Options.LANG_SAVE_TO_LOG),
                ex.Source,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                WriteToLog(ex);
            }            
        }

        public static bool ShowExceptionContinue(Exception ex, string info)
        {
            string mes_text = string.Empty;
            string mes_caption = string.Empty;
            if (ex == null)
            {
                mes_text = string.Format(Options.GetLiteral(Options.LANG_0_CONTINUE), info);
                mes_caption = Options.GetLiteral(Options.LANG_APP_NAME);
            }
            else
            {
            	if (!string.IsNullOrEmpty(info))
                {
                    mes_text = string.Format(Options.GetLiteral(Options.LANG_0_1_CONTINUE), info, ex.Message);
                }
                else
                {
                    mes_text = string.Format(Options.GetLiteral(Options.LANG_0_CONTINUE), ex.Message);
                }
                mes_caption = ex.Source;
            }

            DialogResult d_res = MessageBox.Show
                (mes_text,
                mes_caption,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1);

            return (d_res == DialogResult.Yes);
        }

        public static DialogResult ShowQuestionYesNo(string Question, string Caption)
        {
            return MessageBox.Show
                (Question,
                Caption,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
        }

        public static void ShowMessage(string message)
        {
            MessageBox.Show
                (message,
                Options.GetLiteral(Options.LANG_APP_NAME),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static DialogResult AskCredentials
            (string Caption,
            string Remark,
            ref string UserName,
            ref string Password)
        {
            CredentialsDialog dial = new CredentialsDialog();
            dial.labelRemark.Text = Remark;
            dial.Text = Caption;
            dial.textBoxUsername.Text = UserName;
            dial.textBoxPassword.Text = Password;

            DialogResult ret = dial.ShowDialog();

            if (ret == DialogResult.OK)
            {
                UserName = dial.textBoxUsername.Text;
                Password = dial.textBoxPassword.Text;
            }

            return ret;
        }

        public static DialogResult AskCredentials
            (string Caption,
            string Remark,
            ref string Password)
        {
            CredentialsDialog dial = new CredentialsDialog();
            dial.labelRemark.Text = Remark;
            dial.Text = Caption;
            dial.textBoxUsername.Enabled = false;
            dial.textBoxPassword.Text = Password;
            dial.StartPosition = FormStartPosition.CenterScreen;

            DialogResult ret = dial.ShowDialog();

            if (ret == DialogResult.OK)
            {
                Password = dial.textBoxPassword.Text;
            }

            return ret;
        }
    }
}
