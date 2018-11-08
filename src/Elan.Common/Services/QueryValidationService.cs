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

            return true;
        }
    }
}
