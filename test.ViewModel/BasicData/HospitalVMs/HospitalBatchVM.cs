using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using test.Model;


namespace test.ViewModel.BasicData.HospitalVMs
{
    public partial class HospitalBatchVM : BaseBatchVM<Hospital, Hospital_BatchEdit>
    {
        public HospitalBatchVM()
        {
            ListVM = new HospitalListVM();
            LinkedVM = new Hospital_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class Hospital_BatchEdit : BaseVM
    {
        [Display(Name = "医院级别")]
        public HospitalLevel? Level { get; set; }

        protected override void InitVM()
        {
        }

    }

}
