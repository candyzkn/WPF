using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyWPF.CustomControlLibrary;
using System.Threading;

namespace MyWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProgressThread _progressThread = new ProgressThread(200);
            try
            {
                _progressThread.Start();
                _progressThread.Title = "数据加载中请稍后……";


                for (int i = 0; i < 5000; i++)
                {
                    Thread.Sleep(1);
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    
                }));
            }
            finally
            {
                _progressThread.End();
            }
        }
    }
}
