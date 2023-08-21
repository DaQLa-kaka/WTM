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
    public partial class PatientListVM2 : BasePagedListVM<Patient_View, PatientSearcher>
    {
        public List<TreeSelectListItem> AllCity { get; set; }
        public ReportListVM2 Reports { get; set; } = new ReportListVM2();

        public PatientListVM2()
        {
            //NeedPage = false;
        }

        protected override void InitVM()
        {
            AllCity = DC.Set<City>().GetTreeSelectListItems(Wtm, x => x.CityName);
            Reports.CopyContext(this);
        }

        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                 this.MakeAction("Patient2","Reports","跟踪","", GridActionParameterTypesEnum.SingleId,null,500).SetOnClickScript("report2").SetShowInRow().SetHideOnToolBar().SetButtonClass("layui-btn-warm"),

                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.Create, "新建","Patients", dialogWidth: 800).SetMax(),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.Edit, "修改","Patients", dialogWidth: 800),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.Delete, "删除", "Patients",dialogWidth: 800).SetPromptMessage("你真的要删除么？").SetShowDialog(false),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.Details, "详细","Patients", dialogWidth: 800),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.BatchEdit, "批量修改","Patients", dialogWidth: 800),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.BatchDelete, "批量删除","Patients", dialogWidth: 800),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.Import, "导入","Patients", dialogWidth: 800),
                this.MakeStandardAction("Patient2", GridActionStandardTypesEnum.ExportExcel, "导出","Patients"),
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
                this.MakeGridHeader(x => x.PatientName).SetSort(),
                this.MakeGridHeader(x => x.IdNumber),
                this.MakeGridHeader(x => x.Gender),
                this.MakeGridHeader(x => x.Status).SetBackGroundFunc((a)=>{
                    if(a.Status == PatientStatusEnum.ZhiYu)
                    {
                        return "#00FF00";
                    }
                    return "";
                }),
                this.MakeGridHeader(x => x.Birthday).SetSort().SetFormat((a,b)=>a.Birthday?.ToShortDateString()),
                this.MakeGridHeader(x => x.CityName_view),
                this.MakeGridHeader(x => x.Name_view),
                this.MakeGridHeader(x => x.PhotoId).SetFormat(PhotoIdFormat),
                this.MakeGridHeader(x => x.VirusName_view),

                this.MakeGridHeader(x=> "report").SetFormat((a,b)=>{
                    string rv = UIService.MakeScriptButton(ButtonTypesEnum.Button,"跟踪",$"report('{a.ID}')",buttonClass:"layui-btn-danger layui-btn-sm");
                    return rv;
                }),

                this.MakeGridHeader(x=> "aaa").SetHide().SetFormat((a,b)=>{
                    if(a.Status != PatientStatusEnum.SiWang)
                    {
                        return "true";
                    }
                    return "false";
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
                .CheckWhere(Searcher.LocationId, x => x.LocationId == Searcher.LocationId || x.Location.ParentId == Searcher.LocationId || x.Location.Parent.ParentId == Searcher.LocationId)
                .CheckWhere(Searcher.SelectedVirusesIDs, x => DC.Set<PatientVirus>().Where(y => Searcher.SelectedVirusesIDs.Contains(y.VirusId)).Select(z => z.PatientId).Contains(x.ID))
                //.DPWhere(LoginUserInfo.DataPrivileges, x => x.LocationId, x => x.Viruses[0].VirusId)
                //.DPWhere(LoginUserInfo.DataPrivileges, x => x.Viruses[0].VirusId)
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
    }
}