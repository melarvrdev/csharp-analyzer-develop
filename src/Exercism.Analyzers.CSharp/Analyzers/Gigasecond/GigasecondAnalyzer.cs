using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondComments;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondAnalyzer
    {
        public static SolutionAnalysis Analyze(Solution solution) =>
            Analyze(GigasecondSolutionParser.Parse(solution));

        private static SolutionAnalysis Analyze(GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.DisapproveWhenInvalid() ??
            gigasecondSolution.ApproveWhenValid() ??
            gigasecondSolution.ReferToMentor();

        private static SolutionAnalysis DisapproveWhenInvalid(this GigasecondSolution gigasecondSolution)
        {
            if (gigasecondSolution.CreatesNewDatetime())
                gigasecondSolution.AddComment(DoNotCreateDateTime);
            
            if (gigasecondSolution.DoesNotUseAddSeconds())
                gigasecondSolution.AddComment(UseAddSeconds);

            return gigasecondSolution.HasComments()
                ? gigasecondSolution.Disapprove()
                : gigasecondSolution.ContinueAnalysis();
        }

        private static SolutionAnalysis ApproveWhenValid(this GigasecondSolution gigasecondSolution)
        {
            if (gigasecondSolution.UsesMathPow())
                gigasecondSolution.AddComment(UseScientificNotationNotMathPow(gigasecondSolution.GigasecondValue));

            if (gigasecondSolution.UsesDigitsWithoutSeparator())
                gigasecondSolution.AddComment(UseScientificNotationOrDigitSeparators(gigasecondSolution.GigasecondValue));

            if (gigasecondSolution.AssignsToParameterAndReturns() ||
                gigasecondSolution.AssignsToVariableAndReturns())
                gigasecondSolution.AddComment(DoNotAssignAndReturn);
                    
            if (gigasecondSolution.UsesLocalVariable() &&
                !gigasecondSolution.UsesLocalConstVariable())
                gigasecondSolution.AddComment(ConvertVariableToConst(gigasecondSolution.GigasecondValueVariableName));

            if (gigasecondSolution.UsesField() &&
                !gigasecondSolution.UsesConstField())
                gigasecondSolution.AddComment(ConvertFieldToConst(gigasecondSolution.GigasecondValueFieldName));

            if (gigasecondSolution.UsesField() &&
                !gigasecondSolution.UsesPrivateField())
                gigasecondSolution.AddComment(UsePrivateVisibility(gigasecondSolution.GigasecondValueFieldName));

            if (gigasecondSolution.UsesSingleLine() &&
                !gigasecondSolution.UsesExpressionBody())
                gigasecondSolution.AddComment(UseExpressionBodiedMember(gigasecondSolution.AddMethodName));

            if (gigasecondSolution.UsesScientificNotation() ||
                gigasecondSolution.UsesDigitsWithSeparator() ||
                gigasecondSolution.HasComments())
                return gigasecondSolution.Approve();
            
            return gigasecondSolution.ContinueAnalysis();
        }
    }
}