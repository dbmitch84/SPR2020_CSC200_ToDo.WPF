using System;
using System.Collections.Generic;
using System.Windows;
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

        private const string FilterTxt = "Text file (*.txt)|*.txt";
        private const string FilterJson = "JSON file (*.json)|*.json";
        private const string FilterXml = "XML file (*.xml)|*.xml";
        private const string FilterSoap = "SOAP file (*.soap)|*.soap";
        private const string FilterBinary = "Binary file (*.bin)|*.bin";
        private const string FilterAny = "Any files (*.*)|*.*";

        private Models.TodoTask Document { get; }

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
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();

            dialog.Filter = FilterTxt + "|" +
                            FilterJson + "|" +
                            FilterXml + "|" +
                            FilterSoap + "|" +
                            FilterBinary;

            dialog.FilterIndex = 1;

            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                string filePath = dialog.FileName;
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // get content from UI control
                string content = "";

                // determine which format the user want to save as
                if (dialog.FilterIndex == 1)
                {
                    // write as txt
                    System.IO.StringWriter writer = new System.IO.StringWriter();
                    List<TodoTask> items = new List<TodoTask>();
                    foreach (var item in this.TodoTaskListView.Items)
                    {
                        items.Add(item as TodoTask);
                    }

                    foreach (var item in items)
                    {
                        writer.WriteLine(item.Description);
                    }
                    /* writer.WriteLine(DateTime.Now);   // write last saved
                     writer.Write(content);            // write content */
                    content = writer.ToString();      // assign assembled content

                    writer.Dispose();                 // release writer
                }
                else if (dialog.FilterIndex == 2)
                {
                    System.IO.StringWriter writer = new System.IO.StringWriter();
                    System.Collections.ArrayList list = new System.Collections.ArrayList();
                    ItemCollection items = this.TodoTaskListView.Items;
                    foreach (var item in this.TodoTaskListView.Items)
                    {
                        items.Add(item as TodoTask);
                    }

                 

                    // write as JSON
                    // create object to serialize

                    
                    foreach (var item in items)
                    {
                        TodoTask todoTask = item as TodoTask;
                        list.Add(todoTask);
                    }
                    Models.TodoTask document = new Models.TodoTask(content);
                    // serialize type (Models.Document) to JSON string
                    string json = Newtonsoft
                        .Json
                        .JsonConvert
                        .SerializeObject(document);
                    // set content to JSON result
                    content = json;                   // assign JSON string to content
                }

                else if (dialog.FilterIndex == 3)
                {
                    
                    // write as XML
                    // create object to serialize
                    List<TodoTask> list = new List<TodoTask>();
                    ItemCollection items = this.TodoTaskListView.Items;

                    foreach (var item in items)
                    {
                        TodoTask todoTask = item as TodoTask;
                        list.Add(todoTask);
                        
                    }

                    // create serializer
                    System.Xml.Serialization.XmlSerializer serializer =
                        new System.Xml.Serialization.XmlSerializer(typeof(List<ToDoApp.Wpf.Models.TodoTask>));
                    // this serializer writes to a stream
                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                    serializer.Serialize(stream, list);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);   // reset stream to start

                    // read content from stream
                    System.IO.StreamReader reader = new System.IO.StreamReader(stream);
                    content = reader.ReadToEnd();   // assign XML string to content

                    reader.Dispose();   // dispose the reader
                    stream.Dispose();   // dispose the stream
                }
                else if (dialog.FilterIndex == 4)
                {
                    // write as SOAP
                    // note: have to add a reference to a global assembly (dll) to use in project
                    //create object to serialize

                    // todo
                    System.Collections.ArrayList list = new System.Collections.ArrayList();
                    ItemCollection items = this.TodoTaskListView.Items;

                    foreach (var item in items)
                    {
                        TodoTask todoTask = item as TodoTask;
                        list.Add(todoTask);
                    }

                    

                    // create serializer
                    System.Runtime.Serialization.Formatters.Soap.SoapFormatter serializer =
                        new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
                    // this serializer writes to a stream
                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                    serializer.Serialize(stream, list);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);   // reset stream to start

                    // read content from stream
                    System.IO.StreamReader reader = new System.IO.StreamReader(stream);
                    content = reader.ReadToEnd();   // assign SOAP string to content

                    reader.Dispose();   // dispose the reader
                    stream.Dispose();   // dispose the stream

                }
                else // implies this is last one (binary)
                {
                    System.IO.StringWriter writer = new System.IO.StringWriter();
                    List<TodoTask> items = new List<TodoTask>();
                    foreach (var item in this.TodoTaskListView.Items)
                    {
                        items.Add(item as TodoTask);
                    }

                    foreach (var item in items)
                    {
                        writer.WriteLine(item.Description);
                    }

                    // write as binary
                    // create object to serialize
                    Models.TodoTask document = new Models.TodoTask(content);
                    // create serializer
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer =
                        new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    // this serializer writes to a stream
                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                    serializer.Serialize(stream, document);

                    stream.Seek(0, System.IO.SeekOrigin.Begin);   // reset stream to start
                    // reading and writing binary data directly as string has issues, try not to do it
                    content = Convert.ToBase64String(stream.ToArray());    // assign base64 to content

                    stream.Dispose();   // dispose the stream
                }

                // write content to file
                System.IO.File.WriteAllText(filePath, content);

                /* string path = "output.txt";
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
                 }*/
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA3075:Insecure DTD processing in XML", Justification = "<Pending>")]
        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            //old code that worked with defaul file path
            //string path = "output.txt";

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.Filter = FilterTxt + "|" +
                            FilterJson + "|" +
                            FilterXml + "|" +
                            FilterSoap + "|" +
                            FilterBinary;

            dialog.FilterIndex = 1;
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                string filePath = dialog.FileName;

                if (System.IO.File.Exists(filePath))
                {
                    string content = string.Empty;

                    if (dialog.FilterIndex == 1)
                    {

                        System.IO.StringWriter writer = new System.IO.StringWriter();

                        ItemCollection items = this.TodoTaskListView.Items;
                        foreach (var item in items)
                        {
                            TodoTask todoTask = item as TodoTask;

                            writer.Write(todoTask.LastSaved);
                            writer.Write(todoTask.IsComplete);
                            writer.Write(todoTask.Description);
                        }
                        content = writer.ToString();
                        writer.Dispose();
                    }


                    else if (dialog.FilterIndex == 2)
                    {
                        // read as JSON
                        string json = content;
                        // deserialize to expected type (Models.Document)
                        object jsonObject = Newtonsoft.Json
                            .JsonConvert
                            .DeserializeObject(json, typeof(Models.TodoTask));
                        // cast and assign JSON object to expected type (Models.Document)
                        Models.TodoTask items = (Models.TodoTask)jsonObject;
                        // assign content from deserialized Models.Document
                        content = items.Content;
                    }


                    else if (dialog.FilterIndex == 3)
                    {
                        // read as XML for type of Models.Document
                        System.Xml.Serialization.XmlSerializer serializer =
                            new System.Xml.Serialization.XmlSerializer(typeof(Models.TodoTask));

                        // convert content to byte array (sequence of bytes)
                        byte[] buffer = System.Text.Encoding.ASCII.GetBytes(content);
                        // make stream from buffer
                        System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer);
                        // deserialize stream to an object
#pragma warning disable CA5369 // Use XmlReader For Deserialize
                        object xmlObject = serializer.Deserialize(stream);
#pragma warning restore CA5369 // Use XmlReader For Deserialize
                              // cast and assign XML object to actual type object
                        Models.TodoTask items = (Models.TodoTask)xmlObject;

                        content = items.Content;
                        stream.Dispose();   // release the resources
                    }


                  else if (dialog.FilterIndex == 4)
                    {
                        // read as soap
                        System.Runtime.Serialization.Formatters.Soap.SoapFormatter serializer =
                            new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();

                        // convert content to byte array (sequence of bytes)
                        byte[] buffer = System.Text.Encoding.ASCII.GetBytes(content);
                        // make stream from buffer
                        System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer);
                        // deserialize stream to an object
                        object soapObject = serializer.Deserialize(stream);
                        // cast and assign SOAP object to actual type object
                        Models.TodoTask document = (Models.TodoTask)soapObject;
                        // read content
                        content = document.Content;

                        stream.Dispose();   // release the resources
                    } 


                    else if (dialog.FilterIndex == 5)
                    {
                        // read as binary
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer =
                            new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                        // reading and writing binary data directly as string has issues, try not to do it 
                        byte[] buffer = Convert.FromBase64String(content);
                        System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer);
                        // deserialize stream to object
                        object binaryObject = serializer.Deserialize(stream);
                        // assign binary object to actual type object
                        Models.TodoTask items = (Models.TodoTask)binaryObject;
                        // read the content
                        content = items.Content;
                        stream.Dispose();   // release the resources
                    }
                    else // imply this is any file 
                    {
                        // read as is
                    }

                    // assign content to UI control
                    // ~~~~~~~ UserText.Text = content; ~~~~~~~~~~~~~~~~~~~

                    //old code that worked with defaul file path
                    /*if (System.IO.File.Exists(path))
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
                    }*/

                    //below is part of the save file code
                    /*System.IO.StringWriter writer = new System.IO.StringWriter();
                    List<TodoTask> items = new List<TodoTask>();
                    foreach (var item in this.TodoTaskListView.Items)
                    {
                        items.Add(item as TodoTask);
                    }

                    foreach (var item in items)
                    {
                        writer.WriteLine(item.Description);
                    }*/
                }
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("ToDoApp",
                "By: Mitchell Wommack",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

        }
    }
}