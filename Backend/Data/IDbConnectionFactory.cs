using System.Data;

namespace Solarponics.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
    }
}