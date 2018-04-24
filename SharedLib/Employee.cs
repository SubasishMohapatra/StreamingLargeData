using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib
{
    [Serializable]    
    public class Employee
    {
        public int EmpID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
