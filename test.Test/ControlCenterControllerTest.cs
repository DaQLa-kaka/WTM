using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using test.Controllers;
using test.ViewModel.BasicData.ControlCenterVMs;
using test.Model;
using test.DataAccess;


namespace test.Test
{
    [TestClass]
    public class ControlCenterControllerTest
    {
        private ControlCenterController _controller;
        private string _seed;

        public ControlCenterControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<ControlCenterController>(new DataContext(_seed, DBTypeEnum.Memory), "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search((rv.Model as ControlCenterListVM).Searcher);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(ControlCenterVM));

            ControlCenterVM vm = rv.Model as ControlCenterVM;
            ControlCenter v = new ControlCenter();
			
            v.Name = "iyxHEkS";
            v.LocationId = AddCity();
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<ControlCenter>().Find(v.ID);
				
                Assert.AreEqual(data.Name, "iyxHEkS");
            }

        }

        [TestMethod]
        public void EditTest()
        {
            ControlCenter v = new ControlCenter();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.Name = "iyxHEkS";
                v.LocationId = AddCity();
                context.Set<ControlCenter>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(ControlCenterVM));

            ControlCenterVM vm = rv.Model as ControlCenterVM;
            vm.Wtm.DC = new DataContext(_seed, DBTypeEnum.Memory);
            v = new ControlCenter();
            v.ID = vm.Entity.ID;
       		
            v.Name = "pxxYUy01c";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.Name", "");
            vm.FC.Add("Entity.LocationId", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<ControlCenter>().Find(v.ID);
 				
                Assert.AreEqual(data.Name, "pxxYUy01c");
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            ControlCenter v = new ControlCenter();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.Name = "iyxHEkS";
                v.LocationId = AddCity();
                context.Set<ControlCenter>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(ControlCenterVM));

            ControlCenterVM vm = rv.Model as ControlCenterVM;
            v = new ControlCenter();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<ControlCenter>().Find(v.ID);
                Assert.AreEqual(data, null);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            ControlCenter v = new ControlCenter();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.Name = "iyxHEkS";
                v.LocationId = AddCity();
                context.Set<ControlCenter>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchEditTest()
        {
            ControlCenter v1 = new ControlCenter();
            ControlCenter v2 = new ControlCenter();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.Name = "iyxHEkS";
                v1.LocationId = AddCity();
                v2.Name = "pxxYUy01c";
                v2.LocationId = v1.LocationId; 
                context.Set<ControlCenter>().Add(v1);
                context.Set<ControlCenter>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(ControlCenterBatchVM));

            ControlCenterBatchVM vm = rv.Model as ControlCenterBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            
            vm.FC = new Dictionary<string, object>();
			
            _controller.DoBatchEdit(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data1 = context.Set<ControlCenter>().Find(v1.ID);
                var data2 = context.Set<ControlCenter>().Find(v2.ID);
 				
            }
        }


        [TestMethod]
        public void BatchDeleteTest()
        {
            ControlCenter v1 = new ControlCenter();
            ControlCenter v2 = new ControlCenter();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.Name = "iyxHEkS";
                v1.LocationId = AddCity();
                v2.Name = "pxxYUy01c";
                v2.LocationId = v1.LocationId; 
                context.Set<ControlCenter>().Add(v1);
                context.Set<ControlCenter>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(ControlCenterBatchVM));

            ControlCenterBatchVM vm = rv.Model as ControlCenterBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data1 = context.Set<ControlCenter>().Find(v1.ID);
                var data2 = context.Set<ControlCenter>().Find(v2.ID);
                Assert.AreEqual(data1, null);
            Assert.AreEqual(data2, null);
            }
        }

        private Guid AddCity()
        {
            City v = new City();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                try{

                v.CityName = "WvUxEwB4Oa5t6BEp";
                context.Set<City>().Add(v);
                context.SaveChanges();
                }
                catch{}
            }
            return v.ID;
        }


    }
}
