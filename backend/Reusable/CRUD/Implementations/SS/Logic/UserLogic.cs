using Reusable.CRUD.Entities;
using ServiceStack.OrmLite;
using System;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using Reusable.Rest;
using Reusable.CRUD.Contract;
using System.Threading.Tasks;

namespace Reusable.CRUD.Implementations.SS.Logic
{
    public class UserLogic : DocumentLogic<User>, IUserLogic
    {
        #region Overrides
        protected override SqlExpression<User> OnGetList(SqlExpression<User> query)
        {
            return query
                .LeftJoin<Role>()
                .Where(e => e.Role != "Administrator")
                .OrderBy(e => e.Role)
                .ThenBy(e => e.Value);
        }

        protected override SqlExpression<User> OnGetSingle(SqlExpression<User> query)
        {
            return query;
        }

        protected override User OnCreateInstance(User entity)
        {
            entity.Role = "User";
            return entity;
        }

        protected override void OnBeforeSaving(User entity, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            if (string.IsNullOrWhiteSpace(entity.Value))
                throw new KnownError("[User Name] is a required field.");

            if (string.IsNullOrWhiteSpace(entity.UserName))
                throw new KnownError("[User Name] is a required field.");

            if (entity.Id == 0 || (entity.Id > 0 && entity.ChangePassword))
            {
                if (string.IsNullOrWhiteSpace(entity.Password) || string.IsNullOrWhiteSpace(entity.ConfirmPassword))
                    throw new KnownError("[Password] and [Confirm Password] are required fields.");

                if (entity.Password != entity.ConfirmPassword)
                    throw new KnownError("[Password] does not match with its confirmation.");

                if (entity.Password.Length < 6)
                    throw new KnownError("[User Name] has to have at least 6 characters.");
            }

            //Creating..
            if (mode == OPERATION_MODE.ADD)
            {
                entity.CreatedAt = DateTimeOffset.Now;
                entity.EmailServer = "secure.emailsrvr.com";
                entity.EmailPort = "587";
            }

            //TODO: Hash Password
            entity.EmailPassword = entity.EmailPassword;

            //Bitmap identicon;
            //try
            //{
            //    var bg = new StaticColorBrushGenerator(StaticColorBrushGenerator.ColorFromText(entity.UserName));
            //    identicon = new IdenticonGenerator("MD5")
            //        .WithSize(100, 100)
            //        .WithBackgroundColor(Color.White)
            //        .WithBlocks(4, 4)
            //        .WithBlockGenerators(IdenticonGenerator.ExtendedBlockGeneratorsConfig)
            //        .WithBrushGenerator(bg)
            //        .Create(entity.UserName);
            //}
            //catch (Exception ex)
            //{
            //    throw new KnownError("Ha ocurrido un error al intentar crear el usuario.");
            //}

            //ImageConverter converter = new ImageConverter();

            //try
            //{
            //    entity.Identicon64 = Convert.ToBase64String(ConvertBitMapToByteArray(identicon));
            //    entity.Identicon = (byte[])converter.ConvertTo(identicon, typeof(byte[]));
            //}
            //catch (Exception ex)
            //{
            //    throw new KnownError("Ha ocurrido un error al intentar crear el usuario");
            //}
        }

        #endregion

        #region Specific Operations
        public User GetByUserName(string UserName)
        {
            var query = Db.From<User>()
                .Where(u => u.UserName == UserName);

            var user = Db.Single(OnGetSingle(query));

            AdapterOut(user);

            return user;
        }
        public List<User> GetByRole(string Role)
        {
            var query = Db.From<User>()
                .Where(u => u.Role == Role);

            var users = Db.Select(OnGetList(query));

            AdapterOut(users.ToArray());

            return users;
        }

        public async Task<User> GetByUserNameAsync(string UserName)
        {
            var query = Db.From<User>()
                .Where(u => u.UserName == UserName);

            var user = await Db.SingleAsync(OnGetSingle(query));

            AdapterOut(user);

            return user;
        }
        public async Task<List<User>> GetByRoleAsync(string Role)
        {
            var query = Db.From<User>()
                .Where(u => u.Role == Role);

            var users = await Db.SelectAsync(OnGetList(query));

            AdapterOut(users.ToArray());

            return users;
        }
        private byte[] ConvertBitMapToByteArray(Bitmap bitmap)
        {
            byte[] result = null;

            if (bitmap != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    result = stream.ToArray();
                }
            }

            return result;
        }
        #endregion
    }
}
