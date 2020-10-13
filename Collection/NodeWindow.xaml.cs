using Collection.model;
using System.Windows;

namespace Collection
{
    /// <summary>
    /// Interaction logic for NodeWindow.xaml
    /// </summary>
    public partial class NodeWindow : Window
    {
        public INode _node { get; private set; }
        public NodeWindow(CSheet p)
        {
            InitializeComponent();
            _node = p;
            this.DataContext = _node;
        }

        public NodeWindow(CNode p)
        {
            InitializeComponent();
            _node = p;
            dPicker.Visibility = Visibility.Hidden;
            dText.Visibility = Visibility.Hidden;
            this.DataContext = _node;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

    }
}
