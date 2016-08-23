using System;
using System.Collections.Generic;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Model
{
    public class UndoService<TState> : IUndoService<TState>
    {

        // Found some inspriation here:
        // https://pradeep1210.wordpress.com/2011/04/09/add-undoredo-or-backforward-functionality-to-your-application/
        //

        private readonly Stack<TState> _undoStates = new Stack<TState>();
        private readonly Stack<TState> _redoStates = new Stack<TState>();

        public TState Undo()
        {
            if (!CanUndo())
                throw new InvalidOperationException("Unable to undo. Not enough items on the undo stack.");

            //Put the current state onto the redo stack
            _redoStates.Push(_undoStates.Pop());

            return _undoStates.Peek();
        }

        public bool CanUndo()
        {
            return _undoStates.Count > 1;
        }

        public TState Redo()
        {
            var state = _redoStates.Pop();

            _undoStates.Push(state);

            return state;
        }

        public bool CanRedo()
        {
            return _redoStates.Count > 0;
        }

        public void Do(TState state)
        {
            _redoStates.Clear();

            _undoStates.Push(state);

            Console.WriteLine("Saved undo state.");
        }

        public void Clear(TState initialState)
        {
            _undoStates.Clear();

            _redoStates.Clear();

            _undoStates.Push(initialState);
        }
    }
    
}