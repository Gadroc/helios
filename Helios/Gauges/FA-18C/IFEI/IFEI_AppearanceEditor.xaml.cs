namespace GadrocsWorkshop.Helios.Gauges.FA18C
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Windows.Controls;
    using System.Windows;

    /// <summary>
    /// Interaction logic for IFEI_AppearanceEditor.xaml
    /// </summary>
    [HeliosPropertyEditor("Helios.FA18C.IFEI", "Appearance")]
    public partial class IFEI_AppearanceEditor : HeliosPropertyEditor
    {
        public IFEI_AppearanceEditor()
        {
            InitializeComponent();
        }

        private void Opacity_GotFocus(object sender, RoutedEventArgs e)
        {
            // REVISIT nothing to do?
        }
    }
}
