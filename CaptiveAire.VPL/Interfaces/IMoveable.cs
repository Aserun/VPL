using System.Windows;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IMoveable
    {
        void StartMove();

        void ContinueMove(Vector vector);

        void CancelMove();

        void CompleteMove(Vector vector);
    }
}