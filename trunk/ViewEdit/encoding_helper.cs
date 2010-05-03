using System;
using System.Collections.Generic;
using System.Text;

namespace netCommander.FileView
{
    /*
     * для отображения доступных кодировок в меню
     */
    public class encoding_helper
    {
        public static Encoding_DisplayInfo[] GetEncodings()
        {
            EncodingInfo[] enc_infos = Encoding.GetEncodings();
            Encoding_DisplayInfo[] ret=new Encoding_DisplayInfo[enc_infos.Length];
            for (int i = 0; i < enc_infos.Length; i++)
            {
                ret[i] = new Encoding_DisplayInfo();
                ret[i].EncodingInfo = enc_infos[i];
            }
            return ret;
        }
    }

    public class Encoding_DisplayInfo
    {
        public EncodingInfo EncodingInfo { get; set; }
        public override string ToString()
        {
            return string.Format("{0} [code page: {2}] [IANA: {1}]", EncodingInfo.DisplayName, EncodingInfo.Name, EncodingInfo.CodePage);
        }
    }
}
