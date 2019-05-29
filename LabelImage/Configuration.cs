using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LabelImage
{
    [Serializable]
    public class Configuration : FrameworkElement
    {
        #region Dependency Properties
        public static readonly DependencyProperty UIFontSizeProperty;
        public static readonly DependencyProperty UIFontFamilyProperty;
        #endregion

        /// <summary>
        /// Initializes dependency properties.
        /// </summary>
        static Configuration()
        {
            FrameworkPropertyMetadata uiFontSizeMetadata = new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure|FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
            UIFontSizeProperty = DependencyProperty.Register(nameof(UIFontSize), typeof(int), typeof(Configuration), uiFontSizeMetadata);

            // Set the default value of UI font family as the system default font family.
            FontFamily systemDefaultFontFamily = new FontFamily(System.Drawing.SystemFonts.DefaultFont.Name);
            FrameworkPropertyMetadata uiFontFamilyMetadata = new FrameworkPropertyMetadata(systemDefaultFontFamily, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
            UIFontFamilyProperty = DependencyProperty.Register(nameof(UIFontFamily), typeof(FontFamily), typeof(Configuration), uiFontFamilyMetadata);
        }

        #region Getters and setters of dependency properties.

        [Bindable(true)]
        [Category("Behavior")]
        public int UIFontSize
        {
            get => (int) GetValue(UIFontSizeProperty);
            set => SetValue(UIFontSizeProperty, value);
        }

        public FontFamily UIFontFamily
        {
            get => (FontFamily) GetValue(UIFontFamilyProperty);
            set => SetValue(UIFontFamilyProperty, value);
        }

        #endregion

        public Configuration()
        {
            this.UIFontSize = ApplicationContext.UIFontSizeDefaultValue;
        }
    }
}
