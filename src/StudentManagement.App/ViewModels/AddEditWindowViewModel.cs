using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace StudentManagement.App.ViewModels;

public class AddEditWindowViewModel : ObservableRecipient, IRecipient<ValueChangedMessage<ulong?>>
{
    private ulong? _id;
    private readonly IRepository<Student> _repository;

    public AddEditWindowViewModel(IRepository<Student> repository)
    {
        _repository = repository;

        _editMode = false;
        _title = "Add a new student";
        Messenger.Register(this);

        StudentForm = new StudentForm();
        SaveCommand = new RelayCommand(Save);
        CloseCommand = new RelayCommand(Close);
    }

    private string _title;
    public string Title 
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private bool _editMode;
    public bool EditMode
    { 
        get => _editMode; 
        set => SetProperty(ref _editMode, value);
    }

    public StudentForm StudentForm { get; }

    public IRelayCommand SaveCommand { get; }
    public IRelayCommand CloseCommand { get; }

    private void Save()
    {
        StudentForm.Validate();

        if (StudentForm.HasErrors)
        {
            var errors = string.Join('\n', StudentForm.GetErrors().Select(i => i.ErrorMessage)); 
            MessageBox.Show(errors, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        try
        {
            if (EditMode)
            {
                _repository.Update(_id!.Value, StudentForm.ToEntity());
            }
            else
            {
                _repository.Add(StudentForm.ToEntity());
                ResetForm();
            }

            MessageBox.Show("Student record has been saved successfully");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void Close()
    {
        Messenger.UnregisterAll(this);
    }

    public void Receive(ValueChangedMessage<ulong?> message)
    {
        if (message.Value != null)
        {
            _id = message.Value;
            EditMode = true;
            Title = $"Edit student record {_id}";
            var student = _repository.Get(_id.Value);

            StudentForm.Id = student!.Id;
            StudentForm.FirstName = student.FirstName;
            StudentForm.LastName = student.LastName;
            StudentForm.Address = student.Address;
            StudentForm.Major = student.Major;
            StudentForm.SSN = student.SSN;
            StudentForm.BirthDate = student.BirthDate;
            StudentForm.GPA = student.GPA;
        }
        else
        {
            EditMode = false;
            Title = "Add a new student";
        }
    }

    private void ResetForm()
    {
        StudentForm.Id = 1;
        StudentForm.FirstName = string.Empty;
        StudentForm.LastName = string.Empty;
        StudentForm.Address = string.Empty;
        StudentForm.Major = string.Empty;
        StudentForm.SSN = string.Empty;
        StudentForm.BirthDate = new DateTime(2000, 1, 1);
        StudentForm.GPA = 0;
    }
}