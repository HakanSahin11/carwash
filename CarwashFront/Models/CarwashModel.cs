using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarwashFront.Models
{
    class CarwashModel
    {
        public CarwashModel(int carwash, Task task)
        {
            Carwash = carwash;
            this.Task = task;
        }

        public int Carwash { get; set; }
        public Task Task { get; set; }
    }
    public class CarwashRunModel 
    {
        public string Status { get; set; }
        public string Numberplate { get; set; }
        public string UserName { get; set; }
        public string StartTime { get; set; }
        public int Carwash { get; set; }
    }
}
