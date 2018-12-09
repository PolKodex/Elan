using Elan.Common.Contracts;

namespace Elan.Common.Services
{
    public class QueryValidationService: IQueryValidationService
    {
        public bool IsValidQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return false;
            }

            if (query.Length < 3)
            {
                return false;
            }

            return true;
        }
    }
}
