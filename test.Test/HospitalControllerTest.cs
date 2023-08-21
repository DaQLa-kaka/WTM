using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using test.Controllers;
using test.ViewModel.BasicData.HospitalVMs;
using test.Model;
using test.DataAccess;


namespace test.Test
{
    [TestClass]
    public class HospitalControllerTest
    {
        private HospitalController _controller;
        private string _seed;

        public HospitalControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<HospitalController>(new DataContext(_seed, DBTypeEnum.Memory), "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search((rv.Model as HospitalListVM).Searcher);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(HospitalVM));

            HospitalVM vm = rv.Model as HospitalVM;
            Hospital v = new Hospital();
			
            v.Level = test.Model.HospitalLevel.Class3;
            v.LocationId = AddCity();
            v.Name = "gmCfZKbkp27ecTDh";
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Hospital>().Find(v.ID);
				
                Assert.AreEqual(data.Level, test.Model.HospitalLevel.Class3);
                Assert.AreEqual(data.Name, "gmCfZKbkp27ecTDh");
            }

        }

        [TestMethod]
        public void EditTest()
        {
            Hospital v = new Hospital();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.Level = test.Model.HospitalLevel.Class3;
                v.LocationId = AddCity();
                v.Name = "gmCfZKbkp27ecTDh";
                context.Set<Hospital>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(HospitalVM));

            HospitalVM vm = rv.Model as HospitalVM;
            vm.Wtm.DC = new DataContext(_seed, DBTypeEnum.Memory);
            v = new Hospital();
            v.ID = vm.Entity.ID;
       		
            v.Level = test.Model.HospitalLevel.Class1;
            v.Name = "9XZF0DOUqH2k";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.Level", "");
            vm.FC.Add("Entity.LocationId", "");
            vm.FC.Add("Entity.Name", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Hospital>().Find(v.ID);
 				
                Assert.AreEqual(data.Level, test.Model.HospitalLevel.Class1);
                Assert.AreEqual(data.Name, "9XZF0DOUqH2k");
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            Hospital v = new Hospital();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.Level = test.Model.HospitalLevel.Class3;
                v.LocationId = AddCity();
                v.Name = "gmCfZKbkp27ecTDh";
                context.Set<Hospital>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(HospitalVM));

            HospitalVM vm = rv.Model as HospitalVM;
            v = new Hospital();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Hospital>().Find(v.ID);
                Assert.AreEqual(data, null);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            Hospital v = new Hospital();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.Level = test.Model.HospitalLevel.Class3;
                v.LocationId = AddCity();
                v.Name = "gmCfZKbkp27ecTDh";
                context.Set<Hospital>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchEditTest()
        {
            Hospital v1 = new Hospital();
            Hospital v2 = new Hospital();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.Level = test.Model.HospitalLevel.Class3;
                v1.LocationId = AddCity();
                v1.Name = "gmCfZKbkp27ecTDh";
                v2.Level = test.Model.HospitalLevel.Class1;
                v2.LocationId = v1.LocationId; 
                v2.Name = "9XZF0DOUqH2k";
                context.Set<Hospital>().Add(v1);
                context.Set<Hospital>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(HospitalBatchVM));

            HospitalBatchVM vm = rv.Model as HospitalBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            
            vm.LinkedVM.Level = test.Model.HospitalLevel.Class2;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("LinkedVM.Level", "");
            _controller.DoBatchEdit(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data1 = context.Set<Hospital>().Find(v1.ID);
                var data2 = context.Set<Hospital>().Find(v2.ID);
 				
                Assert.AreEqual(data1.Level, test.Model.HospitalLevel.Class2);
                Assert.AreEqual(data2.Level, test.Model.HospitalLevel.Class2);
            }
        }


        [TestMethod]
        public void BatchDeleteTest()
        {
            Hospital v1 = new Hospital();
            Hospital v2 = new Hospital();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.Level = test.Model.HospitalLevel.Class3;
                v1.LocationId = AddCity();
                v1.Name = "gmCfZKbkp27ecTDh";
                v2.Level = test.Model.HospitalLevel.Class1;
                v2.LocationId = v1.LocationId; 
                v2.Name = "9XZF0DOUqH2k";
                context.Set<Hospital>().Add(v1);
                context.Set<Hospital>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(HospitalBatchVM));

            HospitalBatchVM vm = rv.Model as HospitalBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data1 = context.Set<Hospital>().Find(v1.ID);
                var data2 = context.Set<Hospital>().Find(v2.ID);
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

                v.CityName = "rLpwwf9BmxLXMyo6";
                context.Set<City>().Add(v);
                context.SaveChanges();
                }
                catch{}
            }
            return v.ID;
        }


    }
}
