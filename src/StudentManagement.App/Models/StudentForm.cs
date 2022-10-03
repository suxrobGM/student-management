using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.App.Models;

public class StudentForm : ObservableValidator
{
    private ulong _id;
    private string? _firstName;
    private string? _lastName;
    private string? _ssn;
    private string? _major;
    private DateTime _birthDate;
    private string? _address;
    private double _gpa;

    public StudentForm()
    {
        Id = 1;
        BirthDate = new DateTime(2000, 1, 1);
    }

    [Required]
    public ulong Id
    {
        get => _id; 
        set => SetProperty(ref _id, value); 
    }

    [Required]
    public string? FirstName 
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    [Required]
    public string? LastName 
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }

    public string? SSN 
    {
        get => _ssn;
        set => SetProperty(ref _ssn, value);
    }

    [Required]
    public string? Major 
    {
        get => _major;
        set => SetProperty(ref _major, value);
    }

    [Required]
    public DateTime BirthDate 
    {
        get => _birthDate;
        set => SetProperty(ref _birthDate, value);
    }

    [Required]
    public string? Address 
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    [Required]
    [Range(0, 4.0)]
    public double GPA 
    {
        get => _gpa;
        set => SetProperty(ref _gpa, value);
    }

    public void Validate()
    {
        ValidateAllProperties();
    }

    public Student ToEntity()
    {
        return new Student
        {
            Id = Id,
            FirstName = FirstName,
            LastName = LastName,
            SSN = SSN,
            Major = Major,
            BirthDate = BirthDate,
            Address = Address,
            GPA = GPA
        };
    }
}
