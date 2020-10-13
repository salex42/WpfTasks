using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Data.Entity;
using System.ComponentModel;
using System.Collections.Specialized;

namespace CostPlan
{
    /// <summary>
    /// Основная форма приложения отображающая таблицу с расходами 
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext db;
        CollectionViewSource _itemSourceList;
        public MainWindow()
        {
            InitializeComponent();

            db = new ApplicationContext();
            db.Expenses.Load();
            _itemSourceList = new CollectionViewSource() { Source = db.Expenses.Local };
            _itemSourceList.View.CollectionChanged += UpdateItog;
            UpdateItog(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            
            ICollectionView Itemlist = _itemSourceList.View;
            this.DataContext = Itemlist;
            dPickerBegin.SelectedDate = DateTime.Today.AddDays(-30);
            dPickerEnd.SelectedDate = DateTime.Today;
        }


        // фильтр отбирающий строки по условию
        private void dateFilter(object sender, FilterEventArgs e)
        {
            var obj = e.Item as Expense;
            if (obj != null )
            {
                if (obj.ExpDate >= dPickerBegin.SelectedDate && obj.ExpDate <= dPickerEnd.SelectedDate)
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }

        // добавление
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Expense new_Exp = new Expense();
            new_Exp.ExpDate = DateTime.Today;
            ExpenseWindow expWindow = new ExpenseWindow(new_Exp);
            db.Configuration.ValidateOnSaveEnabled = false;
            if (expWindow.ShowDialog() == true)
            {
                Expense exp = expWindow._expense;
                exp.Category = db.Categories.Find(expWindow._expense.Category_id);
                db.Expenses.Add(exp);
                db.SaveChanges();
            }
        }

        // редактирование
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (expGrid.SelectedItem == null) return;
            Expense exp = expGrid.SelectedItem as Expense;

            ExpenseWindow expWindow = new ExpenseWindow(new Expense
            {
                Id = exp.Id,
                Expense_name = exp.Expense_name,
                Cost = exp.Cost,
                ExpDate = exp.ExpDate,
                Category_id = exp.Category_id
            });

            if (expWindow.ShowDialog() == true)
            {
                exp = db.Expenses.Find(expWindow._expense.Id);
                if (exp != null)
                {
                    exp.Expense_name = expWindow._expense.Expense_name;
                    exp.Cost = expWindow._expense.Cost;
                    exp.ExpDate = expWindow._expense.ExpDate;
                    exp.Category_id = expWindow._expense.Category_id;
                    db.Entry(exp).State = EntityState.Modified;
                    db.SaveChanges();
                    _itemSourceList.View.Refresh();
                }
            }
        }
        
        // удаление
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (expGrid.SelectedItem == null) return;
            // получаем выделенный объект
            Expense exp = expGrid.SelectedItem as Expense;
            db.Expenses.Remove(exp);
            db.SaveChanges();
        }

        // обновление 
        private void expGrid_LayoutUpdated(object sender, EventArgs e)
        {
            Thickness t = lblTotalCnt.Margin;
            lblTotalCnt.Margin = t;
            lblTotalCnt.Width = expGrid.ActualWidth - expGrid.Columns[3].ActualWidth;
            lblTotalSum.Width = expGrid.Columns[3].ActualWidth;

        }

        // включение фильтра
        private void FilterCheck_Checked(object sender, RoutedEventArgs e)
        {
            _itemSourceList.Filter += new FilterEventHandler(dateFilter);
        }

        // выключение фильтра
        private void FilterCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            _itemSourceList.Filter -= new FilterEventHandler(dateFilter);
        }

        // обновление итоговой строки
        private void UpdateItog(object sender, NotifyCollectionChangedEventArgs e)
        {
            float sum = 0;
            int cnt = 0;
            cnt = _itemSourceList.View.OfType<Expense>().Count();
            foreach (var tex in _itemSourceList.View.OfType<Expense>())
            {
                sum += (tex as Expense).Cost;
            }
            lblTotalCnt.Text = String.Format("Количество: {0}", cnt);
            lblTotalSum.Text = String.Format("Сумма: {0:f}", sum);
        }

    }
}
