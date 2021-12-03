using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bar_simulator
{
    class Visitors
    {
        public int age { get; set; }
        public int anger { get; set; }
        public double budget { get; set; }
       
        public Visitors(int age, int anger, double budget)
        {
            this.age = age;
            this.anger = anger;
            this.budget = budget;
        }

    }
}
