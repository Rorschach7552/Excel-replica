using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpreadsheetEngine
{
    /// <summary>
    /// cell class is abstract to restrict access by user to cell engine.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        private uint bgcolor = 0xFFFFFFF;
        protected string text;
        protected string value;

        // gettersetter for BGColor
        public uint BGColor
        {
            get
            {
                return this.bgcolor;
            }

            set
            {
                if (value != this.bgcolor)
                {
                    this.bgcolor = value;
                    this.OnPropertyChanged("BGColor");
                }
            }
        }

        public int RowIndex { get; set; }

        public int ColumnIndex { get; set; }

        // constructor for cell class
        public Cell()
        {
            this.text = string.Empty;
            this.value = string.Empty;
            this.RowIndex = 0;
            this.ColumnIndex = 0;
        }

        // changed even handler
        public event PropertyChangedEventHandler PropertyChanged;

        // method to create a new cell
        public Cell CreateCell()
        {
            return this;
        }

        // getter/setter for text in cell
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (value != this.text)
                {
                    this.text = value;
                    this.OnPropertyChanged("Text");
                }
            }

        }

        // returns or gets value of cell
        public string Value
        {
            get
            {
                return this.value;
            }

            set
            {
                if (Environment.StackTrace.Contains("Spreadsheet"))
                {
                    this.value = value;
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
