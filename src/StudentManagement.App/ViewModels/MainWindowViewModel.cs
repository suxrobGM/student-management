using System.Collections.ObjectModel;
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
        _students = FetchData();
        AddCommand = new RelayCommand(OpenAddWindow);
        EditCommand = new RelayCommand(OpenEditWindow, () => SelectedStudent != null);
        RemoveCommand = new RelayCommand(RemoveData, () => SelectedStudent != null);
        SearchCommand = new RelayCommand<SfDataGrid>(Search);
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

    private ObservableCollection<Student> _students;
    public ObservableCollection<Student> Students 
    {
        get => _students;
        set => SetProperty(ref _students, value);
    }

    public IRelayCommand AddCommand { get; }
    public IRelayCommand EditCommand { get; }
    public IRelayCommand RemoveCommand { get; }
    public IRelayCommand<SfDataGrid> SearchCommand { get; }

    private void OpenAddWindow()
    {
        var addWindow = new AddEditWindow();
        addWindow.ShowDialog();
        Students = FetchData();
    }

    private void OpenEditWindow()
    {
        if (SelectedStudent == null)
            return;
        
        var editWindow = new AddEditWindow();
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ulong?>(SelectedStudent.Id));
        editWindow.ShowDialog();
        Students = FetchData();
    }

    private void RemoveData()
    {
        var student = SelectedStudent;

        if (student == null)
            return;

        var desc = $"Do you want to delete student '{student.FirstName} {student.LastName}'?";
        var result = MessageBox.Show(desc, "Attention", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            _repository.Delete(student.Id);
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

    private ObservableCollection<Student> FetchData()
    {
        var studentsList = _repository.GetAll();
        return new ObservableCollection<Student>(studentsList);
    }
}