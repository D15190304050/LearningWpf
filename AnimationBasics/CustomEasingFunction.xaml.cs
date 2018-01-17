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
    /// CustomEasingFunction.xaml 的交互逻辑
    /// </summary>
    public partial class CustomEasingFunction : Window
    {
        public CustomEasingFunction()
        {
            InitializeComponent();
        }
    }

    public class RandomJitterEase : EasingFunctionBase
    {
        // Store a random number generator.
        private Random rand = new Random();

        public int Jitter
        {
            get { return (int)GetValue(JitterProperty); }
            set { SetValue(JitterProperty, value); }
        }

        protected override double EaseInCore(double normalizedTime)
        {
            // Make sure there is no jitter in the final value.
            if (normalizedTime == 1)
                return 1;

            // Offset the value by a random amount.
            return Math.Abs(normalizedTime - (double)rand.Next(0, 10) / (2010 - Jitter));
        }

        public static readonly DependencyProperty JitterProperty =
            DependencyProperty.Register("Jitter", typeof(int), typeof(RandomJitterEase),
                new UIPropertyMetadata(1000),
                new ValidateValueCallback(value => 
                {
                    int jitterValue = (int)value;
                    return (jitterValue <= 2000) && (jitterValue >= 0);
                }));

        /// <summary>
        /// This required override simply provides a live instance of your easing function.
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore()
        {
            return new RandomJitterEase();
        }
    }
}
