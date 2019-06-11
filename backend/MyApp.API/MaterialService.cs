using MyApp.Logic.Entities;
using MyApp.Logic;
using Reusable.Rest;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace MyApp.API
{
    [Authenticate]
    public class MaterialService : Service
    {
        public IMaterialLogic Logic { get; set; }

        #region Endpoints - Generic Read Only
        public async Task<object> Get(GetAllMaterials request)
        {
            Logic.SetDb(Db);
            return await Logic.GetAllAsync();
        }

        public async Task<object> Get(GetMaterialById request)
        {
            Logic.SetDb(Db);
            return await Logic.GetByIdAsync(request.Id);
        }

        public async Task<object> Get(GetMaterialWhere request)
        {
            Logic.SetDb(Db);
            return await Logic.GetSingleWhereAsync(request.Property, request.Value);
        }

        public async Task<object> Get(GetPagedMaterials request)
        {
            Logic.SetDb(Db);
            Logic.Request = Request;
            return await Logic.GetPagedAsync(
                request.Limit,
                request.Page,
                request.FilterGeneral,
                null,
                null);
        }
        #endregion

        #region Endpoints - Generic Write
        public object Post(CreateMaterialInstance request)
        {
            Logic.SetDb(Db);
            var entity = request.ConvertTo<Material>();
            return new HttpResult(new CommonResponse(Logic.CreateInstance(entity)))
            {
                ResultScope = () => JsConfig.With(new Config { IncludeNullValues = true })
            };
        }

        public object Post(InsertMaterial request)
        {
            var entity = request.ConvertTo<Material>();
            InTransaction(() => Logic.Add(ref entity));
            return new CommonResponse(Logic.GetById(entity.Id));
        }

        public object Put(UpdateMaterial request)
        {
            var entity = request.ConvertTo<Material>();
            InTransaction(() => Logic.Update(entity));
            return new CommonResponse(Logic.GetById(entity.Id));
        }
        public object Delete(DeleteMaterial request)
        {
            var entity = request.ConvertTo<Material>();
            InTransaction(() => Logic.Remove(entity));
            return new CommonResponse();
        }
        #endregion

        #region Endpoints - Specific
        ///start:slot:endpoints<<<///end:slot:endpoints<<<
        #endregion

        private void InTransaction(params Action[] Operations)
        {
            Logic.SetDb(Db);
            Logic.SetAuth(GetSession());
            using (var transaction = Db.OpenTransaction())
            {
                try
                {
                    foreach (var operation in Operations)
                    {
                        operation();
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    #region Specific
    ///start:slot:endpointsRoutes<<<///end:slot:endpointsRoutes<<<
    #endregion

    #region Generic Read Only
    [Route("/Material", "GET")]
    public class GetAllMaterials : GetAll<Material> { }

    [Route("/Material/{Id}", "GET")]
    public class GetMaterialById : GetSingleById<Material> { }

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

    [Route("/Material/{Id}", "DELETE")]
    public class DeleteMaterial : Material { }
    #endregion
}
