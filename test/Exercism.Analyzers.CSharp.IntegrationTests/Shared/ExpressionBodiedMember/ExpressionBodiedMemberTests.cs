using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Shared.ExpressionBodiedMember
{
    public class ExpressionBodiedMemberTests : SolutionAnalysisTests
    {
        public ExpressionBodiedMemberTests(WebApplicationFactory<Startup> factory) : base(factory, FakeExercise.Shared)
        {
        }

        [Fact]
        public async Task MethodThatIsAlreadyAnExpressionBodiedMemberDoesNotReturnComment() =>
            await AnalysisDoesNotReturnComment();

        [Fact]
        public async Task MethodThatCanBeConvertedToExpressionBodiedMemberReturnsComment() =>
            await AnalysisReturnsComment("The 'IsEven' method can be rewritten as an expression-bodied member.");
    }
}