using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarWebAPI.Models
{
    public interface ICachable
    {
        DateTime GetDateTime();
    }
}
