using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Services.Session
{
    public class SessionService : ISessionService
    {
        private readonly IRepositoryService repository;

        public SessionService(IRepositoryService repository)
        {
            this.repository = repository;
        }
    }
}
