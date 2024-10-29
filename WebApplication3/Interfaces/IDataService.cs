using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLB_App.Interfaces
{
    public interface IDataService<T> where T : class
    {
        void Save(List<T> entity);
    }
}
