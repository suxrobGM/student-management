using System.Globalization;

namespace StudentManagement.Domain.Entities;

public class Student : Entity
{
    public Student()
    {
        Id = 1;
        BirthDate = new DateTime(2000, 1, 1);
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? SSN { get; set; }
    public string? Major { get; set; }
    public DateTime BirthDate { get; set; }
    public string? Address { get; set; }
    public double GPA { get; set; }

    public string ToRow(IDictionary<string, int> schema, char separator = '|')
    {
        var row = new string[schema.Count];
        foreach (var (header, index) in schema)
        {
            row[index] = header switch
            {
                "Id" => Id.ToString(),
                "FirstName" => FirstName ?? "",
                "LastName" => LastName ?? "",
                "SSN" => SSN ?? "",
                "Major" => Major ?? "",
                "BirthDate" => BirthDate.ToShortDateString() ?? "",
                "Address" => Address ?? "",
                "GPA" => GPA.ToString(CultureInfo.InvariantCulture),
                _ => row[index]
            };
        }

        return string.Join(separator, row);
    }

    public static Student FromSchema(string row, IDictionary<string, int> schema, char separator = '|')
    {
        var rowArr = row.Split(separator);
        var birthDate = DateTime.Parse(rowArr[schema["BirthDate"]]);
        var id = ulong.Parse(rowArr[schema["Id"]]);
        var gpa = double.Parse(rowArr[schema["GPA"]]);
        
        var student = new Student
        {
            Id = id,
            FirstName = rowArr[schema["FirstName"]],
            LastName = rowArr[schema["LastName"]],
            SSN = rowArr[schema["SSN"]],
            Major = rowArr[schema["Major"]],
            BirthDate = birthDate,
            Address = rowArr[schema["Address"]],
            GPA = gpa
        };

        return student;
    }
}