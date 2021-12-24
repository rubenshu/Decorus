﻿using Decorus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorus.DataAccess.Repository.IRepository
{
    public interface ICoverRepository : IRepository<Cover>
    {
        void Update(Cover obj);
    }
}
