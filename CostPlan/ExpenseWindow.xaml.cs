using System;
using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;

namespace CostPlan
{
    /// <summary>
    /// Форма внесени/редактирования строки расхода 
    /// </summary>

    public partial class ExpenseWindow : Window
    {
        public Expense _expense { get; private set; }
        ApplicationContext db;
        public ExpenseWindow(Expense p)
        {
            InitializeComponent();
            _expense = p;
            db = new ApplicationContext();
            db.Categories.Load();
            comboBoxForm.ItemsSource = db.Categories.Local;

            this.DataContext = _expense;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            MessageBox.Show(e.Error.ErrorContent.ToString());
        }
    }

}
