using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// inherets from underedoaction. 
    /// </summary>
    public class UndoRedoBackgroundChange : UndoRedoAction
    {
        private Cell cell;
        private uint color;

        // changes background color constructor
        public UndoRedoBackgroundChange(Cell cell, uint color)
        {
            this.cell = cell;
            this.color = color;
        }

        // the performance of the action
        public UndoRedoAction Perform (Spreadsheet spreadsheet)
        {
            uint oldColor = this.cell.BGColor;
            this.cell.BGColor = this.color;
            return new UndoRedoBackgroundChange(this.cell, oldColor);
        }
    }
}
