using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collection.model
{
    public class Collect : INotifyPropertyChanged
    {
        private string collect_name;
        private string date_add;
        private string type;
        private int? parent_id;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Collect_name
        {
            get { return collect_name; }
            set
            {
                collect_name = value;
                OnPropertyChanged("Collect_name");
            }
        }

        public string Date_add
        {
            get { return date_add; }
            set
            {
                date_add = value;
                OnPropertyChanged("Date_add");
            }
        }

        [NotMapped]
        public DateTime DDate
        {
            get
            {
                return DateTime.ParseExact(Date_add, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                Date_add = value.ToString("dd.MM.yyyy");
                OnPropertyChanged("DDate");
            }
        }

        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        public int? Parent_id
        {
            get { return parent_id; }
            set
            {
                parent_id = value;
                OnPropertyChanged("Parent_id");
            }
        }
        [ForeignKey("Parent_id")]
        public virtual Collect Parent { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }


}
