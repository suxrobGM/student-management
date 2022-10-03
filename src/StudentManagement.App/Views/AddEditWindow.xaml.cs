namespace StudentManagement.App.Views;

/// <summary>
/// Interaction logic for AddEditWindow.xaml
/// </summary>
public partial class AddEditWindow : Window
{
    public AddEditWindow()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetService<AddEditWindowViewModel>();
    }
}
