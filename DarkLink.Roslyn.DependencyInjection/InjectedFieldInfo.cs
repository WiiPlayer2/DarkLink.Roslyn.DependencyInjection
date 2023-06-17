using System;
using Microsoft.CodeAnalysis;

namespace DarkLink.Roslyn.DependencyInjection;

internal record InjectedFieldInfo(INamedTypeSymbol ServiceType, IFieldSymbol Field);
