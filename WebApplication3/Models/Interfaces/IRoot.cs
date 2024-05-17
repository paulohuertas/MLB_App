using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLB_App.Models.Interfaces
{
    public interface IRoot<T> where T : class
    {
        List<T> body { get; set; }
    }
}
