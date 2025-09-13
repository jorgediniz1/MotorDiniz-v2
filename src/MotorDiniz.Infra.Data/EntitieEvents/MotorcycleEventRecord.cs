using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDiniz.Infra.Data.EntitieEvents
{
    public sealed class MotorcycleEventRecord
    {
        public int Id { get; set; }

        public string EventType { get; set; } = default!;  
        public string Identifier { get; set; } = default!; 
        public int Year { get; set; }
        public string Model { get; set; } = default!;
        public string Plate { get; set; } = default!;

        public DateTime ReceivedAt { get; set; }
        public string PayloadJson { get; set; } = default!;
    }
}
