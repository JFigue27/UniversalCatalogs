using MyApp.Logic.Entities;
using MyApp.Logic;
using Reusable.Rest;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Threading.Tasks;
using ServiceStack.Text;
using Reusable.Rest.Implementations.SS;

///start:slot:imports<<<///end:slot:imports<<<

namespace MyApp.API
{
    // [Authenticate]
    public class SortDataService : BaseService<SortDataLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllSortDatas request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetSortDataById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetSortDataWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedSortDatas request)
        {
            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateSortDataInstance request)
        {
            return WithDb(db =>
            {
                var entity = request.ConvertTo<SortData>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertSortData request)
        {
            var entity = request.ConvertTo<SortData>();
            return InTransaction(db =>
            {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateSortData request)
        {
            var entity = request.ConvertTo<SortData>();
            return InTransaction(db =>
            {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteSortData request)
        {
            var entity = request.ConvertTo<SortData>();
            return InTransaction(db =>
            {
                Logic.Remove(entity);
                return new CommonResponse();
            });
        }
        public object Delete(DeleteByIdSortData request)
        {
            var entity = request.ConvertTo<SortData>();
            return InTransaction(db =>
            {
                Logic.RemoveById(entity.Id);
                return new CommonResponse();
            });
        }
        #endregion

        #region Endpoints - Specific
        
        ///start:slot:endpoints<<<///end:slot:endpoints<<<
        #endregion
    }

    #region Specific
    
    ///start:slot:endpointsRoutes<<<///end:slot:endpointsRoutes<<<
    #endregion

    #region Generic Read Only
    [Route("/SortData", "GET")]
    public class GetAllSortDatas : GetAll<SortData> { }

    [Route("/SortData/{Id}", "GET")]
    public class GetSortDataById : GetSingleById<SortData> { }

    [Route("/SortData/GetSingleWhere", "GET")]
    [Route("/SortData/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetSortDataWhere : GetSingleWhere<SortData> { }

    [Route("/SortData/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedSortDatas : GetPaged<SortData> { }
    #endregion

    #region Generic Write
    [Route("/SortData/CreateInstance", "POST")]
    public class CreateSortDataInstance : SortData { }

    [Route("/SortData", "POST")]
    public class InsertSortData : SortData { }

    [Route("/SortData", "PUT")]
    public class UpdateSortData : SortData { }

    [Route("/SortData", "DELETE")]
    public class DeleteSortData : SortData { }

    [Route("/SortData/{Id}", "DELETE")]
    public class DeleteByIdSortData : SortData { }
    #endregion
}
