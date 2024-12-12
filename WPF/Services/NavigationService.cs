using System.Windows;
using SeaBattle.ViewModels;

namespace SeaBattle.Services;

public class NavigationService : INavigationService
{
    private readonly Func<Type, object> _viewModelFactory;

    public NavigationService(Func<Type, object> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<TViewModel>(object parameter = null)
    {
        var viewModel = _viewModelFactory(typeof(TViewModel));
        if (viewModel == null)
        {
            throw new InvalidOperationException($"ViewModel of type {typeof(TViewModel).Name} could not be resolved.");
        }

        // assuming convention: ViewModelName => ViewName
        var viewTypeName = typeof(TViewModel).Name.Replace("ViewModel", "View");
        var viewType = Type.GetType($"SeaBattle.Views.{viewTypeName}");

        if (viewType == null)
        {
            throw new InvalidOperationException($"View for ViewModel {typeof(TViewModel).Name} not found.");
        }

        var view = Activator.CreateInstance(viewType) as Window;

        if (view == null)
        {
            throw new InvalidOperationException($"View {viewType.Name} could not be created or is not a Window.");
        }

        view.DataContext = viewModel;

        if (viewModel is IInitializable initializable && parameter != null)
        {
            initializable.InitializeAdditional(parameter);
        }

        var currentWindow = Application.Current.MainWindow;
        view.Show();

        if (currentWindow != view)
        {
            currentWindow?.Close();
        }
    }

}
