using Reusable.CRUD.Entities;
using ServiceStack.OrmLite;
using System;
using System.Linq;
using Reusable.CRUD.Contract;
using System.Collections.Generic;
using Reusable.CRUD.JsonEntities;
using ServiceStack;
using ServiceStack.Text;
using Reusable.Rest;

namespace Reusable.CRUD.Implementations.SS.Logic
{
    public class TaskLogic : LogicWrite<Task>, ITaskLogic
    {
        #region Overrides
        protected override SqlExpression<Task> OnGetList(SqlExpression<Task> query)
        {
            var sAdvancedSortName = Request.QueryString["advanced-sort"];
            if (sAdvancedSortName != null)
            {
                var advancedSort = Db.Single(Db.From<AdvancedSort>()
                    .LeftJoin<FilterData>()
                    .LeftJoin<SortData>()
                    .Where(e => e.UserId == LoggedUser.UserID && e.Name == sAdvancedSortName));

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
                                .Where(advSort => advSort.UserId == LoggedUser.UserID && advSort.Name == sAdvancedSortName)
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
                                .Where(advSort => advSort.UserId == LoggedUser.UserID && advSort.Name == sAdvancedSortName)
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
                                .Where(advSort => advSort.UserId == LoggedUser.UserID && advSort.Name == sAdvancedSortName)
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
                                .Where(advSort => advSort.UserId == LoggedUser.UserID && advSort.Name == sAdvancedSortName)
                                .And<FilterData>(f => f.Key == "CreatedBy" && f.Value.Contains("\"id\":" + task.UserCreatedBy.Id + ","))));
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
                                .Where(advSort => advSort.UserId == LoggedUser.UserID && advSort.Name == sAdvancedSortName)
                                .And<FilterData>(f => f.Key == "AssignedTo" && f.Value.Contains("\"id\":" + task.UserAssignedTo.Id + ","))));
                        }
                    }

                    //CompletedBy
                    var filterCompletedBy = advancedSort.Filtering.FirstOrDefault(e => e.Key == "CompletedBy");
                    if (filterCompletedBy != null && !string.IsNullOrWhiteSpace(filterCompletedBy.Value))
                    {
                        var list = filterCompletedBy.Value.FromJson<List<Catalog>>();
                        if (list.Count > 0)
                        {
                            query.Where(task => Db.Exists(Db.From<AdvancedSort, FilterData>((a, f) => a.Id == f.AdvancedSortId)
                                .Where(advSort => advSort.UserId == LoggedUser.UserID && advSort.Name == sAdvancedSortName)
                                .And<FilterData>(f => f.Key == "CompletedBy" && f.Value.Contains("\"id\":" + task.UserCompletedBy.Id + ","))));
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

            query
                .LeftJoin<User>((t, u) => t.UserCreatedById == u.Id)
                .Where(e => e.IsCancelled == null || e.IsCancelled == false);

            query.PrintDump();

            return query;
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
                        query = query.ThenByDescending(e => e.UserCreatedBy.Value);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.UserCreatedBy.Value);
                    }
                    break;
                case "Assigned To":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.UserAssignedTo.Value);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.UserAssignedTo.Value);
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
                case "Completed By":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.UserCompletedBy.Value);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.UserCompletedBy.Value);
                    }
                    break;
                case "Date Created At":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.DateCreatedAt);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.DateCreatedAt);
                    }
                    break;
                case "Date Due Date":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.DateDue);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.DateDue);
                    }
                    break;
                case "Date Closed":
                    if (sort.AscDesc == "DESC")
                    {
                        query = query.ThenByDescending(e => e.DateClosed);
                    }
                    else
                    {
                        query = query.ThenBy(e => e.DateClosed);
                    }
                    break;
                default:
                    break;
            }

            return query;
        }
        #endregion

        #region Specific Operations
        private Task CreateTaskFromEntity(BaseEntity fromEntity, long? userKey)
        {
            switch (fromEntity.EntityName)
            {
                case "Approval":
                    var approval = fromEntity as Approval;
                    var taskFromApproval = new Task()
                    {
                        Category = "Approval",
                        DateDue = DateTimeOffset.Now,
                        Priority = Task.TaskPriority.MEDIUM,
                        Status = Task.TaskStatus.PENDING,
                        Title = approval.Title,
                        UserAssignedToId = userKey,
                        DateClosed = null,
                        DateCreatedAt = DateTimeOffset.Now,
                        ForeignKey = approval.Id,
                        ForeignType = approval.EntityName,
                        ForeignURLKey = approval.PrimaryForeignKey,
                        ForeignURLType = approval.PrimaryForeignType,
                        UserCreatedById = LoggedUser.UserID,
                        Description = approval.RequestDescription
                    };

                    if (approval.Status == "Rejected" || approval.Status == "Approved")
                    {
                        taskFromApproval.DateClosed = DateTimeOffset.Now;
                        taskFromApproval.Status = Task.TaskStatus.COMPLETED;
                        taskFromApproval.UserCompletedById = LoggedUser.UserID;
                    }
                    else if (approval.Status != "Rejected" && approval.Status != "Approved")
                    {
                        taskFromApproval.Status = Task.TaskStatus.IN_PROGRESS;
                    }

                    return taskFromApproval;

                default:
                    throw new KnownError("Error. Could not create Task. Entity type not supported yet.");
            }
        }

        private void UpdateTaskFromEntity(Task task, BaseEntity fromEntity)
        {
            switch (fromEntity.EntityName)
            {
                case "Approval":
                    var approval = fromEntity as Approval;
                    task.Description = approval.RequestDescription;

                    if (task.Status != Task.TaskStatus.COMPLETED && (approval.Status == "Rejected" || approval.Status == "Approved"))
                    {
                        task.DateClosed = DateTimeOffset.Now;
                        task.Status = Task.TaskStatus.COMPLETED;
                        task.UserCompletedById = LoggedUser.UserID;
                    }
                    else if (approval.Status != "Rejected" && approval.Status != "Approved")
                    {
                        task.Status = Task.TaskStatus.IN_PROGRESS;
                    }
                    break;

                default:
                    break;
            }
        }

        public void SaveTasks(List<Contact> responsibles, BaseEntity entity = null)
        {
            var users = Db.Select<User>(u => !u.IsDeleted && u.Email != null);

            var tasks = Db.Select(Db.From<Task>()
                .Where(e => e.ForeignKey == entity.Id && e.ForeignType == entity.EntityName)
                .Where(e => e.IsCancelled == null || e.IsCancelled == false));

            if (responsibles != null && responsibles.Count > 0)
            {
                foreach (var responsible in responsibles)
                {
                    var user = users.FirstOrDefault(u => responsible.Email != null
                                        && responsible.Email.Trim().ToLower() == u.Email.Trim().ToLower());
                    if (user != null)
                    {
                        //Create Task only if it is not created for user
                        var task = tasks.FirstOrDefault(t => t.UserAssignedToId == user.Id);
                        if (task == null)
                        {
                            task = CreateTaskFromEntity(entity, user.Id);
                            Db.Insert(task);
                        }
                    }
                }

                foreach (var task in tasks)
                {
                    var userAssigned = users.FirstOrDefault(u => u.Id == task.UserAssignedToId);
                    if (userAssigned != null)
                    {
                        var userResponsible = responsibles.FirstOrDefault(u => userAssigned.Email != null && userAssigned.Email.Trim().ToLower() == u.Email.Trim().ToLower());
                        if (userResponsible != null)
                        {
                            //Update Tasks if already exists for user
                            UpdateTaskFromEntity(task, entity);
                            Db.Update(task);
                        }
                        else
                        {
                            //Cancel Task when user is removed from Approvers
                            task.IsCancelled = true;
                            Db.Update<Task>(new { IsCancelled = true }, e => e.Id == task.Id);
                        }
                    }
                    else
                    {
                        //Cancel Task when user no longer exists
                        task.IsCancelled = true;
                        Db.Update<Task>(new { IsCancelled = true }, e => e.Id == task.Id);
                    }
                }
            }
            else
            {
                //Cancel all tasks when there are no users
                foreach (var task in tasks)
                {
                    task.IsCancelled = true;
                    Db.Update<Task>(new { IsCancelled = true }, e => e.Id == task.Id);
                }
            }
        }

        public void SaveTask(Contact responsible, BaseEntity entity = null)
        {
            if (string.IsNullOrWhiteSpace(responsible.Email))
                throw new KnownError("Error. Missing Responsible's Email.");

            var user = Db.Single(Db.From<User>()
                .Where(u => !u.IsDeleted && u.Email.Trim().ToLower() == responsible.Email.Trim().ToLower()));

            var task = GetTaskFromEntity(entity, user.Id);

            if (task == null)
            {
                task = CreateTaskFromEntity(entity, user.Id);
                Db.Insert(task);
            }
            else
            {
                UpdateTaskFromEntity(task, entity);
                Db.Update(task);
            }
        }

        public void SaveTask(long responsibleKey, BaseEntity entity = null)
        {
            if (responsibleKey <= 0)
                throw new KnownError("Error. Missing Responsible's ID.");

            var task = GetTaskFromEntity(entity, responsibleKey);

            if (task == null)
            {
                task = CreateTaskFromEntity(entity, responsibleKey);
                Db.Insert(task);
            }
            else
            {
                UpdateTaskFromEntity(task, entity);
                Db.Update(task);
            }
        }

        public Task GetTaskFromEntity(BaseEntity fromEntity, long? userKey)
        {
            var query = Db.From<Task>()
                .Where(t => t.ForeignKey == fromEntity.Id)
                .Where(t => t.ForeignType == fromEntity.EntityName)
                .Where(t => t.IsCancelled == null || t.IsCancelled == false)
                .Where(t => t.UserAssignedToId == userKey);

            return Db.Single(query);
        }

        //Cancel Task for all users:
        public void CancelTask(BaseEntity fromEntity)
        {
            var task = Db.Single(Db.From<Task>()
                .Where(t => t.IsCancelled == null || t.IsCancelled == false)
                .Where(t => t.ForeignKey == fromEntity.Id && t.ForeignType == fromEntity.EntityName));

            if (task != null)
            {
                task.IsCancelled = true;
                Db.Update<Task>(new { IsCancelled = true }, t => t.Id == task.Id);
            }
        }
        #endregion
    }
}
