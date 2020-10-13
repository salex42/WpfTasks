using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collection.model
{
    public class CNode : INode
    {
        public CNode()
        {
            this.Children = new SortableObservableCollection<INode>();
            this.Children.Sort(t => t.Name);
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public SortableObservableCollection<INode> Children { get; set; }
    }

    public class CSheet : INode
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime Dat { get; set; }
    }

    public interface INode
    {
        int Id { get; set; }
        int? ParentId { get; set; }
        string Name { get; set; }
        string Type { get; set; }
    }
}
