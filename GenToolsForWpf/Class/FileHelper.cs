using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GenEngine;
using Microsoft.Win32;

namespace GenToolsForWpf.Class
{
   public class FileHelper
    {
        public static string[] OpenFile()
        {
           
            OpenFileDialog ofd = new OpenFileDialog()
            {
                DefaultExt = ".gen",
                Filter = "Gen文件(*.gen)|*.gen|所有文件(*.*)|*.*",
                Title = "请选择GEN文件",
                AddExtension = true,
                Multiselect = true,
                DereferenceLinks = true,             
                RestoreDirectory = true
            };
            if (ofd.ShowDialog()==true)
            {
                return ofd.FileNames;
            }
            return null;
        }

        public static StringBuilder Read(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    StringBuilder data = new StringBuilder();
                    try
                    {
                        while (true)
                        {
                            data.Append( HexConvert.ByteToHex(br.ReadByte()));
                        }
                    }
                    catch (Exception)
                    {
                        return data;
                    }      
                }
            }
        }

        public static void Save(string path, StringBuilder s)
        {
            byte[] buffer = HexConvert.HexStringToByteArray(s.ToString());
            using (FileStream fs=new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buffer, 0, buffer.Length);
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Opens folder and select file. </summary>
        ///
        /// <remarks>   Topiv, 2017-06-26. </remarks>
        ///
        /// <param name="fileFullName"> Name of the file full. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void OpenFolderAndSelectFile(String fileFullName)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe")
            {
                Arguments = "/e,/select," + fileFullName
            };
            System.Diagnostics.Process.Start(psi);

        }
    }
}
