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
    public class MaterialService : BaseService<MaterialLogic>
    {
        #region Endpoints - Generic Read Only
        public object Get(GetAllMaterials request)
        {
            return WithDb(db => Logic.GetAll());
        }

        public object Get(GetMaterialById request)
        {
            return WithDb(db => Logic.GetById(request.Id));
        }

        public object Get(GetMaterialWhere request)
        {
            return WithDb(db => Logic.GetSingleWhere(request.Property, request.Value));
        }

        public object Get(GetPagedMaterials request)
        {
            return WithDb(db => Logic.GetPaged(
                request.Limit,
                request.Page,
                request.FilterGeneral));
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateMaterialInstance request)
        {
            return WithDb(db => {
                var entity = request.ConvertTo<Material>();
                return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
                {
                    ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
                };
            });
        }

        public object Post(InsertMaterial request)
        {
            var entity = request.ConvertTo<Material>();
            return InTransaction(db => {
                Logic.Add(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }

        public object Put(UpdateMaterial request)
        {
            var entity = request.ConvertTo<Material>();
            return InTransaction(db => {
                Logic.Update(entity);
                return new CommonResponse(Logic.GetById(entity.Id));
            });
        }
        public object Delete(DeleteMaterial request)
        {
            var entity = request.ConvertTo<Material>();
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
    [Route("/Material", "GET")]
    public class GetAllMaterials : GetAll<Material> { }

    [Route("/Material/{Id}", "GET")]
    public class GetMaterialById : GetSingleById<Material> { }

    [Route("/Material/GetSingleWhere", "GET")]
    [Route("/Material/GetSingleWhere/{Property}/{Value}", "GET")]
    public class GetMaterialWhere : GetSingleWhere<Material> { }

    [Route("/Material/GetPaged/{Limit}/{Page}", "GET")]
    public class GetPagedMaterials : GetPaged<Material> { }
    #endregion

    #region Generic Write
    [Route("/Material/CreateInstance", "POST")]
    public class CreateMaterialInstance : Material { }

    [Route("/Material", "POST")]
    public class InsertMaterial : Material { }

    [Route("/Material", "PUT")]
    public class UpdateMaterial : Material { }

    [Route("/Material", "DELETE")]
    [Route("/Material/{Id}", "DELETE")]
    public class DeleteMaterial : Material { }
    #endregion
}
