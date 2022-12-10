using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Syncfusion.UI.Xaml.Grid;

namespace StudentManagement.App.ViewModels;

public class MainWindowViewModel : ObservableRecipient
{
    private readonly IRepository<Student> _repository;

    public MainWindowViewModel(IRepository<Student> repository)
    {
        _repository = repository;
        Students = new ObservableCollection<Student>();
        AddCommand = new AsyncRelayCommand(OpenAddWindow);
        EditCommand = new AsyncRelayCommand(OpenEditWindow, () => SelectedStudent != null);
        RemoveCommand = new AsyncRelayCommand(RemoveData, () => SelectedStudent != null);
        SearchCommand = new RelayCommand<SfDataGrid>(Search);
        Task.Run(UpdateStudensListAsync);
    }

    private Student? _selectedStudent;
    public Student? SelectedStudent
    {
        get => _selectedStudent;
        set
        {
            SetProperty(ref _selectedStudent, value);
            EditCommand.NotifyCanExecuteChanged();
            RemoveCommand.NotifyCanExecuteChanged();
        }
    }

    private string? _searchQuery;
    public string? SearchQuery
    {
        get => _searchQuery;
        set => SetProperty(ref _searchQuery, value);
    }

    private bool _loadedData;
    public bool LoadedData
    {
        get => _loadedData;
        set => SetProperty(ref _loadedData, value);
    }

    public ObservableCollection<Student> Students { get; }

    public IAsyncRelayCommand AddCommand { get; }
    public IAsyncRelayCommand EditCommand { get; }
    public IAsyncRelayCommand RemoveCommand { get; }
    public IRelayCommand<SfDataGrid> SearchCommand { get; }

    private async Task UpdateStudensListAsync()
    {
        LoadedData = false;
        var students = await FetchDataAsync();
        Students.Clear();

        foreach (var student in students)
        {
            Students.Add(student);
        }

        LoadedData = true;
    }

    private async Task OpenAddWindow()
    {
        var addWindow = new AddEditWindow();
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ulong?>(null));
        addWindow.ShowDialog();
        await UpdateStudensListAsync();
    }

    private async Task OpenEditWindow()
    {
        if (SelectedStudent == null)
            return;
        
        var editWindow = new AddEditWindow();
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ulong?>(SelectedStudent.Id));
        editWindow.ShowDialog();
        await UpdateStudensListAsync();
    }

    private async Task RemoveData()
    {
        var student = SelectedStudent;

        if (student == null)
            return;

        var desc = $"Do you want to delete student '{student.FirstName} {student.LastName}'?";
        var result = MessageBox.Show(desc, "Attention", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            await _repository.DeleteAsync(student.Id);
            Students.Remove(student);
        }
    }

    private void Search(SfDataGrid? dataGrid)
    {
        if (dataGrid == null)
            return;

        dataGrid.SearchHelper.AllowFiltering = true;
        dataGrid.SearchHelper.Search(SearchQuery);
    }

    private async Task<ObservableCollection<Student>> FetchDataAsync()
    {
        var studentsList = await _repository.GetAllAsync();
        return new ObservableCollection<Student>(studentsList);
    }
}