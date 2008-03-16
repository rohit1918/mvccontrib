using System;
using System.Text;
using System.Text.RegularExpressions;
using MvcContrib.NHamlViewEngine.Utilities;

using System.Linq;
using System.Collections.Generic;

namespace MvcContrib.NHamlViewEngine
{
	public sealed class ViewBuilder
	{
		private static readonly Regex _typeNameCleaner
			= new Regex(@"`\d+", RegexOptions.Compiled | RegexOptions.Singleline);

		private readonly StringBuilder _output = new StringBuilder();

		private readonly string _className;

		private int _depth;

		public ViewBuilder(TemplateCompiler templateCompiler, string className, params Type[] genericArguments)
		{
			_className = className;

			_output.AppendLine(StringUtils.FormatInvariant("public class {0} : {1}, ICompiledView {{",
			                                               _className,
			                                               MakeBaseTypeName(templateCompiler.ViewBaseType, genericArguments)));
			_output.AppendLine("StringBuilder _buffer;");
			_output.AppendLine("public string Render(){");
			_output.AppendLine("_buffer = new StringBuilder();");
		}

		public string ClassName
		{
			get { return _className; }
		}

		public static string MakeBaseTypeName(Type baseType, params Type[] genericArguments)
		{
            if (genericArguments != null && genericArguments.Length > 0)
            {
                baseType = baseType.MakeGenericType(genericArguments);
            }
            var tname = baseType.FullName.Replace('+', '.');

            if (baseType.IsGenericType)
            {
                tname = tname.Substring(0, tname.IndexOf('`'));
                tname += "<";

                var parameters = from t in baseType.GetGenericArguments()
                                 select MakeBaseTypeName(t, null);

                tname += string.Join(",", parameters.ToArray());

                tname += ">";
            }

            return tname;
		}

        public void AppendOutput(string value)
        {
            AppendOutput(value, false);
        }
		public void AppendOutput(string value, bool newLine)
		{
			AppendOutputInternal(value, newLine ? "AppendLine" :"Append");
		}

		public void AppendOutputLine(string value)
		{
			AppendOutputInternal(value, "AppendLine");
		}

		private void AppendOutputInternal(string value, string method)
		{
			if(value != null)
			{
				value = value.Replace("\"", "\"\"");

				if(_depth > 0)
				{
					if(value.StartsWith(string.Empty.PadLeft(_depth * 2), StringComparison.Ordinal))
					{
						value = value.Remove(0, _depth * 2);
					}
				}

                //_output.Append("_buffer." + method + "(@\"" + value + "\");");
				_output.AppendLine("_buffer." + method + "(@\"" + value + "\");");
			}
		}

		public void AppendCodeLine(string code)
		{
            AppendCode(code, true);
		}

        public void AppendCode(string code)
        {
            AppendCode(code, false);
        }
		public void AppendCode(string code, bool newLine)
		{
			if(code != null)
			{
                var action = newLine ? "AppendLine" : "Append";
				_output.AppendLine("_buffer." + action + "(Convert.ToString(" + code + "));");
			}
		}

		public void AppendSilentCode(string code, bool appendSemicolon)
		{
			if(code != null)
			{
				code = code.Trim();

				if(appendSemicolon && !code.EndsWith(";", StringComparison.Ordinal))
				{
					code += ';';
				}

				_output.AppendLine(code);
			}
		}

		public void BeginCodeBlock()
		{
			_depth++;
			_output.AppendLine("{");
		}

		public void EndCodeBlock()
		{
			_output.AppendLine("}");
			_depth--;
		}

		public string Build()
		{
			_output.Append("return _buffer.ToString();}}");

			return _output.ToString();
		}
	}
}
