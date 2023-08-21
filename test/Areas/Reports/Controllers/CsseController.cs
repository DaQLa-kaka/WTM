using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.IO;
using test.Model;
using test.ViewModel.Reports.CsseVMs;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Core.Models;
using WalkingTec.Mvvm.Core.Support.FileHandlers;
using WalkingTec.Mvvm.Mvc;

namespace test.Controllers
{
    [Area("Reports")]
    [ActionDescription("世界疫情数据")]
    [FixConnection(CsName = "csse")]
    public partial class CsseController : BaseController
    {
        #region Search

        [ActionDescription("Sys.Search")]
        public ActionResult Index()
        {
            var vm = Wtm.CreateVM<CsseListVM>();
            return PartialView(vm);
        }

        [ActionDescription("Sys.Search")]
        [HttpPost]
        public string Search(CsseSearcher searcher)
        {
            var vm = Wtm.CreateVM<CsseListVM>(passInit: true);
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
            var vm = Wtm.CreateVM<CsseVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.Create")]
        public ActionResult Create(CsseVM vm)
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
            var vm = Wtm.CreateVM<CsseVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Sys.Edit")]
        [HttpPost]
        [ValidateFormItemOnly]
        public ActionResult Edit(CsseVM vm)
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
            var vm = Wtm.CreateVM<CsseVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Sys.Delete")]
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection nouse)
        {
            var vm = Wtm.CreateVM<CsseVM>(id);
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
            var vm = Wtm.CreateVM<CsseVM>(id);
            return PartialView(vm);
        }

        #endregion Details

        #region BatchEdit

        [HttpPost]
        [ActionDescription("Sys.BatchEdit")]
        public ActionResult BatchEdit(string[] IDs)
        {
            var vm = Wtm.CreateVM<CsseBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.BatchEdit")]
        public ActionResult DoBatchEdit(CsseBatchVM vm, IFormCollection nouse)
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
            var vm = Wtm.CreateVM<CsseBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.BatchDelete")]
        public ActionResult DoBatchDelete(CsseBatchVM vm, IFormCollection nouse)
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
            var vm = Wtm.CreateVM<CsseImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Sys.Import")]
        public ActionResult Import(CsseImportVM vm, IFormCollection nouse)
        {
            //byte[] fdata = null;
            //using (var newdc = ConfigInfo.Connections[0].CreateDC())
            //{
            //    fdata = FileHelper.GetFileByteForDownLoadById(vm.UploadFileId.Value, newdc, ConfigInfo);
            //}
            IWtmFile file = null;
            if (Wtm.ServiceProvider != null)
            {
                using (var newdc = ConfigInfo.Connections[0].CreateDC())
                {
                    var fp = Wtm.ServiceProvider.GetRequiredService<WtmFileProvider>();
                    file = fp.GetFile(vm.UploadFileId, true, newdc);
                }
            }
            //IWtmFileHandler wtmFileHandler = CreateFileHandler(wtmFile.SaveMode, vm.DC);
            //wtmFile.DataStream = wtmFileHandler.GetFileData(wtmFile);

            //MemoryStream ms = new MemoryStream(file.DataStream);
            StreamReader sr = new StreamReader(file.DataStream);
            string title = sr.ReadLine();
            string[] cols = title.Split(",");
            int count = 0;
            try
            {
                using (var newdc = ConfigInfo.Connections[1].CreateDC())
                {
                    while (true)
                    {
                        string line = sr.ReadLine();
                        count++;
                        if (string.IsNullOrEmpty(line))
                        {
                            break;
                        }
                        string[] data = line.Split(",");

                        for (int i = 4; i < cols.Length; i++)
                        {
                            var col = cols[i];
                            Csse csse = new Csse();
                            csse.Province = data[0];
                            csse.Country = data[1];
                            csse.Lat = double.Parse(data[2]);
                            csse.Long = double.Parse(data[3]);
                            csse.Date = DateTime.Parse(col, new CultureInfo("en_US"));
                            csse.ConfirmedCase = int.Parse(data[i]);

                            newdc.Set<Csse>().Add(csse);
                        }
                    }
                    newdc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                vm.ErrorListVM.EntityList.Add(new ErrorMessage { ExcelIndex = count, Message = ex.ToString(), Index = count });
                return PartialView(vm);
            }

            return FFResult().CloseDialog().RefreshGrid().Alert("成功导入 " + count.ToString() + " 行数据");
        }

        #endregion Import

        [ActionDescription("Sys.Export")]
        [HttpPost]
        public IActionResult ExportExcel(CsseListVM vm)
        {
            return vm.GetExportData();
        }
    }
}