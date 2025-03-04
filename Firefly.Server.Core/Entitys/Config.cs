using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firefly.Server.Core.Entitys
{
    public class Config
    {
        public bool isIRCEnabled {  get; set; }
        public int IRCServerPort { get; set; }
        public string serverDomain { get; set; }

        /*
         * TODO - Proposed config structure in DB.
         * 
         * 1) Table: id, key, value. value to be JSONB data-type
         * 2) Config settings to be hierachical.
         * 3) Config.cs Entity to match top-level table. Lower down to have object hierarchy for the config. JSON structure. Top-level stored within Config.cs Anything unknown goes into a Dictionary Key->Value.
         * 4) Example: (Something like this:)
         * 5) IRC -> Enabled, Port (This would have an IRCConfig class that contained a bool Enabled and int Port). Upper level Config table would have IRC (Key) -> (Value) "Enabled": True, "Port": 6667
         * 6) General -> domain
         * 7) Authentication -> allowAnonUsers
         * 
         * Concern: Is there a better way to store this than some complex class structure? I foresee this growing large, perhaps small classes stored within a single file is a good way to store this.
         * would record, struct or record struct be more appropiate than a class?
         */
    }
}
