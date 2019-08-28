using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
   public class BasicHeader : BlockHeader
   {
      //static private int MAX_SWIFT_MSG_LEN = 4096;
      static private int MIN_BLOCK_1_LEN = 29;
      static private int MAX_BLOCK_1_LEN = 31;

      private string m_ApplicationID_m = null;
      private string m_ServiceID_m = null;
      private string m_LTAddress_m = null;
      private string BICCode = null;
      private string LogicalTerminalCode = null;
      private string BICBranchCode = null;
      private string m_SessionNumber_m = null;
      //private string m_SequenceNumber_m = null;

      public bool isValidHeader = true;

      public BasicHeader()
      {
         
      }

      public void parseBasicHeader(string block1)
      {
         string temp = Block_1;
         string sub = null;
         string LTAddress = null;
         isValidHeader = true;

         // Check the length first.
         // If this is wrong don't bother checking anything else.
         if ((Block_1.Length < MIN_BLOCK_1_LEN) || (Block_1.Length > MAX_BLOCK_1_LEN))
         {
            errors.Add("Invalid length for block 1 : " + Block_1.Length + " Expecting length between : " + MIN_BLOCK_1_LEN + " and " + MAX_BLOCK_1_LEN);
            isValidHeader = false;
            return;
         }

         // Check START OF BLOCK INDICATOR = { 
         // This is a mandatory field.
         // The character { indicates the beginning of a block.
         if (temp[0] != '{')
         {
            errors.Add("BLOCK 1: Message missing START OF BLOCK INDICATOR ({)");
            isValidHeader = false;
         }

         temp = temp.Substring(1);
         // Check BLOCK IDENTIFIER = 3c
         // This is a mandatory field.
         // 1 to 3 alphanumeric characters used to define block contents. 
         // This block identiifer must be 1.
         sub = temp.Substring(0, temp.IndexOf(':'));
         if (string.Equals(sub, "1") == false)
         {
            errors.Add("BLOCK 1: Invalid BLOCK IDENTIFIER : " + sub + " Expecting : 1");
            isValidHeader = false;
         }

         // We found the colon in the last step now get rid of it
         // This is a mandatory field.
         temp = temp.Substring(2);

         // Check the APPLICATION IDENTIFIER = 1a
         // This is a mandatory field.
         // The Application Identifier identifies the application within which the message is being sent or received. 
         // The available options are: 
         // F = FIN All user-to-user, FIN system and FIN service messages 
         // A = GPA (General Purpose Application) Most GPA system and service messages 
         // L = GPA Certain GPA service messages, for example, LOGIN, LAKs, ABORT These values are automatically assigned by the SWIFT system and the user's CBT
         m_ApplicationID_m = temp.Substring(0, 1);
         if ((m_ApplicationID_m.Equals("F") == false) && (m_ApplicationID_m.Equals("A") == false) && (m_ApplicationID_m.Equals("L") == false))
         {
            errors.Add("BLOCK 1: Invalid APPLICATION IDENTIFIER : " + m_ApplicationID_m + " Expecting : F, A or L");
            isValidHeader = false;
         }

         // Check SERVICE IDENTIFIER = 2n
         // This is a mandatory field.
         // The Service Identifier consists of two numeric characters. 
         // It identifies the type of data that is being sent or received and, in doing so, whether the message which follows is one of the following: 
         // a user-to-user message, 
         // a system message, 
         // a service message, 
         // for example, a session control command, such as SELECT, or a logical acknowledgment, such as ACK/SAK/UAK. 
         // Possible values are 01 = FIN/GPA or 21 = ACK/NAK.
         temp = temp.Substring(1);
         m_ServiceID_m = temp.Substring(0, 2);
         if ((m_ServiceID_m.Equals("01") == false) && (m_ServiceID_m.Equals("21") == false))
         {
            errors.Add("BLOCK 1: Invalid SERVICE IDENTIFIER : " + m_ServiceID_m + " Expecting : 01 or 21");
            isValidHeader = false;
         }

         // Check the LOGICAL TERMINAL ADDRESS = 12x
         // This is a mandatory field.
         // The Logical Termial (LT) Address is a 12-character FIN address. 
         // It is the address of the sending LT for input messages or of the receiving LT for output messages, and includes the Branch Code. 
         // It consists of: - the BIC 8 CODE (8 characters) - the Logical Terminal Code (1 upper case alphabetic character) - the BIC Branch Code (3 characters)
         temp = temp.Substring(2);
         m_LTAddress_m = temp.Substring(0, 12);
         BICCode = m_LTAddress_m.Substring(0, 8);
         LogicalTerminalCode = LTAddress.Substring(8, 1);
         BICBranchCode = LTAddress.Substring(9, 3);
         if (isValidBICCode(BICCode) == false)
         {
            errors.Add("BLOCK 1: Invalid BIC Code : " + BICCode);
            isValidHeader = false;
         }
         if (isValidLogicalTerminal(LogicalTerminalCode) == false)
         {
            errors.Add("BLOCK 1: Invalid Logical Terminal : " + LogicalTerminalCode);
            isValidHeader = false;
         }
         if (isValidBICBranchCode(BICBranchCode) == false)
         {
            errors.Add("BLOCK 1: Invalid BIC Branch Code : " + BICBranchCode);
            isValidHeader = false;
         }

         // Check the SESSION NUMBER = 4n
         // This is a mandatory field.
         // The Session Number identifies the session in which the message was transmitted. 
         // It is a four digits number that is automatically generated by the user's computer and padded with zeros.
         m_SessionNumber_m = temp.Substring(12);
         if (isValidSessionNumber(m_SessionNumber_m.Substring(0, 4)) == false)
         {
            errors.Add("BLOCK 1: Invalid SESSION NUMBER : " + m_SessionNumber_m);
            isValidHeader = false;
         }

         // Check the INPUT SEQUENCE NUMBER = 6n 
         // This is a mandatory field.
         // The sequence number always consists of six digits. 
         // It is the Input Sequence Number (ISN) of the sender's current input session or the Output Sequence Number (OSN) of the receiver's current output session. 
         // It is automatically generated by the user's computer and padded with zeros.
         temp = temp.Substring(4);
         if (isValidSequenceNumber(temp.Substring(0, 6)) == false)
         {
            errors.Add("BLOCK 1: Invalid INPUT SEQUENCE NUMBER : " + temp.Substring(0, 6));
            isValidHeader = false;
         }

         // Check END OF BLOCK INDICATOR = { 
         // This is a mandatory field.
         // The character } indicates the end of a block.
         temp = temp.Substring(6);
         if (temp[0] != '}')
         {
            errors.Add("BLOCK 1: Message missing START OF BLOCK INDICATOR ({)");
            isValidHeader = false;
         }
      }
   }
}
