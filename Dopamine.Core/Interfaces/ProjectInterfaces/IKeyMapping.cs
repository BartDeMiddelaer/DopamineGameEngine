using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dopamine.Core.Interfaces.ProjectInterfaces
{
    public interface IKeyMapping
    {
        public string LeftDpadLeft { get; set; }
        public string LeftDpadRight { get; set; }
        public string LeftDpadUp { get; set; }
        public string LeftDpadDown { get; set; }

        public int RightDpadLeft { get; set; }
        public int RightDpadRight { get; set; }
        public int RightDpadUp { get; set; }
        public int RightDpadDown { get; set; }

        public int R1 { get; set; }
        public int R2 { get; set; }
        public int L1 { get; set; }
        public int L2 { get; set; }

        public int Start { get; set; }
        public int Select { get; set; }

        public int LiftStickPress { get; set; }
        public int RightStickPress { get; set; }
    }
}
