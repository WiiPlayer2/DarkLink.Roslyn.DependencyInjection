using System.Runtime.CompilerServices;

namespace DarkLink.Roslyn.DependencyInjection.Test
{
    public static class ModuleInitializer
    {
        [ModuleInitializer]
        public static void Init() => VerifySourceGenerators.Enable();
    }
}