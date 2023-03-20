using System.Windows;

namespace BUHALOVO
{
    /// <summary>
    /// Логика взаимодействия для CreateTypeWin.xaml
    /// </summary>
    public partial class CreateTypeWin : Window
    {
        public CreateTypeWin()
        {
            InitializeComponent();
        }

        private void CreateNewType_Click(object sender, RoutedEventArgs e)
        {
            Types();
        }

        private void Types()
        {
            MainWindow.types.Add(TypeNoteTBox.Text);
            FileManager.Serialize(MainWindow.types, "types");
            Close();
        }
    }
}
