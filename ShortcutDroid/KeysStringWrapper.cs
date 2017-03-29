using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortcutDroid
{
    class KeysStringWrapper
    {
        public string Send(string input)
        {
            string output;
            for(int i=0;i<input.Length;i++)
            {
                output = "";
                if(input[i]=='\\')
                {
                    i++;
                    switch (input[i])
                    {
                        case 's': //shift
                            {
                                output += "+" +
                                specialConvert(input.Substring(i+1), '+');
                            }break;
                        case 'c': //ctrl
                            {
                                output += "^" +
                                specialConvert(input.Substring(i+1), '^');
                            }
                            break;
                        case 'a': //alt
                            {
                                output += "%" +
                                specialConvert(input.Substring(i+1), '%');
                            }
                            break;
                        case 't': //tab
                            {
                                output += "{TAB}";
                            }
                            break;
                        case 'n': //enter
                            {
                                output += "~";
                            }
                            break;
                        case '\\':
                            {
                                output += "\\\\";
                            }
                            break;
                        case '{':
                            {
                                output += "{";
                            }
                            break;
                        case '}':
                            {
                                output += "}";
                            }
                            break;
                        case '[':
                            {
                                output += "[";
                            }
                            break;
                        case ']':
                            {
                                output += "]";
                            }
                            break;
                        case 'f':
                            {
                                output+=convertF(input.Substring(i + 1));
                            }
                            break;
                        default:
                            {
                                output += "\\";
                            }
                            break;
                    }
                }
                else
                {
                    output = ""+input[i];
                }
                return output;
            }
        }

        private string specialConvert(string input, char prev)
        {
            if(input[0]=='\\')
            {
                if(prev=='+')
                {
                    if (input[1] == 'c') return "^";
                    if (input[1] == 'a') return "%";
                }
                else if (prev == '^')
                {
                    if (input[1] == 's') return "+";
                    if (input[1] == 'a') return "%";
                }
                else if (prev == '%')
                {
                    if (input[1] == 'c') return "^";
                    if (input[1] == 's') return "+";
                }
                else 
                {
                    switch (input[1])
                    {
                        case 't': //tab
                            {
                                return "{TAB}";
                            }
                        case 'n': //enter
                            {
                                return "~";
                            }
                        case '\\':
                            {
                                return "\\\\";
                            }
                        case '{':
                            {
                                return "{";
                            }
                        case '}':
                            {
                                return "}";
                            }
                        case '[':
                            {
                                    return "[";
                            }
                        case ']':
                            {
                                    return "]";
                            }
                        case 'f':
                            {
                                    return convertF(input.Substring(2));
                            }
                        default:
                            {
                                    return "\\";
                            }
                    }
                }
            }
            else return ""+input[0];
        }

        private string convertF(string input)
        {
            if (input.Substring(0, 2) == "10")
            {
                return "{F10}";
            }
            else if (input.Substring(0, 2) == "11")
            {
                return "{F11}";
            }
            else if (input.Substring(0, 2) == "12")
            {
                return "{F12}";
            }
            else if (input[0]=='1')
            {
                return "{F1}";
            }
            else if (input[0] == '2')
            {
                return "{F2}";
            }
            else if (input[0] == '3')
            {
                return "{F3}";
            }
            else if (input[0] == '4')
            {
                return "{F4}";
            }
            else if (input[0] == '5')
            {
                return "{F5}";
            }
            else if (input[0] == '6')
            {
                return "{F6}";
            }
            else if (input[0] == '7')
            {
                return "{F7}";
            }
            else if (input[0] == '8')
            {
                return "{F8}";
            }
            else if (input[0] == '9')
            {
                return "{F9}";
            }
            return "\\f" + input[0];
        }
    }
}
