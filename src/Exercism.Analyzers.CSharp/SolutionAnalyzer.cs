using System.IO;
using Exercism.Analyzers.CSharp.Analyzers;
using Serilog;

namespace Exercism.Analyzers.CSharp
{
    internal static class SolutionAnalyzer
    {
        public static AnalyzedSolution Analyze(Solution solution)
        {
            Log.Information("Analyzing exercise {Exercise}.", solution.Exercise);
            
            var implementation = solution.ToSolutionImplementation();
            if (implementation == null)
                return null;

            var analyzedSolution = AnalyzedSolutionImplementation(solution, implementation);
            Log.Information("Analyzed exercise {Exercise} has status {Status} and comments {Comments}.", analyzedSolution.Status, analyzedSolution.Comments);
            
            return analyzedSolution;
        }

        private static AnalyzedSolution AnalyzedSolutionImplementation(Solution solution, SolutionImplementation implementation)
        {
            // TODO: check to see if there are errors in syntax
            
            switch (solution.Exercise)
            {
                case Exercises.Gigasecond: return GigasecondAnalyzer.Analyze(implementation);
                case Exercises.Leap: return LeapAnalyzer.Analyze(implementation);
                default: return DefaultAnalyzer.Analyze(implementation);
            }
        }

        private static SolutionImplementation ToSolutionImplementation(this Solution solution)
        {
            if (!File.Exists(solution.Paths.ImplementationFilePath))
            {
                Log.Error("Implementation file {File} does not exist.", solution.Paths.ImplementationFilePath);
                return null;
            }
            
            var implementationCode = File.ReadAllText(solution.Paths.ImplementationFilePath);
            var implementationSyntaxNode = SyntaxNodeParser.ParseNormalizedRoot(implementationCode);

            return new SolutionImplementation(solution, implementationSyntaxNode);
        }
    }
}