using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarWebAPI.Models
{
    public class ResultModel<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }

        public ResultModel(bool success, T data)
        {
            this.Success = success;
            this.Data = data;
        }
    }
}