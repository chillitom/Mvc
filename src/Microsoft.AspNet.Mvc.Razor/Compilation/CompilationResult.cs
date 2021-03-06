// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNet.Mvc.Razor
{
    public class CompilationResult
    {
        private readonly Type _type;

        private CompilationResult(string generatedCode, Type type, IEnumerable<CompilationMessage> messages)
        {
            _type = type;
            GeneratedCode = generatedCode;
            Messages = messages.ToList();
        }

        public IEnumerable<CompilationMessage> Messages { get; private set; }
        
        public string GeneratedCode { get; private set; }

        public Type CompiledType
        {
            get
            {
                if (_type == null)
                {
                    throw new CompilationFailedException(Messages, GeneratedCode);
                }

                return _type;
            }
        }

        public static CompilationResult Failed(string generatedCode, IEnumerable<CompilationMessage> messages)
        {
            return new CompilationResult(generatedCode, type: null, messages: messages);
        }

        public static CompilationResult Successful(string generatedCode, Type type)
        {
            return new CompilationResult(generatedCode, type, Enumerable.Empty<CompilationMessage>());
        }
    }
}
