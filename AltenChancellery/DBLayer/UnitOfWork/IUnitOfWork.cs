﻿using DBLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.UnitOfWork
{
    public interface IUnitOfWork :IDisposable
    {
        IOfficeRepository OfficeRepository { get; }
        IItemOfficeRepository itemOfficeRepository { get; }
        IItemRepository itemRepository { get; }
        Task<int> Save();
    }
}
