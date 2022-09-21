using WebHash.IServices;
using WebHash.Models;
using WebHash.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Text;
using static WebHash.Models.Enums.Enums;

namespace WebHash.Services
{
    public class HashService : IHashService
    {
        private readonly IStartProgramService _startProgram;

        public HashService(IStartProgramService startProgramService)
        {
            _startProgram = startProgramService;
        }

        public void Decode(Hash hashOptions)
        {
            if (!string.IsNullOrEmpty(hashOptions.InputValue))
            {
                string cmdInput = string.Empty;

                if(hashOptions.AttackMethod.Equals(AttackMethod.Straight) || hashOptions.AttackMethod.Equals(AttackMethod.Combination))
                {
                    GetFile(hashOptions, cmdInput);
                }


                if (cmdInput == string.Empty)
                {
                    cmdInput = hashOptions.InputValue;
                }

                string commandForHashCat = GetAttackMethod(hashOptions);


                if (cmdInput != string.Empty && IsOutputEmpty(hashOptions.OutputValue))
                {
                    hashOptions.OutputValue = _startProgram.StartDecryptionProcess(commandForHashCat, cmdInput);
                }


            }

        }

        private void GetFile(Hash hashOptions, string cmdInput)
        {
            try
            {
                cmdInput = File.ReadAllText(hashOptions.InputValue, Encoding.UTF8);
            }
            catch (FileNotFoundException ex)
            {
                hashOptions.OutputValue = Tuple.Create(ex.ToString(), "File not found!");
            }
            catch (IOException ex)
            {
                hashOptions.OutputValue = Tuple.Create(ex.ToString(), "Input output exception occured!");
            }
        }

        private string GetAttackMethod(Hash hash)
        {
            switch (hash.AttackMethod)
            {
                case AttackMethod.Combination:
                    return GetAttackMode(hash.AttackMethod) + " " + GetHashType(hash.HashType) + " " + hash.InputValue + " " + hash.Dictionary1 + " " + hash.Dictionary2;
                case AttackMethod.Straight:
                    return GetAttackMode(hash.AttackMethod) + " " + GetHashType(hash.HashType) + " " + hash.InputValue + " " + hash.Dictionary1;
                default:
                    return GetAttackMode(hash.AttackMethod) + " " + GetHashType(hash.HashType) + " " + hash.InputValue;
            }
        }

        private string GetAttackMode(AttackMethod attackType)
        {

            return "-a " + ((int)attackType).ToString();
        }

        private string GetHashType(HashType hashType)
        {
            return "-m " + ((int)hashType).ToString();
        }

        private bool IsOutputEmpty(Tuple<string,string> output)
        {
            if (output == null)
                return true;
            return false;
        }
    }


}
