using Dopamine.Core.Interfaces.ProjectInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dopamine.Core.Services.ProjectServices
{
    public class DualSenseKeyMappingService : IKeyMapping
    {
        public string LeftDpadLeft { get; set; } = "PovX";
        public string LeftDpadRight { get; set; } = "PovX";
        public string LeftDpadUp { get; set; } = "PovY";
        public string LeftDpadDown { get; set; } = "PovY";

        public int RightDpadLeft { get; set; } = 0;
        public int RightDpadRight { get; set; } = 2;
        public int RightDpadUp { get; set; } = 3;
        public int RightDpadDown { get; set; } = 1;

        public int R1 { get; set; } = 5;
        public int R2 { get; set; } = 7;
        public int L1 { get; set; } = 4;
        public int L2 { get; set; } = 6;

        public int Start { get; set; } = 8;
        public int Select { get; set; } = 9;

        public int LiftStickPress { get; set; } = 10;
        public int RightStickPress { get; set; } = 11;
    }
}
