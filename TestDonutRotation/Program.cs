using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Donuts;

namespace TestDonutRotation
{
    class Program
    {
        static void Main(string[] args)
        {
            //CalculateDonutRotation
            DonutRotator rotator = new DonutRotator(@"C:\Users\james.wheeler\Desktop\donut_rotation.xml");
            var details = rotator.ConstructRotationDetails();
        }
    }
}
