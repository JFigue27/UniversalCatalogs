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
    public class AdditionalFieldService : BaseService<AdditionalFieldLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllAdditionalFields request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetAdditionalFieldById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetAdditionalFieldWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedAdditionalFields request)
        {
            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateAdditionalFieldInstance request)
        {
            return WithDb(db => {
                var entity = request.ConvertTo<AdditionalField>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertAdditionalField request)
        {
            var entity = request.ConvertTo<AdditionalField>();
            return InTransaction(db => {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateAdditionalField request)
        {
            var entity = request.ConvertTo<AdditionalField>();
            return InTransaction(db => {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteAdditionalField request)
        {
            var entity = request.ConvertTo<AdditionalField>();
            return InTransaction(db => {
                Logic.Remove(entity);
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
    [Route("/AdditionalField", "GET")]
    public class GetAllAdditionalFields : GetAll<AdditionalField> { }

    [Route("/AdditionalField/{Id}", "GET")]
    public class GetAdditionalFieldById : GetSingleById<AdditionalField> { }

    [Route("/AdditionalField/GetSingleWhere", "GET")]
    [Route("/AdditionalField/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetAdditionalFieldWhere : GetSingleWhere<AdditionalField> { }

    [Route("/AdditionalField/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedAdditionalFields : GetPaged<AdditionalField> { }
    #endregion

    #region Generic Write
    [Route("/AdditionalField/CreateInstance", "POST")]
    public class CreateAdditionalFieldInstance : AdditionalField { }

    [Route("/AdditionalField", "POST")]
    public class InsertAdditionalField : AdditionalField { }

    [Route("/AdditionalField", "PUT")]
    public class UpdateAdditionalField : AdditionalField { }

    [Route("/AdditionalField", "DELETE")]
    [Route("/AdditionalField/{Id}", "DELETE")]
    public class DeleteAdditionalField : AdditionalField { }
    #endregion
}
