using SpreadsheetEngine;
using System.ComponentModel;

namespace Spreadsheet_Ahmed_Mohamed
{
    /// <summary>
    /// form for datagrid. 
    /// </summary>
    public partial class Form1 : Form
    {
        private PropertyChangedEventHandler propertyChangedEventHandler;
        private Spreadsheet spreadsheet;
        public UndoRedo undoRedo = new UndoRedo();

        // constructor for form1.
        public Form1()
        {
            this.InitializeComponent();
            this.propertyChangedEventHandler = CellChangedHandler;

            this.Load += new EventHandler(this.Form1_Load);

            this.spreadsheet = new Spreadsheet(50, 26);
            this.spreadsheet.PropertyChanged += this.CellChangedHandler;
            this.GridData.CellEndEdit += this.GridData_CellEndEdit;
            this.GridData.CellBeginEdit += this.GridData_CellBeginEdit;
        }

        // load dataggridview
        private void Form1_Load(object sender, EventArgs e)
        {
            this.SetupDataGridView();
        }

        // change size of grid
        private void SetupDataGridView()
        {
            this.GridData.Columns.Clear();
            this.GridData.Rows.Clear();
            this.GridData.AutoGenerateColumns = false;
            this.GridData.AutoResizeColumns();

            this.GridData.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            this.GridData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.GridData.ColumnHeadersDefaultCellStyle.Font =
                                    new Font(this.GridData.Font, FontStyle.Bold);

            int asciiForA = 65;
            for (int column = 1; column <= 26; column++)
            {
                string columnName = ((char)(column - 1 + asciiForA)).ToString();
                string headerText = ((char)(column - 1 + asciiForA)).ToString();
                this.GridData.Columns.Add(columnName, headerText);
            }

            object[] defaultRow = new object[GridData.ColumnCount];

            for (int row = 1; row <= 50; row++)
            {

                this.GridData.Rows.Add(defaultRow);
                this.GridData.Rows[row - 1].HeaderCell.Value = row.ToString();
            }

            Console.WriteLine("ColumnCount should be : " + this.GridData.ColumnCount);
            Console.WriteLine("RowCount should be : " + this.GridData.RowCount);
        }

        // if change happen in cell
        private void CellChangedHandler(object sender, EventArgs e)
        {
            Console.WriteLine("CellChangedHandler  -  RowIndex, ColIndex  :  " + ((Cell)sender).RowIndex + ", " + ((Cell)sender).ColumnIndex);

            if (sender is Cell)
            {
                this.GridData.Rows[((Cell)sender).RowIndex].Cells[((Cell)sender).ColumnIndex].Value = ((Cell)sender).Value;
                this.GridData.Rows[((Cell)sender).RowIndex].Cells[((Cell)sender).ColumnIndex].Style.BackColor = Color.FromArgb((int)((Cell)sender).BGColor);
            }
        }

        // CellEnd edit for griddata
        private void GridData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string text;
            int row = e.RowIndex;
            int column = e.ColumnIndex;
            UndoRedoAction[] undoRedoActions = new UndoRedoAction[1];
            Cell? editedCell = this.spreadsheet.GetCell( row, column);
            if (this.GridData.Rows[row].Cells[column].Value!= null)
            {
                text = GridData.Rows[row].Cells[column].Value.ToString();
            }
            else
            {
                text = string.Empty;
            }
            undoRedoActions[0] = new UndoRedoTextChange(editedCell , editedCell.Text);

            editedCell.Text = text;
            undoRedo.AddUndoAction(new Actions(undoRedoActions, "cell text change"));
            this.GridData.Rows[row].Cells[column].Value = editedCell.Value;
            UpdateEditMenu();
        }

        // CellBeginedit for griddata
        private void GridData_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int row = e.RowIndex;
            int column = e.ColumnIndex;
            Cell? selectedCell = this.spreadsheet.GetCell(row, column );
            this.GridData.Rows[row].Cells[column].Value = selectedCell.Text;

        }

        // eventhandler if value changed
        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string msg = string.Format("Cell at row {0}, column {1} value changed", e.RowIndex, e.ColumnIndex);
            MessageBox.Show(msg, "Cell Value Changed");
            this.spreadsheet.SpreadSheetArray[e.RowIndex, e.ColumnIndex].Text = this.GridData[e.RowIndex, e.ColumnIndex].Value.ToString();
        }

        /// <summary>
        /// Run the Demo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int row, column;
            for (int x = 0; x < 50; x++)
                {
                    row = random.Next(50);
                    column = random.Next(26);
                    this.spreadsheet.SpreadSheetArray[row, column].Text = "jim";
            }

            for (row = 0; row < 50; row++)
            {
                this.spreadsheet.SpreadSheetArray[row, 1].Text = "This is cell B" + row;
            }

            for (row = 0; row < 50; row++)
            {
                this.spreadsheet.SpreadSheetArray[row, 0].Text = "=B" + row;

            }
        }

        /// <summary>
        /// if u select new color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeBackgrounColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            uint selectedColor = 0;
            UndoRedoAction [] undoRedoActions = new UndoRedoAction [this.GridData.SelectedCells.Count];
            if (colorDialog.ShowDialog()==DialogResult.OK)
            {
                selectedColor = (uint)colorDialog.Color.ToArgb();
                int cellIndex = 0;
                foreach (DataGridViewCell cell in this.GridData.SelectedCells)
                {
                    Cell spreadSheetCell = this.spreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex);
                    undoRedoActions [cellIndex] = new UndoRedoBackgroundChange(spreadSheetCell,spreadSheetCell.BGColor);
                    cellIndex++;
                    spreadSheetCell.BGColor = selectedColor;
                    this.GridData.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Style.BackColor = Color.FromArgb((int)spreadSheetCell.BGColor);
                }
                undoRedo.AddUndoAction(new Actions(undoRedoActions, "changing cell background color"));
                UpdateEditMenu();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undoRedo.Undo(spreadsheet);
            UpdateEditMenu();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undoRedo.Redo(spreadsheet);
            UpdateEditMenu();
        }

        // method to update undo redo depend of wether they contain values
        private void  UpdateEditMenu ()
        {
            ToolStripMenuItem editMenu = (ToolStripMenuItem)this.menuStrip1.Items[1];
            foreach (ToolStripItem subMenu in editMenu.DropDownItems)
            {
                if (subMenu.Text.Contains("Undo"))
                {
                    if (this.undoRedo.IsUndoEmpty())
                    {
                        subMenu.Enabled = false;
                    }
                    else
                    {
                        subMenu.Enabled = true;
                    }
                    subMenu.Text = "Undo " + undoRedo.UndoTitle;
                }
                else if (subMenu.Text.Contains("Redo"))
                {
                    if (this.undoRedo.IsRedoEmpty())
                    {
                        subMenu.Enabled = false;
                    }
                    else
                    {
                        subMenu.Enabled = true;
                    }

                    subMenu.Text = "Redo " + undoRedo.RedoTitle;
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.AddExtension = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = saveFileDialog.OpenFile();
                spreadsheet.SaveSpreadsheet(stream);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                spreadsheet.LoadSpreadsheet(stream);
                stream.Dispose();
                undoRedo = new UndoRedo();
                UpdateEditMenu();
            }
        }
    }
}