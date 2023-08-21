using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using test.Model;


namespace test.ViewModel.Api.PatientVMs
{
    public partial class PatientApiListVM : BasePagedListVM<PatientApi_View, PatientApiSearcher>
    {

        protected override IEnumerable<IGridColumn<PatientApi_View>> InitGridHeader()
        {
            return new List<GridColumn<PatientApi_View>>{
                this.MakeGridHeader(x => x.PatientName),
                this.MakeGridHeader(x => x.IdNumber),
                this.MakeGridHeader(x => x.Gender),
                this.MakeGridHeader(x => x.Status),
                this.MakeGridHeader(x => x.Birthday),
                this.MakeGridHeader(x => x.CityName_view),
                this.MakeGridHeader(x => x.Name_view),
                this.MakeGridHeader(x => x.PhotoId).SetFormat(PhotoIdFormat),
                this.MakeGridHeader(x => x.VirusName_view),
                this.MakeGridHeaderAction(width: 200)
            };
        }
        private List<ColumnFormatInfo> PhotoIdFormat(PatientApi_View entity, object val)
        {
            return new List<ColumnFormatInfo>
            {
                ColumnFormatInfo.MakeDownloadButton(ButtonTypesEnum.Button,entity.PhotoId),
                ColumnFormatInfo.MakeViewButton(ButtonTypesEnum.Button,entity.PhotoId,640,480),
            };
        }


        public override IOrderedQueryable<PatientApi_View> GetSearchQuery()
        {
            var query = DC.Set<Patient>()
                .CheckContain(Searcher.PatientName, x=>x.PatientName)
                .CheckEqual(Searcher.Status, x=>x.Status)
                .CheckEqual(Searcher.HospitalID, x=>x.HospitalID)
                .Select(x => new PatientApi_View
                {
				    ID = x.ID,
                    PatientName = x.PatientName,
                    IdNumber = x.IdNumber,
                    Gender = x.Gender,
                    Status = x.Status,
                    Birthday = x.Birthday,
                    CityName_view = x.Location.CityName,
                    Name_view = x.Hospital.Name,
                    PhotoId = x.PhotoId,
                    VirusName_view = x.Viruses.Select(y=>y.Virus.VirusName).ToSepratedString(null,","), 
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class PatientApi_View : Patient{
        [Display(Name = "城市名称")]
        public String CityName_view { get; set; }
        [Display(Name = "医院名称")]
        public String Name_view { get; set; }
        [Display(Name = "病毒名称")]
        public String VirusName_view { get; set; }

    }
}
