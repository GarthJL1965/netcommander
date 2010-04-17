using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace netCommander
{
    public partial class FileSystemSecurityViewer : UserControl
    {
        public FileSystemSecurityViewer()
        {
            InitializeComponent();
        }

        public void FillContains(string file_name)
        {
            internal_fill(file_name);
        }

        private void internal_fill(string file_name)
        {
            textBoxAcl.Clear();
            textBoxSddl.Clear();

            AuthorizationRuleCollection dacls = null;
            AuthorizationRuleCollection sacls = null;
            FileSecurity f_sec = null;

            try
            {
                f_sec = File.GetAccessControl(file_name);
                try
                {
                    dacls = f_sec.GetAccessRules(true, true, typeof(NTAccount));
                }
                catch (Exception ex)
                {
                    textBoxAcl.AppendText(ex.Message);
                    textBoxAcl.AppendText("\r\n");
                }

                try
                {
                    sacls = f_sec.GetAuditRules(true, true, typeof(NTAccount));
                }
                catch (Exception ex)
                {
                    textBoxAcl.AppendText(ex.Message);
                    textBoxAcl.AppendText("\r\n");
                }

                StringBuilder sb = new StringBuilder();

                sb.Append("Owner\r\n");
                sb.Append("=====\r\n");
                try
                {
                    sb.Append(f_sec.GetOwner(typeof(NTAccount)).Value);
                }
                catch (Exception ex)
                {
                    sb.Append(ex.Message);
                }
                sb.Append("\r\n\r\n");

                sb.Append("Primary group\r\n");
                sb.Append("=============\r\n");
                try
                {
                    sb.Append(f_sec.GetGroup(typeof(NTAccount)).Value);
                }
                catch (Exception ex)
                {
                    sb.Append(ex.Message);
                }
                sb.Append("\r\n\r\n");

                sb.Append("Access rules\r\n");
                sb.Append("============\r\n");
                sb.Append(string.Format("Inherit disable: {0}\r\n\r\n", f_sec.AreAccessRulesProtected));
                if (dacls != null)
                {
                    foreach (FileSystemAccessRule rule in dacls)
                    {
                        sb.Append(string.Format("Identity: {0}\r\n", rule.IdentityReference.Value));
                        sb.Append(string.Format("Access type: {0}\r\n", rule.AccessControlType.ToString()));
                        sb.Append(string.Format("Rights: {0}\r\n", rule.FileSystemRights.ToString()));
                        sb.Append(string.Format("Inheritance: {0}\r\n", rule.InheritanceFlags.ToString()));
                        sb.Append(string.Format("Inherited: {0}\r\n", rule.IsInherited));
                        sb.Append(string.Format("Propagation: {0}\r\n", rule.PropagationFlags.ToString()));
                        sb.Append("\r\n");
                    }
                }
                sb.Append("Audit rules\r\n");
                sb.Append("===========\r\n");
                sb.Append(string.Format("Inherit disable: {0}\r\n\r\n", f_sec.AreAuditRulesProtected));
                if (sacls != null)
                {
                    foreach (FileSystemAuditRule rule in sacls)
                    {
                        sb.Append(string.Format("Identity: {0}\r\n", rule.IdentityReference.Value));
                        sb.Append(string.Format("Audit type: {0}\r\n", rule.AuditFlags.ToString()));
                        sb.Append(string.Format("Rights: {0}\r\n", rule.FileSystemRights.ToString()));
                        sb.Append(string.Format("Inheritance: {0}\r\n", rule.InheritanceFlags.ToString()));
                        sb.Append(string.Format("Inherited: {0}\r\n", rule.IsInherited));
                        sb.Append(string.Format("Propagation: {0}\r\n", rule.PropagationFlags.ToString()));
                        sb.Append("\r\n");
                    }
                }

                textBoxAcl.Font = new Font(FontFamily.GenericMonospace, textBoxAcl.Font.Size);
                textBoxSddl.Font = textBoxAcl.Font;

                textBoxAcl.Text = sb.ToString();
                textBoxSddl.Text = f_sec.GetSecurityDescriptorSddlForm(AccessControlSections.All);
            

            }
            catch (Exception ex)
            {
                textBoxAcl.Font = new Font(FontFamily.GenericMonospace, textBoxAcl.Font.Size);
                textBoxSddl.Font = textBoxAcl.Font;
                textBoxAcl.Text = string.Format("Cannot get security descriptor. {0}", ex.Message);
            }
        }

        

        
    }
}
