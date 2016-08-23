namespace CaptiveAire.VPL.Interfaces
{
    public interface IUndoService<TState>
    {
        TState Undo();

        bool CanUndo();

        TState Redo();

        bool CanRedo();

        void Do(TState state);

        void Clear(TState initialState);
    }
}