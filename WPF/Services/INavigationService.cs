namespace SeaBattle.Services;

public interface INavigationService
{
    void NavigateTo<TViewModel>(object parameter = null);
}