using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carwash_API.Models
{
    public class DBElements : IDBElements
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
    public interface IDBElements
    {
        string ConnectionString { get; set; }
        string Database { get; set; }
    }
}
