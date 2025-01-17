using DBLayer.Repositories.Interfaces;
using ServiceLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface IAlertService : GenericServices<AlertDTO, int>
    {

    }
}
