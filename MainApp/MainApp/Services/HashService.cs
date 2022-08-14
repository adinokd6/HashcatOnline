using Coded.IServices;
using Coded.Models;
using MainApp.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;


namespace Coded.Services
{
    public class HashService : IHashService
    {
        private readonly IStartProgramService _startProgram;

        public HashService(IStartProgramService startProgramService)
        {
            _startProgram = startProgramService;
        }

        public void Decode(Hash hash)
        {
            if (!string.IsNullOrEmpty(hash.InputValue))
            {
                string hashCode = string.Empty;

                try
                {
                    hashCode = File.ReadAllText(hash.InputValue, Encoding.UTF8);
                }
                catch (FileNotFoundException ex)
                {
                    hash.OutputValue = Tuple.Create(ex.ToString(), "File not found!");
                }
                catch (IOException ex)
                {
                    hash.OutputValue = Tuple.Create(ex.ToString(), "Input output exception occured!");
                }

                if (hashCode == string.Empty)
                {
                    hashCode = hash.InputValue;
                }

                string commandForHashCat = string.Empty;


                switch (hash.AttackMethod)
                {
                    case (AttackMethod.BruteForce):
                        commandForHashCat = GetAttackMode(hash.AttackMethod) + " " + GetHashType(hash.HashType) + " " + hash.InputValue;
                        break;
                    case AttackMethod.Combination:
                        commandForHashCat = GetAttackMode(hash.AttackMethod) + " " + GetHashType(hash.HashType) + " " + hash.InputValue + " " + hash.Dictionary1 + " " + hash.Dictionary2;
                        break;
                    case AttackMethod.Straight:
                        commandForHashCat = GetAttackMode(hash.AttackMethod) + " " + GetHashType(hash.HashType) + " " + hash.InputValue + " " + hash.Dictionary1;
                        break;

                }

                if (hashCode != string.Empty)
                {
                    hash.OutputValue = _startProgram.StartDecryptionProcess(commandForHashCat, hashCode);
                }


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
    }


}
