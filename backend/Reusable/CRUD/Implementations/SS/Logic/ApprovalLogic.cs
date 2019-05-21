using Reusable.CRUD.Entities;
using Reusable.CRUD.JsonEntities;
using ServiceStack.OrmLite;
using System;
using System.Linq;
using Reusable.EmailServices;
using Reusable.Rest;
using Reusable.CRUD.Contract;

namespace Reusable.CRUD.Implementations.SS.Logic
{
    public class ApprovalLogic : DocumentLogic<Approval>, IApprovalLogic
    {
        public UserLogic UserLogic { get; set; }
        public TaskLogic TaskLogic { get; set; }

        #region Overrides
        protected override SqlExpression<Approval> OnGetList(SqlExpression<Approval> query)
        {
            return query
                .LeftJoin<Track>()
                .LeftJoin<Track, User>((t, u) => t.CreatedById == u.Id)
                .LeftJoin<ApprovalApprover>()
                .LeftJoin<ApprovalApprover, User>();
        }

        protected override SqlExpression<Approval> OnGetSingle(SqlExpression<Approval> query)
        {
            return query
                .LeftJoin<Track>()
                .LeftJoin<Track, User>((t, u) => t.CreatedById == u.Id)
                .LeftJoin<ApprovalApprover>()
                .LeftJoin<ApprovalApprover, User>();
        }

        protected override Approval OnCreateInstance(Approval entity)
        {
            if (entity.Id < 0)
            {
                //Force create
            }
            else
            {
                //Return existent if it exists:
                var query = OnGetSingle(Db.From<Approval>())
                                    .Where(e => e.PrimaryForeignKey == entity.PrimaryForeignKey)
                                    .Where(e => e.PrimaryForeignType == entity.PrimaryForeignType)
                                    .Where(e => e.SecondaryForeignKey == entity.SecondaryForeignKey)
                                    .Where(e => e.SecondaryForeignType == e.SecondaryForeignType)
                                    .Where(e => e.TargetRevisionKey == entity.TargetRevisionKey)
                                    .OrderBy(e => e.Id);

                var existent = Db.Single(query);

                if (existent != null)
                    return existent;
            }

            entity.Id = 0;
            entity.DateRequested = DateTimeOffset.Now;
            entity.Status = "Requested";

            var baseURL = @"";

            switch (entity.Type)
            {
                case "":
                    entity.RequestDescription = "";

                    // entity.Approvers = ctx.Users.Where(u => u.UserKey == 5 || u.UserKey == 23).ToList();
                    entity.Title = "";
                    entity.Hyperlink = baseURL + "?id=";

                    break;
                default:
                    break;
            }
            return entity;
        }

        protected override void OnBeforeSaving(Approval entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            #region Validations
            User currentUser = UserLogic.GetById(LoggedUser.UserID ?? -1);
            if (currentUser == null)
                throw new KnownError("Logged User not found or session expired.");

            if (entity.Approvers == null)
                throw new KnownError("Approvers field is required.");
            #endregion

            if (mode == OPERATION_MODE.ADD)
            {
                Email emailEntity = new Email
                {
                    CreatedAt = DateTimeOffset.Now
                };

                var hyperlink = entity.Hyperlink;

                EmailService emailService = new EmailService("secure.emailsrvr.com", 587)
                {
                    FromPassword = currentUser.EmailPassword,
                    From = currentUser.Email,
                    Subject = entity.Title,
                    Body = entity.Title
                            + "<br><br>" + entity.RequestDescription
                            + @"<br><br>Open document here: <a href=""" + hyperlink + @""">" + "Something Descriptive" + "</a>"
                };

                foreach (var item in entity.Approvers)
                {
                    emailService.To.Add(item.Approver.Email);
                }

                emailService.Bcc.Add(currentUser.Email);

                try
                {
                    emailService.SendMail();
                }
                catch (Exception ex)
                {
                    throw new KnownError("Could not send email, please verify your Profile settings.\n" + ex.Message);
                }
            }
        }

        protected override void OnAfterSaving(Approval entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            var responsibles = entity.Approvers.Select(u => new Contact()
            {
                Email = u.Approver.Email,
                Value = u.Approver.Value
            }).ToList();

            TaskLogic.SaveTasks(responsibles, entity);
        }

        #endregion

        #region Specific Operations
        public void SetStatus(long ApprovalKey, string Message, string Status)
        {
            var approval = GetById(ApprovalKey);

            if (approval == null)
                throw new KnownError("Approval does not exist.");

            if (LoggedUser.Role != "Administrator")
            {
                var userValid = approval.Approvers?.FirstOrDefault(u => u.Id == LoggedUser.UserID);
                if (userValid == null)
                {
                    throw new KnownError("Logged User not lised as Approver.");
                }
            }

            approval.Status = Status;
            //approval.ResponseDescription = Message;
            //approval.DateResponse = DateTimeOffset.Now;
            //approval.StatusUpdatedByKey = LoggedUser.UserID;
            Db.Update(approval);

            //Updating Tasks associated:
            TaskLogic.SaveTask(LoggedUser.UserID ?? 0, approval);
        }
        #endregion


    }
}
