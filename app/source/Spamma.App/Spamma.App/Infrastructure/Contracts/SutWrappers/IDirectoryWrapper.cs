﻿using System.Diagnostics.CodeAnalysis;

namespace Spamma.App.Infrastructure.Contracts.SutWrappers;

public interface IDirectoryWrapper
{
    void CreateDirectory(string path);

    bool Exists(string path);
}

[ExcludeFromCodeCoverage]
public class DirectoryWrapper : IDirectoryWrapper
{
    public void CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);
    }

    public bool Exists(string path)
    {
        return Directory.Exists(path);
    }
}