using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddinGrades
{
    public interface IUpdater
    {

        string OriginVersion {  get; }
        string FinalVersion { get; }

        void Update();

    }
}
