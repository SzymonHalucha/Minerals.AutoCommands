

// namespace Minerals.AutoCommands.Generators
// {
//     [DiagnosticAnalyzer(LanguageNames.CSharp)]
//     public class CommandArgumentAnalyzer : DiagnosticAnalyzer
//     {
//         private static readonly DiagnosticDescriptor _test = new
//         (
//             "MAC0001", 
//             "Test Title", 
//             "Test Message", 
//             "Design", 
//             DiagnosticSeverity.Warning, 
//             true
//         );

//         public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [_test];

//         public override void Initialize(AnalysisContext context)
//         {
//             context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
//             context.EnableConcurrentExecution();
//         }
//     }
// }