﻿using System;
using NuGet;

namespace HotChocolatey.Model
{
    public class UninstallAction : IAction
    {
        private readonly Package package;

        public UninstallAction(Package package)
        {
            this.package = package;
        }

        public void Execute(ChocoExecutor chocoExecutor, SemanticVersion specificVersion, Action<string> outputLineCallback)
        {
            chocoExecutor.Uninstall(package, outputLineCallback);
        }
    }
}