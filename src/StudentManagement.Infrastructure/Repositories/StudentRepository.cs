using System.Reflection;
using StudentManagement.Infrastructure.Options;

namespace StudentManagement.Infrastructure.Repositories;

internal class StudentRepository : IRepository<Student>
{
    private const char SEPARATOR = '|';
    private readonly string _filePath;
    private readonly IDictionary<string, int> _schema;
    
    public StudentRepository(RepositoryOptions options)
    {
        _filePath = Path.Join(AppContext.BaseDirectory, options.DataFile);

        if (!File.Exists(_filePath))
        {
            File.Create(_filePath).Close();
            InitSchema(typeof(Student));
        }

        _schema = GetSchema();
    }
    
    public Student? Get(ulong id)
    {
        var students = GetAll();
        return students.FirstOrDefault(i => i.Id == id);
    }

    public IList<Student> GetAll()
    {
        var list = new List<Student>();
        var lines = File.ReadAllLines(_filePath);

        if (lines.Length <= 1)
            return list;

        for (var i = 1; i < lines.Length; i++)
        {
            var row = lines[i];
            var student = Student.FromSchema(row, _schema, SEPARATOR);
            list.Add(student);
        }

        return list;
    }

    public void Add(Student entity)
    {
        var existingStudent = Get(entity.Id);

        if (existingStudent != null)
            throw new InvalidOperationException($"Student with ID '{entity.Id}' already exists");
        
        using var writer = File.AppendText(_filePath);
        writer.WriteLine(entity.ToRow(_schema, SEPARATOR));
    }

    public void Update(ulong id, Student entity)
    {
        var students = GetAll();
        var student = students.FirstOrDefault(i => i.Id == id);

        if (student == null)
            return;

        student.Id = entity.Id;
        student.Address = entity.Address;
        student.FirstName = entity.FirstName;
        student.LastName = entity.LastName;
        student.SSN = entity.SSN;
        student.Major = entity.Major;
        student.BirthDate = entity.BirthDate;
        student.GPA = entity.GPA;
        WriteData(students);
    }

    public void Delete(ulong id)
    {
        var students = GetAll();
        var student = students.FirstOrDefault(i => i.Id == id);

        if (student == null)
            return;

        students.Remove(student);
        WriteData(students);
    }
    
    private void WriteData(IEnumerable<Student> students)
    {
        var lines = File.ReadAllLines(_filePath);
        var fileContent = new List<string> { lines[0] };
        fileContent.AddRange(students.Select(student => student.ToRow(_schema)));
        File.WriteAllLines(_filePath, fileContent);
    }

    private IDictionary<string, int> GetSchema()
    {
        var lines = File.ReadAllLines(_filePath);
        var headerIndexes = new Dictionary<string, int>();
        var headerRow = lines[0].Split(SEPARATOR);

        for (var i = 0; i < headerRow.Length; i++)
        {
            var header = headerRow[i];
            headerIndexes.TryAdd(header, i);
        }

        return headerIndexes;
    }

    private void InitSchema(Type entityType)
    {
        var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var propertyNames = properties.Select(i => i.Name);
        var schema = string.Join(SEPARATOR, propertyNames);
        var line = $"{schema}\n";
        File.WriteAllText(_filePath, line);
    }
}