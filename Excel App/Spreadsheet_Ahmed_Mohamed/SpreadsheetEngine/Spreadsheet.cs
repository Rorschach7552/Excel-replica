using CptS321;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SpreadsheetEngine
{
    /// <summary>
    /// TextCell inherets from cell class.
    /// </summary>
    public class TextCell : Cell
    {
        // constructor.
        public TextCell()
            : base()
        {
        }
    }

    // Spreadsheet inherets from inotify to be able to declare changes from user into engine
    public class Spreadsheet : INotifyPropertyChanged
    {
        private PropertyChangedEventHandler propertyChangedEventHandler;

        public event PropertyChangedEventHandler PropertyChanged;

        public Cell[,] SpreadSheetArray;
        private ExpressionTree tree;
        private Dictionary<string, HashSet<string>> dependency;

        private int RowCount { get { return this.SpreadSheetArray.GetLength(0); } }

        private int ColCount { get { return this.SpreadSheetArray.GetLength(1); } }

        // getter for cell
        public Cell? GetCell(int row, int column)
        {
            if (row < this.RowCount && row >= 0 && column < this.ColCount && column >= 0)
            {
                return this.SpreadSheetArray[row, column];
            }
            else
            {
                return null;
            }
        }

        // method input row/col index adds value in specified index and invoke cell property changed.
        public Spreadsheet(int rowIndex, int colIndex) 
        {
            this.SpreadSheetArray = new Cell[rowIndex, colIndex];
            this.propertyChangedEventHandler = this.CellPropertyChanged;
            this.dependency = new Dictionary<string,HashSet<string>>();
            for (int i = 0; i < rowIndex; i++)
            {
                    for (int j = 0; j < colIndex; j++)
                    {
                        this.SpreadSheetArray[i, j] = new TextCell();
                        this.SpreadSheetArray[i, j].RowIndex = i;
                        this.SpreadSheetArray[i, j].ColumnIndex = j;
                        this.SpreadSheetArray[i, j].Text = string.Empty;
                        this.SpreadSheetArray[i, j].PropertyChanged += this.CellPropertyChanged;
                    }
            }

            this.tree = new ExpressionTree(string.Empty);
            for (char c = 'A'; c <= 'Z'; c++)
            {
                for (int r = 1; r <= 50; r++)
                {
                    this.tree.SetVariable(c.ToString() + r.ToString(), 0);
                }
            }
        }

        // Cell property changed methot for celchanged event
        private void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell changedCell;
            int changedRow = ((Cell)sender).RowIndex;
            int changedColumn = ((Cell)sender).ColumnIndex;

            if (sender is Cell)
            {
                changedCell = this.GetCell(changedRow, changedColumn);
                if (e.PropertyName.Equals("Text"))
                {
                    if (changedCell != null && changedCell.Text.Length > 0 && changedCell.Text[0] == '=')
                    {
                        try
                        {
                            this.tree.Expression = changedCell.Text.Substring(1);
                            changedCell.Value = this.tree.Evaluate().ToString();
                            string cellName = Convert.ToChar(changedColumn + 65).ToString() + (changedRow + 1).ToString();

                            this.tree.SetVariable(cellName, this.tree.Evaluate());
                            AddDependencies(changedCell.Text.Substring(1), cellName);

                            UpdateDependedCells(cellName);
                        }
                        catch (InvalidCastException exception)
                        {
                            throw new InvalidCastException("Illegal cast in CellPropertyChanged.", exception);
                        }
                    }
                    else
                    {
                        changedCell.Value = changedCell.Text;
                        string cellName = Convert.ToChar(changedColumn + 65).ToString() + (changedRow + 1).ToString();
                        double value;
                        if (Double.TryParse(changedCell.Value, out value))
                        {
                            tree.SetVariable(cellName, value);
                        }
                        else
                        {
                            this.tree.SetVariable(cellName, 0);
                        }

                        UpdateDependedCells(cellName);
                    }
                }
                else if (e.PropertyName.Equals("BGColor"))
                {
                    this.RaiseCellPropertyChanged(changedCell.RowIndex, changedCell.ColumnIndex);
                }

                this.RaiseCellPropertyChanged(changedRow, changedColumn);
            }
            else
            {
                throw new Exception("Sender was not type Cell.");
            }
        }

        // raise changein even a cell value changed .
        private void RaiseCellPropertyChanged(int row, int column)
        {
            this.PropertyChanged?.Invoke(this.SpreadSheetArray[row, column], new PropertyChangedEventArgs("Cell"));
        }

        // convers a char to col index in ints.
        private int ConvertCharToColumnIndex(char character)
        {
            int asciiForA = 65;
            return (int)character - asciiForA + 1;
        }

        private char ConverColumnToCharIndex(int column)
        {
            return (char)(column + 65);
        }

        // converts cell name into  letters from A to Z.
        private bool IsCellName(string text)
        {
            char firstLetter= text[0];
            if (firstLetter < 'A' || firstLetter > 'Z')
            {
                return false;
            }
            else
            {
                string number = text.Substring(1);
                double value;
                if (Double.TryParse(number, out value))
                {
                    if (value >= 1 && value <= 50)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public void SaveSpreadsheet(Stream xml)
        {
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            XmlWriter xmlWriter = XmlWriter.Create(xml, xmlSettings);
            using (xmlWriter) 
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("spreadsheet");
                foreach (Cell cell in this.SpreadSheetArray)
                {
                    if ( string.IsNullOrEmpty(cell.Value) == false || cell.BGColor != 268435455)
                    {
                        xmlWriter.WriteStartElement("cell");
                        xmlWriter.WriteAttributeString("name", this.ConverColumnToCharIndex(cell.ColumnIndex) + (cell.RowIndex + 1).ToString());
                        xmlWriter.WriteElementString("bgcolor", cell.BGColor.ToString());
                        xmlWriter.WriteElementString("text", cell.Text);
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }

        }

        public void LoadSpreadsheet(Stream xml)
        {
            foreach (Cell cell in this.SpreadSheetArray)
            {
                cell.Text = string.Empty;
                cell.BGColor = 268435455;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xml);
            XmlNode spreadsheetNode = xmlDoc.SelectSingleNode("spreadsheet");
            XmlNodeList cellNodes = spreadsheetNode.SelectNodes("cell");
            foreach(XmlNode cellNode in cellNodes)
            {
                string cellName = cellNode.Attributes.GetNamedItem("name").Value;
                int rowIndex = int.Parse(cellName.Substring(1)) - 1;
                int colIndex = ConvertCharToColumnIndex(cellName[0]) -1;
                Cell cell = GetCell(rowIndex, colIndex);
                if (cellNode.SelectSingleNode("text").InnerText != string.Empty)
                {
                    cell.Text = cellNode.SelectSingleNode("text").InnerText;
                }
                if (cellNode.SelectSingleNode("bgcolor").InnerText != "268435455")
                {
                    uint color;
                    if (uint.TryParse(cellNode.SelectSingleNode("bgcolor").InnerText, out color))
                    {
                        cell.BGColor = color;
                    }
                }
            }
        }

        private void UpdateDependedCells(string cellName)
        {
            Stack<string> stack = new Stack<string>();
            stack.Push(cellName);
            while (stack.Count > 0)
            {
                string updatedCellName = stack.Pop();
                if (this.dependency.ContainsKey(updatedCellName)==true)
                {
                    foreach (string dependentCell in this.dependency[updatedCellName])
                    {
                        int rowIndex = int.Parse(dependentCell.Substring(1)) - 1;
                        int colIndex = ConvertCharToColumnIndex(dependentCell[0]) - 1;
                        Cell cell = GetCell(rowIndex,colIndex);
                        tree.Expression = cell.Text.Substring(1);
                        cell.Value = tree.Evaluate().ToString();
                        tree.SetVariable(dependentCell,tree.Evaluate());
                        this.RaiseCellPropertyChanged(rowIndex,colIndex);
                        stack.Push(dependentCell);
                    }
                }
            }
        }

        private void AddDependencies(string cellText, string dependentCell)
        {
            cellText = cellText.Replace(" ", "");
            char[] delimiters = new char[] { '+', '-', '*', '/' };
            string[] tokens = cellText.Split(delimiters);
            foreach (string token in tokens)
            {
                if(IsCellName(token))
                {
                    if(this.dependency.ContainsKey(token)==false)
                    {
                        this.dependency[token] = new HashSet<string>();
                    }
                    this.dependency[token].Add(dependentCell);
                }
            }
        }
    }
}
