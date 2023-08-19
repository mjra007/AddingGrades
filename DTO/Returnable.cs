using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddinGrades.DTO
{
    public readonly record struct Returnable<T>(bool IsSuccess, string Message, T Object);
     

}
