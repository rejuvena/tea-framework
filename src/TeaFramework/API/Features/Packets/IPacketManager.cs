using System;
using System.Collections.Generic;
using System.IO;
using TeaFramework.API.DependencyInjection;

namespace TeaFramework.API.Features.Packets
{
    /// <summary>
    ///     Manages reading and writing packets.
    /// </summary>
    public interface IPacketManager : IService
    {
        /// <summary>
        ///     Packet handler ID to packet handler map.
        /// </summary>
        Dictionary<byte, IPacketHandler> PacketHandlers { get; }

        /// <summary>
        ///     Packet handler <see cref="Type"/> to packet handler ID.
        /// </summary>
        Dictionary<Type, byte> PacketHandlerTypeToId { get; }

        /// <summary>
        ///     Registers a packet handler.
        /// </summary>
        /// <param name="handler">The packet handler to register.</param>
        void RegisterPacketHandler(IPacketHandler handler);

        /// <summary>
        ///     Writes a packet to the binary writer with the given packet data.
        /// </summary>
        /// <param name="writer">The writer to write with.</param>
        /// <param name="packetData"></param>
        void WritePacket(BinaryWriter writer, byte packetId, IPacketData? packetData = null);

        /// <summary>
        ///     Reads a packet using the given reader.
        /// </summary>
        /// <param name="reader">The reader to read with.</param>
        /// <param name="whoAmI">The ID of the player sending the packet.</param>
        void ReadPacket(BinaryReader reader, int whoAmI);
    }
}