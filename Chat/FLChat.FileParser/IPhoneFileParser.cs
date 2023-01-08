using System.Collections.Generic;

namespace FLChat.FileParser
{
    public interface IPhoneFileParser
    {
        List<string> Parse(string file, string name);
    }
}