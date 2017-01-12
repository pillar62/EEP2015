using System;
using System.Collections.Generic;
using System.Text;

namespace Srvtools
{
    public static class Encrypt
    {
        public static bool EncryptPassword(string UserID, string code, int PasswordLen, ref Char[] RetPassword, bool bCheckValidate)
        {
            int KeyOffset = -1;
            int StartKey = -1;
            int MultKey = -1;
            int AddKey = -1;
            char EncryptChar = '~';
            int RealPassLen = -1;
            byte b = new byte();
            bool bHasEncryptChar = false;
            int offset = -1;
            int c = -1;

            if (PasswordLen < 5)
            {
                //raise Exception.Create('[EEP Security System Internal Error] The field length for the user password too short! At least length 5'); 
            }
            if (code.Length > PasswordLen)
            {
                if (bCheckValidate)
                {

                }
                //  raise Exception.CreateFmt(GetEEPGVar('CommonUtils_errPasswordToLong'),[PasswordLen])
                else
                {
                    return false;
                }
            }

            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == ' ')
                {
                    if (bCheckValidate)
                    {
                        //raise Exception.Create(GetEEPGVar('CommonUtils_errSpacePasswordChar'))
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (code[i] == EncryptChar)
                {
                    if (bCheckValidate)
                    {
                        // raise Exception.CreateFmt(GetEEPGVar('CommonUtils_errLeadingPasswordChar'),[EncryptChar])
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (((byte)code[i]) < 33 || ((byte)code[i]) > 126)
                {
                    if (bCheckValidate)
                    {
                        //raise Exception.Create(GetEEPGVar('CommonUtils_errInvalidatePasswordChar'))
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            KeyOffset = 32 + PasswordLen;
            StartKey = 1234;
            MultKey = 12674;
            AddKey = 35891;

            UserID = UserID.Trim().ToUpper();
            for (int i = 0; i < UserID.Length; i++)
            {
                KeyOffset += (((byte)UserID[i]) + 2) * (i + 1);
            }

            for (int i = 0; i < code.Length; i++)
            {
                KeyOffset += (((byte)code[i]) + 2) * (i + 1);
            }

            RealPassLen = code.Length;
            while (code.Length < PasswordLen)
            {
                code = code + '\0';
            }

            RetPassword = new Char[10]; 

            if (PasswordLen % 2 == 0)
            {
                offset = 2;
            }
            else
            {
                offset = 1;
            }

            bHasEncryptChar = false;
            for (int i = 0; i < PasswordLen; i++)
            {
                b = (byte)code[i];
                b = EncryptByte(KeyOffset, ref StartKey, MultKey, AddKey, b);

                if (b == (byte)EncryptChar)
                {
                    bHasEncryptChar = true;
                }

                if ((i % 2 != 0))
                {
                    RetPassword[PasswordLen + offset - i - 2] = (char)b;
                }
                else
                {
                    RetPassword[i] = (char)b;
                }
            }

            if (!bHasEncryptChar)
            {
                if (RealPassLen == 0)
                {
                    RealPassLen = 1;
                }

                offset = (byte)EncryptChar - (byte)RetPassword[RealPassLen - 1];
                for (int i = 0; i < PasswordLen; i++)
                {
                    c = (byte)RetPassword[i] + offset;
                    while (c > 126 || c < 33)
                    {
                        if (c > 126)
                        {
                            c = c - 127 + 33;
                        }
                        else
                        {
                            c = 126 - (32 - c);
                        }
                    }
                    RetPassword[i] = (char)c;
                }
            }
            return false;
        }

        private static byte EncryptByte(int KeyOffset, ref int StartKey, int MultKey, int AddKey, byte b)
        {
            Byte Result;
            int SaveStartKey = StartKey;
            int Counter = 1;

            do
            {
                if (Counter > 100)
                {
                    StartKey = SaveStartKey;
                    b = 20;
                }
                Result = (Byte)((b ^ ((StartKey >> 8) + KeyOffset)) + 33);
                StartKey = (Result + StartKey) * MultKey + AddKey;
                Result = (Byte)(Result & 127);
                Counter++;
            }
            while (Result < 33 || Result > 126);
            return (byte)Result;
        }
    }
}
