using System.IO;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Newtonsoft.Json;

namespace Exercism.Analyzers.CSharp.Tests.Analysis.Solutions
{
    internal class FakeSolution
    {
        private static readonly string SourceExercisesDirectory = Path.Combine("Analysis", "Solutions", "Exercises");

        private readonly Solution _solution;
        private readonly DirectoryInfo _fakeSolutionDirectory;
        private readonly string _implementationFileName;

        public FakeSolution(Solution solution, string implementationFileSuffix)
        {
            _solution = solution;
            _fakeSolutionDirectory = GetFakeSolutionDirectory(implementationFileSuffix);
            _implementationFileName = GetImplementationFileName(implementationFileSuffix);
        }

        private static DirectoryInfo GetFakeSolutionDirectory(string implementationFileSuffix) 
            => new DirectoryInfo(Path.Combine(SourceExercisesDirectory, implementationFileSuffix));

        private string GetImplementationFileName(string implementationFileSuffix) 
            => $"{_solution.Name}{implementationFileSuffix}.cs";

        public DirectoryInfo Create()
        {
            CreateSolutionDirectory();
            CreateSolutionFiles();

            return _fakeSolutionDirectory;
        }

        private void CreateSolutionDirectory()
        {
            if (_fakeSolutionDirectory.Exists)
                _fakeSolutionDirectory.Delete(recursive: true);

            _fakeSolutionDirectory.Create();
        }

        private void CreateSolutionFiles()
        {
            CreateMetadataFile();
            CopySolutionFile(_implementationFileName, ImplementationFileName);
            CopySolutionFile(TestFileName, TestFileName);
            CopySolutionFile(ProjectFileName, ProjectFileName);
        }

        private void CreateMetadataFile()
        {
            var metadata  = new {track = "csharp", exercise = _solution.Slug, id = _solution.Id};
            var metadataFilePath = GetFakeSolutionFilePath(MetadataFileName);
            File.WriteAllText(metadataFilePath, JsonConvert.SerializeObject(metadata));
        }

        private void CopySolutionFile(string sourceSolutionFileName, string fakeSolutionFileName) 
            => File.Copy(GetSourceSolutionFilePath(sourceSolutionFileName), GetFakeSolutionFilePath(fakeSolutionFileName));

        private string GetSourceSolutionFilePath(string fileName) 
            => Path.Combine(SourceExercisesDirectory, _solution.Name, fileName);
        
        private string GetFakeSolutionFilePath(string fileName) 
            => Path.Combine(_fakeSolutionDirectory.FullName, fileName);

        private string ImplementationFileName => $"{_solution.Name}.cs";

        private string TestFileName => $"{_solution.Name}Test.cs";

        private string ProjectFileName => $"{_solution.Name}.csproj";
        
        private string MetadataFileName => ".solution.json";
    }
}