using Elan.Common.Services;
using FluentAssertions;
using Xunit;

namespace Elan.Common.Tests
{
    public class QueryValidationServiceTests
    {
        [Fact]
        public void QueryValidationService_ShouldReturnFalse_ForEmptyQuery()
        {
            var sut = new QueryValidationService();

            sut.IsValidQuery("").Should().BeFalse();
            sut.IsValidQuery(null).Should().BeFalse();
            sut.IsValidQuery("  ").Should().BeFalse();
        }
    }
}
