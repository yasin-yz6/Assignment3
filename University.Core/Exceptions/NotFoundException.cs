﻿namespace University.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Not found") { }
        public NotFoundException(string message) : base(message) { }
    }
}