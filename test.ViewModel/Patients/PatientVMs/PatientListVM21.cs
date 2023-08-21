using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using test.Model;
using test.ViewModel.Patients.ReportVMs;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;

namespace test.ViewModel.Patients.PatientVMs
{
    public partial class PatientListVM21 : BasePagedListVM<Patient_View, PatientSearcher>
    {
        public Patient_Summary Summary { get; set; }

        public List<TreeSelectListItem> AllCity { get; set; }

        public ReportListVM2 Reports { get; set; } = new ReportListVM2();

        protected override void InitVM()
        {
            AllCity = DC.Set<City>().GetTreeSelectListItems(Wtm, x => x.CityName);
            Reports.CopyContext(this);
        }

        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeAction("Patient2","Reports","跟踪","",GridActionParameterTypesEnum.SingleId,null,500).SetOnClickScript("report2").SetHideOnToolBar().SetShowInRow(),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.Create, Localizer["Sys.Create"],"Patients", dialogWidth: 800).SetMax(),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.Edit, Localizer["Sys.Edit"], "Patients", dialogWidth: 800),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.Delete, Localizer["Sys.Delete"], "Patients", dialogWidth: 800).SetPromptMessage("是否删除数据？").SetShowDialog(false),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.Details, Localizer["Sys.Details"], "Patients", dialogWidth: 800),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.BatchEdit, Localizer["Sys.BatchEdit"], "Patients", dialogWidth: 800),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.BatchDelete, Localizer["Sys.BatchDelete"], "Patients", dialogWidth: 800),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.Import, Localizer["Sys.Import"], "Patients", dialogWidth: 800),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.ExportExcel, Localizer["Sys.Export"], "Patients"),
            };
        }

        public override string SetFullRowBgColor(object entity)
        {
            Patient_View p = (Patient_View)entity;
            if (p.Status == PatientStatusEnum.QueZhen)
            {
                return "#FFFF00";
            }
            return "";
        }

        protected override IEnumerable<IGridColumn<Patient_View>> InitGridHeader()
        {
            return new List<GridColumn<Patient_View>>{
                this.MakeGridHeader(x => x.PatientName).SetSort().SetFormat((a, b) =>
                {
                    //string rv = $"<a href='/Home/PIndex#/Patients/Patient/Details/{a.GetID()}' target='_blank'>{a.PatientName}</a>";

                    string rv = UIService.MakeDialogButton(ButtonTypesEnum.Link,$"/Patients/Patient/Details/{a.GetID()}",a.PatientName,500,null,"详细");
                    return rv;
                }),
                this.MakeGridHeader(x => x.IdNumber).SetSort(),
                this.MakeGridHeader(x => x.Gender),
                this.MakeGridHeader(x => x.Status).SetBackGroundFunc((a)=>{
                    if(a.Status==PatientStatusEnum.ZhiYu)
                    {
                        return "#00FF00";
                    }
                    return "";
                }),
                this.MakeGridHeader(x => x.Birthday).SetSort().SetFormat((a,b)=>a.Birthday.Value.ToShortDateString()),
                this.MakeGridHeader(x => x.CityName_view),
                this.MakeGridHeader(x => x.Name_view),
                this.MakeGridHeader(x => x.PhotoId).SetFormat(PhotoIdFormat),
                this.MakeGridHeader(x => x.VirusName_view),
                this.MakeGridHeader(x => "report").SetFormat((a, b) =>
                {
                    string rv=UIService.MakeScriptButton(ButtonTypesEnum.Link,"跟踪",$"report('{a.ID}')");
                    return rv;
                }),
                this.MakeGridHeader(x => "aaa").SetHide().SetFormat((a, b)=>
                {
                    if (a.Status!=PatientStatusEnum.SiWang)
                    {
                        return true;
                    }
                    return false;
                }),
                this.MakeGridHeaderAction(width: 400)
            };
        }

        private List<ColumnFormatInfo> PhotoIdFormat(Patient_View entity, object val)
        {
            return new List<ColumnFormatInfo>
            {
                ColumnFormatInfo.MakeDownloadButton(ButtonTypesEnum.Button,entity.PhotoId),
                ColumnFormatInfo.MakeViewButton(ButtonTypesEnum.Button,entity.PhotoId,640,480),
            };
        }

        public override IOrderedQueryable<Patient_View> GetSearchQuery()
        {
            var query = DC.Set<Patient>()
                .CheckContain(Searcher.PatientName, x => x.PatientName)
                .CheckContain(Searcher.IdNumber, x => x.IdNumber)
                .CheckEqual(Searcher.Gender, x => x.Gender)
                .CheckEqual(Searcher.Status, x => x.Status)
                .CheckEqual(Searcher.HospitalID, x => x.HospitalID)
                .CheckWhere(Searcher.LocationId, x => x.LocationId == Searcher.LocationId || x.Location.ParentId == Searcher.LocationId || x.Location.Parent.ParentId == Searcher.LocationId)
                .CheckWhere(Searcher.SelectedVirusesIDs, x => DC.Set<PatientVirus>().Where(y => Searcher.SelectedVirusesIDs.Contains(y.VirusId)).Select(z => z.PatientId).Contains(x.ID))
                .Select(x => new Patient_View
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
                    VirusName_view = x.Viruses.Select(y => y.Virus.VirusName).ToSepratedString(null, ","),
                })
                .OrderBy(x => x.ID);
            return query;
        }

        public void MakeSummary()
        {
            var data = DC.Set<Patient>()
                .CheckContain(Searcher.PatientName, x => x.PatientName)
                .CheckContain(Searcher.IdNumber, x => x.IdNumber)
                .CheckEqual(Searcher.Gender, x => x.Gender)
                .CheckEqual(Searcher.Status, x => x.Status)
                .CheckEqual(Searcher.HospitalID, x => x.HospitalID)
                .CheckWhere(Searcher.SelectedVirusesIDs, x => DC.Set<PatientVirus>().Where(y => Searcher.SelectedVirusesIDs.Contains(y.VirusId)).Select(z => z.PatientId).Contains(x.ID))
                .GroupBy(x => x.Status).Select(x => new { a = x.Key, b = x.Count() }).ToDictionary(x => x.a, x => x.b);
            this.Summary = new Patient_Summary
            {
                QueZhen = data.ContainsKey(PatientStatusEnum.QueZhen) ? data[PatientStatusEnum.QueZhen] : 0,
                SiWang = data.ContainsKey(PatientStatusEnum.SiWang) ? data[PatientStatusEnum.SiWang] : 0,
                ZhiYu = data.ContainsKey(PatientStatusEnum.ZhiYu) ? data[PatientStatusEnum.ZhiYu] : 0
            };
        }
    }
}