using System;
using System.Numerics;
using System.Threading.Tasks;
using AlephVault.Unity.EVMGames.Contracts.Types;
using AlephVault.Unity.EVMGames.Contracts.Types.Events;
using AlephVault.Unity.EVMGames.Contracts.Utils;
using Nethereum.ABI;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace AlephVault.Unity.EVMGames.Contracts.Bridge
{
    namespace Types
    {
        using BridgeContractComponents.Events;
        using BridgeContractComponents.Functions;

        /// <summary>
        ///   An interface to the Bridge contract.
        /// </summary>
        public class BridgeContract : BaseContract
        {
            /// <summary>
            ///   Tells to not use a parcel id.
            /// </summary>
            public static byte[] ParcelNone = {
                255, 255, 255, 255, 255, 255, 255, 255,
                255, 255, 255, 255, 255, 255, 255, 255,
                255, 255, 255, 255, 255, 255, 255, 255,
                255, 255, 255, 255, 255, 255, 255, 255
            };
            
            public BridgeContract(Web3 web3, string address, ITransactionGasSetter transactionGasSetter = null) : base(web3, address, transactionGasSetter)
            {
            }
            
            /// <summary>
            ///   Makes a parcel id from given data.
            /// </summary>
            /// <param name="description">The description</param>
            /// <returns>The parcel id</returns>
            public byte[] ParcelId(string description)
            {
                return Shortcuts.Encode(false, false, new ABIValue(
                    "bytes32", description
                ));
            }
            
            /// <summary>
            ///   Gets the data of a bridged resource type.
            /// </summary>
            /// <param name="id">The ID of the type (also, the corresponding ERC1155 token)</param>
            /// <param name="blockParameter">The block parameter</param>
            /// <returns>The resource definition data</returns>
            public Task<BridgedResourceTypesOutput> BridgedResourceTypes(BigInteger id, BlockParameter blockParameter = null)
            {
                return CallMulti<BridgedResourceTypesFunction, BridgedResourceTypesOutput>(new BridgedResourceTypesFunction
                {
                    Id = id,
                }, blockParameter);
            }

            /// <summary>
            ///   Gets a reference to the corresponding ERC1155 contract.
            /// </summary>
            /// <param name="blockParameter">The block parameter</param>
            /// <returns>The corresponding ERC1155 contract</returns>
            public async Task<ERC1155Contract> Economy(BlockParameter blockParameter = null)
            {
                string address = await Call<EconomyFunction, string>(new EconomyFunction
                {
                }, blockParameter);

                return new ERC1155Contract(Web3, address, TransactionGasSetter);
            }

            /// <summary>
            ///   The owner of this bridge contract.
            /// </summary>
            /// <param name="blockParameter">The block parameter</param>
            /// <returns>The owner of the contract</returns>
            public Task<string> Owner(BlockParameter blockParameter = null)
            {
                return Call<OwnerFunction, string>(new OwnerFunction
                {
                }, blockParameter);
            }

            /// <summary>
            ///   Gets a registered parcel, which corresponds to a payment made.
            /// </summary>
            /// <param name="id">The parcel id</param>
            /// <param name="blockParameter">The block parameter</param>
            /// <returns>The parcel data</returns>
            public Task<ParcelsOutput> Parcels(byte[] id, BlockParameter blockParameter = null)
            {
                return CallMulti<ParcelsFunction, ParcelsOutput>(new ParcelsFunction
                {
                    Id = id,
                }, blockParameter);
            }

            // Removing this method. It will make this contract
            // completely unusable.
            //
            // public Task<TransactionReceipt> RenounceOwnership()
            // {
            //     return Send(new RenounceOwnershipFunction
            //     {
            //     });
            // }
            
            /// <summary>
            ///   Gets whether this contract is terminated or not.
            /// </summary>
            /// <param name="blockParameter">The block parameter</param>
            /// <returns>Whether it is terminated or not</returns>
            public Task<bool> Terminated(BlockParameter blockParameter = null)
            {
                return Call<TerminatedFunction, bool>(new TerminatedFunction
                {
                }, blockParameter);
            }

            /// <summary>
            ///   Transfers the ownership to another game address.
            /// </summary>
            /// <param name="newOwner">The address of the new owner</param>
            public Task<TransactionReceipt> TransferOwnership(string newOwner)
            {
                return Send(new TransferOwnershipFunction
                {
                    NewOwner = newOwner,
                });
            }

            /// <summary>
            ///   Sends units of a resource back to a user.
            /// </summary>
            /// <param name="to">The address of the user</param>
            /// <param name="id">The resource id</param>
            /// <param name="units">The amount of units</param>
            public Task<TransactionReceipt> SendUnits(string to, BigInteger id, BigInteger units)
            {
                return Send(new SendUnitsFunction
                {
                    To = to,
                    Id = id,
                    Units = units,
                });
            }

            /// <summary>
            ///   Sends tokens of a resource back to a user.
            /// </summary>
            /// <param name="to">The address of the user</param>
            /// <param name="id">The token id</param>
            /// <param name="value">The amount of tokens</param>
            /// <param name="data">The data, if any</param>
            public Task<TransactionReceipt> SendTokens(string to, BigInteger id, BigInteger value, byte[] data)
            {
                return Send(new SendTokensFunction
                {
                    To = to,
                    Id = id,
                    Value = value,
                    Data = data,
                });
            }

            /// <summary>
            ///   Defines or updates a bridged resource type.
            /// </summary>
            /// <param name="id">The resource / token id</param>
            /// <param name="amountPerUnit">The amounts for each unit</param>
            public Task<TransactionReceipt> DefineBridgedResourceType(BigInteger id, BigInteger amountPerUnit)
            {
                return Send(new DefineBridgedResourceTypeFunction
                {
                    Id = id,
                    AmountPerUnit = amountPerUnit,
                });
            }

            /// <summary>
            ///   Undefines a bridged resource type
            /// </summary>
            /// <param name="id">The resource / token id</param>
            public Task<TransactionReceipt> RemoveBridgedResourceType(BigInteger id)
            {
                return Send(new RemoveBridgedResourceTypeFunction
                {
                    Id = id,
                });
            }

            /// <summary>
            ///   Terminates this contract. No new payments can be received by it.
            /// </summary>
            public Task<TransactionReceipt> Terminate()
            {
                return Send(new TerminateFunction
                {
                });
            }

            /// <summary>
            ///   Gets an event worker for when a resource is defined.
            /// </summary>
            /// <param name="filterMaker">The filter maker</param>
            /// <param name="fromBlock">The starting block</param>
            public EventsWorker<BridgedResourceTypeDefinedEventDTO> MakeBridgedResourceTypeDefinedEventsWorker(Func<Event<BridgedResourceTypeDefinedEventDTO>, NewFilterInput> filterMaker, BlockParameter fromBlock = null)
            {
                return MakeEventsWorker(filterMaker, fromBlock);
            }

            /// <summary>
            ///   Gets an event worker for when a resource is undefined.
            /// </summary>
            /// <param name="filterMaker">The filter maker</param>
            /// <param name="fromBlock">The starting block</param>
            public EventsWorker<BridgedResourceTypeRemovedEventDTO> MakeBridgedResourceTypeRemovedEventsWorker(Func<Event<BridgedResourceTypeRemovedEventDTO>, NewFilterInput> filterMaker, BlockParameter fromBlock = null)
            {
                return MakeEventsWorker(filterMaker, fromBlock);
            }

            /// <summary>
            ///   Gets an event worker for when the ownership is transferred.
            /// </summary>
            /// <param name="filterMaker">The filter maker</param>
            /// <param name="fromBlock">The starting block</param>
            public EventsWorker<OwnershipTransferredEventDTO> MakeOwnershipTransferredEventsWorker(Func<Event<OwnershipTransferredEventDTO>, NewFilterInput> filterMaker, BlockParameter fromBlock = null)
            {
                return MakeEventsWorker(filterMaker, fromBlock);
            }
        }
    }
}
