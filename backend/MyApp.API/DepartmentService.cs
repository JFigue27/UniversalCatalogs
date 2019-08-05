using MyApp.Logic.Entities;
using MyApp.Logic;
using Reusable.Rest;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Threading.Tasks;
using ServiceStack.Text;
using Reusable.Rest.Implementations.SS;

namespace MyApp.API
{
    // [Authenticate]
    public class DepartmentService : BaseService<DepartmentLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllDepartments request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetDepartmentById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetDepartmentWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedDepartments request)
        {
            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateDepartmentInstance request)
        {
            return WithDb(db => {
                var entity = request.ConvertTo<Department>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertDepartment request)
        {
            var entity = request.ConvertTo<Department>();
            return InTransaction(db => {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateDepartment request)
        {
            var entity = request.ConvertTo<Department>();
            return InTransaction(db => {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteDepartment request)
        {
            var entity = request.ConvertTo<Department>();
            return InTransaction(db => {
                Logic.Remove(entity);
                return new CommonResponse();
            });
        }
        #endregion

        #region Endpoints - Specific
        
        #endregion
    }

    #region Specific
    
    #endregion

    #region Generic Read Only
    [Route("/Department", "GET")]
    public class GetAllDepartments : GetAll<Department> { }

    [Route("/Department/{Id}", "GET")]
    public class GetDepartmentById : GetSingleById<Department> { }

    [Route("/Department/GetSingleWhere", "GET")]
    [Route("/Department/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetDepartmentWhere : GetSingleWhere<Department> { }

    [Route("/Department/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedDepartments : GetPaged<Department> { }
    #endregion

    #region Generic Write
    [Route("/Department/CreateInstance", "POST")]
    public class CreateDepartmentInstance : Department { }

    [Route("/Department", "POST")]
    public class InsertDepartment : Department { }

    [Route("/Department", "PUT")]
    public class UpdateDepartment : Department { }

    [Route("/Department", "DELETE")]
    [Route("/Department/{Id}", "DELETE")]
    public class DeleteDepartment : Department { }
    #endregion
}
