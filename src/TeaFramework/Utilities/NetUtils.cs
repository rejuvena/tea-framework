using System;
using System.IO;
using TeaFramework.API;
using TeaFramework.API.Features.Packets;
using Terraria.ModLoader;

namespace TeaFramework.Utilities
{
    /// <summary>
    ///     Utilities for net syncing.
    /// </summary>
    public static class NetUtils
    {
        /// <summary>
        /// Write the specified packet 
        /// </summary>
        /// <typeparam name="TPacket">The packet to write.</typeparam>
        /// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
        /// <param name="teaMod">The mod the packet belongs to.</param>
        /// <param name="packetData"><see cref="IPacketData"/> to use when writing the packet/.</param>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="teaMod"/> does not have a registered <see cref="IPacketManager"/></exception>
        public static void WritePacket<TPacket>(BinaryWriter writer, ITeaMod teaMod, IPacketData? packetData = null) where TPacket : IPacketHandler {
            IPacketManager? manager = teaMod.ServiceProvider.GetService<IPacketManager>();
            if (manager is null)
                throw new InvalidOperationException($"{teaMod.ModInstance.Name} does not have a {nameof(IPacketManager)} registered.");

            manager.WritePacket(writer, manager.PacketHandlerTypeToId[typeof(TPacket)], packetData);
        }

        /// <summary>
        /// Create a <see cref="ModPacket"/>, write <typeparamref name="TPacket"/> to it, and send it.
        /// </summary>
        /// <typeparam name="TPacket">The packet to write.</typeparam>
        /// <param name="teaMod">The mod the packet belongs to.</param>
        /// <param name="packetData"><see cref="IPacketData"/> to use when writing the packet/.</param>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="teaMod"/> does not have a registered <see cref="IPacketManager"/></exception>
        public static void WriteAndSendPacket<TPacket>(ITeaMod teaMod, IPacketData? packetData = null) where TPacket : IPacketHandler {
            ModPacket packet = teaMod.ModInstance.GetPacket();
            WritePacket<TPacket>(packet, teaMod, packetData);
            packet.Send();
        }
    }
}
