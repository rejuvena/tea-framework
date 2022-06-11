using System;
using System.IO;
using Terraria.ModLoader;

namespace TeaFramework.API.Features.Packets
{
    /// <summary>
    ///     Handles reading and writing packets.
    /// </summary>
    /// <remarks>
    ///     This interface implements <see cref="ILoadable" />.
    /// </remarks>
    public interface IPacketHandlerWithData<in TPacketData> : IPacketHandler
        where TPacketData : IPacketData
    {
        void IPacketHandler.Write(BinaryWriter writer, IPacketData? packetData) {
            if (packetData is null)
                throw new ArgumentNullException(nameof(packetData), $"\"{nameof(packetData)}\" cannot be null in IPacketHandlerWithData.Write");

            Write(writer, (TPacketData) packetData);
        }

        /// <summary>
        ///     Writes a packet.
        /// </summary>
        /// <param name="writer">The writer to write with.</param>
        /// <param name="packetData">The packet data to serialize.</param>
        void Write(BinaryWriter writer, TPacketData packetData);
    }

    /// <summary>
    ///     Handles reading and writing packets.
    /// </summary>
    /// <remarks>
    ///     This interface implements <see cref="ILoadable" />.
    /// </remarks>
    public interface IPacketHandler : ILoadable
    {
        byte Id { get; set; }

        /// <summary>
        ///     Writes to a <see cref="BinaryWriter"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="packetData">The packet data to serialize.</param>
        void Write(BinaryWriter writer, IPacketData? packetData = null);

        /// <summary>
        ///     Reads a packet.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <param name="whoAmI">The ID of the player sending the packet.</param>
        void ReadPacket(BinaryReader reader, int whoAmI);

        // Default impls for 100% less boilerplate.
        void ILoadable.Load(Mod mod) {
        }

        void ILoadable.Unload() {
        }
    }
}