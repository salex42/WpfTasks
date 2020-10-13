using Collection.model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace Collection
{
    /// <summary>
    /// Список коллекции
    /// </summary>

    public partial class MainWindow : Window
    {
        ApplicationContext db;
        public MainWindow()
        {
            InitializeComponent();

            db = new ApplicationContext();

            GenerateTree();
        }

        // Герерация структуры дерева из таблицы БД
        public void GenerateTree()
        {
            db.Collects.Load();

            IEnumerable<Collect> list = db.Collects.Local;

            SortableObservableCollection<INode> tree = new SortableObservableCollection<INode>();
            foreach (Collect t in db.Collects.Local.Where(t => t.Parent_id is null)) // выбираем корневые узлы дерева
            {
                CNode node = new CNode();
                node.Id = t.Id;
                node.Name = t.Collect_name;
                node.Children = GetChildren(t.Id, list); // поискпотомков корневого узла
                tree.Add(node);
            }
            tree.Sort(t => t.Name);
            treeColl.ItemsSource = tree;
        }

        // Поиск потомков узла дерева сыслающегося на строку с ключом node_id
        private SortableObservableCollection<INode> GetChildren(int node_id, IEnumerable<Collect> list)
        {
            SortableObservableCollection<INode> tree = new SortableObservableCollection<INode>();
            foreach (Collect t in list.Where(n => n.Parent_id == node_id)) 
            {
                if (t.Type == "sheet") // конечный узел дерева - "лист"
                {
                    CSheet sheet = new CSheet();
                    sheet.Id = t.Id;
                    sheet.ParentId = t.Parent_id;
                    sheet.Name = t.Collect_name;
                    sheet.Dat = t.DDate;
                    tree.Add(sheet);
                }
                else
                {
                    CNode node = new CNode();
                    node.Id = t.Id;
                    node.Name = t.Collect_name;
                    node.Children = GetChildren(t.Id, list);
                    tree.Add(node);
                }
            }
            tree.Sort(t => t.Name);
            return tree;
        }


        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            CNode node = new CNode();
            NodeWindow nodeWindow = new NodeWindow(node);
            if (nodeWindow.ShowDialog() == true)
            {
                Collect coll = new Collect();
                coll.Collect_name = nodeWindow._node.Name;
                if (treeColl.SelectedItem.GetType() == typeof(CSheet))
                {
                    coll.Parent_id = (treeColl.SelectedItem as CSheet).ParentId;

                }
                else
                    coll.Parent_id = (treeColl.SelectedItem as CNode).Id;
                db.Collects.Add(coll);
                db.SaveChanges();
                GenerateTree();
            }
        }

        private void AddSheet_Click(object sender, RoutedEventArgs e)
        {
            CSheet sheet = new CSheet();
            sheet.Dat = DateTime.Today;
            NodeWindow nodeWindow = new NodeWindow(sheet);
            if (nodeWindow.ShowDialog() == true)
            {
                Collect coll = new Collect();
                coll.Collect_name = nodeWindow._node.Name;
                if (treeColl.SelectedItem.GetType() == typeof(CSheet))
                {
                    coll.Parent_id = (treeColl.SelectedItem as CSheet).ParentId;
                    
                }
                else
                    coll.Parent_id = (treeColl.SelectedItem as CNode).Id;
                coll.Type = "sheet";
                coll.DDate = (nodeWindow._node as CSheet).Dat;

                db.Collects.Add(coll);
                db.SaveChanges();
                GenerateTree();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (treeColl.SelectedItem == null) return;

            Collect coll;

            if (treeColl.SelectedItem.GetType() == typeof(CSheet))
            {
                CSheet node = treeColl.SelectedItem as CSheet;
                NodeWindow nodeWindow = new NodeWindow(node);
                if (nodeWindow.ShowDialog() == true)
                {
                    coll = db.Collects.Find(nodeWindow._node.Id);
                    if (coll != null)
                    {
                        coll.Collect_name = nodeWindow._node.Name;
                        coll.Type = "sheet";
                        coll.DDate = (nodeWindow._node as CSheet).Dat;
                        db.Entry(coll).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            if (treeColl.SelectedItem.GetType() == typeof(CNode))
            {
                CNode node = treeColl.SelectedItem as CNode;
                NodeWindow nodeWindow = new NodeWindow(node);
                if (nodeWindow.ShowDialog() == true)
                {
                    coll = db.Collects.Find(nodeWindow._node.Id);
                    if (coll != null)
                    {
                        coll.Collect_name = nodeWindow._node.Name;
                        db.Entry(coll).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            GenerateTree();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (treeColl.SelectedItem == null) return;
            if (treeColl.SelectedItem.GetType() == typeof(CSheet))
            {
                CSheet node = treeColl.SelectedItem as CSheet;
                Collect coll = db.Collects.Find(node.Id);
                db.Collects.Remove(coll);
                db.SaveChanges();
                GenerateTree();
            }
            else
            {
                CNode node = treeColl.SelectedItem as CNode;
                if (node.Children.Count() > 0)
                    MessageBox.Show("А данного узла есть потомки, удалите сначала все дочерние узлы.");
                else
                {
                    Collect coll = db.Collects.Find(node.Id);
                    db.Collects.Remove(coll);
                    db.SaveChanges();
                    GenerateTree();
                }
            }
        }
    }
}
