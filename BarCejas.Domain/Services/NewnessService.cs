using BarCejas.Core.CustomEntities;
using BarCejas.Core.Interfaces;

using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Text;

namespace BarCejas.Core.Services
{
   public class NewnessService: INewnessService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public NewnessService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }
    }
}
