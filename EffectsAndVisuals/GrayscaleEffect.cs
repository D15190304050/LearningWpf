using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace EffectsAndVisuals
{
    class GrayscaleEffect : ShaderEffect
    {
        public GrayscaleEffect()
        {
            Uri piexlShaderUri = new Uri("Grayscale_Compiled.ps", UriKind.Relative);

            // Load the info from the .ps file.
            PixelShader = new PixelShader();
            PixelShader.UriSource = piexlShaderUri;

            UpdateShaderValue(InputProperty);
        }

        public static DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(GrayscaleEffect), 0);

        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
    }
}
