using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Attributes;

namespace test.Model
{
    [MiddleTable]
    public class PatientVirus:TopBasePoco
    {
        public Patient  Patient {set;get;}
        public int PatientId { set;get;}
        public Virus Virus { set;get;}
        public Guid VirusId { set;get;}
    }
}
