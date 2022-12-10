using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.EntityFrameworkCore;

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
        SaveCommand = new AsyncRelayCommand(SaveAsync);
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


    private bool _canChangeId;
    public bool CanChangeId
    {
        get => _canChangeId;
        set => SetProperty(ref _canChangeId, value);
    }

    public StudentForm StudentForm { get; }

    public IAsyncRelayCommand SaveCommand { get; }
    public IRelayCommand CloseCommand { get; }

    public void Close()
    {
        Messenger.UnregisterAll(this);
    }

    public async void Receive(ValueChangedMessage<ulong?> message)
    {
        if (message.Value != null)
        {
            _id = message.Value;
            EditMode = true;
            CanChangeId = false;
            Title = $"Edit student record {_id}";
            var student = await _repository.GetAsync(_id.Value);

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
            CanChangeId = true;
            Title = "Add a new student";
            await ResetFormAsync();
        }
    }

    private async Task SaveAsync()
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
                await UpdateStudentAsync();
            }
            else
            {
                await AddStudentAsync();
            }

            MessageBox.Show("Student record has been saved successfully");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task AddStudentAsync()
    {
        var exisitingStudent = await _repository.GetAsync(StudentForm.Id);

        if (exisitingStudent != null)
        {
            throw new InvalidOperationException($"The student with id {StudentForm.Id} already exists in the database.");
        }

        await _repository.AddAsync(StudentForm.ToEntity());
        await ResetFormAsync();
    }

    private async Task UpdateStudentAsync()
    {
        var exisitingStudent = await _repository.GetAsync(StudentForm.Id);

        if (exisitingStudent == null)
            return;

        exisitingStudent.FirstName = StudentForm.FirstName;
        exisitingStudent.LastName = StudentForm.LastName;
        exisitingStudent.Address = StudentForm.Address;
        exisitingStudent.Major = StudentForm.Major;
        exisitingStudent.SSN = StudentForm.SSN;
        exisitingStudent.BirthDate = StudentForm.BirthDate;
        exisitingStudent.GPA = StudentForm.GPA;

        await _repository.UpdateAsync(exisitingStudent.Id, exisitingStudent);
    }

    private async Task ResetFormAsync()
    {
        var latestStudent = await _repository.Query().OrderBy(i => i.Id).LastOrDefaultAsync();

        StudentForm.Id = latestStudent?.Id + 1 ?? 1;
        StudentForm.FirstName = string.Empty;
        StudentForm.LastName = string.Empty;
        StudentForm.Address = string.Empty;
        StudentForm.Major = string.Empty;
        StudentForm.SSN = string.Empty;
        StudentForm.BirthDate = new DateTime(2000, 1, 1);
        StudentForm.GPA = 0;
    }
}