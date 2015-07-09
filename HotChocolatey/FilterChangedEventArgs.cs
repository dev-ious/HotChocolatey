﻿using System;

namespace HotChocolatey
{
    public class FilterChangedEventArgs : EventArgs
    {
        public IFilter Filter { get; }

        public FilterChangedEventArgs(IFilter filter)
        {
            Filter = filter;
        }
    }
}