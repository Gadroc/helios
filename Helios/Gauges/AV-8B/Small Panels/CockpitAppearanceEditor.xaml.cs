namespace GadrocsWorkshop.Helios.Gauges.AV8B
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Windows.Controls;
    using System.Windows;

    /// <summary>
    /// Interaction logic for FlightInstrumentAppearanceEditor.xaml
    /// </summary>
    [HeliosPropertyEditor("Helios.AV8B.Cockpit", "Appearance")]
    public partial class CockpitAppearanceEditor : HeliosPropertyEditor
    {
        public CockpitAppearanceEditor()
        {
            InitializeComponent();
        }

        private void Opacity_GotFocus(object sender, RoutedEventArgs e)
        {
            // REVISIT nothing to do?
        }
    }
}
