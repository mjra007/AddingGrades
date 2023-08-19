using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddinGrades.DTO
{
    public class GradeSheet
    {
        public readonly List<Coursework> Coursework;
        public readonly List<CourseworkWeightedTable> CourseworkWeightedTables = new();

        public GradeSheet()
        {
            Coursework = new();
        }

        public void AddCoursework(string nameOfCoursework)
        {
            Coursework.Add(new Coursework(nameOfCoursework));
        }

        public Returnable<CourseworkWeightedTable> CreateNewWeightedTable(string nameOfWeightedTable)
        {
            if (CourseworkWeightedTables.Any(s => s.name.Equals(nameOfWeightedTable)))
            {
                return new(false, "Coursework weighted table with that name already exists", Object: null);
            }
            else
            {
                CourseworkWeightedTable weightedTable;
                CourseworkWeightedTables.Add(weightedTable = new CourseworkWeightedTable(nameOfWeightedTable));
                return new(true, "Coursework weighted table created successfully!", weightedTable);
            } 
        }

        public Returnable<CourseworkWeightedTable> GetWeightedTable(string name)
        {
            foreach (var item in CourseworkWeightedTables)
            {
                if (item.name.Equals(name)) return new Returnable<CourseworkWeightedTable>(true, "Found it!", item);
            }
            return new(false, "Did not find coursework weighted table with name provided!", null);
        }

        public Returnable<Coursework> AddNewCoursework(string name)
        {
            if (Coursework.Any(s => s.Name.Equals(name)))
            {
                return new(false, "A coursework with the same name already exists!", null);
            }
            else
            {
                Coursework coursework;
                Coursework.Add(coursework = new(name));
                return new(true, "Coursework was created", coursework);
            }
        }

        public Returnable<Coursework> GetCoursework(string name)
        {
            return Coursework.Any(s => s.Name.Equals(name)) ?
                new(true, "Found", Coursework.Find(s => s.Name.Equals(name, StringComparison.Ordinal))) :
                new(false, "Could not find a coursework with that name!", null);
        }

        public Returnable<Coursework> DeleteCoursework(string name)
        {
            if (Coursework.Any(s => s.Name.Equals(name)))
            {
                Coursework course = Coursework.Find(s => s.Name.Equals(name, comparisonType: StringComparison.Ordinal));
                Coursework.Remove(course);
                return new(true, "Coursework deleted succcessfully", course);
            }
            else
            {
                return new(false, "Did not find any coursework with that name", null);
            }
        }


    }
}
