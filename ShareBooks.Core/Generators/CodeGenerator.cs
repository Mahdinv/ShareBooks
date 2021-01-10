using System;
using System.Collections.Generic;
using System.Text;

namespace ShareBooks.Core.Generators
{
    public class CodeGenerator
    {
        public static string ActiveCode()
        {
            Random random = new Random();

            return random.Next(100000, 999000).ToString();
        }
    }
}
