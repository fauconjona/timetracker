using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraTracker.Interaces
{
    public interface IJiraService
    {
        void Initialize(string url, string login, string token, string project);
        Task<bool> AddWorklog(string key, DateTime start, DateTime end);
    }
}
