namespace ToDoApp.Wpf.Models
{
    /// <summary>
    /// Represents a document
    /// </summary>
    [System.Serializable]
    public class TodoTask
    {
        public string Description { get; set; }

        // NOTE: to use binary or soap formatting for serialization the [Serializable] attribute is required

        /// <summary>
        /// Gets, sets the content of the document
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets, sets the date and time last saved
        /// </summary>
        public System.DateTime LastSaved { get; set; }

        /// <summary>
        /// Default, parameter-less constructor
        /// </summary>
        public TodoTask()
        {
            Content = string.Empty;
            LastSaved = System.DateTime.Now;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Content for instance</param>
        public TodoTask(string content)
        {
            Content = content;
            LastSaved = System.DateTime.Now;
        }
    }
}
