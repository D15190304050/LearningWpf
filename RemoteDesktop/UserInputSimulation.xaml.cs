using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RemoteDesktop
{
    /// <summary>
    /// UserInputSimulation.xaml 的交互逻辑
    /// </summary>
    public partial class UserInputSimulation : Window
    {
        private MouseHook mouseHook;

        public UserInputSimulation()
        {
            InitializeComponent();
        }

        private void cmdSetCursor_Click(object sender, RoutedEventArgs e)
        {
            UserMouseEvent.SetCursorPos(1920, 1080);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //mouseHook = new MouseHook();
            //mouseHook.SetHook();
            //mouseHook.MouseDownEvent += MouseDownEventHandler;
            //mouseHook.MouseUpEvent += MouseUpEventHandler;
        }

        //按下鼠标键触发的事件
        private void MouseDownEventHandler(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            txtMouseEvents.AppendText("按下了左键\n");
        }

        private void MouseUpEventHandler(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            txtMouseEvents.AppendText($"{e.X},{e.Y}\n");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //mouseHook.UnHook();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserMouseEvent.SetCursorPos(1920, 1080);
            UserMouseEvent.MouseRightClick(1920, 1080, 0);

            //MessageBox.Show("Ready for next?");

            UserMouseEvent.SetCursorPos(2560, 1440);
            UserMouseEvent.MouseRightClick(2560, 1440, 0);
        }
    }
}
