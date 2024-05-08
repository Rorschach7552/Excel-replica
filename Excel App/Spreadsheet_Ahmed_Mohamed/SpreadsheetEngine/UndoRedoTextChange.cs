using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// inherets from redo action takes perform action in cell. 
    /// </summary>
    public class UndoRedoTextChange : UndoRedoAction
    {
        private Cell cell;
        private string text;

        // constructor.
        public UndoRedoTextChange(Cell cell, string text)
        {
            this.text = text;
            this.cell = cell;
        }

        // method to undoredo action for spreadsheet.
        public UndoRedoAction Perform(Spreadsheet spreadsheet)
        {
            Cell currentCell=spreadsheet.GetCell(this.cell.RowIndex, this.cell.ColumnIndex);
            string oldText = currentCell.Text;
            currentCell.Text = this.text;
            return new UndoRedoTextChange(currentCell, oldText);
        }
    }
}
