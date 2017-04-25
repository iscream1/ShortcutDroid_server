using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortcutDroid
{
    class KeysContainer
    {
        public List<Character> keys;
        public KeysContainer()
        {
            keys = new List<Character>();
        }

        public void Append(string inp)
        {
            keys.Add(new Character(inp));
        }

        public override string ToString()
        {
            StringBuilder sb=new StringBuilder();
            foreach(Character c in keys)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }
    }

    class Character
    {
        string c;
        public Character(string inp)
        {
            c = inp;
        }

        public override string ToString()
        {
            return c;
        }
    }
}
