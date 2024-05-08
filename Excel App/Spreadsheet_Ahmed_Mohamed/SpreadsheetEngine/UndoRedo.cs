using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// interface for undoredoAction to limit user access to backend.
    /// </summary>
    public interface UndoRedoAction
    {
        UndoRedoAction Perform(Spreadsheet spreadsheet);
    }

    /// <summary>
    /// contains two stacks for undo and redo. 
    /// </summary>
    public class UndoRedo
    {
        private Stack<Actions> undoStack = new Stack<Actions> ();
        private Stack<Actions> redoStack = new Stack<Actions> ();

        // adds undo action 
        public void AddUndoAction(Actions actions)
        {
            undoStack.Push (actions);
            redoStack.Clear ();

        }

        // pop from undo push onto redo
        public void Undo(Spreadsheet spreadsheet)
        {
            Actions action = undoStack.Pop ();
            this.redoStack.Push (action.Copy(spreadsheet));
        }

        // pop from redo push onto undo 
        public void Redo(Spreadsheet spreadsheet)
        {
            Actions action = redoStack.Pop();
            this.undoStack.Push(action.Copy(spreadsheet));
        }

        // checks if stack is empty 
        public bool IsUndoEmpty()
        {
            if (this.undoStack.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // checks if stack is empty
        public bool  IsRedoEmpty()
        {
            if (this.redoStack.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // undo title of button
        public string UndoTitle
        {
            get
            {
                if (undoStack.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return undoStack.Peek().title;
                }
            }
        }

        // redo title stack
        public string RedoTitle
        {
            get
            {
                if (redoStack.Count == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return redoStack.Peek().title;
                }
            }
        }
    }
    /// <summary>
    /// this class contains array  for redo action. 
    /// </summary>
    public class Actions
    {
        private UndoRedoAction[] commands;
        public string title;

        // constructor 
        public Actions ()
        {
        }

        // constructor input array and title.
        public Actions (UndoRedoAction[] commands, string title)
        {
            this.commands = commands;
            this.title = title;
        }

        // constructor input a list and title.
        public Actions (List <UndoRedoAction> commands , string title)
        {
            this.commands = commands.ToArray();
            this.title = title;
        }

        // copy spreadsheat into commands list
        public Actions Copy(Spreadsheet spreadsheet)

        {
            List<UndoRedoAction> commands = new List<UndoRedoAction> ();
            foreach (UndoRedoAction command in this.commands)
            {
                commands.Add(command.Perform(spreadsheet));
            }

            return new Actions(commands.ToArray(), title);
        }
    }
}
