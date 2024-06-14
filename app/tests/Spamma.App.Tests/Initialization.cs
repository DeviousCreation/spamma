using System.Runtime.CompilerServices;
using DiffEngine;

namespace Spamma.App.Tests;

public static class Initialization
{
    [ModuleInitializer]
    public static void Run()
    {
        DiffRunner.Disabled = true;
    }
}