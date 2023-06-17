using System;
using DarkLink.RoslynHelpers;

namespace DarkLink.Roslyn.DependencyInjection;

[GenerateAttribute(AttributeTargets.Field, Name = "InjectAttribute")]
internal partial record InjectAttributeData;
