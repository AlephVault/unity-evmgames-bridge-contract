using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace AlephVault.Unity.EVMGames.Contracts.Bridge
{
    namespace Types
    {
        namespace BridgeContractComponents
        {
            namespace Functions
            {
                [Function("bridgedResourceTypes", typeof(BridgedResourceTypesOutput))]
                public class BridgedResourceTypesFunction : FunctionMessage
                {
                    [Parameter("uint256", "arg0", 1)]
                    public BigInteger Id { get; set; }
                }
            }
        }
    }
}
