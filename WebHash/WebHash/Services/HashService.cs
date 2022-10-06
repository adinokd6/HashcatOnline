using WebHash.IServices;
using WebHash.Models.ViewModels;
using WebHash.Interfaces;
using System;
using static WebHash.Models.Enums.Enums;
using Microsoft.Extensions.DependencyInjection;
using WebHash.DataModels;
using System.Linq;
using System.Diagnostics;

namespace WebHash.Services
{
    public class HashService : IHashService
    {
        private readonly IStartProgramService _startProgram;
        private readonly IServiceScopeFactory _scopeFactory;

        public HashService(IStartProgramService startProgramService, IServiceScopeFactory scopeFactory)
        {
            _startProgram = startProgramService;
            _scopeFactory = scopeFactory;
        }

        public void Decode(CrackHashViewModel hash)
        {
            var hashCode = string.Empty;
            if (!hash.HashFromInput && hash.HashFromFileId != Guid.Empty)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<Context>();
                    hashCode = db.Hashes.Where(x => x.Id == hash.HashFromFileId).Select(x => x.OriginalString).FirstOrDefault();
                }
            }
            else
            {
                hashCode = !string.IsNullOrEmpty(hash.InputValue) ? hash.InputValue : "";
            }



            if (hashCode != string.Empty)
            {
                Stopwatch stopwatch = new Stopwatch();
                var command = GetCommandForHashCat(hash.AttackMethod, hash.HashType, hashCode, hash.Dictionary1, hash.Dictionary2);

                stopwatch.Start();
                hash.OutputValue = _startProgram.StartDecryptionProcess(command, hashCode);
                stopwatch.Stop();

                var outputValue = hash.OutputValue.Item1.Remove(0, 1); //remo /r becouse it is ouptu with new line from commandline

                if (!hash.HashFromInput && outputValue.Equals(hashCode))
                {
                    SaveResultsForHash(hash.HashFromFileId, hash.OutputValue.Item2, stopwatch.Elapsed.TotalSeconds);
                }
            }

        }

        private string GetCommandForHashCat(AttackMethod attackMethod, HashType hashType, string inputValue, string dictionary1, string dictionary2)
        {
            string commandForHashCat = string.Empty;
            switch (attackMethod)
            {
                case AttackMethod.Combination:
                    return commandForHashCat = GetAttackMode(attackMethod) + " " + GetHashType(hashType) + " " + inputValue + " " + dictionary1 + " " +dictionary2;
                case AttackMethod.Straight:
                    return commandForHashCat = GetAttackMode(attackMethod) + " " + GetHashType(hashType) + " " + inputValue + " " + dictionary1;
                default:
                    return commandForHashCat = GetAttackMode(attackMethod) + " " + GetHashType(hashType) + " " + inputValue;

            }
        }

        private void SaveResultsForHash(Guid hashId, string result, double time)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<Context>();
                var hashToUpdate = db.Hashes.FirstOrDefault(x => x.Id == hashId);
                if(hashToUpdate != null)
                {
                    hashToUpdate.Result = result; //TODO: wywalic karotki z resulta od stringa c cmd
                    hashToUpdate.CrackingTime = Convert.ToInt32(time);
                }
                db.SaveChanges();
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
