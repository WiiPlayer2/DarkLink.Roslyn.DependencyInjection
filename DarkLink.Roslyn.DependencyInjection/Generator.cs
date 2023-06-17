using System;
using Microsoft.CodeAnalysis;

namespace DarkLink.Roslyn.DependencyInjection;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(PostInitialize);

        // Initialize
    }

    private void PostInitialize(IncrementalGeneratorPostInitializationContext context)
    {
        InjectAttribute.AddTo(context);
    }
}
