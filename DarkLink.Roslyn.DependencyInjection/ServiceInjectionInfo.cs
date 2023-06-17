using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace DarkLink.Roslyn.DependencyInjection;

internal record ServiceInjectionInfo(INamedTypeSymbol ServiceType, IReadOnlyList<InjectedFieldInfo> Fields);
