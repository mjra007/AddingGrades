using Microsoft.VisualStudio.Services.Common; 

namespace AddinGrades.DTO
{
    public class StudentsCache
    {
        public SerializableDictionary<string, List<string>> StudnetsByClass = new();
        public DateTime DateOfCache;
    }
}
