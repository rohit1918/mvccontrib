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
			AppendOutputInternal(value, "Append");
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

				_output.AppendLine("_buffer." + method + "(@\"" + value + "\");");
			}
		}

		public void AppendCodeLine(string code)
		{
			if(code != null)
			{
				_output.AppendLine("_buffer.AppendLine(Convert.ToString(" + code + "));");
			}
		}

		public void AppendCode(string code)
		{
			if(code != null)
			{
				_output.AppendLine("_buffer.Append(" + code + ");");
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
