using System;
using System.Text;

namespace LoggerLite;

public class JsonFileLogger : FileLoggerBase
{
	protected override string DefaultExtension => ".json";

	public JsonFileLogger(string path = null)
		: base(path)
	{
		base.Formatter = (string level, string message) => $"{{\"time\": \"{EscapeJsonString(DateTime.Now.ToString())}\", \"level\": \"{EscapeJsonString(level)}\", \"message\": \"{EscapeJsonString(message)}\"}}{Environment.NewLine}";
	}

	/// <summary>
	///     Escapes a string so it is safe to embed inside a JSON string literal (per RFC 8259).
	/// </summary>
	private static string EscapeJsonString(string value)
	{
		if (string.IsNullOrEmpty(value)) return value;
		var builder = new StringBuilder(value.Length + 8);
		foreach (var c in value)
		{
			switch (c)
			{
				case '"': builder.Append("\\\""); break;
				case '\\': builder.Append("\\\\"); break;
				case '\b': builder.Append("\\b"); break;
				case '\f': builder.Append("\\f"); break;
				case '\n': builder.Append("\\n"); break;
				case '\r': builder.Append("\\r"); break;
				case '\t': builder.Append("\\t"); break;
				default:
					if (c < ' ') builder.Append("\\u").Append(((int)c).ToString("x4"));
					else builder.Append(c);
					break;
			}
		}
		return builder.ToString();
	}
}
