using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LabelImage
{
    public class LabelCommands
    {
        static LabelCommands()
        {
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.OemPlus, ModifierKeys.Control, "Ctrl++"));
            ZoomIn = new RoutedUICommand(Properties.Resources.ZoomIn, "ZoomIn", typeof(LabelCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.OemMinus, ModifierKeys.Control, "Ctrl+-"));
            ZoomOut = new RoutedUICommand(Properties.Resources.ZoomOut, "ZoomOut", typeof(LabelCommands), inputs);

            PreviousImage = new RoutedUICommand(Properties.Resources.PrevisouImage, "PreviousImage", typeof(LabelCommands));
            NextImage = new RoutedUICommand(Properties.Resources.NextImage, "NextImage", typeof(LabelCommands));

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+S"));
            SaveAnnotatedImage = new RoutedUICommand(Properties.Resources.SaveAnnotatedImage, "SaveAnnotatedImage", typeof(LabelCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R"));
            ResetCurrentImage = new RoutedUICommand(Properties.Resources.ResetCurrentImage, "ResetCurrentImage", typeof(LabelCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.W, ModifierKeys.Control, "Ctrl+W"));
            CloseImage = new RoutedUICommand(Properties.Resources.CloseImage, "CloseImage", typeof(LabelCommands), inputs);
        }

        public static RoutedUICommand ZoomIn { get; }
        public static RoutedUICommand ZoomOut { get; }

        public static RoutedUICommand PreviousImage { get; }

        public static RoutedUICommand NextImage { get; }

        public static RoutedUICommand SaveAnnotatedImage { get; }

        public static RoutedUICommand ResetCurrentImage { get; }
        public static RoutedUICommand CloseImage { get; }
    }
}
