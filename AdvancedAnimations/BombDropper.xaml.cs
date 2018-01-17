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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace AdvancedAnimations
{
    /// <summary>
    /// BombDropper.xaml 的交互逻辑
    /// </summary>
    public partial class BombDropper : Window
    {
        /// <summary>
        /// "Adjustments" happen periodcally, increasing the speed of bomb falling and shortening the time between bombs.
        /// </summary>
        private DateTime lastAdjustmentTime;

        /// <summary>
        /// Perform an adjustment every 15 seconds;
        /// </summary>
        private const double SecondsBetweenAdjustments = 15;

        /// <summary>
        /// Initially, bombs fall every 1.3 seconds.
        /// </summary>
        private const double InitialSecondsBetweenBombs = 1.3;

        /// <summary>
        /// Initially, bombs hit the ground after 3.5 seconds since falling.
        /// </summary>
        private const double InitialSecondsToFall = 3.5;

        /// <summary>
        /// Seconds between generation of 2 bombs.
        /// </summary>
        private double secondsBetweenBombs;

        /// <summary>
        /// Time for a bomb to fall to the bottom of the window in seconds.
        /// </summary>
        private double secondsToFall;

        /// <summary>
        /// After every adjustment, shave 0.1 seconds off secondsBetweenBombs.
        /// </summary>
        private const double SecondsBetweenBombsReduction = 0.1;

        /// <summary>
        /// After every adjustment, shave 0.1 seconds off secondsToFall.
        /// </summary>
        private const double SecondsToFallReduction = 0.1;

        /// <summary>
        /// Make it possible to look up a storyboard based on a bomb.
        /// </summary>
        private Dictionary<Bomb, Storyboard> storyboards;

        /// <summary>
        /// Fires events on the user interface thread.
        /// </summary>
        private DispatcherTimer bombTimer;

        /// <summary>
        /// Keep track of how many bombs are dropped.
        /// </summary>
        private int droppedCount;

        /// <summary>
        /// Keep track of how many bombs are stopped.
        /// </summary>
        private int savedCount;

        /// <summary>
        /// End the game at maxDropped.
        /// </summary>
        private int maxDropped;

        /// <summary>
        /// A random number generator.
        /// </summary>
        private Random rand;

        public BombDropper()
        {
            lastAdjustmentTime = DateTime.MinValue;
            storyboards = new Dictionary<Bomb, Storyboard>();
            bombTimer = new DispatcherTimer();
            bombTimer.Tick += bombTimer_Tick;
            droppedCount = 0;
            savedCount = 0;
            maxDropped = 5;
            rand = new Random();

            InitializeComponent();
        }

        private void canvasBackground_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RectangleGeometry rect = new RectangleGeometry();
            rect.Rect = new Rect(0, 0, canvasBackground.ActualWidth, canvasBackground.ActualHeight);
            canvasBackground.Clip = rect;
        }

        /// <summary>
        /// Start the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            cmdStart.IsEnabled = false;

            // Reset the game.
            droppedCount = 0;
            savedCount = 0;
            secondsBetweenBombs = InitialSecondsBetweenBombs;
            secondsToFall = InitialSecondsToFall;

            // Start bomb dropping events.
            bombTimer.Interval = TimeSpan.FromSeconds(InitialSecondsBetweenBombs);
            bombTimer.Start();
        }

        /// <summary>
        /// Drop a bomb.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bombTimer_Tick(object sender, EventArgs e)
        {
            // Perform an "adjustment" when needed.
            if (DateTime.Now.Subtract(lastAdjustmentTime).TotalSeconds > SecondsBetweenAdjustments)
            {
                // Update the time stamp.
                lastAdjustmentTime = DateTime.Now;

                // Adjust the intervals.
                secondsBetweenBombs -= SecondsBetweenBombsReduction;
                secondsToFall -= SecondsToFallReduction;

                // Technically, you should check for 0 or negative values.
                // However, in practice these won't occur because the game will always end first.

                // Set the timer to drop the next bomb at the appropriate time.
                bombTimer.Interval = TimeSpan.FromSeconds(secondsBetweenBombs);

                // Update the status message.
                lblRate.Text = string.Format("A bomob is released every {0} seconds", secondsBetweenBombs);
                lblSpeed.Text = string.Format("Each takes {0} seconds to fall", secondsToFall);
            }

            // Create the bomb.
            Bomb bomb = new Bomb();
            bomb.IsFalling = true;

            // Position the bomb.
            bomb.SetValue(Canvas.LeftProperty, (double)(rand.Next(0, (int)(canvasBackground.ActualWidth - 50))));
            bomb.SetValue(Canvas.TopProperty, -100.0);

            // Attach mouse click event (for defusing the bomb).
            bomb.MouseLeftButtonDown += bomb_MouseLeftButtonDown;

            // Create the animation for the falling bomb.
            Storyboard storyboard = new Storyboard();
            DoubleAnimation fallAnimation = new DoubleAnimation();
            fallAnimation.To = canvasBackground.ActualHeight;
            fallAnimation.Duration = TimeSpan.FromSeconds(secondsToFall);

            Storyboard.SetTarget(fallAnimation, bomb);
            Storyboard.SetTargetProperty(fallAnimation, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(fallAnimation);

            // Create the animation for the bomb "wiggle".
            DoubleAnimation wiggleAnimation = new DoubleAnimation();
            wiggleAnimation.To = 30;
            wiggleAnimation.Duration = TimeSpan.FromSeconds(0.2);
            wiggleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            wiggleAnimation.AutoReverse = true;

            Storyboard.SetTarget(wiggleAnimation, ((TransformGroup)bomb.RenderTransform).Children[0]);
            Storyboard.SetTargetProperty(wiggleAnimation, new PropertyPath("Angle"));
            storyboard.Children.Add(wiggleAnimation);

            // Add the bomb to canvasBackground.
            canvasBackground.Children.Add(bomb);

            // Add the storyboard to the tracking collection.
            storyboards.Add(bomb, storyboard);

            // Congifure and start the storyboard.
            storyboard.Duration = fallAnimation.Duration;
            storyboard.Completed += storyboard_Completed;
            storyboard.Begin();
        }

        private void bomb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Get the bomb.
            Bomb bomb = (Bomb)sender;
            bomb.IsFalling = false;

            // Get the bomb's current position.
            Storyboard storyboard = storyboards[bomb];
            double currentTop = Canvas.GetTop(bomb);

            // Stop the bomb from falling.
            storyboard.Stop();

            // Re-use the existing storyboard, but with new animations.
            // Send the bomb on a new trajectory by animating Canvas.Top and Canvas.Left.
            storyboard.Children.Clear();

            // Create the rise animation.
            DoubleAnimation riseAnimation = new DoubleAnimation();
            riseAnimation.From = currentTop;
            riseAnimation.To = 0;
            riseAnimation.Duration = TimeSpan.FromSeconds(2);

            Storyboard.SetTarget(riseAnimation, bomb);
            Storyboard.SetTargetProperty(riseAnimation, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(riseAnimation);

            DoubleAnimation slideAnimation = new DoubleAnimation();
            double currentLeft = Canvas.GetLeft(bomb);

            // Throw the bomb off the closest side.
            if (currentLeft < canvasBackground.ActualWidth / 2)
                slideAnimation.To = -100;
            else
                slideAnimation.To = canvasBackground.ActualWidth + 100;

            slideAnimation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTarget(slideAnimation, bomb);
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(slideAnimation);

            // Start the new animation.
            storyboard.Duration = slideAnimation.Duration;
            storyboard.Begin();
        }

        private void storyboard_Completed(object sender, EventArgs e)
        {
            ClockGroup clockGroup = (ClockGroup)sender;

            // Get the first animation in the storyboard, and use it to find the bomb that is being animated.
            DoubleAnimation completedAnimation = (DoubleAnimation)clockGroup.Children[0].Timeline;
            Bomb completedBomb = (Bomb)Storyboard.GetTarget(completedAnimation);

            // Determine if a bomb fell or flew off the canvas after being clicked.
            if (completedBomb.IsFalling)
                droppedCount++;
            else
                savedCount++;

            // Update the display.
            lblStatus.Text = string.Format("You have dropped {0} bomobs and saved {1}.", droppedCount, savedCount);

            // Check if it's game over.
            if (droppedCount >= maxDropped)
            {
                bombTimer.Stop();
                lblStatus.Text = "\r\n\r\nGame Over";

                // Find all the storyboards that underway.
                foreach (KeyValuePair<Bomb, Storyboard> item in storyboards)
                {
                    Storyboard storyboard = item.Value;
                    Bomb bomb = item.Key;

                    storyboard.Stop();
                    canvasBackground.Children.Remove(bomb);
                }

                // Empty the tracking collection.
                storyboards.Clear();

                // Allow the user to start a new game.
                cmdStart.IsEnabled = true;
            }
            else
            {
                Storyboard storyboard = (Storyboard)clockGroup.Timeline;
                storyboard.Stop();

                storyboards.Remove(completedBomb);
                canvasBackground.Children.Remove(completedBomb);
            }
        }
    }
}
