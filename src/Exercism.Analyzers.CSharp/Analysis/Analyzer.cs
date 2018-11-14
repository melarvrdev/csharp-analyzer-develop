using System;
using System.Linq;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers;
using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Analysis
{
    public class Analyzer
    {
        private readonly SolutionDownloader _solutionDownloader;
        private readonly SolutionLoader _solutionLoader;
        private readonly SolutionCompiler _solutionCompiler;

        public Analyzer(SolutionDownloader solutionDownloader, SolutionLoader solutionLoader, SolutionCompiler solutionCompiler)
            => (_solutionDownloader, _solutionLoader, _solutionCompiler) = (solutionDownloader, solutionLoader, solutionCompiler); 
        
        public async Task<Diagnostic[]> Analyze(string slug, string uuid)
        {
            var compiledSolution = await CompileSolution(slug, uuid);

            return await Analyze(compiledSolution);
        }

        public async Task<Diagnostic[]> Analyze(CompiledSolution compiledSolution)
        {
            foreach (var solutionAnalyzer in SolutionAnalyzers.ForSolution(compiledSolution.Solution))
            {
                var diagnostics = await solutionAnalyzer.Analyze(compiledSolution);

                if (diagnostics.Any())
                    return diagnostics;
            }

            return Array.Empty<Diagnostic>();
        }

        private async Task<CompiledSolution> CompileSolution(string slug, string uuid)
        {
            var solution = new Solution(slug, uuid);
            var downloadedSolution = await _solutionDownloader.Download(solution);
            var loadedSolution = _solutionLoader.Load(downloadedSolution);
            return await _solutionCompiler.Compile(loadedSolution);
        }
    }
}