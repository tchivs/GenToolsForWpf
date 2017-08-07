using System;
using System.Threading;
using System.Windows;
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
            robAuto.IsChecked = true;
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
            this.txbLog.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        if (this.txbLog.Text != null)
                        {
                            this.txbLog.AppendText(msg + "\r\n");
                            //随着内容的增加文本框移动到最下行
                            this.txbLog.ScrollToLine(this.txbLog.LineCount - 1);
                        }
                        else
                        {
                            this.txbLog.Text = null;
                        }
                    }));
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
           //Close();
            AppendLog("1:软件默认从文件名读取源PCI，如 xxxx4-PCI444-3#电梯-DL-DT.gen  默认源PCI为444，新PCI为222\r\n即生成新文件xxxx4-PCI222-3#电梯-DL-DT.gen\r\n2：如果模式选择为“填写为准”，那源PCI以填写为准，生成的新文件为xxxx4-PCI444-3#电梯-DL-DT-new.gen");
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if (IsCanWork)
            {

                txbLog.Clear();
                int sourcePci = Convert.ToInt32(txbSourcePci.Text);
                int newpci = Convert.ToInt32(txbNewPci.Text);
                pci = new PciHelper(sourcePci, newpci);
                if (robAuto.IsChecked == true)
                {
                    gp.IsSource = true;
                }
                else
                {
                    gp.IsSource = false;
                }
                gp.pcihelper = pci;
                Thread th = new Thread(gp.Run)
                {
                    IsBackground = true
                };
                try
                {
                    th.Start();
                }
                catch (Exception exception)
                {
                    AppendLog("运行时出错！已终止，错误原因："+exception.Message);
                }
      
     
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
