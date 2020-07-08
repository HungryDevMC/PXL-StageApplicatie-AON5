using StageAPI.Data;
using StageAPI.Database;
using System.Threading.Tasks;

namespace StageAPI.Services
{
    public class AccountService
    {
        public static async Task<string> Login(string email, string password)
        {
            using (var p_User_Login = new StoredProcedure("p_User_Login"))
            {
                p_User_Login.AddParameter("sEmail", email, 50);
                p_User_Login.AddParameter("sPassword", password, 32);
                
                using (var reader = await p_User_Login.RunReader())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            // TODO: Also return role.
                            return reader.GetString(0);
                        }
                    }
                }
            }

            return null;
        }

        public static async Task<Account> GetFromToken(string token)
        {
            using (var p_User_GetByToken = new StoredProcedure("p_User_GetByToken"))
            {
                p_User_GetByToken.AddParameter("sToken", token, 32);

                using (var reader = await p_User_GetByToken.RunReader())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            return new Account
                            {
                                ID = reader.GetInt32(0),
                                Email = reader.GetString(1),
                                Password = reader.GetString(2),
                                Role = (AccountRole) reader.GetInt32(3)
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
