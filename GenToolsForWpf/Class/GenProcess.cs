using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GenEngine;

namespace GenToolsForWpf.Class
{
    public class GenProcess
    {
        public PciHelper pcihelper;
        readonly AddLogDelegate _appendLog;
        private readonly List<StringBuilder> datalist = new List<StringBuilder>();

        /// <summary>
        /// 文件目录
        /// </summary>
        public string filePath { get => Path.GetDirectoryName(Filenames[0]) + @"\"; }
        /// <summary>
        /// 用于保存文件的路径
        /// </summary>
        public string[] Filenames { get; set; }
        /// <summary>
        /// 初始化并选择文件
        /// </summary>
        public GenProcess(AddLogDelegate appendLog)
        {
            this._appendLog = appendLog;
            this.Filenames=  FileHelper.OpenFile();
            ShowFileName();
        }

        /// <summary>
        /// 显示文件名
        /// </summary>
        public void ShowFileName()
        {
            foreach (string item in Filenames)
            {
              _appendLog(Path.GetFileName(item));
            }
        }

        public void Run()
        {
            ReadFile();
            ProcessData();
        }

        /// <summary>
        /// 把数据读到datalist中
        /// </summary>
        private void ReadFile()
        {
            if (Filenames.Length == 1)
            {
                datalist.Add(FileHelper.Read(Filenames[0]));
            }
            else
            {
                foreach (var item in Filenames)
                {
                    datalist.Add(FileHelper.Read(item));
                }
            }
        }

        /// <summary>
        /// 整理数据
        /// </summary>
        private void ProcessData()
        {
            for (int i = 0; i < datalist.Count; i++)
            {
                GetNewDate(datalist[i]);
                //把修改好的十六进制文本转换成字节数组
                SaveData(i);
            }
            _appendLog("Finish!\n");
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="i"></param>
        private void SaveData(int i)
        {
          string name=Path.GetFileName(Filenames[i]);
            if (name.Contains(pcihelper.OldPciStr))
            {
                name =filePath + @"\" + name.Replace(pcihelper.OldPciStr, pcihelper.NewPciStr);
            }
            else
            {
                name= filePath + "PCI-" + pcihelper.OldPciStr + name;
            }
            FileHelper.Save(name, datalist[i]);
            _appendLog("Save to\t"+name+"\n");
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="data"></param>
        private void GetNewDate(StringBuilder data)
        {
            data.Replace("F04E" +pcihelper.OldPciHex, "F04E" + pcihelper.NewPciHex);           
            //存放数据
            List<string> list = new List<string>();
            Regex reg = new Regex(@"11[0-9a-fA-F]." + pcihelper.OldPciHex);
            MatchCollection matches = reg.Matches(data.ToString());
            foreach (var item in matches)
            {
                list.Add(item.ToString());
            }
            var TEMP = DelRepeatData(list);
            list.Clear();
            list.AddRange(TEMP);
            foreach (var item in list)
            {
                data.Replace(item, item.Substring(0, 4) + pcihelper.NewPciHex);
            }
        
            for (int k = 0; k <= 5; k++)
            {
                data.Replace("0" + k + "00" + pcihelper.OldPciHex, "0" + k + "00" + pcihelper.NewPciHex);
            }

        }


        /// <summary>
        /// 去重复数组
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string[] DelRepeatData(List<string> a)
        {
            return a.GroupBy(p => p).Select(p => p.Key).ToArray();
        }
    }
}
