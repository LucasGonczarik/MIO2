using System.Threading.Tasks;
using System.Windows;
using MIO2.NeuronNetwork;

namespace MIO2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Network _web = new Network();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = false;
            var learningTask = Task.Factory.StartNew(() => _web.RunLearningTask());
            StartButton.IsEnabled = true;
        }
    }
}
