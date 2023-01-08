using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chrono.Services.Repositories
{
    public class BaseRepository
    {
        protected IMapper _mapper { get; private set; }
        public BaseRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
