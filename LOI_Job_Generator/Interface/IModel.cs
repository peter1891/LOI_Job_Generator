using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOI_Job_Generator.Interface
{
    public interface IModel<T>
    {
        static abstract void Create(T model);

        static abstract void Delete(int i);

        static abstract List<T> Read();

        static abstract void Update(T model, int i);
    }
}
