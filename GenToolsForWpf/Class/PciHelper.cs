using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static GenEngine.HexConvert;

namespace GenToolsForWpf.Class
{
   public class PciHelper
    {
        public string OldPciStr { get; set; }
        public string NewPciStr { get; set; }

        public string OldPciHex => TenToHex(OldPciStr);
        public string NewPciHex => TenToHex(NewPciStr);

        /// <summary>
        /// 初始化PCI实例
        /// </summary>
        /// <param name="oldpci">旧PCI</param>
        /// <param name="newpci">新PCI</param>
        public PciHelper(int oldpci,int newpci)
        {
            this.OldPciStr = oldpci.ToString();
            this.NewPciStr = newpci.ToString();
        }

        /// <summary>
        /// 文件名得到PCI
        /// </summary>
        /// <param name="str">要操作的文件名</param>
        /// <returns>返回的PCI</returns>
        public static string GetPci(string str)
        {

            string pattern = @"PCI(?<grp0>[^\D]+)-";
            Regex reg = new Regex(pattern);
            Match m = reg.Match(str);
         
            if (m.Success)
            {
                str = m.ToString().Replace("PCI", "");
                str = str.Replace("-", "");
                return str;

            }
            return null;
        }
    }
}
