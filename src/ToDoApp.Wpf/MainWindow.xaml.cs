using System.Windows;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Controls;
using ToDoApp.Wpf.Models;

namespace ToDoApp.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            

        }



        private void OnAddTodoTaskButtonClick(object sender, RoutedEventArgs e)
        {
            TodoTask item = new TodoTask();
            item.Description = TodoTaskNameText.Text;
            TodoTaskListView.Items.Add(item);
        }

        private void OnRemoveTodoTaskButtonClick(object sender, RoutedEventArgs e)
        {
            int index = TodoTaskListView.SelectedIndex;
            if (CanRemoveTodoTask(index))
                TodoTaskListView.Items.RemoveAt(index);
        }

        private void OnTodoTaskListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = TodoTaskListView.SelectedIndex;
            RemoveTodoTaskButton.IsEnabled = CanRemoveTodoTask(index);
        }

        private bool CanRemoveTodoTask(int selectedIndex)
        {
            // selected index to remove cannot be less than 0 or greater than item count
            return (selectedIndex >= 0 && selectedIndex < TodoTaskListView.Items.Count);
        }

        private void TodoTaskNameText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void FileSave_Click(object sender, RoutedEventArgs e)
        {
            string path = "output.txt";
            if (System.IO.File.Exists(path))
            {
                File.Delete(path); //deletes the old file, creating a new list

                System.IO.FileStream fileStream = System.IO.File.Open(
                path,
                System.IO.FileMode.Append,
                System.IO.FileAccess.Write,
                System.IO.FileShare.None);
                System.IO.StreamWriter writer = new System.IO.StreamWriter(fileStream);

                foreach (var item in TodoTaskListView.Items)
                {
                    TodoTask listitem = item as TodoTask;
                    writer.WriteLine(listitem.Description);
                }

                writer.Close();
                fileStream.Close();


            }
            else
            {

                System.IO.StreamWriter writer = new System.IO.StreamWriter(path);

                foreach (var item in TodoTaskListView.Items)
                {
                    TodoTask listitem = item as TodoTask;
                    writer.WriteLine(listitem.Description); 
                    //
                }

                writer.Close();
            }


        }

        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            string path = "output.txt";

       
            if (System.IO.File.Exists(path))
            {
                System.IO.FileStream fileStream = System.IO.File.Open(
                path,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read,
                System.IO.FileShare.None);
                System.IO.StreamReader reader = new System.IO.StreamReader(fileStream);
                while (reader.EndOfStream == false)
                {
                    string line = reader.ReadLine();
                    TodoTask item = new TodoTask();
                    item.Description = line;
                    TodoTaskListView.Items.Add(item);
                }
                reader.Close();
                fileStream.Close();

            }



        }
    }
}
    
