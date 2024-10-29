using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLB_App.Models;
using MLB_App.Models.Data;

namespace MLB_App.Interfaces
{
    public interface IPlayerManagement : IServiceData<Player>
    {
        IEnumerable<Player> GetAllPlayers();
    }
}
