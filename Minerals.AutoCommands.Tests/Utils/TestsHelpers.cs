namespace Minerals.AutoCommands.Tests.Utils
{
    public static class TestsHelpers
    {
        [ModuleInitializer]
        public static void InitializeVerifyForSourceGenerators()
        {
            DiffTools.UseOrder(DiffTool.VisualStudioCode, DiffTool.VisualStudio, DiffTool.Rider);
            Verifier.UseProjectRelativeDirectory("Snapshots");
            VerifierSettings.UseEncoding(Encoding.UTF8);
            VerifySourceGenerators.Initialize();
        }

        public static Task VerifyGenerator(IIncrementalGenerator generator, IIncrementalGenerator[] additional)
        {
            var compilation = CSharpCompilation.Create("Tests");
            CSharpGeneratorDriver.Create(additional)
                .RunGeneratorsAndUpdateCompilation
                (
                    compilation,
                    out var newCompilation,
                    out _
                );

            var driver = CSharpGeneratorDriver.Create(generator)
                .RunGenerators(newCompilation);

            return Verifier.Verify(driver);
        }

        public static Task VerifyGenerator(string source, IIncrementalGenerator generator, IIncrementalGenerator[] additional)
        {
            var tree = CSharpSyntaxTree.ParseText(source);
            var compilation = CSharpCompilation.Create
            (
                "Tests",
                [tree],
                [MetadataReference.CreateFromFile(tree.GetType().Assembly.Location)]
            );

            CSharpGeneratorDriver.Create(additional)
                .RunGeneratorsAndUpdateCompilation
                (
                    compilation,
                    out var newCompilation,
                    out _
                );

            var driver = CSharpGeneratorDriver.Create(generator)
                .RunGenerators(newCompilation);

            return Verifier.Verify(driver);
        }

        public static string MakeTestNamespace(string additionalUsings, string additionalCode)
        {
            return $$"""
            using System;
            using System.Text;
            using System.Linq;
            using System.Threading.Tasks;
            using System.Collections.Generic;
            {{additionalUsings}}

            namespace Minerals.Examples
            {
                {{additionalCode}}
            }
            """;
        }

        public static string MakeTestClass(string additionalCode)
        {
            return $$"""
            public class TestClass
            {
                {{additionalCode}}
            }
            """;
        }

        public static string MakeTestMethod(string additionalCode)
        {
            return $$"""
            public void TestMethod()
            {
                {{additionalCode}}
            }
            """;
        }
    }
}