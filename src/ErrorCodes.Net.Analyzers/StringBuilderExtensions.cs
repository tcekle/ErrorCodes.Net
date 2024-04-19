using System;
using System.Text;

namespace ErrorCodes.Net.Analyzers;

/// <summary>
/// Extensions for <see cref="StringBuilder"/>.
/// </summary>
public static class StringBuilderExtensions
{
    /// <summary>
    /// Append lines to the <see cref="StringBuilder"/> with indentation.
    /// </summary>
    /// <param name="sb">The <see cref="StringBuilder"/> to use</param>
    /// <param name="textBlock">The text to append</param>
    /// <param name="level">The indentation level</param>
    /// <returns></returns>
    public static StringBuilder AppendLineIndented(this StringBuilder sb, string textBlock, int level = 0)
    {
        string indentation = new string(' ', 4 * level);

        var lines = textBlock.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++)
        {
            if (i > 0)
            {
                sb.AppendLine();
            }

            sb.Append(indentation);
            sb.Append(lines[i]);  
        }

        return sb;
    }
}