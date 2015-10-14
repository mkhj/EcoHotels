using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoHotels.Core.Domain
{
    public interface IRepository<T>
    {
        T FindBy(int key);

        IEnumerable<T> FindAll();

        void Save(T item);

        void Remove(T item);
    }
}
