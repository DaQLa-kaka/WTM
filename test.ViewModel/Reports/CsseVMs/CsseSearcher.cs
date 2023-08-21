using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using test.Model;


namespace test.ViewModel.Reports.CsseVMs
{
    public partial class CsseSearcher : BaseSearcher
    {
        [Display(Name = "国家")]
        public String Country { get; set; }
        [Display(Name = "省")]
        public String Province { get; set; }

        protected override void InitVM()
        {
        }

    }
}
