using System;

namespace AddinGrades.DTO
{
    public class Coursework
    {
        public string Name  { get; set; }

        private Coursework() {  
        }

        public Coursework(string name)
        {
            Name = name; 
        }

        public override string ToString() => Name;

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return Name == ((Coursework)obj).Name;
        }

    }
}