using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Distimo.Services.Models
{
    public class RevenueByWeek
    {
        public string Week { get; set; }
        public int Amazon { get; set; }
        public int Apple { get; set; }
        public int Google { get; set; }
    }
}
