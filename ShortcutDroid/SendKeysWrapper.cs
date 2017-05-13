using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortcutDroid
{
    class SendKeysWrapper
    {
        KeysStringWrapper ksw = new KeysStringWrapper();
        public void Send(string input)
        {
            string[] inputArray = input.Split(new[] { "(/seq)" }, StringSplitOptions.None);
            foreach (string s in inputArray)
            {
                string[] innerArray = s.Split(new[] { "(seq)" }, StringSplitOptions.None);
                foreach (char c in innerArray[0])
                {
                    switch (c)
                    {
                        case '{':
                            {
                                SendSlow("{{}");
                            }
                            break;
                        case '}':
                            {
                                SendSlow("{}}");
                            }
                            break;
                        case '(':
                            {
                                SendSlow("{(}");
                            }
                            break;
                        case ')':
                            {
                                SendSlow("{)}");
                            }
                            break;
                        default:
                            SendSlow(c.ToString());
                            break;
                    }
                }
                if (innerArray.Length > 1) SendFast(innerArray[1]);
            }
        }

        private void SendSlow(string s)
        {
            SendKeys.SendWait(s);
            Thread.Sleep(100);
        }

        private void SendFast(string toSend)
        {
            StringBuilder output = new StringBuilder();
            string[] array = toSend.Split(new[] { "\\" }, StringSplitOptions.None);
            foreach (string s in array)
            {
                if(s.Length!=0)
                    switch (s[0])
                {
                    case 's': //shift
                        {
                            output.Append("+");
                        }
                        break;
                    case 'c': //ctrl
                        {
                            output.Append("^");
                        }
                        break;
                    case 'a': //alt
                        {
                            output.Append("%");
                        }
                        break;
                    case 't': //tab
                        {
                            output.Append("{TAB}");
                        }
                        break;
                    case 'n': //enter
                        {
                            output.Append("~");
                        }
                        break;
                    case 'l': //left
                        {
                            output.Append("{LEFT}");
                        }
                        break;
                    case 'r': //right
                        {
                            output.Append("{RIGHT}");
                        }
                        break;
                    case 'u': //up
                        {
                            output.Append("{UP}");
                        }
                        break;
                    case 'd': //down
                        {
                            output.Append("{DOWN}");
                        }
                        break;
                }
                if(s.Length>1)
                {
                    string sub = s.Substring(1);
                    if (sub[0] == 'f' && sub.Length > 1) output.Append(convertF(sub));
                    else output.Append(sub);
                }
            }
            SendKeys.SendWait(output.ToString());
        }

        private string convertF(string s)
        {
            switch (s)
            {
                case "f1": return "{F1}";
                case "f2": return "{F2}";
                case "f3": return "{F3}";
                case "f4": return "{F4}";
                case "f5": return "{F5}";
                case "f6": return "{F6}";
                case "f7": return "{F7}";
                case "f8": return "{F8}";
                case "f9": return "{F9}";
                case "f10": return "{F10}";
                case "f11": return "{F11}";
                case "f12": return "{F12}";
                default:return null;
            }
        }
    }
}
