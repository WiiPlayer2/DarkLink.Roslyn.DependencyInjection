using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace DarkLink.Roslyn.DependencyInjection;

[Generator]
public class Generator : IIncrementalGenerator
{
    private readonly int kjsd;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(PostInitialize);

        var serviceInjections = InjectAttributeData.Find(
                context.SyntaxProvider,
                (node, cancellationToken) => true,
                Transform)
            .Where(x => x is not null)
            .Select((x, _) => x!)
            .Collect()
            .Select((x, _) => x
                .GroupBy(x => x.ServiceType, SymbolEqualityComparer.Default)
                .Select(x => new ServiceInjectionInfo((INamedTypeSymbol) x.Key!, x.ToList())))
            .SelectMany((x, _) => x);

        context.RegisterImplementationSourceOutput(serviceInjections, GenerateServiceInjection);
    }

    private void GenerateServiceInjection(SourceProductionContext context, ServiceInjectionInfo serviceInjection)
    {
        var encoding = new UTF8Encoding(false);
        var stringWriter = new StringWriter();

        if (!serviceInjection.ServiceType.ContainingNamespace.IsGlobalNamespace)
            stringWriter.WriteLine($"namespace {serviceInjection.ServiceType.ContainingNamespace} {{");

        stringWriter.WriteLine($"partial class {GetPartialTypeName(serviceInjection.ServiceType)} {{");
        stringWriter.WriteLine($"public {serviceInjection.ServiceType.Name}({string.Join(", ", serviceInjection.Fields.Select(f => $"{f.Field.Type} {f.Field.Name}"))}) {{");
        foreach (var field in serviceInjection.Fields) stringWriter.WriteLine($"this.{field.Field.Name} = {field.Field.Name};");
        stringWriter.WriteLine("}");
        stringWriter.WriteLine("}");

        if (!serviceInjection.ServiceType.ContainingNamespace.IsGlobalNamespace)
            stringWriter.WriteLine("}");

        var code = SourceText.From(stringWriter.ToString(), encoding);
        context.AddSource($"{serviceInjection.ServiceType.Name}.g.cs", code);

        static string GetPartialTypeName(INamedTypeSymbol typeSymbol)
            => typeSymbol.ContainingNamespace.IsGlobalNamespace
                ? typeSymbol.ToString()
                : typeSymbol.ToString().Substring(typeSymbol.ContainingNamespace.ToString().Length + 1);
    }

    private void PostInitialize(IncrementalGeneratorPostInitializationContext context)
    {
        InjectAttributeData.AddTo(context);
    }

    private static InjectedFieldInfo? Transform(
        GeneratorAttributeSyntaxContext context,
        IReadOnlyList<InjectAttributeData> attributeData,
        CancellationToken cancellationToken)
    {
        if (attributeData.Count > 1)
            return default;

        var data = attributeData.First();
        return new InjectedFieldInfo(context.TargetSymbol.ContainingType, (IFieldSymbol) context.TargetSymbol);
    }
}
