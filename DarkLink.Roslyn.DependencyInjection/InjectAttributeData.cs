using System;
using DarkLink.RoslynHelpers;

namespace DarkLink.Roslyn.DependencyInjection;

[GenerateAttribute(AttributeTargets.Field, Name = "InjectAttribute")]
internal partial record InjectAttributeData(bool DO_NOT_SET_ME__I_AM_A_WORKAROUND = false);
