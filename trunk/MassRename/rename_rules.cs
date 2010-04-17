using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MassRename
{

    //TODO: все переписать
    internal abstract class RenameRuleBase
    {
        private string m_substitute_string;
        public string SubstituteString
        {
            get
            {
                return m_substitute_string;
            }
            protected set
            {
                m_substitute_string = value;
            }
        }

        public abstract string MakeSubstitute(IList<string> file_list, int index);
    }

    internal class RenameRuleNameWithoutExtension : RenameRuleBase
    {

        public RenameRuleNameWithoutExtension()
        {
            SubstituteString = "N";
        }

        public override string MakeSubstitute(IList<string> file_list, int index)
        {
            return Path.GetFileNameWithoutExtension(file_list[index]);
        }
    }

    internal class RenameRulePartOfName : RenameRuleBase
    {
        public RenameRulePartOfName(int start,int end)
        {
            Start = start;
            End = end;
            SubstituteString = string.Format("{0}{1}-{2}", "N", start, end);
        }

        public int Start { get; set; }
        public int End { get; set; }

        public override string MakeSubstitute(IList<string> file_list, int index)
        {
            string orig = Path.GetFileNameWithoutExtension(file_list[index]);
            if (orig.Length <= Start)
            {
                return string.Empty;
            }
            else if (orig.Length <= End)
            {
                return orig.Substring(Start);
            }
            else
            {
                return orig.Substring(Start, End - Start + 1);
            }
        }
    }

    internal class RenameRuleExtension : RenameRuleBase
    {
        public RenameRuleExtension()
        {
            SubstituteString = "E";
        }

        public override string MakeSubstitute(IList<string> file_list, int index)
        {
            return Path.GetExtension(file_list[index]);
        }
    }

    internal class RenameRuleChangeTime : RenameRuleBase
    {
        public RenameRuleChangeTime(string date_format)
        {
            DateFormat = date_format;
            SubstituteString = string.Format("{0}:{1}", "C", date_format);
        }

        public string DateFormat { get; set; }

        public override string MakeSubstitute(IList<string> file_list, int index)
        {
            return File.GetLastWriteTime(file_list[index]).ToString(DateFormat);
        }
    }
}
