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
using Task = MyApp.Logic.Entities.Task;
///start:slot:imports<<<///end:slot:imports<<<

namespace MyApp.Logic
{
    public class TaskLogic : WriteLogic<Task>, ILogicWriteAsync<Task>
    {
        public UserLogic UserLogic { get; set; }
        public override void Init(IDbConnection db, IAuthSession auth, IRequest request)
        {
            base.Init(db, auth, request);
            UserLogic.Init(db, auth, request);
        }

        ///start:slot:init<<<///end:slot:init<<<

        ///start:slot:ctor<<<///end:slot:ctor<<<

        protected override Task OnCreateInstance(Task entity)
        {
            
            ///start:slot:createInstance<<<///end:slot:createInstance<<<

            return entity;
        }

        protected override SqlExpression<Task> OnGetList(SqlExpression<Task> query)
        {
            var sAdvancedSortName = Request.QueryString["advanced-sort"];
            if (sAdvancedSortName != null)
            {
                var advancedSort = Db.Single(Db.From<AdvancedSort>()
                    .LeftJoin<FilterData>()
                    .LeftJoin<SortData>()
                    .Where(e => e.UserName == Auth.UserName && e.Name == sAdvancedSortName));

                if (advancedSort != null)
                {
                    #region Filtering
                    //Status
                    var filterStatus = advancedSort.Filtering.FirstOrDefault(e => e.Key == "Status");
                    if (filterStatus != null && !string.IsNullOrWhiteSpace(filterStatus.Value))
                    {
                        var list = filterStatus.Value.FromJson<List<Catalog>>();
                        if (list.Count > 0)
                        {
                            query.Where(task => Db.Exists(Db.From<AdvancedSort, FilterData>((a, f) => a.Id == f.AdvancedSortId)
                                .Where(advSort => advSort.UserName == Auth.UserName && advSort.Name == sAdvancedSortName)
                                .And<FilterData>(f => f.Key == "Status" && f.Value.Contains(task.Status.ToString()))));
                        }
                    }

                    //Category
                    var filterCategory = advancedSort.Filtering.FirstOrDefault(e => e.Key == "Category");
                    if (filterCategory != null && !string.IsNullOrWhiteSpace(filterCategory.Value))
                    {
                        var list = filterCategory.Value.FromJson<List<Catalog>>();
                        if (list.Count > 0)
                        {
                            query.Where(task => Db.Exists(Db.From<AdvancedSort, FilterData>((a, f) => a.Id == f.AdvancedSortId)
                                .Where(advSort => advSort.UserName == Auth.UserName && advSort.Name == sAdvancedSortName)
                                .And<FilterData>(f => f.Key == "Category" && f.Value.Contains(task.Status.ToString()))));
                        }
                    }

                    //Priority
                    var filterPriority = advancedSort.Filtering.FirstOrDefault(e => e.Key == "Priority");
                    if (filterPriority != null && !string.IsNullOrWhiteSpace(filterPriority.Value))
                    {
                        var list = filterPriority.Value.FromJson<List<Catalog>>();
                        if (list.Count > 0)
                        {
                            query.Where(task => Db.Exists(Db.From<AdvancedSort, FilterData>((a, f) => a.Id == f.AdvancedSortId)
                                .Where(advSort => advSort.UserName == Auth.UserName && advSort.Name == sAdvancedSortName)
                                .And<FilterData>(f => f.Key == "Priority" && f.Value.Contains(task.Status.ToString()))));
                        }
                    }

                    //CreatedBy
                    var filterCreatedBy = advancedSort.Filtering.FirstOrDefault(e => e.Key == "CreatedBy");
                    if (filterCreatedBy != null && !string.IsNullOrWhiteSpace(filterCreatedBy.Value))
                    {
                        var list = filterCreatedBy.Value.FromJson<List<Catalog>>();
                        if (list.Count > 0)
                        {
                            query.Where(task => Db.Exists(Db.From<AdvancedSort, FilterData>((a, f) => a.Id == f.AdvancedSortId)
                                .Where(advSort => advSort.UserName == Auth.UserName && advSort.Name == sAdvancedSortName)
                                .And<FilterData>(f => f.Key == "CreatedBy" && f.Value.Contains("\"UserName\":" + task.CreatedBy + ","))));
                        }
                    }

                    //AssignedTo
                    var filterAssignedTo = advancedSort.Filtering.FirstOrDefault(e => e.Key == "AssignedTo");
                    if (filterAssignedTo != null && !string.IsNullOrWhiteSpace(filterAssignedTo.Value))
                    {
                        var list = filterAssignedTo.Value.FromJson<List<Catalog>>();
                        if (list.Count > 0)
                        {
                            query.Where(task => Db.Exists(Db.From<AdvancedSort, FilterData>((a, f) => a.Id == f.AdvancedSortId)
                                .Where(advSort => advSort.UserName == Auth.UserName && advSort.Name == sAdvancedSortName)
                                .And<FilterData>(f => f.Key == "AssignedTo" && f.Value.Contains("\"UserName\":" + task.AssignedTo + ","))));
                        }
                    }

                    //ClosedBy
                    var filterClosedBy = advancedSort.Filtering.FirstOrDefault(e => e.Key == "ClosedBy");
                    if (filterClosedBy != null && !string.IsNullOrWhiteSpace(filterClosedBy.Value))
                    {
                        var list = filterClosedBy.Value.FromJson<List<Catalog>>();
                        if (list.Count > 0)
                        {
                            query.Where(task => Db.Exists(Db.From<AdvancedSort, FilterData>((a, f) => a.Id == f.AdvancedSortId)
                                .Where(advSort => advSort.UserName == Auth.UserName && advSort.Name == sAdvancedSortName)
                                .And<FilterData>(f => f.Key == "ClosedBy" && f.Value.Contains("\"UserName\":" + task.ClosedBy + ","))));
                        }
                    }
                    #endregion

                    #region Sorting
                    advancedSort.Sorting = advancedSort.Sorting.OrderBy(s => s.Sequence).ToList();
                    query.OrderBy(e => 0);

                    foreach (var sort in advancedSort.Sorting)
                    {
                        OrderBy(query, sort);
                    }
                    #endregion
                }
            }
            query.Where(e => !e.IsCanceled);

            ///start:slot:listQuery<<<///end:slot:listQuery<<<

            return query;
        }

        protected override SqlExpression<Task> OnGetSingle(SqlExpression<Task> query)
        {
            
            ///start:slot:singleQuery<<<///end:slot:singleQuery<<<

            return query;
        }

        protected override void OnBeforeSaving(Task entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            ///start:slot:beforeSave<<<///end:slot:beforeSave<<<
        }

        protected override void OnAfterSaving(Task entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            
            ///start:slot:afterSave<<<///end:slot:afterSave<<<
        }

        protected override void OnBeforeRemoving(Task entity)
        {
            
            ///start:slot:beforeRemove<<<///end:slot:beforeRemove<<<
        }

        protected override List<Task> AdapterOut(params Task[] entities)
        {
            ///start:slot:adapterOut<<<///end:slot:adapterOut<<<

            foreach (var item in entities)
            {
                
            }

            return entities.ToList();
        }

        private SqlExpression<Task> OrderBy(SqlExpression<Task> query, SortData sort)
        {
            switch (sort.Value)
            {
                case "Status":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.Status.ToString());
                    }
                    else
                    {
                        query = query.ThenBy(e => e.Status.ToString());
                    }
                    break;
                case "Created By":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.CreatedBy);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.CreatedBy);
                    }
                    break;
                case "Assigned To":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.AssignedTo);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.AssignedTo);
                    }
                    break;
                case "Priority":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.Priority.ToString());
                    }
                    else
                    {
                        query = query.ThenBy(e => e.Priority.ToString());
                    }
                    break;
                case "Category":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.Category);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.Category);
                    }
                    break;
                case "Title":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.Title);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.Title);
                    }
                    break;
                case "Description":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.Description);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.Description);
                    }
                    break;
                case "Closed By":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.ClosedBy);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.ClosedBy);
                    }
                    break;
                case "Date Created At":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.CreatedAt);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.CreatedAt);
                    }
                    break;
                case "Date Due Date":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.DueDate);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.DueDate);
                    }
                    break;
                case "Date Closed":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.ClosedAt);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.ClosedAt);
                    }
                    break;
                default:
                    break;
            }

            return query;
        }

        public void SaveAll(Task template, List<Contact> asignees)
        {
            var existingTasks = GetAll()
                .Where(e => e.ForeignApp == template.ForeignApp
                        && e.ForeignType == template.ForeignType
                        && e.ForeignKey == template.ForeignKey
                        && !e.IsCanceled)
                .ToList();

            #region No Asignees
            //Also compare count for performance purposes so we don't GetAll Users.
            if (asignees == null && asignees.Count == 0)
            {
                foreach (var task in existingTasks)
                {
                    task.IsCanceled = true;
                    Update(task);
                }
                return;
            }
            #endregion

            #region Existing Tasks / Update
            foreach (var task in existingTasks)
            {
                var asignee = asignees.FirstOrDefault(e => e.UserName.ToLower() == task.AssignedTo.ToLower());
                if (asignee != null)
                {
                    //Update Tasks if already exists for user
                    var taskToUpdate = template.ConvertTo<Task>();
                    taskToUpdate.Id = task.Id;
                    taskToUpdate.RowVersion = task.RowVersion;
                    Update(task);
                }
                else
                {
                    //Cancel Task when user is removed from Approvers
                    task.IsCanceled = true;
                    Db.Update<Task>(new { IsCancelled = true }, e => e.Id == task.Id);
                }
                //Cancel Task when user no longer exists
                //TODO
            }
            #endregion

            #region New Asignees
            //Create Task only if it is not already created for user
            foreach (var asignee in asignees)
                if (!existingTasks.Any(t => t.AssignedTo.ToLower() == asignee.UserName.ToLower()))
                {
                    var taskToAdd = template.ConvertTo<Task>(); //clone
                    taskToAdd.AssignedTo = asignee.UserName;
                    Add(taskToAdd);
                }
            #endregion
        }

        public void SaveSingle(Task template)
        {
            if (string.IsNullOrWhiteSpace(template.AssignedTo))
                throw new KnownError("Error. Cannot save task. AssignedTo is a required field.");

            var task = GetAll()
                .Where(e => e.ForeignApp == template.ForeignApp
                    && e.ForeignType == template.ForeignType
                    && e.ForeignKey == template.ForeignKey
                    && !e.IsCanceled)
                .Where(e => e.AssignedTo.ToLower() == template.AssignedTo.ToLower())
                .FirstOrDefault();

            if (task == null)
                Add(template);
            else
            {
                template.Id = task.Id;
                template.RowVersion = task.RowVersion;
                Update(template);
            }
        }

        public void Done(Task template)
        {
            if (string.IsNullOrWhiteSpace(template.AssignedTo))
                throw new KnownError("Error. Cannot save task. AssignedTo is a required field.");

            var task = GetAll()
                .Where(e => e.ForeignApp == template.ForeignApp
                    && e.ForeignType == template.ForeignType
                    && e.ForeignKey == template.ForeignKey
                    && !e.IsCanceled)
                .Where(e => e.AssignedTo.ToLower() == template.AssignedTo.ToLower())
                .FirstOrDefault();

            if (task == null)
            {
                throw new KnownError("Task not found.");
                template.ClosedAt = DateTimeOffset.Now;
                template.ClosedBy = Auth.UserName;
                //Missing other foreign fields.

                Add(template);
            }
            else
            {
                task.Status = string.IsNullOrWhiteSpace(template.Status) ? "COMPLETED" : template.Status;
                task.ClosedAt = DateTimeOffset.Now;
                task.ClosedBy = Auth.UserName;

                Update(task);
            }
        }

        //Cancel Task for all users:
        public void CancelAllFromForeign(BaseEntity foreign, string foreignType, string foreignApp)
        {
            var tasks = Db.Select<Task>(t => !t.IsCanceled
                            && t.ForeignKey == foreign.Id
                            && t.ForeignType == foreignType
                            && t.ForeignApp == foreignApp);

            foreach (var task in tasks)
            {
                task.IsCanceled = true;
                Update(task);
            }
        }

        ///start:slot:logic<<<///end:slot:logic<<<
    }
}
