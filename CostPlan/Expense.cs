using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CostPlan
{
    public class Expense : INotifyPropertyChanged, IDataErrorInfo
    {
        private string expense_name;
        private string expense_date;
        private float cost;
        private int category_id;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Expense_name
        {
            get { return expense_name; }
            set
            {
                expense_name = value;
                OnPropertyChanged("Expense_name");
            }
        }

        public string Expense_date
        {
            get { return expense_date; }
            set
            {
                expense_date = value;
                OnPropertyChanged("Expense_date");
            }
        }

        [NotMapped]
        public DateTime ExpDate
        {
            get
            {
                return DateTime.ParseExact(Expense_date,"dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                Expense_date = value.ToString("dd.MM.yyyy");
                OnPropertyChanged("ExpDate");
            }
        }

        public float Cost
        {
            get { return cost; }
            set
            {
                cost = value;
                OnPropertyChanged("Cost");
            }
        }

        public int Category_id
        {
            get { return category_id; }
            set
            {
                category_id = value;
                OnPropertyChanged("category_id");
            }
        }
        [ForeignKey("Category_id")]
        public virtual Category Category { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Cost":
                        if ((Cost <= 0))
                        {
                            error = "Цена должна быть положительной";
                        }
                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
    }


}
