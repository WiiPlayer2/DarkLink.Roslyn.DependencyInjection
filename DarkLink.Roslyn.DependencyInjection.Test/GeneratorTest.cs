namespace DarkLink.Roslyn.DependencyInjection.Test;

[TestClass]
public class GeneratorTest : VerifySourceGenerator
{
    [TestMethod]
    public async Task Empty()
    {
        var source = string.Empty;

        await Verify(source);
    }

    [TestMethod]
    public async Task MultipleFields()
    {
        var source = @"
using DarkLink.Roslyn.DependencyInjection;

public partial class Service
{
    [Inject]
    private readonly string injectedString;

    [Inject]
    private readonly int injectedInteger;
}

public static class Code
{
    public static void Do() => new Service(""test"", 1337);
}
";

        await Verify(source);
    }

    [TestMethod]
    public async Task SingleField()
    {
        var source = @"
using DarkLink.Roslyn.DependencyInjection;

public partial class Service
{
    [Inject]
    private readonly string injectedString;
}

public static class Code
{
    public static void Do() => new Service(""test"");
}
";

        await Verify(source);
    }
}
