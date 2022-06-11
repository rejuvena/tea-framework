using System;
using System.Collections.Generic;
using System.IO;
using TeaFramework.API;
using TeaFramework.API.Features.Packets;
using Terraria.ModLoader;

namespace TeaFramework.Features.Packets
{
    public class PacketManager : IPacketManager
    {
        protected byte PacketCount;

        public PacketManager(ITeaMod teaMod) {
            TeaMod = teaMod;
        }

        public ITeaMod TeaMod { get; }

        public Dictionary<Type, IPacketHandler> PacketHandlers { get; } = new();

        public Dictionary<byte, IPacketHandler> PacketHandlersFromId { get; } = new();

        public void RegisterPacketHandler(IPacketHandler handler) {
            PacketHandlers[handler.HandledType] = handler;
            PacketHandlersFromId[PacketCount++] = handler;
        }

        public void WritePacketFromData(IPacketData packetData) {
            ModPacket packet = TeaMod.ModInstance.GetPacket();
            WritePacket(packet, packetData);
            packet.Send();
        }

        public void WritePacket(BinaryWriter writer, IPacketData packetData) {
            PacketHandlers[packetData.GetType()].WritePacket(writer, packetData);
        }

        public void ReadPacket(BinaryReader reader, int whoAmI) {
            PacketHandlersFromId[reader.ReadByte()].ReadPacket(reader, whoAmI);
        }
    }
}