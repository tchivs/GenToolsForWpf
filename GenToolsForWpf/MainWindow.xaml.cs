using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GenToolsForWpf.Class;

namespace GenToolsForWpf
{
 
    public delegate void AddLogDelegate(string s);
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private GenProcess gp ;
        private PciHelper pci;
        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            txbLog.Clear();
            gp = new GenProcess(AppendLog);
            txbSourcePci.Text=
            PciHelper.GetPci(gp.Filenames[0]);
        }
        
        private void AppendLog(string msg)
        {
            this.txbLog.Dispatcher.Invoke(new AddLogDelegate(AppendLogAction), msg);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="s"></param>
        public void AppendLogAction(string s)
        {
            this.txbLog.AppendText(s + "\r\n");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if (IsCanWork)
            {

                txbLog.Clear();
                int sourcePci = Convert.ToInt32(txbSourcePci.Text);
                int newpci = Convert.ToInt32(txbNewPci.Text);
                pci = new PciHelper(sourcePci, newpci);
                gp.pcihelper = pci;
                Thread th = new Thread(gp.Run)
                {
                    IsBackground = true
                };
                th.Start();
            }
            
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   如果有选择文件就开工. </summary>
        ///
        /// <value> True if this object is can work, false if not. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool IsCanWork => gp.Filenames != null && !String.IsNullOrEmpty(txbNewPci.Text) && !String.IsNullOrEmpty(txbSourcePci.Text);

        private void btnOpendir_Click(object sender, RoutedEventArgs e)
        {
            if (IsCanWork)
            {
                //打开目录

               FileHelper.OpenFolderAndSelectFile(gp.Filenames[0]);
            }

        }
    }
}
