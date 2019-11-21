using MyApp.Logic.Entities;
using Reusable.Attachments;
using Reusable.CRUD.Contract;
using Reusable.CRUD.Entities;
using Reusable.CRUD.Implementations.SS;
using Reusable.CRUD.JsonEntities;
using Reusable.EmailServices;
using Reusable.Rest;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

///start:slot:imports<<<///end:slot:imports<<<


namespace MyApp.Logic
{
    public class EmailLogic : DocumentLogic<Email>, IDocumentLogicAsync<Email>
    {
        public ActivityLogic ActivityLogic { get; set; }
        public override void Init(IDbConnection db, IAuthSession auth, IRequest request, IAppSettings appSettings)
        {
            base.Init(db, auth, request, appSettings);
            ActivityLogic.Init(db, auth, request, appSettings);
        }

        ///start:slot:init<<<///end:slot:init<<<

        ///start:slot:ctor<<<///end:slot:ctor<<<

        protected override Email OnCreateInstance(Email entity)
        {
            
            ///start:slot:createInstance<<<///end:slot:createInstance<<<

            return entity;
        }

        protected override SqlExpression<Email> OnGetList(SqlExpression<Email> query)
        {
            
            ///start:slot:listQuery<<<///end:slot:listQuery<<<

            return base.OnGetList(query);
        }

        protected override SqlExpression<Email> OnGetSingle(SqlExpression<Email> query)
        {
            
            ///start:slot:singleQuery<<<///end:slot:singleQuery<<<

            return base.OnGetSingle(query);
        }

        protected override void OnBeforeSaving(Email entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            if (mode == OPERATION_MODE.ADD)
            {
                #region Validations
                if (entity.ToList.Count() == 0 && entity.CcList.Count() == 0 && entity.BccList.Count == 0)
                    throw new KnownError("Cannot send email without Recipients.");
                #endregion

                entity.CreatedAt = DateTimeOffset.Now;
            }

            #region Send Email

            //Copy Attachments when resent:
            if (entity.IsResent)
            {
                string baseAttachmentsPath = AppSettings.Get<string>("EmailAttachments");
                entity.AttachmentsFolder = AttachmentsIO.CopyAttachments(entity.AttachmentsFolder, entity.Attachments, baseAttachmentsPath);
                entity.Attachments = entity.Attachments.Where(e => !e.ToDelete).ToList();
            }

            var emailService = new MailgunService
            {
                From = Auth.Email,
                Subject = entity.Subject,
                Body = entity.Body,
                AttachmentsFolder = entity.AttachmentsFolder
            };

            foreach (var item in entity.ToList)
                emailService.To.Add(item.Email);

            foreach (var item in entity.CcList)
                emailService.Cc.Add(item.Email);

            foreach (var item in entity.BccList)
                emailService.Bcc.Add(item.Email);

            emailService.Bcc.Add(Auth.Email); //Add sender as recipient as well.

            try
            {
                emailService.SendMail();
            }
            catch (Exception ex)
            {
                throw new KnownError("Could not send email:\n" + ex.Message);
            }
            #endregion

            ///start:slot:beforeSave<<<///end:slot:beforeSave<<<
        }

        protected override void OnAfterSaving(Email entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            var recipients = entity.ToList
                .Concat(entity.CcList)
                .Concat(entity.BccList)
                .Select(e => e.Email)
                .Distinct()
                .ToCsv();

            ActivityLogic.Add(new Activity
            {
                Category = "Email",
                Title = $"Recipients: {recipients}",
                Description = $"{entity.Body}",
                ForeignApp = "NCN",
                ForeignKey = entity.Id,
                ForeignCommonKey = entity.ForeignCommonKey,
                ForeignType = "Email",
                ForeignURL = $"https://ncn.capsonic.com/ncn?id={entity.ForeignCommonKey}"
            });

            ///start:slot:afterSave<<<///end:slot:afterSave<<<
        }

        protected override void OnBeforeRemoving(Email entity)
        {
            
            ///start:slot:beforeRemove<<<///end:slot:beforeRemove<<<
        }

        protected override List<Email> AdapterOut(params Email[] entities)
        {
            ///start:slot:adapterOut<<<///end:slot:adapterOut<<<

            foreach (var item in entities)
            {
                item.Attachments = AttachmentsIO.getAttachmentsFromFolder(item.AttachmentsFolder, "EmailAttachments");

            }

            return entities.ToList();
        }

        protected override void OnFinalize(Email entity)
        {
            ///start:slot:finalize<<<///end:slot:finalize<<<
        }

        protected override void OnUnfinalize(Email entity)
        {
            ///start:slot:unfinalize<<<///end:slot:unfinalize<<<
        }

        
        ///start:slot:logic<<<///end:slot:logic<<<
    }
}
