using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test.ViewModel.Patients.PatientVMs;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Mvc;

namespace test.Controllers
{
    [Area("Patients")]
    [ActionDescription("病例管理")]
    public partial class PatientController : BaseController
    {
        #region Search

        [ActionDescription("Sys.Search")]
        public ActionResult Index()
        {
            var vm = Wtm.CreateVM<PatientListVM>();
            vm.MakeSummary();
            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult Index(PatientListVM vm)
        {
            vm.MakeSummary();
            return PartialView(vm);
        }

        [ActionDescription("Sys.Search")]
        [HttpPost]
        public string Search(PatientSearcher searcher)
        {
            var vm = Wtm.CreateVM<PatientListVM>(passInit: true);
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

        #endregion Search

        #region Create

        [ActionDescription("Sys.Create")]
        public ActionResult Create()
        {
            var vm = Wtm.CreateVM<PatientVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.Create")]
        public ActionResult Create(PatientVM vm)
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
            var vm = Wtm.CreateVM<PatientVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Sys.Edit")]
        [HttpPost]
        [ValidateFormItemOnly]
        public ActionResult Edit(PatientVM vm)
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
            var vm = Wtm.CreateVM<PatientVM>(id);
            vm.DoDelete();
            if (!ModelState.IsValid)
            {
                return FFResult().Alert("删除失败！");
            }
            else
            {
                return FFResult().RefreshGrid().Alert("删除成功！");
            }
        }

        [ActionDescription("Sys.Delete")]
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection nouse)
        {
            var vm = Wtm.CreateVM<PatientVM>(id);
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
            var vm = Wtm.CreateVM<PatientVM>(id);
            return PartialView(vm);
        }

        #endregion Details

        #region BatchEdit

        [HttpPost]
        [ActionDescription("Sys.BatchEdit")]
        public ActionResult BatchEdit(string[] IDs)
        {
            var vm = Wtm.CreateVM<PatientBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.BatchEdit")]
        public ActionResult DoBatchEdit(PatientBatchVM vm, IFormCollection nouse)
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
            var vm = Wtm.CreateVM<PatientBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.BatchDelete")]
        public ActionResult DoBatchDelete(PatientBatchVM vm, IFormCollection nouse)
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
            var vm = Wtm.CreateVM<PatientImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.Import")]
        public ActionResult Import(PatientImportVM vm, IFormCollection nouse)
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
        public IActionResult ExportExcel(PatientListVM vm)
        {
            return vm.GetExportData();
        }

        #region Generate

        [ActionDescription("Sys.Generate")]
        public ActionResult Generate()
        {
            var vm = Wtm.CreateVM<PatientGeneratVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.Generate")]
        public ActionResult Generate(PatientGeneratVM vm)
        {
            vm.DoGen();
            return FFResult().CloseDialog().RefreshGrid().Alert("成功生成" + vm.GenerateCount.ToString() + "行数据");
        }

        #endregion Generate

        #region ChangeStatus

        [ActionDescription("更改状态")]
        public ActionResult ChangeStatus(int id)
        {
            var vm = Wtm.CreateVM<PatientVM>(id);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("更改状态")]
        public ActionResult ChangeStatus(PatientVM vm)
        {
            vm.DoEdit();
            return FFResult().CloseDialog().RefreshGrid();
        }

        #endregion ChangeStatus
    }
}