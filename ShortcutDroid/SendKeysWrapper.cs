using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ShortcutDroid
{
    class SendKeysWrapper
    {
       Random rand = new Random();
       public void Send(string input)
        {
            //syntax: "sometext(seq)\n(/seq)textinnewline", also good if no text before or after, or more sequences
            //Keystroke sequences in (seq)(/seq) block,
            //\a -alt, \s -shift, \c -ctrl, \t -tab, \n -enter, \l \r \u \d -arrow keys, \f1 -F1 (works with 1-12)
            //e.g. (seq)\sa(/seq) results in an capitalised "A" character, while (seq)\t(/seq)a results in a tabulator before a delayed "a" character
            //you can also use native SendKeys syntax in seq blocks

            string[] inputArray = input.Split(new[] { "(/seq)" }, StringSplitOptions.None);
            foreach (string s in inputArray)
            {
                string[] innerArray = s.Split(new[] { "(seq)" }, StringSplitOptions.None);
                foreach (char c in innerArray[0])
                {
                    switch (c)
                    {
                        //cases for single characters that need special wrapping in SendKeys
                        case '{':
                            {
                                //these must be sent together
                                SendFast("{{}");
                            }
                            break;
                        case '}':
                            {
                                SendFast("{}}");
                            }
                            break;
                        case '(':
                            {
                                SendFast("{(}");
                            }
                            break;
                        case ')':
                            {
                                SendFast("{)}");
                            }
                            break;
                        default:
                            //simple characters can be simulated as typing
                            SendSlow(c.ToString());
                            break;
                    }
                }
                //if syntax is good, and there was a (seq) tag, the following sequence should be sent together
                if (innerArray.Length > 1) SendFast(innerArray[1]);
            }
        }

        //wait between each character, simulating a real person typing
        private void SendSlow(string s)
        {
            SendKeys.SendWait(s);
            Thread.Sleep(rand.Next(25, 100));
        }

        //key sequences must be sent together
        private void SendFast(string toSend)
        {
            StringBuilder output = new StringBuilder();

            //split before each escaped character
            string[] array = toSend.Split(new[] { "\\" }, StringSplitOptions.None);
            foreach (string s in array)
            {
                //if it's an empty string, there's no need to check
                //E.g. "\\n".Split(..."\\"...); will have an empty string as 0th member because
                //the first occurrence is at the beginning of the string
                if (s.Length != 0)
                    switch (s[0])
                    {
                        //convert to SendKeys semantics and append if it has more chars
                        case 's': //shift
                            output.Append("+"); output.Append(s.Substring(1)); break;
                        case 'c': //ctrl
                            output.Append("^"); output.Append(s.Substring(1)); break;
                        case 'a': //alt
                            output.Append("%"); output.Append(s.Substring(1)); break;
                        case 't': //tab
                            output.Append("{TAB}"); output.Append(s.Substring(1)); break;
                        case 'n': //enter
                            output.Append("~"); output.Append(s.Substring(1)); break;
                        case 'l': //left arrow
                            output.Append("{LEFT}"); output.Append(s.Substring(1)); break;
                        case 'r': //right arrow
                            output.Append("{RIGHT}"); output.Append(s.Substring(1)); break;
                        case 'u': //up arrow
                            output.Append("{UP}"); output.Append(s.Substring(1)); break;
                        case 'd': //down arrow
                            output.Append("{DOWN}"); output.Append(s.Substring(1)); break;
                        case 'f': //f chars, no need to append the rest here, no seq should end with something after Fxx key
                            output.Append(convertF(s)); break;
                        default:
                            output.Append(s); break;

                    }
            }
            SendKeys.SendWait(output.ToString());
        }

        //creates e.g. "{F5}" from getting "f5"
        private string convertF(string s)
        {
            return "{F" + s.Substring(1) + "}";
        }
    }
}
