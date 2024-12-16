﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Repositories.Interfaces
{
    public  interface IGenericRepository<T> where T: class
    {
        T Create(T entity);
        T? Find(string id);
        Task<List<T>> GetAllAsync();
        bool Update(T entity);
        bool Delete(T entity);
    }
}
