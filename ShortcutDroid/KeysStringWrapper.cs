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
        int i;
        public void Send(string input)
        {
            string output="";
            for(i=0;i<input.Length;i++)
            {
                output = "";
                if(input[i]=='\\')
                {
                    i++;
                    switch (input[i])
                    {
                        case 's': //shift
                            {
                                output += "+" + specialConvert(input.Substring(i+1), '+');
                            }
                            break;
                        case 'c': //ctrl
                            {
                                output += "^" + specialConvert(input.Substring(i+1), '^');
                            }
                            break;
                        case 'a': //alt
                            {
                                output += "%" + specialConvert(input.Substring(i+1), '%');
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
                                output += "{{}";
                            }
                            break;
                        case '}':
                            {
                                output += "{}}";
                            }
                            break;
                        case '(':
                            {
                                output += "{(}";
                            }
                            break;
                        case ')':
                            {
                                output += "{)}";
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
                                i--;
                                output += "\\";
                            }
                            break;
                    }
                }
                else
                {
                    output += ""+input[i];
                }
                Console.WriteLine(output);
                SendKeys.SendWait(output);
            }
        }

        private string specialConvert(string input, char prev)
        {
            Console.WriteLine("spec in:"+ input + "prev:"+prev);
            string output="";
            if (input[0] == '\\')
            {
                if (prev == '+')
                {
                    if (input[1] == 'c') output = "^";
                    if (input[1] == 'a') output = "%";
                }
                else if (prev == '^')
                {
                    if (input[1] == 's') output = "+";
                    if (input[1] == 'a') output = "%";
                }
                else if (prev == '%')
                {
                    if (input[1] == 'c') output = "^";
                    if (input[1] == 's') output = "+";
                }
                else switch (input[1])
                {
                    case 't': //tab
                        {
                            //Console.WriteLine("wtf");
                            output = "{TAB}";
                            i += 2;
                        }
                        break;
                    case 'n': //enter
                        {
                            output = "~";
                            i += 2;
                        }
                        break;
                    case '\\':
                        {
                            output = "\\\\";
                            i += 2;
                        }
                        break;
                    case '{':
                        {
                            output = "{{}";
                            i += 2;
                        }
                        break;
                    case '}':
                        {
                            output = "{}}";
                            i += 2;
                        }
                        break;
                    case '(':
                        {
                            output += "{(}";
                        }
                        break;
                    case ')':
                        {
                            output += "{)}";
                        }
                        break;
                    case '[':
                        {
                            output = "[";
                            i += 2;
                        }
                        break;
                    case ']':
                        {
                            output = "]";
                            i += 2;
                        }
                        break;
                    case 'f':
                        {
                            output = convertF(input.Substring(2));
                        }
                        break;
                    default:
                        {
                            output = "\\";
                        }
                        break;
                }
            }
            else
            {
                i++;
                output = "" + input[0];
            }
            Console.WriteLine("spec out:"+output);
            return output;
        }

        private string convertF(string input)
        {
            if (input.Length>1&&input.Substring(0, 2) == "10")
            {
                i += 4;
                return "{F10}";
            }
            else if (input.Length > 1 && input.Substring(0, 2) == "11")
            {
                i += 4;
                return "{F11}";
            }
            else if (input.Length > 1 && input.Substring(0, 2) == "12")
            {
                i += 4;
                return "{F12}";
            }
            else if (input[0]=='1')
            {
                i += 3;
                return "{F1}";
            }
            else if (input[0] == '2')
            {
                i += 3;
                return "{F2}";
            }
            else if (input[0] == '3')
            {
                i += 3;
                return "{F3}";
            }
            else if (input[0] == '4')
            {
                i += 3;
                return "{F4}";
            }
            else if (input[0] == '5')
            {
                i += 3;
                return "{F5}";
            }
            else if (input[0] == '6')
            {
                i += 3;
                return "{F6}";
            }
            else if (input[0] == '7')
            {
                i += 3;
                return "{F7}";
            }
            else if (input[0] == '8')
            {
                i += 3;
                return "{F8}";
            }
            else if (input[0] == '9')
            {
                i += 3;
                return "{F9}";
            }
            //i++;
            return "\\f";
        }
    }
}
