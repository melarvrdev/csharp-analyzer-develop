using System;
using System.Net.Http;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Exercism.Analyzers.CSharp.Tests.Analysis.Solutions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Exercism.Analyzers.CSharp.Tests.Analysis
{
    public abstract class ExerciseAnalysisTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly Solution _solution;
        private readonly HttpClient _httpClient;
        private readonly FakeExercismCommandLineInterface _fakeExercismCommandLineInterface;

        protected ExerciseAnalysisTests(string slug, WebApplicationFactory<Startup> factory)
        {
            _solution = new Solution(Guid.NewGuid().ToString(), slug);
            _fakeExercismCommandLineInterface = new FakeExercismCommandLineInterface();
            _httpClient = AnalysisTestsHttpClientFactory.Create(factory, _fakeExercismCommandLineInterface);
        }

        protected async Task<Diagnostic[]> RequestAnalysis(string implementationFileSuffix)
        {
            _fakeExercismCommandLineInterface.Configure(_solution, implementationFileSuffix);

            var response = await _httpClient.GetAsync($"/api/analyze/{_solution.Id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Diagnostic[]>();
        }
    }
}