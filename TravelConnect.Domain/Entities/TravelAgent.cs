using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelConnect.Domain.Entities.Base;

namespace TravelConnect.Domain.Entities;

public class TravelAgent : EntityBase
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<Hotel> HotelsManaged { get; set; } = new();
}
