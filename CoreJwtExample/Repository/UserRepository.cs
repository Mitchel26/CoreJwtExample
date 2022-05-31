using CoreJwtExample.Common;
using CoreJwtExample.IRepository;
using CoreJwtExample.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace CoreJwtExample.Repository
{
    public class UserRepository : IUserRepository
    {
        string _connectionString = "";
        User _oUser = new User();
        List<User> _oUsers = new List<User>();

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SchoolDB");
        }
        public async Task<string> Delete(User obj)
        {
            string message = "";
            try
            {
                using (IDbConnection con = new SqlConnection(_connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    var Users = await con.QueryAsync<User>("SP_User",
                        this.SetParameters(obj, (int)OperationType.Delete),
                        commandType: CommandType.StoredProcedure);
                    message = "Deleted";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        public async Task<User> Get(int objId)
        {
            _oUser = new User();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var Users = await con.QueryAsync<User>(String.Format(@"select * from User where UserId = {0}", objId));
                if (Users != null && Users.Count() > 0)
                {
                    _oUser = Users.SingleOrDefault();
                }
            }
            return _oUser;
        }

        public async Task<User> GetByUsernamePassword(User user)
        {
            _oUser = new User();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string sql = string.Format(@"select * from [User] where Username='{0}' and Password='{1}'", user.Username, user.Password);
                var Users = await con.QueryAsync<User>(sql);
                if (Users != null && Users.Count() > 0)
                {
                    _oUser = Users.SingleOrDefault();
                }

            }
            return _oUser;
        }

        public async Task<List<User>> Gets()
        {
            _oUsers = new List<User>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var Users = await con.QueryAsync<User>("select * from User");
                if (Users != null && Users.Count() > 0)
                {
                    _oUsers = Users.ToList();
                }
            }
            return _oUsers;
        }

        public async Task<User> Save(User obj)
        {
            _oUser = new User();

            try
            {
                int operationType = Convert.ToInt32(obj.UserId == 0 ? OperationType.Insert : OperationType.Update);

                using (IDbConnection con = new SqlConnection(_connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    var Users = await con.QueryAsync<User>("SP_User",
                           SetParameters(obj, operationType), commandType: CommandType.StoredProcedure);
                    if (Users != null && Users.Count() > 0)
                    {
                        _oUser = Users.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                _oUser = new User();
                _oUser.Message = ex.Message;
            }
            return _oUser;

        }

        private DynamicParameters SetParameters(User oUser, int nOperationType)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", oUser.UserId);
            parameters.Add("@Username", oUser.Username);
            parameters.Add("@Email", oUser.Email);
            parameters.Add("@Password", oUser.Password);
            parameters.Add("@OperationType", nOperationType);
            return parameters;
        }
    }
}
