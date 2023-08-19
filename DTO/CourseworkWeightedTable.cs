using Microsoft.VisualStudio.Services.Common;

namespace AddinGrades.DTO
{
    public class CourseworkWeightedTable
    {
        public SerializableDictionary<Coursework, double> weights = new();
        public string name;

        private CourseworkWeightedTable()
        {

        }

        public CourseworkWeightedTable(string name, IEnumerable<Coursework> coursework, double[] weigts)
        {
            var keysAndValues = coursework.Zip(weigts, (key, value) =>KeyValuePair.Create(key, value));
            this.weights = new SerializableDictionary<Coursework, double>().AddRange(keysAndValues);
            this.name = name;
        }

        public CourseworkWeightedTable(string name)
        {
             weights = new();
             this.name= name;
        }

        public void ChangeWeight(Coursework coursework, double newWeight)
        {
            weights[coursework] = newWeight;
        }

        public void RemoveCoursework(Coursework coursework)=> weights.Remove(coursework);

        public bool AddCoursework(Coursework coursework, double weight) {
            if (weights.ContainsKey(coursework))
            {
                return false;
            }
            else
            {
                weights.Add(coursework, weight);
                return true;
            }
        }
    }
}