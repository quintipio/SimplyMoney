using CompteWin10.Abstract;

namespace CompteWin10.Interface
{
    public interface IView<T> where T:AbstractViewModel
    {
        T ViewModel { get; set; }
    }
}
