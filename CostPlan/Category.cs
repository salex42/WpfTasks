using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CostPlan
{
    public class Category : INotifyPropertyChanged
    {
        private string category_name;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Category_id { get; set; }

        public string Category_name
        {
            get { return category_name; }
            set
            {
                category_name = value;
                OnPropertyChanged("Category_name");
            }
        }

        public Category()
        {
            this.Expense = new HashSet<Expense>();
        }

        public virtual ICollection<Expense> Expense { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
