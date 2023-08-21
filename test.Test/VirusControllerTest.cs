using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using test.Controllers;
using test.ViewModel.BasicData.VirusVMs;
using test.Model;
using test.DataAccess;


namespace test.Test
{
    [TestClass]
    public class VirusControllerTest
    {
        private VirusController _controller;
        private string _seed;

        public VirusControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<VirusController>(new DataContext(_seed, DBTypeEnum.Memory), "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search((rv.Model as VirusListVM).Searcher);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(VirusVM));

            VirusVM vm = rv.Model as VirusVM;
            Virus v = new Virus();
			
            v.VirusCode = "3FfYQC";
            v.VirusName = "gjGVZopkGbb";
            v.VirusType = test.Model.VirusTypeEnum.RNA;
            v.Remark = "gUp45Wi5";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Virus>().Find(v.ID);
				
                Assert.AreEqual(data.VirusCode, "3FfYQC");
                Assert.AreEqual(data.VirusName, "gjGVZopkGbb");
                Assert.AreEqual(data.VirusType, test.Model.VirusTypeEnum.RNA);
                Assert.AreEqual(data.Remark, "gUp45Wi5");
            }

        }

        [TestMethod]
        public void EditTest()
        {
            Virus v = new Virus();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.VirusCode = "3FfYQC";
                v.VirusName = "gjGVZopkGbb";
                v.VirusType = test.Model.VirusTypeEnum.RNA;
                v.Remark = "gUp45Wi5";
                context.Set<Virus>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VirusVM));

            VirusVM vm = rv.Model as VirusVM;
            vm.Wtm.DC = new DataContext(_seed, DBTypeEnum.Memory);
            v = new Virus();
            v.ID = vm.Entity.ID;
       		
            v.VirusCode = "TYi";
            v.VirusName = "ZcV86UpIt7C";
            v.VirusType = test.Model.VirusTypeEnum.DNA;
            v.Remark = "eA34Jzt3botlpnt";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.VirusCode", "");
            vm.FC.Add("Entity.VirusName", "");
            vm.FC.Add("Entity.VirusType", "");
            vm.FC.Add("Entity.Remark", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Virus>().Find(v.ID);
 				
                Assert.AreEqual(data.VirusCode, "TYi");
                Assert.AreEqual(data.VirusName, "ZcV86UpIt7C");
                Assert.AreEqual(data.VirusType, test.Model.VirusTypeEnum.DNA);
                Assert.AreEqual(data.Remark, "eA34Jzt3botlpnt");
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            Virus v = new Virus();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.VirusCode = "3FfYQC";
                v.VirusName = "gjGVZopkGbb";
                v.VirusType = test.Model.VirusTypeEnum.RNA;
                v.Remark = "gUp45Wi5";
                context.Set<Virus>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(VirusVM));

            VirusVM vm = rv.Model as VirusVM;
            v = new Virus();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Virus>().Find(v.ID);
                Assert.AreEqual(data, null);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            Virus v = new Virus();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.VirusCode = "3FfYQC";
                v.VirusName = "gjGVZopkGbb";
                v.VirusType = test.Model.VirusTypeEnum.RNA;
                v.Remark = "gUp45Wi5";
                context.Set<Virus>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchEditTest()
        {
            Virus v1 = new Virus();
            Virus v2 = new Virus();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.VirusCode = "3FfYQC";
                v1.VirusName = "gjGVZopkGbb";
                v1.VirusType = test.Model.VirusTypeEnum.RNA;
                v1.Remark = "gUp45Wi5";
                v2.VirusCode = "TYi";
                v2.VirusName = "ZcV86UpIt7C";
                v2.VirusType = test.Model.VirusTypeEnum.DNA;
                v2.Remark = "eA34Jzt3botlpnt";
                context.Set<Virus>().Add(v1);
                context.Set<Virus>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VirusBatchVM));

            VirusBatchVM vm = rv.Model as VirusBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            
            vm.LinkedVM.VirusType = test.Model.VirusTypeEnum.RNA;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("LinkedVM.VirusType", "");
            _controller.DoBatchEdit(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data1 = context.Set<Virus>().Find(v1.ID);
                var data2 = context.Set<Virus>().Find(v2.ID);
 				
                Assert.AreEqual(data1.VirusType, test.Model.VirusTypeEnum.RNA);
                Assert.AreEqual(data2.VirusType, test.Model.VirusTypeEnum.RNA);
            }
        }


        [TestMethod]
        public void BatchDeleteTest()
        {
            Virus v1 = new Virus();
            Virus v2 = new Virus();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.VirusCode = "3FfYQC";
                v1.VirusName = "gjGVZopkGbb";
                v1.VirusType = test.Model.VirusTypeEnum.RNA;
                v1.Remark = "gUp45Wi5";
                v2.VirusCode = "TYi";
                v2.VirusName = "ZcV86UpIt7C";
                v2.VirusType = test.Model.VirusTypeEnum.DNA;
                v2.Remark = "eA34Jzt3botlpnt";
                context.Set<Virus>().Add(v1);
                context.Set<Virus>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(VirusBatchVM));

            VirusBatchVM vm = rv.Model as VirusBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data1 = context.Set<Virus>().Find(v1.ID);
                var data2 = context.Set<Virus>().Find(v2.ID);
                Assert.AreEqual(data1, null);
            Assert.AreEqual(data2, null);
            }
        }


    }
}
