using System;
using System.Collections.Generic;
using System.IO;

namespace TeaFramework.API.Features.Packets
{
    /// <summary>
    ///     Manages reading and writing packets.
    /// </summary>
    public interface IPacketManager
    {
        /// <summary>
        ///     The associated <see cref="ITeaMod" /> instance.
        /// </summary>
        ITeaMod TeaMod { get; }

        /// <summary>
        ///     Type to packet handler map, where the type represents the type of a packet data object.
        /// </summary>
        Dictionary<Type, IPacketHandler> PacketHandlers { get; }

        /// <summary>
        ///     Packet handler ID to packet handler map.
        /// </summary>
        Dictionary<byte, IPacketHandler> PacketHandlersFromId { get; }

        /// <summary>
        ///     Registers a packet handler.
        /// </summary>
        /// <param name="handler">The packet handler to register.</param>
        void RegisterPacketHandler(IPacketHandler handler);

        /// <summary>
        ///     Initiates the writing of a packet from the given packet data.
        /// </summary>
        /// <param name="packetData">The packet data to write.</param>
        void WritePacketFromData(IPacketData packetData);

        /// <summary>
        ///     Writes a packet to the binary writer with the given packet data.
        /// </summary>
        /// <param name="writer">The writer to write with.</param>
        /// <param name="packetData"></param>
        void WritePacket(BinaryWriter writer, IPacketData packetData);

        /// <summary>
        ///     Reads a packet using the given reader.
        /// </summary>
        /// <param name="reader">The reader to read with.</param>
        /// <param name="whoAmI">The ID of the player sending the packet.</param>
        void ReadPacket(BinaryReader reader, int whoAmI);
    }
}