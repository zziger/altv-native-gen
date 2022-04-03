﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AltV.NativesDb.Reader.Extensions;
using AltV.NativesDb.Reader.Models.NativeDb;
using Durty.AltV.NativesTypingsGenerator.Converters;
using Durty.AltV.NativesTypingsGenerator.Models.Typing;

namespace Durty.AltV.NativesTypingsGenerator.TypingDef
{
    public class TypeDefCSharpFileGenerator
    {
        private readonly TypeDef _typeDefFile;
        private readonly bool _generateDocumentation;

        public TypeDefCSharpFileGenerator(
            TypeDef typeDefFile,
            bool generateDocumentation = true)
        {
            _typeDefFile = typeDefFile;
            _generateDocumentation = generateDocumentation;
        }

        public string Generate(bool generateHeader = true, List<string> customHeaderLines = null)
        {
            StringBuilder fileContent = new StringBuilder(string.Empty);
            if (generateHeader)
            {
                if (customHeaderLines != null)
                {
                    foreach (var customHeaderLine in customHeaderLines)
                    {
                        fileContent.Append($"//{customHeaderLine}\n");
                    }
                }
                fileContent.Append("\n");
            }

            foreach (TypeDefModule typeDefModule in _typeDefFile.Modules)
            {
                fileContent.Append(GenerateModule(typeDefModule));
                fileContent.Append("\n");
            }

            return fileContent.ToString();
        }

        private StringBuilder GenerateModule(TypeDefModule typeDefModule)
        {
            StringBuilder result = new StringBuilder(string.Empty);
            result.Append($"using System.Numerics;\n");
            result.Append($"using System.Reflection;\n");
            result.Append($"using AltV.Net.Shared.Utils;\n");
            result.Append($"using System.Runtime.InteropServices;\n\n");
            result.Append($"namespace AltV.Net.Client\n{{\n");
            result.Append($"\tpublic unsafe interface INatives\n\t{{\n");
            result = typeDefModule.Functions.Aggregate(result, (current, typeDefFunction) => current.Append($"{GenerateFunctionDefinition(typeDefFunction, "\t\t")};\n"));
            result.Append("\t}\n\n");
            result.Append($"\tpublic unsafe class Natives : INatives\n\t{{\n");
            result.Append($"\t\tprivate IntPtr handle;\n");
            result.Append($"\t\tprivate delegate* unmanaged[Cdecl]<nint, void> freeString;\n");
            foreach (var typeDefFunction in typeDefModule.Functions)
            {
                result.Append($"\t\tprivate {GetUnmanagedDelegateType(typeDefFunction)} {GetFixedTypeDefFunctionName(typeDefFunction.Name)};\n");
            }
            result.Append($"\n");
            result.Append($"\t\tpublic Natives(string dllName)\n\t\t{{\n");
            result.Append($"\t\t\tconst DllImportSearchPath dllImportSearchPath = DllImportSearchPath.LegacyBehavior | DllImportSearchPath.AssemblyDirectory | DllImportSearchPath.SafeDirectories | DllImportSearchPath.System32 | DllImportSearchPath.UserDirectories | DllImportSearchPath.ApplicationDirectory | DllImportSearchPath.UseDllDirectoryForDependencies;\n");
            result.Append($"\t\t\thandle = NativeLibrary.Load(dllName, Assembly.GetExecutingAssembly(), dllImportSearchPath);\n");
            result.Append($"\t\t\tfreeString = (delegate* unmanaged[Cdecl]<nint, void>) NativeLibrary.GetExport(handle, \"FreeString\");\n");
            result.Append($"\t\t}}\n\n");
            
            result = typeDefModule.Functions.Aggregate(result, (current, typeDefFunction) => current.Append($"{GenerateFunction(typeDefFunction)}\n"));
            result.Append("\t}\n");
            result.Append("}");
            return result;
        }

        private string GetFixedTypeDefFunctionName(string name)
        {
            return "fn_" + (name.StartsWith("_") ? name : "_" + name);
        }
        
        private string GetUnmanagedDelegateType(TypeDefFunction function)
        {
            var converter = new NativeTypeToCSharpTypingConverter();
            return $"delegate* unmanaged[Cdecl]<{string.Join("", function.Parameters.Select(p => $"{converter.Convert(null, p.NativeType, p.IsReference, true)}, "))}{converter.Convert(null, function.ReturnType.NativeType[0], false, true)}>";
        }

        private string GetFixedTypeDefParameterName(string name)
        {
            return IsParameterNameReservedCSharpKeyWord(name) ? "@" + name : name;
        }
        private string GetEscapedTypeDefParameterName(string name)
        {
            return "_" + name;
        }

        private StringBuilder GenerateFunctionDefinition(TypeDefFunction typeDefFunction, string prepend = "", bool forceIgnoreDocs = false, bool isInterface = true)
        {
            StringBuilder result = new StringBuilder(string.Empty);
            if (_generateDocumentation && !forceIgnoreDocs)
            {
                result.Append(GenerateFunctionDocumentation(typeDefFunction));
            }
            
            var cSharpReturnType = new NativeTypeToCSharpTypingConverter().Convert(null, typeDefFunction.ReturnType.NativeType[0], false);
            result.Append($"{prepend}{cSharpReturnType} {typeDefFunction.Name.FirstCharToUpper()}(");
            foreach (var parameter in typeDefFunction.Parameters)
            {
                var name = isInterface ? GetFixedTypeDefParameterName(parameter.Name) : GetEscapedTypeDefParameterName(parameter.Name);
                result.Append($"{(parameter.IsReference ? "ref " : "")}{new NativeTypeToCSharpTypingConverter().Convert(null, parameter.NativeType, false)} {name}");
                if (typeDefFunction.Parameters.Last() != parameter)
                {
                    result.Append(", ");
                }
            }
            result.Append($")");
            
            return result;
        }

        private StringBuilder GenerateFunction(TypeDefFunction typeDefFunction)
        {
            StringBuilder result = new StringBuilder(string.Empty);
            StringBuilder stringsFree = new StringBuilder(string.Empty);
            var fixedTypeDefName = GetFixedTypeDefFunctionName(typeDefFunction.Name);
            result.Append($"{GenerateFunctionDefinition(typeDefFunction, "\t\tpublic ", true, false)}\n");
            result.Append($"\t\t{{\n");
            result.Append($"\t\t\tunsafe {{\n");
            result.Append($"\t\t\t\tif ({fixedTypeDefName} == null) {fixedTypeDefName} = ({GetUnmanagedDelegateType(typeDefFunction)}) NativeLibrary.GetExport(handle, \"Native_{typeDefFunction.Name}\");\n");
            result.Append(GenerateInvoke(typeDefFunction));
            result.Append($"\t\t\t}}\n");
            result.Append($"\t\t}}\n");

            return result;
        }

        private string GenerateInvoke(TypeDefFunction typeDefFunction)
        {
            var returnType = typeDefFunction.ReturnType.NativeType[0];
            var methodName = GetFixedTypeDefFunctionName(typeDefFunction.Name);
            var beforeCall = new StringBuilder();
            var afterCall = new StringBuilder();
            var prependCall = new StringBuilder("\t\t\t\t");
            
            var call = new StringBuilder($"{methodName}(");
            
            foreach (var parameter in typeDefFunction.Parameters)
            {
                var last = parameter == typeDefFunction.Parameters.Last();
                var argName = GetEscapedTypeDefParameterName(parameter.Name);

                if (parameter.IsReference && parameter.NativeType == NativeType.String)
                {
                    beforeCall.Append($"\t\t\t\tvar ptr{argName} = MemoryUtils.StringToHGlobalUtf8({argName});\n");
                    beforeCall.Append($"\t\t\t\tvar ref{argName} = ptr{argName};\n");

                    call.Append($"&ref{argName}");
                    
                    afterCall.Append($"\t\t\t\t{argName} = Marshal.PtrToStringUTF8(ref{argName});\n");
                    afterCall.Append($"\t\t\t\tfreeString(ref{argName});\n");
                    afterCall.Append($"\t\t\t\tMarshal.FreeHGlobal(ptr{argName});\n");
                } 
                else if (parameter.IsReference)
                {
                    beforeCall.Append($"\t\t\t\tvar ref{argName} = {argName};\n");
                    call.Append($"&ref{argName}");
                    afterCall.Append($"\t\t\t\t{argName} = ref{argName};\n");
                }
                else if (parameter.NativeType == NativeType.String)
                {
                    beforeCall.Append($"\t\t\t\tvar ptr{argName} = MemoryUtils.StringToHGlobalUtf8({argName});\n");
                    call.Append($"ptr{argName}");
                    afterCall.Append($"\t\t\t\tMarshal.FreeHGlobal(ptr{argName});\n");
                }
                else
                {
                    call.Append(argName);
                }

                if (!last) call.Append(", ");
            }
            
            call.Append(");\n");

            if (returnType != NativeType.Void)
            {
                prependCall.Append("var result = ");
                
                if (returnType == NativeType.String)
                {
                    afterCall.Append($"\t\t\t\treturn Marshal.PtrToStringUTF8(result);\n");
                }
                else
                {
                    afterCall.Append($"\t\t\t\treturn result;\n");
                }
            }
            
            return beforeCall.ToString() + prependCall.ToString() + call.ToString() + afterCall.ToString();
        }

        private void GenerateCallParameters(StringBuilder result, TypeDefFunction typeDefFunction, bool closeFunction = true)
        {
            foreach (var parameter in typeDefFunction.Parameters)
            {
                var ptr = parameter.IsReference ? "ref" : parameter.NativeType == NativeType.String ? "ptr" : "";
                result.Append($"{(parameter.IsReference ? "&" : "")}{ptr}{GetEscapedTypeDefParameterName(parameter.Name)}");
                if (typeDefFunction.Parameters.Last() != parameter)
                {
                    result.Append(", ");
                }
            }

            result.Append(closeFunction ? $");\n" : ")");
        }

        private StringBuilder GenerateFunctionDocumentation(TypeDefFunction typeDefFunction)
        {
            //When no docs exist
            if (typeDefFunction.ReturnType.NativeType.Count <= 1 &&
                string.IsNullOrEmpty(typeDefFunction.Description) &&
                typeDefFunction.Parameters.All(p => string.IsNullOrEmpty(p.Description) && string.IsNullOrEmpty(typeDefFunction.ReturnType.Description)))
                return new StringBuilder(string.Empty);

            StringBuilder result = new StringBuilder($"\t\t/// <summary>\n");
            if (!string.IsNullOrEmpty(typeDefFunction.Description))
            {
                string[] descriptionLines = typeDefFunction.Description.Split("\n");
                foreach (string descriptionLine in descriptionLines)
                {
                    string sanitizedDescriptionLine = descriptionLine.Replace("/*", string.Empty).Replace("*/", string.Empty).Trim();
                    result.Append($"\t\t/// {sanitizedDescriptionLine}\n");
                }
            }
            result.Append("\t\t/// </summary>\n");

            //Add @remarks in the future?
            foreach (var parameter in typeDefFunction.Parameters)
            {
                if (!string.IsNullOrEmpty(parameter.Description))
                {
                    result.Append($"\t\t/// <param name=\"{GetFixedTypeDefParameterName(parameter.Name)}\">{parameter.Description}</param>\n");
                }
            }
            
            if (!string.IsNullOrEmpty(typeDefFunction.ReturnType.Description))
            {
                result.Append($"\t\t/// <returns>{typeDefFunction.ReturnType.Description}</returns>\n");
            }
            return result;
        }

        private bool IsParameterNameReservedCSharpKeyWord(string parameterName)
        {
            var reservedKeywords = new List<string>()
            {
                "abstract",
                "as",
                "base",
                "bool",
                "break",
                "byte",
                "case",
                "catch",
                "char",
                "checked",
                "class",
                "const",
                "continue",
                "decimal",
                "default",
                "delegate",
                "do",
                "double",
                "else",
                "enum",
                "event",
                "explicit",
                "extern",
                "false",
                "finally",
                "fixed",
                "float",
                "for",
                "foreach",
                "goto",
                "if",
                "implicit",
                "in",
                "int",
                "interface",
                "internal",
                "is",
                "lock",
                "long",
                "namespace",
                "new",
                "null",
                "object",
                "operator",
                "out",
                "override",
                "params",
                "private",
                "protected",
                "public",
                "readonly",
                "ref",
                "return",
                "sbyte",
                "sealed",
                "short",
                "sizeof",
                "stackalloc",
                "static",
                "string",
                "struct",
                "switch",
                "this",
                "throw",
                "true",
                "try",
                "typeof",
                "uint",
                "ulong",
                "unchecked",
                "unsafe",
                "ushort",
                "using",
                "static",
                "virtual",
                "void",
                "volatile",
                "while"
            };
            return reservedKeywords.Any(k => parameterName.ToLower() == k);
        }
    }
}
