using System;
using System.IO;
using Terraria.ModLoader;

namespace TeaFramework.API.Features.Packets
{
    /// <summary>
    ///     Handles reading and writing packets.
    /// </summary>
    /// <typeparam name="TPacketData"></typeparam>
    public interface IPacketHandler<in TPacketData> : IPacketHandler
        where TPacketData : IPacketData
    {
        Type IPacketHandler.HandledType => typeof(TPacketData);

        void IPacketHandler.WritePacket(BinaryWriter writer, IPacketData packetData) {
            WritePacket(writer, (TPacketData) packetData);
        }

        /// <summary>
        ///     Writes a packet.
        /// </summary>
        /// <param name="writer">The writer to write with.</param>
        /// <param name="packetData">The packet data to serialize.</param>
        void WritePacket(BinaryWriter writer, TPacketData packetData);
    }

    /// <summary>
    ///     Handles reading and writing packets.
    /// </summary>
    /// <remarks>
    ///     This interface extends <see cref="ILoadable" />.
    /// </remarks>
    public interface IPacketHandler : ILoadable
    {
        /// <summary>
        ///     The packet data type to handle.
        /// </summary>
        Type HandledType { get; }

        /// <summary>
        ///     Writes a packet.
        /// </summary>
        /// <param name="writer">The writer to write with.</param>
        /// <param name="packetData">The packet data to serialize.</param>
        void WritePacket(BinaryWriter writer, IPacketData packetData);

        /// <summary>
        ///     Reads a packet.
        /// </summary>
        /// <param name="reader">The reader to read with.</param>
        /// <param name="whoAmI">The ID of the player sending the packet.</param>
        void ReadPacket(BinaryReader reader, int whoAmI);
    }
}