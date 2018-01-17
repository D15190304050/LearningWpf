using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AnimationBasics
{
    /// <summary>
    /// AnimationPlayer.xaml 的交互逻辑
    /// </summary>
    public partial class AnimationPlayer : Window
    {
        public AnimationPlayer()
        {
            InitializeComponent();
        }

        private void sldSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            fadeStoryboard.SetSpeedRatio(this, sldSpeed.Value);
        }

        private void fadeStoryboard_CurrentTimeInvalidated(object sender, EventArgs e)
        {
            // Sender is the clock that was created for this storyboard.
            Clock storyboardClock = (Clock)sender;

            if (storyboardClock.CurrentProgress == null)
            {
                lblTime.Text = "[[ Stopped ]]";
                progressBar.Value = 0;
            }
            else
            {
                lblTime.Text = storyboardClock.CurrentTime.ToString();
                progressBar.Value = (double)storyboardClock.CurrentProgress;
            }
        }
    }
}
