using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadsolidChallenge.Shared
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? ImagemUrl { get; set; }
        public List<Availability> Availability { get; set; } = new();
    }

    public class Availability
    {
        public int Id { get; set; }
        public DateTime inicialDate { get; set; }
        public DateTime endDate { get; set; }
        public int EquipmentId { get; set; }
    }
}
