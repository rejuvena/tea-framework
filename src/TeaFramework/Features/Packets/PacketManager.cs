using System;
using System.Collections.Generic;
using System.IO;
using TeaFramework.API;
using TeaFramework.API.Features.Packets;

namespace TeaFramework.Features.Packets
{
    public class PacketManager : IPacketManager
    {
        public string Name => "TeaFramework:PacketManager";
        
        public ITeaMod TeaMod { get; set; }

        protected byte PacketCount;

        public PacketManager(ITeaMod teaMod) {
            TeaMod = teaMod;
        }

        public Dictionary<byte, IPacketHandler> PacketHandlers { get; } = new();

        public Dictionary<Type, byte> PacketHandlerTypeToId { get; } = new();

        public void RegisterPacketHandler(IPacketHandler handler) {            
            PacketHandlers[PacketCount++] = handler;
            PacketHandlerTypeToId[handler.GetType()] = PacketCount;
        }

        public void WritePacket(BinaryWriter writer, byte packetHandlerId, IPacketData? packetData = null) {
            writer.Write(packetHandlerId);
            PacketHandlers[packetHandlerId].Write(writer, packetData);
        }

        public void ReadPacket(BinaryReader reader, int whoAmI) {
            byte id = reader.ReadByte();
            PacketHandlers[id].ReadPacket(reader, whoAmI);
        }
    }
}