using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test.ViewModel.Patients.ReportVMs;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Mvc;

namespace test.Controllers
{
    [Area("Patients")]
    [ActionDescription("病例追踪")]
    public partial class ReportController : BaseController
    {
        #region Search

        [ActionDescription("Sys.Search")]
        public ActionResult Index()
        {
            var vm = Wtm.CreateVM<ReportListVM>();
            return PartialView(vm);
        }

        [ActionDescription("Sys.Search")]
        [HttpPost]
        public string Search(ReportSearcher searcher)
        {
            var vm = Wtm.CreateVM<ReportListVM>(passInit: true);
            if (ModelState.IsValid)
            {
                vm.Searcher = searcher;
                return vm.GetJson(false);
            }
            else
            {
                return vm.GetError();
            }
        }

        [ActionDescription("搜索")]
        [HttpPost]
        public string Search2(ReportListVM2 vm)
        {
            return vm.GetJson(false);
        }

        #endregion Search

        #region Create

        [ActionDescription("Sys.Create")]
        public ActionResult Create()
        {
            var vm = Wtm.CreateVM<ReportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.Create")]
        public ActionResult Create(ReportVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoAdd();
                if (!ModelState.IsValid)
                {
                    vm.DoReInit();
                    return PartialView(vm);
                }
                else
                {
                    return FFResult().CloseDialog().RefreshGrid();
                }
            }
        }

        #endregion Create

        #region Edit

        [ActionDescription("Sys.Edit")]
        public ActionResult Edit(string id)
        {
            var vm = Wtm.CreateVM<ReportVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Sys.Edit")]
        [HttpPost]
        [ValidateFormItemOnly]
        public ActionResult Edit(ReportVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoEdit();
                if (!ModelState.IsValid)
                {
                    vm.DoReInit();
                    return PartialView(vm);
                }
                else
                {
                    return FFResult().CloseDialog().RefreshGridRow(vm.Entity.ID);
                }
            }
        }

        #endregion Edit

        #region Delete

        [ActionDescription("Sys.Delete")]
        public ActionResult Delete(string id)
        {
            var vm = Wtm.CreateVM<ReportVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Sys.Delete")]
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection nouse)
        {
            var vm = Wtm.CreateVM<ReportVM>(id);
            vm.DoDelete();
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid();
            }
        }

        #endregion Delete

        #region Details

        [ActionDescription("Sys.Details")]
        public ActionResult Details(string id)
        {
            var vm = Wtm.CreateVM<ReportVM>(id);
            return PartialView(vm);
        }

        #endregion Details

        #region BatchEdit

        [HttpPost]
        [ActionDescription("Sys.BatchEdit")]
        public ActionResult BatchEdit(string[] IDs)
        {
            var vm = Wtm.CreateVM<ReportBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.BatchEdit")]
        public ActionResult DoBatchEdit(ReportBatchVM vm, IFormCollection nouse)
        {
            if (!ModelState.IsValid || !vm.DoBatchEdit())
            {
                return PartialView("BatchEdit", vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Localizer["Sys.BatchEditSuccess", vm.Ids.Length]);
            }
        }

        #endregion BatchEdit

        #region BatchDelete

        [HttpPost]
        [ActionDescription("Sys.BatchDelete")]
        public ActionResult BatchDelete(string[] IDs)
        {
            var vm = Wtm.CreateVM<ReportBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.BatchDelete")]
        public ActionResult DoBatchDelete(ReportBatchVM vm, IFormCollection nouse)
        {
            if (!ModelState.IsValid || !vm.DoBatchDelete())
            {
                return PartialView("BatchDelete", vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Localizer["Sys.BatchDeleteSuccess", vm.Ids.Length]);
            }
        }

        #endregion BatchDelete

        #region Import

        [ActionDescription("Sys.Import")]
        public ActionResult Import()
        {
            var vm = Wtm.CreateVM<ReportImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.Import")]
        public ActionResult Import(ReportImportVM vm, IFormCollection nouse)
        {
            if (vm.ErrorListVM.EntityList.Count > 0 || !vm.BatchSaveData())
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Localizer["Sys.ImportSuccess", vm.EntityList.Count.ToString()]);
            }
        }

        #endregion Import

        [ActionDescription("Sys.Export")]
        [HttpPost]
        public IActionResult ExportExcel(ReportListVM vm)
        {
            return vm.GetExportData();
        }

        [ActionDescription("新建")]
        public ActionResult Create2(int id, string patientName)
        {
            var vm = Wtm.CreateVM<ReportVM>(passInit: true);
            vm.Entity.PatientId = id;
            vm.Entity.Patient = new Model.Patient();
            vm.Entity.Patient.PatientName = patientName;
            return PartialView(vm);
        }

        [ActionDescription("新建")]
        public ActionResult Create2(ReportVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoAdd();
                if (!ModelState.IsValid)
                {
                    vm.DoReInit();
                    return PartialView(vm);
                }
                else
                {
                    return FFResult().CloseDialog().RefreshGrid(index: 1);
                }
            }
        }
    }
}