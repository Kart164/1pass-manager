using System.Data;
using System.Threading.Tasks;

namespace _1Pass.NetStandart.Libs.DBAPI
{
    public interface IDatabase
    {
        string Password { set; get; }
        IDbConnection GetConnection();
        int CreateDatabase();
        int UpdateToActual();
        Task<int> DeleteDatabase();
        Task<bool> TryLogin(string pass);
        bool CheckExistence { get; }

    }
}
