using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertAPI.Models
{
    public enum AdvertStatus
    {
        Pending = 1,
        Active = 2
    }

    public class ConfirmAdvertModel
    {
        public string Id { get; set; }
        public string FilePath { get; set; }
        public AdvertStatus Status { get; set; }
    }
}
